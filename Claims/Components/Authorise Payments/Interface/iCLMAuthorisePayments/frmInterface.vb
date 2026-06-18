Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No.: 129
Imports SharedFiles
Imports System.Runtime.InteropServices

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 11th June 2002
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '   11062002 SJP - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    '   These are used for import export
    Private Const sCOption As String = "Option"
    Private Const sCValue As String = "Value"
    Private Const sCTotalNumber As String = "TotalNumber="

    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMAuthorisePayments.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oChangeCLMStatus As Object

    Private m_oUserAuthorities As bACTUserAuthorities.Business
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    '   This will determine if should exit
    Private m_bExit As Boolean


    'List array
    Private m_vAuthorisePayments(,) As Object
    Private m_vBranches As Object
    Private m_lClaimId As Integer
    Private m_sClaimNumber As String = ""
    Private m_sInsuranceRef As String = ""
    'AAB-05-Mar-2004 10:39 - Changed to currency
    Private m_cPaymentAmount As Decimal
    Private m_sOriginalUser As String = ""
    Private m_vSearchData(,) As Object
    Private m_sClaimStatus As String = ""
    ' START CHANGES - Changed By: AAB  - Changed On: 01-Dec-2003 10:54
    Private m_lOriginalUserID As Integer
    Private m_lPaymentID As Integer
    Private Const ACClaimPaymentsType As Integer = 1
    Private Const ACPaymentsType As Integer = 2
    Private Const ACTClaimAdminTaskGroupID As Integer = 10
    ' END CHANGES - Changed By: AAB  - Changed On: 01-Dec-2003 10:54

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private Const ACAuthoriseTask As String = "Authorise Claim Payments"

    Private m_bClaimPaymentWorkflowEnabled As Boolean

    Private m_lPaymentPayeePartyCnt As Integer
    Private m_lPaymentCurrencyId As Integer
    Private m_lPaymentDocumentId As Integer
    Private m_lPaymentMediaTypeId As Integer
    Private m_lPaymentPayeeAccountId As Integer
    Private m_bNavCompleted As Boolean
    Private m_bProcessComplete As Boolean
    Private m_iIsReferredForRecommendation As Integer
    Private m_sRecommender As String = ""
    Private m_iProcessId As Integer

    Private m_crRecommenderCurrAmount As Decimal
    Private m_iRecommenderCurrencyID As Integer
    Private m_iIsRecommender As Integer
    Private m_iHasClaimPaymentauthority As Integer
    Private m_iClaimPaymentCurrencyID As Integer
    Private m_crClaimPaymentCurrAmount As Decimal
    Private m_sPayeeName As String = String.Empty


    Private m_obACTCurrencyConvert As bACTCurrencyConvert.Form


    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed

    Private m_oProductBusiness As bSIRProduct.Business
    Private m_lProductID As Integer

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Private hScrollValue As Integer = 0
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

    ' Stores the details from the business object.

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

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)


    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

    <DllImport("user32.dll")> _
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwItems.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwItems.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwItems.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwItems.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub


    ' PRIVATE Property Procedures (End)




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

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            If m_iIsRecommender = 0 Then
                cmdRecommend.Enabled = False
            End If
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ******************************************************'
    '
    ' Name: dataToInterface
    '
    ' Description: This is used for the General class only
    '               It will do nothing
    '
    '*******************************************************'
    Public Function dataToProperties() As Integer

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ******************************************************'
    '
    ' Name: dataToInterface
    '
    ' Description: This will refresh the list view based on
    '               the values in the array
    ''
    '*******************************************************'
    Public Function dataToInterface() As Integer
        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sKey, sText As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear the list
            lvwItems.Items.Clear()

            ' If we don't have anything then exit
            If Not Information.IsArray(m_vAuthorisePayments) Then
                Return result
            End If

            '   This adds all Options with the relevant Branch Id
            For iLoop As Integer = 0 To m_vAuthorisePayments.GetUpperBound(1)
                ' Generate a key
                ' Alix - 03/02/2004 - PN9901
                ' We can't use the claim id as a unique key! We might have
                ' two payments against the same claim!
                'sKey = "X" + Trim(CStr(m_vAuthorisePayments(ACColClaimId, iLoop)))
                sKey = "X" & CStr(m_vAuthorisePayments(ACColPaymentID, iLoop)).Trim()
                ' /Alix
                ' Get the text to use
                sText = CStr(m_vAuthorisePayments(ACColClaimNumber, iLoop)).Trim()

                ' Add the item to the list view
                lstItem = lvwItems.Items.Add(sKey, sText, "")
                lstItem.SubItems.Add(0).Text = CStr(m_vAuthorisePayments(ACColPolicyNumber, iLoop))
                lstItem.SubItems.Add(1).Text = CStr(m_vAuthorisePayments(ACColClientName, iLoop))
                lstItem.SubItems.Add(2).Text = CStr(Convert.ToDouble(m_vAuthorisePayments(ACColPaymentAmount, iLoop)).ToString("N2"))
                lstItem.SubItems.Add(3).Text = CStr(Convert.ToDateTime(m_vAuthorisePayments(ACColPaymentDate, iLoop)))
                lstItem.SubItems.Add(4).Text = CStr(m_vAuthorisePayments(ACColCreatedBy, iLoop))
                lstItem.SubItems.Add(5).Text = CStr(m_vAuthorisePayments(ACColStatus, iLoop))
                lstItem.SubItems.Add(6).Text = CStr(m_vAuthorisePayments(ACColIsReferredforRecommendationId, iLoop))
                lstItem.SubItems.Add(7).Text = CStr(m_vAuthorisePayments(ACColRecommenderId, iLoop))
                lstItem.SubItems.Add(8).Text = CStr(m_vAuthorisePayments(ACCOlClaimPaymentCurrencyId, iLoop))

                lstItem.Tag = CStr(iLoop) 'PN37004 (RC) - store array index in Item's Tag

            Next iLoop

            ' Auto size the list
            'Developer Guide No. 178
            m_lReturn = ListView6Func.ListViewAutoSize(lvwList:=lvwItems, bSizeHeaders:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' No great shakes
            End If
            lvwItems.Columns.Item(8).Width = CInt(0)
            lvwItems.Columns.Item(9).Width = CInt(0)
            ' Disable buttons (will only be enabled when user selects a claim from the list)
            cmdView.Enabled = False
            cmdAuthorise.Enabled = False
            cmdDecline.Enabled = False

            'PayeeName Column width by default should not be greater than Client Name column
            lvwItems.Columns(3).Width = IIf(lvwItems.Columns(3).Width > lvwItems.Columns(2).Width,
                                            lvwItems.Columns(2).Width, lvwItems.Columns(3).Width)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="dataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="dataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ''' <summary>
    ''' Recommend a Claim Paymen
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdRecommend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRecommend.Click
        Dim bPMLock As Object

        Const kMethodName As String = "cmdRecommend_Click"

        Dim sTitle As String = ""
        Dim bIsReferredForRecommendation, bCanProceedForAuthorisation, bCanProceedForRecommendation As Boolean
        Dim iNoofPaymentsWaitingRecommendation As Integer
        Dim bIsReferredForAuthorisation As Boolean
        Dim cReserveAmount As Decimal
        Dim sLockedBy As String = ""

        Dim oPMLock As bPMLock.User
        Dim vCashListItemClaimLinkDetails As Object
        Dim crConvertedCurrency As Decimal

        Try

            m_iProcessId = kClaimPaymentAuthProcessRecommend

            If m_sClaimStatus <> "Pending" Then
                MessageBox.Show("This Payment can not be recommended, please check the current status", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Get description from the resource file.

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. 52
            If UCase(RTrim(lvwItems.FocusedItem.SubItems(5).Text)) = UCase(RTrim(g_oObjectManager.UserName)) Then
                MessageBox.Show("Cannot Proceed - Unable to recommend claim payments raised by yourself", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            m_lReturn = GetReferredClaimStatus(bIsReferredForAuthorisation, m_lPaymentID, bIsReferredForRecommendation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdRecommend_Click", "GetReferredClaimStatus failed")
            End If

            If Not bIsReferredForRecommendation Then
                MessageBox.Show("This payment cannot be recommended, please check the current status.", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If

            If m_lPaymentCurrencyId <> g_oObjectManager.CurrencyID Then
                m_lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion( _
                                 v_lCurrencyIdFrom:=m_lPaymentCurrencyId, _
                                 v_crCurrencyAmountFrom:=m_cPaymentAmount, _
                                 v_lCompanyId:=g_iSourceID, _
                                 v_lCurrencyIdTo:=g_oObjectManager.CurrencyID, _
                                 r_crCurrencyAmountTo:=crConvertedCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("cmdRecommend_Click", "CurrencyToCurrencyConversion Failed")
                End If

                If m_crRecommenderCurrAmount < crConvertedCurrency Then
                    MessageBox.Show("Recommend limit is less than Payment Amount.", "Limits Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            If m_crRecommenderCurrAmount < gPMFunctions.ToSafeDouble(lvwItems.FocusedItem.SubItems(3).Text, 0) Then
                MessageBox.Show("Recommend limit is less than Payment Amount on Claim", "Limits Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If m_crRecommenderCurrAmount < cReserveAmount Then
                MessageBox.Show("Recommend limit is less than Gross Incurred on Claim.", "Limits Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'm_lReturn = CheckForPendingClaimPayments(r_bCanProceedForRecommendation:=bCanProceedForRecommendation, r_NoofPaymentsWaitingRecommendation:=iNoofPaymentsWaitingRecommendation)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("cmdRecommend_Click", "CheckForPendingClaimPayments failed")
            'End If

            'If Not bCanProceedForRecommendation Then
            '    MessageBox.Show("You cannot recommend this payment, as there are " & iNoofPaymentsWaitingRecommendation & " prior payments awaiting recommendation for this claim", "Claim Payment Authorisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    Exit Sub
            'End If

            'Lock the Record
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(Constants.vbObjectError), "LockInsurer", CType("Unable to get instance of bPMLock", gPMConstants.PMELogLevel))
            End If

            ' Lock the Claim_Payment Record
            m_lReturn = oPMLock.LockKey(sKeyName:="Claim_Payment", vKeyValue:=m_lPaymentID, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy, _
            v_bOtherUserOnly:=False)
            If sLockedBy <> "" Then
                MessageBox.Show("This payment is locked by " + sLockedBy & ".", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If



            If m_bClaimPaymentWorkflowEnabled Then
                m_lReturn = ProcessCashListPayment(v_lClaimPaymentId:=m_lPaymentID, v_iClaimPaymentAuthProcess:=kClaimPaymentAuthProcessRecommend)

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then

                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Process Cash List Item Payment Failed", "CashList Payment Processing Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else

                    m_lReturn = m_oBusiness.GetCashListItemClaimLinkDetails(v_lClaimPaymentId:=m_lPaymentID, r_vResults:=vCashListItemClaimLinkDetails)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to GetCashListItemClaimLinkDetails")
                    End If

                End If
            End If

            m_lReturn = m_oBusiness.SetClaimPaymentRecommendStatus(v_lClaimid:=m_lClaimId, v_istatus:=0, v_iUserID:=g_oObjectManager.UserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdRecommend_Click", "SetClaimPaymentReferredStstus Failed")
            End If
            MessageBox.Show("Payment Passed for Authorisation.", "Claim Payment ", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'set the status of these rows on the interface
            For iLoop As Integer = 0 To m_vAuthorisePayments.GetUpperBound(1)
                If CStr(m_vAuthorisePayments(ACColClaimNumber, iLoop)) = m_sClaimNumber And CDbl(m_vAuthorisePayments(ACColClaimId, iLoop)) = m_lClaimId Then
                    'm_vAuthorisePayments(ACColStatus, iLoop) = "Recommended"
                    m_vAuthorisePayments(ACColIsReferredforRecommendationId, iLoop) = ""
                    m_vAuthorisePayments(ACColRecommenderId, iLoop) = g_oObjectManager.UserName
                End If
            Next
            '    'set the status of these rows on the interface
            '    For iLoop = 0 To UBound(m_vAuthorisePayments, 2)
            '        If m_vAuthorisePayments(ACColClaimNumber, iLoop) = m_sClaimNumber _
            ''            And m_vAuthorisePayments(ACColClaimId, iLoop) = m_lClaimId Then
            '            'm_vAuthorisePayments(ACColStatus, iLoop) = "Recommended"
            '            m_vAuthorisePayments(ACColIsReferredforRecommendationId, iLoop) = ""
            '            m_vAuthorisePayments(ACColRecommenderId, iLoop) = g_oObjectManager.UserName
            '        End If
            '    Next
            m_lReturn = dataToInterface()
            'Added for the focus to remain on the form.
            Me.Select(True, True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = Unlock_Payment()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAuthorise Command failed to getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                Exit Sub
            End If


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdRecommend Command failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRecommend_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub
    ''' <summary>
    ''' cmdAuthorise_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdAuthorise_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAuthorise.Click
        Dim bPMLock, bCLMAuthorisePayments, iCLMChangeClaimStatus As Object

        Dim sDescription As String = ""
        Dim vKeyArray(,) As Object
        Dim oObject As Object
        Dim sTitle, sMessage As String

        Dim oStepAuthorization As bCLMAuthorisePayments.StepAuthorization
        Dim oChangeStatus As Object
        Dim sErrorMessage As String = ""
        Dim bLastStep As Boolean
        Dim vOptionValue As Object
        Dim sUserGroupCode, sTaskDescComplete As String
        Dim lTaskInstanceCnt As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults As Object
        Dim sLockedBy As String = ""

        Dim oPMLock As bPMLock.User
        Dim cReserveAmount As Decimal
        Dim bCanProceedForAuthorisation As Boolean
        Dim iNoofPaymentsWaitingAuthorisation As Integer
        Dim bIsReferredForAuthorisation, bIsReferredForRecommendation, bIsAlreadyReferredForRecommendation As Boolean
        Dim crConvertedCurrency As Decimal

        Try
            cmdAuthorise.Enabled = False
            m_iProcessId = kClaimPaymentAuthProcessAuthorise

            If m_sClaimStatus = "Pending" Or m_sClaimStatus = "Recommended" Then
            Else
                MessageBox.Show("This Payment can not be authorised, please check the current status", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Get description from the resource file.

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If UCase(RTrim(lvwItems.FocusedItem.SubItems(6).Text)) = UCase(RTrim(g_oObjectManager.UserName)) Then
                MessageBox.Show("Cannot Proceed - Unable to authorise claim payments raised by yourself", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If ToSafeLong(lvwItems.FocusedItem.SubItems(8).Text) = 1 Then
                MsgBox("Cannot Proceed - Claim Payment needs to be recommended first.", vbInformation, sTitle)
                Exit Sub
            End If

            If UCase(RTrim(lvwItems.FocusedItem.SubItems(9).Text)) = UCase(RTrim(g_oObjectManager.UserName)) Then
                MessageBox.Show("Cannot Proceed - Unable to authorise claim payments recommended by yourself", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            m_lReturn = GetReferredClaimStatus(bIsReferredForAuthorisation, m_lPaymentID, bIsReferredForRecommendation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReferredClaimStatus failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Not bIsReferredForAuthorisation Then
                MessageBox.Show("This payment cannot be authorised, please check the current status.", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If

            'PN67204-If already recommended and authorisation is true then incurred checked
            m_lReturn = GetAlreadyReferredClaimStatus(bIsReferredForAuthorisation, m_lPaymentID, bIsAlreadyReferredForRecommendation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdAuthorise_Click", "GetAlreadyReferredClaimStatus failed")
            End If

            If bIsReferredForRecommendation OrElse bIsAlreadyReferredForRecommendation And bIsReferredForAuthorisation Then

                If m_lPaymentCurrencyId <> g_oObjectManager.CurrencyID Then
                    m_lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion( _
                                     v_lCurrencyIdFrom:=m_lPaymentCurrencyId, _
                                     v_crCurrencyAmountFrom:=m_cPaymentAmount, _
                                     v_lCompanyId:=g_iSourceID, _
                                     v_lCurrencyIdTo:=g_oObjectManager.CurrencyID, _
                                     r_crCurrencyAmountTo:=crConvertedCurrency)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("cmdAuthorise_Click", "CurrencyToCurrencyConversion Failed")
                    End If

                    If m_crClaimPaymentCurrAmount < crConvertedCurrency Then
                        MessageBox.Show("Authorise limit is less than Payment Amount.", "Limits Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                End If



                If m_crClaimPaymentCurrAmount < cReserveAmount Then
                    MessageBox.Show("Authorise limit is less than Gross Incurred on Claim", "Limits Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            m_lReturn = CheckForPendingClaimPayments(r_bCanProceedForAuthorisation:=bCanProceedForAuthorisation, r_NoofPaymentsWaitingAuthorisation:=iNoofPaymentsWaitingAuthorisation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForPendingClaimPayments failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Not bCanProceedForAuthorisation Then
                MessageBox.Show("You cannot authorise this payment, as there are " & iNoofPaymentsWaitingAuthorisation & " prior payments awaiting authorisation for this claim", "Claim Payment Authorisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If m_crClaimPaymentCurrAmount < gPMFunctions.ToSafeDouble(lvwItems.FocusedItem.SubItems(3).Text, 0) Then
                MessageBox.Show("Authorise limit is less than Payment Amount on Claim", "Limits Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Lock the Record
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", LockInsurer, Unable to get instance of bPMLock")
            End If


            ' Lock the Claim_Payment Record
            m_lReturn = oPMLock.LockKey(sKeyName:="Claim_Payment", vKeyValue:=m_lPaymentID, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy, v_bOtherUserOnly:=False)
            If sLockedBy <> "" Then
                MessageBox.Show("This payment is locked by " + sLockedBy + ".", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACWarnAuthorise, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            If (MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) = System.Windows.Forms.DialogResult.No Then
                m_lReturn = Unlock_Payment()
                Exit Sub
            End If


            vResults = Nothing


            ' START CHANGES - Changed By: AAB  - Changed On: 10-Dec-2003 12:35
            ' To support ICB's RFC for Multi-step approval
            'developer guide no.98
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = Unlock_Payment()

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAuthorise Command failed to getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If


            Dim temp_oStepAuthorization As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oStepAuthorization, "bCLMAuthorisePayments.StepAuthorization", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oStepAuthorization = temp_oStepAuthorization
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = Unlock_Payment()

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAuthorise Command failed to Create StepAuthorization Class", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'set the properties of the object.

            oStepAuthorization.PaymentType = ACClaimPaymentsType

            oStepAuthorization.PaymentID = m_lPaymentID

            oStepAuthorization.PaymentAmount = Math.Abs(m_cPaymentAmount)

            oStepAuthorization.PaymentCreatorUserID = m_lOriginalUserID

            oStepAuthorization.PaymentCurrencyID = m_lPaymentCurrencyId

            oStepAuthorization.IsRecommenderOn = bIsAlreadyReferredForRecommendation

            m_lReturn = oStepAuthorization.ProcessApproval()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = Unlock_Payment()

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAuthorise Command failed to Process User Authroization", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Get the resutls of the process

            sErrorMessage = oStepAuthorization.ProcessErrorMessage

            bLastStep = oStepAuthorization.LastStep

            If sErrorMessage <> "" Then
                MessageBox.Show(sErrorMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_lReturn = Unlock_Payment()
                Exit Sub
            Else
                MessageBox.Show("You successfully completed the authorization step.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" And bIsAlreadyReferredForRecommendation = False Then


                If Not bLastStep Then
                    sUserGroupCode = ""

                    m_lReturn = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=sUserGroupCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAuthorise Command failed to Process GetStepGroupCode", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If


                    sErrorMessage = oStepAuthorization.ProcessErrorMessage
                    If sErrorMessage <> "" Then
                        m_lReturn = Unlock_Payment()
                        MessageBox.Show(sErrorMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If


                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing

                'Delete WTM

                m_lReturn = m_oBusiness.ProcessWTM(v_lClaimid:=m_lClaimId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_lReturn = Unlock_Payment()

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot create a Work Manager Task for this Authorisation. Check configuration.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                'Create the WTM for the next step.
                If bLastStep Then
                    sTaskDescComplete = "Your Claim Payment for Claim Number: " & m_sClaimNumber & " for the amount of:" & StringsHelper.Format(m_cPaymentAmount, "#,##0.00") & " has been approved."
                Else
                    sTaskDescComplete = "Authorise Claim Payment for Claim Number: " & m_sClaimNumber & " for the amount of: " & StringsHelper.Format(m_cPaymentAmount, "#,##0.00")
                End If

                ReDim vKeyArray(1, 0)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRealClaimID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId


                m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:="SYSTEM", v_sDescription:=sTaskDescComplete, v_dtTaskDueDate:=DateTime.Now, v_sTaskCode:="AUTHPMNT", v_sTaskGroupCode:="CLAIMADM", v_sUserGroupCode:=sUserGroupCode, v_vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_lReturn = Unlock_Payment()

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot create a Work Manager Task for this Authorisation. Check configuration.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                ' END CHANGES - Changed By: AAB  - Changed On: 02-Mar-2004 14:08
            Else
                'set bLaststep to True so we can continue as normal
                bLastStep = True

                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If

            If bLastStep Then
                ReDim vKeyArray(1, 1)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameOperateMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ACModeAuthorise

                Dim temp_oObject As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iCLMChangeClaimStatus.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oObject = temp_oObject


                m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oObject = Nothing
                    Exit Sub
                End If

                m_lReturn = oObject.SetProcessModes(vTransactionType:="C_CP")

                m_lReturn = oObject.Start()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oObject.Dispose()
                    oObject = Nothing
                    Exit Sub
                End If


                oObject.Dispose()
                oObject = Nothing


                ' if claim payment workflow is enabled then
                If m_bClaimPaymentWorkflowEnabled Then

                    lReturn = CType(ProcessCashListPayment(v_lClaimPaymentId:=m_lPaymentID, v_iClaimPaymentAuthProcess:=kClaimPaymentAuthProcessAuthorise), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Process Cash List Item Payment Failed", "CashList Payment Processing Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If

                End If

                sDescription = Interaction.InputBox("Enter your comments ", "Authorise Claim Payment")


                m_lReturn = m_oBusiness.CreateEvent(v_lEventType:=PMBConst.PMBEventClaChange, v_sDescription:=sDescription, v_lClaimid:=m_lClaimId, v_sOriginalUser:=m_sOriginalUser, v_sMode:="A")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Me.Cursor = Cursors.Default
                End If


                If sDescription.Trim() = "" Then
                    sDescription = "Payment of Claim - Payment Authorized"
                Else
                    sDescription = "Payment of Claim - Payment Authorized ,Comments - " & sDescription
                End If


                m_lReturn = m_oChangeCLMStatus.UpdateClaimDesc(v_lClaimid:=m_lClaimId, v_sClaimVersionDescription:=sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdDecline_Click", "UpdateClaimDesc Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                'Set the status of payments to authorised

                m_lReturn = m_oBusiness.ProcessAuthorise(m_lClaimId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to process authorise.", "Error")
                    Exit Sub
                End If

                m_lReturn = Unlock_Payment()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot Unlock the Payment.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                End If

                'set the status of these rows on the interface
                For iLoop As Integer = 0 To m_vAuthorisePayments.GetUpperBound(1)
                    If CDbl(m_vAuthorisePayments(ACColPaymentID, iLoop)) = m_lPaymentID Then
                        m_vAuthorisePayments(ACColStatus, iLoop) = "Authorised"
                    End If
                Next

                m_lReturn = dataToInterface()

                If (MessageBox.Show("Do you wish to print Claim Payment Advice?", "Documents", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) = System.Windows.Forms.DialogResult.Yes Then

                    ReDim vKeyArray(1, 0)


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId


                    Dim temp_oObject2 As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oObject2, sClassName:="iCLMGetClaimDocuments.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oObject = temp_oObject2


                    m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        oObject = Nothing
                        Exit Sub
                    End If

                    m_lReturn = oObject.SetProcessModes(vTransactionType:="C_CP")

                    m_lReturn = oObject.Start()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        oObject.Dispose()
                        oObject = Nothing
                        Exit Sub
                    End If


                    oObject.Dispose()
                    oObject = Nothing
                End If
            End If

            ' If everything goes well refresh the List
            'Get authorise payments list
            m_lReturn = m_oBusiness.GetReferredList(m_vAuthorisePayments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn

                ' Log Error.
                iPMFunc.LogMessage( _
                    iType:=gPMConstants.PMELogLevel.PMLogError, _
                    sMsg:="Failed to get the referred-list from business object", _
                    vApp:=ACApp, _
                    vClass:=ACClass, _
                    vMethod:="Form_Load")
                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            ' END CHANGES - Changed By: AAB  - Changed On: 10-Dec-2003 12:35


        Catch ex As Exception

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
                oObject = Nothing
            End If

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAuthorise Command failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            cmdAuthorise.Enabled = True
        End Try
    End Sub
    Private Function Unlock_Payment() As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object

        Dim oPMLock As bPMLock.User

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the lock object
        Dim temp_oPMLock As Object
        result = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPMLock = temp_oPMLock
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockInsurer, Unable to get instance of bPMLock")
        End If
        ' Unlock the current insurer
        result = oPMLock.UnLockKey(sKeyName:="Claim_Payment", vKeyValue:=m_lPaymentID, iUserID:=g_oObjectManager.UserID)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockInsurer, " + CStr(CDbl("Failed to unlock insurer. Account id: ") + m_lPaymentID))
        End If

        Return result
    End Function


    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
        Try

            m_bExit = True
            Me.Close()

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Exit Command failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdDecline_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDecline.Click

        Dim sDescription As String = ""
        Dim sTitle, sMessage As String


        Dim oStepAuthorization As bCLMAuthorisePayments.StepAuthorization
        Dim sErrorMessage As String = ""
        Dim bLastStep As Boolean
        Dim vOptionValue As Object
        Dim sTaskDescComplete As String = ""
        Dim lTaskInstanceCnt As Integer
        Dim vKeyArray(,) As Object
        Dim vResultArray(,) As Object
        Dim sClaimVersionDescription As String = ""
        Dim vResults As Object

        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""
        Dim bIsReferredForAuthorisation As Boolean
        Dim vCashListItemClaimLinkDetails As Object
        Dim lCashListItemId As Integer
        Dim bCanProceedForAuthorisation As Boolean
        Dim iNoofPaymentsWaitingAuthorisation As Integer
        Dim bIsAlreadyReferredForRecommendation As Boolean
        Try
            cmdDecline.Enabled = False
            If m_sClaimStatus = "Pending" Or m_sClaimStatus = "Recommended" Then
            Else
                MessageBox.Show("This Payment can not be declined, please check the current status", "Status")
                Exit Sub
            End If
            m_iProcessId = 1
            m_lReturn = CheckForPendingClaimPayments(r_bCanProceedForAuthorisation:=bCanProceedForAuthorisation, r_NoofPaymentsWaitingAuthorisation:=iNoofPaymentsWaitingAuthorisation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForPendingClaimPayments failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Not bCanProceedForAuthorisation Then
                MessageBox.Show("You cannot decline this payment, as there are " & iNoofPaymentsWaitingAuthorisation & " prior payments awaiting authorisation for this claim", "Claim Payment Authorisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'PN 45917
            '    m_lReturn = m_oBusiness.GetClaimStatus(m_lClaimId, vResultArray)
            '    If m_lReturn <> PMFalse Then
            '        If Trim(vResultArray(1, 0)) = "CLOSED" Then
            '          ' Is status is found closed then claim status is set to "reopened"
            '            m_lReturn = m_oBusiness.Update_Claim_Status(m_lClaimId)
            '          If m_lReturn <> PMTrue Then
            '           LogMessage iType:=PMLogOnError, _
            ''                   sMsg:="cmdDecline Command failed to Update Claim Status", _
            ''                   vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_click", _
            ''                   vErrNo:=Err.Number, vErrDesc:=Err.Description
            '           End If
            '
            '            Set vResultArray = Nothing
            '        End If
            '    End If

            ' Get description from the resource file.

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACWarnDecline, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            m_lReturn = GetReferredClaimStatus(bIsReferredForAuthorisation, m_lPaymentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReferredClaimStatus failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Not bIsReferredForAuthorisation Then
                MessageBox.Show("This payment cannot be declined, please check the current status.", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If
            m_lReturn = GetAlreadyReferredClaimStatus(bIsReferredForAuthorisation, m_lPaymentID, bIsAlreadyReferredForRecommendation)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("cmdDecline_Click", "GetAlreadyReferredClaimStatus failed")
            End If

            'Lock the Record
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", LockInsurer, Unable to get instance of bPMLock")
            End If


            ' Lock the Claim_Payment Record

            m_lReturn = oPMLock.LockKey(sKeyName:="Claim_Payment", vKeyValue:=m_lPaymentID, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy, v_bOtherUserOnly:=False)
            If sLockedBy <> "" Then
                MessageBox.Show("This payment cannot be declined, please check the current status.", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If

            ' Display message.
            If (MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo)) = System.Windows.Forms.DialogResult.No Then
                m_lReturn = Unlock_Payment()
                Exit Sub
            End If


            vResults = Nothing
            'Check the Referred Claim Status Again
            m_lReturn = GetReferredClaimStatus(bIsReferredForAuthorisation, m_lPaymentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReferredClaimStatus failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAuthorise_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Not bIsReferredForAuthorisation Then
                MessageBox.Show("This payment cannot be authorised, please check the current status.", sTitle, MessageBoxButtons.OK)
                Exit Sub
            End If

            ' We only need to check the user group if multi-step approval is not available

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = Unlock_Payment()

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline Command failed to getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" Then
                Dim temp_oStepAuthorization As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oStepAuthorization, "bCLMAuthorisePayments.StepAuthorization", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oStepAuthorization = temp_oStepAuthorization

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_lReturn = Unlock_Payment()

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline Command failed to Create StepAuthorization Class", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'set the properties of the object.

                oStepAuthorization.PaymentType = ACClaimPaymentsType

                oStepAuthorization.PaymentID = m_lPaymentID

                oStepAuthorization.PaymentAmount = Math.Abs(m_cPaymentAmount)

                oStepAuthorization.PaymentCreatorUserID = m_lOriginalUserID
                oStepAuthorization.IsRecommenderOn = bIsAlreadyReferredForRecommendation

                m_lReturn = oStepAuthorization.ProcessDecline()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline Command failed to Process User Authroization", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                ' Get the resutls of the process

                sErrorMessage = oStepAuthorization.ProcessErrorMessage

                bLastStep = oStepAuthorization.LastStep


                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing

                m_lReturn = Unlock_Payment()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline_Click Command failed to Unlock_Payment", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                If sErrorMessage <> "" Then
                    MessageBox.Show(sErrorMessage, sTitle, MessageBoxButtons.OK)
                    Exit Sub
                End If

                'Delete WTM

                m_lReturn = m_oBusiness.ProcessWTM(v_lClaimid:=m_lClaimId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline_Click Command failed to ProcessWTM", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'Create the WTM for the next step.
                sTaskDescComplete = "Your Claim Payment for Claim Number: " & m_sClaimNumber & " for the amount of: " & StringsHelper.Format(m_cPaymentAmount, "#,##0.00") & " has been declined."

                ReDim vKeyArray(1, 0)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRealClaimID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId


                m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:="SYSTEM", v_sDescription:=sTaskDescComplete, v_dtTaskDueDate:=DateTime.Now, v_sTaskCode:="MEMO", v_sTaskGroupCode:="CLAIMADM", v_vKeyArray:=vKeyArray, v_iUserID:=m_lOriginalUserID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline_Click Command failed to ProcessWTM", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            End If

            ' We will continue if there are no error for step approval
            ' Alix - 02/02/2004 - Added m_lPaymentID parameter

            ' Check whether Cash List Item details Exists againt Claim_payment_id

            m_lReturn = m_oBusiness.GetCashListItemClaimLinkDetails(v_lClaimPaymentId:=m_lPaymentID, r_vResults:=vCashListItemClaimLinkDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdDecline_Click", "Failed to GetCashListItemClaimLinkDetails")
            End If
            If Information.IsArray(vCashListItemClaimLinkDetails) Then

                lCashListItemId = gPMFunctions.ToSafeLong(CStr(vCashListItemClaimLinkDetails(1, 0)))
                m_lReturn = ReverseCashListItem(lCashListItemId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdDecline_Click", "Failed to ReverseCashListItem")
                End If

            End If


            m_lReturn = m_oBusiness.ProcessDecline(m_lClaimId, m_lPaymentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to process decline.", "Error")
                Exit Sub
            End If

            sDescription = Interaction.InputBox("Enter Reason for Decline ", "Decline Claim Payment")

            m_lReturn = m_oBusiness.CreateEvent(v_lEventType:=PMBConst.PMBEventClaChange, v_sDescription:=sDescription, v_lClaimid:=m_lClaimId, v_sOriginalUser:=m_sOriginalUser, v_sMode:="D")

            If sDescription.Trim() = "" Then
                sDescription = "Payment of Claim - Payment Declined"
            Else
                sDescription = "Payment of Claim - Payment Declined ,Comments - " & sDescription
            End If


            m_lReturn = m_oChangeCLMStatus.UpdateClaimDesc(v_lClaimid:=m_lClaimId, v_sClaimVersionDescription:=sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdDecline_Click", "UpdateClaimDesc Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            For iLoop As Integer = 0 To m_vAuthorisePayments.GetUpperBound(1)
                If CDbl(m_vAuthorisePayments(ACColPaymentID, iLoop)) = m_lPaymentID Then
                    m_vAuthorisePayments(ACColStatus, iLoop) = "Declined"
                End If
            Next

            ' If everything goes well refresh the List
            'Get authorise payments list
            m_lReturn = m_oBusiness.GetReferredList(m_vAuthorisePayments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn

                ' Log Error.
                iPMFunc.LogMessage( _
                    iType:=gPMConstants.PMELogLevel.PMLogError, _
                    sMsg:="Failed to get the referred-list from business object", _
                    vApp:=ACApp, _
                    vClass:=ACClass, _
                    vMethod:="Form_Load")
                Exit Sub
            End If

            m_lReturn = dataToInterface()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Me.Cursor = Cursors.Default
                Exit Sub
            End If


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDecline_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="err_cmdDecline_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally
            cmdDecline.Enabled = True
        End Try
    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            ''m_lReturn& = updateOptions(m_vAuthorisePayments, m_bSiriusInstaller)

            cmdExit_Click(cmdExit, New EventArgs())

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdOK_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Try

            If m_sClaimNumber.Trim() <> "" Then
                m_lReturn = ShowClaim()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdView_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: Form_Initialize
    '
    ' Description: This occurs if we initialise the form
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Dim vOptionValue As Object
        Dim sValue As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMAuthorisePayments.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            Dim temp_m_oChangeCLMStatus As Object
            If g_oObjectManager.GetInstance(temp_m_oChangeCLMStatus, "bCLMChangeClaimStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oChangeCLMStatus = temp_m_oChangeCLMStatus

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub

            Else
                m_oChangeCLMStatus = temp_m_oChangeCLMStatus
            End If

            Dim temp_m_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserAuthorities = temp_m_oUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_initalize", "Failed to create instance of UserAuthorities")
            End If

            Dim temp_m_obACTCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_obACTCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_obACTCurrencyConvert = temp_m_obACTCurrencyConvert
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_initalize", "Failed to create instance of CurrencyConvert")
            End If

            ' START CHANGES - Changed By: AAB  - Changed On: 10-Dec-2003 12:10
            ' We only need to check the user group if multi-step approval is not available
            'developer guide no.98
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show("Unable to Get Product Option!!", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                m_oBusiness = Nothing
                Exit Sub
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" Then
                'then we do not need to check the user group
            Else
                'AK 110603 check if user is authorised to run this task?
                'AAB-10-Dec-2003 12:11 - Changed this to allow sysadmin to open it as well

                m_lReturn = m_oBusiness.CheckUserGroup
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    MessageBox.Show("User not authorised to run this task!!", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    m_oBusiness = Nothing
                    Exit Sub
                End If
            End If
            ' END CHANGES - Changed By: AAB  - Changed On: 10-Dec-2003 12:10

            Dim temp_m_oProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oProductBusiness = temp_m_oProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Initalize", "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMAuthorisePayments.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            'Set m_oFormFields = New iPMFormControl.FormFields

            ' Set language
            'm_oFormFields.LanguageID = g_iLanguageID%

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
        Try
            Dim vResult(,) As Object
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
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

            m_lReturn = GetUserAuthorities()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Load", "Failed to get User Authorities")
            End If



            ' Set full row select and grid lines (it breaks up the white space)
            'developer guide no.(This doesnt work so use the dot net property to achieve the same)
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwItems.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=True)
            lvwItems.FullRowSelect = True
            lvwItems.GridLines = True

            ' {* USER DEFINED CODE (End) *}

            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Get authorise payments list
            m_lReturn = m_oBusiness.GetUserOtherParty(iUserID:=g_iUserID, r_vResultArray:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Other Party from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If
            If IsArray(vResult) Then
                If vResult(0, 0) <> "" Then
                    m_lReturn = m_oBusiness.GetReferredList(m_vAuthorisePayments, vResult(0, 0))
                Else
                    m_lReturn = m_oBusiness.GetReferredList(m_vAuthorisePayments)
                End If

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the referred-list from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
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

            If UnloadMode <> vbFormCode Or m_bExit Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    eventArgs.Cancel = True
                    Cancel = 1

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
            'm_lReturn& = m_oFormFields.Terminate()

            ' Check for errors.
            'If (m_lReturn& <> PMTrue) Then
            '    m_lErrorNumber& = PMFalse
            'End If

            ' Destroy the instance of the form control object
            ' from memory.
            'Set m_oFormFields = Nothing

            'Set m_vBranches = Nothing

            m_oUserAuthorities.Dispose()

            m_obACTCurrencyConvert.Dispose()
            ' Check for errors.
            m_vAuthorisePayments = Nothing

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

    Private Function SortListView(ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell it that it's not sorted
            ListViewHelper.SetSortedProperty(lvwItems, False)

            ' Set the column to sort on
            ListViewHelper.SetSortKeyProperty(lvwItems, v_iIndex)

            ' Swap the ascending/descending around
            If ListViewHelper.GetSortOrderProperty(lvwItems) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwItems, SortOrder.Descending)
            Else
                ListViewHelper.SetSortOrderProperty(lvwItems, SortOrder.Ascending)
            End If

            ' Tell it that it's now sorted
            ListViewHelper.SetSortedProperty(lvwItems, True)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SortListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SortListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwItems_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwItems.Click

        Dim iPos As Integer
        Try

            If Information.IsArray(m_vAuthorisePayments) Then


                iPos = Convert.ToString(lvwItems.FocusedItem.Tag) 'PN37004 (RC) - retrive array index from Item's Tag
                m_lClaimId = CInt(m_vAuthorisePayments(ACColClaimId, iPos))
                m_cPaymentAmount = CDec(m_vAuthorisePayments(ACColPaymentAmount, iPos))
                m_sOriginalUser = CStr(m_vAuthorisePayments(ACColCreatedBy, iPos)).Trim()
                m_sClaimNumber = CStr(m_vAuthorisePayments(ACColClaimNumber, iPos)).Trim()
                m_sInsuranceRef = CStr(m_vAuthorisePayments(ACColPolicyNumber, iPos)).Trim()
                m_sClaimStatus = CStr(m_vAuthorisePayments(ACColStatus, iPos)).Trim()
                m_lPaymentID = CInt(Conversion.Val(CStr(m_vAuthorisePayments(ACColPaymentID, iPos))))
                m_lOriginalUserID = CInt(Conversion.Val(CStr(m_vAuthorisePayments(ACColOriginalUserID, iPos))))
                m_iIsReferredForRecommendation = gPMFunctions.ToSafeInteger(CStr(m_vAuthorisePayments(ACColIsReferredforRecommendationId, iPos)), 0)
                m_sRecommender = gPMFunctions.ToSafeString(CStr(m_vAuthorisePayments(ACColRecommenderId, iPos)), "")
                m_lProductID = gPMFunctions.ToSafeLong(CStr(m_vAuthorisePayments(ACColProductIDId, iPos)))
                m_lPaymentCurrencyId = gPMFunctions.ToSafeLong(CStr(m_vAuthorisePayments(ACCOlClaimPaymentCurrencyId, iPos)))
                m_sPayeeName = ToSafeString(m_vAuthorisePayments(KACCOlPayeeName, iPos)).Trim()
                cmdView.Enabled = True
                cmdAuthorise.Enabled = True
                cmdDecline.Enabled = True

                'If m_bSysOptRecommender = False Then
                If m_iIsRecommender = 0 Then
                    cmdRecommend.Enabled = False
                Else
                    cmdRecommend.Enabled = m_iIsReferredForRecommendation = 1
                End If
            End If
            m_lReturn = GetClaimPaymentWorkFlow()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwItems_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwItems_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub lvwItems_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwItems.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwItems.Columns(eventArgs.Column)
        StoreHScrollValue()
        Dim iDirection As Integer

        If ColumnHeader.Index = 4 Then ' if its a paymentdate column
            If ListViewHelper.GetSortOrderProperty(lvwItems) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwItems, SortOrder.Descending)
            Else
                ListViewHelper.SetSortOrderProperty(lvwItems, SortOrder.Ascending)
            End If
            iDirection = ListViewHelper.GetSortOrderProperty(lvwItems)
            m_lReturn = ListView6Func.ListViewSortByDate(v_oListView:=lvwItems, _
                                           v_iSourceColumn:=4, _
                                           v_iDirection:=iDirection)
        Else
            ' Sort the data
            m_lReturn = SortListView(v_iIndex:=ColumnHeader.Index + 1 - 1)
        End If
        RecoverHorizontalScroll()
    End Sub
    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose
        m_bNavCompleted = True
    End Sub

    Private Sub m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavStart.SetProcessStatus
        m_bProcessComplete = v_bProcessComplete
    End Sub

    ' ***************************************************************** '
    ' Name: ShowClaim
    '
    ' Description: Displays claim information.
    '
    ' ***************************************************************** '
    Public Function ShowClaim() As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim m_oOpenClaim As iOpenClaim.Interface_Renamed
        Dim m_oFindClaim As bCLMFindClaim.Business

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vKeyArray(1, 6) As Object
            Dim temp_m_oFindClaim As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindClaim, "bCLMFindClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oFindClaim = temp_m_oFindClaim

            ' changes for underwriting - get claim details correctly

            m_lReturn = m_oFindClaim.GetClaimDetailsUW(r_vResultArray:=m_vSearchData, v_vSiriusProduct:=g_sProduct, v_vClaimNumber:=m_sClaimNumber, v_vPolicyNumber:=m_sInsuranceRef, v_vClientName:="", v_vLossFromdate:="", v_vLossToDate:="", v_vClaimStatus:="")



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vSearchData) Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Claim details from FindClaim", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                m_oFindClaim.Dispose()
                m_oFindClaim = Nothing
                Return result
            End If

            m_lClaimId = CInt(m_vSearchData(ACIClmClaimId, 0))

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRiskTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CInt(m_vSearchData(ACIClmRiskTypeId, 0))


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lClaimId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimDate

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = CDate(m_vSearchData(ACIClmClaimDateU, 0))


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameOperateMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = gPMConstants.PMEComponentAction.PMView


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNamePolicyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = CInt(m_vSearchData(ACIClmPolicyId, 0))


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNamePolicyNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_sInsuranceRef


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientHolder

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = CStr(m_vSearchData(ACIClmClientNameU, 0)).Trim()

            If m_oOpenClaim Is Nothing Then
                ' Get instance of Open Claim Object
                Dim temp_m_oOpenClaim As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oOpenClaim, sClassName:="iOpenClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oOpenClaim = temp_m_oOpenClaim

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iOpenClaim.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_lReturn = m_oOpenClaim.SetKeys(vKeyArray:=vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oOpenClaim = Nothing
                Return result
            End If

            'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.1.1.1)

            m_oOpenClaim.ShowPaymentView = True
            'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.1.1.1)


            m_lReturn = m_oOpenClaim.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oOpenClaim = Nothing

                m_oFindClaim.Dispose()
                m_oFindClaim = Nothing
                Return result
            End If


            m_oOpenClaim.Dispose()



            m_oOpenClaim = Nothing


            m_oFindClaim.Dispose()
            m_oFindClaim = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            If Not (m_oFindClaim Is Nothing) Then

                m_oFindClaim.Dispose()
                m_oFindClaim = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateCashListPayment
    '
    ' Parameters: n/a
    '
    ' Description: Start the navigator process to create a cash list
    '               for the specified claim payment
    '
    ' History:
    '           Created : MEvans : 18-01-2006 : Cheque Production Workflow
    ' ***************************************************************** '
    Public Function CreateCashListPayment(ByVal v_lClaimPaymentId As Integer, ByVal v_crClaimPaymentAmount As Decimal, ByVal v_lMediaTypeId As Integer, ByVal v_lAccountID As Integer, ByVal v_lCurrencyId As Integer, ByVal v_vDocumentIds As Object, ByVal v_lSourceId As Integer, ByVal v_iClaimPaymentAuthProcess As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateCashListPayment"

        Dim vCashListItemClaimLinkDetails As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lClaimPaymentId As Integer
        Dim vCashListItemID As String = ""
        Dim vKeyArray(1, 22) As Object
        Dim bCashListItemLinkExists As Boolean
        Dim lCashListItemCurrencyID As Integer
        Dim cCashListItemAmount As Decimal
        Dim lCompanyID, lMediaTypeId, lCashListId As Integer
        Try



            ' Check whether Cash List Item details Exists againt Claim_payment_id

            lReturn = m_oBusiness.GetCashListItemClaimLinkDetails(v_lClaimPaymentId:=v_lClaimPaymentId, r_vResults:=vCashListItemClaimLinkDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetCashListItemClaimLinkDetails")
            End If

            lClaimPaymentId = v_lClaimPaymentId

            If Information.IsArray(vCashListItemClaimLinkDetails) Then
                bCashListItemLinkExists = True

                lCashListId = CInt(gPMFunctions.ToSafeCurrency(CStr(vCashListItemClaimLinkDetails(0, 0))))

                vCashListItemID = gPMFunctions.ToSafeString(CStr(vCashListItemClaimLinkDetails(1, 0)))

                lCashListItemCurrencyID = CInt(gPMFunctions.ToSafeCurrency(CStr(vCashListItemClaimLinkDetails(2, 0))))

                lCompanyID = gPMFunctions.ToSafeLong(CStr(vCashListItemClaimLinkDetails(3, 0)))

                cCashListItemAmount = gPMFunctions.ToSafeLong(CStr(vCashListItemClaimLinkDetails(4, 0)))

                lMediaTypeId = gPMFunctions.ToSafeLong(CStr(vCashListItemClaimLinkDetails(5, 0)))

            Else
                bCashListItemLinkExists = False
            End If

            ' create an instance of navigator xm
            m_oNavStart = New iPMNavStart.Interface_Renamed()

            ' initialise it
            'Developer Guide No: 10
            lReturn = m_oNavStart.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise iPMNavStart.Interface")
            End If

            ' set its properties
            m_oNavStart.CallingAppName = ACApp

            ' set the process to start
            m_oNavStart.ProcessCode = "ACTPAYV2"

            'The XML roadmap to use
            m_oNavStart.NavXMLFile = "ACTPAYV2.XML"

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "cash_list_roadmap"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "PAYMENTS"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTotalPremium

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_crClaimPaymentAmount


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lCurrencyId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameDocumentID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_vDocumentIds


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameMediaTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = v_lMediaTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_lAccountID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameScreenType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = "CLP"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.ACTKeyAllowAllocateButton

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = 0


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.ACTKeyNameCashListItemMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = 2


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameSourceId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = v_lSourceId

            If v_iClaimPaymentAuthProcess = kClaimPaymentAuthProcessRecommend Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameRecommendation

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = "T"


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameClaimPaymentId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = lClaimPaymentId

            ElseIf v_iClaimPaymentAuthProcess = kClaimPaymentAuthProcessAuthorise Then
                If bCashListItemLinkExists Then

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameRecommendation

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = "F"


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameClaimPaymentId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = lClaimPaymentId


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.ACTKeyNameCashListId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = vCashListItemID


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.ACTKeyNameCashListItemAmount

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = cCashListItemAmount


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.ACTKeyNameCashListProcessAbort

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = 0


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.ACTKeyNameTransactionCurrencyID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = lCashListItemCurrencyID


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.ACTKeyNameCashListAllocationRoadmap

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = "ACTRCTV22P"


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNameCurrentNashStep

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = 1


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 18) = PMNavKeyConst.ACTKeyNameCashListTypeId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 18) = 3


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 19) = PMNavKeyConst.ACTKeyNameBranchID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 19) = lCompanyID


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 20) = PMNavKeyConst.ACTKeyNameMediaTypeID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 20) = lMediaTypeId


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 21) = PMNavKeyConst.ACTKeyNameCashListId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 21) = lCashListId


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 22) = PMNavKeyConst.PMKeyNameClaimPaymentId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 22) = m_lPaymentID
                Else

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameRecommendation

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = ""


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 22) = PMNavKeyConst.PMKeyNameClaimPaymentId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 22) = m_lPaymentID
                End If
            End If

            ' set the navigators processes keys
            lReturn = m_oNavStart.SetKeys(vKeyArray:=vKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMNavStart.Interface.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' default the navigation completed actions to false
            m_bProcessComplete = False
            m_bNavCompleted = False

            ' start the specified navigator process
            lReturn = m_oNavStart.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUNavStart.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' wait while the navigator process is completed
            Do
                Application.DoEvents()
            Loop While Not m_bNavCompleted



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' terminate this instance of the navigator process
            m_oNavStart.Dispose()
            ' clean up the object instances
            m_oNavStart = Nothing

            ' if the process is now complete


        End Try
        If m_bProcessComplete Then
            ' indicate the procedure was successfully run
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            ' indicate the procedure failed to complete
            Return gPMConstants.PMEReturnCode.PMCancel
        End If
    End Function

    ' ***************************************************************** '
    ' Region: m_m_oNavStart Events
    ' ***************************************************************** '

    'Private Sub m_m_oNavStart_NavigatorClose()
    'm_bNavCompleted = True
    'End Sub


    'Private Sub m_m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean)
    'm_bProcessComplete = v_bProcessComplete
    'End Sub

    ' ***************************************************************** '
    ' Name: ProcessCashListPayment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-01-2006 : ATD16 - Cheque Production Workflow
    ' ***************************************************************** '
    Public Function ProcessCashListPayment(ByVal v_lClaimPaymentId As Integer, ByRef v_iClaimPaymentAuthProcess As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCashListPayment"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAccountsPaymentDetails As Object
        Dim lMediaTypeId, lAccountId, lCurrencyId As Integer
        Dim crTotalPaymentAmount As Decimal
        Dim vDocumentIds As Object
        Dim lDocumentId, lSourceId As Integer
        Dim nPaymentID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the required accounts details for the specified claim payment

            lReturn = m_oBusiness.GetClaimPaymentAccountsDetails(v_lClaimPaymentId:=v_lClaimPaymentId, r_vResults:=vAccountsPaymentDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentAccountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if there is no data returned for the specified claim payment id
            If Not Information.IsArray(vAccountsPaymentDetails) Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentAccountsDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get data from array

            nPaymentID = gPMFunctions.ToSafeLong(vAccountsPaymentDetails(kClaimPaymentAccountDetailsClaimPaymentId, 0), 0)

            lMediaTypeId = gPMFunctions.ToSafeLong(CStr(vAccountsPaymentDetails(kClaimPaymentAccountDetailsMediaTypeID, 0)), 0)

            lAccountId = gPMFunctions.ToSafeLong(CStr(vAccountsPaymentDetails(kClaimPaymentAccountDetailsAccountId, 0)), 0)

            lCurrencyId = gPMFunctions.ToSafeLong(CStr(vAccountsPaymentDetails(kClaimPaymentAccountDetailsCurrencyId, 0)), 0)

            crTotalPaymentAmount = gPMFunctions.ToSafeCurrency(CStr(vAccountsPaymentDetails(kClaimPaymentAccountDetailsTotalPaymentAmount, 0)), 0)

            lDocumentId = gPMFunctions.ToSafeLong(CStr(vAccountsPaymentDetails(kClaimPaymentAccountDetailsDocumentID, 0)), 0)

            lSourceId = gPMFunctions.ToSafeLong(CStr(vAccountsPaymentDetails(kClaimPaymentAccountDetailsSourceId, 0)), 0)

            If lDocumentId = 0 Then

                vDocumentIds = Nothing
            Else
                ReDim vDocumentIds(0)

                vDocumentIds(0) = lDocumentId
            End If

            ' start the cash list navigation process to allow the
            ' creation of the cash list payment for the specified claim payment
            lReturn = CType(CreateCashListPayment(v_lClaimPaymentId:=m_lPaymentID, v_crClaimPaymentAmount:=crTotalPaymentAmount, v_lMediaTypeId:=lMediaTypeId, v_lAccountID:=lAccountId, v_lCurrencyId:=lCurrencyId, v_vDocumentIds:=vDocumentIds, v_lSourceId:=lSourceId, v_iClaimPaymentAuthProcess:=v_iClaimPaymentAuthProcess), gPMConstants.PMEReturnCode)
            If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel

            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' indicate to the user that the process failed
                MessageBox.Show("Create Cash List Payment Failed.", "Authorise Claim Payment Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If


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

    '*************************************************************************************************
    '
    'Name        :  CheckForPendingClaimPayments
    'Description :  Check for Claim_Payments on the same Claim that needs Authorisation
    ' If any Such payment exists then only the First one should be allowed to Authorise
    '
    '*************************************************************************************************

    Private Function CheckForPendingClaimPayments(Optional ByRef r_bCanProceedForAuthorisation As Boolean = False, Optional ByRef r_NoofPaymentsWaitingAuthorisation As Integer = 0, Optional ByRef r_bCanProceedForRecommendation As Boolean = False, Optional ByRef r_NoofPaymentsWaitingRecommendation As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckForPendingClaimPayments"

        Dim lReturn As Integer
        Dim bIsReferredForAuthorisation, bIsReferredForRecommendation As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            If m_iProcessId = kClaimPaymentAuthProcessAuthorise Then
                r_bCanProceedForAuthorisation = True

                If Information.IsArray(m_vAuthorisePayments) Then
                    For iPos As Integer = 0 To m_vAuthorisePayments.GetUpperBound(1)

                        If m_sClaimNumber = CStr(m_vAuthorisePayments(ACColClaimNumber, iPos)).Trim() And m_lClaimId > gPMFunctions.ToSafeLong(CStr(m_vAuthorisePayments(ACColClaimId, iPos))) Then
                            m_lReturn = GetReferredClaimStatus(r_bIsReferredForAuthorisation:=bIsReferredForAuthorisation, lClaimPaymentId:=CInt(m_vAuthorisePayments(ACColPaymentID, iPos)))

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CheckForPendingClaimPayments", "CheckForPendingClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            If bIsReferredForAuthorisation Then
                                r_bCanProceedForAuthorisation = False
                                r_NoofPaymentsWaitingAuthorisation += 1
                            End If
                        End If
                    Next
                End If
            Else

                r_bCanProceedForRecommendation = True

                If Information.IsArray(m_vAuthorisePayments) Then
                    For iPos As Integer = 0 To m_vAuthorisePayments.GetUpperBound(1)
                        If m_sClaimNumber = CStr(m_vAuthorisePayments(ACColClaimNumber, iPos)).Trim() And m_lClaimId > gPMFunctions.ToSafeLong(CStr(m_vAuthorisePayments(ACColClaimId, iPos))) Then
                            m_lReturn = GetReferredClaimStatus(r_bIsReferredForAuthorisation:=bIsReferredForRecommendation, lClaimPaymentId:=CInt(m_vAuthorisePayments(ACColPaymentID, iPos)))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CheckForPendingClaimPayments", "CheckForPendingClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            If bIsReferredForRecommendation Then
                                r_bCanProceedForRecommendation = False
                                r_NoofPaymentsWaitingRecommendation += 1
                            End If
                        End If
                    Next
                End If

            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    '***************************************************************************
    'Name              :           GetReferredClaimStatus
    '
    'Description       :           Get the Claim Payment status
    '
    '***************************************************************************
    Private Function GetReferredClaimStatus(ByRef r_bIsReferredForAuthorisation As Boolean, ByVal lClaimPaymentId As Integer, Optional ByRef r_bIsReferredForRecommendation As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReferredClaimStatus"
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            r_bIsReferredForAuthorisation = False
            r_bIsReferredForRecommendation = False


            m_lReturn = m_oBusiness.GetReferredClaimStatus(v_lClaimPaymentId:=lClaimPaymentId, r_vResults:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to getClaim Progress Status", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReferredClaimStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Information.IsArray(vResults) Then

                r_bIsReferredForAuthorisation = Not (CStr(vResults(0, 0)) = "0")


                r_bIsReferredForRecommendation = Not (CStr(vResults(1, 0)) = "0" Or CStr(vResults(1, 0)).Trim() = "")

            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function GetReserveTotalForClaimPayment(ByVal lClaimPaymentId As Integer, ByRef r_cReserveAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReserveTotalForClaimPayment"
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetReserveTotalForClaimPayment(v_lClaimPaymentId:=lClaimPaymentId, r_vResults:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to get GetReserveTotalForClaimPayment")
            End If

            If Information.IsArray(vResults) Then

                r_cReserveAmount = gPMFunctions.ToSafeCurrency(CStr(vResults(0, 0)))
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function GetUserAuthorities() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserAuthorities"
        Dim vResults(,) As Object
        Dim crConvertedCurrency As Decimal
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oUserAuthorities.GetUserAuthoritiesDetails(v_lUserId:=g_oObjectManager.UserID, r_vResults:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to get User Authorities Details")
            End If

            If Information.IsArray(vResults) Then

                m_iIsRecommender = gPMFunctions.ToSafeInteger(CStr(vResults(ACIsRecommender, 0)))

                m_iRecommenderCurrencyID = gPMFunctions.ToSafeInteger(CStr(vResults(ACRecommenderCurrency, 0)))

                m_crRecommenderCurrAmount = gPMFunctions.ToSafeCurrency(CStr(vResults(ACRecommenderCurrAmount, 0)))


                m_iHasClaimPaymentauthority = gPMFunctions.ToSafeInteger(CStr(vResults(ACHasClaimPaymentauthority, 0)))

                m_iClaimPaymentCurrencyID = gPMFunctions.ToSafeInteger(CStr(vResults(ACClaimPaymentCurrencyID, 0)))

                m_crClaimPaymentCurrAmount = gPMFunctions.ToSafeCurrency(CStr(vResults(ACClaimPaymentAmount, 0)))
            End If

            If m_iIsRecommender = 1 And g_oObjectManager.CurrencyID <> m_iRecommenderCurrencyID Then

                m_lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=m_iRecommenderCurrencyID, v_crCurrencyAmountFrom:=m_crRecommenderCurrAmount, v_lCompanyId:=g_iSourceID, v_lCurrencyIdTo:=g_oObjectManager.CurrencyID, r_crCurrencyAmountTo:=crConvertedCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetUserAuthorities", "CurrencyToCurrencyConversion Failed")
                End If

                m_crRecommenderCurrAmount = crConvertedCurrency
            End If

            If m_iHasClaimPaymentauthority = 1 And g_oObjectManager.CurrencyID <> m_iClaimPaymentCurrencyID Then

                m_lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=m_iClaimPaymentCurrencyID, v_crCurrencyAmountFrom:=m_crClaimPaymentCurrAmount, v_lCompanyId:=g_iSourceID, v_lCurrencyIdTo:=g_oObjectManager.CurrencyID, r_crCurrencyAmountTo:=crConvertedCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetUserAuthorities", "CurrencyToCurrencyConversion Failed")
                End If

                m_crClaimPaymentCurrAmount = crConvertedCurrency
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    Private Function ReverseCashListItem(ByRef lPaymentID As Integer) As Integer
        Dim result As Integer = 0
        Dim bACTDocumentReversal As Object
        Dim lTransDetailID As Integer
        Dim vResults(,) As Object

        Dim oACTDocumentReversal As bACTDocumentReversal.Business

        Dim nAllocationStatusID As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetTransDetailFromCashListItem(v_lCashListItemId:=lPaymentID, r_vResults:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ReverseCashListItem", "GetTransDetailFromCashListItem Failed")
            End If
            If Information.IsArray(vResults) Then
                nAllocationStatusID = gPMFunctions.ToSafeInteger(CStr(vResults(1, 0)))
                lTransDetailID = gPMFunctions.ToSafeLong(CStr(vResults(9, 0)))
            End If
            If nAllocationStatusID <> 1 Then
                Dim temp_oACTDocumentReversal As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oACTDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oACTDocumentReversal = temp_oACTDocumentReversal



                oACTDocumentReversal.TransDetailId = lTransDetailID

                oACTDocumentReversal.IsCashlistItemReversal = True

                m_lReturn = oACTDocumentReversal.Start

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ReverseCashListItem", "oACTDocumentReversal.Start Failed")
                End If

            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ReverseCashListItem", r_lFunctionReturn:=result, v_sUsername:=g_oObjectManager.UserName, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function GetClaimPaymentWorkFlow() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentWorkFlow"
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oProductBusiness.GetClaimWorkflow(r_vResults:=vResults, v_lProductId:=m_lProductID, v_lWorkflowID:=3)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Information.IsArray(vResults) Then
                m_bClaimPaymentWorkflowEnabled = gPMFunctions.ToSafeBoolean(vResults(14, 0))
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function GetAlreadyReferredClaimStatus(ByRef r_bIsReferredForAuthorisation As Boolean, ByVal lClaimPaymentId As Integer, Optional ByRef r_bIsAlreadyReferredForRecommendation As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAlreadyReferredClaimStatus"
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            r_bIsReferredForAuthorisation = False
            r_bIsAlreadyReferredForRecommendation = False


            m_lReturn = m_oBusiness.GetAlreadyReferredClaimStatus(v_lClaimPaymentId:=lClaimPaymentId, r_vResults:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to getClaim Progress Status", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReferredClaimStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Information.IsArray(vResults) Then

                r_bIsReferredForAuthorisation = Not (CStr(vResults(0, 0)) = "0")


                If CStr(vResults(1, 0)) = "0" Or CStr(vResults(1, 0)).Trim() = "" Then
                    r_bIsAlreadyReferredForRecommendation = False
                Else
                    'Already Recommended
                    r_bIsAlreadyReferredForRecommendation = True
                End If

            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabLossSchedule.SelectedIndex = 0
        End If
    End Sub
End Class
