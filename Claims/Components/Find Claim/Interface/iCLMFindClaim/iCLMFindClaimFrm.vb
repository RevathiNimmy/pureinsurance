Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:15/07/00
    '
    ' Description: Main interface.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '
    'Replaced iPMFunc.GetResData to GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"

    Private Const Column1 As Integer = 1
    Private Const Column2 As Integer = 2
    Private Const Column3 As Integer = 3
    Private Const Column4 As Integer = 4
    Private Const Column5 As Integer = 5
    Private Const Column6 As Integer = 6
    Private Const Column7 As Integer = 7 '2005
    Private Const Column8 As Integer = 8
    Private Const Column9 As Integer = 9
    Private Const Column10 As Integer = 10

    'Constants for Defining Width of Columns in List View

    Private Const ColWidthBroking As Integer = 1700
    Private Const ColWidthUnderWriting As Integer = 1600

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_iTask As gPMConstants.PMEComponentAction

    ' Variables for Find Claim

    Private m_lClaimCnt As Integer
    Private m_lSelectedClaimCnt As Integer
    Private m_sClaimRef As String
    Private m_lInsuranceFilecnt As Integer
    Private m_sPolicyRef As String
    Private m_sPolicyHolder As String
    Private m_sPartyType As String
    Private m_vLossFromDate As Object
    Private m_vLossToDate As Object
    Private m_bClaimStatus As Boolean
    ' CJB 240904 PN15172
    Private m_lRiskTypeId As Integer
    Private m_lPartyCnt As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMFindClaim.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oParty As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    'Modified,comment and declare in main module because of reuse
    '' Stores the search data from the business object.
    Public m_vSearchData As Object

    Private m_vSourceArray As Object ' MKW 190503 PN2032 START
    Private m_IIsComplaint As Integer

    Private m_bDPAIsActive As Boolean
    Private m_bDPAIsEnforced As Boolean

    Private m_bAskDPAQuestions As Boolean
    Private m_bSelectVersionEnabled As Boolean

    Private m_lRealClaimID As Integer
    Private m_bIncludeClosedClaims As Boolean

    Private m_bMultipleClaimPayments As Boolean
    Private m_iMaxNoofUnAuthorisedClaimPayments As Integer
    Private m_iNoofReferredPayments As Integer

    Private m_bCheckMediaTypeStatusAtClaimPayment As Boolean 'Sankar - (WPRvb64 Media Type Status) - Paralleling

    Private m_lCaseID As Integer
    Private m_bRefreshClaimData As Boolean
    'PN 58569
    Private m_bRecovery As Boolean

    Public ReadOnly Property Recovery() As Boolean
        Get
            Return m_bRecovery
        End Get
    End Property


    Public WriteOnly Property SelectVersionEnabled() As Boolean
        Set(ByVal Value As Boolean)
            m_bSelectVersionEnabled = Value
        End Set
    End Property

    Public WriteOnly Property IsComplaint() As Integer
        Set(ByVal Value As Integer)

            m_IIsComplaint = Value

        End Set
    End Property

    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user


            m_vSourceArray = Value

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


    'DC180202
    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public ReadOnly Property ClaimCnt() As Integer
        Get

            'Return Claimid
            Return m_lClaimCnt

        End Get
    End Property

    Public ReadOnly Property RealClaimID() As Integer
        Get

            Return m_lRealClaimID

        End Get
    End Property


    Public Property SelectedClaimCnt() As Integer
        Get

            Return m_lSelectedClaimCnt

        End Get
        Set(ByVal Value As Integer)

            m_lSelectedClaimCnt = Value

        End Set
    End Property

    ' CJB 240904 PN15172
    Public ReadOnly Property RiskTypeID() As Integer
        Get

            Return m_lRiskTypeId

        End Get
    End Property

    Public Property ClaimRef() As String
        Get

            'Return Claim Number
            Return m_sClaimRef

        End Get
        Set(ByVal Value As String)

            'Set Claim Number
            m_sClaimRef = Value

        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get

            'Return Insurance File Id
            Return m_lInsuranceFilecnt

        End Get
        Set(ByVal Value As Integer)

            'Set Insurance File id
            m_lInsuranceFilecnt = Value

        End Set
    End Property

    Public Property PolicyRef() As String
        Get

            'Return Policy Number
            Return m_sPolicyRef

        End Get
        Set(ByVal Value As String)

            'Set Policy Number
            m_sPolicyRef = Value

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

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property AskDPAQuestions() As Boolean
        Get
            Return m_bAskDPAQuestions
        End Get
        Set(ByVal Value As Boolean)
            m_bAskDPAQuestions = Value
        End Set
    End Property

    Public Property IncludeClosedClaims() As Boolean
        Get
            Return m_bIncludeClosedClaims
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedClaims = Value
        End Set
    End Property

    Public WriteOnly Property CaseID() As Integer
        Set(ByVal Value As Integer)
            m_lCaseID = Value
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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'CMG/PB 04092002 Bug 655 Clear the member variable before searchin

            m_vSearchData = Nothing
            'End CMG

            'RWH(17/04/2001) Search by Risk Index.
            If txtRegNumber.Text.Trim() <> "" Then
                DisplayStatusSearching()

                m_lReturn = CType(ValidateIndex(), gPMConstants.PMEReturnCode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'Assign Values to Interface
                    m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
                End If
                DisplayStatusFound()
                Return result

            Else
                'JMK 18/05/2001 - Underwriting uses separate SQL now
                ' Get the Claimdetails from the business object.


                m_lReturn = g_oBusiness.GetClaimDetailsSFU(r_vResultArray:=m_vSearchData, v_vClaimNumber:=txtClaimRef.Text, v_vClientName:=txtPolicyHolder.Text, v_vPolicyNumber:=txtPolicy.Text, v_vRegNumber:=txtRegNumber.Text, v_vLossFromdate:=m_vLossFromDate, v_vLossToDate:=m_vLossToDate, v_vClaimStatus:=m_bClaimStatus, v_lCaseID:=m_lCaseID, v_sOtherParty:=txtTPA.Text)


                'Assign Values to Interface
                m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)


                ' Check the return values.
                Select Case (m_lReturn)
                    Case gPMConstants.PMEReturnCode.PMTrue
                        ' Found search details.
                    Case gPMConstants.PMEReturnCode.PMNotFound
                        ' No search details found.
                    Case Else
                        ' Failed to get details.
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                        Return result
                End Select

                ' Display the number of item found message.
                DisplayStatusFound()

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            lvwsearchdetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Assign the details to the first column.
                ' Column 1 Claim Type


                'developer guide no.242
                oListItem = lvwsearchdetails.Items.Add(CStr(m_vSearchData(ACIClaimRef, lRow)).Trim(), ACFindImage)

                ' Assign details to other the columns
                ' Column 2 Claim Ref

                oListItem.SubItems.Add(1).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIInsuranceRef, lRow))).Trim()

                ' Column 3 PolicyRef
                'oListItem.SubItems(2) = Trim$(m_vSearchData(ACIInsuranceRef, lRow&))



                ' Column 4 RiskIndex
                'TN20010426 Start
                'oListItem.SubItems(2) = Trim$(m_vSearchData(ACIURiskIndex, lRow&))

                oListItem.SubItems.Add(2).Text = CStr(m_vSearchData(ACIUClientCode, lRow)).Trim()
                'TN20010426 End

                'Start (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)
                'Adding new column "Client Name" after Client Code column
                ' Column 4 Client Name

                oListItem.SubItems.Add(3).Text = CStr(m_vSearchData(ACIUPolicyHolder, lRow)).Trim()

                'Rearranging the indices of subsequent columns to accommodate the newly added column
                ' Column 5 Product Code

                oListItem.SubItems.Add(4).Text = CStr(m_vSearchData(ACIUProductCode, lRow)).Trim()

                ' Column 6 Date Reported

                oListItem.SubItems.Add(5).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACIULossDate, lRow)))
                'End (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)


                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwsearchdetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwsearchdetails.Refresh()
                End If
            Next lRow

            ' Select the first item.
            lvwsearchdetails.Items.Item(0).Selected = True

            ' Enable the interface now that the search has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

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
    ' Date:15/07/00
    '
    ' Edit History:Pandu
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem, lCopyClaimId, lLatestClaimId As Integer
        Dim sMessage As String = ""
        Dim lMessage As Integer
        Dim sClientShortName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.
            'Added a if condition,because program need to store the value of selected item
            If lvwsearchdetails.Items.Count <> 0 Then
                If lvwsearchdetails.Items.Count < 1 Then
                    Return result
                End If
            End If

            'Modified,convert focused item to selected item
            'lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.FocusedItem.Index).Tag)
            'Added a if condition,because program need to store the value of selected item
            If lvwsearchdetails.SelectedItems.Count > 0 Then
                lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.SelectedItems.Item(0).Index).Tag)
            Else
                lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(0).Tag)
            End If
            ' CJB 240904 This previous change should have had u/w only wrapper


            ' Check we can amend this claim (we can't if it is associated to a closed branch)

            If CStr(m_vSearchData(ACIBranchClosed, lSelectedItem)) = "1" Then

                If CStr(m_vSearchData(ACIClaimsAllowed, lSelectedItem)) = "0" Then

                    Select Case (m_sTransactionType)
                        Case "C_SA"
                            ' Salvage recovery
                            lMessage = ACClosedBranchError_Recoveries
                        Case "C_RV"
                            ' Third party recovery
                            lMessage = ACClosedBranchError_Recoveries
                        Case "C_CR"
                            ' Maintain claim
                            lMessage = ACClosedBranchError
                        Case "C_CP"
                            ' Claim payment
                            lMessage = ACClosedBranchError_Payments
                        Case Else
                            ' Other, just in case
                            lMessage = ACClosedBranchError
                    End Select


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lMessage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    MessageBox.Show(sMessage, "Branch closed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If
            End If

            If m_bSelectVersionEnabled Then


                m_lClaimCnt = CInt(CStr(m_vSearchData(ACIClaimCnt, lSelectedItem)).Trim())

                m_sClaimRef = CStr(m_vSearchData(ACIClaimRef, lSelectedItem)).Trim()

                'Determine which version of the claim the user wants to view
                m_lReturn = GetClaimVersion()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' if claim version details havent been retrieved
            Else

                ' use details from the initial search data array

                ' get generic fields

                m_lRiskTypeId = CInt(CStr(m_vSearchData(ACIRiskTypeID, lSelectedItem)).Trim())

                m_lClaimCnt = CInt(CStr(m_vSearchData(ACIClaimCnt, lSelectedItem)).Trim())

                m_sClaimRef = CStr(m_vSearchData(ACIClaimRef, lSelectedItem)).Trim()

                m_lInsuranceFilecnt = CInt(CStr(m_vSearchData(ACIInsuranceid, lSelectedItem)).Trim())

                m_sPolicyRef = CStr(gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIInsuranceRef, lSelectedItem)))).Trim()


                m_sPolicyHolder = CStr(m_vSearchData(ACIUPolicyHolder, lSelectedItem)).Trim()


            End If

            'DC080403 -ISS3387 apply for broking too
            'PSL 11/07/2003 the view button does the dummy delete thing so no lock here either
            'PSL 15/07/2003 removed the dummydelete again
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                ' for underwriting only

                ' clean up any existing dirty claims prior to taking out a new lock

                m_lReturn = g_oBusiness.CleanUpDirtyClaims(m_lClaimCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = CType(LockClaim(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMRecordInUse
                End If



                m_lReturn = g_oBusiness.GetLatestClaimId(v_sClaimRef:=CStr(m_vSearchData(ACIClaimRef, lSelectedItem)).Trim(), r_lLatestClaimId:=lLatestClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

                If m_lClaimCnt <> lLatestClaimId Then
                    UnlockClaim()
                    Return gPMConstants.PMEReturnCode.PMDataChanged
                End If
            End If

            ' only copy a claim when its not in view mode
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then


                m_lReturn = g_oBusiness.ProcessCopyClaim(v_lClaimId:=m_lClaimCnt, r_lCopyClaimId:=lCopyClaimId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lClaimCnt = lCopyClaimId

            Else


                m_sPolicyHolder = CStr(m_vSearchData(ACIBPolicyHolder, lSelectedItem)).Trim()


                sClientShortName = CStr(m_vSearchData(17, lSelectedItem)).Trim()


                m_lReturn = m_oParty.GetPartyCnt(vPartyRef:=sClientShortName, vPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

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
    ' Name: GetPolicyInfo
    '
    ' Description:  Instance FindInsurance to retrieve Policy reference
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetPolicyInfo() As Integer
        Dim result As Integer = 0

        Dim oFindPolicy As iSIRFindInsurance.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iSIRFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oFindPolicy.CallingAppName = ACApp

            oFindPolicy.PolicyNumber = txtPolicy.Text

            m_lReturn = CType(iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), False), gPMConstants.PMEReturnCode)


            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Retrieve InsuranceRef and set as PolicyRef

            m_sPolicyRef = oFindPolicy.PolicyNumber

            ' Destroy Find Insurance object

            oFindPolicy.Dispose()
            oFindPolicy = Nothing

            m_lReturn = CType(iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)
            ' Display Policy Reference on form
            txtPolicy.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPolicyRef.Trim())

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyHolderInfo
    '
    ' Description: Instance FindParty to retrieve Policyholder
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetPolicyHolderInfo() As Integer

        Dim result As Integer = 0
        'developer guide no. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface
            oFindParty.CallingAppName = ACApp
            oFindParty.ShortName = txtPolicyHolder.Text
            oFindParty.IsComplaint = CShort(m_IIsComplaint)
            oFindParty.IgnoreDPAQuestions = True
            oFindParty.NotEditable = 1

            m_lReturn = CType(iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), False), gPMConstants.PMEReturnCode)

            m_lReturn = CType(oFindParty.Start(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Retrieve party details
            m_sPolicyHolder = oFindParty.ShortName

            ' Destroy Find Party object
            oFindParty.Dispose()
            oFindParty = Nothing

            m_lReturn = CType(iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)

            ' Display Agent on form
            txtPolicyHolder.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPolicyHolder.Trim())

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyHolderInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            txtClaimRef.Text = m_sClaimRef.Trim()
            txtPolicy.Text = m_sPolicyRef.Trim()
            txtPolicyHolder.Text = m_sPolicyHolder.Trim()

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
    ' Date :15/07/2000
    '
    ' Edit History : Pandu
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            lvwsearchdetails.Columns.Insert(Column1 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column2 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column3 - 1, "", 94)

            'DC130202
            'If m_lSiriusUnderWritingBroking = ACUnderWriting Then

            lvwsearchdetails.Columns.Insert(Column4 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column5 - 1, "", 94)


            'Start (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)
            'Making the one more column available for the newly added column
            lvwsearchdetails.Columns.Insert(Column6 - 1, "", 94)
            'End (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)

            'DC130202



            'lvwsearchdetails.ColumnHeaders(Column1).Width = ColWidthUnderWriting
            'TN20010426 add 100 to column 1 and 2
            lvwsearchdetails.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting + 100))
            lvwsearchdetails.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting + 100))
            lvwsearchdetails.Columns.Item(Column3 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwsearchdetails.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            lvwsearchdetails.Columns.Item(Column5 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)
            lvwsearchdetails.Columns.Item(Column6 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthUnderWriting))
            'End (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)

            lblClientName.Visible = False
            txtClientName.Visible = False
            cmdInsurer.Visible = False
            txtInsurer.Visible = False
            cmdAccountExec.Visible = False
            txtAccountExec.Visible = False
            lblRegistration.Visible = False
            txtRegistration.Visible = False

            tabMainTab.Height = VB6.TwipsToPixelsY(2415)
            lvwsearchdetails.Top = VB6.TwipsToPixelsY(2640)

            'DC050606 PN28739 no longer required as should be in broking anyway
            'lblSSYesNo.Visible = True
            'ChkCLosedClaim.Visible = True

            'PN32091 - For displaying the Closed Claims
            If m_sTransactionType = "C_CP" Then
                lblSSYesNo.Visible = False
                ChkCLosedClaim.Visible = False
            Else

                lblSSYesNo.Top = lblClientName.Top
                ChkCLosedClaim.Top = txtClientName.Top

                lblSSYesNo.Visible = True
                ChkCLosedClaim.Visible = True
            End If


            'Do we need to show the Data Protection Act questions
            m_lReturn = CType(GetDPASetting(), gPMConstants.PMEReturnCode)

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
                    If Not m_bAskDPAQuestions Then
                        chkDPARequired.Enabled = False
                    End If
                End If

            End If

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(18/04/2001)
            'Made full row select on list views
            'developer guide no.303
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwsearchdetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lvwsearchdetails.FullRowSelect = True
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.
            m_bClaimStatus = True
            If m_bIncludeClosedClaims Then
                ChkCLosedClaim.CheckState = CheckState.Checked
            End If
            ' Set the column widths for the search list.

            m_vLossFromDate = ""
            m_vLossToDate = ""

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'Set the Tag Properties of Date Fields to Check Lost Focus Has Fired or not

            txtclaimstartdate.Tag = CStr(True)
            txtclaimenddate.Tag = CStr(True)


            m_lReturn = GetUserOtherParty()
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
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Dim selectedIndex As Integer = 0

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
            lvwsearchdetails.Items.Clear()

            ' Clear the search status bar.
            'stbstatus.Text = ""
            _stbstatus_Panel1.Text = ""

            ' All fields should be cleared.
            txtClaimRef.Text = ""
            txtPolicy.Text = ""
            txtPolicyHolder.Text = ""
            txtRegNumber.Text = ""
            txtclaimstartdate.Text = ""
            txtclaimenddate.Text = ""

            '    If m_sTransactionType = "C_CP" Then
            '        ChkCLosedClaim.Value = False
            '    End If

            txtClientName.Text = ""
            txtInsurer.Text = ""
            txtAccountExec.Text = ""
            txtRegistration.Text = ""

            m_vLossFromDate = ""
            m_vLossToDate = ""
            If txtTPA.ReadOnly = False Then
                txtTPA.Text = ""
            End If


            If txtTPA.ReadOnly = False Then
                txtTPA.Text = ""
            End If


            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Set focus to the search details.
            txtClaimRef.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)


            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

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
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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


            m_ctlTabFirstLast(ACControlStart, 0) = txtClaimRef
            m_ctlTabFirstLast(ACControlEnd, 0) = txtClaimRef




            m_ctlTabFirstLast(ACControlStart, 1) = txtRegNumber
            m_ctlTabFirstLast(ACControlEnd, 1) = txtRegNumber





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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'tabMainTab.TabCaption(1) = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lId:=ACTabTitle2, _
            'iDataType:=PMResString)

            'Claim Type
            '    lvwsearchdetails.ColumnHeaders(1).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitle1, _
            ''        iDataType:=PMResString)

            'ClaimReference


            lvwsearchdetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Insurance Reference


            lvwsearchdetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))





            lvwsearchdetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Start - (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)
            'Set column header to Client Name


            lvwsearchdetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle11, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Rearranging the next column header indices to accommodate the new coloumn header added


            lvwsearchdetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwsearchdetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'End - (Prakash Varghese) - (Tech Spec - TRAC 2899 Client Code Client Name.docx) - (6.1.2)


            lblClaimRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            CmdClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyHolder, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClientName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_RES_CLIENTNAME, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdInsurer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_RES_INSURER, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAccountExec.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_RES_ACCOUNTEXEC, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRegistration.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_RES_VEHICLEREG, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRegNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRegistrationNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLossDateStartLimit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossFromDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLossDateEndLimit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLossToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSSYesNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIncludeClosedClaims, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            Return result

        Catch excep As System.Exception




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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable

            'JJ 03/9/2003 The following will always disable the view button for underwriting.
            ' This has been confirmed with Pete Finney.
            ' If this button is enabled and because it uses the PMDummyDelete,
            ' claimmode property, and transactiontype to overide the standard task mode all other
            ' components in the roadmap will need to be amended.
            ' If the user wants to view a claim then they should use the view roadmap.


            cmdView.Visible = False
            Return result

        Catch excep As System.Exception




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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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
    ' Date : 15/07/2000
    '
    ' Edit History :Pandu
    '
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

            'stbstatus.Text = " " & lItemsFound & " " & sMessage
            _stbstatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception




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
    ' Date : 15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            If txtClaimRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPolicy.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtPolicyHolder.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtRegNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtclaimstartdate.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtclaimenddate.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtClientName.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtRegistration.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtInsurer.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtAccountExec.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtTPA.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
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
    ' Date :15/07/2000
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

            lvwsearchdetails.Width = Me.Width - VB6.TwipsToPixelsX(360)

            lvwsearchdetails.Height = Me.Height - VB6.TwipsToPixelsY(3870)


            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            'DC180202
            '    If m_lSiriusUnderWritingBroking = ACBroking Then
            '
            '        cmdView.Left = Me.Width - 7335
            '        cmdView.Top = Me.Height - 1110
            '
            '        cmdView.Visible = True
            '    Else
            'JJ 03/9/2003 The following will always disable the view button for underwriting.
            ' This has been confirmed with Pete Finney.
            ' If this button is enabled and because it uses the PMDummyDelete,
            ' claimmode property, and transactiontype to overide the standard task mode all other
            ' components in the roadmap will need to be amended.
            ' If the user wants to view a claim then they should use the view roadmap.

            cmdView.Visible = False
            '
            '    End If

            '    If (cmdNavigate.Visible = True) Then
            '        cmdNavigate.Top = Me.Height - 1110
            '    End If

            Return result

        Catch





            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' PRIVATE Methods (End)

    Private Sub chkDPARequired_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDPARequired.CheckStateChanged
        If chkDPARequired.CheckState = CheckState.Checked Then
            lblDPARequired.Text = "Yes"
        Else
            lblDPARequired.Text = "No"
        End If
    End Sub

    Private Sub cmdAccountExec_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccountExec.Click

        Dim vCnt As Object
        Dim vShortName As String = ""
        Dim vName, vResolvedName As Object




        m_lReturn = CType(SelectParty(vPartyCnt:=CInt(vCnt), vShortName:=vShortName, vName:=CStr(vName), vSpecialParty:="CO", vResolvedName:=CStr(vResolvedName)), gPMConstants.PMEReturnCode)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            txtAccountExec.Text = gPMFunctions.ToSafeString(vShortName)
        End If

    End Sub

    Private Sub CmdClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdClient.Click

        m_lReturn = GetPolicyHolderInfo()

    End Sub

    Private Sub cmdInsurer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurer.Click

        Dim vCnt As Object
        Dim vShortName As String = ""
        Dim vName, vResolvedName As Object




        m_lReturn = CType(SelectParty(vPartyCnt:=CInt(vCnt), vShortName:=vShortName, vName:=CStr(vName), vSpecialParty:="IN", vResolvedName:=CStr(vResolvedName)), gPMConstants.PMEReturnCode)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            txtInsurer.Text = gPMFunctions.ToSafeString(vShortName)
        End If

    End Sub

    Private Sub cmdPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPolicy.Click

        m_lReturn = GetPolicyInfo()

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            txtClaimRef.Focus()
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: FormIntialise
    '
    ' Description: Intialise all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMFindClaim.General()

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oParty = temp_m_oParty

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




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: FormLoad
    '
    ' Description: Loads all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'code transfered to frmInterfaceLoad() function.
    End Sub

    Public Sub frmInterfaceLoad()

        ' Forms load event.

        Try

            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Validate fields using Forms Control

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            'Set the UnderWriting/Broking Constant
            'm_lSiriusUnderWritingBroking = ACBroking

            'm_lSiriusUnderWritingBroking = ACUnderWriting




            m_lSiriusUnderWritingBroking = g_oBackofficelink.Sirius_Product

            'TN20010505 Start

            m_lReturn = g_oBackofficelink.SetProcessModes(vTransactionType:=m_sTransactionType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            'TN20010505 End

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Inadequate data so cannot
                ' continue with the search.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            ' {* USER DEFINED CODE (End) *}

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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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

            'developer guide no. 19 (No Solution)
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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
            Form_Terminate_Renamed()
            ' Terminate the general object.
            m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

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
    ' ***************************************************************** '
    ' Name:Form_KeyDown
    '
    ' Description: Determine the Position of Tab and Control on
    '              pressing pageup,pagedown,home,end buttons
    '
    ' Date:15/07/00
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

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




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
        'If isInitializingComponent Then
        '    Exit Sub
        'End If

        Try
            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
        Catch
            Exit Sub
        End Try

    End Sub

    Private Sub ChkCLosedClaim_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ChkCLosedClaim.CheckStateChanged

        If ChkCLosedClaim.CheckState = CheckState.Unchecked Then
            'if not Selected donot include closed claims
            m_bClaimStatus = True

        Else
            'if selected include closed claims
            m_bClaimStatus = False

        End If


    End Sub


    Private Sub Form_Terminate_Renamed()
        If Not (m_oParty Is Nothing) Then

            m_oParty.Dispose()
            m_oParty = Nothing
        End If
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
                '        DoEvents

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch





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

        Try
            m_lReturn = ProcessOKClick()

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    ''' <summary>
    ''' Set Properties of the form on clicking OK Button from the
    ''' relevant list item under focus or clicked
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessOKClick() As Integer

        Dim nResult As Integer
        Dim bProceed As Boolean
        Dim bStatus As Boolean
        Dim nSelectedItem As Integer
        Dim nClaimStatus As Integer
        Dim sInsReference As String
        Dim oPMUPolicy As Object = Nothing
        Dim oResult(,) As Object
        Dim bIsPendingPortfolioTransfer As Boolean
        Dim bIsPendingCloneTransfer As Boolean
        m_iNoofReferredPayments = 0

        Try

            nResult = PMEReturnCode.PMTrue

            nResult = CType(ProcessFSA(r_bProceed:=bProceed), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Or Not bProceed Then
                Return nResult
            End If

            If m_iTask <> PMEComponentAction.PMView Then
                m_iTask = PMEComponentAction.PMEdit
            End If

            If m_bRefreshClaimData Then
                Call cmdFindNow_Click(New Object, New System.EventArgs)
            End If

            'Modified,change focuseditem to selected item
            'Added a if condition,because program need to store the value of selected item
            If lvwsearchdetails.SelectedItems.Count > 0 Then
                nSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.SelectedItems.Item(0).Index).Tag)
            Else
                nSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(0).Tag)
            End If

            m_lClaimCnt = CInt(CStr(m_vSearchData(ACIClaimCnt, nSelectedItem)).Trim())

            sInsReference = CStr(m_vSearchData(ACIInsuranceRef, nSelectedItem)).Trim()

            nResult = GetProductDetails()
            If nResult <> PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError, sMsg:="Failed to GetProductDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOKClick")

                Return PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = "C_CP" Then

                'Modified,change focuseditem to selected item
                'Added a if condition,because program need to store the value of selected item
                If lvwsearchdetails.SelectedItems.Count > 0 Then
                    nSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.SelectedItems.Item(0).Index).Tag)
                Else
                    nSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(0).Tag)
                End If

                nClaimStatus = ToSafeInteger(CStr(m_vSearchData(ACIUClaimStatus, nSelectedItem)).Trim())
                If nClaimStatus = 3 Then
                    MessageBox.Show("Claim Payments cannot be requisitioned on Closed Claims." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "You will need to first Reopen the claim if you wish to " & Strings.Chr(13) & Strings.Chr(10) & "perform a Claim Payment.", "Claim Payment", MessageBoxButtons.OK)
                    Return nResult
                End If

                If m_bCheckMediaTypeStatusAtClaimPayment Then
                    nResult = CType(ProcessPolicyReceiptMediatTypeStatus(v_lInsuranceFileId:=ToSafeInteger(m_vSearchData(ACIInsuranceid, nSelectedItem)), v_dtLossDate:=gPMFunctions.ToSafeDate(m_vSearchData(ACIULossDate, nSelectedItem), DateTime.Parse(DateTime.Now)), r_bProceed:=bProceed), PMEReturnCode)

                    If nResult <> PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError, sMsg:="Failed to execute ProcessPolicyReceiptMediatTypeStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOKClick")
                        Return nResult
                    End If
                    If Not bProceed Then
                        MessageBox.Show("Please refer to accounts as the status of receipts is not cleared", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return nResult
                    End If
                End If
            End If

            If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR" Or m_sTransactionType = "C_SA" Or m_sTransactionType = "C_RV" Then

                'Modified,change focuseditem to selected item,and Added a if condition,because program need to store the value of selected item
                If lvwsearchdetails.SelectedItems.Count > 0 Then
                    nSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.SelectedItems.Item(0).Index).Tag)
                Else
                    nSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(0).Tag)
                End If

                'Check for pending portfolio or clone transfer
                Dim temp_o_policyBusiness As Object
                nResult = g_oObjectManager.GetInstance(temp_o_policyBusiness, "bPMUPolicy.Business", vInstanceManager:=PMGetViaClientManager)
                oPMUPolicy = temp_o_policyBusiness

                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError, sMsg:="Failed to create bPMUPolicy object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOKClick")

                    Return nResult
                End If

                nResult = oPMUPolicy.IsPendingPortfolioTransfer(sInsuranceFileRef:=sInsReference, r_oResult:=oResult, r_bIsPendingPortfolioTransfer:=bIsPendingPortfolioTransfer, r_bIsPendingCloneTransfer:=bIsPendingCloneTransfer)
                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError, sMsg:="IsPendingPortfolioTransfer method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOKClick")

                    Return nResult
                End If

                If Information.IsArray(oResult) OrElse bIsPendingPortfolioTransfer Then
                    MessageBox.Show("Pending Portfolio Transfer.", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return nResult

                ElseIf bIsPendingCloneTransfer Then
                    MessageBox.Show("Pending Clone Transfer.", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return nResult
                End If

                nResult = g_oBusiness.CheckReferredPayment(v_lClaimId:=m_lClaimCnt, r_bStatus:=bStatus, r_iNoofReferredPayments:=m_iNoofReferredPayments)
                If m_sTransactionType = "C_CP" Then 'Check multiple claim payments only in case of Claim Payments
                    If Not m_bMultipleClaimPayments Then
                        If bStatus Then
                            MessageBox.Show("Claim currently locked - awaiting claim payment authorisation", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return nResult
                        End If
                    Else
                        If m_iNoofReferredPayments >= m_iMaxNoofUnAuthorisedClaimPayments And m_iMaxNoofUnAuthorisedClaimPayments <> 0 Then
                            MessageBox.Show("Claim currently locked -  " & m_iNoofReferredPayments & " claim payments " & "awaiting claim payment authorisation", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return nResult
                        End If
                    End If
                Else
                    ' Won't let user to Proceed with Maintain, Salvage and TP when No of referred payments are greater than Zero
                    If m_iNoofReferredPayments > 0 Then
                        MessageBox.Show("Claim currently locked -  " & m_iNoofReferredPayments & " claim payments " & "awaiting claim payment authorisation", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return nResult
                    End If
                End If
            End If

            ' Set the interface status.
            m_lStatus = PMEReturnCode.PMOK

            ' Process the next set of actions.
            nResult = CType(m_oGeneral.ProcessCommand(), PMEReturnCode)
            If nResult = PMEReturnCode.PMRecordInUse Then
                Return nResult
            End If

            If nResult = gPMConstants.PMEReturnCode.PMDataChanged Then
                Me.cmdOK.Enabled = False
                MessageBox.Show("Selected Claim Record '" + CStr(m_vSearchData(ACIClaimRef, nSelectedItem)).Trim() + "' has been changed. Please click 'Find Now' button again to refresh search data.", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return nResult
            End If

            ' Check the return value.
            If nResult = PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                If Me.Visible Then
                    Me.Hide()
                End If
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to process the OKClick", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        Finally

            If oPMUPolicy IsNot Nothing Then
                oPMUPolicy.Dispose()


                oPMUPolicy = Nothing
            End If

        End Try
        Return nResult
    End Function
    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Dim bProceed As Boolean

        Try

            m_lReturn = CType(ProcessFSA(r_bProceed:=bProceed), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not bProceed Then
                Exit Sub
            End If

            m_iTask = gPMConstants.PMEComponentAction.PMDummyDelete

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)


            'If m_lReturn = PMRecordInUse Then
            '    Exit Sub
            'End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the View command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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

        ' Click event of the Find Now button.

        Try

            Application.DoEvents()

            Dim sDisplaytext As String = ""

            If CBool(Convert.ToString(txtclaimstartdate.Tag)) Then

                If txtclaimstartdate.Text <> "" Then

                    If Information.IsDate(txtclaimstartdate.Text.Trim()) Then
                        m_vLossFromDate = StringsHelper.Format(txtclaimstartdate.Text.Trim(), ACDateConversion)
                    Else
                        DisplayMessage(ACInvalidDateMsg, Mid(lblLossDateStartLimit.Name, 4))
                        txtclaimstartdate.Text = ""

                        'm_vLossFromDate = ""

                        txtclaimstartdate.Focus()

                        Exit Sub

                    End If
                End If
            End If

            If CBool(Convert.ToString(txtclaimenddate.Tag)) Then

                If txtclaimenddate.Text <> "" Then

                    If Information.IsDate(txtclaimenddate.Text.Trim()) Then

                        m_vLossToDate = StringsHelper.Format(txtclaimenddate.Text.Trim(), ACDateConversion)

                    Else

                        DisplayMessage(ACInvalidDateMsg, Mid(lblLossDateEndLimit.Name, 4))

                        txtclaimenddate.Text = ""

                        'm_vLossToDate = ""

                        txtclaimenddate.Focus()

                        Exit Sub

                    End If
                End If
            End If


            If m_vLossFromDate <> "" And m_vLossToDate <> "" Then

                m_lReturn = CType(CheckDateDiff(m_vLossFromDate, m_vLossToDate), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                    DisplayMessage(ACDateDiffError, "Date Difference Error")

                    txtclaimstartdate.Focus()

                    Exit Sub

                End If
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            End If

            If lvwsearchdetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)
                'DC180202
                VB6.SetDefault(cmdView, False)
            End If

            ' Set the focus.
            'Changes done by Krishna Nand
            'Purpose: Reset the sorting order
            'Date: 04/02/2010
            'PN: 67176
            ListViewHelper.SetSortOrderProperty(lvwsearchdetails, SortOrder.Descending)
            'End of Changes done by Krishna Nand on 04/02/2010 for PN 67176

            lvwsearchdetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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
            m_lReturn = CType(ClearInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Sub lvwsearchdetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can select with Enter key.
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOK, False)
            'DC180202
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception




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
    Private Sub lvwsearchdetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name:lvwSearchDetails_Click
    '
    ' Description:Fill the Claim Reference,Policy No.,Client Short Name
    '              in Text Box for the listitem clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub lvwsearchdetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.Click

        Dim sIndex As Integer


        If lvwsearchdetails.Items.Count > 0 Then


            sIndex = Convert.ToString(lvwsearchdetails.FocusedItem.Tag)


            txtClaimRef.Text = CStr(m_vSearchData(ACIClaimRef, sIndex)).Trim()

            txtPolicy.Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIInsuranceRef, sIndex))).Trim()



            txtPolicyHolder.Text = CStr(m_vSearchData(ACIUClientCode, sIndex)).Trim()


            VB6.SetDefault(cmdOK, True)
            'DC180202
            VB6.SetDefault(cmdView, True)

        End If

    End Sub
    ' ***************************************************************** '
    ' Name: lvwSearchDetails_DblClick
    '
    ' Description:Move to the next form in the road map
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwsearchdetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwsearchdetails.DoubleClick
        Dim bStatus, bProceed As Boolean
        Dim lSelectedItem, lClaimStatus As Integer
        m_iNoofReferredPayments = 0
        Try

            ' Check if there are any items available.
            If lvwsearchdetails.Items.Count = 0 Then
                Exit Sub
            End If

            m_lReturn = CType(ProcessFSA(r_bProceed:=bProceed), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not bProceed Then
                Exit Sub
            End If

            lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.FocusedItem.Index).Tag)

            m_lClaimCnt = CInt(CStr(m_vSearchData(ACIClaimCnt, lSelectedItem)).Trim())

            m_lReturn = GetProductDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetProductDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOKClick")

                Exit Sub
            End If

            If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR" Or m_sTransactionType = "C_SA" Or m_sTransactionType = "C_RV" Then


                m_lReturn = g_oBusiness.CheckReferredPayment(v_lClaimId:=m_lClaimCnt, r_bStatus:=bStatus, r_iNoofReferredPayments:=m_iNoofReferredPayments)
                If m_sTransactionType = "C_CP" Then
                    If Not m_bMultipleClaimPayments Then
                        If bStatus Then
                            MessageBox.Show("Claim currently locked - awaiting claim payment authorisation", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    Else
                        If m_iNoofReferredPayments > m_iMaxNoofUnAuthorisedClaimPayments And m_iMaxNoofUnAuthorisedClaimPayments <> 0 Then
                            MessageBox.Show("Claim currently locked -  " & m_iNoofReferredPayments & " claim payments " & "awaiting claim payment authorisation", "Claim Payment", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    End If
                Else
                    ' Won't let user to Proceed with Maintain, Salvage and TP when No of referred payments are greater than Zero
                    If m_iNoofReferredPayments > 0 Then
                        MessageBox.Show("Claim currently locked -  " & m_iNoofReferredPayments & " claim payments " & "awaiting claim payment authorisation", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                End If
            End If

            If m_sTransactionType = "C_CP" Then

                lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.FocusedItem.Index).Tag)

                lClaimStatus = CInt(CStr(m_vSearchData(ACIUClaimStatus, lSelectedItem)).Trim())
                If lClaimStatus = 3 Then
                    MessageBox.Show("Claim Payments cannot be requisitioned on Closed Claims." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "You will need to first Reopen the claim if you wish to " & Strings.Chr(13) & Strings.Chr(10) & "perform a Claim Payment.", "Claim Payment", MessageBoxButtons.OK)
                    Exit Sub
                End If
                'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
                If m_bCheckMediaTypeStatusAtClaimPayment Then
                    m_lReturn = CType(ProcessPolicyReceiptMediatTypeStatus(v_lInsuranceFileId:=gPMFunctions.ToSafeInteger(m_vSearchData(ACIInsuranceid, lSelectedItem)), v_dtLossDate:=gPMFunctions.ToSafeDate(m_vSearchData(ACIULossDate, lSelectedItem), DateTime.Now), r_bProceed:=bProceed), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to execute ProcessPolicyReceiptMediatTypeStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick")

                        Exit Sub
                    End If
                    If Not bProceed Then
                        MessageBox.Show("Please refer to accounts as the status of receipts is not cleared", "Find Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                End If
                'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
            End If
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'PSL 15/07/2003 Override view with dummy delete in an attempt to make it work
            'like it used to do before the roadmap
            'included m_itask bit to be same as OK button
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                'Do nothing
                'PN12937 - No need of this line "m_iTask% = PMDummyDelete"
            Else
                'DC190602 (Edit Mode)
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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
    Private Sub lvwsearchdetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwsearchdetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
            'DC180202
            VB6.SetDefault(cmdView, False)
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

    Private Sub lvwsearchdetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwsearchdetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Dim sIndex As Integer

        If KeyAscii = 13 Then
            If lvwsearchdetails.Items.Count > 0 Then

                sIndex = Convert.ToString(lvwsearchdetails.FocusedItem.Tag)


                txtClaimRef.Text = CStr(m_vSearchData(ACIClaimRef, sIndex)).Trim()

                txtPolicy.Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIInsuranceRef, sIndex))).Trim()




                txtPolicyHolder.Text = CStr(m_vSearchData(ACIUPolicyHolder, sIndex)).Trim()


                VB6.SetDefault(cmdOK, True)
                'DC180202
                VB6.SetDefault(cmdView, True)

            End If
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
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
    Private Sub lvwsearchdetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwsearchdetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwsearchdetails.Columns(eventArgs.Column)

        Dim iDirection As SortOrder

        ' Column click event for the search details

        Try

            'DC211003 -PN7301 -was using wrong column for date sort in broking


            With lvwsearchdetails

                ' If date column clicked, then sort by date sort column
                If ListViewHelper.GetSortOrderProperty(lvwsearchdetails) = 1 Then
                    iDirection = SortOrder.Descending
                Else
                    iDirection = SortOrder.Ascending
                End If
                ' Changes Done by : Krishna Nand
                ' Purpose: correct the sorting on Date Column
                ' PN: 67176
                ' Dated: 04/02/2010
                If ColumnHeader.Index + 1 - 1 = 5 Then
                    ''If (ColumnHeader.Index - 1 = 6) Then
                    'TN20010425 Start
                    '.Sorted = False

                    'If (.SortKey <> 5) Then
                    ''If (.SortKey <> 4) Then
                    ''.SortKey = 4
                    '.SortKey = 5

                    ''    iDirection = 0
                    ''Else


                    ''End If
                    'End of Changes done by Krishna Nand on 04/02/2010 for PN 67176
                    'developer guide no. 178
                    m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwsearchdetails, v_iSourceColumn:=5, v_iDirection:=iDirection), gPMConstants.PMEReturnCode)

                    '            .Sorted = True
                    'TN20010425 End

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwsearchdetails)) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwsearchdetails, iDirection)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwsearchdetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwsearchdetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwsearchdetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwsearchdetails, True)
                End If
            End With

            'DC211003 -PN7301 -was using wrong column for date sort in broking

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtAccountExec_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountExec.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Me.cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub


    Private Sub txtclaimenddate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtclaimenddate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls


        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

        'm_vLossToDate = Trim(txtclaimenddate.Text)

        If txtclaimenddate.Text.Trim() = Nothing Then

            txtclaimenddate.Tag = CStr(True)

            m_vLossToDate = ""
        End If

        If Information.IsDate(txtclaimenddate.Text.Trim()) Then

            m_vLossToDate = StringsHelper.Format(txtclaimenddate.Text.Trim(), ACDateConversion)

            txtclaimenddate.Tag = CStr(True)
        End If

    End Sub

    Private Sub txtclaimenddate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtclaimenddate.Enter

        iPMFunc.SelectText(txtclaimenddate)

        If txtclaimenddate.Text.Trim() <> "" Then

            txtclaimenddate.Text = StringsHelper.Format(m_vLossToDate, ACShortDate)

        End If

        'm_vLossToDate = ""

    End Sub

    Private Sub txtclaimenddate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtclaimenddate.Leave

        Dim sDisplaytext As String = ""

        If txtclaimenddate.Text <> "" Then

            m_lReturn = CType(CheckValiddate(txtclaimenddate.Text, 1, sDisplaytext), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                txtclaimenddate.Text = sDisplaytext

            Else
                txtclaimenddate.Text = sDisplaytext

                DisplayMessage(ACInvalidDateMsg, Mid(lblLossDateEndLimit.Name, 4))

                txtclaimenddate.Focus()

            End If

        End If

    End Sub

    Private Sub txtClaimRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimRef.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtClaimRef)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtClaimRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtclaimstartdate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtclaimstartdate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls


        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)



        If txtclaimstartdate.Text.Trim() = Nothing Then

            txtclaimstartdate.Tag = CStr(True)

            m_vLossFromDate = ""

        End If

        If Information.IsDate(txtclaimstartdate.Text.Trim()) Then

            m_vLossFromDate = StringsHelper.Format(txtclaimstartdate.Text.Trim(), ACDateConversion)

            txtclaimstartdate.Tag = CStr(True)

        End If

    End Sub

    Private Sub txtclaimstartdate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtclaimstartdate.Enter

        iPMFunc.SelectText(txtclaimstartdate)

        If txtclaimstartdate.Text.Trim() <> "" Then

            txtclaimstartdate.Text = StringsHelper.Format(m_vLossFromDate, ACShortDate)

        End If

        'm_vLossFromDate = ""


    End Sub

    Private Sub txtclaimstartdate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtclaimstartdate.Leave

        Dim sDisplaytext As String = ""

        If txtclaimstartdate.Text <> "" Then

            m_lReturn = CType(CheckValiddate(txtclaimstartdate.Text, 0, sDisplaytext), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                txtclaimstartdate.Text = sDisplaytext

            Else
                txtclaimstartdate.Text = sDisplaytext

                DisplayMessage(ACInvalidDateMsg, Mid(lblLossDateStartLimit.Name, 4))

                txtclaimstartdate.Focus()

            End If


        End If


    End Sub

    Private Sub txtClientName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClientName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Me.cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtInsurer_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurer.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Me.cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtPolicy_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicy.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtPolicy)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtPolicy_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicy.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls


        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtPolicyHolder_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyHolder.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtPolicyHolder)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtPolicyHolder_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyHolder.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls


        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub


    'Private Sub SSAgentFind_Click()
    '
    'm_lReturn& = GetAgentInfo()
    '
    'End Sub


    'Private Sub SSPolicyHolderFind_Click()
    '
    'm_lReturn& = GetPolicyHolderInfo()
    '
    'End Sub


    'Private Sub SSPolicyRefFind_Click()
    '
    'm_lReturn& = GetPolicyInfo()
    '
    'End Sub
    ' PRIVATE Events (End)

    Private Sub txtRegistration_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegistration.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Me.cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtRegNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'If User Enters data enable find now if no data is
        'available in the other controls

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtRegNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNumber.Enter

        iPMFunc.SelectText(txtRegNumber)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)


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

                    'm_vLossFromDate = Format(dtDate, ACShortDate)

                    txtclaimstartdate.Tag = CStr(False)

                Case 1

                    'm_vLossToDate = Format(dtDate, ACShortDate)

                    txtclaimenddate.Tag = CStr(False)

            End Select


            sReturnValue = StringsHelper.Format(dtDate, ACDateDispaly)

        Else


            Select Case Controlnum
                Case 0

                    txtclaimstartdate.Tag = CStr(False)

                    'm_vLossFromDate = ""

                Case 1

                    txtclaimenddate.Tag = CStr(False)

                    'm_vLossToDate = ""

            End Select

            sReturnValue = Nothing

            result = gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result
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

    Public Function CheckDateDiff(ByRef vFromDate As String, ByRef vToDate As String) As Integer


        Dim nDiffDays As Double = DateAndTime.DateDiff("d", CDate(vFromDate), CDate(vToDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)

        If nDiffDays < 0 Then

            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            Return gPMConstants.PMEReturnCode.PMTrue
        End If

    End Function

    ' ***************************************************************** '
    ' Name: DisplayMessage
    '
    ' Description: Display the Suitable Message
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu

    ' ***************************************************************** '
    Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)

        Static sMessage As String = ""

        Try



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


    '' PUBLIC Methods (Begin)
    '' ***************************************************************** '
    '' Name: SetFieldValidation
    ''
    '' Description: Sets the rules for validating fields.
    ''
    '' ***************************************************************** '
    'Public Function SetFieldValidation() As Long
    '
    '    On Error GoTo Err_SetFieldValidation
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    'Claimref
    '    m_lReturn = m_oFormFields.AddNewFormField( _
    ''                   ctlControl:=txtClaimRef, _
    ''                   lFieldType:=PMString, _
    ''                   lFormat:=PMFormatString, _
    ''                   lMandatory:=PMMandatory)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetFieldValidation = PMFalse
    '        Exit Function
    '    End If
    '
    '     'Policy
    '    m_lReturn = m_oFormFields.AddNewFormField( _
    ''                   ctlControl:=txtPolicy, _
    ''                   lFieldType:=PMString, _
    ''                   lFormat:=PMFormatString, _
    ''                   lMandatory:=PMNonMandatory)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetFieldValidation = PMFalse
    '        Exit Function
    '    End If
    '
    '     'PolicyHolder
    '    m_lReturn = m_oFormFields.AddNewFormField( _
    ''                   ctlControl:=txtPolicyHolder, _
    ''                   lFieldType:=PMString, _
    ''                   lFormat:=PMFormatString, _
    ''                   lMandatory:=PMNonMandatory)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetFieldValidation = PMFalse
    '        Exit Function
    '    End If
    '
    '     'RegNumber
    '    m_lReturn = m_oFormFields.AddNewFormField( _
    ''                   ctlControl:=txtRegNumber, _
    ''                   lFieldType:=PMString, _
    ''                   lFormat:=PMFormatString, _
    ''                   lMandatory:=PMNonMandatory)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetFieldValidation = PMFalse
    '        Exit Function
    '    End If
    '
    '
    '
    '    'FromDate
    '    m_lReturn = m_oFormFields.AddNewFormField( _
    ''                             ctlControl:=txtClaimStartDate, _
    ''                             lFieldType:=PMString, _
    ''                             lFormat:=PMFormatDateLong, _
    ''                             lMandatory:=PMNonMandatory)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetFieldValidation = PMFalse
    '        Exit Function
    '    End If
    '
    '     'ToDate
    '    m_lReturn = m_oFormFields.AddNewFormField( _
    ''                             ctlControl:=txtClaimEndDate, _
    ''                             lFieldType:=PMString, _
    ''                             lFormat:=PMFormatDateLong, _
    ''                             lMandatory:=PMNonMandatory)
    '
    '    If m_lReturn <> PMTrue Then
    '        SetFieldValidation = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' ************************************************************
    '    ' Enter your code here to assign all of the controls to
    '    ' PMFormControl
    '    '
    '    ' Example:-
    '    '
    '    '        ' Pass control and required settings to FormControl
    '    '        m_lReturn = m_oFormFields.AddNewFormField( _
    ''    '                       ctlControl:=<Control Name>, _
    ''    '                       lFieldType:=<PM field type>, _
    ''    '                       lFormat:=<PM format string>, _
    ''    '                       lMandatory:=<PMMandatory or PMNonMandatory)
    '    '
    '    '        'Error checking
    '    '        If m_lReturn <> PMTrue Then
    '    '          SetFieldValidation = PMFalse
    '    '          Exit Function
    '    '        End If
    '    '
    '    ' NOTE: Replace this section with your new code.
    '    ' ************************************************************
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    SetFieldValidation = PMTrue
    '
    '    Exit Function
    '
    'Err_SetFieldValidation:
    '
    '    ' Error Section.
    '
    '    SetFieldValidation = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''    iType:=PMLogOnError, _
    ''    sMsg:="Failed to SetFieldValidation", _
    ''    vApp:=ACApp, _
    ''    vClass:=ACClass, _
    ''    vMethod:="SetFieldValidation", _
    ''    vErrNo:=Err.Number, _
    ''    vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    '
    ' ***************************************************************** '
    ' Name: ValidateIndex
    '
    ' Description: Validates the interface index.
    '
    '   RWH 17/04/2001 amended copy of Back Office Gis search
    ' ***************************************************************** '
    Private Function ValidateIndex() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sIndex As String = ""
        Dim vGISSearchDataArray(,) As Object

        Try

            Const csGISDataModelTypeClaim As String = "CLAIM"

            result = gPMConstants.PMEReturnCode.PMTrue

            sIndex = txtRegNumber.Text.Trim()

            'DC310701 changed ACMaxSearchDetails to PMAllRecords

            lReturn = g_oBackofficelink.FindLikeIndex(sIndex:=sIndex, lNumberOfRecords:=gPMConstants.PMAllRecords, vResultArray:=vGISSearchDataArray, sDataModelType:=csGISDataModelTypeClaim)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetAllGISSearchResults", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex")

                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' We have the Insurance File Cnt

                lReturn = g_oBusiness.GetMultiPolicyClaims(vGISSearchDataArray, m_vSearchData, v_vSiriusProduct:=m_lSiriusUnderWritingBroking, v_vClaimNumber:=txtClaimRef.Text, v_vClientName:=txtPolicyHolder.Text, v_vPolicyNumber:=txtPolicy.Text, v_vRegNumber:=txtRegNumber.Text, v_vLossFromdate:=m_vLossFromDate, v_vLossToDate:=m_vLossToDate, v_vClaimStatus:=m_bClaimStatus)

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




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LockClaim
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function LockClaim() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""
        Dim lOriginalClaimId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="LockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_lReturn = g_oBusiness.GetOriginalClaimId(m_lClaimCnt, lOriginalClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimId Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="LockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If


            m_lReturn = oPMLock.LockKey(sKeyName:="claim_id", vKeyValue:=lOriginalClaimId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy, v_bOtherUserOnly:=False)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK
                    m_lRealClaimID = lOriginalClaimId
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse

                        m_bRefreshClaimData = gPMConstants.PMEReturnCode.PMTrue

                        MessageBox.Show("Claim currently locked by " & sLockedBy &
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Find Claim")

                        Return result
                    End If


                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the screen", vApp:=ACApp, vClass:=ACClass, vMethod:="LockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Function UnLockClaim() As Integer
        Dim result As Integer
        Dim lOriginalClaimId As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = g_oBusiness.GetOriginalClaimId(m_lClaimCnt, lOriginalClaimId)
            'Tracy Richards - Protect against trying to unlcok claims of id = 0,
            'which may be the case for brand new claims, but which do not need unlocking.
            If lOriginalClaimId > 0 Then
                If g_oBusiness.UnLockKey(v_sKeyName:="claim_id", v_nKeyValue:=CInt(lOriginalClaimId), v_nUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: Claim_ID" & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(lOriginalClaimId) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

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


    Private Function ProcessFSA(ByRef r_bProceed As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessFSA"

        Dim lSelectedItem As Integer
        Dim sClientCode As String = ""
        Dim lPartyTypeID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Do we need to show the Data Protection Act questions
            If m_bDPAIsActive And chkDPARequired.CheckState = CheckState.Checked And m_bAskDPAQuestions Then


                lSelectedItem = Convert.ToString(lvwsearchdetails.Items.Item(lvwsearchdetails.FocusedItem.Index).Tag)

                sClientCode = CStr(m_vSearchData(17, lSelectedItem)).Trim()


                m_lReturn = m_oParty.GetPartyCnt(vPartyRef:=sClientCode, vPartyCnt:=m_lPartyCnt)

                m_lReturn = m_oParty.GetDetails(vPartyCnt:=m_lPartyCnt)

                m_lReturn = m_oParty.GetNext(vPartyTypeID:=lPartyTypeID)

                m_lReturn = m_oParty.GetPartyType(vPartyTypeID:=lPartyTypeID, vPartyTypeCode:=m_sPartyType)

                'If FSA compliance is enabled then check why the user is viewing the client
                m_lReturn = CType(ProcessFSAAccess(lPartyCnt:=m_lPartyCnt, sPartyType:=m_sPartyType, bProceed:=r_bProceed), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessFSAAccess", "Failed FSA Customer Validate.", gPMConstants.PMELogLevel.PMLogError)
                End If

                'User is proceeding
                If r_bProceed Then
                    'Store this setting so that we can read it when using the recent files in CM
                    m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ShowFSA", v_sSettingValue:="1"), gPMConstants.PMEReturnCode)
                End If

            Else

                r_bProceed = True

                'Store this setting so that we can read it when using the recent files in CM
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ShowFSA", v_sSettingValue:="0"), gPMConstants.PMEReturnCode)

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

    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "") As Integer

        Dim result As Integer = 0
        ' developer guide no. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 108
            oFindParty = New iPMBFindParty.Interface_Renamed()

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = CType(oFindParty.SetKeys(vKeyArray), gPMConstants.PMEReturnCode)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lErrorNumber = CType(CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = "Claims"
            oFindParty.NotEditable = 1

            m_lErrorNumber = CType(oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lErrorNumber = CType(oFindParty.Start(), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName


                vName = oFindParty.LongName

                vResolvedName = oFindParty.ResolvedName
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()
            oFindParty = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimVersion
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function GetClaimVersion() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimVersion"

        Dim lReturn As Integer
        Dim ofrmClaimVersions As frmClaimVersions

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create new instance of the form
            ofrmClaimVersions = New frmClaimVersions()

            ' set process modes
            ofrmClaimVersions.Task = m_iTask
            ofrmClaimVersions.Navigate = m_lNavigate
            ofrmClaimVersions.ProcessMode = m_lProcessMode
            ofrmClaimVersions.EffectiveDate = m_dtEffectiveDate
            ofrmClaimVersions.TransactionType = m_sTransactionType

            ' pass in the currently selected claim id
            ofrmClaimVersions.ClaimId = m_lClaimCnt
            ofrmClaimVersions.SelectedClaimId = m_lSelectedClaimCnt
            ofrmClaimVersions.ClaimNumber = m_sClaimRef

            ' load the form into memory


            If Me.Visible Then
                Me.Hide()
            End If
            ' display the claim versions form
            ofrmClaimVersions.ShowDialog()

            ' if the user hit ok...
            If ofrmClaimVersions.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' set the selected claims versions details in the main modular variables
                m_lRiskTypeId = ofrmClaimVersions.RiskCnt
                m_lClaimCnt = ofrmClaimVersions.ClaimId
                m_sClaimRef = ofrmClaimVersions.ClaimNumber
                m_lInsuranceFilecnt = ofrmClaimVersions.InsuranceFileCnt
                m_sPolicyRef = ofrmClaimVersions.InsuranceRef
                m_sPolicyHolder = ofrmClaimVersions.InsuranceHolder
                m_bRecovery = ofrmClaimVersions.Recovery ' PN 58569
                m_bIncludeClosedClaims = ChkCLosedClaim.CheckState = CheckState.Checked

            Else
                ' the user cancelled so stall process and await for new selection
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                result = gPMConstants.PMEReturnCode.PMCancel
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' unload the form
            ofrmClaimVersions.Close()

            ' destroy the claim versions form
            ofrmClaimVersions = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    Private Function GetProductDetails() As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "GetProductDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_ProductBusiness As bSIRProduct.Business
        Dim vProductDetails As Object
        Dim bIs_Multiple_claims_payments As Boolean
        Dim iMax_unauthorised_No_claim_payments As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_o_ProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Sankar - (WPRvb64 Media Type Status) - Paralleling
            'Added the parameter m_bCheckMediaTypeStatusAtClaimPayment

            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimCnt, r_bIs_Multiple_claims_payments:=bIs_Multiple_claims_payments, r_iMax_unauthorised_no_claim_payments:=iMax_unauthorised_No_claim_payments, r_bCheckMediaTypeStatusAtClaimPayment:=m_bCheckMediaTypeStatusAtClaimPayment)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            m_bMultipleClaimPayments = bIs_Multiple_claims_payments
            m_iMaxNoofUnAuthorisedClaimPayments = iMax_unauthorised_No_claim_payments




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Private Function ProcessPolicyReceiptMediatTypeStatus(ByVal v_lInsuranceFileId As Integer, ByVal v_dtLossDate As Date, ByRef r_bProceed As Boolean) As Integer
        Dim result As Integer = 0
        Dim bCLMFindClaim As Object

        Const kMethodName As String = "ProcessPolicyReceiptMediatTypeStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_FindClaimBusiness As bCLMFindClaim.Business
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_o_FindClaimBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_o_FindClaimBusiness, "bCLMFindClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_FindClaimBusiness = temp_o_FindClaimBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bCLMFindClaim.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = o_FindClaimBusiness.ProcessPolicyReceiptMediaTypeStatus(v_lInsuranceFileId:=v_lInsuranceFileId, v_dtLossDate:=v_dtLossDate, r_bProceed:=r_bProceed)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute bCLMFindClaim.ProcessPolicyReceiptMediaTypeStatus", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' terminate the object

            o_FindClaimBusiness.Dispose()
            o_FindClaimBusiness = Nothing



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

    Public Function GetUserOtherParty() As Long
        Dim vResult(,) As Object
        Dim iResult As Integer

        Try


            m_lReturn = g_oBusiness.GetUserOtherParty(iUserID:=g_iUserID, r_vResultArray:=vResult)

            If IsArray(vResult) Then
                If vResult(1, 0) <> "" Then
                    txtTPA.Text = vResult(2, 0)
                    cmdTPA.Enabled = False
                    txtTPA.ReadOnly = True
                Else
                    cmdTPA.Enabled = True
                End If
            Else
                cmdTPA.Enabled = True
            End If


        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Other Party ID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherParty", excep:=ex)
        Finally

        End Try

        GetUserOtherParty = iResult

    End Function

    Private Sub cmdTPA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTPA.Click
        Dim vCnt, vName, vShortName As Object
        Dim vResolvedName As String = ""


        Try

            m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=vShortName, vResolvedName:=vResolvedName, vSpecialParty:="OTTPA")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            txtTPA.Tag = CStr(vCnt)

            txtTPA.Text = vShortName



        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentCode_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub




    Private Sub txtTPA_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTPA.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Me.cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub
End Class
