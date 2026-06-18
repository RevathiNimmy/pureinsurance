Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports System.Text.RegularExpressions
Imports System.Net
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF031298 - Menu & Toolbar activity
    '               MK 990929
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'EK 11/11/99 Add access to IPT Extras
    ' Declare an instance of the IPT Find interface.
    Private m_oIPTExtras As Object
    Private m_bIsNRMA As Boolean

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyEX.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oFeeBusiness As Object

    'GW 250504 PN013
    'added so that underwriting fee calls
    'could go through the same business layer
    Private m_oUFeeBusiness As Object
    Private m_frmUInterface As Object

    ' PM Lookup Business Component (Private)
    ' CTAF 300700 Changed to late binding
    Private m_oLookup As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    'Stores the return value for reference id
    'TO CHECK DO I NEED THIS VAR. MK 990929
    Private m_iRefID As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(1, 0) As Control

    ' Stores the details from the business object.
    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lPartyCnt As Integer
    ' AMB 09-Oct-03: 1.8.6 Accident Management development
    Private m_sAgencyNumber As String = ""
    Private m_sShortName As String = ""
    Private m_sName As String = ""
    Private m_lPartyTypeID As Integer
    Private m_sPartyType As String = ""
    Private m_sPaymentMethodCode As String = ""
    Private m_sTermsOfPayment As String = ""
    Private m_lPartyCategoryID As Integer
    Private m_iDisplayOnQuotes As Integer
    Private m_lCurrencyID As Integer
    Private m_sCurrency As String = ""
    Private m_nIsFeeCharge As CheckState

    Private m_vFeeDetails(,) As Object
    Private m_vUFeeDetails(,) As Object
    Private m_iFeeElements As Integer
    Private m_vDeletedFeeIDs() As Object
    Private m_bIsAnyDeletedFee As Boolean

    'Flag to indicate whether we need to check the headoffice id matches
    'the headoffice ref as user may change the reference directly
    'TO CHECK DO WE NEED THIS VAR. FOR THIS CONTROL MK 990929
    Private m_bVerifyHeadOfficeCnt As Boolean

    'Note the index in the lookup array of the main address
    'TO CHECK DO WE NEED THIS VAR. FOR THIS CONTROL MK 990929
    Private m_iMainAddressIndex As Integer

    ' Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    ' Declare an instance of the Fee interface.
    Private m_oFee As Object
    'Addresses and Contacts
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_sAddressLine1 As String = ""
    'Note the index in the lookup array of the main address
    Private m_sMainPostCode As String = ""
    Private m_lAddressCount As Integer
    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    ' Declare an instance of the contact interface.

    Private m_oContact As iPMBContact.Interface_Renamed
    ' Gemini List Manager

    ' AMB 10-Oct-03: 1.8.6 Accident Management development - accident management system options
    Private m_lAccManEnabled As Integer
    Private m_sAccManAgencyNumber As String = ""
    Private m_lAccManAgencyNumberIsDefault As Integer
    Private m_sUnderwritingOrAgency As String = ""

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double
    '**************************************************
    Private m_bHideScheme As Boolean

    Private m_bFSAEnabled As Boolean

    Private m_bRiskTransferAgreement As Boolean
    Private m_bDelegatedAuthority As Boolean
    Private m_lFSAProductID As Integer
    Private m_bIsViewOnlyEXMaintenance As Boolean
    Private m_sUniqueId As String
    Private m_sScreenHeirarchy As String

    Public Function DeleteUnderwritingFee1() As Integer

        Dim oListItem As ListViewItem
        Dim lTag As Integer

        Try

            'Set row to be deleted - if a valid one selected
            If lvwFees.Items.Count < 1 Then
                Exit Function
            End If

            oListItem = lvwFees.FocusedItem

            lTag = Convert.ToString(oListItem.Tag)

            'set property for the object call

            m_oUFeeBusiness.FeeAmountID = CInt(m_vUFeeDetails(0, lTag))

            m_iLine = oListItem.Index + 1

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'call the delete function in the business layer

            m_lReturn = m_oUFeeBusiness.Delete()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If

            'If not cancelled, edit grid

            If m_oUFeeBusiness.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Function
            End If

            'Blank the one we are deleting.
            m_vUFeeDetails(1, lTag) = ""
            m_vUFeeDetails(2, lTag) = ""
            m_vUFeeDetails(3, lTag) = CStr(0)
            m_vUFeeDetails(4, lTag) = ""
            m_vUFeeDetails(5, lTag) = CStr(0)
            m_vUFeeDetails(6, lTag) = CStr(0)
            m_vUFeeDetails(7, lTag) = ""
            m_vUFeeDetails(8, lTag) = CStr(0)
            m_vUFeeDetails(9, lTag) = CStr(0)
            m_vUFeeDetails(10, lTag) = CStr(0)

            lvwFees.Items.RemoveAt(lvwFees.FocusedItem.Index)

            lvwFees.Focus()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteUnderwritingFee1", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUnderwritingFee1", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function


    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property IsNRMA() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsNRMA = Value

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

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Standard Property.
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

    Public Property PartyTypeID() As Integer
        Get
            Return m_lPartyTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyTypeID = Value
        End Set
    End Property
    Public Property PartyType() As String
        Get
            Return m_sPartyType
        End Get
        Set(ByVal Value As String)
            m_sPartyType = Value
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
    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property
    Public Property LongName() As String
        Get
            Return m_sName
        End Get
        Set(ByVal Value As String)
            'For some reason this wont compile if use 'name' as the property
            'name.
            m_sName = Value
        End Set
    End Property
    Public Property AddressLine1() As String
        Get
            Return m_sAddressLine1
        End Get
        Set(ByVal Value As String)
            m_sAddressLine1 = Value
        End Set
    End Property
    Public Property AgencyNumber() As String
        Get
            ' AMB 09-Oct-03: 1.8.6 Accident Management development - created
            Return m_sAgencyNumber
        End Get
        Set(ByVal Value As String)
            ' AMB 09-Oct-03: 1.8.6 Accident Management development - created
            m_sAgencyNumber = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Reference must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Percentage to be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Amount to be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            'effective date hidden field
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            'transaction type hidden field
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTransType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' {* USER DEFINED CODE (End) *}


            ' AMB 10-Oct-03: 1.8.6 Accident Management development - set agency number as mandatory
            If m_sPartyType = PMBConst.PMBPartyTypeExtra Then
                If m_lAccManEnabled = 0 Then
                    m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyNum, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    If m_lAccManAgencyNumberIsDefault = 0 Then
                        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyNum, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyNum, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            Else
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyNum, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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
            'SP090998
            'm_lPartyCnt& = 25
            'TO DELETE this Procedure does not do anything now

            m_lReturn = m_oBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)
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
            'MKW PN13710 - Set Agency Number Default for accidentcare (ACCCA).
            If (m_sPartyType = PMBConst.PMBPartyTypeExtra) And (m_sShortName.Trim().ToUpper() = "ACCCA") Then
                If m_lAccManEnabled <> 0 And m_lAccManAgencyNumberIsDefault = 1 Then
                    m_sAgencyNumber = m_sAccManAgencyNumber
                End If
            End If
            'SP090998
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountCode, vControlValue:=m_sShortName)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountName, vControlValue:=m_sName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'to check variable not found message for the following lines 990929 1114

            cboPaymentMethod.Text = m_sPaymentMethodCode
            uctCurrency.CompanyId = g_iSourceID
            uctCurrency.RefreshList()
            uctCurrency.CurrencyId = m_lCurrencyID


            'GW180504 - check to see if business is agency or underwriting
            'and ammend check box value
            'AR20050121 - PN18243 Cr Fee used for Underwriting and Broking

            If m_nIsFeeCharge = CheckState.Checked Then
                'assign value to fee charge checkbox
                chkCRFee_charge.CheckState = CheckState.Checked
            Else
                chkCRFee_charge.CheckState = CheckState.Unchecked
            End If


            ' AMB 13-Oct-03: 1.8.6 Accident Management development
            'If Len(txtAgencyNum.Text) = 0 Then 'PN13710 Only for ACCA account
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgencyNum, vControlValue:=m_sAgencyNumber)
            If txtAccountCode.Text = "ACCCA" Then
                txtAgencyNum.Enabled = False 'PN13710
                txtAgencyNum.BackColor = SystemColors.Control 'PN13710
            Else
                txtAgencyNum.Enabled = True 'PN13710
                txtAgencyNum.BackColor = SystemColors.Window 'PN13710
            End If

            PopulateUWFees()


            'Fill the contact grid
            PopulateContacts()

            'Fill the address grid
            PopulateAddresses()

            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage

            If m_sShortName.Trim().ToUpper() <> "ACCCA" Then
                m_bHideScheme = True
            End If

            If m_bRiskTransferAgreement Then
                chkRiskTransferAgreement.CheckState = CheckState.Checked
            Else
                chkRiskTransferAgreement.CheckState = CheckState.Unchecked
            End If

            If m_bDelegatedAuthority Then
                chkDelegatedAuthority.CheckState = CheckState.Checked
            Else
                chkDelegatedAuthority.CheckState = CheckState.Unchecked
            End If

            cboFSAProduct.ItemId = m_lFSAProductID

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
        Const kMethodName As String = "InterfaceToBusiness"

        Dim lBusinessDataID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the mouse pointer to an hour glass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("InterfaceToData", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1
            m_sUniqueId = GetUniqueID()

            If m_sScreenHeirarchy = "" Then
                If m_lPartyTypeID = 9 Then
                    m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 10 Then
                    m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 11 Then
                    m_sScreenHeirarchy = $"Discount Account({txtAccountCode.Text.Trim()})"
                End If
            End If
            'Check the task.
            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd

                    m_lReturn = m_oFeeBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=m_lPartyTypeID, vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sName, vCurrencyId:=m_lCurrencyID, vPaymentMethodCode:=m_sPaymentMethodCode, vUniqueId:=m_sUniqueId, vScreenHeirarchy:=m_sScreenHeirarchy)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oFeeBusiness.DirectAdd", "vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = m_oBusiness.DirectAdd(vPartyCnt:=m_lPartyCnt, vAgencyNumber:=m_sAgencyNumber, vIsFeeCharge:=m_nIsFeeCharge, vRiskTransferAgreement:=m_bRiskTransferAgreement, vDelegatedAuthority:=m_bDelegatedAuthority, vFSAProductID:=m_lFSAProductID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.DirectAdd", "vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit

                    m_lReturn = m_oFeeBusiness.UpdatePartyEX(v_vPartyCnt:=m_lPartyCnt, v_vPartyTypeID:=m_lPartyTypeID, v_vShortName:=m_sShortName, v_vName:=m_sName, v_vCurrencyID:=m_lCurrencyID, v_vPaymentMethodCode:=m_sPaymentMethodCode, v_vResolvedName:=m_sName, v_vUniqueId:=m_sUniqueId, v_vScreenHeirarchy:=m_sScreenHeirarchy)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oFeeBusiness.UpdatePartyEX", "v_vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = m_oBusiness.UpdatePartyExtra(vPartyCnt:=m_lPartyCnt, vFeeCharge:=m_nIsFeeCharge, vRiskTransferAgreement:=m_bRiskTransferAgreement, vDelegatedAuthority:=m_bDelegatedAuthority, vFSAProductID:=m_lFSAProductID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oFeeBusiness.UpdatePartyEX", "v_vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

            End Select


            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            'Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
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
            'SP090998
            'to check putting a remark 990929 MK
            'm_lReturn& = GetLookupDetails( _
            'sLookupTable:=SIRLookupPartyAgentOrigin, _
            'ctlLookup:=cboSource)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateFees
    '
    ' Description: Fills the grid control with Fee details
    '
    ' ***************************************************************** '
    Private Sub PopulateFees()

        Dim oListItem As ListViewItem
        Dim colX As ColumnHeader
        Dim bShowScheme As Boolean

        'MKR 09/11/2004 PN 16101/16487
        Static bColumWidthChanged As Boolean

        'Const FeeImage As String = "FeeImage"          ''Unused Local Variable

        Try

            If Not Information.IsArray(m_vFeeDetails) Then
                Exit Sub
            End If

            lvwFees.Items.Clear()

            If m_sPartyType <> PMBConst.PMBPartyTypeExtra Then ' To Not to Display Commission Columns for non Extras Account
                colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_CommPerc)
                colX.Width = CInt(0)
                colX.Text = ""
                colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_CommAmount)
                colX.Width = CInt(0)
                colX.Text = ""
                colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_Scheme)
                colX.Width = CInt(0)
                colX.Text = ""

                'MKR 09/11/2004 PN 16101/16487 -- A flag is applied so that the width of the
                'column must be doubled only once...
                If Not bColumWidthChanged Then

                    colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_RiskGroup)
                    colX.Width = CInt(colX.Width * 2)
                    colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_Perc)
                    colX.Width = CInt(colX.Width * 2)
                    colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_Amount)
                    colX.Width = CInt(colX.Width * 2)

                    bColumWidthChanged = True
                End If
            End If


            m_iFeeElements = m_vFeeDetails.GetUpperBound(1)
            ' Assign the details to the interface.
            For i As Integer = m_vFeeDetails.GetLowerBound(1) To m_vFeeDetails.GetUpperBound(1)

                'Don't display ones that have been deleted.
                If CStr(m_vFeeDetails(1, i)).Trim() <> "" Then

                    ' Column - Risk Group
                    oListItem = lvwFees.Items.Add(CStr(m_vFeeDetails(1, i)).Trim())


                    ' Column - Scheme
                    If m_sPartyType = PMBConst.PMBPartyTypeExtra Then
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_Scheme).Text = gPMFunctions.NullToString(CStr(m_vFeeDetails(10, i)))
                        If Not bShowScheme Then
                            bShowScheme = ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_Scheme).Text.Length
                        End If
                    End If

                    'oListItem.SubItems(eFeesColumns.Col_Currency) = Trim(m_vFeeDetails(12, i))

                    ' Column - Percentage
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=CStr(m_vFeeDetails(2, i)).Trim())
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_Perc).Text = txtPercentage.Text

                    ' Column - Amount
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=CStr(m_vFeeDetails(3, i)).Trim())
                    ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_Amount).Text = txtAmount.Text

                    'If extras account put value else 0
                    If m_sPartyType = PMBConst.PMBPartyTypeExtra Then
                        ' Column - Commission Percentage
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=CStr(m_vFeeDetails(4, i)).Trim())
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_CommPerc).Text = txtPercentage.Text

                        ' Column - Commission Amount
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=CStr(m_vFeeDetails(5, i)).Trim())
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_CommAmount).Text = txtAmount.Text
                    Else
                        ' Column - Commission Percentage
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:="")
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_CommPerc).Text = txtPercentage.Text

                        ' Column - Commission Amount
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:="")
                        ListViewHelper.GetListViewSubItem(oListItem, MainModule.eFeesColumns.Col_CommAmount).Text = txtAmount.Text
                    End If

                    ' Store the Fee_cnt
                    oListItem.Tag = CStr(i)

                End If

            Next i

            'Only show scheme column if there is something in it
            If (Not bShowScheme) Or m_bHideScheme Then
                colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_Scheme)
                colX.Width = CInt(0)
                colX.Text = ""
            Else
                colX = lvwFees.Columns.Item(MainModule.eFeesColumns.Col_Scheme)
                colX.Width = CInt(VB6.TwipsToPixelsX(1500))
                colX.Text = "Scheme"
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateFees", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: PopulateAddresses
    '
    ' Description: Fills the grid control with address details
    '
    ' ***************************************************************** '
    Private Sub PopulateAddresses()

        Dim k As Integer
        Dim oListItem As ListViewItem


        Try

            'Just go if no addresses
            If Not Information.IsArray(m_vAddresses) Then
                Exit Sub
            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwAddresses.Items.Clear()
            m_lAddressCount = 0

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                For k = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If m_vAddresses(1, i).Equals(m_vAddressTypes(0, k)) Then
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddressTypes(1, k))
                        Exit For
                    End If
                Next k
                'See if this is the main address
                If CStr(m_vAddressTypes(2, k)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                    m_sMainPostCode = CStr(m_vAddresses(0, i))
                    m_iMainAddressIndex = CInt(m_vAddressTypes(0, k))
                End If


                ' Column 3
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(2, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(3, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(4, i)).Trim()

                ' Column 6
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(5, i)).Trim()

                ' Store the Address_cnt
                oListItem.Tag = CStr(m_vAddresses(6, i)).Trim()
                ' {* USER DEFINED CODE (End) *}
                m_lAddressCount += 1
                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateContacts
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Sub PopulateContacts()

        Dim oListItem As ListViewItem


        Try

            'Just go if no contacts
            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If
            lvwContacts.Items.Clear()


            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vContacts(2, i)).Trim()

                ' Column 3
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vContacts(3, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vContacts(4, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vContacts(5, i)).Trim()


                ' Store the Contact_cnt
                oListItem.Tag = CStr(m_vContacts(0, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

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
        Const kMethodName As String = "AMethod"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve the party details

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vAgencyNumber:=m_sAgencyNumber, vCurrencyId:=m_lCurrencyID, vShortName:=m_sShortName, vName:=m_sName, vPaymentMethodCode:=m_sPaymentMethodCode, vIsFeeCharge:=m_nIsFeeCharge, vRiskTransferAgreement:=m_bRiskTransferAgreement, vDelegatedAuthority:=m_bDelegatedAuthority, vFSAProductID:=m_lFSAProductID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetNext", "vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Retrieve additional party details
            m_lReturn = GetPartyDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPartyDetails", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get Fees for the party


            m_lReturn = LoadUnderwritingFees()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError("LoadUnderwritingFees", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetAddressDetails", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get address type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vAddressTypes:=m_vAddressTypes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetAddresstypelookups", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get contacts for the party

            m_lReturn = m_oBusiness.GetContactDetails(vContacts:=m_vContacts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetContactDetails", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here



            ' This is for debugging only



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "InterfaceToData"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtAccountCode))

            m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtAccountName))
            m_lCurrencyID = uctCurrency.CurrencyId
            m_sCurrency = uctCurrency.CurrencyName
            m_sPaymentMethodCode = cboPaymentMethod.Text

            m_sAgencyNumber = CStr(m_oFormFields.UnformatControl(ctlControl:=txtAgencyNum))
            m_nIsFeeCharge = chkCRFee_charge.CheckState
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage
            m_bRiskTransferAgreement = chkRiskTransferAgreement.CheckState = CheckState.Checked
            m_bDelegatedAuthority = chkDelegatedAuthority.CheckState = CheckState.Checked
            m_lFSAProductID = cboFSAProduct.ItemId



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here



            ' This is for debugging only



        End Try
        Return result
    End Function

    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer
        Dim nResult As Integer
        Dim colX As ColumnHeader

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=SIRLookupPartyType, v_sCode:=m_sPartyType, v_dtEffectiveDate:=DateTime.Now, r_lID:=m_lPartyTypeID)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            Select Case m_sPartyType
                Case PMBPartyTypeFee
                    lblAccountType.Text = PMBPartyTypeFeeText
                    Me.Text = PMBPartyTypeFeeText
                    lblAgencyNum.Visible = False
                    txtAgencyNum.Enabled = False
                    txtAgencyNum.Visible = False
                    lvwFees.Top = lblAgencyNum.Top
                    cmdAddFees.Top -= VB6.TwipsToPixelsY(510)
                    cmdEditFees.Top = cmdAddFees.Top
                    cmdDeleteFees.Top = cmdAddFees.Top
                    fmeMain.Height -= VB6.TwipsToPixelsY(510)
                    colX = lvwFees.Columns.Item(eFeesColumns.Col_CommPerc)
                    colX.Width = CInt(0)
                    colX.Text = ""
                    colX = lvwFees.Columns.Item(eFeesColumns.Col_CommAmount)
                    colX.Width = CInt(0)
                    colX.Text = ""
                    ' AMB 09-Oct-03: 1.8.6 Accident Management development
                    colX = lvwFees.Columns.Item(eFeesColumns.Col_Scheme)
                    colX.Width = CInt(0)
                    colX.Text = ""

                    If (m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit) And m_lPartyCnt <> 0 Then
                        'DC280205 : PN19107 : only show buttons for broking
                        'cmdAddFees.Enabled = True
                        cmdDeleteFees.Enabled = False
                        cmdEditFees.Enabled = False

                    Else
                        cmdAddFees.Enabled = False
                        cmdDeleteFees.Enabled = False
                        cmdEditFees.Enabled = False
                    End If

                    If m_iTask = PMEComponentAction.PMAdd Then
                        cmdAddFees.Enabled = False
                    End If

                    cmdDeleteCon.Enabled = False
                    cmdEditCon.Enabled = False
                    cmdDeleteAd.Enabled = False
                    cmdEditAd.Enabled = False
                    chkCRFee_charge.Visible = False

                    chkRiskTransferAgreement.Visible = False
                    chkDelegatedAuthority.Visible = False
                    lblFSAProduct.Visible = False
                    cboFSAProduct.Visible = False

                Case PMBPartyTypeExtra

                    lblAccountType.Text = PMBPartyTypeExtraText
                    Me.Text = PMBPartyTypeExtraText

                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        'DC280205 : PN19107 : only show buttons for broking
                        cmdAddFees.Enabled = False
                        cmdDeleteFees.Enabled = False
                        cmdEditFees.Enabled = False
                    Else
                        cmdAddFees.Enabled = True
                        cmdDeleteFees.Enabled = True
                        cmdEditFees.Enabled = True
                    End If

                    chkCRFee_charge.Visible = False

                    chkRiskTransferAgreement.Visible = False
                    chkDelegatedAuthority.Visible = False
                    lblFSAProduct.Visible = False
                    cboFSAProduct.Visible = False

                Case PMBPartyTypeDiscount

                    lblAccountType.Text = PMBPartyTypeDiscountText
                    Me.Text = PMBPartyTypeDiscountText
                    lvwFees.Visible = False
                    lvwFees.Enabled = False
                    cmdAddFees.Visible = False
                    cmdAddFees.Enabled = False
                    cmdDeleteFees.Visible = False
                    cmdDeleteFees.Enabled = False
                    cmdEditFees.Visible = False
                    cmdEditFees.Enabled = False
                    lblAgencyNum.Visible = False
                    txtAgencyNum.Enabled = False
                    txtAgencyNum.Visible = False

                    fmeMain.Height = fraAddress.Height

                    cmdNext(0).Top = tabMainTab.Height - VB6.TwipsToPixelsY(400)
                    cmdNext(1).Top = cmdNext(0).Top
                    cmdNext(2).Top = cmdNext(0).Top
                    cmdPrev(0).Top = cmdNext(0).Top
                    cmdPrev(1).Top = cmdNext(0).Top
                    cmdPrev(2).Top = cmdNext(0).Top

                    chkCRFee_charge.Visible = False

                    chkRiskTransferAgreement.Visible = False
                    chkDelegatedAuthority.Visible = False
                    lblFSAProduct.Visible = False
                    cboFSAProduct.Visible = False

            End Select

            m_lReturn = SetupUWFeeList()

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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


            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtAccountName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            'SP090998
            m_ctlTabFirstLast(MainModule.ACControlStart, 0) = txtAccountCode
            m_ctlTabFirstLast(MainModule.ACControlEnd, 0) = lvwFees


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


            'developer guide no. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddFees.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditFees.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDeleteFees.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '-----------------


            lblAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Added Remark MK 990929 1655
            'lblAccountCode = "Agent Code:"

            'lblAdReference.Caption = iPMFunc.GetResData( _
            ''iLangID:=g_iLanguageID%, _
            ''lID:=ACReference, _
            ''iDataType:=PMResString)

            'lblAdReference = "Agent Code:"

            'lblConReference.Caption = iPMFunc.GetResData( _
            ''    iLangID:=g_iLanguageID%, _
            ''    lID:=ACReference, _
            ''    iDataType:=PMResString)

            'lblConReference = "Agent Code:"

            'lblIDPostcode.Caption = iPMFunc.GetResData( _
            ''    iLangID:=g_iLanguageID%, _
            ''    lID:=ACPostcode, _
            ''    iDataType:=PMResString)

            'lblAdPostcode.Caption = iPMFunc.GetResData( _
            ''    iLangID:=g_iLanguageID%, _
            ''    lID:=ACPostcode, _
            ''    iDataType:=PMResString)



            lblAccountName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkRiskTransferAgreement.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskTransferAgreement, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkDelegatedAuthority.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelegatedAuthority, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFSAProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFSAProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

                Case gPMConstants.PMEComponentAction.PMEdit
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



            ' Error Section.

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
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: UpdateAddressPostCodeProperties
    '
    ' Description: This checks for the main address and gets the
    ' post code and address line 1 for it via the address business
    '
    ' ***************************************************************** '
    Private Sub UpdateAddressPostCodeProperties()
        Dim lAddressCnt As Integer
        Dim oAddressBusiness As bSIRAddress.Business
        Dim sAddressUsage As String = ""

        Try

            'Find the main address
            For i As Integer = 1 To lvwAddresses.Items.Count

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(i - 1), 1).Text.Trim()
                    Case Else
                        sAddressUsage = lvwAddresses.Items.Item(i - 1).Text.Trim()
                End Select

                For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                        lAddressCnt = Convert.ToString(lvwAddresses.Items.Item(i - 1).Tag)
                        Exit For
                    End If
                Next j
            Next i


            'Get address business to retrieve
            Dim temp_oAddressBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAddressBusiness, "bSIRAddress.Business", vInstanceManager:="ClientManager")
            oAddressBusiness = temp_oAddressBusiness


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If



            oAddressBusiness.AddressCnt = lAddressCnt


            m_lReturn = oAddressBusiness.GetDetails(vAddressCnt:=lAddressCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
                oAddressBusiness = Nothing
                Exit Sub
            End If


            m_lReturn = oAddressBusiness.GetNext(vPostalCode:=m_sMainPostCode, vAddress1:=m_sAddressLine1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
                oAddressBusiness = Nothing
                Exit Sub
            End If


            oAddressBusiness.Dispose()
            oAddressBusiness = Nothing

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodePropertiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: This goes thru all addresses in the the grid control
    ' and the original address array and sees what the differences
    ' are. It then adds new addresses or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateAddresses() As Integer

        Dim result As Integer = 0
        Dim vNewAddresses, vOldAddresses(,) As Object
        Dim bFirst As Boolean
        Dim i As Integer
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Go thru original address array to get list of old addresses
            If Information.IsArray(m_vAddresses) Then
                ReDim vOldAddresses(1, m_vAddresses.GetUpperBound(1))
                For i = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                    vOldAddresses(0, i) = CInt(m_vAddresses(6, i))

                    vOldAddresses(1, i) = CInt(m_vAddresses(1, i))
                Next i
            End If

            'Go thru addresses grid to get list of new addresses
            i = 1
            bFirst = True
            Do
                If i > lvwAddresses.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwAddresses.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewAddresses(1, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewAddresses(1, i - 1)
                    End If



                    vNewAddresses(0, i - 1) = Convert.ToString(oListItem.Tag)

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddressTypes(1, j)) Then

                            vNewAddresses(1, i - 1) = m_vAddressTypes(0, j)
                            Exit For
                        End If
                    Next j

                End If
                i += 1
            Loop


            'Delete old address usages in database
            If (Information.IsArray(vOldAddresses)) And (Not Information.IsArray(vNewAddresses)) Then
                For i = 0 To vOldAddresses.GetUpperBound(1)
                    m_lReturn = m_oBusiness.DeleteAddress(m_lPartyCnt, vOldAddresses(0, i))
                Next

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vAddAddresses:=vNewAddresses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'If we have old and new addresses, delete common ones
            If (Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                'Delete unchanged addresses (ie set them to 0)

                For i = vOldAddresses.GetLowerBound(1) To vOldAddresses.GetUpperBound(1)

                    For j As Integer = vNewAddresses.GetLowerBound(1) To vNewAddresses.GetUpperBound(1)




                        If (vNewAddresses(0, j).Equals(vOldAddresses(0, i))) And (vNewAddresses(1, j).Equals(vOldAddresses(1, i))) Then

                            vNewAddresses(0, j) = 0

                            vOldAddresses(0, i) = 0
                        End If
                    Next j
                Next i

                For m As Integer = 0 To vOldAddresses.GetUpperBound(1)
                    Dim nUnmatchRecord As Integer = 0
                    nUnmatchRecord = DeleteAddress(CInt(vOldAddresses(0, m)), vNewAddresses)
                    If nUnmatchRecord = 1 Then
                        m_lReturn = m_oBusiness.DeleteAddress(m_lPartyCnt, vOldAddresses(0, m))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next

                'update the database
                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses, vAddAddresses:=vNewAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddressesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateAgencyNumber
    '
    ' Desc: Save away the agency number
    '
    ' AMB 13-Oct-03: 1.8.6 Accident Management development - created
    ' ***************************************************************** '
    Private Function UpdateAgencyNumber() As Integer

        Dim result As Integer = 0
        Try


            m_lReturn = m_oBusiness.UpdateAgencyNumber(v_lPartyCnt:=m_lPartyCnt, v_sAgencyNumber:=m_sAgencyNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update agency number failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAgencyNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: This goes thru all contacts in the the grid control
    ' and the original contact array and sees what the differences
    ' are. It then adds new contacts or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateContacts() As Integer

        Dim result As Integer = 0
        Dim vNewContacts, vOldContacts As Object
        Dim bFirst As Boolean
        Dim i As Integer
        Dim oListItem As ListViewItem


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Go thru original contact array to get list of old contacts
            If Information.IsArray(m_vContacts) Then
                ReDim vOldContacts(m_vContacts.GetUpperBound(1))
                For i = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                    vOldContacts(i) = CInt(m_vContacts(0, i))
                Next i
            End If

            'Go thru contacts grid to get list of new contacts
            i = 1
            bFirst = True

            Do
                If i > lvwContacts.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwContacts.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewContacts(i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewContacts(i - 1)
                    End If



                    vNewContacts(i - 1) = Convert.ToString(oListItem.Tag)

                End If
                i += 1
            Loop


            'Delete old contact usages in database
            If (Information.IsArray(vOldContacts)) And (Not Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vAddContacts:=vNewContacts)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'If we have old and new Contacts, delete common ones
            If (Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                'Delete unchanged Contacts (ie set them to 0)

                For i = vOldContacts.GetLowerBound(0) To vOldContacts.GetUpperBound(0)

                    For j As Integer = vNewContacts.GetLowerBound(0) To vNewContacts.GetUpperBound(0)


                        If vNewContacts(j).Equals(vOldContacts(i)) Then

                            vNewContacts(j) = 0

                            vOldContacts(i) = 0
                        End If
                    Next j
                Next i

                'update the database

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts, vAddContacts:=vNewContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateOrion
    '
    ' Description: Add Orion account
    '
    ' eck240101 - Taken from uctPartyPCControl (by ECK)
    '
    ' ***************************************************************** '
    Private Function UpdateOrion(ByRef vPartyCnt As Object) As Integer
        Dim result As Integer = 0
        Dim oSIROrionUpdate As bSIROrionUpdate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oSIROrionUpdate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIROrionUpdate, "bSIROrionUpdate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSIROrionUpdate = temp_oSIROrionUpdate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIROrionUpdate.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' CTAF 021000 - Commented out the source code for now until it's
            '               decided what to do about insurers and changing sources

            ' Get Orion Account IDs
            ' eck010900
            '    Select Case Task
            '        Case PMAdd

            m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt)
            '        Case PMEdit
            '            m_lReturn& = oSIROrionUpdate.SiriusToOrion( _
            ''                            v_lPartyCnt:=vPartyCnt, _
            ''                            v_iOldSourceId:=m_iPartySourceId, _
            ''                            v_iOldPartyId:=m_iPartyId)
            '    End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oSIROrionUpdate.Dispose()

            oSIROrionUpdate = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub cboPaymentMethod_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentMethod.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldChange(Me)
    End Sub

    Private Sub cboPaymentMethod_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentMethod.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboPaymentMethod)

    End Sub

    Private Sub cboPaymentMethod_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPaymentMethod.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboPaymentMethod_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentMethod.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboPaymentMethod)

    End Sub

    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click
        Dim sTmp As String = ""
        Dim oListItem As ListViewItem

        Try

            'Create icontact if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the address interface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_oAddress.Reference = txtAccountCode.Text
            '    m_lReturn& = ControlLostFocus(cboTermsOfPayment)
            'PSL 20/02/2003  NRMA don't have postcodes

            m_oAddress.IsNRMA = m_bIsNRMA

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtAccountCode.Text

            m_oAddress.PostCode = m_sMainPostCode

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHeirarchy = "" Then
                If m_lPartyTypeID = 9 Then
                    m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 10 Then
                    m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 11 Then
                    m_sScreenHeirarchy = $"Discount Account({txtAccountCode.Text.Trim()})"
                End If
            End If

            m_oAddress.UniqueId = m_sUniqueId
            m_oAddress.ScreenHeirarchy = m_sScreenHeirarchy
            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()


            oListItem = lvwAddresses.Items.Add(m_oAddress.PostalCode)

            ' Assign details to other the columns
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType

            'MKR 09/11/2004 PN 16102    -- Commenting the code to change the caption of Form.
            'If m_oAddress.AddressUsageType = SIRMainAddressABIDescription Then
            '    m_sMainPostCode = m_oAddress.PostalCode
            '    Caption = "Insurer: " & m_sShortName & " " & m_sMainPostCode
            'End If
            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3

            ' Column 6

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4

            ' Store the Address_cnt

            oListItem.Tag = m_oAddress.AddressCnt

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
        If lvwAddresses.Items.Count <> 0 Then
            lvwAddresses.Items(0).Selected = True
        End If

    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click
        Dim oListItem As ListViewItem

        Try

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = m_oContact.ContactCnt


            m_oContact.Reference = txtAccountCode.Text

            m_oContact.PostCode = m_sMainPostCode

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHeirarchy = "" Then
                If m_lPartyTypeID = 9 Then
                    m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 10 Then
                    m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 11 Then
                    m_sScreenHeirarchy = $"Discount Account({txtAccountCode.Text.Trim()})"
                End If
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_sScreenHeirarchy
            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()


            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode)

            ' Assign details to other the columns
            ' Column 2
            'Temporary thing

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description


            ' Store the Address_cnt

            oListItem.Tag = m_oContact.ContactCnt

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
        If lvwContacts.Items.Count <> 0 Then
            lvwContacts.Items(0).Selected = True
        End If
    End Sub

    ' PRIVATE Methods (End)

    Private Sub cmdAddFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFees.Click

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)



            lReturn = CType(AddUWPolicyFee(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddUWPolicyFee Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingFeeInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddFees_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddFees_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
        If lvwFees.Items.Count <> 0 Then
            lvwFees.Items(0).Selected = True
        End If

    End Sub

    Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click
        Dim oListItem As ListViewItem

        Try

            'Set row to be deleted - if a valid one selected
            'Set row to be deleted - if a valid one selected
            If lvwAddresses.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'oListItem = lvwAddresses.FocusedItem
            oListItem = lvwAddresses.SelectedItems.Item(0)
            'set the address id


            m_oAddress.AddressCnt = Convert.ToString(oListItem.Tag)

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            'm_oAddress.addresscnt = m_lAddressCnt
            'm_oAddress.AddressUsageTypeId = m_lAddressUsageTypeID


            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Update the address details
            ' & postcode



            'lvwAddresses.Items.RemoveAt(lvwAddresses.FocusedItem.Index)
            lvwAddresses.Items.RemoveAt(lvwAddresses.SelectedItems(0).Index)

            lvwAddresses.Focus()
            'lvwAddresses.Items(0).Selected = True


        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
        'lvwAddresses.FocusedItem.Selected = True
        If lvwAddresses.Items.Count <> 0 Then
            lvwAddresses.Items(0).Selected = True
        End If
    End Sub

    Private Sub cmdDeleteCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteCon.Click

        Try

            'Set row to be deleted - if a valid one selected
            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id


            'm_oContact.ContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
            m_oContact.ContactCnt = Convert.ToString(lvwContacts.SelectedItems.Item(0).Tag)


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'lvwContacts.Items.RemoveAt(lvwContacts.FocusedItem.Index)
            lvwContacts.Items.RemoveAt(lvwContacts.SelectedItems(0).Index)

            lvwContacts.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
        If lvwContacts.Items.Count <> 0 Then
            lvwContacts.Items(0).Selected = True
        End If

    End Sub

    Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click
        Dim oListItem As ListViewItem
        Dim sTmp As String = ""

        Try

            'Set the address count being edited - if a valid one selected

            'Set the address count being edited - if a valid one selected

            If lvwAddresses.Items.Count < 1 Then
                Exit Sub
            End If



            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtAccountCode.Text

            m_oAddress.PostCode = m_sMainPostCode
            'PSL 20/02/2003  NRMA don't have postcodes

            m_oAddress.IsNRMA = m_bIsNRMA

            oListItem = lvwAddresses.FocusedItem
            'set the address id


            m_oAddress.AddressCnt = Convert.ToString(oListItem.Tag)

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHeirarchy = "" Then
                If m_lPartyTypeID = 9 Then
                    m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 10 Then
                    m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 11 Then
                    m_sScreenHeirarchy = $"Discount Account({txtAccountCode.Text.Trim()})"
                End If
            End If

            m_oAddress.UniqueId = m_sUniqueId
            m_oAddress.ScreenHeirarchy = m_sScreenHeirarchy
            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Column 1

            oListItem.Text = m_oAddress.PostalCode

            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType

            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
            End If
            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3

            ' Column 6

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try
    End Sub

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click
        Dim oListItem As ListViewItem

        Try

            'Set the contact count being edited - if a valid one selected

            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oContact.Reference = txtAccountCode.Text

            m_oContact.PostCode = m_sMainPostCode

            'set the contact id
            oListItem = lvwContacts.FocusedItem



            m_oContact.ContactCnt = Convert.ToString(oListItem.Tag)

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_sScreenHeirarchy = "" Then
                If m_lPartyTypeID = 9 Then
                    m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 10 Then
                    m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 11 Then
                    m_sScreenHeirarchy = $"Discount Account({txtAccountCode.Text.Trim()})"
                End If
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_sScreenHeirarchy

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If


            oListItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdEditFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditFees.Click

        'MKR 09/11/2004 PN 15615/14685
        'MKW PN16680
        Dim lReturn As Integer

        Try

            'Set row to be deleted - if a valid one selected
            If lvwFees.Items.Count < 1 Then
                Exit Sub
            End If

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            lReturn = EditUWPolicyFee()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUWPolicyFee Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingFeeInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub

            End If

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception







            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditFees_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditFees_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
    ''' <summary>
    ''' cmdDeleteFees_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdDeleteFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteFees.Click

        Dim bDeleteClick As Boolean = True
        Dim bConfirmDelete As Boolean = True
        Dim vFeeList As Object = Nothing
        Try

            'Set row to be deleted - if a valid one selected
            If lvwFees.Items.Count < 1 Then
                Exit Sub
            End If


            If lvwFees.FocusedItem IsNot Nothing Then
                vFeeList = lvwFees.FocusedItem
            End If

            m_lReturn = EditUWPolicyFee(bDeleteClick, bConfirmDelete)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Underwriting Fees", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingFeeInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

            'extra code branch for underwriting

            If bConfirmDelete Then

                m_lReturn = TempDeletingUWFee(vFeeList)

                'm_lReturn& = DeleteUnderwritingFee()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Underwriting Fees", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingFeeInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If
                m_bIsAnyDeletedFee = gPMConstants.PMEReturnCode.PMTrue

            End If
        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteFees_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteFees_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
        If lvwFees.Items.Count <> 0 Then
            lvwFees.Items(0).Selected = True
        End If
    End Sub


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'pass control to show help file
        'Developer Guide No 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            Exit Sub
        End Try


    End Sub

    Private Sub cmdPrev_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrev_2.Click, _cmdPrev_1.Click, _cmdPrev_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrev, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            Exit Sub
        End Try


    End Sub

    Private Function GetUserAuthorities() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oUserAuthorities As bACTUserAuthorities.Business
            Dim temp_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            End If


            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="is_view_only_account_executive_maintenance", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=m_bIsViewOnlyEXMaintenance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            Else
                'Start - Prakash Varghese - PN 61117
                'Modified the condition ToSafeBoolean(m_bIsViewOnlyEXMaintenance) = True
                'to ToSafeBoolean(m_bIsViewOnlyEXMaintenance) since it is giving problems in runtime
                If gPMFunctions.ToSafeBoolean(m_bIsViewOnlyEXMaintenance) Then
                    m_iTask = gPMConstants.PMEComponentAction.PMView
                End If
                'End - Prakash Varghese - PN 61117
            End If

            'Terminate the object

            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthoritiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

        Dim sMessage, sTitle, sValue As String

        ' Forms initialise event.

        Try
            'shifted here as Form_Initialize_Renamed called first
            ' Create an instance of the object manager.
            'g_oObjectManager = New bObjectManager.ObjectManager()
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyEX.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            Dim temp_m_oFeeBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFeeBusiness, "bSIRPartyFee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oFeeBusiness = temp_m_oFeeBusiness

            'GW250504 PN103 extra business object for
            'underwriting party fee branch
            Dim temp_m_oUFeeBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oUFeeBusiness, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUFeeBusiness = temp_m_oUFeeBusiness

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
            'EK 16/10/99 Add Instance of Lookup
            Dim temp_m_oLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oLookup = temp_m_oLookup
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

            ' Initialise PM Lookup Business passing our Database Reference.

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            'added to show help
            PMHelpFunc.g_sProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPartyEX.General()

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

            ' List manager
            PMBGeneralFunc.g_oListManager = New iGEMListManager.Interface_Renamed()

            ' Initialise it
            m_lReturn = PMBGeneralFunc.g_oListManager.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Check for latest version
            m_lReturn = PMBGeneralFunc.g_oListManager.CheckListVersions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=1, r_vUnderwriting:=sValue)
            m_bFSAEnabled = sValue = "1"

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'developer guide no. 36
            Me.cboFSAProduct.FirstItem = "(EMPTY)"
            Me.uctCurrency.FirstItem = ""

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()


        ' Forms load event.

        Try


            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get User Authorities for Setting up the mode
            m_lReturn = GetUserAuthorities()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            'GW180504 - check to see if business is agency or underwriting
            'and ammend check box visibilty

            chkCRFee_charge.Visible = False

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.

            m_oBusiness.PartyCnt = m_lPartyCnt

            ' AMB 10-Oct-03: 1.8.6 Accident Management development - get the accident management system options
            If m_sPartyType = PMBConst.PMBPartyTypeExtra Then

                m_lReturn = m_oBusiness.GetAccidentManSysOptions(r_lEnabled:=m_lAccManEnabled, r_sAgencyNumber:=m_sAccManAgencyNumber, r_lAgencyNumberIsDefault:=m_lAccManAgencyNumberIsDefault)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddresstypelookups(vAddressTypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the address type look up details from the business object LoadControl", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                End If

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                        Exit For
                    End If

                Next i
                'PN16993
                uctCurrency.CurrencyId = g_oObjectManager.CurrencyID

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

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
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
            'EK 16/10/99
            ' Terminate the general object.

            m_oLookup.Dispose()

            ' Destroy the instance of the general object
            ' from memory.
            m_oLookup = Nothing

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

            ' Terminate the address object (if used)
            If Not (m_oAddress Is Nothing) Then


                m_oAddress.Dispose()

                ' Destroy the instance of the Address object
                ' from memory.
                m_oAddress = Nothing

            End If

            ' Terminate the Fee object (if used)
            If Not (m_oFee Is Nothing) Then


                m_oFee.Dispose()

                ' Destroy the instance of the Fee object
                ' from memory.
                m_oFee = Nothing

            End If

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
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwAddresses_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwAddresses.Click
        lvwAddresses.FocusedItem.Selected = True
    End Sub

    Private Sub lvwAddresses_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.DoubleClick

        ' Activate the edit button
        cmdEditAd_Click(cmdEditAd, New EventArgs())

    End Sub

    Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Not (lvwAddresses.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteAd.Enabled = True
            cmdEditAd.Enabled = True
        Else
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False
        End If

    End Sub
    Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Not (lvwContacts.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        Else
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        End If

    End Sub

    Private Sub lvwContacts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwContacts.Click
        lvwContacts.FocusedItem.Selected = True
    End Sub

    Private Sub lvwContacts_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.DoubleClick

        cmdEditCon_Click(cmdEditCon, New EventArgs())

    End Sub

    Private Sub lvwFees_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwFees.Click
        lvwFees.FocusedItem.Selected = True
    End Sub

    Private Sub lvwFees_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwFees.DoubleClick

        ' Activate the edit button
        cmdEditFees_Click(cmdEditFees, New EventArgs())

    End Sub

    Private Sub tabMainTab_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMainTab.Enter
        If lvwFees.Items.Count <> 0 Then
            lvwFees.Items(0).Selected = True
        End If
        'If lvwAddresses.Items.Count <> 0 Then
        '    lvwAddresses.Items(0).Selected=True
        'End If
        'If lvwContacts.Items.Count <> 0 Then
        '    lvwContacts.Items(0).Selected = True
        'End If
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab

                ' Set the default button.
                '        If (.Tab < cmdNext.Count) Then
                '            cmdNext(.Tab).Default = True
                '        Else
                '            cmdOK.Default = True
                '        End If

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

                If lvwAddresses.Items.Count <> 0 Then
                    lvwAddresses.Items(0).Selected = True
                End If
                If lvwContacts.Items.Count <> 0 Then
                    lvwContacts.Items(0).Selected = True
                End If

            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim nReturn As Integer
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


            m_lReturn = DeleteTempDeletedFee()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdOK_Click", "Delete Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'EK 12/11/99 Check that we are not creating a duplicate code
            m_lReturn = ValidateOK()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            '
            'Validate some PartEX stuff
            'TO CHECK Remark put on temporarily MK 991011
            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMView

                Case gPMConstants.PMEComponentAction.PMEdit
                    'm_lReturn = ValidateFeeDetailsEdit() 'ValidateOK()

            End Select
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the mouse pointer to the hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Set the mouse back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    'update the party cnt property
                    'm_lPartyCnt = m_oBusiness.PartyCnt
                End If

                ' save additional details back to party record.
                m_lReturn = UpdatePartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                ' AMB 13-Oct-03: 1.8.6 Accident Management development - save the agency number
                m_lReturn = UpdateAgencyNumber()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Agency Number", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                'Update party addresses
                m_lReturn = UpdateAddresses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If
                ' CTAF 021000 - Update Orion
                m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                'Update party contacts
                m_lReturn = UpdateContacts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()


                'DC260106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

                Dim oPartyBusiness As bSIRParty.Business = Nothing
                nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If

                oPartyBusiness.AddPartyHistory(m_lPartyCnt, String.Empty)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If
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



    ' PRIVATE Events (End)


    ' PRIVATE Events (End)

    Private Sub txtAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAccountCode)

        txtAccountCode.MaxLength = 20

    End Sub
    Private Sub txtAccountCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAccountCode)

    End Sub

    Private Sub txtAccountName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountName.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAccountName)

    End Sub

    Private Sub txtAccountName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountName.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAccountName)

    End Sub

    '991006 MK
    'UPGRADE_NOTE: (7001) The following declaration (ValidateFeeDetailsAdd) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidateFeeDetailsAdd() As gPMConstants.PMEReturnCode
    '
    'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    'Dim lRiskGroupId, lSchemeID, lTransactionTypeID As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If Not Information.IsArray(m_vFeeDetails) Then
    'Return result
    'End If
    '

    'lRiskGroupId = m_oFee.riskGroupId

    'lSchemeID = m_oFee.ExtraSchemeID
    'For 'lRow As Integer = m_vFeeDetails.GetLowerBound(1) To m_vFeeDetails.GetUpperBound(1)
    'If CInt(m_vFeeDetails(0, lRow)) = lRiskGroupId Then
    ' AMB 16-Oct-03: 1.8.6 Accident Management development - need to check scheme now also
    'MKR 09/11/2004 PN 15615/14685 -- Applying the check to avoid Type mismatch error
    'Dim dbNumericTemp As Double
    'If Double.TryParse(CStr(m_vFeeDetails(9, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And lSchemeID > 0 Then
    'If CInt(m_vFeeDetails(9, lRow)) = lSchemeID Then
    'MessageBox.Show("The risk group and scheme combination must be unique", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    'result = gPMConstants.PMEReturnCode.PMFalse
    'Exit For
    'End If
    'End If
    'End If
    'Next lRow
    '
    'MKW PN16680 Check Madatory On is unique for Risk Group.

    'lRiskGroupId = m_oFee.riskGroupId

    'lTransactionTypeID = m_oFee.TransactionTypeID
    'For 'lRow As Integer = m_vFeeDetails.GetLowerBound(1) To m_vFeeDetails.GetUpperBound(1)
    'If CInt(m_vFeeDetails(0, lRow)) = lRiskGroupId Then
    ' AMB 16-Oct-03: 1.8.6 Accident Management development - need to check scheme now also
    'MKR 09/11/2004 PN 15615/14685 -- Applying the check to avoid Type mismatch error
    'Dim dbNumericTemp2 As Double
    'If Double.TryParse(CStr(m_vFeeDetails(7, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And lTransactionTypeID > 0 Then
    'If CInt(m_vFeeDetails(7, lRow)) = lTransactionTypeID Then
    'MessageBox.Show("The risk group and 'Mandatory On' field must be unique", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    'result = gPMConstants.PMEReturnCode.PMFalse
    'Exit For
    'End If
    'End If
    'End If
    'Next lRow
    'PopulateFees()
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Fee Details", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateFeeDetailsAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    'UPGRADE_NOTE: (7001) The following declaration (ValidateFeeDetailsEdit) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidateFeeDetailsEdit() As gPMConstants.PMEReturnCode
    '
    'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If Not Information.IsArray(m_vFeeDetails) Then
    'Return result
    'End If
    'For 'lRow As Integer = m_vFeeDetails.GetLowerBound(1) To m_vFeeDetails.GetUpperBound(1)


    'If m_oFee.BusinessTypeID = CInt(m_vFeeDetails(0, lRow)) And m_oFee.RiskCodeID = CInt(m_vFeeDetails(1, lRow)) Then
    'm_vFeeDetails(0, lRow&) = CStr(m_oFee.BusinessTypeID)
    'm_vFeeDetails(1, lRow&) = CStr(m_oFee.RiskCodeID)

    'm_vFeeDetails(2, lRow) = m_oFee.riskcode

    'm_vFeeDetails(3, lRow) = m_oFee.FeePercentage

    'm_vFeeDetails(4, lRow) = m_oFee.FeeAmount

    'm_vFeeDetails(5, lRow) = m_oFee.FeeCommissionPercentage

    'm_vFeeDetails(6, lRow) = m_oFee.FeeCommissionAmount
    ' AMB 13-Oct-03: 1.8.6 Accident Management development

    'm_vFeeDetails(9, lRow) = gPMFunctions.NullToLong(m_oFee.ExtraSchemeID)

    'm_vFeeDetails(10, lRow) = gPMFunctions.NullToString(m_oFee.ExtraSchemeDesc)
    'result = gPMConstants.PMEReturnCode.PMTrue
    ' AMB 13-Oct-03: 1.8.6 Accident Management development - refresh listview
    'PopulateFees()
    'Return result
    'End If
    'Next lRow
    '
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Fee Details", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateFeeDetailsEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    'EK 12/11/99
    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates mandatory data
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer


        Dim result As Integer = 0
        Dim iMainAddresses As Integer
        Dim bDuplicate As Boolean
        Dim lAddressCnt As Integer
        Dim oListItem, oListItem2 As ListViewItem
        Dim sAddressUsage, sChangeCode, sPartyCode, sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check to see if client code already exists

            'If this is a new client or an existing one with it's client code changed then
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtAccountCode.Text.Trim() <> m_sShortName.Trim()) Then

                sPartyCode = txtAccountCode.Text.Trim()


                m_lReturn = m_oBusiness.CheckReference(sPartyCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to access business object", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If the returned code is an empty string, then the code already exists
                If sPartyCode = "" Then

                    sMsg = "The party code entered already exists."
                    MessageBox.Show(sMsg, "Invalid Party Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Regex.IsMatch(sPartyCode, "^[A-Za-z0-9]+$") Then
                    MessageBox.Show("Special characters are not allowed in Account Code", "Invalid Account Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'Add this just in case Apply is added at later stage
                m_sShortName = txtAccountCode.Text
            End If

            'Display warning message that client code has changed.
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtAccountCode.Text.Trim() <> m_sShortName.Trim() Then

                sChangeCode = CStr(MessageBox.Show("Warning! You are about to change the party code. Do you wish to change the code?", "Client Code Changed", MessageBoxButtons.YesNo))
                Select Case sChangeCode
                    Case CStr(System.Windows.Forms.DialogResult.No)
                        txtAccountCode.Text = m_sShortName
                End Select

            End If

            ' mkw 170103 Only process Mandatory Correspondance Address for Extras.
            If m_sPartyType = PMBConst.PMBPartyTypeExtra Then

                'IJB 14/10/02 Make Correspondance Address Mandatory

                iMainAddresses = 0

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                        Exit For
                    End If

                Next i

                'Count how many addresses are main address
                If lvwAddresses.Items.Count > 0 Then
                    For i As Integer = 1 To lvwAddresses.Items.Count
                        oListItem = lvwAddresses.Items.Item(i - 1)

                        sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(i - 1), 1).Text.Trim()

                        For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                            If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                                iMainAddresses += 1
                            End If
                        Next j
                    Next i
                End If

                Select Case iMainAddresses
                    Case 0
                        'No
                        MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                        cmdAddAd.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Case 1
                        'Yes
                    Case Else
                        'No.
                        MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                        cmdAddAd.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                End Select

                'Now Ensure addresses are not used twice
                If lvwAddresses.Items.Count < 2 Then
                    'less than 2 addresses so cant have duplicates
                    Return result
                End If

                bDuplicate = False

                'Check for duplicates
                'Need to find out how to calculate no of lines in the grid
                For i As Integer = 1 To (lvwAddresses.Items.Count)
                    oListItem = lvwAddresses.Items.Item(i - 1)
                    If CBool(CStr(Convert.ToString(oListItem.Tag) <> "").Trim()) Then
                        lAddressCnt = Convert.ToString(oListItem.Tag)
                        'RWH(24/07/2000) Why not just use the count property?
                        '            For j = (i + 1) To m_lAddressCount
                        For j As Integer = (i + 1) To lvwAddresses.Items.Count
                            oListItem2 = lvwAddresses.Items.Item(j - 1)
                            If CBool(CStr(Convert.ToString(oListItem2.Tag) <> "").Trim()) Then
                                If (Convert.ToString(oListItem2.Tag)) = lAddressCnt Then
                                    bDuplicate = True
                                    Exit For
                                End If
                            End If
                        Next j
                    End If
                Next i

                If bDuplicate Then
                    MessageBox.Show("An address can only be used once by a particular party.", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'IJB 14/10/02 Make Correspondance Address Mandatory End

            End If ' mkw 170103 Only process Mandatory Correspondance Address for Extras End

            ' AMB 10-Oct-03: 1.8.6 Accident Management development - check agency number if necessary
            If m_sPartyType = PMBConst.PMBPartyTypeExtra Then
                If m_lAccManEnabled <> 0 And txtAccountCode.Text = "ACCCA" Then 'PN13710 Only for ACCA account
                    If Strings.Len(txtAgencyNum.Text) = 0 Then
                        MessageBox.Show("The agency number is mandatory.", "Agency Number", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtAgencyNum.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UPopulateFees
    '
    ' Description: Fills the grid control with Underwriting Fee data
    ' PN013 Underwriting fees
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UPopulateFees) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub UPopulateFees()
    '
    'Dim i As Integer
    'Dim cTemp As Object
    'Dim oListItem As ListViewItem
    'Dim colX As ColumnHeader
    'Dim bShowScheme As Boolean
    '
    'Const FeeImage As String = "FeeImage"
    '
    'Try 
    '
    'If Not Information.IsArray(m_vUFeeDetails) Then
    'Exit Sub
    'End If
    '
    'lvwFees.Items.Clear()
    '
    '    Set colX = lvwFees.ColumnHeaders.Item(eUFeesColumns.Col_Product + 1)
    '    colX.Width = 1600
    '    colX.Text = "Product"
    '    Set colX = lvwFees.ColumnHeaders.Item(eUFeesColumns.Col_Transaction + 1)
    '    colX.Width = 1600
    '    colX.Text = "Transaction Type"
    '    Set colX = lvwFees.ColumnHeaders.Item(eUFeesColumns.Col_UPerc + 1)
    '    colX.Width = 1400
    '    colX.Text = "Percentage"
    '    Set colX = lvwFees.ColumnHeaders.Item(eUFeesColumns.Col_UAmount + 1)
    '    colX.Width = 1400
    '    colX.Text = "Amount"
    '    Set colX = lvwFees.ColumnHeaders.Item(eUFeesColumns.Col_EffecDate + 1)
    '    colX.Width = 1400
    '    colX.Text = "Effective Date"
    '    Set colX = lvwFees.ColumnHeaders.Item(eUFeesColumns.Col_Currency + 1)
    '    colX.Width = 1400
    '    colX.Text = "Currency"
    '
    '    m_iFeeElements = UBound(m_vUFeeDetails, 2)
    ' Assign the details to the interface.
    '
    ''    For i = LBound(m_vUFeeDetails, 2) To UBound(m_vUFeeDetails, 2)
    ''
    ''        'Don't display ones that have been deleted.
    ''        If Trim$(m_vUFeeDetails(1, i)) <> "" Then
    ''
    ''            ' Column - product
    ''            Set oListItem = lvwFees.ListItems.Add(, , _
    '''                    Trim$(m_vUFeeDetails(2, i)), , ksPolicyImage)
    ''
    ''            'Column - Transaction
    ''            m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtTransType, _
    '''                    vControlValue:=Trim$(m_vUFeeDetails(4, i)))
    ''            oListItem.SubItems(eUFeesColumns.Col_Transaction) = txtTransType.Text
    ''
    ''            ' Column - Percentage
    ''            m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtPercentage, _
    '''                    vControlValue:=Trim$(m_vUFeeDetails(5, i)))
    ''            oListItem.SubItems(eUFeesColumns.Col_UPerc) = txtPercentage.Text
    ''
    ''            ' Column - Amount
    ''            m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtAmount, _
    '''                    vControlValue:=Trim$(m_vUFeeDetails(6, i)))
    ''            oListItem.SubItems(eUFeesColumns.Col_UAmount) = txtAmount.Text
    ''
    ''            ' Column - effective date
    ''            m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, _
    '''                    vControlValue:=Trim$(m_vUFeeDetails(7, i)))
    ''            oListItem.SubItems(eUFeesColumns.Col_EffecDate) = Format(txtEffectiveDate.Text, "Short date")
    ''
    ''            'currency column
    ''            oListItem.SubItems(eUFeesColumns.Col_Currency) = Trim(m_vUFeeDetails(8, i))
    ''
    ''            ' Store the Fee_cnt
    ''            oListItem.Tag = i
    ''
    ''        End If
    ''
    '    Next i
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UPopulateFees", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub


    ' ***************************************************************** '
    ' Name: SetupUWFeeList
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function SetupUWFeeList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupUWFeeList"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            lvwFees.Columns.Clear()
            lvwFees.Items.Clear()

            lvwFees.Columns.Insert(kUWFeeColHeaderAppliesTo - 1, "kUWFeeColHeaderAppliesTo", "Applies To", CInt(VB6.TwipsToPixelsX(2000)))

            lvwFees.Columns.Insert(kUWFeeColHeaderEffectiveOn - 1, "kUWFeeColHeaderEffectiveOn", "Effective On", CInt(VB6.TwipsToPixelsX(1550)))

            lvwFees.Columns.Insert(kUWFeeColHeaderAppliesToType - 1, "kUWFeeColHeaderAppliesToType ", "Type", CInt(VB6.TwipsToPixelsX(1150)))

            lvwFees.Columns.Insert(kUWFeeColHeaderRate - 1, "kUWFeeColHeaderRate", "Rate", CInt(VB6.TwipsToPixelsX(1500)))

            lvwFees.Columns.Insert(kUWFeeColHeaderEffectiveDate - 1, "kUWFeeColHeaderEffectiveDate", "Effective Date", CInt(VB6.TwipsToPixelsX(1350)))

            lvwFees.Columns.Insert(kUWFeeColHeaderTaxed - 1, "kUWFeeColHeaderTaxed", "Taxed", CInt(VB6.TwipsToPixelsX(800)))

            lvwFees.Columns.Insert(kUWFeeColHeaderTaxGroup - 1, "kUWFeeColHeaderTaxGroup", "Tax Group", CInt(VB6.TwipsToPixelsX(1500)))

            lvwFees.Columns.Insert(kUWFeeColHeaderIns - 1, "kUWFeeColHeaderIns", "Include Fee In Instalments", CInt(VB6.TwipsToPixelsX(1500)))

            lvwFees.Columns.Insert(kUWFeeColHeaderSpread - 1, "kUWFeeColHeaderSpread", "Spread the Fee across Instalment", CInt(VB6.TwipsToPixelsX(1500)))


            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' PopulateUWFees
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PopulateUWFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateUWFees"

        Dim lReturn, llBound, lUBound As Integer
        Dim oListItem As ListViewItem
        Dim sAppliesTo, sFeeAmountId As String
        Dim lTransSubType As Integer
        Dim sEffectiveOn, sAppliesToType, sRate, sFormatString As String
        Dim dtEffectiveDate As Date
        Dim sTaxGroup, sTaxed As String
        Dim lType, lProductId, lRiskGroupId, lPerilGroupId, llBound1, lUBound1 As Integer
        Dim bFound As Boolean
        Dim lIncludeIns, lSpread As Integer
        Dim sIncludeIns, sSpread As String
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 170 (Latest guide)
            ListViewFunc.ListViewBatchStart(lvwFees)


            lvwFees.Items.Clear()

            If Information.IsArray(m_vUFeeDetails) Then

                llBound = m_vUFeeDetails.GetLowerBound(1)
                lUBound = m_vUFeeDetails.GetUpperBound(1)

                '0 ' fa.fee_amount_id,
                '1 ' fa.product_id,
                '2 ' fa.risk_type_group_id,
                '3 ' fa.peril_group_id,
                '4 ' p.description AS ProductDescription,
                '5 ' pg.description as PerilGroupDescription,
                '6 ' rg.description as RiskGroupDescription
                '7 ' fa.transaction_sub_type,
                '8 ' fa.fee_percentage,
                '9 ' fa.fee_amount,
                '10 ' fa.effective_date,
                '11 ' fa.tax_group_id
                '12 ' c.format_string

                For lFee As Integer = llBound To lUBound

                    bFound = False
                    lType = 0
                    sAppliesTo = ""
                    sAppliesToType = ""
                    sFeeAmountId = CStr(m_vUFeeDetails(UWItemsFeeAmountId, lFee))

                    If Information.IsArray(m_vDeletedFeeIDs) Then
                        llBound1 = m_vDeletedFeeIDs.GetLowerBound(0)
                        lUBound1 = m_vDeletedFeeIDs.GetUpperBound(0)
                        For lItem As Integer = llBound1 To lUBound1
                            If sFeeAmountId = CStr(m_vDeletedFeeIDs(lItem)) Then
                                bFound = True
                            End If
                        Next lItem
                    End If

                    If Not bFound Then

                        ' NB: 0 is a valid value for product - indicates that the fee applies across all products
                        If CStr(m_vUFeeDetails(UWItemsProductId, lFee)) <> "" Then
                            If CDbl(m_vUFeeDetails(UWItemsProductId, lFee)) = 0 Then
                                sAppliesTo = "(All)"
                            Else
                                sAppliesTo = CStr(m_vUFeeDetails(UWItemsProductDescription, lFee)).Trim()
                            End If
                            lType = kFeeTypeProduct
                            sAppliesToType = "Product"
                        ElseIf CStr(m_vUFeeDetails(UWItemsRiskTypeGroupId, lFee)) <> "" Then
                            lType = kFeeTypeRiskTypeGroup
                            sAppliesTo = CStr(m_vUFeeDetails(UWItemsRiskTypeGroupDescription, lFee)).Trim()
                            sAppliesToType = "Risk Type Group"
                        ElseIf CStr(m_vUFeeDetails(UWItemsPerilGroupId, lFee)) <> "" Then
                            lType = kFeeTypePerilGroup
                            sAppliesTo = CStr(m_vUFeeDetails(UWItemsPerilGroupDescription, lFee)).Trim()
                            sAppliesToType = "Peril Group"
                        End If

                        If Len(m_vUFeeDetails(UWItemsTransactionSubType, lFee)) <> 0 Then
                            lTransSubType = CInt(m_vUFeeDetails(UWItemsTransactionSubType, lFee))
                        Else
                            lTransSubType = kTransSubTypeAll
                        End If
                        sEffectiveOn = "(All)"

                        Select Case lTransSubType
                            Case gPMConstants.kTransSubTypeNB
                                sEffectiveOn = "New Business"
                            Case gPMConstants.kTransSubTypeAdditionMTA
                                sEffectiveOn = "Additional MTA"
                            Case gPMConstants.kTransSubTypeReturnMTA
                                sEffectiveOn = "Return MTA"
                            Case gPMConstants.kTransSubTypeCancellation
                                sEffectiveOn = "Cancellation"
                            Case gPMConstants.kTransSubTypeRenewal
                                sEffectiveOn = "Renewal"
                            Case gPMConstants.kTransSubTypeReInstatement
                                sEffectiveOn = "Re-Instatement"
                        End Select

                        sRate = "0"
                        'changes to make effects as vb 6
                        If CDbl(m_vUFeeDetails(UWItemsFeePercentage, lFee)) <> 0 Then
                            sRate = CStr(m_vUFeeDetails(UWItemsFeePercentage, lFee)).Trim()
                            If (m_vUFeeDetails(UWItemsFeePercentage, lFee)).Substring((m_vUFeeDetails(UWItemsFeePercentage, lFee)).IndexOf(".")) > 0 Then
                                sRate = CStr(Math.Round(CDbl(sRate), 2))
                            Else
                                sRate = CStr(CInt(sRate))
                            End If

                            sRate = sRate & "%"
                        ElseIf CDbl(m_vUFeeDetails(UWItemsFeeAmount, lFee)) <> 0 Then
                            sRate = CStr(m_vUFeeDetails(UWItemsFeeAmount, lFee)).Trim()
                            sFormatString = CStr(m_vUFeeDetails(UWItemsCurrencyFormatString, lFee)).Trim()
                            sRate = StringsHelper.Format(sRate, sFormatString)
                        End If

                        dtEffectiveDate = CDate(m_vUFeeDetails(UWItemsEffectiveDate, lFee))

                        sTaxGroup = CStr(m_vUFeeDetails(UWItemsTaxGroup, lFee)).Trim()
                        lIncludeIns = (CInt(m_vUFeeDetails(UWItemsIncludeIns, lFee)))
                        lSpread = (CInt(m_vUFeeDetails(UWItemsSpread, lFee)))
                        If sTaxGroup = "" Then
                            sTaxed = "No"
                        Else
                            sTaxed = "Yes"
                        End If
                        If lIncludeIns = 1 Then
                            sIncludeIns = "Yes"
                        Else
                            sIncludeIns = "No"
                        End If
                        If lSpread = 1 Then
                            sSpread = "Yes"
                        Else
                            sSpread = "No"
                        End If



                        ' populate list view with formatted values
                        oListItem = lvwFees.Items.Add(sAppliesTo)
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderEffectiveOn - 1).Text = sEffectiveOn
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderAppliesToType - 1).Text = sAppliesToType
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderRate - 1).Text = sRate
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderEffectiveDate - 1).Text = dtEffectiveDate.ToString("dd/MM/yyyy")
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderTaxed - 1).Text = sTaxed
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderTaxGroup - 1).Text = sTaxGroup
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderIns - 1).Text = sIncludeIns
                        ListViewHelper.GetListViewSubItem(oListItem, kUWFeeColHeaderSpread - 1).Text = sSpread

                        oListItem.Tag = sFeeAmountId
                    End If
                Next

            End If

            If lvwFees.Items.Count < 1 Then
                cmdDeleteFees.Enabled = False
                cmdEditFees.Enabled = False
            Else
                cmdDeleteFees.Enabled = True
                cmdEditFees.Enabled = True
            End If
            'developer guide no. 170(latest guide)
            ListViewFunc.ListViewBatchEnd()

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LoadUnderwritingFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function LoadUnderwritingFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadUnderwritingFees"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Fees for the party (underwriting)

            m_lReturn = m_oUFeeBusiness.GetAllFeeDetails(vPartyCnt:=m_lPartyCnt, vFeeDetails:=m_vUFeeDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError("m_oUFeeBusiness.GetAllFeeDetails", "vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DeleteUnderwritingFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function DeleteUnderwritingFee() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteUnderwritingFee"

        Dim lReturn, lItem As Integer
        Dim bItemSelected As Boolean
        Dim lFeeAmountId, lTag As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' get the selected uw policy fee to delete
            lReturn = GetSelectedUWFee(r_lFeeAmountId:=lFeeAmountId)
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'set property for the object call

                m_oUFeeBusiness.FeeAmountID = lFeeAmountId

                ' delete the specified fee amount

                lReturn = m_oUFeeBusiness.Delete()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Delete Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' reload the underwriting fees
                lReturn = ReloadUWPolicyFees()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ReloadUWPolicyFees Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function
    ''' <summary>
    ''' TempDeletingUWFee
    ''' </summary>
    ''' <param name="vFeeList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TempDeletingUWFee(ByVal vFeeList As Object) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "TempDeletingUWFee"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lFeeAmountId As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If Not Information.IsArray(m_vDeletedFeeIDs) Then
                ReDim m_vDeletedFeeIDs(0)
            ElseIf Information.IsArray(m_vDeletedFeeIDs) Then
                ReDim Preserve m_vDeletedFeeIDs(m_vDeletedFeeIDs.GetUpperBound(0) + 1)
            End If

            ' get the selected uw policy fee to delete
            m_lReturn = GetSelectedUWFee(r_lFeeAmountId:=lFeeAmountId, v_vFeeList:=vFeeList)
            m_vDeletedFeeIDs(m_vDeletedFeeIDs.GetUpperBound(0)) = lFeeAmountId

            lReturn = CType(PopulateUWFees(), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Delete Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return nResult
        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return nResult
    End Function
    ' ***************************************************************** '
    ' Name: DeleteTempDeleteFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function DeleteTempDeletedFee() As Integer

        Dim result As Integer = 0
        Dim llBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If m_bIsAnyDeletedFee And Information.IsArray(m_vDeletedFeeIDs) Then
                llBound = m_vDeletedFeeIDs.GetLowerBound(0)
                lUBound = m_vDeletedFeeIDs.GetUpperBound(0)

                For lRow As Integer = llBound To lUBound
                    'set property for the object call

                    m_oUFeeBusiness.FeeAmountID = m_vDeletedFeeIDs(lRow)

                    ' delete the specified fee amount

                    m_lReturn = m_oUFeeBusiness.Delete()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("DeleteTempDeletedFee", "Delete Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next lRow
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DeleteTempDeletedFee", r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ReloadUWPolicyFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function ReloadUWPolicyFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ReloadUWPolicyFees"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' reload the underwriting fees
            lReturn = LoadUnderwritingFees()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadUnderwritingFees Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = PopulateUWFees()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateUWFees Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' EditUWPolicyFee
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function EditUWPolicyFee() As Integer

        Return EditUWPolicyFee(v_bDeleteClick:=False, r_bConfirmDelete:=False)

    End Function
    ''' <summary>
    ''' EditUWPolicyFee
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <param name="r_bConfirmDelete"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function EditUWPolicyFee(ByVal v_bDeleteClick As Boolean, ByRef r_bConfirmDelete As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EditUWPolicyFee"

        Dim lReturn, lFeeAmountId As Integer
        Dim oListItem As ListViewItem
        Dim lTag As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected uw policy fee to edit
            lReturn = GetSelectedUWFee(lFeeAmountId)

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' create an instance of the underwriting policy fees maintenance component
                lReturn = CreateUWPolicyFeeMaint()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateUWPolicyFeeMaint Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' let the interface known this is for an edit action

                lReturn = m_oFee.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' let it know which policy fee item we want to edit

                m_oFee.FeeAmountID = lFeeAmountId
                m_sUniqueId = gPMFunctions.GetUniqueID()
                If m_lPartyTypeID = 9 Then
                    m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
                ElseIf m_lPartyTypeID = 10 Then
                    m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
                End If

                m_oFee.UniqueId = m_sUniqueId
                m_oFee.ScreenHierarchy = m_sScreenHeirarchy
                ' start the interface

                lReturn = m_oFee.Start(v_bDeleteClick, r_bConfirmDelete)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
                Else
                    ' if the user confirmed

                    If m_oFee.Status = gPMConstants.PMEReturnCode.PMOK Then
                        ' reload the underwriting fees and refresh the fees list view
                        lReturn = ReloadUWPolicyFees()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "ReloadUWPolicyFees Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddUWPolicyFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddUWPolicyFee() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddUWPolicyFee"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create an instance of the underwriting policy fees maintenance component
            lReturn = CreateUWPolicyFeeMaint()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateUWPolicyFeeMaint Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' let the interface known this is for an add action

            lReturn = m_oFee.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_sUniqueId = gPMFunctions.GetUniqueID()
            If m_lPartyTypeID = 9 Then
                m_sScreenHeirarchy = $"Fee Account({txtAccountCode.Text.Trim()})"
            ElseIf m_lPartyTypeID = 10 Then
                m_sScreenHeirarchy = $"Extra Account({txtAccountCode.Text.Trim()})"
            End If

            m_oFee.UniqueId = m_sUniqueId
            m_oFee.ScreenHierarchy = m_sScreenHeirarchy
            m_oFee.PartyCnt = m_lPartyCnt

            ' start the interface

            lReturn = m_oFee.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                ' if the user confirmed

                If m_oFee.Status = gPMConstants.PMEReturnCode.PMOK Then
                    ' reload the underwriting fees and refresh the fees list view
                    lReturn = ReloadUWPolicyFees()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ReloadUWPolicyFees Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateUWPolicyFeeMaint
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CreateUWPolicyFeeMaint() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateUWPolicyFeeMaint"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Create iFee if not already done so
            If m_oFee Is Nothing Then

                ' Get an instance of the Fee interface object via
                ' the public object manager.
                Dim temp_m_oFee As Object
                lReturn = g_oObjectManager.GetInstance(temp_m_oFee, "iPMBPartyFee.UInterface", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFee = temp_m_oFee

                ' Check for errors.
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInstance iPMBPartyFee.UInterface Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' GetSelectedUWFee
    ''' </summary>
    ''' <param name="r_lFeeAmountId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetSelectedUWFee(ByRef r_lFeeAmountId As Integer) As Integer

        Return GetSelectedUWFee(r_lFeeAmountId:=r_lFeeAmountId, v_vFeeList:=Nothing)

    End Function
    ''' <summary>
    ''' GetSelectedUWFee
    ''' </summary>
    ''' <param name="r_lFeeAmountId"></param>
    ''' <param name="v_vFeeList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetSelectedUWFee(ByRef r_lFeeAmountId As Integer, ByVal v_vFeeList As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedUWFee"

        Dim lReturn, lFeeAmountId As Integer
        Dim bItemSelected As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if there are any selected items
            For lItem As Integer = 1 To lvwFees.Items.Count
                If v_vFeeList Is Nothing Then
                    If lvwFees.Items.Item(lItem - 1).Selected Then

                        r_lFeeAmountId = Convert.ToString(lvwFees.Items.Item(lItem - 1).Tag)
                        bItemSelected = True
                        Exit For
                    End If
                Else
                    If v_vFeeList.Selected Then

                        r_lFeeAmountId = Convert.ToString(v_vFeeList.Tag)
                        bItemSelected = True
                        Exit For
                    End If
                End If
            Next

            If Not bItemSelected Then
                MessageBox.Show("No Fee has been selected. Select a fee item.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description: Returns additional party details from party record
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetPartyDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim vPartyDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetPartyDetails(v_lPartyCnt:=m_lPartyCnt, r_vResults:=vPartyDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetPartyDetails", "v_lPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vPartyDetails) Then


                m_sTaxNumber = CStr(vPartyDetails(kPartyDetailTaxNumber, 0))

                'changes as string ca not be typecasting not required
                'm_bDomiciledForTax = gPMFunctions.ToSafeBoolean(CBool(vPartyDetails(kPartyDetailDomiciledForTax, 0)), 0)
                m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailDomiciledForTax, 0))

                'm_bTaxExempt = gPMFunctions.ToSafeBoolean(CBool(vPartyDetails(kPartyDetailTaxExempt, 0)), 0)
                m_bTaxExempt = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailTaxExempt, 0))

                'm_dTaxPercentage = gPMFunctions.ToSafeDouble(CBool(vPartyDetails(kPartyDetailTaxPercentage, 0)), 0)
                m_dTaxPercentage = gPMFunctions.ToSafeDouble(vPartyDetails(kPartyDetailTaxPercentage, 0))

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here



            ' This is for debugging only



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function UpdatePartyDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPartyDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = CType(BuildPartyDetailArray(r_vPartyDetails:=vPartyDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BuildPartyDetailArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update party details

            lReturn = m_oBusiness.UpdatePartyDetails(v_lPartyCnt:=m_lPartyCnt, v_vPartyDetails:=vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: BuildPartyDetailArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function BuildPartyDetailArray(ByRef r_vPartyDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BuildPartyDetailArray"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim r_vPartyDetails(3, 0)

            If m_sTaxNumber <> "" Then

                r_vPartyDetails(kPartyDetailTaxNumber, 0) = m_sTaxNumber
            Else


                r_vPartyDetails(kPartyDetailTaxNumber, 0) = DBNull.Value
            End If

            If Not m_bDomiciledForTax Then

                r_vPartyDetails(kPartyDetailDomiciledForTax, 0) = 0
            Else

                r_vPartyDetails(kPartyDetailDomiciledForTax, 0) = 1
            End If

            If Not m_bTaxExempt Then

                r_vPartyDetails(kPartyDetailTaxExempt, 0) = 0
            Else

                r_vPartyDetails(kPartyDetailTaxExempt, 0) = 1
            End If

            If m_dTaxPercentage = 0 Then


                r_vPartyDetails(kPartyDetailTaxPercentage, 0) = DBNull.Value
            Else

                r_vPartyDetails(kPartyDetailTaxPercentage, 0) = m_dTaxPercentage
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function DeleteAddress(ByVal Value As Integer, ByVal vAddress As Object(,)) As Integer
        Dim nResult As Integer = 1
        Const kMethodName As String = "DeleteAddress"

        Try
            For i As Integer = 0 To vAddress.GetUpperBound(1)
                If Value = CInt(vAddress(0, i)) Then
                    nResult = 0
                    Exit For
                End If
            Next

            Return nResult
        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function

End Class
