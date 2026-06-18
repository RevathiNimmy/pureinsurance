Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmViewAllocation
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02nd October 2002
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RAW 12/03/2003 : ISS2893 : Reverse Allocations by AllocationId and AllocationDetailId instead of CashListItem and TransDetailId
    '                            AllocationId replaces TransDetailId in ListViews and columns reordered
    ' RAW 17/04/2003 : CQ819 : added write off column to list views
    ' RAW 15/05/2003 : CQ954 : added document ref to list views and moved account
    '                          added icon to list view to mark all rows in the same allocation as that entry that is clicked
    ' ***************************************************************** '
    'replaced iPMFunc.GetResData with GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmViewAllocation"
    'developer guide no.7(Latest guide)
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)

    Private m_lAllocationID As Integer ' RAW 12/03/2003 : ISS2893 : added
    Private m_lAllocationDetailID As Integer ' RAW 12/03/2003 : ISS2893 : added
    Private m_lTransdetailID As Integer ' RAW 12/03/2003 : ISS2893 : renamed and made private (original name replaced with public property)
    'Public CashListItemId As Long              ' RAW 12/03/2003 : ISS2893 : removed
    '28/05/2003 - PWC - 186 - Debt Rollup
    Private m_bRollup As Boolean
    Private m_lDocumentId As Integer
    Private m_lAccountID As Integer
    Private m_sSpare As String = ""
    Private m_bIsThirdParty As Boolean = False

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

    ' Payment Maintenance
    Private m_dtTransactionDate As Date
    Private m_iAllowReverseAllocation As gPMConstants.PMEReturnCode
    Private m_iReverseAllocationDays As Integer
    Private m_iTransCurrencyId As Integer
    Private m_crTransAmt As Decimal

    Private m_iHasPaymentAuthority As gPMConstants.PMEReturnCode
    Private m_lPaymentCurrencyId As Integer
    Private m_crPaymentAmount As Decimal
    Private m_iHasClaimAuthority As gPMConstants.PMEReturnCode
    Private m_lClaimCurrencyId As Integer
    Private m_crClaimAmount As Decimal
    Private m_crAmt As Decimal
    Private m_lTransId As Integer
    Private m_bIsLead As Boolean


    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTFindTransaction.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Public m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores private details from the business object.

    ' {* USER DEFINED CODE (Begin) *}

    ' Stores the List data from the business object.
    Private m_aResultArray(,) As Object
    Public m_sPlanRef As String
    Private bCloseButtonClicked As Boolean = False
    'Columns for ListViews
    Private Enum ListViewColumns ' RAW 12/03/2003 : ISS2893 : renamed
        ' renamed by adding eListCol_ prefix to avoid clash with property names
        eListCol_DocRef = 0 ' RAW 15/05/2003 : CQ954 : added
        eListCol_TransDetailId
        eListCol_TransDate
        eListCol_AllocatedDate
        eListCol_AllocatedAmount
        eListCol_OriginalAmount
        eListCol_WriteOffAmount ' RAW 17/04/2003 : CQ819 : added
        eListCol_DocType
        eListCol_InsuranceRef
        eListCol_AccountID ' RAW 15/05/2003 : CQ954 : moved
        eListCol_User ' RAW 12/03/2003 : ISS2893 : moved
        eListCol_CashListItem_Id
        eListCol_AllocationId ' RAW 12/03/2003 : ISS2893 : added
        eListCol_AllocationDetailId ' RAW 12/03/2003 : ISS2893 : added
        eListCol_RoundOffAmount
    End Enum

    ' RAW 12/03/2003 : ISS2893 : added to cater for differences between listview and result set columns
    Private Enum DataColumns
        eDataCol_AccountID = 0
        eDataCol_TransDetailId
        eDataCol_TransDate
        eDataCol_AllocatedDate
        eDataCol_AllocatedAmount
        eDataCol_OriginalAmount
        eDataCol_User
        eDataCol_DocType
        eDataCol_InsuranceRef
        eDataCol_CashListItem_Id
        eDataCol_AllocationCashListItemId ' RAW 12/03/2003 : ISS2893 : added
        eDataCol_AllocationId ' RAW 12/03/2003 : ISS2893 : added
        eDataCol_AllocationDetailId ' RAW 12/03/2003 : ISS2893 : added
        eDataCol_WriteOffAmount ' RAW 17/04/2003 : CQ819 : added
        eDataCol_DocRef
        eDataCol_Spare
        eDataCol_LossGainAmount
        ' RAW 15/05/2003 : CQ954 : added
        eDataCol_RoundOffAmount
        eDataCol_IsSplitAllocated

    End Enum
    ' RAW 12/03/2003 : ISS2893 : end

    Private m_lDisableReverse As gPMConstants.PMEReturnCode

    Private m_sUnderwritingOrAgency As String = ""
    Private m_bCurrGainLoss As Boolean
    Private v_duplicate As Boolean = False
    Private v_fAddAmount As Boolean = False
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PRIVATE Const Members (Begin)
    ' {* USER DEFINED CODE (Begin) *}

    'Columns for ListView


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Const Members (End)

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

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}

    ' RAW 12/03/2003 : ISS2893 : added
    Public WriteOnly Property TransDetailId() As Integer
        Set(ByVal Value As Integer)
            m_lTransdetailID = Value
        End Set
    End Property

    Public WriteOnly Property Spare() As String
        Set(ByVal Value As String)
            m_sSpare = Value
        End Set
    End Property

    '27/05/2003 - PWC - 186 - Debt Rollup
    Public WriteOnly Property Rollup() As Boolean
        Set(ByVal Value As Boolean)
            m_bRollup = Value
        End Set
    End Property
    Public WriteOnly Property DocumentId() As Integer
        Set(ByVal Value As Integer)
            m_lDocumentId = Value
        End Set
    End Property
    Public WriteOnly Property AccountID() As Integer
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property

    Public WriteOnly Property DisableReverse() As Integer
        Set(ByVal Value As Integer)
            m_lDisableReverse = Value
        End Set
    End Property

    ' Payment Maintenance Set Properties
    Public WriteOnly Property TransactionDate() As Date
        Set(ByVal Value As Date)
            m_dtTransactionDate = Value
        End Set
    End Property

    Public WriteOnly Property AllowReverseAllocation() As Integer
        Set(ByVal Value As Integer)
            m_iAllowReverseAllocation = Value
        End Set
    End Property

    Public WriteOnly Property ReverseAllocationDays() As Integer
        Set(ByVal Value As Integer)
            m_iReverseAllocationDays = Value
        End Set
    End Property

    Public WriteOnly Property TransCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_iTransCurrencyId = Value
        End Set
    End Property

    Public WriteOnly Property TransAmt() As Decimal
        Set(ByVal Value As Decimal)
            m_crTransAmt = Value
        End Set
    End Property

    Public WriteOnly Property HasPaymentAuthority() As Integer
        Set(ByVal Value As Integer)
            m_iHasPaymentAuthority = Value
        End Set
    End Property

    Public WriteOnly Property PaymentCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_lPaymentCurrencyId = Value
        End Set
    End Property

    Public WriteOnly Property PaymentAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_crPaymentAmount = Value
        End Set
    End Property

    Public WriteOnly Property HasClaimAuthority() As Integer
        Set(ByVal Value As Integer)
            m_iHasClaimAuthority = Value
        End Set
    End Property

    Public WriteOnly Property ClaimCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimCurrencyId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_crClaimAmount = Value
        End Set
    End Property

    Public WriteOnly Property TransId() As Integer
        Set(ByVal Value As Integer)
            m_lTransId = Value
        End Set
    End Property

    Public WriteOnly Property IsLead() As Boolean
        Set(ByVal value As Boolean)
            m_bIsLead = value
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

            '27/05/2003 - PWC - 186 - Debt Rollup
            If Not m_bRollup Then

                m_lReturn = m_oBusiness.ViewAllocationDetails(v_lTransDetailId:=m_lTransdetailID, r_vResultArray:=m_aResultArray)
            Else
                m_lReturn = GetRollupAllocationDetail()
            End If

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
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Update the interface details.

            'Poulate the listview with the results
            'Did we get any results back
            If Information.IsArray(m_aResultArray) Then
                PopulateLists()

                'Data is now stored in the listview so we can erase the array

                m_aResultArray = Nothing
            Else
                'Display standard message
                DisplayMessage(ACNotAllocatedTitle, ACNotAllocatedDetails, MsgBoxStyle.Exclamation)

                m_lReturn = gPMConstants.PMEReturnCode.PMNotFound
                Return result

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    ' Use DirectAdd as we need to get the ID back

                    ' {* USER DEFINED CODE (Begin) *}

                    ' ************************************************************
                    ' Enter your code here to perform a direct add in to the database
                    '
                    ' Example:-
                    '
                    ' m_lReturn& = m_oBusiness.DirectAdd( _
                    ''                    vCashListID:=m_lCashlistID, _
                    ''                    vCashliststatusID:=m_lCashListStatusID, _
                    ''                    vCashListTypeID:=m_lCashlistTypeID)
                    '
                    ' NOTE: Replace this section with your new code.
                    ' ************************************************************

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    ' ************************************************************
                    ' Enter your code here to perform an update to the database
                    '
                    ' Example:-
                    '
                    ' m_lReturn& = m_oBusiness.EditUpdate( _
                    ''                    lRow:=lBusinessDataID&, _
                    ''                    vCashListID:=m_lCashlistID, _
                    ''                    vCashliststatusID:=m_lCashListStatusID, _
                    ''                    vCashListTypeID:=m_lCashlistTypeID)
                    '
                    ' NOTE: Replace this section with your new code.
                    ' ************************************************************

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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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
            'DisplayCaptions functions should be local.
            m_lReturn = DisplayCaptions(Me)

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

            ' ************************************************************
            ' Enter your code here to set up interface defaults.
            '
            ' Example:-
            '
            '    cmdOK.Default = True
            '    uctType.ListIndex = 0
            '    txtDate.Text = FormatField( _
            ''                        iFormatType:=PMFormatDateLong, _
            ''                        vFieldValue:=Now)
            '
            '   'Setup default data for Add
            '   If (m_iTask% = PMAdd) Then
            '       cmdPopulate.Enabled = True
            '       uctType.ListIndex = 0
            '   Else
            '       uctType.ListIndex = 1
            '   End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

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

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        ' Click event of the Close button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
                bCloseButtonClicked = True
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Close command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClose_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdReverse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReverse.Click
        Dim bACTAllocationPost As Object

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdReverse_Click
        ' PURPOSE: Call the Allocate component to do a reversal
        ' AUTHOR: Paul Cunnigham
        ' DATE: 17 November 2002, 16:12:11
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Dim oAllocate As bACTAllocationPost.Automated
        Dim bDoAllocationDetailPairsExist As Boolean ' RAW 15/05/2003 : CQ954 : added
        ' RAW 15/05/2003 : CQ954 : added
        Dim sText As New StringBuilder ' RAW 15/05/2003 : CQ954 : added
        'Payment Maintenance
        Dim vAllocationData As Object
        Dim sCashListTypeCode As String = ""
        'SMJB: 25/07/03 CQ 969
        Const ACCreditControlOptionNo As String = "5001"
        Dim sCreditControlValue As String = ""
        Dim sMessage As String
        Dim dtTransDate As Date
        Dim crPaymentAmt As Decimal
        Dim crClaimAmt As Decimal


        Try

            ' Payment Maintenance

            m_lReturn = m_oBusiness.GetCashListTypeCode(vResultArray:=vAllocationData, v_iTransDetailId:=m_lTransId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:="cmdReverse_Click", v_sDescription:="Failed to Get CashListTypeCode")
                Exit Sub
            End If

            'Carry - R/CP/P
            If Information.IsArray(vAllocationData) Then
                sCashListTypeCode = gPMFunctions.ToSafeString(vAllocationData(0, 0)).Trim()
            End If

            ''''''''''Currency Conversion and Payment
            ' that means amount has to be checked
            If (m_iHasPaymentAuthority = gPMConstants.PMEReturnCode.PMTrue) And (sCashListTypeCode = "P") Then

                ' Do Conversion of Currency
                m_lReturn = GetConvertedPaymentAmount("P")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise Error.
                    gPMFunctions.RaiseError(v_sSource:="cmdReverse_Click", v_sDescription:="Currency Conversion Failed")
                    Exit Sub
                End If

                ' User Authority allow Payment Cancel Amount
                crPaymentAmt = m_crPaymentAmount

                ' Checks Transaction Converted amount with
                ' User Authority allow Payment Amount
                If m_crAmt > crPaymentAmt Then
                    MessageBox.Show("User has exceeded the maximum Payment Reversal Amount", "View Allocation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

            End If
            ' else he is allowed to cancel any amount

            If sCashListTypeCode = "CP" Then 'Claim Payment
                If m_iHasClaimAuthority = gPMConstants.PMEReturnCode.PMTrue Then

                    ' Do Conversion of Currency
                    m_lReturn = GetConvertedPaymentAmount("CP")

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Raise Error.
                        gPMFunctions.RaiseError(v_sSource:="cmdReverse_Click", v_sDescription:="Currency Conversion Failed")
                        Exit Sub
                    End If

                    ' User Authority allow Claim Cancel Amount
                    crClaimAmt = m_crClaimAmount

                    ' Checks Transaction Converted amount with
                    ' User Authority allow Payment Amount
                    If m_crAmt > crClaimAmt Then
                        MessageBox.Show("User has exceeded the maximum Claim Reversal Amount", "View Allocation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If

                Else
                    'MessageBox.Show("User has no Authority to Cancel Claim Payment", "View Allocation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'Exit Sub
                End If
            End If

            dtTransDate = m_dtTransactionDate.AddDays(m_iReverseAllocationDays)
            If m_iAllowReverseAllocation <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Cannot reverse an allocation , user has no rights", "View Allocation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            'PN: 45534
            If dtTransDate < DateTime.Today Then
                MessageBox.Show("Cannot reverse an allocation , Days exceeds", "View Allocation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            'Only process if there is a selection made

            'DD 02/05/2003: Changed to module variables which works when the
            'item click event is handled.
            If m_lAllocationID = 0 Or m_lAllocationID = 0 Then
                'Display standard message
                DisplayMessage(ACNoSelectionTitle, ACNoSelectionDetails, MsgBoxStyle.Exclamation)

                Exit Sub
            End If

            'Allocation of a reversed trasaction can not be reversed.
            If m_sSpare.ToUpper().StartsWith("REVER") Then
                MessageBox.Show("Cannot reverse an allocation on a reversed/reversal transaction.", "Invalid Selection",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            'Confirm reversal
            If _
                DisplayMessage(ACConfirmReversalTitle, ACConfirmReversalDetails, MsgBoxStyle.Question + MsgBoxStyle.YesNo) =
                System.Windows.Forms.DialogResult.Yes Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_oAllocate As Object
                If _
                    g_oObjectManager.GetInstance(temp_oAllocate, "bACTAllocationPost.Automated",
                                                 vInstanceManager:="ClientManager") <> gPMConstants.PMEReturnCode.PMTrue Then
                    oAllocate = temp_oAllocate

                    Exit Sub
                Else
                    oAllocate = temp_oAllocate
                End If

                'Lead will only be true for a split receipt
                'This msg needs to be shown for a split receipt which is also the lead .
                If m_bIsLead = True Then

                    If _
                        MessageBox.Show("This is a split receipt - reversal will also affect split items.", "Information",
                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) =
                        Windows.Forms.DialogResult.Cancel Then
                        Exit Sub
                    End If
                End If

                ' RAW 15/05/2003 : CQ954 : added
                ' Can just this allocation detail be reversed or can we only reverse the complete allocation
                ' ==========================================================================================

                If _
                    oAllocate.DoAllocationDetailPairsExist(r_bDoAllocationDetailPairsExist:=bDoAllocationDetailPairsExist,
                                                           v_lTransDetailID:=m_lTransdetailID,
                                                           v_lAllocationID:=m_lAllocationID) <>
                    gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Failed to identify whether partial reversals are allowed", "View Allocation",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Not bDoAllocationDetailPairsExist Then

                    ' not every credit can be matched to a debit within this allocation fo we have to reverse the lot
                    ' unless there is only one credit and one debit

                    If lvwCredits.Items.Count > 1 Or lvwDebits.Items.Count > 1 Then

                        ' Display a warning to user

                        For i As Integer = 1 To lvwCredits.Items.Count
                            If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwCredits.Items.Item(i - 1), ListViewColumns.eListCol_AllocationId).Text) = m_lAllocationID Then
                                sText.Append(lvwCredits.Items.Item(i - 1).Text & " = " &
                                                 ListViewHelper.GetListViewSubItem(lvwCredits.Items.Item(i - 1), ListViewColumns.eListCol_AllocatedAmount).Text & ",  ")
                            End If
                        Next i

                        For i As Integer = 1 To lvwDebits.Items.Count
                            If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwDebits.Items.Item(i - 1), ListViewColumns.eListCol_AllocationId).Text) = m_lAllocationID Or m_bIsLead = True Then
                                sText.Append(lvwDebits.Items.Item(i - 1).Text & " = " &
                                             ListViewHelper.GetListViewSubItem(lvwDebits.Items.Item(i - 1), ListViewColumns.eListCol_AllocatedAmount).Text & ",  ")
                            End If
                        Next i

                        If MessageBox.Show("It is not possible to reverse just this transaction" & Strings.Chr(13) & Strings.Chr(13) &
                                           "All transactions within the same allocation must be reversed together." & Strings.Chr(13) & Strings.Chr(13) &
                                           sText.ToString() & Strings.Chr(13) & "Are you Sure ?", "Reverse Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then

                            Exit Sub
                        End If
                        'Lead will only be true for a split receipt
                        'This msg needs to be shown for a split receipt which is also the lead .
                        If m_bIsLead = True Then
                            If MessageBox.Show("This is a split receipt - reversal will not affect split items." & Strings.Chr(13) & "Are you Sure ?", "Reverse Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Exit Sub
                            End If
                        End If

                    End If
                End If
                ' RAW 15/05/2003 : CQ954 : end


                'Call the routine to do the revesal for this transaction
                ' RAW 12/03/2003 : ISS2893 : replaced CashListItemID argument with AllocationID

                If oAllocate.ReverseAllocation(v_lTransDetailID:=m_lTransdetailID, v_lAllocationID:=m_lAllocationID) <> gPMConstants.PMEReturnCode.PMTrue Then

                    'Check system options, if Credit Control is set, then that is likely to be
                    'the reason that we've failed

                    m_lReturn = iPMFunc.GetSystemOption(CInt(ACCreditControlOptionNo), sCreditControlValue)

                    'Not checking m_lReturn as we're only checking the system option so
                    'we can provide a nice message to the user.  If it fails we're in an
                    'error handler anyway
                    If sCreditControlValue = "1" Then
                        sMessage = "Credit Control is switched on but there are no rules/steps associated " &
                                   "with this debt's branch and business type. Reversal cannot proceed." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                   "Please correct this using Credit Control Maintenance and try again."
                    Else
                        sMessage = "Failed to reverse allocation"
                    End If
                    MessageBox.Show(sMessage, "View Allocation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    If m_bIsThirdParty Then
                        Try
                            m_lReturn = m_oBusiness.PlanStatusUpdate(m_sPlanRef, bSIRPremFinConst.PFStatusIndLive)

                        Catch Ex As Exception
                        End Try

                    End If

                End If

                ' Close window
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()

            End If


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally
            If Not (oAllocate Is Nothing) Then

                oAllocate.Dispose()
            End If
            oAllocate = Nothing

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTFindTransaction.Business", vInstanceManager:="ClientManager")
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
            m_oGeneral = New iACTFindTransaction.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


            m_sUnderwritingOrAgency = g_oBusiness.UnderwritingOrAgency

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


    Private Sub frmViewAllocation_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the rules for validating fields.

            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)

            'Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the field validation.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
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

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            cmdReverse.Enabled = (m_lDisableReverse <> gPMConstants.PMEReturnCode.PMTrue)


            ' Gets the interface details to be displayed.
            If m_oGeneral.GetInterfaceDetails() <> gPMConstants.PMEReturnCode.PMTrue Then

                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                    Exit Sub
                End If
            End If
            'added the following lines of code to dispose the form if no allocation done for the transaction
            'start
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Me.Dispose()
            End If
            'end
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmViewAllocation_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode AndAlso Not bCloseButtonClicked Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7(Latest guide)
                    eventArgs.Cancel = True
                    Cancel = 1
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
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

            If Not (m_oFormfields Is Nothing) Then

                m_oFormfields.Dispose()

                m_oFormfields = Nothing
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

    Private Sub frmViewAllocation_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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

    Private Sub HandleListItemClick(ByRef r_lvwHotListView As ListView, ByRef r_lvwOldListView As ListView)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: HandleListItemClick
        ' PURPOSE: Handle the common processing required for the _ItemClick events of the
        '          lvwCredits and lvwDebits ListViews
        ' AUTHOR: Paul Cunningham
        ' DATE: 03 December 2002, 09:21:14
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lvwDebits_Temp As ListView


        Try

            'Ensure that we only have one item selected across both lists
            r_lvwOldListView.FocusedItem = Nothing

            ' Alix - 03/03/2003 - Issue 2525
            ' RAW 12/03/2003 : ISS2893 : replaced CashListItemID with AllocationID
            Dim dbNumericTemp As Double
            'developer guide no.52
            If Double.TryParse(r_lvwHotListView.FocusedItem.SubItems(ListViewColumns.eListCol_AllocationId).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                'developer guide no.52
                m_lAllocationID = CInt(r_lvwHotListView.FocusedItem.SubItems(ListViewColumns.eListCol_AllocationId).Text)
            Else
                m_lAllocationID = 0
            End If

            ' RAW 12/03/2003 : ISS2893 : added
            Dim dbNumericTemp2 As Double
            'developer guide no.52
            If Double.TryParse(r_lvwHotListView.FocusedItem.SubItems(ListViewColumns.eListCol_AllocationDetailId).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                'developer guide no.52
                m_lAllocationDetailID = CInt(r_lvwHotListView.FocusedItem.SubItems(ListViewColumns.eListCol_AllocationDetailId).Text)
            Else
                m_lAllocationDetailID = 0
            End If

            Dim dbNumericTemp3 As Double
            'developer guide no.52
            If Double.TryParse(r_lvwHotListView.FocusedItem.SubItems(ListViewColumns.eListCol_TransDetailId).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                'developer guide no.52
                m_lTransdetailID = CInt(r_lvwHotListView.FocusedItem.SubItems(ListViewColumns.eListCol_TransDetailId).Text)
            Else
                m_lTransdetailID = 0
            End If

            ' RAW 15/05/2003 : CQ954 : added
            If m_lAllocationID > 0 Then

                ' loop through list of credits and debits and set the icon to mark those entries that are
                ' members of the same allocation as the entry that was clicked

                For i As Integer = 1 To r_lvwHotListView.Items.Count
                    If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(r_lvwHotListView.Items.Item(i - 1), ListViewColumns.eListCol_AllocationId).Text) = m_lAllocationID Then

                        'developer guide no.49 & 162
                        r_lvwHotListView.Items(i - 1).ImageKey = ACIconCheck
                    Else

                        'developer guide no.49 & 162
                        r_lvwHotListView.Items(i - 1).ImageIndex = ACIconBlank
                    End If
                Next i

                For i As Integer = 1 To r_lvwOldListView.Items.Count
                    If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(r_lvwOldListView.Items.Item(i - 1), ListViewColumns.eListCol_AllocationId).Text) = m_lAllocationID Then

                        'developer guide no.49 & 162
                        r_lvwOldListView.Items(i - 1).ImageKey = ACIconCheck
                    Else

                        'developer guide no.49 & 162
                        r_lvwOldListView.Items(i - 1).ImageIndex = ACIconBlank
                    End If
                Next i

                'PN: 47427
                'Sum up the debit amounts of an AllocationID from lvwDebits
                If r_lvwHotListView.Name = "lvwDebits" Then
                    lvwDebits_Temp = r_lvwHotListView
                ElseIf r_lvwOldListView.Name = "lvwDebits" Then
                    lvwDebits_Temp = r_lvwOldListView
                End If

                For i As Integer = 1 To lvwDebits_Temp.Items.Count
                    If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwDebits_Temp.Items.Item(i - 1), ListViewColumns.eListCol_AllocationId).Text) = m_lAllocationID Then
                        'Take the transaction id to find correct cashlist type if the transaction type is Payment.
                        If ListViewHelper.GetListViewSubItem(lvwDebits_Temp.Items.Item(i - 1), ListViewColumns.eListCol_DocType).Text = "Payment" Then
                            m_lTransId = CInt(ListViewHelper.GetListViewSubItem(lvwDebits_Temp.Items.Item(i - 1), ListViewColumns.eListCol_TransDetailId).Text)
                        End If
                        m_crTransAmt += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwDebits_Temp.Items.Item(i - 1), ListViewColumns.eListCol_AllocatedAmount).Text)
                    End If
                Next i

            End If
            ' RAW 15/05/2003 : CQ954 : end

            ' /Alix


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="HandleListItemClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally



        End Try
    End Sub



    Private Sub lvwCredits_ItemClick(ByVal Item As ListViewItem)

        'Handle the click
        HandleListItemClick(r_lvwHotListView:=lvwCredits, r_lvwOldListView:=lvwDebits)
    End Sub

    Private Sub lvwDebits_ItemClick(ByVal Item As ListViewItem)

        'Handle the click
        HandleListItemClick(r_lvwHotListView:=lvwDebits, r_lvwOldListView:=lvwCredits)
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                Else
                    'cmdOK.Default = True
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



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
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

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_0.Click
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



            ' Error Section

            Exit Sub
        End Try


    End Sub


    ' ***************************************************************** '
    ' Name: PopulateList
    '
    ' Description: Populates the listview with data.
    '
    ' ***************************************************************** '
    Private Function PopulateLists() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PopulateLists
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 18 November 2002, 14:44:19
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer
        Dim oListView As ListView


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Clear the existing items
            lvwCredits.Items.Clear()
            lvwDebits.Items.Clear()


            lLower = m_aResultArray.GetLowerBound(1)

            lUpper = m_aResultArray.GetUpperBound(1)

            'Check for any currency gain loss
            m_bCurrGainLoss = False

            For lRow As Integer = lLower To lUpper
                If gPMFunctions.ToSafeDouble(m_aResultArray(DataColumns.eDataCol_LossGainAmount, lRow)) <> 0 Then
                    m_bCurrGainLoss = True

                    'Set Captions
                    lvwDebits.Columns.Item(6).Text = "Currency Gain/Loss"
                    lvwCredits.Columns.Item(6).Text = "Currency Gain/Loss"
                    Exit For
                End If
            Next

            'Populate the listviews
            For lRow As Integer = lLower To lUpper

                If ToSafeDouble(m_aResultArray(DataColumns.eDataCol_AllocatedAmount, lRow)) < 0 Then
                    'Credits
                    oListView = lvwCredits
                Else
                    'Debits
                    oListView = lvwDebits
                End If

                'Add the data to the listview
                If AddListViewItem(r_oListView:=oListView, v_lRow:=lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                    'DD 17/10/2003: Manual Reversals for Instalments is not allowed

                    m_sPlanRef = m_aResultArray(DataColumns.eDataCol_InsuranceRef, lRow)

                    If m_sPlanRef <> "" Then
                        m_lReturn = m_oBusiness.CheckIsThirdParty(m_aResultArray(DataColumns.eDataCol_InsuranceRef, lRow), m_bIsThirdParty)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to get details.
                            result = gPMConstants.PMEReturnCode.PMFalse

                        End If
                    End If

                    If m_bIsThirdParty = False Then
                        If ToSafeString(m_aResultArray(DataColumns.eDataCol_DocRef, lRow)) <> "" Then
                            If ChkDocTypeIsInstalments(ToSafeString(m_aResultArray(DataColumns.eDataCol_DocRef, lRow)).Substring(0, 3)) Then
                                cmdReverse.Enabled = False
                            End If
                        End If
                    End If
                End If
            Next lRow

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateLists", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function AddListViewItem(ByRef r_oListView As ListView, ByVal v_lRow As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddListViewItem
        ' PURPOSE: Adds a new ListItem to the ListView
        ' AUTHOR: Paul Cunningham
        ' DATE: 15 October 2002, 14:05:03
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim sTransDetailId As String = ""
        Dim sTransDetailIdForDebit As String = ""
        Dim iRow As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMFalse



            sTransDetailId = "Key" & ToSafeString(m_aResultArray(DataColumns.eDataCol_TransDetailId, v_lRow)) &
                                     ToSafeString(m_aResultArray(DataColumns.eDataCol_AllocationDetailId, v_lRow))
            sTransDetailIdForDebit = ToSafeString(m_aResultArray(DataColumns.eDataCol_TransDetailId, v_lRow))
            'Reset local variable v_duplicate and v_fAddAmount
            v_duplicate = True
            v_fAddAmount = True

            'Add the new item to the list
            ' RAW 15/05/2003 : CQ954 : DocRef replaces account
            'developer guide no.(For unique collection)
            Dim ext As Boolean = CheckIfExists(r_oListView, sTransDetailIdForDebit, iRow)
            If ext Then
                oListItem = r_oListView.Items.Add(sTransDetailId, ToSafeString(m_aResultArray(DataColumns.eDataCol_DocRef, v_lRow)).Trim(), "")

                'Populate the subitems
                With oListItem
                    ' Alix - 03/03/2003 - Added transdetailID line

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_TransDetailId).Text = CStr(m_aResultArray(DataColumns.eDataCol_TransDetailId, v_lRow))
                    ' /Alix

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_TransDate).Text = CStr(m_aResultArray(DataColumns.eDataCol_TransDate, v_lRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_AllocatedDate).Text = CStr(m_aResultArray(DataColumns.eDataCol_AllocatedDate, v_lRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_AllocatedAmount).Text = StringsHelper.Format(m_aResultArray(DataColumns.eDataCol_AllocatedAmount, v_lRow), "0.00")

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_OriginalAmount).Text = StringsHelper.Format(m_aResultArray(DataColumns.eDataCol_OriginalAmount, v_lRow), "0.00")
                    ' RAW 17/04/2003 : CQ819 : added
                    If Not m_bCurrGainLoss Then

                        ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_WriteOffAmount).Text = StringsHelper.Format(m_aResultArray(DataColumns.eDataCol_WriteOffAmount, v_lRow), "0.00")
                    Else

                        ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_WriteOffAmount).Text = StringsHelper.Format(m_aResultArray(DataColumns.eDataCol_LossGainAmount, v_lRow), "0.00")
                    End If


                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_User).Text = CStr(m_aResultArray(DataColumns.eDataCol_User, v_lRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_DocType).Text = CStr(m_aResultArray(DataColumns.eDataCol_DocType, v_lRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_InsuranceRef).Text = CStr(m_aResultArray(DataColumns.eDataCol_InsuranceRef, v_lRow))
                    ' RAW 15/05/2003 : CQ954 : added

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_AccountID).Text = CStr(m_aResultArray(DataColumns.eDataCol_AccountID, v_lRow)).Trim()

                    ' RAW 12/03/2003 : ISS2893 : added
                    ' RAW 15/05/2003 : CQ954 : added test for ""

                    Dim auxVar As Object = m_aResultArray(DataColumns.eDataCol_CashListItem_Id, v_lRow)


                    If (Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar))) And (CStr(m_aResultArray(DataColumns.eDataCol_CashListItem_Id, v_lRow)) <> "") Then
                        ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_CashListItem_Id).Text = gPMFunctions.NullToString(m_aResultArray(DataColumns.eDataCol_CashListItem_Id, v_lRow))
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_CashListItem_Id).Text = gPMFunctions.NullToString(m_aResultArray(DataColumns.eDataCol_AllocationCashListItemId, v_lRow))
                    End If

                    ' RAW 12/03/2003 : ISS2893 : added

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_AllocationId).Text = CStr(m_aResultArray(DataColumns.eDataCol_AllocationId, v_lRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_AllocationDetailId).Text = CStr(m_aResultArray(DataColumns.eDataCol_AllocationDetailId, v_lRow))
                    ListViewHelper.GetListViewSubItem(oListItem, ListViewColumns.eListCol_RoundOffAmount).Text = StringsHelper.Format(m_aResultArray(DataColumns.eDataCol_RoundOffAmount, v_lRow), "0.00")
                End With

                'Test to see if this item was the one originally selected in the search results listview
                ' RAW 12/03/2003 : ISS2893 : replaced TransDetailID with AllocationDetailID

                If ToSafeInteger(m_aResultArray(DataColumns.eDataCol_AllocationDetailId, v_lRow)) = m_lAllocationDetailID Then
                    r_oListView.FocusedItem = oListItem
                End If


                'Not sure why this code was writen but it's creating problem. If required then this can be handled correctly later. 
                'Else
                'If r_oListView.Items IsNot Nothing AndAlso r_oListView.Items.Count > 0 AndAlso r_oListView.Items(iRow).SubItems IsNot Nothing AndAlso String.IsNullOrEmpty(m_aResultArray(DataColumns.eDataCol_CashListItem_Id, v_lRow)) = False Then
                'r_oListView.Items(iRow).SubItems(4).Text = StringsHelper.Format(ToSafeDouble(r_oListView.Items(iRow).SubItems(4).Text) + ToSafeDouble(m_aResultArray(DataColumns.eDataCol_AllocatedAmount, v_lRow)), "0.00")
                ' result = gPMConstants.PMEReturnCode.PMTrue
                'End If
            Else
                sTransDetailId = "Key" & CStr(m_aResultArray(DataColumns.eDataCol_TransDetailId, v_lRow)) & CStr(m_aResultArray(DataColumns.eDataCol_AllocationDetailId, v_lRow))
                If r_oListView.Items(0).Name.Length > 0 Then
                    r_oListView.Items(0).SubItems(4).Text = StringsHelper.Format(ToSafeDouble(r_oListView.Items(0).SubItems(4).Text) + ToSafeDouble(m_aResultArray(DataColumns.eDataCol_AllocatedAmount, v_lRow)), "0.00")
                End If
                result = gPMConstants.PMEReturnCode.PMTrue
            End If
            result = gPMConstants.PMEReturnCode.PMTrue
        Catch ex As Exception

            Select Case Information.Err().Number
                'todolist
                'Case ccNonUniqueKey 'Key already exists
                'GoTo Finally_Renamed

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListViewItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (UnloadfrmDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UnloadfrmDetails() As Integer
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UnloadfrmDetails
    ' PURPOSE: Common routine to unload forms and release references
    ' AUTHOR: Paul Cunningham
    ' DATE: 11 October 2002, 12:07:05
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    '
    'Dim result As Integer = 0
    '
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Me.Close()
    'Me.Finalize()
    '
    'GoTo Finally_Renamed
    '
    '----------------------------------------------------------------------------------------
    'Only for Debugging, the code will never execute this line
    '----------------------------------------------------------------------------------------
    'Resume 
    '
    'Catch_Renamed: '
    'Select Case Information.Err().Number
    'Case Else
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadfrmDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'GoTo Finally_Renamed
    'End Select
    '
    'Finally_Renamed: '
    'Return result
    '
    'End Function


    Private Function GetRollupAllocationDetail() As Integer

        Dim nresult As Integer = 0
        Const ksMethod As String = "GetRollupAllocationDetail"
        Dim aTransArray(,) As Object
        Dim nTransDetailId As Integer
        Dim aAllocationArray(,) As Object
        Dim aAllocationRollUpArray(,) As Object
        Const knRowDimension As Integer = 2
        Const knTransDetailId As Integer = 0
        Const knAllocationTransDetailsId As Integer = 1
        Dim nRollupRow As Integer = 0
        Dim bIsDuplicateTrans As Boolean = False
        Dim nRollupLowerRow As Integer
        Dim nRollupUpperRow As Integer
        Dim nRollupLowerCol As Integer
        Dim nRollupUpperCol As Integer
        nresult = gPMConstants.PMEReturnCode.PMTrue

        m_aResultArray = Nothing

        If m_oBusiness.GetRollupTransactions(v_lDocumentID:=m_lDocumentId, v_lAccountId:=m_lAccountID, r_vResultArray:=aTransArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            nresult = gPMConstants.PMEReturnCode.PMFalse
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ksMethod + ", " + "Unable to get Transactions for document = " & m_lDocumentId & ", account = " & CStr(m_lAccountID))
        End If

        If Information.IsArray(aTransArray) Then
            For lRow As Integer = aTransArray.GetLowerBound(knRowDimension - 1) To aTransArray.GetUpperBound(knRowDimension - 1)

                nTransDetailId = CInt(aTransArray(knTransDetailId, lRow))
                aAllocationArray = Nothing
                aAllocationRollUpArray = Nothing
                nRollupRow = 0

                If m_oBusiness.ViewAllocationDetails(v_lTransDetailId:=nTransDetailId, r_vResultArray:=aAllocationArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    nresult = gPMConstants.PMEReturnCode.PMFalse
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ksMethod + ", " + "Unable to get Allocation details for TransDetailId = " & nTransDetailId)
                End If

                'Eliminates the duplicate allocation transaction entries
                If Information.IsArray(aAllocationArray) Then
                    If Information.IsArray(aAllocationRollUpArray) Then
                        nRollupLowerRow = aAllocationRollUpArray.GetLowerBound(klRowDimension - 1)
                        nRollupUpperRow = aAllocationRollUpArray.GetUpperBound(klRowDimension - 1)
                        nRollupLowerCol = aAllocationRollUpArray.GetLowerBound(klColDimension - 1)
                        nRollupUpperCol = aAllocationRollUpArray.GetUpperBound(klColDimension - 1)
                    Else
                        nRollupLowerRow = 0
                        nRollupUpperRow = 0
                        nRollupLowerCol = aAllocationArray.GetLowerBound(klColDimension - 1)
                        nRollupUpperCol = aAllocationArray.GetUpperBound(klColDimension - 1)
                    End If

                    If Information.IsArray(m_aResultArray) AndAlso m_aResultArray IsNot Nothing Then
                        For nAllocationRows As Integer = 0 To aAllocationArray.GetLength(1) - 1
                            bIsDuplicateTrans = False
                            For nAllocationCols As Integer = 0 To aAllocationArray.GetLength(0) - 1
                                'Check duplicate transaction in destination array
                                For nDestinationRows As Integer = 0 To m_aResultArray.GetLength(1) - 1
                                    If ToSafeLong(aAllocationArray(knAllocationTransDetailsId, nAllocationRows)) = ToSafeLong(m_aResultArray(knAllocationTransDetailsId, nDestinationRows)) Then
                                        bIsDuplicateTrans = True
                                        Exit For
                                    End If
                                Next

                                'Rebuild the allocation rollup array with unique transaction
                                If Not bIsDuplicateTrans Then
                                    aAllocationRollUpArray = ArraysHelper.RedimPreserve(Of Object(,))(aAllocationRollUpArray, New Integer() {nRollupUpperCol - nRollupLowerCol + 1, nRollupUpperRow - nRollupLowerRow + 1 + nRollupRow}, New Integer() {nRollupLowerCol, nRollupLowerRow})
                                    aAllocationRollUpArray(nAllocationCols, nRollupRow) = aAllocationArray(nAllocationCols, nAllocationRows)
                                Else
                                    Exit For
                                End If
                            Next
                            nRollupRow = nRollupRow + 1
                        Next
                    End If

                    'First iteration entries will be unique
                    If aAllocationRollUpArray Is Nothing AndAlso lRow = 0 Then
                        aAllocationRollUpArray = aAllocationArray
                    End If

                    If aAllocationRollUpArray IsNot Nothing Then
                        If AddSourceArrayToDestinationArray(r_vSourceArray:=aAllocationRollUpArray, r_vDestinationArray:=m_aResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                            nresult = gPMConstants.PMEReturnCode.PMFalse
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ksMethod + ", " + "Unable to add Allocation details to master array for TransDetailId = " & nTransDetailId)
                        End If
                    End If
                End If
            Next lRow
        End If

        Return nresult

    End Function

    Public Function GetConvertedPaymentAmount(ByVal sCurrcode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetConvertedPaymentAmount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crTransAmt, crPayAmt As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            crTransAmt = m_crTransAmt
            If crTransAmt < 0 Then
                crTransAmt = -(m_crTransAmt)
            End If

            ' get the total amount to pay in the selected payment currency
            If sCurrcode = "P" Then

                m_iTransCurrencyId = m_lPaymentCurrencyId

                lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=m_iTransCurrencyId, v_crCurrencyAmountFrom:=crTransAmt, v_lCompanyID:=g_iSourceID, v_lCurrencyIdTo:=m_lPaymentCurrencyId, r_crCurrencyAmountTo:=crPayAmt)

            Else

                m_iTransCurrencyId = m_lClaimCurrencyId

                lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=m_iTransCurrencyId, v_crCurrencyAmountFrom:=crTransAmt, v_lCompanyID:=g_iSourceID, v_lCurrencyIdTo:=m_lClaimCurrencyId, r_crCurrencyAmountTo:=crPayAmt)

            End If



            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("GetConvertedPaymentAmount Failed.", Application.ProductName)
            End If

            ' store the converted payment amount
            m_crAmt = crPayAmt


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

    'Sumeet: added the display captions functions from shared files.

    ' ***************************************************************** '
    ' Name:         DisplayCaptions
    '
    ' Description:  Display all language specific captions.
    ' Histroy:      TR22112002 - TR23 Changed parameter from Form to Object
    '               to support UserControls as well as Forms. Explicitly
    '               named the assumed default properties.
    ' ***************************************************************** '
    Public Function DisplayCaptions(ByRef r_frmSource As Form) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCaptions
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 14 October 2002, 15:21:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim iSkip As Integer
        'Used in the .Tag property of ListView.CoulmnHeading
        Const ksHidden As String = "HIDDEN"
        Dim sSetRoundOff As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            'Get the caption from the tag
            lCaptionID = GetCaptionID(Convert.ToString(r_frmSource.Tag))
            If lCaptionID > 0 Then
                'Get the caption from the res file using Id from tag property

                r_frmSource.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Check for an error.
                If r_frmSource.Text = "" Then
                    ' Failed to get data from the resource file.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                       "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                    Return result
                End If
            End If



            For i As Integer = 0 To ContainerHelper.Controls(r_frmSource).Count - 1

                lCaptionID = GetCaptionID(Convert.ToString(ControlHelper.GetTag(ContainerHelper.Controls(r_frmSource)(i))))
                If lCaptionID > 0 Then


                    Select Case ContainerHelper.Controls(r_frmSource)(i).GetType().Name
                        Case "SSTab"


                            For j As Integer = 0 To ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Tabs") - 1



                                ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "TabCaption", New Object() {j}, iPMFunc.GetResData(g_iLanguageID, lCaptionID + j, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            Next j
                            'Added ListView column headers
                        Case "ListView"
                            iSkip = 0

                            For j As Integer = 0 To ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns"), "Count") - 1
                                'Test for hidden ListView columns and skip as appropriate

                                If Not Information.IsNothing(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Tag")) Then
                                    If CStr(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Tag")).IndexOf(ksHidden) >= 0 Then
                                        iSkip += 1
                                    End If
                                Else
                                    'PWC - 16/10/2002 - No need to get caption if skipping this time



                                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Text", iPMFunc.GetResData(g_iLanguageID, lCaptionID + j - iSkip, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                                    If ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Text") = "User".Trim() Then
                                        sSetRoundOff = "Round Off Amount"
                                    End If
                                End If
                                If j = ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns"), "Count") - 1 Then
                                    If sSetRoundOff <> "" Then
                                        ReflectionHelper.SetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Text", sSetRoundOff)
                                        sSetRoundOff = ""
                                    End If
                                End If
                            Next j
                            'Added Picklist
                        Case "PickList"

                            ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "AvailableCaption", iPMFunc.GetResData(g_iLanguageID, lCaptionID, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        Case Else



                            ContainerHelper.Controls(r_frmSource)(i).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    End Select
                End If
            Next i


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    Public Function CheckIfExists(ByVal lstView As ListView, ByVal AccID As String, Optional ByRef r_iRow As Integer = 0, Optional ByVal v_sAllocationDetailID As String = "", Optional ByVal v_dAmount As Double = 0) As Boolean
        For Each item As ListViewItem In lstView.Items
            If item.SubItems(13).Text = AccID Then
                r_iRow = item.Index
                v_duplicate = False
            End If
            If v_duplicate = False AndAlso ToSafeDouble(item.SubItems(5).Text) = v_dAmount Then
                v_fAddAmount = False
            End If
        Next
        CheckIfExists = v_duplicate
    End Function
End Class
