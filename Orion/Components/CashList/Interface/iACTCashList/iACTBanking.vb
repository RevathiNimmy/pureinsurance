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
Partial Friend Class frmBanking
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmBanking
    '
    ' Date: 10th October 2002
    '
    ' Description: Banking interface. Created 10/10/2002 by Paul Harris (CMG)
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmBanking"

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
    'pkh 07/10/2002 - New variables to support Front Office Receipting
    Private m_lCashList_drawer_id As Integer
    Private m_lBatch_id As Integer
    Private m_iPMUser_id As Integer
    Private m_iConfirm_pmuser_id As Integer
    Private m_iConfirm2_pmuser_id As Integer
    Private m_dtDate_Approved As Date
    Private m_cBanking_Total As Decimal
    Private m_cCash_Float_Amount As Decimal
    'sw we forgot about this field
    Private m_dtDepositDate As Date

    'KG 16/06/03
    Private m_lSubBranchID As Integer


    'pkh 07/10/2002 - ends
    Private m_sTabsMediaType() As String ' Holds what Media type each Tab is
    Private m_vCurrencyDenom(,) As Object ' Holds the Currency Denominations
    Private iMaxCol As Integer ' Number of columns
    Private iMaxRow As Integer ' Number of rows
    Private vCashBreakDown(,,) As Object ' Array to store the data
    Private vFloatBreakDown(,,) As Object ' Array to store the data
    Private m_blnHasCash As Boolean ' Keep a track wether the form has any cash items
    Private m_blnUseFloat As Boolean ' Using the float or not
    Private m_blnFormSaved As Boolean ' Form already saved?
    Private m_blnFormApproved As Boolean ' Form has been approved
    Private o_frmAdjustment As frmAdjustment
    Private o_frmListAdjusts As frmListAdjusts
    'sw 09/12/2002
    Private m_bLoadingData As Boolean


    ' KG 19/09/03 - Should use a globally available const - but i can't find one...
    Private Const ACJournalBanking As String = "Banking"
    Private Const ACJournalAdjustments As String = "Adjustments"
    Private Const vbFormCode As Integer = 3



    Public Property SubBranchID() As Integer
        Get

            ' Return the SubBranch ID
            Return m_lSubBranchID

        End Get
        Set(ByVal Value As Integer)

            ' Set the SubBranch ID
            m_lSubBranchID = Value

        End Set
    End Property
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

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



    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

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
    '''Public Property Get CashListRef() As String
    '''    CashListRef = m_sCashListRef
    '''End Property
    '''

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

    '''Public Property Let CashlistTypeID(lCashListTypeID As Long)
    '''
    '''    ' Set the Cash List Type ID
    '''    m_lCashlistTypeID = lCashListTypeID
    '''
    '''End Property
    '''Public Property Get CashlistTypeID() As Long
    '''
    '''    ' Return the Cash List Type ID
    '''    CashlistTypeID = m_lCashlistTypeID
    '''
    '''End Property
    '''
    Public Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_iCurrencyID = Value

        End Set
    End Property
    ''''eck100500
    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the Company ID.
            m_iCompanyID = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    'sw added 23/12/2002, accept reference to business object from calling form
    Public WriteOnly Property Business() As Object
        Set(ByVal Value As Object)

            m_oBusiness = Value

        End Set
    End Property


    Public Property HasCashItems() As Boolean
        Get
            Return m_blnHasCash
        End Get
        Set(ByVal Value As Boolean)
            m_blnHasCash = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)


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


            m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID)

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
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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

            If m_iConfirm_pmuser_id > 0 Then 'there is a confirming user
                m_blnFormApproved = True
                'this data is already approved so it's either being looked at
                'as historical data or its about to get its second authorisation
                '        cboPMUserAuthorise1.SingleUserID = m_iConfirm_pmuser_id
                '        cboPMUserAuthorise1.DefaultUserID = m_iConfirm_pmuser_id
                '        cboPMUserAuthorise1.ListIndex = 1
                cboPMUserAuthorise1.UserID = m_iConfirm_pmuser_id
                'cboPMUserAuthorise1.ListIndex = 1




                cmdReverse.Enabled = False
                cmdAdjustment.Enabled = False

                'check if the first confirming user is the current user, if so disable the approve button
                If g_oObjectManager.UserID = m_iConfirm_pmuser_id Then
                    cmdApprove.Enabled = False
                End If

                'bring back previously saved data...
                LoadCash()

                'recalculate totals
                CalculateTotals()

                If m_iConfirm2_pmuser_id > 0 Then 'there is a confirming user
                    cboPMUserAuthorise2.UserID = m_iConfirm2_pmuser_id

                    cmdApprove.Enabled = False

                    txtDateApproved.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=m_dtDate_Approved)
                End If

            End If

            'Now load the Adjustments!!   if any...
            txtAdjustments.Text = CStr(0)

            m_lReturn = m_oBusiness.ListAdjustments(vAdjustments:=vResultArray, lCashList_ID:=m_lCashlistID)

            If Information.IsArray(vResultArray) Then

                For ii As Integer = 0 To vResultArray.GetUpperBound(1)

                    txtAdjustments.Text = CStr(CDec(txtAdjustments.Text) + CDec(vResultArray(3, ii)))
                Next ii
            End If

            SSTabHelper.SetSelectedIndex(tabMediaTypes, 0)

            If Information.IsDate(m_dtDepositDate) And m_dtDepositDate <> CDate("00:00:00") Then
                txtDepositDate.Text = m_dtDepositDate.ToString("dd/MM/yyyy")
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
                    '''            m_lReturn& = m_oBusiness.DirectAdd( _
                    ''''                                vCashListID:=m_lCashlistID, _
                    ''''                                vCashliststatusID:=m_lCashListStatusID, _
                    ''''                                vCashListTypeID:=m_lCashlistTypeID, _
                    ''''                                vCashlistRef:=m_sCashListRef, _
                    ''''                                vCompanyID:=m_iCompanyID, _
                    ''''                                vBankAccountID:=m_lBankAccountID, _
                    ''''                                vCurrencyID:=m_iCurrencyID, _
                    ''''                                vListDate:=m_dtListDate, _
                    ''''                                vControlTotal:=m_cControlTotal, _
                    ''''                                vItemCount:=m_lItemCount, _
                    ''''                                vCashlist_drawer_id:=m_lCashList_drawer_id, _
                    ''''                                vBatch_id:=m_lBatch_id, _
                    ''''                                vPMUser_id:=m_iPMUser_id, _
                    ''''                                vConfirm_PMUser_id:=m_iConfirm_pmuser_id, _
                    ''''                                vConfirm2_PMUser_id:=m_iConfirm2_pmuser_id, _
                    ''''                                vDate_Approved:=m_dtDate_Approved, _
                    ''''                                vBanking_Total:=m_cBanking_Total, _
                    ''''                                vCash_Float_Amount:=m_cCash_Float_Amount, _
                    ''''                                vDepositDate:=m_dtDepositDate)
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vCashListID:=m_lCashlistID, vCashliststatusID:=m_lCashListStatusID, vCashListTypeID:=m_lCashlistTypeID, vCashlistRef:=m_sCashListRef.Value, vCompanyID:=m_iCompanyID, vBankAccountID:=m_lBankAccountID, vCurrencyID:=m_iCurrencyID, vListDate:=m_dtListDate, vControlTotal:=m_cControlTotal, vItemCount:=m_lItemCount, vCashlist_drawer_id:=m_lCashList_drawer_id, vBatch_id:=m_lBatch_id, vPMUser_id:=m_iPMUser_id, vConfirm_PMUser_id:=m_iConfirm_pmuser_id, vConfirm2_PMUser_id:=m_iConfirm2_pmuser_id, vDate_Approved:=m_dtDate_Approved, vBanking_Total:=m_cBanking_Total, vCash_Float_Amount:=m_cCash_Float_Amount, vDepositDate:=m_dtDepositDate)

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


            ' Add the controls...

            '''    ' Reference
            '''    m_lReturn& = m_oFormfields.AddNewFormField( _
            ''''        ctlControl:=txtReference, _
            ''''        lFieldType:=PMString, _
            ''''        lFormat:=PMFormatString, _
            ''''        lMandatory:=PMNonMandatory)
            '''    If (m_lReturn& <> PMTrue) Then
            '''        SetFieldValidation = PMFalse
            '''        Exit Function
            '''    End If
            '''
            '''    ' Date
            '''    m_lReturn& = m_oFormfields.AddNewFormField( _
            ''''        ctlControl:=txtDate, _
            ''''        lFieldType:=PMDate, _
            ''''        lFormat:=PMFormatDateLong, _
            ''''        lMandatory:=PMMandatory)
            '''    If (m_lReturn& <> PMTrue) Then
            '''        SetFieldValidation = PMFalse
            '''        Exit Function
            '''    End If
            '''
            '''    ' Amounts total
            '''    m_lReturn& = m_oFormfields.AddNewFormField( _
            ''''        ctlControl:=txtTotalAmount, _
            ''''        lFieldType:=PMCurrency, _
            ''''        lFormat:=PMFormatMoney, _
            ''''        lMandatory:=PMNonMandatory)
            '''    If (m_lReturn& <> PMTrue) Then
            '''        SetFieldValidation = PMFalse
            '''        Exit Function
            '''    End If
            '''
            '''    ' Total Items
            '''    m_lReturn& = m_oFormfields.AddNewFormField( _
            ''''        ctlControl:=txtTotalItems, _
            ''''        lFieldType:=PMLong, _
            ''''        lFormat:=PMFormatLong, _
            ''''        lMandatory:=PMNonMandatory)
            '''    If (m_lReturn& <> PMTrue) Then
            '''        SetFieldValidation = PMFalse
            '''        Exit Function
            '''    End If



            Return gPMConstants.PMEReturnCode.PMTrue

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


            m_lReturn = m_oBusiness.GetNext(vCashListID:=m_lCashlistID, vCashliststatusID:=m_lCashListStatusID, vCashListTypeID:=m_lCashlistTypeID, vCashlistRef:=m_sCashListRef.Value, vCompanyID:=m_iCompanyID, vBankAccountID:=m_lBankAccountID, vCurrencyID:=m_iCurrencyID, vListDate:=m_dtListDate, vControlTotal:=m_cControlTotal, vItemCount:=m_lItemCount, vCashlist_drawer_id:=m_lCashList_drawer_id, vBatch_id:=m_lBatch_id, vPMUser_id:=m_iPMUser_id, vConfirm_PMUser_id:=m_iConfirm_pmuser_id, vConfirm2_PMUser_id:=m_iConfirm2_pmuser_id, vDate_Approved:=m_dtDate_Approved, vBanking_Total:=m_cBanking_Total, vCash_Float_Amount:=m_cCash_Float_Amount, vDepositDate:=m_dtDepositDate)

            ' Check for errors.
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                Case gPMConstants.PMEReturnCode.PMNotFound
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                    Return result
            End Select


            m_lReturn = m_oBusiness.CashFloat(m_blnUseFloat, m_lCashList_drawer_id)

            m_lReturn = GetBankingData()

            ' Check for errors.
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                Case gPMConstants.PMEReturnCode.PMNotFound
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                    Return result
            End Select

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

            m_iConfirm2_pmuser_id = cboPMUserAuthorise2.UserID

            If txtDateApproved.Text <> "" Then
                m_dtDate_Approved = CDate(txtDateApproved.Text)
            End If

            ' This use of CCur is temporary as it is locale specific
            Dim dbNumericTemp As Double
            If Double.TryParse(txtBankingTotal.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                m_cBanking_Total = CDec(txtBankingTotal.Text)
            Else
                m_cBanking_Total = 0
            End If

            'sw save the bank deposit date
            If Information.IsDate(txtDepositDate.Text) Then
                m_dtDepositDate = CDate(CDate(txtDepositDate.Text).ToString("dd/MM/yyyy"))
            End If


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

        '''Dim lCashListTypeID As Long

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            '    cboPMUserAuthorise1.SingleUserID = g_iUserID
            '    cboPMUserAuthorise1.DefaultUserID = g_iUserID
            cboPMUserAuthorise1.FirstItem = "Not Authorised"
            cboPMUserAuthorise1.Enabled = False

            '    cboPMUserAuthorise2.SingleUserID = g_iUserID
            '    cboPMUserAuthorise2.DefaultUserID = g_iUserID
            cboPMUserAuthorise2.FirstItem = "Not Authorised"
            cboPMUserAuthorise2.Enabled = False

            ' Make all the tabs invisible at first.
            For ii As Integer = ACFirstMediaTypeTab To ACLastMediaTypeTab
                SSTabHelper.SetTabVisible(tabMediaTypes, ii, False)
            Next ii

            ' Display all language specific captions.
            m_lReturn = iPMForms.DisplayCaptions(Me)
            '29/04/2003 - PWC - ENDVR00000848
            HasCashItems = False

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

            txtDepositDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=DateTime.Now)
            VB6.SetDefault(cmdOK, True)


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMView

                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Setup default data for Add

                    '''        txtTotalAmount.Text = FormatField( _
                    ''''                            iFormatType:=PMFormatCurrency, _
                    ''''                            vFieldValue:=0)
                    '''
                    '''        txtTotalItems.Text = FormatField( _
                    ''''                            iFormatType:=PMFormatInteger, _
                    ''''                            vFieldValue:=0)

            End Select

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
    ' Name: ValidateLookups
    '
    ' Description: Validate form controls (lookups)
    ' ***************************************************************** '
    Private Function ValidateLookups() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            '''    If uctCurrency.ListIndex = -1 Then
            '''        MsgBox "You must select a Currency", vbExclamation, Me.Caption
            '''        Exit Function
            '''    End If
            '''

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate form controls", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Sub cmdAdjustment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdjustment.Click

        o_frmAdjustment = New frmAdjustment()

        With o_frmAdjustment
            'Initialise the form


            'developer guide no.24
            .Business = m_oBusiness
            .General = m_oGeneral
            .m_lCashDrawerID = m_lCashList_drawer_id
            .m_lCashlistID = CashlistID
            .m_blnHasCash = m_blnHasCash
            .Task = gPMConstants.PMEComponentAction.PMAdd
            .ShowDialog()

            'DD 14/08/2003: Skip if the user clicks Cancel
            If .Status = gPMConstants.PMEReturnCode.PMOK Then
                txtAdjustments.Text = CStr(CDec(txtAdjustments.Text) + CDec(o_frmAdjustment.txtAmount.Text))
            End If

            'Release the memory
            o_frmAdjustment = Nothing

            Exit Sub

        End With

    End Sub



    Private Sub cmdApprove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApprove.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdApprove_Click
        ' PURPOSE: Approve the banking
        ' AUTHOR: Paul Harris
        ' DATE: 13 December 2002, 11:45:03
        ' CHANGES: 13/12/2002 - PWC - Wrap db writes in transaction and also post
        '                             EFTPos / CC total
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "cmdApprove_Click"

        Dim lCollectionAccountId, lBankAccountId, lAdjustmentAccountId, lSuspenseAccountId As Integer
        Dim vTransDetailIDs As Object
        Dim bTransactionStarted As Boolean

        Dim vMatchingBankingDebits, vMatchingCCDebits, vCreditTransDetailId As Object

        'SMJB CQ2621 22/09/03
        Dim vAdjustmentTransDetailId As Double
        Dim cBankingTotal, cBankingMismatch As Decimal


        Try

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            bTransactionStarted = False
            m_iTask = gPMConstants.PMEComponentAction.PMEdit

            If Not m_blnFormApproved Then
                With cboPMUserAuthorise1
                    .SingleUserID = g_iUserID
                    .DefaultUserID = g_iUserID
                    .UserID = g_iUserID
                End With

                'cboPMUserAuthorise1.ListIndex = 1
                m_iConfirm_pmuser_id = g_iUserID

                'It's OK to go to confirm tab so save the Cash and Float data...
                SaveCash()

                'set the m_lCashListStatusID = "B"

                m_lReturn = m_oBusiness.GetCashListStatus(vCashlistStatusCode:="B", lCashlistStatueID:=m_lCashListStatusID)
            Else
                'DD 30/09/2003: The same user cannot Approve and close the Banking
                If g_iUserID = cboPMUserAuthorise1.UserID Then
                    DisplayMessage(ACBankingNotAllowedTitle, ACBankingNotAllowedDetails, MsgBoxStyle.Critical, "")
                    Exit Sub
                End If

                With cboPMUserAuthorise2
                    .SingleUserID = g_iUserID
                    .DefaultUserID = g_iUserID
                    .UserID = g_iUserID
                End With

                'cant see why the listindex is being altered?? SW 06-12-2002
                'cboPMUserAuthorise2.ListIndex = 1

                m_iConfirm2_pmuser_id = g_iUserID

                'get the Bank Account IDs

                If m_oBusiness.GetBankAccounts(v_lCashListID:=m_lCashlistID, r_vCollectionAccount:=lCollectionAccountId, r_vBankAccount:=lBankAccountId, r_vAdjustmentAccount:=lAdjustmentAccountId, r_vSuspenseAccount:=lSuspenseAccountId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get bank accounts")
                End If

                bTransactionStarted = True

                'SMJB CQ2621 22/09/03: Post the adjustment transaction first, then we can use
                'the transdetail id to allocate discrepancies against banking total

                'SMJB CQ2621 22/09/03: Swapped accounts round, & negated adjustment value
                'Post the Adjustment transaction
                If Conversion.Val(txtAdjustments.Text) <> 0 Then

                    If m_oBusiness.ProcessBinder(v_lCreditAccountId:=lCollectionAccountId, v_lDebitAccountId:=lAdjustmentAccountId, v_cPayment:=CDec(txtAdjustments.Text) * -1, v_iCurrencyID:=m_iCurrencyID, r_vCreditTransDetailID:=vAdjustmentTransDetailId, v_lCashListID:=m_lCashlistID, v_lCompanyID:=m_iCompanyID, v_lSubBranchID:=Me.SubBranchID, v_vJournalType:=ACJournalAdjustments, v_vCashListDrawerID:=m_lCashList_drawer_id) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to post Adjustments")
                    End If
                End If

                'Post Banking total
                If Conversion.Val(txtBankingTotal.Text) <> 0 Then


                    If m_oBusiness.ProcessBinder(v_lCreditAccountId:=lCollectionAccountId, v_lDebitAccountId:=lBankAccountId, v_cPayment:=CDec(txtBankingTotal.Text), v_iCurrencyID:=m_iCurrencyID, r_vCreditTransDetailID:=vCreditTransDetailId, v_lCashListID:=m_lCashlistID, v_lCompanyID:=m_iCompanyID, v_lSubBranchID:=Me.SubBranchID, v_bSecondApprove:=True, v_vJournalType:=ACJournalBanking, v_vCashListDrawerID:=m_lCashList_drawer_id) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to post Banking total")
                    End If

                    'Get the debits to allocate the new credit posting against

                    If m_oBusiness.GetMatchingDebits(v_lCashListID:=Me.CashlistID, r_vMatchingDebits:=vMatchingBankingDebits, v_vIsBanking:=1) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get matching debits for banking")
                    End If

                    'SMJB : Now sum the total of debits in the banking array

                    For lCount As Integer = 0 To vMatchingBankingDebits.GetUpperBound(1)

                        cBankingTotal += CDec(vMatchingBankingDebits(1, lCount))
                    Next

                    'SMJB : Do we have a mismatch?
                    cBankingMismatch = CDec(txtBankingTotal.Text) - cBankingTotal

                    'SMJB : If yes then add an item to our allocation array for the amount of the mismatch
                    'to ensure the allocation balances
                    If cBankingMismatch <> 0 Then

                        ReDim Preserve vMatchingBankingDebits(2, vMatchingBankingDebits.GetUpperBound(1) + 1)


                        vMatchingBankingDebits(0, vMatchingBankingDebits.GetUpperBound(1)) = vAdjustmentTransDetailId


                        vMatchingBankingDebits(1, vMatchingBankingDebits.GetUpperBound(1)) = cBankingMismatch


                        vMatchingBankingDebits(2, vMatchingBankingDebits.GetUpperBound(1)) = cBankingMismatch
                    End If

                    'Allocate the total credit just posted against the original
                    'matching debits


                    If AllocateTotal(r_lAccountId:=lCollectionAccountId, r_lTransDetailId:=CInt(Conversion.Val(CStr(vCreditTransDetailId))), r_cAmount:=CDec(txtBankingTotal.Text), r_vTransDetails:=vMatchingBankingDebits) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to allocate banking total against matching debits")
                    End If
                End If

                'Post EFTPos / CC total
                If Conversion.Val(txtCCTotal.Text) <> 0 Then

                    If m_oBusiness.ProcessBinder(v_lCreditAccountId:=lCollectionAccountId, v_lDebitAccountId:=lSuspenseAccountId, v_cPayment:=CDec(txtCCTotal.Text), v_iCurrencyID:=m_iCurrencyID, r_vCreditTransDetailID:=vCreditTransDetailId, v_lCashListID:=m_lCashlistID, v_lCompanyID:=m_iCompanyID, v_lSubBranchID:=Me.SubBranchID, v_vJournalType:=ACJournalBanking, v_vCashListDrawerID:=m_lCashList_drawer_id) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to post EFTPos / CC total")
                    End If

                    'Get the debits to allocate the new credit posting against

                    If m_oBusiness.GetMatchingDebits(v_lCashListID:=Me.CashlistID, r_vMatchingDebits:=vMatchingCCDebits, v_vIsBanking:=0) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get matching debits for EFTPos / CC")
                    End If


                    'Allocate the total credit just posted against the original
                    'matching debits


                    If AllocateTotal(r_lAccountId:=lCollectionAccountId, r_lTransDetailId:=CInt(Conversion.Val(CStr(vCreditTransDetailId))), r_cAmount:=CDec(txtCCTotal.Text), r_vTransDetails:=vMatchingCCDebits) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to allocate banking total against matching debits")
                    End If
                End If

                'Select and display reports
                m_lReturn = DisplayCashListReports(r_lCashListId:=m_lCashlistID)

                'set the m_lCashListStatusID = "C"

                m_lReturn = m_oBusiness.GetCashListStatus(vCashlistStatusCode:="C", lCashlistStatueID:=m_lCashListStatusID)

                txtDateApproved.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=DateTime.Now)

            End If


            m_oGeneral.ProcessCommand()

            cmdApprove.Enabled = False
            cmdReverse.Enabled = False
            cmdAdjustment.Enabled = False
            m_blnFormApproved = True

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    Exit Sub

                Case Else
                    ' Log internal error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to approve banking", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub

            End Select

        Finally


        End Try
        Exit Sub
    End Sub
    ' PRIVATE Events (Begin)

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'developer guide no.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdReverse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReverse.Click
        Dim o As Object
        Dim sComponent As String = ""
        Dim vKeys As Object

        Try

            sComponent = "iACTCashListItem.Interface"

            'Create the component
            o = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(sComponent + "," + sComponent.Substring(0, sComponent.LastIndexOf(".")))).FullName, sComponent).Unwrap()

            With o

                CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                o.SetProcessModes(gPMConstants.PMEComponentAction.PMEdit, 0, 0, "", DateTime.Now)

                ReDim vKeys(1, 1)


                vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListId

                vKeys(1, 0) = m_lCashlistID

                vKeys(0, 1) = PMNavKeyConst.ACTKeyNameCashListTypeId

                vKeys(1, 1) = m_lCashlistTypeID


                o.SetKeys(vKeys)

                '        o.CashlistID = Val(txtCashListId.Text)

                o.Start()

                o.Dispose()
            End With

            o = Nothing

            'Now refresh the summary tab.  If a receipt has
            'been amended then the totals won't be right!!
            GetBusiness()
            BusinessToInterface()

        Catch


            o = Nothing
        End Try


    End Sub

    Private Sub cmdViewAdj_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewAdj.Click

        o_frmListAdjusts = New frmListAdjusts()

        With o_frmListAdjusts
            'Initialise the form
            '        .Business = m_oBusiness
            '        .General = m_oGeneral
            .m_lCashlistID = CashlistID
            '        .m_blnHasCash = True
            .Task = gPMConstants.PMEComponentAction.PMView
            .ShowDialog()

            '        txtAdjustments = Val(txtAdjustments) + Val(o_frmListAdjusts.txtAmount)
            'Release the memory
            o_frmListAdjusts = Nothing

            Exit Sub
        End With


    End Sub

    Private Sub frmBanking_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Static bMessageShown As Boolean

            'Flag a message if no items to process
            If Me.ErrorNumber = gPMConstants.PMEReturnCode.PMNotFound Then
                If Not bMessageShown Then
                    'Display standard message
                    DisplayMessage(ACNoBankingItemsTitle, ACNoBankingItemsDetails, MsgBoxStyle.Exclamation)

                    bMessageShown = True
                    Me.Hide()
                End If
            End If

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            'sw 23/12/2002, don't need this code below as references to business object is passed from frmList
            'PSL 28/02/2003 yes we do, becase it might be called by FindCashList
            'So I'll only do it if it doesn't exist

            '
            ' Get an instance of the business object via
            ' the public object manager.
            If m_oBusiness Is Nothing Then
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


    Private Sub frmBanking_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTCashList.General()

            '     Call the initialise method passing this interface
            '     and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


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
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMNotFound

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Me.tabMainTab.Enabled = False

                Else
                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If

                Exit Sub
            End If

            'PSL 04/03/2003 Disable these fields cos its view only and if you click on them
            'the form comes up and you can't exit
            'Its always view only, but if it is called from findcashlist you can't do adjustmens
            If m_sCallingAppName = "iACTFindCashList.Interface" Then
                cmdAdjustment.Enabled = False
                cmdReverse.Enabled = False
            End If

            If m_iConfirm_pmuser_id = 0 Or (m_iConfirm_pmuser_id <> 0 And m_iConfirm_pmuser_id <> g_oObjectManager.UserID And m_iConfirm2_pmuser_id = 0) Then
                're-enable the deposit date control as this is not read only
                txtDepositDate.Enabled = True
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

    Private Sub frmBanking_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'developer guide no.7
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
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

            'sw 23/12/2002, no longer need to do this as the business object is passed in from the listform

            ''    ' Terminate the business object
            ''    m_lReturn& = m_oBusiness.Terminate()
            ''
            ''    ' Check for errors.
            ''    If (m_lReturn& <> PMTrue) Then
            ''        m_lErrorNumber& = PMFalse
            ''
            ''        ' Log Error.
            ''        LogMessage _
            '''            iType:=PMLogError, _
            '''            sMsg:="Failed to terminate the business object", _
            '''            vApp:=ACApp, _
            '''            vClass:=ACClass, _
            '''            vMethod:="Form_QueryUnload"
            ''    End If
            ''
            ''    ' Destroy the instance of the business object
            ''    ' from memory.
            ''    Set m_oBusiness = Nothing

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

    Private Sub frmBanking_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    ''Private Sub fraFloat_Click(Index As Integer)
    ''If Index = 0 Then Stop
    ''End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            'Set as a case statement for readability and
            'if any more tabs are required later on.
            Select Case SSTabHelper.GetSelectedIndex(tabMainTab)
                Case 0 'Summary Tab
                    cmdApprove.Visible = False
                    cmdViewAdj.Visible = False
                    'cmdOK.Visible = True
                Case 1 'Confirm Tab
                    'check that it's alright to go to the Confirm Tab first
                    '(you don't want to save untill you know all is ok!)
                    If Not m_blnFormSaved Then
                        ''' needs to check if its been entered, in which case save, or
                        ''' a new bod is looking at it.  If it's just the user flicking back
                        ''' across the tabs then it shouldn't be saved.  Should only be saved once!!!
                        If m_blnUseFloat Then
                            For ii As Integer = ACFirstMediaTypeTab To m_sTabsMediaType.GetUpperBound(0)
                                If m_sTabsMediaType(ii).Trim() = "CASH" Then
                                    If txtFloatRem(ii).Text <> "0.00" Then
                                        MessageBox.Show("The Total Float does not match the Float Amount for this Cash Drawer", "Warning: Total Float mismatch", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        SSTabHelper.SetSelectedIndex(tabMainTab, tabMainTabPreviousTab)
                                        Exit Sub
                                    End If
                                End If
                            Next ii
                        End If
                    End If

                    cmdApprove.Visible = True
                    cmdViewAdj.Visible = True
                    'cmdOK.Visible = False

            End Select

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

            '
            '    Dim ctl As Control
            '
            '    For Each ctl In Me.Controls
            '        If ctl.Visible = True Then
            '            Debug.Print ctl.Name
            '        End If
            '    Next

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

    'UPGRADE_NOTE: (7001) The following declaration (txtReference_GotFocus) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtReference_GotFocus()
    '''    SelectText txtReference
    '''
    '''    m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtReference)
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (txtReference_LostFocus) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtReference_LostFocus()
    '
    '''    m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtReference)
    '
    'End Sub

    Private Sub tdgCash_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles _tdgCash_11.CellEndEdit, _tdgCash_15.CellEndEdit, _tdgCash_14.CellEndEdit, _tdgCash_12.CellEndEdit, _tdgCash_9.CellEndEdit, _tdgCash_8.CellEndEdit, _tdgCash_3.CellEndEdit, _tdgCash_2.CellEndEdit, _tdgCash_1.CellEndEdit, _tdgCash_0.CellEndEdit, _tdgCash_6.CellEndEdit, _tdgCash_13.CellEndEdit, _tdgCash_10.CellEndEdit, _tdgCash_7.CellEndEdit, _tdgCash_5.CellEndEdit, _tdgCash_4.CellEndEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim Index As Integer = Array.IndexOf(tdgCash, eventSender)
        'this allows the rows to be calculated if the user tabs of the control, etc.
        tdgCash(Index).UpdateCurrentRow()
    End Sub

    Private Sub tdgCash_CellBeginEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellCancelEventArgs) Handles _tdgCash_11.CellBeginEdit, _tdgCash_15.CellBeginEdit, _tdgCash_14.CellBeginEdit, _tdgCash_12.CellBeginEdit, _tdgCash_9.CellBeginEdit, _tdgCash_8.CellBeginEdit, _tdgCash_3.CellBeginEdit, _tdgCash_2.CellBeginEdit, _tdgCash_1.CellBeginEdit, _tdgCash_0.CellBeginEdit, _tdgCash_6.CellBeginEdit, _tdgCash_13.CellBeginEdit, _tdgCash_10.CellBeginEdit, _tdgCash_7.CellBeginEdit, _tdgCash_5.CellBeginEdit, _tdgCash_4.CellBeginEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim KeyAscii As Integer = 0
        Dim Cancel As Integer = 0
        Dim Index As Integer = Array.IndexOf(tdgCash, eventSender)

        If Convert.ToString(tdgCash(Index).Tag).Trim() = "BANK" Then
            Cancel = True 'Exit Sub
        End If

        If ColIndex = 2 Then
            Cancel = True
        Else
            If KeyAscii < 48 Or KeyAscii > 57 Then
                Cancel = True
            End If
        End If
        If Cancel <> 0 Then
            'TODO
            tdgCash(eventArgs.RowIndex).CancelEdit()
        End If
    End Sub

    'Private Sub tdgCash_UnboundGetRelativeBookmark(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid60.TrueDBGridEvents_UnboundGetRelativeBookmarkEvent) Handles _tdgCash_11.UnboundGetRelativeBookmark, _tdgCash_15.UnboundGetRelativeBookmark, _tdgCash_14.UnboundGetRelativeBookmark, _tdgCash_12.UnboundGetRelativeBookmark, _tdgCash_9.UnboundGetRelativeBookmark, _tdgCash_8.UnboundGetRelativeBookmark, _tdgCash_3.UnboundGetRelativeBookmark, _tdgCash_2.UnboundGetRelativeBookmark, _tdgCash_1.UnboundGetRelativeBookmark, _tdgCash_0.UnboundGetRelativeBookmark, _tdgCash_6.UnboundGetRelativeBookmark, _tdgCash_13.UnboundGetRelativeBookmark, _tdgCash_10.UnboundGetRelativeBookmark, _tdgCash_7.UnboundGetRelativeBookmark, _tdgCash_5.UnboundGetRelativeBookmark, _tdgCash_4.UnboundGetRelativeBookmark
    '    ' TDBGrid calls this routine each time it needs to
    '    ' reposition itself. StartLocation is a bookmark
    '    ' supplied by the grid to indicate the "current"
    '    ' position -- the row we are moving from. Offset is
    '    ' the number of rows we must move from StartLocation
    '    ' in order to arrive at the desired destination row.
    '    ' A positive offset means the desired record is after
    '    ' the StartLocation, and a negative offset means the
    '    ' desired record is before StartLocation.
    '    ' If StartLocation is NULL, then we are positioning
    '    ' from either BOF or EOF. Once we determine the
    '    ' correct index for StartLocation, we will simply add
    '    ' the offset to get the correct destination row.
    '    ' GetRelativeBookmark already does all of this, so we
    '    ' just call it here.


    '    eventArgs.NewLocation = GetRelativeBookmark(CStr(eventArgs.StartLocation), eventArgs.offset)
    '    ' If we are on a valid data row (i.e., not at BOF or
    '    ' EOF), then set the ApproximatePosition (the ordinal
    '    ' row number) to improve scroll bar accuracy. We can
    '    ' call IndexFromBookmark to do this.

    '    If Not (Convert.IsDBNull(eventArgs.NewLocation) Or IsNothing(eventArgs.NewLocation)) Then

    '        eventArgs.ApproximatePosition = IndexFromBookmark(CStr(eventArgs.NewLocation), 0)
    '    End If
    'End Sub


    Private Sub tdgCash_CellValueNeeded(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles _tdgCash_11.CellValueNeeded, _tdgCash_15.CellValueNeeded, _tdgCash_14.CellValueNeeded, _tdgCash_12.CellValueNeeded, _tdgCash_9.CellValueNeeded, _tdgCash_8.CellValueNeeded, _tdgCash_3.CellValueNeeded, _tdgCash_2.CellValueNeeded, _tdgCash_1.CellValueNeeded, _tdgCash_0.CellValueNeeded, _tdgCash_6.CellValueNeeded, _tdgCash_13.CellValueNeeded, _tdgCash_10.CellValueNeeded, _tdgCash_7.CellValueNeeded, _tdgCash_5.CellValueNeeded, _tdgCash_4.CellValueNeeded

        Dim ReadPriorRows As Boolean = False
        Dim Index As Integer = Array.IndexOf(tdgCash, eventSender)
        'TODO
        Dim RowBuf As DataGridViewRow = tdgCash(Index).Rows(eventArgs.RowIndex)
        'TODO
        Dim StartLocation As Object = RowBuf
        ' UnboundReadData is fired by an unbound grid whenever
        ' it requires data for display. This event will fire
        ' when the grid is first shown, when Refresh or ReBind
        ' is used, when the grid is scrolled, and after a
        ' record in the grid is modified and the user commits
        ' the change by moving off of the current row. The
        ' grid fetches data in "chunks", and the number of rows
        ' the grid is asking for is given by RowBuf.RowCount.
        ' RowBuf is the row buffer where you place the data and
        ' the bookmarks for the rows that the grid is requesting
        ' to display. It will also hold the number of rows that
        ' were successfully supplied to the grid.
        ' StartLocation is a bookmark which specifies the row
        ' before or after the desired data, depending on the
        ' value of ReadPriorRows. If we are reading rows after
        ' StartLocation (ReadPriorRows = False), then the first
        ' row of data the grid wants is the row after
        ' StartLocation, and if we are reading rows before
        ' StartLocation (ReadPriorRows = True) then the first
        ' row of data the grid wants is the row before
        ' StartLocation.
        ' ReadPriorRows is a boolean value indicating whether
        ' we are reading rows before (ReadPriorRows = True) or
        ' after (ReadPriorRows = False) StartLocation.
        Dim RelPos As Integer

        'Check that it's a Cash or Cheque Tab first.  If it's not, get out..
        If Convert.ToString(fraCash(Index).Tag).Trim() <> "CASH" And Convert.ToString(fraCash(Index).Tag).Trim() <> "BANK" Then
            Exit Sub
        End If

        ' Get a bookmark for the start location

        'developer guide no.33
        Dim Bookmark As Object = StartLocation

        If ReadPriorRows Then
            RelPos = -1 ' Requesting data in rows prior to
            ' StartLocation
        Else
            RelPos = 1 ' Requesting data in rows after
            ' StartLocation
        End If

        Dim RowsFetched As Integer = 0
        For i As Integer = 0 To 1 - 1
            ' Get the bookmark of the next available row
            Bookmark = GetRelativeBookmark(Bookmark, RelPos)

            ' If the next row is BOF or EOF, then stop
            ' fetching and return any rows fetched up to this
            ' point.

            If Convert.IsDBNull(Bookmark) Or IsNothing(Bookmark) Then Exit For

            ' Place the record data into the row buffer
            For j As Integer = 0 To RowBuf.Cells.Count - 1

                RowBuf.Cells(j).Value = GetCashData(Index, Bookmark, j)
            Next j

            ' Set the bookmark for the row


            RowBuf = Bookmark

            ' Increment the count of fetched rows
            RowsFetched += 1
        Next i

        ' Tell the grid how many rows we fetched

        'todolist
        'RowBuf.RowCount = RowsFetched
    End Sub


    Private Sub tdgCash_CellValuePushed(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles _tdgCash_11.CellValuePushed, _tdgCash_15.CellValuePushed, _tdgCash_14.CellValuePushed, _tdgCash_12.CellValuePushed, _tdgCash_9.CellValuePushed, _tdgCash_8.CellValuePushed, _tdgCash_3.CellValuePushed, _tdgCash_2.CellValuePushed, _tdgCash_1.CellValuePushed, _tdgCash_0.CellValuePushed, _tdgCash_6.CellValuePushed, _tdgCash_13.CellValuePushed, _tdgCash_10.CellValuePushed, _tdgCash_7.CellValuePushed, _tdgCash_5.CellValuePushed, _tdgCash_4.CellValuePushed

        Dim Index As Integer = Array.IndexOf(tdgCash, eventSender)
        'TODO
        Dim RowBuf As DataGridViewRow = tdgCash(Index).Rows(eventArgs.RowIndex)
        'TODO
        Dim WriteLocation As Object = RowBuf


        If Conversion.Val(CStr(RowBuf.Cells(1).FormattedValue)) = 0 Or StringsHelper.Format(Conversion.Val(CStr(RowBuf.Cells(1).FormattedValue)), "0") <> CStr(RowBuf.Cells(1).FormattedValue) Then


            vCashBreakDown(Index, 1, CInt(WriteLocation)) = ""


            vCashBreakDown(Index, 2, CInt(WriteLocation)) = ""
        Else



            vCashBreakDown(Index, 1, CInt(WriteLocation)) = RowBuf.Cells(1).FormattedValue



            vCashBreakDown(Index, 2, CInt(WriteLocation)) = StringsHelper.Format(Conversion.Val(CStr(RowBuf.Cells(1).FormattedValue)) * Conversion.Val(CStr(m_vCurrencyDenom(ACCurrDenomValue, CInt(WriteLocation)))), "0.00")
        End If

        'Total up the 'Cash'
        txtTotalCash(Index).Text = CStr(0)
        For ii As Integer = 0 To vCashBreakDown.GetUpperBound(2)

            txtTotalCash(Index).Text = StringsHelper.Format(Conversion.Val(txtTotalCash(Index).Text) + Conversion.Val(CStr(vCashBreakDown(Index, 2, ii))), "0.00")
        Next ii

    End Sub

    Private Sub tdgFloat_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles _tdgFloat_11.CellEndEdit, _tdgFloat_1.CellEndEdit, _tdgFloat_0.CellEndEdit, _tdgFloat_15.CellEndEdit, _tdgFloat_14.CellEndEdit, _tdgFloat_13.CellEndEdit, _tdgFloat_12.CellEndEdit, _tdgFloat_10.CellEndEdit, _tdgFloat_9.CellEndEdit, _tdgFloat_8.CellEndEdit, _tdgFloat_7.CellEndEdit, _tdgFloat_6.CellEndEdit, _tdgFloat_5.CellEndEdit, _tdgFloat_4.CellEndEdit, _tdgFloat_3.CellEndEdit, _tdgFloat_2.CellEndEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim Index As Integer = Array.IndexOf(tdgFloat, eventSender)
        'this allows the rows to be calculated if the user tabs of the control, etc.
        tdgFloat(Index).UpdateCurrentRow()
    End Sub

    Private Sub tdgFloat_CellBeginEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellCancelEventArgs) Handles _tdgFloat_11.CellBeginEdit, _tdgFloat_1.CellBeginEdit, _tdgFloat_0.CellBeginEdit, _tdgFloat_15.CellBeginEdit, _tdgFloat_14.CellBeginEdit, _tdgFloat_13.CellBeginEdit, _tdgFloat_12.CellBeginEdit, _tdgFloat_10.CellBeginEdit, _tdgFloat_9.CellBeginEdit, _tdgFloat_8.CellBeginEdit, _tdgFloat_7.CellBeginEdit, _tdgFloat_6.CellBeginEdit, _tdgFloat_5.CellBeginEdit, _tdgFloat_4.CellBeginEdit, _tdgFloat_3.CellBeginEdit, _tdgFloat_2.CellBeginEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim KeyAscii As Integer = 0
        Dim Cancel As Integer = 0
        If ColIndex = 2 Then
            Cancel = True
        Else
            If KeyAscii < 48 Or KeyAscii > 57 Then
                Cancel = True
            End If
        End If
        If Cancel <> 0 Then
            'TODO
            tdgFloat(eventArgs.RowIndex).CancelEdit()
        End If
    End Sub

    'Private Sub tdgFloat_UnboundGetRelativeBookmark(ByVal eventSender As Object, ByVal eventArgs As AxTrueDBGrid60.TrueDBGridEvents_UnboundGetRelativeBookmarkEvent) Handles _tdgFloat_11.UnboundGetRelativeBookmark, _tdgFloat_1.UnboundGetRelativeBookmark, _tdgFloat_0.UnboundGetRelativeBookmark, _tdgFloat_15.UnboundGetRelativeBookmark, _tdgFloat_14.UnboundGetRelativeBookmark, _tdgFloat_13.UnboundGetRelativeBookmark, _tdgFloat_12.UnboundGetRelativeBookmark, _tdgFloat_10.UnboundGetRelativeBookmark, _tdgFloat_9.UnboundGetRelativeBookmark, _tdgFloat_8.UnboundGetRelativeBookmark, _tdgFloat_7.UnboundGetRelativeBookmark, _tdgFloat_6.UnboundGetRelativeBookmark, _tdgFloat_5.UnboundGetRelativeBookmark, _tdgFloat_4.UnboundGetRelativeBookmark, _tdgFloat_3.UnboundGetRelativeBookmark, _tdgFloat_2.UnboundGetRelativeBookmark


    '	eventArgs.NewLocation = GetRelativeBookmark(CStr(eventArgs.StartLocation), eventArgs.offset)

    '	If Not (Convert.IsDBNull(eventArgs.NewLocation) Or IsNothing(eventArgs.NewLocation)) Then

    '		eventArgs.ApproximatePosition = IndexFromBookmark(CStr(eventArgs.NewLocation), 0)
    '	End If
    'End Sub


    Private Sub tdgFloat_CellValueNeeded(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles _tdgFloat_11.CellValueNeeded, _tdgFloat_1.CellValueNeeded, _tdgFloat_0.CellValueNeeded, _tdgFloat_15.CellValueNeeded, _tdgFloat_14.CellValueNeeded, _tdgFloat_13.CellValueNeeded, _tdgFloat_12.CellValueNeeded, _tdgFloat_10.CellValueNeeded, _tdgFloat_9.CellValueNeeded, _tdgFloat_8.CellValueNeeded, _tdgFloat_7.CellValueNeeded, _tdgFloat_6.CellValueNeeded, _tdgFloat_5.CellValueNeeded, _tdgFloat_4.CellValueNeeded, _tdgFloat_3.CellValueNeeded, _tdgFloat_2.CellValueNeeded

        Dim ReadPriorRows As Boolean = False
        Dim Index As Integer = Array.IndexOf(tdgFloat, eventSender)
        'TODO
        Dim RowBuf As DataGridViewRow = tdgFloat(Index).Rows(eventArgs.RowIndex)
        'TODO
        Dim StartLocation As Object = RowBuf
        Dim RelPos As Integer

        ' Get a bookmark for the start location

        'developer guide no.33
        Dim Bookmark As Object = StartLocation

        If ReadPriorRows Then
            RelPos = -1 ' Requesting data in rows prior to StartLocation
        Else
            RelPos = 1 ' Requesting data in rows after StartLocation
        End If

        Dim RowsFetched As Integer = 0
        For i As Integer = 0 To 1 - 1
            ' Get the bookmark of the next available row
            Bookmark = GetRelativeBookmark(Bookmark, RelPos)

            ' If the next row is BOF or EOF, then stop
            ' fetching and return any rows fetched up to this
            ' point.

            If Convert.IsDBNull(Bookmark) Or IsNothing(Bookmark) Then Exit For

            ' Place the record data into the row buffer
            For j As Integer = 0 To RowBuf.Cells.Count - 1

                RowBuf.Cells(j).Value = GetFloatData(Index, Bookmark, j)
            Next j

            ' Set the bookmark for the row


            RowBuf = Bookmark

            ' Increment the count of fetched rows
            RowsFetched += 1
        Next i

        ' Tell the grid how many rows we fetched

        'TODO
        'RowBuf.RowCount = RowsFetched
    End Sub


    Private Sub tdgFloat_CellValuePushed(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles _tdgFloat_11.CellValuePushed, _tdgFloat_1.CellValuePushed, _tdgFloat_0.CellValuePushed, _tdgFloat_15.CellValuePushed, _tdgFloat_14.CellValuePushed, _tdgFloat_13.CellValuePushed, _tdgFloat_12.CellValuePushed, _tdgFloat_10.CellValuePushed, _tdgFloat_9.CellValuePushed, _tdgFloat_8.CellValuePushed, _tdgFloat_7.CellValuePushed, _tdgFloat_6.CellValuePushed, _tdgFloat_5.CellValuePushed, _tdgFloat_4.CellValuePushed, _tdgFloat_3.CellValuePushed, _tdgFloat_2.CellValuePushed

        Dim Index As Integer = Array.IndexOf(tdgFloat, eventSender)
        'TODO
        Dim RowBuf As DataGridViewRow = tdgFloat(Index).Rows(eventArgs.RowIndex)
        'TODO
        Dim WriteLocation As Object = RowBuf


        If Conversion.Val(CStr(RowBuf.Cells(1).FormattedValue)) = 0 Or StringsHelper.Format(Conversion.Val(CStr(RowBuf.Cells(1).FormattedValue)), "0") <> CStr(RowBuf.Cells(1).FormattedValue) Then


            vFloatBreakDown(Index, 1, CInt(WriteLocation)) = ""


            vFloatBreakDown(Index, 2, CInt(WriteLocation)) = ""
        Else



            vFloatBreakDown(Index, 1, CInt(WriteLocation)) = RowBuf.Cells(1).FormattedValue



            vFloatBreakDown(Index, 2, CInt(WriteLocation)) = StringsHelper.Format(Conversion.Val(CStr(RowBuf.Cells(1).FormattedValue)) * Conversion.Val(CStr(m_vCurrencyDenom(ACCurrDenomValue, CInt(WriteLocation)))), "0.00")
        End If

        'Total up the 'Float'
        txtTotalFloat(Index).Text = CStr(0)
        For ii As Integer = 0 To vFloatBreakDown.GetUpperBound(2)


            txtTotalFloat(Index).Text = StringsHelper.Format(Conversion.Val(txtTotalFloat(Index).Text) + (IIf(CStr(vFloatBreakDown(Index, 2, ii)) = "", 0, CDec(vFloatBreakDown(Index, 2, ii)))), "0.00")
        Next ii

        'Keep a total of the 'Float Remaining'
        txtFloatRem(Index).Text = StringsHelper.Format(m_cCash_Float_Amount - CDec(txtTotalFloat(Index).Text), "0.00")
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAdjustments_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAdjustments.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Static blnChanging As Boolean

        'this will stop the change event running the code again when it's formatted
        If Not blnChanging Then
            blnChanging = True
            txtBalance.Text = CStr(Conversion.Val(txtSubTotal.Text) + Conversion.Val(txtAdjustments.Text))
            txtAdjustments.Text = StringsHelper.Format(txtAdjustments.Text, "0.00")
            blnChanging = False
        End If

    End Sub

    Private Sub txtBalance_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBalance.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        Static blnChanging As Boolean

        'this will stop the change event running the code again when it's formatted
        If Not blnChanging Then
            blnChanging = True

            txtBalance.Text = StringsHelper.Format(txtBalance.Text, "0.00")

            If txtBalance.Text <> "0.00" Then
                lblStatus.Text = "DRAWER DOES NOT BALANCE"
                cmdApprove.Enabled = False
            Else
                lblStatus.Text = ""
                'If (m_iConfirm2_pmuser_id < 1) And (m_iConfirm_pmuser_id <> g_oObjectManager.UserID) Then
                cmdApprove.Enabled = True
                'End If
            End If
            blnChanging = False
        End If

    End Sub

    Private Sub txtBankingTotal_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankingTotal.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtSubTotal.Text = StringsHelper.Format(Conversion.Val(txtTotalReceipts.Text) - Conversion.Val(txtBankingTotal.Text) - Conversion.Val(txtCCTotal.Text), "0.00")
    End Sub

    Private Sub txtCCTotal_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCCTotal.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtSubTotal.Text = StringsHelper.Format(Conversion.Val(txtTotalReceipts.Text) - Conversion.Val(txtBankingTotal.Text) - Conversion.Val(txtCCTotal.Text), "0.00")
    End Sub

    Private Sub txtDepositDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDepositDate.Leave

        If Not Information.IsDate(txtDepositDate.Text) Then
            MessageBox.Show("Date not recognised", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDepositDate.Text = DateTime.Today.ToString("dd/MM/yyyy")
            Exit Sub
        End If

        txtDepositDate.Text = CDate(txtDepositDate.Text).ToString("dd/MM/yyyy")

        m_dtDepositDate = CDate(CDate(txtDepositDate.Text).ToString("dd/MM/yyyy"))

    End Sub

    Private Sub txtFloat_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFloat.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtFloat.Text = StringsHelper.Format(txtFloat.Text, "0.00")
    End Sub

    Private Sub txtSubTotal_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSubTotal.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtBalance.Text = CStr(Conversion.Val(txtSubTotal.Text) + Conversion.Val(txtAdjustments.Text))
    End Sub

    Private Sub txtTotalCash_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtTotalCash_11.TextChanged, _txtTotalCash_15.TextChanged, _txtTotalCash_14.TextChanged, _txtTotalCash_12.TextChanged, _txtTotalCash_9.TextChanged, _txtTotalCash_8.TextChanged, _txtTotalCash_3.TextChanged, _txtTotalCash_2.TextChanged, _txtTotalCash_1.TextChanged, _txtTotalCash_0.TextChanged, _txtTotalCash_6.TextChanged, _txtTotalCash_13.TextChanged, _txtTotalCash_10.TextChanged, _txtTotalCash_7.TextChanged, _txtTotalCash_5.TextChanged, _txtTotalCash_4.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CalculateTotals()
    End Sub

    Private Sub txtTotalCC_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtTotalCC_15.TextChanged, _txtTotalCC_14.TextChanged, _txtTotalCC_13.TextChanged, _txtTotalCC_12.TextChanged, _txtTotalCC_11.TextChanged, _txtTotalCC_10.TextChanged, _txtTotalCC_9.TextChanged, _txtTotalCC_8.TextChanged, _txtTotalCC_7.TextChanged, _txtTotalCC_6.TextChanged, _txtTotalCC_5.TextChanged, _txtTotalCC_3.TextChanged, _txtTotalCC_2.TextChanged, _txtTotalCC_1.TextChanged, _txtTotalCC_0.TextChanged, _txtTotalCC_4.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CalculateTotals()
    End Sub

    Private Sub txtTotalFloat_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtTotalFloat_11.TextChanged, _txtTotalFloat_1.TextChanged, _txtTotalFloat_0.TextChanged, _txtTotalFloat_15.TextChanged, _txtTotalFloat_14.TextChanged, _txtTotalFloat_13.TextChanged, _txtTotalFloat_12.TextChanged, _txtTotalFloat_10.TextChanged, _txtTotalFloat_9.TextChanged, _txtTotalFloat_8.TextChanged, _txtTotalFloat_7.TextChanged, _txtTotalFloat_6.TextChanged, _txtTotalFloat_5.TextChanged, _txtTotalFloat_4.TextChanged, _txtTotalFloat_3.TextChanged, _txtTotalFloat_2.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CalculateTotals()
    End Sub

    Private Sub txtTotalReceipts_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalReceipts.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtSubTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=Conversion.Val(txtTotalReceipts.Text) - Conversion.Val(txtBankingTotal.Text) - Conversion.Val(txtCCTotal.Text))
    End Sub

    ' PRIVATE Events (End)


    ' ***************************************************************** '
    ' Name: PopulateCurDenom
    '
    ' Description: Retrieves the Currency Denominations
    '
    ' ***************************************************************** '
    Private Function PopulateCurDenom(ByRef intTabNo As Integer) As Integer
        Dim result As Integer = 0

        '***************************************************

        Try

            result = True

            If iMaxRow < m_vCurrencyDenom.GetUpperBound(1) + 1 Then
                iMaxRow = m_vCurrencyDenom.GetUpperBound(1) + 1
                vCashBreakDown = ArraysHelper.RedimPreserve(Of Object(,,))(vCashBreakDown, New Integer() {ACLastMediaTypeTab - ACFirstMediaTypeTab + 1, iMaxCol, iMaxRow}, New Integer() {ACFirstMediaTypeTab, 0, 0})
                vFloatBreakDown = ArraysHelper.RedimPreserve(Of Object(,,))(vFloatBreakDown, New Integer() {ACLastMediaTypeTab - ACFirstMediaTypeTab + 1, iMaxCol, iMaxRow}, New Integer() {ACFirstMediaTypeTab, 0, 0})
            End If

            FormatCashGrid(intTabNo)
            FormatFloatGrid(intTabNo)

            ' Assign the details to the interface.
            For ii As Integer = m_vCurrencyDenom.GetLowerBound(1) To m_vCurrencyDenom.GetUpperBound(1)

                vCashBreakDown(intTabNo, 0, ii) = CStr(m_vCurrencyDenom(ACCurrDenomDescription, ii)).Trim()

                vFloatBreakDown(intTabNo, 0, ii) = CStr(m_vCurrencyDenom(ACCurrDenomDescription, ii)).Trim()
            Next ii

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Currency Denominations", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCurDenom", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetBankingData() As Integer

        Dim result As Integer = 0
        Dim vBankingData(,) As Object
        Dim iCurrentTab As Integer
        Dim lLastTab As Integer
        Dim iCurrentCheques As Integer
        Dim cTotalReceipts As Decimal
        'Dim cTotalBanking As Currency

        Const ACMediaTypeID As Integer = 1
        Const ACMediaDescription As Integer = 2
        Const ACValidationCode As Integer = 4
        'Const ACValidationCodeDescription = 3
        Const ACPaymentName As Integer = 6
        Const ACAmount As Integer = 7
        Const ACBankName As Integer = 9
        Const ACIsBanking As Integer = 10

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bLoadingData = True

            'Get the Currency Denominations ready

            m_lReturn = m_oBusiness.GetCurrencyDenom(m_vCurrencyDenom, m_iCurrencyID)

            'should raise an error if no denominations!!!
            If Not Information.IsArray(m_vCurrencyDenom) Then
                Return False
            End If


            m_lReturn = m_oBusiness.GetBankingItems(vBankingData, CashlistID)

            'should raise an error if no items!!!
            If Not Information.IsArray(vBankingData) Then
                'GetBankingData = False
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            iMaxCol = 3
            iMaxRow = 1
            vCashBreakDown = Array.CreateInstance(GetType(Object), New Integer() {ACLastMediaTypeTab - ACFirstMediaTypeTab + 1, iMaxCol, iMaxRow}, New Integer() {ACFirstMediaTypeTab, 0, 0})
            vFloatBreakDown = Array.CreateInstance(GetType(Object), New Integer() {ACLastMediaTypeTab - ACFirstMediaTypeTab + 1, iMaxCol, iMaxRow}, New Integer() {ACFirstMediaTypeTab, 0, 0})

            '29/04/2003 - PWC - ENDVR00000848
            'Ensure that if the CashDrawer has a float then it appears on the CASH
            'tab regardless if there is any CASH
            iCurrentTab = ACFirstMediaTypeTab
            lLastTab = ACFirstMediaTypeTab
            cTotalReceipts = 0
            ' Assign the details to the interface.

            For ii As Integer = vBankingData.GetLowerBound(1) To vBankingData.GetUpperBound(1)

                cTotalReceipts += Conversion.Val(CStr(vBankingData(ACAmount, ii)))
                '       cTotalBanking = cTotalBanking + Val(IIf(vBankingData(ACIsBanking, ii) = PMTrue, vBankingData(ACAmount, ii), 0))


                If Conversion.Val(CStr(vBankingData(ACMediaTypeID, ii))) <> lLastTab Then

                    SSTabHelper.SetTabCaption(tabMediaTypes, iCurrentTab, CStr(vBankingData(ACMediaDescription, ii)).Trim())


                    lLastTab = CInt(Conversion.Val(CStr(vBankingData(ACMediaTypeID, ii))))

                    ReDim Preserve m_sTabsMediaType(iCurrentTab)

                    m_sTabsMediaType(iCurrentTab) = CStr(vBankingData(ACValidationCode, ii)).Trim()


                    ' at this point we have created a new tab for the media ID

                    'now we must decide which format the tab will take dependant on the media val code



                    Select Case CStr(vBankingData(ACValidationCode, ii)).Trim()
                        Case "CASH"
                            'It's a CASH Item
                            HasCashItems = True
                            m_lReturn = PopulateCurDenom(iCurrentTab)
                            If m_blnUseFloat Then
                                '''                if float?? [cashdrawer.cash_float=1] then
                                'with float
                                fraCash(iCurrentTab).Left = VB6.TwipsToPixelsX(4320)
                                fraFloat(iCurrentTab).Visible = True
                            Else
                                'without float
                                fraCash(iCurrentTab).Left = VB6.TwipsToPixelsX(2280)
                            End If
                            txtFloatRem(iCurrentTab).Text = StringsHelper.Format(m_cCash_Float_Amount, "0.00")


                            txtTotalCash(iCurrentTab).Tag = CStr(vBankingData(ACIsBanking, ii))
                            fraCash(iCurrentTab).Visible = True


                        Case "BANK"
                            'It's a Bank/Cheque Item
                            iCurrentCheques = 1
                            FormatCheque(iCurrentTab)


                            vCashBreakDown(iCurrentTab, 0, 0) = CStr(vBankingData(ACPaymentName, ii)).Trim()
                            'Bank name


                            vCashBreakDown(iCurrentTab, 1, 0) = CStr(vBankingData(ACBankName, ii)).Trim()
                            'Amount


                            vCashBreakDown(iCurrentTab, 2, 0) = StringsHelper.Format(vBankingData(ACAmount, ii), "0.00")
                            tdgCash(iCurrentTab).RowsCount = 1

                            'Position the Frame containing the Cheque details
                            With fraCash(iCurrentTab)
                                .Width = VB6.TwipsToPixelsX(5415)
                                .Text = "Cheque Breakdown"
                                .Visible = True
                            End With

                            txtTotalCash(iCurrentTab).Text = StringsHelper.Format(vBankingData(ACAmount, ii), "0.00")


                            txtTotalCash(iCurrentTab).Tag = CStr(vBankingData(ACIsBanking, ii))

                            tdgCash(iCurrentTab).Refresh()

                        Case "CC", "BASIC"
                            'It's a Credit Card Item
                            txtTotalCC(iCurrentTab).Visible = True
                            lblTotalCC(iCurrentTab).Visible = True
                            'Add Amount

                            txtTotalCC(iCurrentTab).Text = StringsHelper.Format(Conversion.Val(CStr(vBankingData(ACAmount, ii))), "0.00")


                            txtTotalCC(iCurrentTab).Tag = CStr(vBankingData(ACIsBanking, ii))


                    End Select


                    fraCash(iCurrentTab).Tag = CStr(vBankingData(ACValidationCode, ii)).Trim()

                    SSTabHelper.SetTabVisible(tabMediaTypes, iCurrentTab, True)

                    iCurrentTab += 1
                Else
                    'It's still for the same tab...

                    Select Case CStr(vBankingData(ACValidationCode, ii)).Trim().ToUpper()
                        Case "CASH"
                            'It's a CASH Item - don't need to do anything...
                        Case "BANK"
                            'It's a Bank/Cheque Item
                            If iMaxRow < iCurrentCheques + 1 Then
                                iMaxRow = iCurrentCheques + 1
                                vCashBreakDown = ArraysHelper.RedimPreserve(Of Object(,,))(vCashBreakDown, New Integer() {ACLastMediaTypeTab - ACFirstMediaTypeTab + 1, iMaxCol, iMaxRow}, New Integer() {ACFirstMediaTypeTab, 0, 0})
                            End If



                            vCashBreakDown(iCurrentTab - 1, 0, iCurrentCheques) = CStr(vBankingData(ACPaymentName, ii)).Trim()
                            'Bank name


                            vCashBreakDown(iCurrentTab - 1, 1, iCurrentCheques) = CStr(vBankingData(ACBankName, ii)).Trim()
                            'Amount


                            vCashBreakDown(iCurrentTab - 1, 2, iCurrentCheques) = StringsHelper.Format(vBankingData(ACAmount, ii), "0.00")

                            iCurrentCheques += 1
                            tdgCash(iCurrentTab - 1).RowsCount = iCurrentCheques

                            txtTotalCash(iCurrentTab - 1).Text = StringsHelper.Format(Conversion.Val(txtTotalCash(iCurrentTab - 1).Text) + Conversion.Val(CStr(vBankingData(ACAmount, ii))), "0.00")
                            tdgCash(iCurrentTab - 1).Refresh()
                        Case "CC", "BASIC"
                            'It's a Credit Card Item

                            txtTotalCC(iCurrentTab - 1).Text = StringsHelper.Format(Conversion.Val(txtTotalCC(iCurrentTab - 1).Text) + Conversion.Val(CStr(vBankingData(ACAmount, ii))), "0.00")
                    End Select

                End If
            Next ii

            '29/04/2003 - PWC - ENDVR00000848
            If Not m_blnHasCash And m_blnUseFloat Then
                iCurrentTab += 1
                SSTabHelper.SetTabCaption(tabMediaTypes, iCurrentTab, "Cash")

                'lLastTab = Val(vBankingData(ACMediaTypeID, ii))

                ReDim Preserve m_sTabsMediaType(iCurrentTab)
                m_sTabsMediaType(iCurrentTab) = "CASH"
                fraCash(iCurrentTab).Tag = m_sTabsMediaType(iCurrentTab)
                SSTabHelper.SetTabVisible(tabMediaTypes, iCurrentTab, True)

                m_lReturn = PopulateCurDenom(iCurrentTab)

                fraCash(iCurrentTab).Visible = False
                fraFloat(iCurrentTab).Visible = True

                txtFloatRem(iCurrentTab).Text = StringsHelper.Format(m_cCash_Float_Amount, "0.00")
                'txtTotalCash(iCurrentTab).Tag = vBankingData(ACIsBanking, ii)

            End If

            m_bLoadingData = False

            CalculateTotals()

            txtTotalReceipts.Text = StringsHelper.Format(cTotalReceipts, "0.00")
            '    txtBankingTotal = Format$(cTotalBanking, "0.00")


            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Banking Data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankingData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub CalculateTotals()
        Dim dTotalCC, dBankingTotal, dTotalFloats As Double

        'sw added to stop this code running over and over when form first loaded
        If m_bLoadingData Then Exit Sub

        'Remember, Total receipts will come from the database (ie. all the records)
        'Banking Total and EFTPos/CC Total are taken from the summary form.
        'The Sub-Total field is the Total Receipts (DB) - banking - CC.
        'ie. To be correct the Sub-Total field should be 0 (Zero)!!
        For ii As Integer = ACFirstMediaTypeTab To ACLastMediaTypeTab
            dBankingTotal += (IIf(Conversion.Val(Convert.ToString(txtTotalCash(ii).Tag)) = gPMConstants.PMEReturnCode.PMTrue, Conversion.Val(txtTotalCash(ii).Text), 0))
            dBankingTotal += (IIf(Conversion.Val(Convert.ToString(txtTotalCC(ii).Tag)) = gPMConstants.PMEReturnCode.PMTrue, Conversion.Val(txtTotalCC(ii).Text), 0))
            dTotalFloats += Conversion.Val(txtTotalFloat(ii).Text)
            dTotalCC += (IIf(Conversion.Val(Convert.ToString(txtTotalCC(ii).Tag)) = gPMConstants.PMEReturnCode.PMFalse, Conversion.Val(txtTotalCC(ii).Text), 0))
        Next ii

        txtCCTotal.Text = StringsHelper.Format(dTotalCC, "0.00")
        txtBankingTotal.Text = StringsHelper.Format(dBankingTotal, "0.00")
        txtFloat.Text = StringsHelper.Format(dTotalFloats, "0.00")

    End Sub

    Private Function MakeBookmark(ByRef Index As Integer) As String
        ' This support function is used only by the remaining
        ' support functions. It is not used directly by the
        ' unbound events. It is a good idea to create a
        ' MakeBookmark function such that all bookmarks can be
        ' created in the same way. Thus the method by which
        ' bookmarks are created is consistent and easy to
        ' modify. This function creates a bookmark when given
        ' an array row index.
        ' Since we have data stored in an array, we will just
        ' use the array index as our bookmark. We will convert
        ' it to a string first, using the CStr function.
        Return CStr(Index)
    End Function

    Private Function GetCashData(ByRef TabIndex_Renamed As Integer, ByRef Bookmark As String, ByRef Col As Integer) As String
        ' In this example, GetCashData is called by
        ' UnboundReadData to ask the user what data should be
        ' displayed in a specific cell in the grid. The grid
        ' row the cell is in is the one referred to by the
        ' Bookmark parameter, and the column it is in it given
        ' by the Col parameter. GetCashData is called on a
        ' cell-by-cell basis.
        ' Figure out which row the bookmark refers to
        Dim Index As Integer = IndexFromBookmark(Bookmark, False)

        If Index < 0 Or Index >= iMaxRow Or Col < 0 Or Col >= iMaxCol Then
            ' Cell position is invalid, so just return null
            ' to indicate failure

            Return Nothing
        Else

            Return CStr(vCashBreakDown(TabIndex_Renamed, Col, Index))
        End If
    End Function

    Private Function GetFloatData(ByRef TabIndex_Renamed As Integer, ByRef Bookmark As String, ByRef Col As Integer) As String
        ' Figure out which row the bookmark refers to
        Dim Index As Integer = IndexFromBookmark(Bookmark, False)

        If Index < 0 Or Index >= iMaxRow Or Col < 0 Or Col >= iMaxCol Then
            ' Cell position is invalid, so just return null
            ' to indicate failure

            Return Nothing
        Else

            Return CStr(vFloatBreakDown(TabIndex_Renamed, Col, Index))
        End If
    End Function

    Private Function GetRelativeBookmark(ByRef Bookmark As String, ByRef RelPos As Integer) As String
        ' GetRelativeBookmark is used to get a bookmark for a
        ' row that is a given number of rows away from the given
        ' row. This specific example will always use either -1
        ' or +1 for a relative position, since we will always be
        ' retrieving either the row previous to the current one,
        ' or the row following the current one.
        ' IndexFromBookmark expects a Bookmark and a Boolean
        ' value: this Boolean value is True if the next row to
        ' read is before the current one [in this case,
        ' (RelPos < 0) is True], or False if the next row to
        ' read is after the current one [(RelPos < 0) is False].
        ' This is necessary to distinguish between BOF and EOF
        ' in the IndexFromBookmark function if our bookmark is
        ' Null. Once we get the correct row index from
        ' IndexFromBookmark, we simply add RelPos to it to get
        ' the desired row index and create a bookmark using
        ' that index.

        Dim Index As Integer = IndexFromBookmark(Bookmark, RelPos < 0) + RelPos
        If Index < 0 Or Index >= iMaxRow Then
            ' Index refers to a row before the first or after
            ' the last, so just return Null.

            Return Nothing
        Else
            Return MakeBookmark(Index)
        End If
    End Function

    Private Function IndexFromBookmark(ByRef Bookmark As String, ByRef ReadPriorRows As Boolean) As Integer
        ' This support function is used only by the remaining
        ' support functions. It is not used directly by the
        ' unbound events.

        ' This function is the inverse of MakeBookmark. Given
        ' a bookmark, IndexFromBookmark returns the row index
        ' that the given bookmark refers to. If the given
        ' bookmark is Null, then it refers to BOF or EOF. In
        ' such a case, we need to use ReadPriorRows to
        ' distinguish between the two. If ReadPriorRows = True,
        ' the grid is requesting rows before the current
        ' location, so we must be at EOF, because no rows exist
        ' before BOF. Conversely, if ReadPriorRows = False,
        ' we must be at BOF.

        Dim Index As Integer


        If Convert.IsDBNull(Bookmark) Or IsNothing(Bookmark) Then
            If ReadPriorRows Then
                ' Bookmark refers to EOF. Since (MaxRow - 1)
                ' is the index of the last record, we can use
                ' an index of (MaxRow) to represent EOF.
                Return iMaxRow
            Else
                ' Bookmark refers to BOF. Since 0 is the
                ' index of the first record, we can use an
                ' index of -1 to represent BOF.
                Return -1
            End If
        Else
            ' Convert string to long integer
            Index = CInt(Conversion.Val(Bookmark))

            ' Check to see if the row index is valid:
            '  (0 <= Index < MaxRow).
            ' If not, set it to a large negative number to
            ' indicate that the bookmark is invalid.
            If Index < 0 Or Index >= iMaxRow Then Index = -9999
            Return Index
        End If
    End Function


    Private Sub FormatCheque(ByRef iCurrentTab As Integer)

        With tdgCash(iCurrentTab)
            .Left = 240
            '    .Top = 330
            .Width = 4935
            .TabStop = False
            .Tag = "BANK"
        End With
        ' Set column heading text
        Dim tdgColumns As DataGridViewColumn = tdgCash(iCurrentTab).Columns(0)
        With tdgColumns
            .HeaderText = "Drawer"

            'TODO
            '.AllowFocus = False
            .Width = 1835
        End With
        tdgColumns = tdgCash(iCurrentTab).Columns(1)
        With tdgColumns
            .HeaderText = "Bank"

            'TODO
            '.AllowFocus = False
            .Width = 1630
        End With

        Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing
        newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
        tdgCash(iCurrentTab).Columns.Insert(2, newColumn)
        tdgColumns = newColumn
        tdgColumns.Visible = True

        With tdgColumns
            .HeaderText = "Amount"

            'TODO
            '.AllowFocus = False
            .Width = 100 '1415
        End With
        tdgColumns = Nothing

        With txtTotalCash(iCurrentTab)
            .Left = VB6.TwipsToPixelsX(3720)
            '      .Top = 2910
            .Width = VB6.TwipsToPixelsX(1440)
        End With
        With lblCashTot(iCurrentTab)
            .Left = VB6.TwipsToPixelsX(2160)
            '     .Top = 2940
            .Width = VB6.TwipsToPixelsX(1455)
        End With

    End Sub

    Public Sub FormatFloatGrid(ByRef iCurrentTab As Integer)

        Dim tdgColumns As DataGridViewColumn = tdgFloat(iCurrentTab).Columns(0)
        ' Set column heading text
        With tdgColumns
            .HeaderText = "Denomination"

            'TODO
            '.AllowFocus = False
            .Width = 1275.02
        End With

        tdgColumns = tdgFloat(iCurrentTab).Columns(1)
        With tdgColumns
            .HeaderText = "Value"

            'TODO
            '.AllowFocus = True
            .Width = 750
        End With

        Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing
        newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
        tdgFloat(iCurrentTab).Columns.Insert(2, newColumn)
        tdgColumns = newColumn
        With tdgColumns
            .Visible = True
            .HeaderText = "Amount"

            'TODO
            '.AllowFocus = False
            .Width = 100 '1184.88
        End With

        tdgColumns = Nothing

        tdgFloat(iCurrentTab).RowsCount = m_vCurrencyDenom.GetUpperBound(1) + 1

    End Sub

    Public Sub FormatCashGrid(ByRef iCurrentTab As Integer)

        ' For the sake of efficiency, we use Column objects
        ' to reference column properties instead of repeatedly
        ' going through the grid's Columns collection object.

        ' Set column heading text
        Dim tdgColumns As DataGridViewColumn = tdgCash(iCurrentTab).Columns(0)
        With tdgColumns
            .HeaderText = "Denomination"

            'TODO
            '.AllowFocus = False
            .Width = 1275.02
        End With

        tdgColumns = tdgCash(iCurrentTab).Columns(1)
        With tdgColumns
            .HeaderText = "Value"

            'TODO
            '.AllowFocus = True
            .Width = 750
        End With

        Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing
        newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
        tdgCash(iCurrentTab).Columns.Insert(2, newColumn)
        tdgColumns = newColumn
        With tdgColumns
            .Visible = True
            .HeaderText = "Amount"

            'TODO
            '.AllowFocus = False
            .Width = 100 'will Auto size to fill rest of bar.
        End With

        tdgColumns = Nothing

        ' Inform the grid of how many rows are in the data set.
        ' This helps with scroll bar positioning.
        tdgCash(iCurrentTab).RowsCount = m_vCurrencyDenom.GetUpperBound(1) + 1

    End Sub


    Private Function SaveCash() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SaveCash
        ' PURPOSE: Save the Cash Breakdown values
        ' AUTHOR: Danny Davis
        ' DATE: 14 August 2003, 01:37 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            For ii As Integer = ACFirstMediaTypeTab To m_sTabsMediaType.GetUpperBound(0)
                If m_sTabsMediaType(ii).Trim() = "CASH" Then
                    For jj As Integer = 0 To m_vCurrencyDenom.GetUpperBound(1)
                        'Save the Cash and Float Denominations.
                        'These will be used to reload the summary cash tab
                        'for the second confirming user

                        If m_blnUseFloat And Conversion.Val(CStr(vFloatBreakDown(ii, 1, jj))) > 0 Then


                            m_lReturn = m_oBusiness.SaveCash(v_lCashListID:=m_lCashlistID, v_lCurrencyDenomID:=CInt(m_vCurrencyDenom(ACCurrencyDenomID, jj)), v_blnFloat:=1, v_lAmount:=CInt(vFloatBreakDown(ii, 1, jj)))
                        End If

                        '29/04/2003 - PWC - ENDVR00000848
                        'Only save cash if we actualy have cash (and not just a cash tab for the foat)

                        If m_blnHasCash And Conversion.Val(CStr(vCashBreakDown(ii, 1, jj))) > 0 Then


                            m_lReturn = m_oBusiness.SaveCash(v_lCashListID:=m_lCashlistID, v_lCurrencyDenomID:=CInt(m_vCurrencyDenom(ACCurrencyDenomID, jj)), v_blnFloat:=0, v_lAmount:=CInt(vCashBreakDown(ii, 1, jj)))
                        End If
                    Next jj
                    tdgFloat(ii).ReadOnly = False
                    tdgCash(ii).ReadOnly = False
                End If
            Next ii
            m_blnFormSaved = True



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function


    'called when first approval already OK'd

    Private Function LoadCash() As Object
        Dim vCashData(,) As Object

        Const cCurrencyDenomID As Integer = 0
        Const cIsFloatFlag As Integer = 1
        Const cAmount As Integer = 2

        Try



            m_lReturn = m_oBusiness.LoadCash(vCashData, CashlistID)
            'vCashData now holds all the current data for the Float and Cash grids

            For ii As Integer = ACFirstMediaTypeTab To m_sTabsMediaType.GetUpperBound(0)
                If m_sTabsMediaType(ii) = "CASH" Then

                    For jj As Integer = 0 To vCashData.GetUpperBound(1)
                        For kk As Integer = 0 To m_vCurrencyDenom.GetUpperBound(1)

                            If vCashData(cCurrencyDenomID, jj).Equals(m_vCurrencyDenom(ACCurrencyDenomID, kk)) Then

                                If Conversion.Val(CStr(vCashData(cIsFloatFlag, jj))) = gPMConstants.PMEReturnCode.PMFalse Then 'its cash


                                    vCashBreakDown(ii, 1, kk) = vCashData(cAmount, jj)


                                    vCashBreakDown(ii, 2, kk) = StringsHelper.Format(Conversion.Val(CStr(vCashBreakDown(ii, 1, kk))) * Conversion.Val(CStr(m_vCurrencyDenom(ACCurrDenomValue, kk))), "0.00")
                                    Exit For
                                Else
                                    'its float


                                    vFloatBreakDown(ii, 1, kk) = vCashData(cAmount, jj)


                                    vFloatBreakDown(ii, 2, kk) = StringsHelper.Format(Conversion.Val(CStr(vFloatBreakDown(ii, 1, kk))) * Conversion.Val(CStr(m_vCurrencyDenom(ACCurrDenomValue, kk))), "0.00")
                                    Exit For
                                End If
                            End If
                        Next kk
                    Next jj

                    tdgFloat(ii).ReadOnly = False
                    tdgCash(ii).ReadOnly = False

                    'Total up the 'Cash'
                    txtTotalCash(ii).Text = CStr(0)
                    txtTotalFloat(ii).Text = CStr(0)
                    For jj As Integer = 0 To vCashBreakDown.GetUpperBound(2)

                        txtTotalCash(ii).Text = StringsHelper.Format(Conversion.Val(txtTotalCash(ii).Text) + Conversion.Val(CStr(vCashBreakDown(ii, 2, jj))), "0.00")

                        txtTotalFloat(ii).Text = StringsHelper.Format(Conversion.Val(txtTotalFloat(ii).Text) + Conversion.Val(CStr(vFloatBreakDown(ii, 2, jj))), "0.00")
                    Next jj

                    'Keep a total of the 'Float Remaining'
                    txtFloatRem(ii).Text = StringsHelper.Format(m_cCash_Float_Amount - Conversion.Val(txtTotalFloat(ii).Text), "0.00")

                End If
            Next ii
            m_blnFormSaved = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load the cash details", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadCash", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function


        End Try

    End Function

    Private Function DisplayCashListReports(ByRef r_lCashListId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCashListReports
        ' PURPOSE: Call the Crystal Reports component
        ' AUTHOR: Paul Cunnigham
        ' DATE: 17 October 2002, 16:12:11
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oReport As iPMBReportPrint.Interface_Renamed
        Const ksReportGroupCode As String = "sysFCL1"


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oReport = New iPMBReportPrint.Interface_Renamed()

            With oReport

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                .CallingAppName = "iACTFindCashList.Interface"

                'Ensure the PMView is set so we enter in read only mode
                m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                'Set the start up options
                Dim vKeys(1, 4) As Object
                'Set the name of parameter1

                vKeys(0, 0) = "param_name1"

                vKeys(1, 0) = "cashlist_id"
                'set the value of parameter1

                vKeys(0, 1) = "cashlist_id"

                vKeys(1, 1) = r_lCashListId
                'Tell the Report Print component that we want to
                'filter the list of reports that are displayed

                vKeys(0, 2) = "filter_reports"
                'set the filter name to 'report_group'

                vKeys(1, 2) = "report_group"
                'set the filter value for the above filter to csFindCashListReportGroup

                vKeys(0, 3) = "report_group"

                vKeys(1, 3) = ksReportGroupCode
                'set so the params we pass don't get trashed

                vKeys(0, 4) = "save_params"
                'vKeys(1, 4) 'NOT USED

                .SetKeys(vKeys)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
                m_lErrorNumber = .Start()

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End With

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCashListReports", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Return result
            End Select

        Finally
            If Not (oReport Is Nothing) Then
                oReport.Dispose()
                oReport = Nothing
            End If



        End Try
        Return result
    End Function

    Private Function AllocateTotal(ByRef r_lAccountId As Integer, ByRef r_lTransDetailId As Integer, ByRef r_cAmount As Decimal, ByRef r_vTransDetails As Array) As Integer
        Dim result As Integer = 0
        Dim bACTAllocate As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AllocateTotal
        ' PURPOSE:
        ' AUTHOR: Paul Cunningham
        ' DATE: 16 December 2002, 10:26:42
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "AllocateTotal"


        Dim oAllocate As bACTAllocate.Business
        Dim lNewUpperBound As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Create an instance of the business object to do the allocation
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oAllocate As Object
            If g_oObjectManager.GetInstance(temp_oAllocate, "bACTAllocate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                oAllocate = temp_oAllocate

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get instance of object: bACTAllocate.Business")
            Else
                oAllocate = temp_oAllocate
            End If

            'Add the new transaction to the array (this is the format that
            'PerformAutoAllocation requires)
            lNewUpperBound = r_vTransDetails.GetUpperBound(1) + 1
            r_vTransDetails = ArraysHelper.RedimPreserve(Of Object(,))(r_vTransDetails, New Integer() {r_vTransDetails.GetUpperBound(0) - r_vTransDetails.GetLowerBound(0) + 1, lNewUpperBound - r_vTransDetails.GetLowerBound(1) + 1}, New Integer() {r_vTransDetails.GetLowerBound(0), r_vTransDetails.GetLowerBound(1)})


            r_vTransDetails(0, lNewUpperBound) = r_lTransDetailId

            r_vTransDetails(1, lNewUpperBound) = r_cAmount * -1


            r_vTransDetails(2, lNewUpperBound) = r_vTransDetails(1, lNewUpperBound)

            'Attempt to do the allocation

            If oAllocate.PerformAutoAllocation(r_lAccountId:=r_lAccountId, r_lTransDetailId:=r_lTransDetailId, v_vOSTransactions:=r_vTransDetails) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to allocate total")
            End If

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select

        Finally
            If Not (oAllocate Is Nothing) Then

                oAllocate.Dispose()
                oAllocate = Nothing
            End If




        End Try
        Return result
    End Function


    Private Function DisplayMessage(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer, ByVal ParamArray r_vTokens() As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayMessage
        ' PURPOSE: Displays a message based on passed resource file Ids
        ' AUTHOR: Sirius Financial Systems Plc
        ' DATE: 09 October 2002, 16:03:54
        ' RETURNS: PMTrue for success
        ' CHANGES: PWC 16/10/2002 - Added param array to enable substition of tokens
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sTitle, sMessage As String


        Try

            'Get the title from the res file

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Get the message from the res file

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Replace Tokens in the message
            ReplaceTokens(sMessage, New Object() {r_vTokens})

            'Now display the message to the user
            result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function
End Class
