Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
'Developer Guide no.129
Imports SharedFiles
Imports System.Runtime.InteropServices

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:11/07/00
    '
    ' Description: Main interface.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"

    'Constants for Defining the Columns in List View

    Private Const Column1 As Integer = 1 'Policy Holder
    Private Const Column2 As Integer = 2 'Policy Reference
    Private Const Column3 As Integer = 3 'Risk Index
    Private Const Column4 As Integer = 4 'Product Code
    Private Const Column5 As Integer = 5 'From Date - In Underwriting it's no longer used
    Private Const Column6 As Integer = 6 'To Date - In Underwriting it's renewal date
    Private Const Column7 As Integer = 7 'Post Code

    'TN20010426 Start
    Private Const Column8 As Integer = 8
    Private Const Column9 As Integer = 9
    'TN20010426 End

    'Constants for Defining Width of Columns in List View
    'DC061200 increased size of column widths
    'Private Const ColWidthBroking As Integer = 1300
    'Private Const ColWidthUnderWriting As Integer = 1000
    Private Const ColWidthBroking As Integer = 1900
    Private Const ColWidthUnderWriting As Integer = 1400

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'Claims -Find Insurance Properties
    Private m_nInsuranceFilecnt As Integer
    Private m_sPolicyNumber As String = ""
    Private m_sPolicyCode As String = ""
    Private m_sRiskIndex As String = ""
    Private m_dtClaimDate As Object
    Private m_sShortName As String = ""
    Private m_sPostCode As String = ""
    Private m_dtFromDate As Object
    Private m_dtToDate As Object
    Private m_sPolicyHolder As String = ""
    Private m_sProductCode As String = ""
    'S4B ClaimsEnhancements R&D 2005
    Private m_sClientName As String = ""

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields



    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRFindInsurance.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oInsuranceFile As Object
    Private m_oParty As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

    'JMK 11/04/2001 stores GIS Search
    Public vGISSearchDataArray As Object

    Private m_iSortKey As Integer
    Private m_iDirection As SortOrder

    Private m_vSourceArray As Object ' MKW 190503 PN2032 START

    Private m_bDPAIsActive As Boolean
    Private m_bDPAIsEnforced As Boolean


    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.2.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean

    Private hScrollValue As Integer = 0
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0



    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
        End Set
    End Property


    Public Property EnablePartialWildcardSearchOption() As Boolean
        Get
            Return m_bEnablePartialWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bEnablePartialWildcardSearchOption = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.2.1)


    ' MKW 190503 PN2032 START
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user


            m_vSourceArray = Value

        End Set
    End Property
    ' MKW 190503 PN2032 END

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
    'DC020701 was Integer now Long
    'DC020701 was Integer now Long
    Public Property InsuranceFilecnt() As Integer
        Get

            'Return Insurance File cnt
            Return m_nInsuranceFilecnt

        End Get
        Set(ByVal Value As Integer)

            'Set Insurance File cnt
            m_nInsuranceFilecnt = Value

        End Set
    End Property
    Public Property PolicyNumber() As String
        Get

            'Return Insurance Policy Number
            Return m_sPolicyNumber

        End Get
        Set(ByVal Value As String)

            'Set Insurance Policy Number
            m_sPolicyNumber = Value

        End Set
    End Property

    Public Property PolicyHolder() As String
        Get

            'Return Client Name
            Return m_sPolicyHolder

        End Get
        Set(ByVal Value As String)

            'Set Client Name
            m_sPolicyHolder = Value

        End Set
    End Property

    Public Property RiskIndex() As String
        Get

            'Return RiskInex Property
            Return m_sRiskIndex


        End Get
        Set(ByVal Value As String)

            'Set RiskIndex property
            m_sRiskIndex = Value


        End Set
    End Property

    Public Property ProductCode() As String
        Get

            'Return the Product Code
            Return m_sProductCode

        End Get
        Set(ByVal Value As String)

            'Set the Product Code
            m_sProductCode = Value

        End Set
    End Property


    Public Property FromDate() As Object
        Get

            'Return Claim Start Date
            Return m_dtFromDate

        End Get
        Set(ByVal Value As Object)

            'Set Claim Start Date


            m_dtFromDate = Value

        End Set
    End Property
    Public Property ClaimDate() As Object
        Get

            'Return Claim  Date
            Return m_dtClaimDate

        End Get
        Set(ByVal Value As Object)

            'Set Claim  Date


            m_dtClaimDate = Value

        End Set
    End Property


    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu

    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim dtClaimDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.
            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.

            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Is Claim from date null?
            If Information.IsDate(m_dtFromDate) Then



                m_dtFromDate = StringsHelper.Format(m_dtFromDate, ACShortDate)

            End If

            'Is Claim to date null?
            If Information.IsDate(m_dtToDate) Then



                m_dtToDate = StringsHelper.Format(m_dtToDate, ACShortDate)

            End If

            'Set Claim Date

            If Information.IsDate(m_dtClaimDate) Then



                m_dtClaimDate = StringsHelper.Format(m_dtClaimDate, ACShortDate)

            End If
            ' Get the details from the business object.

            'TN20001204 - Start

            'JMK 10/04/2001 - add search by GIS Risk Index (search only if there's a value)
            'TN20010426        Dim vTempData As Variant
            If txtRiskIndex.Text.Trim() <> "" Then
                DisplayStatusSearching()

                m_lReturn = ValidateIndex()
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'Assign Values to Interface
                    m_lReturn = DataToInterface()
                End If

                DisplayStatusFound()

                Return result
            Else
                ' Alix - 17/02/2004 - Add claim date parameter
                dtClaimDate = CDate(txtClaimDate.Text)

                m_lReturn = g_oBusiness.GetUWPolicyList(r_vResultArray:=m_vSearchData, v_vPolicyNo:=txtPolicyNumber.Text.Trim, v_vPartyShortName:=txtShortName.Text, v_vPostCode:=txtPostcode.Text, v_vPolicyStartDate:=m_dtFromDate, v_vPolicyEndDate:=m_dtToDate, v_vClaimDate:=dtClaimDate, v_bLimitResults:=True, v_lCoverNoteSheetNumber:=gPMFunctions.ToSafeInteger(txtCoverNoteSheetNo.Text), v_lNumberofRecords:=500)
            End If


            'Assign Values to Interface
            m_lReturn = DataToInterface()


            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.

                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ValidateIndex
    '
    ' Description: Validates the interface index.
    '
    '   JMK 10/04/2001 amended copy of Back Office Gis search
    ' ***************************************************************** '
    Private Function ValidateIndex() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sIndex As String = ""
        Dim dtClaimDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sIndex = txtRiskIndex.Text.Trim()


            lReturn = g_oBusiness.FindLikeIndex(sIndex:=sIndex, lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=vGISSearchDataArray, sInsuranceRef:=txtPolicyNumber.Text.Trim)

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Information.IsArray(vGISSearchDataArray) Then

                ''PSL 21/02/2003 Issue 2403 If there is no matching Risk Index then
                'Just return no matches

                If Not Information.IsArray(vGISSearchDataArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Else
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetAllGISSearchResults", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                dtClaimDate = CDate(txtClaimDate.Text)

                ' We have the Insurance Refs, Insurance File Cnt

                lReturn = g_oBusiness.GetUWPolicyByGISSearchIndex(vGISSearchDataArray, m_vSearchData, v_vPolicyNo:=txtPolicyNumber.Text.Trim, v_vPartyShortName:=txtShortName.Text, v_vPostCode:=txtPostcode.Text, v_vPolicyStartDate:=m_dtFromDate, v_vPolicyEndDate:=m_dtToDate, v_vClaimDate:=dtClaimDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetUWPolicyByGISSearchIndex failed to get Policy Details", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO Indexes were found return Not Found
                If Not Information.IsArray(m_vSearchData) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate index", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem


        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If
            Dim bFlag As Boolean
            Dim lstRowNo As Integer = 0
            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Assign the details to the first column.



                'TN20010426 Start - repositioning column position and add extra columns for party name and address1


                'developer guide no.242
                'For lstRowNoDup As Integer = 0 To lvwSearchDetails.Items.Count - 1
                '    If lvwSearchDetails.Items.Count > 0 AndAlso lvwSearchDetails.Items(lstRowNoDup).SubItems(9).Text.Trim = CStr(m_vSearchData(ACIInsuranceFilecnt, lRow)).Trim() Then
                '        bFlag = True
                '    End If
                'Next lstRowNoDup
                lstRowNo = 0

                While lstRowNo <= lvwSearchDetails.Items.Count - 1
                    If lvwSearchDetails.Items.Count > 0 AndAlso lvwSearchDetails.Items(lstRowNo).SubItems(9).Text.Trim = CStr(m_vSearchData(ACIInsuranceFilecnt, lRow)).Trim() Then
                        bFlag = True
                    ElseIf lvwSearchDetails.Items(lstRowNo).SubItems(3).Text.Trim = CStr(m_vSearchData(ACIPolicyNumber, lRow)).Trim() AndAlso Convert.ToDateTime(CStr(m_vSearchData(ACIUFromDate, lRow))) > Convert.ToDateTime(lvwSearchDetails.Items(lstRowNo).SubItems(6).Text) Then
                        lvwSearchDetails.Items.RemoveAt(lstRowNo)
                        lvwSearchDetails.Refresh()
                        lstRowNo = -1
                    ElseIf lvwSearchDetails.Items(lstRowNo).SubItems(3).Text.Trim = CStr(m_vSearchData(ACIPolicyNumber, lRow)).Trim() AndAlso Convert.ToDateTime(CStr(m_vSearchData(ACIUFromDate, lRow))) < Convert.ToDateTime(lvwSearchDetails.Items(lstRowNo).SubItems(6).Text) Then
                        bFlag = True
                    End If
                    lstRowNo = lstRowNo + 1
                End While
                If bFlag = False Then

                    lvwSearchDetails.Items.ContainsKey(CStr(m_vSearchData(ACIInsuranceFilecnt, lRow)).Trim())


                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIPolicyHolder, lRow)).Trim(), ACFindImage)

                    oListItem.SubItems.Add(1).Text = CStr(m_vSearchData(ACIUPartyName, lRow)).Trim()

                    oListItem.SubItems.Add(2).Text = CStr(m_vSearchData(ACIUAddress1, lRow)).Trim()

                    oListItem.SubItems.Add(3).Text = CStr(m_vSearchData(ACIPolicyNumber, lRow)).Trim()

                    If Convert.IsDBNull(m_vSearchData(ACIURiskIndex, lRow)) Or IsNothing(m_vSearchData(ACIURiskIndex, lRow)) Then
                        oListItem.SubItems.Add(4).Text = ""
                    Else
                        oListItem.SubItems.Add(4).Text = CStr(m_vSearchData(ACIURiskIndex, lRow)).Trim()
                    End If

                    oListItem.SubItems.Add(5).Text = CStr(m_vSearchData(ACIUProductCode, lRow)).Trim()

                    oListItem.SubItems.Add(6).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACIUFromDate, lRow)))

                    oListItem.SubItems.Add(7).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACIUToDate, lRow)))

                    ListViewHelper.GetListViewSubItem(oListItem, 7).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACIURenewalDate, lRow)))

                    oListItem.SubItems.Add(8).Text = CStr(m_vSearchData(ACIUPostCode, lRow)).Trim()

                    oListItem.SubItems.Add(9).Text = CStr(m_vSearchData(ACIInsuranceFilecnt, lRow)).Trim()


                    ' Set the tag property with the index of
                    ' the search data storage.
                    oListItem.Tag = CStr(lRow)

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwSearchDetails.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwSearchDetails.Refresh()
                    End If
                End If
                bFlag = False
            Next lRow
            lvwSearchDetails.Columns(9).Width = 0
            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Const ReturnCode_Error As Integer = 0
        Const ReturnCode_Ok As Integer = 1
        Const ReturnCode_TooEarly As Integer = 2
        Const ReturnCode_TooLate As Integer = 3
        Const ReturnCode_Voided As Integer = 4

        Dim iMsgButtons As MsgBoxStyle
        Dim lSelectedItem As Integer
        Dim dtClaimDate, dtStartDate As Date
        Dim lReturnCode As Integer
        Dim sMessage As String = ""
        Dim lRenewalStatus As Integer
        Dim lReturn As DialogResult

        Dim obPMUPolicy As Object
        Dim r_oResult(,) As Object
        Dim r_bIsPendingPortfolioTransfer As Boolean
        Dim r_bIsPendingCloneTransfer As Boolean
        Dim sInsuranceRef As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.


            ' Check branch is not closed, and if it is, make sure claims are allowed
            If CStr(m_vSearchData(ACIUIsSourceClosed, lSelectedItem)) = "1" Then
                If CStr(m_vSearchData(ACIUClaimsAllowed, lSelectedItem)) = "0" Then
                    ' The branch is closed, and claims are not allowed, exit
                    sMessage = "Claims cannot be opened against this policy because it is associated to a closed branch."
                    MessageBox.Show(sMessage, "Branch is closed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Get selected insurance file count
            m_nInsuranceFilecnt = CInt(m_vSearchData(ACIInsuranceFilecnt, lSelectedItem))
            sInsuranceRef = CStr(Trim(m_vSearchData(ACIPolicyNumber, lSelectedItem)))

            ' Alix - 16/02/2004 - Get correct version of policy for claim date
            dtClaimDate = DateTime.Today
            If txtClaimDate.Text <> "" Then
                dtClaimDate = CDate(txtClaimDate.Text)
            End If



            m_lReturn = g_oObjectManager.GetInstance(oObject:=obPMUPolicy,
                                sClassName:="bPMUPolicy.Business",
                                vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to set the Party UIK.",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="DataToProperties",
                                   vErrNo:=Err.Number,
                                   vErrDesc:=Err.Description)
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = obPMUPolicy.IsPendingPortfolioTransfer(sInsuranceFileRef:=sInsuranceRef,
                                                                   r_oResult:=r_oResult,
                                                                   r_bIsPendingPortfolioTransfer:=r_bIsPendingPortfolioTransfer,
                                                                   r_bIsPendingCloneTransfer:=r_bIsPendingCloneTransfer)

            obPMUPolicy.Dispose()

            If IsArray(r_oResult) Or r_bIsPendingPortfolioTransfer Then
                MsgBox("Pending Portfolio Transfer.", vbExclamation + vbOKOnly, "Pending portfolio transfer")
                Return PMEReturnCode.PMFalse
            ElseIf r_bIsPendingCloneTransfer Then
                MsgBox("Pending Clone Transfer.", vbExclamation + vbOKOnly, "Pending portfolio transfer")
                Return PMEReturnCode.PMFalse
            End If


            result = g_oBusiness.GetPolicyForClaimDate(v_dtClaimDate:=dtClaimDate, r_lInsuranceFileCnt:=m_nInsuranceFilecnt, r_sPolicyNumber:=m_sPolicyNumber, r_dtStartDate:=dtStartDate, r_lReturnCode:=lReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Loss Date against policy cover dates.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            iMsgButtons = MsgBoxStyle.YesNo
            ' Alix - 16/02/2004 - Process 52/53

            Select Case (lReturnCode)
                Case ReturnCode_Ok
                    ' We found a valid policy version for this claim date, carry on as normal
                Case ReturnCode_TooEarly
                    ' We failed to find a valid policy version because the claim date is earlier
                    ' than the start date of the earliest policy version

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossDateBefore, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case ReturnCode_TooLate
                    ' We failed to find a valid policy version because the claim date is later
                    ' than the expiry date of the latest policy version

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossDateAfter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case ReturnCode_Voided
                    ' We found a valid policy version regarding the dates, but it was voided

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyVoided, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    iMsgButtons = MsgBoxStyle.OkOnly
                Case ReturnCode_Error
                    ' We couldn't find a valid policy for an unexpected reason

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoPolicyFound, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End Select

            ' Report to user
            If m_vSearchData(ACIInsuranceFileStatus, lSelectedItem) = "CAN" AndAlso
                CDate(m_vSearchData(ACILapseDate, lSelectedItem)) <= dtClaimDate Then
                If MessageBox.Show("Policy Cancelled with effect from " & Format(CDate(m_vSearchData(ACILapseDate, lSelectedItem)), "dd/MM/yyyy") & ". Do you wish to proceed?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            ElseIf sMessage <> "" Then

                lReturn = Interaction.MsgBox(sMessage & Strings.Chr(13) & Strings.Chr(10) & " Do you wish to continue?", MsgBoxStyle.Exclamation + iMsgButtons, "Loss Date Information")
                If iMsgButtons = MsgBoxStyle.YesNo Then
                    If lReturn = System.Windows.Forms.DialogResult.No Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                ElseIf iMsgButtons = MsgBoxStyle.OkOnly Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_dtFromDate = dtStartDate


            If Convert.IsDBNull(m_vSearchData(ACIURiskIndex, lSelectedItem)) Or IsNothing(m_vSearchData(ACIURiskIndex, lSelectedItem)) Then
                m_sRiskIndex = ""
            Else
                m_sRiskIndex = CStr(m_vSearchData(ACIURiskIndex, lSelectedItem)).Trim()
            End If

            m_sProductCode = CStr(m_vSearchData(ACIUProductCode, lSelectedItem)).Trim()

            m_dtFromDate = CStr(m_vSearchData(ACIUFromDate, lSelectedItem)).Trim()

            'Warn the user if the Policy is in Renewal

            result = g_oBusiness.CheckInRenewal(m_nInsuranceFilecnt, lRenewalStatus)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(result.ToString() + ", " + +", CheckInRenewal call failed.")
            End If

            If lRenewalStatus <> -1 Then
                MessageBox.Show("Warning: This Policy is currently in Renewal.", "Policy in Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    ' Date:11/07/00
    '
    ' Edit History: Pandu
    ' RAM20030401 : Disable the ShortName Field and cmdRelatedPartyFind if
    '                the shortname is passed in. Normally from Client Manager -->
    '                GoTo --> OpenClaim
    '               Ref. Issue 2670
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            txtPolicyNumber.Text = m_sPolicyNumber.Trim()

            '*****************
            ' MEvans : 12-03-2003 : Issue 2357
            ' set the shortname so we find policies for the
            ' required party only...
            txtShortName.Text = m_sPolicyHolder.Trim()
            '*****************

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030401 : Disable the ShortName Field and cmdRelatedPartyFind
            '               Ref. Issue 2670 - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If m_sPolicyHolder.Trim().Length > 0 Then
                ' We have a short name, so we can disable the controls
                txtShortName.Enabled = False
                cmdRelatedPartyFind.Enabled = False
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030401 - END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            lvwSearchDetails.Columns.Insert(Column1 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column2 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column3 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column4 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column5 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column6 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column7 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column8 - 1, "", 94)
            lvwSearchDetails.Columns.Insert(Column9 - 1, "", 94)

            ' Set the column widths for the search list.



            lvwSearchDetails.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column3 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column5 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column6 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column7 - 1).Width = CInt(0)
            lvwSearchDetails.Columns.Item(Column8 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwSearchDetails.Columns.Item(Column9 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))


            'TN20010509 Start
            'set default to selecting whole row in listview
            'developer guide no.303
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=Me.lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwSearchDetails.FullRowSelect = True

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'TN20010509 End


            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Set the status of the Navigate button.
            '    Select Case (m_lNavigate&)
            '        Case PMNavigateEnabled
            '            cmdNavigate.Visible = True
            '            cmdNavigate.Enabled = True
            '
            '        Case PMNavigateDisabled
            '            cmdNavigate.Visible = True
            '            cmdNavigate.Enabled = False
            '
            '        Case Else
            '            cmdNavigate.Visible = False
            '    End Select

            '    ' Position View control
            '    If (cmdNavigate.Visible = False) Then
            '        cmdView.Left = cmdNavigate.Left
            '    Else
            '        cmdView.Left = cmdNavigate.Left = cmdNavigate.Width + 105
            '    End If
            '
            ' Disable until a policy is selected

            'cmdView.Enabled = False

            'Do we need to show the Data Protection Act questions
            m_lReturn = GetDPASetting()

            If Not m_bDPAIsActive Then
                ' Hide DPA stuff
                chkDPARequired.Visible = False
                lblDPARequired.Visible = False
                chkDPARequired.CheckState = CheckState.Unchecked
            Else
                ' Display DPA stuff
                chkDPARequired.Visible = True
                lblDPARequired.Visible = True

                If m_bDPAIsEnforced Then
                    chkDPARequired.CheckState = CheckState.Checked
                    chkDPARequired.Enabled = False
                Else
                    chkDPARequired.CheckState = CheckState.Unchecked
                End If

            End If

            lblPolicyNumber.Top = chkDPARequired.Top
            txtPolicyNumber.Top = chkDPARequired.Top

            'UW -no need for Find Party on 1st tab
            cmdRelatedPartyFind2.Visible = False
            txtShortname2.Visible = False

            'S4B ClaimsEnhancements R&D 2005
            lblClientName.Visible = False
            txtClientName.Visible = False

            'DC261001
            lblExcLapsed.Visible = False
            chkExcLapsed.Visible = False

            'S4B ClaimsEnhancements R&D 2005
            lblclaimdate.Left = lblClientName.Left
            lblclaimdate.Top = lblClientName.Top

            txtClaimDate.Left = txtClientName.Left
            txtClaimDate.Top = txtClientName.Top



            'Write the properties to list item or textboxes
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            txtClaimDate.Tag = CStr(True)
            txtFromDate.Tag = CStr(True)
            txtToDate.Tag = CStr(True)

            'DC251001 setting default for Broking
            '           was taken out of previous version


            m_dtClaimDate = ""
            txtClaimDate.Text = ""



            'Set Default From and ToDate to Null

            m_dtFromDate = ""


            m_dtToDate = ""

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            _stbstatus_Panel1.Text = ""

            'Clear all other Controls on the form
            txtPolicyNumber.Text = ""

            'txtPolicyCode.Text = ""

            txtClaimDate.Text = ""

            txtRiskIndex.Text = ""

            txtShortName.Text = ""

            'DC251001
            txtShortname2.Text = ""

            txtPostcode.Text = ""

            txtFromDate.Text = ""

            txtToDate.Text = ""

            'S4B ClaimsEnhancements R&D 2005
            txtClientName.Text = ""

            txtCoverNoteSheetNo.Text = ""

            m_dtClaimDate = DateTime.Today


            m_dtFromDate = ""


            m_dtToDate = ""

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Set focus to the search details.
            txtPolicyNumber.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' Disable parts of the interface, so the
            ' user can now only enter a new search

            m_lReturn = DisableInterface(bDisable:=True)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 1)

            ' Set the first and last data entry controls for
            ' all of the tabs.


            m_ctlTabFirstLast(ACControlStart, 0) = txtPolicyNumber
            'DC251001 -was txtPolicyNumber
            m_ctlTabFirstLast(ACControlEnd, 0) = txtClaimDate

            'DC251001 -added check for UW and Broking as prompts different

            '
            m_ctlTabFirstLast(ACControlStart, 1) = txtShortName
            'DC251001 -was txtShortName
            m_ctlTabFirstLast(ACControlEnd, 1) = txtToDate


            Return result

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
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.15.1.1)
        Dim result As Integer = 0
        Dim iLanguageId As Integer
        'In all instances where the GetResData function is called the g_iLanguageID% is replaced with iLanguageId%
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.15.1.1)
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.15.1.1)
            m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.15.1.1)
            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            'Caption of OK Button

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Cancel Button

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Help Button

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            '
            '    'Caption of Navigate Button
            '    cmdNavigate.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACNavigateButton, _
            ''        iDataType:=PMResString)
            '
            '    'Caption of View Button
            '    cmdView.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACViewButton, _
            ''        iDataType:=PMResString)
            '
            'Caption of FindNow Button

            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of NewSearch Button

            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of First Tab

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Second Tab

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Default Tab is First Tab
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'Caption of Column 1-Policy Holder in List View


            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 2 -Policy Reference in List View


            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))





            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleClientName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 3 -RiskIndex in List View


            lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleAddress1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 4 -Product Code in List View


            lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Caption of Column 5 - From Date in List View


            lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 6 - To Date in List View


            lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 7 - Post Code in List View


            lvwSearchDetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 8
            '        lvwSearchDetails.ColumnHeaders(8).Text = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACListTitle6, _
            'iDataType:=PMResString)



            lvwSearchDetails.Columns.Item(7).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitleRenewalDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Column 9


            lvwSearchDetails.Columns.Item(8).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Caption of Label -Policy Number

            lblPolicyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    'Caption of Label -Policy Code
            '    lblPolicyCode.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACPolicyCode, _
            ''        iDataType:=PMResString)

            'Caption of Label -Risk Index

            lblRiskIndex.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRiskIndex, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Label -Claim Date

            lblclaimdate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACClaimDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Label -Short Name

            cmdRelatedPartyFind.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACShortName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC251001 -same as above
            'Caption of Label -Short Name

            cmdRelatedPartyFind2.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACShortName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'DC251001

            'Caption of Label -Post Code

            lblPostCode.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Label -Loss From Date

            lblInForceFromDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACFromDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Caption of Label -Loss To Date

            lblInForceTodate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



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
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            'cmdView.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu

    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbstatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbstatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu

    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data

            If txtClaimDate.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPolicyNumber.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            '    If (Trim$(txtPolicyCode.Text) <> "") Then
            '        CheckMandatory = PMTrue
            '    End If

            If txtShortName.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            'DC251001
            If txtShortname2.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPostcode.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtRiskIndex.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtFromDate.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtToDate.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            'S4B Claim Enhancements R&D 2005
            If txtClientName.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtCoverNoteSheetNo.Text.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1425)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1425)

            ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(1035)

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1650)

            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(3840) - 10

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1140)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1140)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1140)

            'cmdView.Top = Me.Height - 1110

            'cmdNew.Top = Me.Height - 1110
            'cmdEdit.Top = Me.Height - 1110

            '    If (cmdNavigate.Visible = True) Then
            '        cmdNavigate.Top = Me.Height - 1110
            '    End If

        Catch
        End Try





        ' Error Section.


        Return gPMConstants.PMEReturnCode.PMError

    End Function
    ' PRIVATE Methods (End)

    Private Sub chkDPARequired_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDPARequired.CheckStateChanged
        If chkDPARequired.CheckState = CheckState.Checked Then
            lblDPARequired.Text = "Yes"
        Else
            lblDPARequired.Text = "No"
        End If
    End Sub

    Private Sub cmdRelatedPartyFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRelatedPartyFind.Click

        'Show Find Client Interface
        m_lReturn = GetPolicyHolderInfo(sShortName:=txtShortName.Text)

        'DC251001
        ' Display Agent on form
        txtShortName.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPolicyHolder.Trim())


    End Sub

    'DC251001 -start
    Private Sub cmdRelatedPartyFind2_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRelatedPartyFind2.Click

        'Show Find Client Interface
        m_lReturn = GetPolicyHolderInfo(sShortName:=txtShortname2.Text)

        'DC251001
        ' Display Agent on form
        txtShortname2.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPolicyHolder.Trim())


    End Sub
    'DC251001 -end

    ' ***************************************************************** '
    ' Name: FormIntialise
    '
    ' Description: Intialise all required details of the form
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRFindInsurance.General()

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



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: FormLoad
    '
    ' Description: Intialise all required details of the form
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

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

            'Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Set the UnderWriting/Broking Constant

            m_lSiriusUnderWritingBroking = g_oBusiness.Sirius_Product

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse


                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check if the search contains more or equal
            ' to the miniumum search length.


            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' No supplied data so cannot
                ' continue with the search.

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            '******* BugID-9 Start Of Code of Change to Stop Populating Details on Load

            ' Gets the interface details to be displayed.
            'm_lReturn& = m_oGeneral.GetInterfaceDetails()

            '******* BugID-9 End Of Code of Change to Stop Populating Details on Load


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
    ' ***************************************************************** '
    ' Name: Form_Query Unload
    '
    ' Description: Store all Property Details before unloading form
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide no.7
            If UnloadMode <> vbFormCode Then

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
    ' ***************************************************************** '
    ' Name:Form_KeyDown
    '
    ' Description: Determine the Position of Tab and Control on
    '              pressing pageup,pagedown,home,end buttons
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
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
            'developer guide no.293
            'start
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If

            'end
        Catch


            ' Error Section.

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name:Form_Resize
    '
    ' Description: Resize the the controls on form
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = ResizeInterface()

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lblclaimdate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblclaimdate.Click

        txtClaimDate.Focus()

        Exit Sub

    End Sub

    Private Sub lblInForceFromDate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblInForceFromDate.Click

        txtFromDate.Focus()

        Exit Sub

    End Sub

    Private Sub lblInForceTodate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblInForceTodate.Click

        txtToDate.Focus()

        Exit Sub

    End Sub

    Private Sub lblPolicyNumber_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblPolicyNumber.Click

        txtPolicyNumber.Focus()

        Exit Sub

    End Sub

    Private Sub lblPostCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblPostCode.Click

        txtPostcode.Focus()

        Exit Sub

    End Sub

    Private Sub lblRiskIndex_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblRiskIndex.Click

        txtRiskIndex.Focus()

        Exit Sub

    End Sub

    ' ***************************************************************** '
    ' Name:lvwSearchDetails_Click
    '
    ' Description:Fill the Policy Number in Text Box for the listitem clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sIndex As Integer

        If lvwSearchDetails.Items.Count > 0 Then

            sIndex = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

            ' stick the other details in here...?

            sIndex = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

            txtPolicyNumber.Text = CStr(m_vSearchData(ACIPolicyNumber, sIndex))


            VB6.SetDefault(cmdOK, True)

            ' Activate View button
            'cmdView.Enabled = True

        End If

    End Sub

    ' ***************************************************************** '
    ' Name:lvwSearchDetails_KeyDown
    '
    ' Description:Set Command Button Ok as Not Default on Pressing Enter Key
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
        End If

    End Sub

    ' ***************************************************************** '
    ' Name:lvwSearchDetails_KeyPress
    '
    ' Description:Fill the Policy Number in Text Box when enter button is
    '               pressed when focus is  on list item
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Dim sIndex As Integer
        If KeyAscii = 13 Then


            If lvwSearchDetails.Items.Count > 0 Then


                sIndex = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)


                ' stick the other details in here...?

                sIndex = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

                txtPolicyNumber.Text = CStr(m_vSearchData(ACIPolicyNumber, sIndex))


                VB6.SetDefault(cmdOK, True)

                ' Activate View button
                'cmdView.Enabled = True


            End If


        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: tabMainTab_Click
    '
    ' Description:Set the Focus on the First control on the relevant Tab Clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab

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
    ' ***************************************************************** '
    ' Name: cmdOK_Click
    '
    ' Description:Set Properties of the form on clicking OK Button from the
    '               relevant list item under focus or clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bProceed As Boolean
        Dim lSelectedItem, lInsuranceFileCnt, lPartyCnt As Integer
        Dim sPartyType As String = ""

        Try

            'Do we need to show the Data Protection Act questions
            If m_bDPAIsActive And chkDPARequired.CheckState = CheckState.Checked Then


                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
                lInsuranceFileCnt = CInt(m_vSearchData(ACIInsuranceFilecnt, lSelectedItem))

                m_lReturn = GetPartyDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lPartyCnt:=lPartyCnt, r_sPartyTypeCode:=sPartyType)

                'If FSA compliance is enabled then check why the user is viewing the client
                m_lReturn = ProcessFSAAccess(lPartyCnt:=lPartyCnt, sPartyType:=sPartyType, bProceed:=bProceed)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed FSA Customer Validate.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'User is not proceeding
                If Not bProceed Then
                    Exit Sub
                End If

                'Store this setting so that we can read it when using the recent files in CM
                m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ShowFSA", v_sSettingValue:="1")

            Else

                'Store this setting so that we can read it when using the recent files in CM
                m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ShowFSA", v_sSettingValue:="0")

            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
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
    ' ***************************************************************** '
    ' Name: cmdCancel_Click
    '
    ' Description:Unload the Form
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
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
    ' ***************************************************************** '
    ' Name: cmdFindNow_Click
    '
    ' Description:Get the Details from Bussiness Object
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        Dim sWildcardErrorMessage As String = ""
        m_vSearchData = Nothing
        lvwSearchDetails.Items.Clear()

        Try

            Application.DoEvents()

            If CBool(Convert.ToString(txtClaimDate.Tag)) Then


                If CStr(m_dtClaimDate) <> "" Then

                Else

                    If txtClaimDate.Text <> "" Then


                        If Information.IsDate(txtClaimDate.Text.Trim()) Then


                            m_dtClaimDate = StringsHelper.Format(txtClaimDate.Text.Trim(), ACShortDate)

                        Else

                            DisplayMessage(ACInvalidDateMsg, Mid(lblclaimdate.Name, 4))

                            txtClaimDate.Text = ""

                            txtClaimDate.Focus()

                            Exit Sub

                        End If

                    End If

                End If

            End If

            If CBool(Convert.ToString(txtFromDate.Tag)) Then

                If txtFromDate.Text <> "" Then


                    If Information.IsDate(txtFromDate.Text.Trim()) Then


                        m_dtFromDate = StringsHelper.Format(txtFromDate.Text.Trim(), ACShortDate)

                    Else

                        DisplayMessage(ACInvalidDateMsg, Mid(lblInForceFromDate.Name, 4))

                        txtFromDate.Text = ""

                        'm_dtFromDate = ""

                        txtFromDate.Focus()

                        Exit Sub

                    End If

                End If

            End If

            If CBool(Convert.ToString(txtToDate.Tag)) Then

                If txtToDate.Text <> "" Then


                    If Information.IsDate(txtToDate.Text.Trim()) Then


                        m_dtToDate = StringsHelper.Format(txtToDate.Text.Trim(), ACShortDate)

                    Else

                        DisplayMessage(ACInvalidDateMsg, Mid(lblInForceTodate.Name, 4))

                        txtToDate.Text = ""

                        'm_dtToDate = ""

                        txtToDate.Focus()

                        Exit Sub

                    End If

                End If

            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Is From Date Greater than to Date?



            If CStr(m_dtFromDate) <> "" And CStr(m_dtToDate) <> "" Then

                m_lReturn = CheckDateDiff(m_dtFromDate, m_dtToDate)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                    DisplayMessage(ACDateDiffError, "Date Difference Error")

                    txtFromDate.Focus()

                    Exit Sub

                End If

            End If


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)
            'Check wildcard searches

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPolicyNumber.Text.Trim, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
                txtPolicyNumber.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtRiskIndex.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
                txtRiskIndex.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtClientName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
                txtClientName.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtShortName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
                txtShortName.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtPostcode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Insurance")
                txtPostcode.Focus()
                Exit Sub

            End If

            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            If lvwSearchDetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)


                ListViewHelper.SetSortKeyProperty(lvwSearchDetails, 3)
                ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)

            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdNewSearch_Click
    '
    ' Description:Clear all controls on the form
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdNavigate_Click
    '
    ' Description:Move to next form in the road map
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '



    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Cancel button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions.
    'm_lReturn = m_oGeneral.ProcessCommand()
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: lvwSearchDetails_DblClick
    '
    ' Description:Move to the next form in the road map
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: lvwSearchDetails_GotFocus
    '
    ' Description:Set Ok Button a default
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: lvwSearchDetails_lostfocus
    '
    ' Description:Set find now as default
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: lvwSearchDetails_ColumnClick
    '
    ' Description:Sort the Details of List View as per the column clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick





        Dim lvwSelectedItem As ListViewItem = Nothing

        If lvwSearchDetails.SelectedItems IsNot Nothing AndAlso lvwSearchDetails.SelectedItems.Count > 0 Then
            lvwSelectedItem = lvwSearchDetails.SelectedItems(0)
        End If
        StoreHScrollValue()
        ListViewFunc.SortListView(lvwSearchDetails, eventArgs)
        RecoverHorizontalScroll()
        If lvwSelectedItem IsNot Nothing Then
            lvwSelectedItem.Selected = True
            lvwSelectedItem.EnsureVisible()
        End If

    End Sub
    ' PRIVATE Events (End)
    <DllImport("user32.dll")>
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function

    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwSearchDetails.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwSearchDetails.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub

    Private Sub txtClaimDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

        If txtClaimDate.Text.Trim() = Nothing Then

            txtClaimDate.Tag = CStr(True)


            m_dtClaimDate = ""
        End If

        If Information.IsDate(txtClaimDate.Text.Trim()) Then


            m_dtClaimDate = StringsHelper.Format(txtClaimDate.Text.Trim(), ACShortDate)

            txtClaimDate.Tag = CStr(True)

        End If


    End Sub

    Private Sub txtClaimDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtClaimDate)

        '    SelectText txtClaimDate

        '    If txtClaimDate.Text <> "" Then
        '
        '        txtClaimDate.Text = Format(m_dtClaimDate, ACShortDate)
        '
        '    End If

        '    If Trim(txtClaimDate.Text) = vbNullString Then
        '
        '        txtClaimDate.Text = Format(Date, ACShortDate)
        '
        '    End If

        'm_dtClaimDate = ""

    End Sub

    Private Sub txtClaimDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimDate.Leave

        Dim sDisplayText As String = ""

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClaimDate)

        If Information.IsDate(txtClaimDate.Text) Then
            'cmdFindNow_Click
        End If

        '    If txtClaimDate.Text <> "" Then
        '
        '    'Check Date is Valid or Not
        '
        '    m_lReturn = CheckValiddate(txtClaimDate.Text, 0, sDisplayText)
        '
        '        If m_lReturn = PMTrue Then
        '
        '            txtClaimDate.Text = sDisplayText
        '
        '        Else
        '            txtClaimDate.Text = sDisplayText
        '
        '            'Display Invalid Date Message
        '            Call DisplayMessage(ACInvalidDateMsg, Mid(lblclaimdate.Name, 4))
        '
        '            txtClaimDate.SetFocus
        '
        '        End If
        '
        '    End If

    End Sub

    Private Sub txtClientName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClientName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'S4B Claim Enhancements R&D 2005
        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtCoverNoteSheetNo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCoverNoteSheetNo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub txtCoverNoteSheetNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCoverNoteSheetNo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If Not Strings.Chr(KeyAscii).ToString() Like "['0-9']" And KeyAscii <> 8 Then
            KeyAscii = 0
        ElseIf StringsHelper.ToDoubleSafe(Strings.Chr(KeyAscii).ToString()) = 0 Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFromDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)


        If txtFromDate.Text.Trim() = Nothing Then

            txtFromDate.Tag = CStr(True)


            m_dtFromDate = ""

        End If

        If Information.IsDate(txtFromDate.Text.Trim()) Then


            m_dtFromDate = StringsHelper.Format(txtFromDate.Text.Trim(), ACShortDate)

            txtFromDate.Tag = CStr(True)

        End If

    End Sub

    Private Sub txtFromDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFromDate.Enter

        If txtFromDate.Text.Trim() <> "" Then


            txtFromDate.Text = StringsHelper.Format(m_dtFromDate, ACShortDate)

        End If

        'm_dtFromDate = " "


    End Sub

    Private Sub txtFromDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFromDate.Leave

        Dim sDisplayText As String = ""

        If txtFromDate.Text <> "" Then

            m_lReturn = CheckValiddate(txtFromDate.Text, 1, sDisplayText)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                txtFromDate.Text = sDisplayText

            Else
                txtFromDate.Text = sDisplayText

                DisplayMessage(ACInvalidDateMsg, Mid(lblInForceFromDate.Name, 4))

                txtFromDate.Focus()

            End If

        End If


    End Sub


    'Private Sub txtPolicyCode_Change()
    '
    'If User Enters data enable find now if no data is
    'available in the other controls
    '
    'cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
    '
    'End Sub

    Private Sub txtPolicyNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtPostcode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostcode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtRiskIndex_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRiskIndex.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtRiskIndex_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtRiskIndex.Validating
        Dim Cancel As Boolean = eventArgs.Cancel

        'PSL 21/02/2003 Issue 2403 Prevent Wildcard Search on Risk Index

        Dim sTitle, sMessage As String

        If txtRiskIndex.Text.IndexOf("%"c) >= 0 Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskIndexTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskIndexMessage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Cancel = True
        End If

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtShortName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    'DC251001 -start
    Private Sub txtShortname2_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortname2.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub
    'DC251001 -end

    Private Sub txtToDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtToDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

        If txtToDate.Text.Trim() = Nothing Then

            txtToDate.Tag = CStr(True)


            m_dtToDate = ""

        End If

        If Information.IsDate(txtToDate.Text.Trim()) Then


            m_dtToDate = StringsHelper.Format(txtToDate.Text.Trim(), ACShortDate)

            txtToDate.Tag = CStr(True)

        End If


    End Sub

    ' ***************************************************************** '
    ' Name: CheckValidDate
    '
    ' Description:Checks the Date Passed as Parameter is Valid or not and
    '             returns Long Date if Valid else Null String
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Public Function CheckValiddate(ByRef dtDate As String, ByRef Controlnum As Integer, ByRef sReturnValue As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsDate(dtDate) Then


            Select Case Controlnum
                Case 0

                    'm_dtClaimDate = Format(dtDate, ACShortDate)

                    txtClaimDate.Tag = CStr(False)

                Case 1

                    'm_dtFromDate = Format(dtDate, ACShortDate)

                    txtFromDate.Tag = CStr(False)

                Case 2

                    'm_dtToDate = Format(dtDate, ACShortDate)

                    txtToDate.Tag = CStr(False)

            End Select


            sReturnValue = StringsHelper.Format(dtDate, ACDateDispaly)

        Else


            Select Case Controlnum
                Case 0

                    'm_dtClaimDate = ""

                    txtClaimDate.Tag = CStr(False)

                Case 1

                    'm_dtFromDate = ""

                    txtFromDate.Tag = CStr(False)

                Case 2

                    'm_dtToDate = ""

                    txtToDate.Tag = CStr(False)

            End Select

            sReturnValue = Nothing

            result = gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result
    End Function

    Private Sub txtToDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtToDate.Enter

        If txtToDate.Text.Trim() <> "" Then


            txtToDate.Text = StringsHelper.Format(m_dtToDate, ACShortDate)

        End If

        'm_dtToDate = ""


    End Sub

    Private Sub txtToDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtToDate.Leave


        Dim sDisplayText As String = ""

        If txtToDate.Text <> "" Then

            m_lReturn = CheckValiddate(txtToDate.Text, 2, sDisplayText)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                txtToDate.Text = sDisplayText

            Else
                txtToDate.Text = sDisplayText

                DisplayMessage(ACInvalidDateMsg, Mid(lblInForceTodate.Name, 4))

                txtToDate.Focus()

            End If

        End If


    End Sub


    ' ***************************************************************** '
    ' Name: GetPolicyHolderInfo
    '
    ' Description: Instance FindParty to retrieve Policyholder
    '
    ' Date:11/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    'DC251001 -parameter now passed as can come from two places
    Public Function GetPolicyHolderInfo(ByRef sShortName As String) As Integer
        Dim result As Integer = 0
        Dim oFindParty As iPMBFindParty.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            'DC251001 -modded string
            'oFindParty.ShortName = txtShortName.Text

            oFindParty.ShortName = sShortName.Trim()

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Retrieve Party Shortname and set as Agent

            m_sPolicyHolder = oFindParty.ShortName

            ' Destroy Find Party object

            oFindParty.Dispose()
            oFindParty = Nothing

            ' Display Agent on form
            'txtShortName.Text = FormatField( _
            ''    iFormatType:=PMFormatString, _
            ''    vFieldValue:=Trim$(m_sPolicyHolder$))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyHolderInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckDateDiff
    '
    ' Description: Checks Whether claim Loss from Date is greater than to date
    '
    ' Date:11/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    Public Function CheckDateDiff(ByRef vFromDate As Object, ByRef vToDate As Object) As Integer




        Dim nDiffDays As Double = DateAndTime.DateDiff("d", CDate(vFromDate), CDate(vToDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)

        If nDiffDays < 0 Then

            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            Return gPMConstants.PMEReturnCode.PMTrue
        End If

    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' Date:11/07/2000
    '
    ' Edit History:Pandu

    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try


            'PolicyNumber
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolicyNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PolicyCode
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                   ctlControl:=txtPolicyCode, _
            ''                   lFieldType:=PMString, _
            ''                   lFormat:=PMFormatString, _
            ''                   lMandatory:=PMNonMandatory)
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RiskIndex
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRiskIndex, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ShortName
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtShortName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC251001 -start
            'ShortName2
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtShortname2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'DC251001 -end

            'Postcode
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPostcode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ClaimDate
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClaimDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'FromDate
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFromDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ToDate
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtToDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4B Claim Enhancements R&D 2005
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayMessage
    '
    ' Description: Display the status searching message.
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu

    ' ***************************************************************** '
    Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.


            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' Display the status message.

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function GetDPASetting() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bDPAIsActive = False
            m_bDPAIsEnforced = False

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDPASetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDPASetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function



    Private Function GetPartyDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPartyCnt As Integer, ByRef r_sPartyTypeCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim lPartyTypeID As Integer
        Dim vArray As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Use insurance file object to get the party cnt
            m_lReturn = CreateObjectInsuranceFile()


            m_lReturn = m_oInsuranceFile.GetDetails(vInsuranceFileCnt:=v_lInsuranceFileCnt)

            m_lReturn = m_oInsuranceFile.GetNext(r_vFieldArray:=vArray)

            r_lPartyCnt = CInt(vArray(InsuranceFileConst.ACInsuredCnt))

            'Use the party object to get the party type code
            m_lReturn = CreateObjectParty()


            m_lReturn = m_oParty.GetDetails(vPartyCnt:=r_lPartyCnt)

            m_lReturn = m_oParty.GetNext(vPartyTypeID:=lPartyTypeID)

            m_lReturn = m_oParty.GetPartyType(vPartyTypeID:=lPartyTypeID, vPartyTypeCode:=r_sPartyTypeCode)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            m_lReturn = DestroyObjectInsuranceFile()
            m_lReturn = DestroyObjectParty()



            ' This is for debugging only



        End Try
        Return result
    End Function

    Private Function CreateObjectInsuranceFile() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateObjectInsuranceFile"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFile = temp_m_oInsuranceFile

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to create object 'bSIRInsuranceFile.Business'.", gPMConstants.PMELogLevel.PMLogError)
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

    Private Function DestroyObjectInsuranceFile() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DestroyObjectInsuranceFile"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oInsuranceFile Is Nothing) Then

                m_oInsuranceFile.Dispose()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            m_oInsuranceFile = Nothing



            ' This is for debugging only



        End Try
        Return result
    End Function

    Private Function CreateObjectParty() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateObjectParty"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oParty = temp_m_oParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to create object 'bSIRParty.Business'.", gPMConstants.PMELogLevel.PMLogError)
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

    Private Function DestroyObjectParty() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DestroyObjectParty"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oParty Is Nothing) Then

                m_oParty.Dispose()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            m_oParty = Nothing



            ' This is for debugging only



        End Try
        Return result
    End Function
End Class
