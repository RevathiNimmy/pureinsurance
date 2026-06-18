Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/10/2000
    '
    ' Description: Main interface.
    '
    ' Edit History: TF171000 - Created
    ' ***************************************************************** '

    'Developer Guide No 7
    Private Const vbFormCode As Integer = 0
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


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

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyFP.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    'TN10072000 variable to determine system mode ie "Underwriting or Agency"
    Private m_sUnderwritingOrAgency As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sShortName As String = ""
    Private m_sName As String = ""
    Private m_lCurrencyId As Integer
    Private m_lSourceID As Integer
    Private m_lSubBranchID As Integer

    'For New Party Id for PN -40845
    'Developer Guide No 98
    Private m_vNewPartyId As Object

    Private m_lNumber As Integer
    Private m_sAgencyNumber As String = ""
    Private m_sMailboxNumber As String = ""
    Private m_iDOB As CheckState
    Private m_iCompanyReg As CheckState
    Private m_lPFEDIDefinitionID As Integer

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_sAddressLine1 As String = ""
    'Note the index in the lookup array of the main address
    Private m_sMainPostCode As String = ""
    Private m_iMainAddressIndex As Integer
    Private m_lAddressCount As Integer

    ' Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    ' Declare an instance of the contact interface.

    Private m_oContact As iPMBContact.Interface_Renamed

    Public IsNRMA As Boolean
    Private m_lPartyCnt As Integer
    Private m_bShowSubBranchID As Boolean

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double
    '**************************************************



    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ShortName() As String
        Get
            Return m_sShortName
        End Get
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


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)


    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
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

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

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
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            'Reference must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Number must be entered
            'PWF 25/07/2002 - Changed format to Long
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNumber, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Agency Number is optional
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Mailbox Number is optional
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMailbox, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNumber, vControlValue:=m_lNumber)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgencyNumber, vControlValue:=m_sAgencyNumber)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMailbox, vControlValue:=m_sMailboxNumber)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboPFEDIDefinition.ItemId = m_lPFEDIDefinitionID
            chkDOB.CheckState = m_iDOB
            chkCompanyReg.CheckState = m_iCompanyReg

            cboSource.ItemId = m_lSourceID

            cboCurrency.CompanyId = m_lSourceID
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = m_lCurrencyId

            'Reload the sub_branch list
            cboSubBranch.WhereClause = "source_id=" & m_lSourceID
            cboSubBranch.RefreshList()
            cboSubBranch.ItemId = m_lSubBranchID

            'Fill the contact grid
            PopulateContacts()

            'Fill the address grid
            PopulateAddresses()

            '*************************************************
            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage
            '*************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

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
    ' DD 24/07/2003: Changed to use Direct methods
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the task.
            Select Case (m_iTask)
                'SP090989
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.DirectAdd(r_lPartyCnt:=m_lPartyCnt, vFinanceProviderNumber:=m_lNumber, vAgencyNumber:=m_sAgencyNumber, vMailboxNumber:=m_sMailboxNumber, vShortName:=m_sShortName, vName:=m_sName, vCurrencyId:=m_lCurrencyId, vSourceID:=m_lSourceID, vSubBranchID:=m_lSubBranchID, vPFEDIDefinitionID:=m_lPFEDIDefinitionID, vDOB:=m_iDOB, vCompanyReg:=m_iCompanyReg)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.DirectEdit(lPartyCnt:=m_lPartyCnt, vFinanceProviderNumber:=m_lNumber, vAgencyNumber:=m_sAgencyNumber, vMailboxNumber:=m_sMailboxNumber, vShortName:=m_sShortName, vName:=m_sName, vCurrencyId:=m_lCurrencyId, vSourceID:=m_lSourceID, vSubBranchID:=m_lSubBranchID, vPFEDIDefinitionID:=m_lPFEDIDefinitionID, vDOB:=m_iDOB, vCompanyReg:=m_iCompanyReg, vPartyId:=m_vNewPartyId)

                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateAddresses
    '
    ' Description: Fills the grid control with address details
    '
    ' ***************************************************************** '
    Private Sub PopulateAddresses()

        Dim k As Integer
        Dim oLIstItem As ListViewItem


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

                oLIstItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim(), "")

                ' Assign details to other the columns
                ' Column 2
                For k = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If m_vAddresses(1, i).Equals(m_vAddressTypes(0, k)) Then
                        ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = CStr(m_vAddressTypes(1, k))
                        Exit For
                    End If
                Next k
                'See if this is the main address
                If CStr(m_vAddressTypes(2, k)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                    m_sMainPostCode = CStr(m_vAddresses(0, i))
                    m_iMainAddressIndex = CInt(m_vAddressTypes(0, k))
                End If


                ' Column 3
                ListViewHelper.GetListViewSubItem(oLIstItem, 2).Text = CStr(m_vAddresses(2, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oLIstItem, 3).Text = CStr(m_vAddresses(3, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oLIstItem, 4).Text = CStr(m_vAddresses(4, i)).Trim()

                ' Column 6
                ListViewHelper.GetListViewSubItem(oLIstItem, 5).Text = CStr(m_vAddresses(5, i)).Trim()

                ' Store the Address_cnt
                oLIstItem.Tag = CStr(m_vAddresses(6, i)).Trim()
                ' {* USER DEFINED CODE (End) *}
                m_lAddressCount += 1
                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception




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

        Dim oLIstItem As ListViewItem


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

                oLIstItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim(), "")

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = CStr(m_vContacts(2, i)).Trim()

                ' Column 3
                ListViewHelper.GetListViewSubItem(oLIstItem, 2).Text = CStr(m_vContacts(3, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oLIstItem, 3).Text = CStr(m_vContacts(4, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oLIstItem, 4).Text = CStr(m_vContacts(5, i)).Trim()


                ' Store the Contact_cnt
                oLIstItem.Tag = CStr(m_vContacts(0, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates mandatory address types and duplicate
    ' addresses
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Dim iMainAddresses As Integer
        Dim bDuplicate As Boolean
        Dim lAddressCnt As Integer
        Dim oLIstItem, oListItem2 As ListViewItem
        Dim sChangeCode, sPartyCode, sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check to see if client code already exists

            'If this is a new client or an existing one with it's client code changed then
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtIDReference.Text.Trim() <> m_sShortName.Trim()) Then

                sPartyCode = txtIDReference.Text.Trim()


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

            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'Add this just in case Apply is added at later stage
                m_sShortName = txtIDReference.Text
            End If

            'Display warning message that client code has changed.
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtIDReference.Text.Trim() <> m_sShortName.Trim() Then

                sChangeCode = CStr(MessageBox.Show("Warning! You are about to change the party code. Do you wish to change the code?", "Client Code Changed", MessageBoxButtons.YesNo))
                Select Case sChangeCode
                    Case CStr(System.Windows.Forms.DialogResult.No)
                        txtIDReference.Text = m_sShortName
                End Select

            End If

            iMainAddresses = 0

            'Count how many addresses are main address
            If lvwAddresses.Items.Count > 0 Then
                For i As Integer = 1 To lvwAddresses.Items.Count
                    oLIstItem = lvwAddresses.Items.Item(i - 1)
                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        If (ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                            iMainAddresses += 1
                        End If
                    Next j
                Next i
            End If

            Select Case iMainAddresses
                Case 0
                    'No
                    MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                Case 1
                    'Yes
                Case Else
                    'No.
                    MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            'Now Ensure addresses are not used twice
            If m_lAddressCount < 2 Then
                'less than 2 addresses so cant have duplicates
                Return result
            End If

            bDuplicate = False

            'Check for duplicates
            'Need to find out how to calculate no of lines in the grid
            For i As Integer = 1 To (m_lAddressCount)
                oLIstItem = lvwAddresses.Items.Item(i - 1)
                If CBool(CStr(Convert.ToString(oLIstItem.Tag) <> "").Trim()) Then
                    lAddressCnt = Convert.ToString(oLIstItem.Tag)
                    For j As Integer = (i + 1) To m_lAddressCount
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

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


        Try

            'Find the main address
            For i As Integer = 1 To lvwAddresses.Items.Count
                For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If (ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(i - 1), 1).Text = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
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



            oAddressBusiness.addresscnt = lAddressCnt


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




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodePropertiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vFinanceProviderNumber:=m_lNumber, vAgencyNumber:=m_sAgencyNumber, vMailboxNumber:=m_sMailboxNumber, vShortName:=m_sShortName, vName:=m_sName, vCurrencyId:=m_lCurrencyId, vSourceID:=m_lSourceID, vSubBranchID:=m_lSubBranchID, vPFEDIDefinitionID:=m_lPFEDIDefinitionID, vDOB:=m_iDOB, vCompanyReg:=m_iCompanyReg)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' returns additional party details
            ' these detail are not returned from get next because of parameter limit of 60
            m_lReturn = CType(GetPartyDetails(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(lPartyCnt:=m_lPartyCnt, vAddresses:=m_vAddresses)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get address type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vAddressTypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get contacts for the party

            m_lReturn = m_oBusiness.GetContactDetails(lPartyCnt:=m_lPartyCnt, vContacts:=m_vContacts)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
        Dim sMsg As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))

            m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))

            m_lNumber = CInt(m_oFormFields.UnformatControl(ctlControl:=txtNumber))

            m_sAgencyNumber = CStr(m_oFormFields.UnformatControl(ctlControl:=txtAgencyNumber))

            m_sMailboxNumber = CStr(m_oFormFields.UnformatControl(ctlControl:=txtMailbox))

            m_lCurrencyId = cboCurrency.CurrencyId
            'Changes made For PN-40845
            'DN 01/06/01 - Change PartyIDs to Longs
            If cboSource.ListIndex <> -1 Then
                If m_lSourceID <> cboSource.ItemData(cboSource.ListIndex) Then
                    m_vNewPartyId = 0
                End If
            Else
                'MKW180703 PN5425 Default to original party source id (if available)
            End If
            m_lSourceID = cboSource.ItemId
            m_lSubBranchID = cboSubBranch.ItemId
            m_iDOB = chkDOB.CheckState
            m_iCompanyReg = chkCompanyReg.CheckState
            m_lPFEDIDefinitionID = cboPFEDIDefinition.ItemId


            '***************************
            ' get the party tax details from the party tax control
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage
            '***************************
            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'Set up the address grid control columns and headers

            ' TF291298 - Disable menu options if New Client
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                mnuFinancial.Enabled = False
                mnuCommission.Enabled = False
                mnuNotes.Enabled = False
                mnuLetter.Enabled = False

                Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonFinancial - 1).Enabled = False
                '        Toolbar1.Buttons.Item(ACIButtonCommission).Enabled = False
                Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonPolicy - 1).Enabled = False
                Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonPolicy + 1).Enabled = False
                ' Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonLetter - 1).Enabled = False

            End If

            lblSubBranch.Visible = m_bShowSubBranchID
            cboSubBranch.Visible = m_bShowSubBranchID
            If Not m_bShowSubBranchID Then
                lblCurrency.Top = lblSource.Top
                cboCurrency.Top = cboSource.Top
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 2)

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
            'SP090998
            m_ctlTabFirstLast(ACControlStart, 0) = txtIDReference
            m_ctlTabFirstLast(ACControlEnd, 0) = cboCurrency

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If



            'Developer Guide No 243
            'Starts
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMailbox.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMailboxNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TN07072000-Tinny what hidden system option are we in?
            'Display "Treaty Number" for (Underwriting) and "AgencyNumber" for (Agency)


            lblAgencyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTreatyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Ends
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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

    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub cboSource_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSource.Click
        Dim iCurrencyID As Integer

        Try

            cboSubBranch.WhereClause = "source_id=" & cboSource.ItemId
            cboSubBranch.RefreshList()
            If Not m_bShowSubBranchID Then
                'when hidden choose default
                cboSubBranch.ListIndex = 0
            End If

            cboCurrency.CompanyId = cboSource.ItemId

            If cboSource.ItemId > 0 Then

                m_lReturn = m_oBusiness.GetBranchBaseCurrency(vSourceID:=cboSource.ItemId, iCurrencyID:=iCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Branch Base Currency", vApp:=ACApp, vClass:=ACClass, vMethod:="cboSource_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
                cboCurrency.DefaultCurrencyId = iCurrencyID
            End If

            cboCurrency.RefreshList()
            If cboCurrency.ListIndex = 0 Then cboCurrency.ListIndex = 1

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboSource_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cboSource_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Methods (End)

    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click

        Dim sTmp As String = ""
        Dim oLIstItem As ListViewItem


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

            m_oAddress.Reference = txtIDReference.Text
            'PSL 18/02/2003 Issue 2092 NRMA don't have postcodes

            m_oAddress.IsNRMA = IsNRMA
            m_lReturn = CType(m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.PostCode = m_sMainPostCode


            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()



            oLIstItem = lvwAddresses.Items.Add(m_oAddress.PostalCode, "")

            ' Assign details to other the columns
            ' Column 2

            ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = m_oAddress.AddressUsageType

            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
                'Caption = "Insurer: " & m_sShortName & " " & m_sMainPostCode
            End If

            ' Column 3

            ListViewHelper.GetListViewSubItem(oLIstItem, 2).Text = m_oAddress.Address1

            ' Column 4

            ListViewHelper.GetListViewSubItem(oLIstItem, 3).Text = m_oAddress.Address2

            ' Column 5

            ListViewHelper.GetListViewSubItem(oLIstItem, 4).Text = m_oAddress.Address3

            ' Column 6

            ListViewHelper.GetListViewSubItem(oLIstItem, 5).Text = m_oAddress.Address4

            ' Store the Address_cnt

            oLIstItem.Tag = m_oAddress.addresscnt

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try


    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click

        Dim oLIstItem As ListViewItem

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

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = m_oContact.ContactCnt


            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()



            oLIstItem = lvwContacts.Items.Add(m_oContact.AreaCode, "")

            ' Assign details to other the columns
            ' Column 2
            'Temporary thing

            ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oLIstItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oLIstItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oLIstItem, 4).Text = m_oContact.Description


            ' Store the Address_cnt

            oLIstItem.Tag = m_oContact.ContactCnt

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub


    Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click

        Dim oLIstItem As ListViewItem

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

            m_lReturn = CType(m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            oLIstItem = lvwAddresses.FocusedItem
            'set the address id


            m_oAddress.addresscnt = Convert.ToString(oLIstItem.Tag)

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeId = m_vAddressTypes(0, k)
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

            'Reset Interface
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False

            lvwAddresses.Items.RemoveAt(lvwAddresses.FocusedItem.Index)

            lvwAddresses.Focus()

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id


            m_oContact.ContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            lvwContacts.Items.RemoveAt(lvwContacts.FocusedItem.Index)

            lvwContacts.Focus()

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click

        Dim oLIstItem As ListViewItem
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

            m_lReturn = CType(m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtIDReference.Text
            'PSL 18/02/2003 Issue 2092 NRMA don't have postcodes

            m_oAddress.IsNRMA = IsNRMA

            m_oAddress.PostCode = m_sMainPostCode

            oLIstItem = lvwAddresses.FocusedItem
            'set the address id
            m_oAddress.AddressCnt = Convert.ToString(oLIstItem.Tag)

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeId = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k


            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False


            'Column 1

            oLIstItem.Text = m_oAddress.PostalCode

            ' Column 2

            ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = m_oAddress.AddressUsageType

            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
            End If
            ' Column 3

            ListViewHelper.GetListViewSubItem(oLIstItem, 2).Text = m_oAddress.Address1

            ' Column 4

            ListViewHelper.GetListViewSubItem(oLIstItem, 3).Text = m_oAddress.Address2

            ' Column 5

            ListViewHelper.GetListViewSubItem(oLIstItem, 4).Text = m_oAddress.Address3

            ' Column 6

            ListViewHelper.GetListViewSubItem(oLIstItem, 5).Text = m_oAddress.Address4

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click

        Dim oLIstItem As ListViewItem

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

            m_lReturn = CType(m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode

            'set the contact id
            oLIstItem = lvwContacts.FocusedItem



            m_oContact.ContactCnt = Convert.ToString(oLIstItem.Tag)



            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If


            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False


            oLIstItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oLIstItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oLIstItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oLIstItem, 4).Text = m_oContact.Description

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '

    'Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vAgentOnly As String = "") As Integer
    'Dim result As Integer = 0
    'Dim iSIRFindParty As Object
    '
    'Dim oFindParty, vKeyArray As Object
    '
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oFindParty = New iSIRFindParty.Interface()
    '
    'Set appropriate key if agent only


    'If (Not Information.IsNothing(vAgentOnly)) And (Not String.IsNullOrEmpty(vAgentOnly)) Then
    '
    ''ReDim vKeyArray(1, 0)

    'vKeyArray(0, 0) = gSIRLibrary.SIRNavKeyAgentOnly

    'vKeyArray(1, 0) = vAgentOnly
    '

    'm_lErrorNumber = oFindParty.SetKeys(vKeyArray)
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'm_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'oFindParty.CallingAppName = "iPMBPartyFP.Interface"
    '

    'm_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lErrorNumber = oFindParty.Start()
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
    '


    'vPartyCnt = oFindParty.PartyCnt


    'vShortName = oFindParty.ShortName

    'If Not Information.IsNothing(vName) Then


    'vName = oFindParty.LongName
    'End If
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lErrorNumber = oFindParty.Terminate()
    '
    'oFindParty = Nothing
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
        Dim oLIstItem As ListViewItem

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
            'Go thru addresses grid to get list of new addresses
            i = 1
            bFirst = True
            Do
                If i > lvwAddresses.Items.Count Then
                    Exit Do
                End If
                oLIstItem = lvwAddresses.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewAddresses(1, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewAddresses(1, i - 1)
                    End If



                    vNewAddresses(0, i - 1) = Convert.ToString(oLIstItem.Tag)

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text = CStr(m_vAddressTypes(1, j)) Then

                            vNewAddresses(1, i - 1) = m_vAddressTypes(0, j)
                            Exit For
                        End If
                    Next j

                End If
                i += 1
            Loop


            'Delete old address usages in database
            If (Information.IsArray(vOldAddresses)) And (Not Information.IsArray(vNewAddresses)) Then

                m_lReturn = m_oBusiness.UpdateAddresses(lPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                m_lReturn = m_oBusiness.UpdateAddresses(lPartyCnt:=m_lPartyCnt, vAddAddresses:=vNewAddresses)

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

                'update the database

                m_lReturn = m_oBusiness.UpdateAddresses(lPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses, vAddAddresses:=vNewAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddressesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oLIstItem As ListViewItem


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
                oLIstItem = lvwContacts.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oLIstItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewContacts(i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewContacts(i - 1)
                    End If



                    vNewContacts(i - 1) = Convert.ToString(oLIstItem.Tag)

                End If
                i += 1
            Loop


            'Delete old contact usages in database
            If (Information.IsArray(vOldContacts)) And (Not Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(lPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(lPartyCnt:=m_lPartyCnt, vAddContacts:=vNewContacts)

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

                m_lReturn = m_oBusiness.UpdateContacts(lPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts, vAddContacts:=vNewContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateOrion
    '
    ' Description: Apply party updates to Account in Orion
    '
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


            m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt)

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


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Giode No 51
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=ScreenHelpID)
    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                'The previous button has index 0 on tab 1, etc
                'TabMainTab.Tab = Index - 1
                SSTabHelper.SetSelectedIndex(tabMainTab, Index)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                'm_ctlTabFirstLast(ACControlStart, Index + 1).SetFocus
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch

            Exit Sub
        End Try


    End Sub
    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Dim vValue As Object

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFP.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPartyFP.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            'Broking doesn't show Sub Branch by default

            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vValue))

            iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)

            m_bShowSubBranchID = True

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

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

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            'Developer Guide no 220
            cboPFEDIDefinition.FirstItem = "(none)"
            cboPFEDIDefinition.ListIndex = 0
            cboSource.FirstItem = ""
            cboSource.ListIndex = 0

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            '    If (m_iTask% = PMAdd) Then
            'And if the insurer has no addresses and so hasn't set up the main address index
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iMainAddressIndex = 0) Then

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddresstypelookups(vAddressTypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                End If

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                        Exit For
                    End If

                Next i

                'No need to set the default company... (PN 14149)
                cboSource.ListIndex = -1

            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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
                'If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No 7
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

            ' Terminate the contact object (if used)
            If Not (m_oContact Is Nothing) Then


                m_oContact.Dispose()

                ' Destroy the instance of the contact object
                ' from memory.
                m_oContact = Nothing

            End If

            'Set the arrays to nothing
            m_vAddresses = Nothing
            m_vAddressTypes = Nothing
            m_vContacts = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

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
                mnuFind.Checked = False
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
                mnuFind.Checked = False
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
        Catch




            Exit Sub
        End Try


    End Sub


    Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If lvwAddresses.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False
        Else
            cmdEditAd.Enabled = True
            cmdDeleteAd.Enabled = True
        End If

    End Sub

    Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwContacts.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False
        Else
            cmdEditCon.Enabled = True
            cmdDeleteCon.Enabled = True
        End If


    End Sub



    ' ***************************************************************** '
    ' Name          : mnuFinancial_Click
    '
    ' Edit History  :
    ' RAM20020820   : ProcessToolbar is Modified to send the Button Key
    '                   rather than button index
    ' ***************************************************************** '
    Public Sub mnuFinancial_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFinancial.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SharedFiles.PMBToolbarFunc.ProcessToolbar(v_sButtonKey:=ACIButtonFinancialKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub
    ' ***************************************************************** '
    ' Name          : mnuLetter_Click
    '
    ' Edit History  :
    ' RAM20020820   : ProcessToolbar is Modified to send the Button Key
    '                   rather than button index
    ' ***************************************************************** '
    Public Sub mnuLetter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuLetter.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SharedFiles.PMBToolbarFunc.ProcessToolbar(v_sButtonKey:=ACIButtonLetterKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub
    ' ***************************************************************** '
    ' Name          : mnuNotes_Click
    '
    ' Edit History  :
    ' RAM20020820   : ProcessToolbar is Modified to send the Button Key
    '                   rather than button index
    ' ***************************************************************** '
    Public Sub mnuNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNotes.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(SharedFiles.PMBToolbarFunc.ProcessToolbar(v_sButtonKey:=ACIButtonNotesKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch

            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                Else
                    VB6.SetDefault(cmdOK, True)
                End If

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
        Dim nReturn As Integer
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Validate some address stuff
            m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' save additional details back to party record.
                m_lReturn = CType(UpdatePartyDetails(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'Update party addresses
                m_lReturn = CType(UpdateAddresses(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                ' eck 030501 - Update Orion
                m_lReturn = CType(UpdateOrion(vPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If


                'Update party contacts
                m_lReturn = CType(UpdateContacts(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()

                'DC260106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = CType(PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)

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
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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

    ' ***************************************************************** '
    ' Name          : Toolbar1_ButtonClick
    '
    ' Edit History  :
    ' RAM20020820   : 1. Changed the Button.Index to Button.Key
    '                 2. Added an optional parameter to pass the party_cnt
    ' ***************************************************************** '
    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles FINANCIAL.Click, COMMISSION.Click, _Toolbar1_Button3.Click, NOTE.Click, LETTER.Click, _Toolbar1_Button6.Click, EMAIL.Click, WEB.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        ' TF291298

        Try

            ' Call Toolbar Control function
            ' RAM20020820 : Added code to sent the Party Cnt

            m_lReturn = CType(SharedFiles.PMBToolbarFunc.ProcessToolbar(v_sButtonKey:=Button.Name, v_lPartyCnt:=m_lPartyCnt, v_ProductName:="iPMBPartyFP"), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch

            'Continue as not serious
            Exit Sub
        End Try

    End Sub



    Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtName)

    End Sub


    Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtName)

    End Sub


    Private Sub txtNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNumber.Enter 'PN 20040

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtNumber)

    End Sub

    Private Sub txtNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNumber.Leave 'PN 20040

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtNumber)

    End Sub

    ' PRIVATE Events (End)

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

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPartyDetails As Object



        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetPartyDetails(v_lPartyCnt:=m_lPartyCnt, r_vResults:=vPartyDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vPartyDetails) Then


                m_sTaxNumber = CStr(vPartyDetails(kPartyDetailTaxNumber, 0))

                m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(CStr(vPartyDetails(kPartyDetailDomiciledForTax, 0)), 0)

                m_bTaxExempt = gPMFunctions.ToSafeBoolean(CStr(vPartyDetails(kPartyDetailTaxExempt, 0)), 0)

                m_dTaxPercentage = gPMFunctions.ToSafeDouble(CStr(vPartyDetails(kPartyDetailTaxPercentage, 0)), 0)

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

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            tabMainTab.SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            tabMainTab.SelectedIndex = 3
        End If
    End Sub
End Class