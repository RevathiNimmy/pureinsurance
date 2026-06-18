Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmRecovery
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Form Name: frmRecovery
    '
    ' Date:15/07/00
    '
    ' Description: Main interface.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmRecovery"

    ' Constant for listview icon
    Private Const ACFindImage As String = "FindImage"
    'Developer Guide no. 7
    Private Const vbFormCode As Integer = 0
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.3)
    ' Enumerator for columns
    Private Enum RecoveryColumnEnum
        'Standard columns for receipt
        rceRecoveryType = 0
        rceRecoveryPartyType = 1
        rceRecoveryParty = 2
        rceReserve = 3
        rceReceivedToDate = 4
        rceThisReceipt = 5
        rceBalance = 6
        rceTaxBand = 7
        rceTaxAmount = 8
        rceNetAmount = 9
        'Alternate use columns for reserve
        rceInitialReserve = 3
        rceRevisedReserve = 4
        rceThisReserve = 5
        ''Saurabh PN56294 - Added New Constant Received to date
        rceReceivedtodateReserve = 6
    End Enum
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.3)
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Recovery properties
    Private m_sUniqueID As String = ""
    Private m_lRecoveryID As Integer
    Private m_sRecoveryType As String = ""
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private m_lRecoveryPartyTypeId As Integer
    Private m_lRecoveryPartyCnt As Integer
    Private m_sRecoveryPartyType As String = ""
    Private m_sRecoveryParty As String = ""
    Private m_sRecoveryPartyDesc As String = ""
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private m_lRecoveryTypeID As Integer
    Private m_cInitialReserve As Decimal
    Private m_cRevisedReserve As Decimal
    Private m_cThisReserve As Decimal
    Private m_sReceiptCurrency As String = ""
    Private m_lReceiptCurrencyID As Integer
    Private m_dCurrencyRate As Double
    Private m_cReceivedToDate As Decimal
    Private m_cThisReceipt As Decimal
    Private m_cBalance As Decimal
    Private m_sTaxType As String = ""
    Private m_lTaxTypeID As Integer
    Private m_sTaxBand As String = ""
    Private m_lTaxBandID As Integer
    Private m_cTaxAmount As Decimal
    Private m_cNetReceipt As Decimal
    Private m_bIsPostTaxes As Boolean

    ' Coinsurance and reinsurance details for current recovery
    Private m_vCoinsurance(,) As Object
    Private m_vReinsurance(,) As Object

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMRecovery.General

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Currency flags
    Private m_bAllowCurrencyRateOverride As Boolean
    Private m_bReceiptCurrencySet As Boolean


    ' ***************************************************************** '
    '                       PUBLIC PROPERTIES
    '
    ' Note:
    '      Public PerilID As Long
    ' is the SAME as:
    '      Public Property Let PerilID(ByVal RHS As Long)
    '      End Property
    '      Public Property Get PerilID() As Long
    '      End Property
    ' ***************************************************************** '

    Public InsuranceFileCnt As Integer
    Public CompanyID As Integer

    Public ClaimId As Integer
    Public ClaimNumber As String = ""
    Public ClientName As String = ""

    Public PerilID As Integer
    Public PerilType As String = ""
    Public PerilTypeID As Integer

    Public LossCurrency As String = ""
    Public LossCurrencyID As Integer

    Public ClassOfBusiness As String = ""
    Public ClassOfBusinessID As Integer

    Public IsSalvage As Boolean
    Private m_bIsRecoveriesReadOnly As Boolean = False

    ' ***************************************************************** '
    '                       STANDARD PROPERTIES
    ' ***************************************************************** '
    ''' <summary>
    ''' Holds the flag for recoveryReadonly configured in product maintenanc.
    ''' </summary>
    Public WriteOnly Property IsRecoveriesReadOnly() As Boolean
        Set(ByVal value As Boolean)
            m_bIsRecoveriesReadOnly = value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property


    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
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
            ' Set the objects task.
            m_iTask = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property


    ' ***************************************************************** '
    '                         PUBLIC FUNCTIONS
    ' ***************************************************************** '

    ' Check if all reserves total zero and prompt to close claim
    Public Function CheckCurrentReserve() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String
        Dim vDataArray(,) As Object

        Const kMethodName As String = "CheckCurrentReserve"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get current reserve balances

            lReturn = g_oBusiness.GetCurrentReserveRecovery(ClaimId, vDataArray)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Information.IsArray(vDataArray) Then
                gPMFunctions.RaiseError("g_oBusiness.GetCurrentReserveRecovery()", "Failed to check current claim reserves")
            End If

            ' Check if claim is already closed

            If CStr(vDataArray(0, 0)) = "3" Then
                Return result
            End If

            ' Check the current reserve.

            If gPMFunctions.ToSafeCurrency(CStr(vDataArray(1, 0))) <> 0 Then
                Return result
            End If

            ' Check the current recovery reserve

            If gPMFunctions.ToSafeCurrency(CStr(vDataArray(2, 0))) <> 0 Then
                Return result
            End If

            ' Confirmation before closing the claim

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDialogCloseClaimTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDialogCloseClaimDetail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            If MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                Return result
            End If

            ' Close the Claim

            lReturn = g_oBusiness.CloseClaim(ClaimId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oBusiness.CloseClaim()", "Failed to close claim")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' Check if the supplied recovery type is already in use
    Public Function CheckRecoveryTypeID(ByRef lRecoveryTypeID As Integer) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        For Each oListItem As ListViewItem In lvwRecovery.Items
            ' Reserve type is stored in the list items tag :-)
            If lRecoveryTypeID = Convert.ToString(oListItem.Tag) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Exit For
            End If
        Next oListItem

        Return result
    End Function

    ' Updates all interface details from the business object.
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BusinessToInterface"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the business object to the data storage.
            lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
            Do While lReturn = gPMConstants.PMEReturnCode.PMTrue
                ' Add the new item to the list

                'developer guide no.49  and (No with block needed)
                Dim olistItem As ListViewItem
                olistItem = lvwRecovery.Items.Add("r" & m_lRecoveryID, m_sRecoveryType, ACFindImage)
                ' Populate appropriate columns
                ' Note: All "This" values are assumed to be zero for screen load!!
                If (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReserve) Or (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReserve) Then
                    ' Set sub items
                    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    If m_sRecoveryParty = "" Then
                        'Start Girija - PN 54404

                        ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceRecoveryParty - 1).Text = ACPartyTypeDefaultCaption
                        'End Girija - PN 54404
                    Else

                        ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceRecoveryParty - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sRecoveryPartyDesc.Trim())
                    End If

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceInitialReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cInitialReserve)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceRevisedReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cRevisedReserve)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceThisReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceBalance - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cInitialReserve + m_cRevisedReserve - m_cReceivedToDate)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceReceivedtodateReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cReceivedToDate)
                    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                Else
                    ' Set sub items

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cInitialReserve + m_cRevisedReserve)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceReceivedToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cReceivedToDate)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceThisReceipt).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceBalance).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cBalance)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceTaxBand).Text = m_sTaxBand.Trim()

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceTaxAmount).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)

                    ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceNetAmount).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)
                End If

                ' Set id and select the item


                olistItem.Tag = CStr(m_lRecoveryTypeID)
                'End With

                ' Assign the details from the business object to the data storage.
                lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
            Loop

            ' Select the first item
            If lvwRecovery.Items.Count > 0 Then
                lvwRecovery.Items.Item(0).Selected = True
            End If

            ' Refresh the insurer details
            PopulateInsurers()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' Retrieves the details from the business object.
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            m_lReturn = g_oBusiness.GetDetails(vPerilId:=PerilID, vIsSalvage:=IsSalvage)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
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


    ' Get receipt party details
    Public Function GetReceiptPartyID(ByVal cAmount As Decimal, ByRef lReceiptPartyId As Integer, ByRef sReceiptPartyName As String, ByRef sComments As String) As Integer
        Dim result As Integer = 0
        Dim iCLMPaymentMethod As Object

        Const kMethodName As String = "GetReceiptPartyID"
        Dim lReturn As Integer


        Dim oReceiptMethod As iCLMPaymentMethod.Interface_Renamed
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the client and agent for this policy
            ' Note: do this first as we already have this object

            lReturn = g_oBusiness.GetClientAgentID(v_lWorkClaimId:=ClaimId, r_vResultArray:=vResultArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oBusiness.GetClientAgentID()", "Failed to get client and agent details.")
            End If

            ' Create Find Party object
            Dim temp_oReceiptMethod As Object
            lReturn = g_oObjectManager.GetInstance(temp_oReceiptMethod, sClassName:="iCLMPaymentMethod.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReceiptMethod = temp_oReceiptMethod
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to get instance of iCLMPaymentMethod.Interface")
            End If

            ' Set component properties

            oReceiptMethod.CallingAppName = ACApp

            oReceiptMethod.ScreenMethod = 1

            oReceiptMethod.Amount = cAmount

            oReceiptMethod.CurrencyID = m_lReceiptCurrencyID

            oReceiptMethod.ClaimID = ClaimId

            oReceiptMethod.InsuranceFileCnt = InsuranceFileCnt


            oReceiptMethod.ClientID = gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0)))


            oReceiptMethod.ClientName = gPMFunctions.ToSafeString(CStr(vResultArray(1, 0)))


            oReceiptMethod.AgentID = gPMFunctions.ToSafeLong(CStr(vResultArray(2, 0)))


            oReceiptMethod.AgentName = gPMFunctions.ToSafeString(CStr(vResultArray(3, 0)))


            oReceiptMethod.ProductID = gPMFunctions.ToSafeLong(CStr(vResultArray(4, 0)))

            ' Start interface

            lReturn = oReceiptMethod.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oReceiptMethod.Start()", "Failed to start interface")
            End If

            ' Check button pressed

            If oReceiptMethod.ButtonClicked <> gPMConstants.PMEReturnCode.PMOK Then
                result = gPMConstants.PMEReturnCode.PMCancel
            Else

                ' Retrieve data

                lReceiptPartyId = oReceiptMethod.Partyid

                sReceiptPartyName = oReceiptMethod.PartyName

                sComments = oReceiptMethod.Comments
            End If

            ' Destroy Find Party object

            oReceiptMethod.Dispose()
            oReceiptMethod = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' Get posting details for the receipt
    Public Function GetReceiptPostingDetails(ByRef lReceiptPartyCnt As Integer, ByRef sAccountCode As String, ByRef sMappingCode As String, ByRef sReceiptComments As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Form_Load"
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim cReceiptTotal As Decimal
        Dim sPostReceiptToSuspense As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the receipt amount and check we have something to post

            lReturn = g_oBusiness.GetReceiptTotal(r_cReceiptTotal:=cReceiptTotal)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oBusiness.GetReceiptTotal()", "Failed to check receipt total")
            End If

            If cReceiptTotal <> 0 Then
                ' Check if we post to suspense
                lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=ACOptionPostReceiptToSuspense, r_sOptionValue:=sPostReceiptToSuspense), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetSystemOption()", "Failed to get system options for ACOptionPostReceiptToSuspense")
                End If

                ' Process appropriately
                If gPMFunctions.ToSafeLong(sPostReceiptToSuspense) = 1 Then
                    lReceiptPartyCnt = 0
                    sAccountCode = "CLAIMSUS"
                    sMappingCode = "CLMSUS" & ClassOfBusiness
                    sReceiptComments = ""
                Else
                    ' Get receipt party
                    ' If we are posting taxes then use receipt total, if not use net receipt.
                    lReturn = CType(GetReceiptPartyID(cReceiptTotal, lReceiptPartyCnt, sMappingCode, sReceiptComments), gPMConstants.PMEReturnCode)
                    If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        Return gPMConstants.PMEReturnCode.PMCancel
                    ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetReceiptPartyID()", "Failed to get receipt party")
                    End If

                    ' Check party selected and set default if necessary.
                    If lReceiptPartyCnt <> 0 Then
                        sAccountCode = ""
                    Else
                        sAccountCode = "CLAIMREC"
                        sMappingCode = "CLMRECEIVABLE"
                    End If
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    ' Updates all business members from the interface details.
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = g_oBusiness.Update
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

    ' Sets the rules for validating fields.
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Public function to terminate this object
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then

            End If
        End If
        Me.disposedValue = True
    End Sub





    ' ***************************************************************** '
    '                        PRIVATE FUNCTIONS
    ' ***************************************************************** '

    ' Updates the data storage from the business object.
    Private Function BusinessToData() As Integer

        Try

            ' Assign the details to the data storage.
            'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

            'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
            Return g_oBusiness.GetNext(vRecoveryId:=m_lRecoveryID, vRecoveryType:=m_sRecoveryType, vRecoveryTypeID:=m_lRecoveryTypeID, vInitialReserve:=m_cInitialReserve, vRevisedReserve:=m_cRevisedReserve, vThisReserve:=m_cThisReserve, vReceiptCurrency:=m_sReceiptCurrency, vReceiptCurrencyID:=m_lReceiptCurrencyID, vCurrencyRate:=m_dCurrencyRate, vReceivedToDate:=m_cReceivedToDate, vThisReceipt:=m_cThisReceipt, vBalance:=m_cBalance, vTaxType:=m_sTaxType, vTaxTypeID:=m_lTaxTypeID, vTaxBand:=m_sTaxBand, vTaxBandID:=m_lTaxBandID, vTaxAmount:=m_cTaxAmount, vCoinsurance:=m_vCoinsurance, vReinsurance:=m_vReinsurance, vIsPostTaxes:=m_bIsPostTaxes, lRecoveryPartyTypeId:=m_lRecoveryPartyTypeId, lRecoveryPartyCnt:=m_lRecoveryPartyCnt, sRecoveryParty:=m_sRecoveryParty, sRecoveryPartyDesc:=m_sRecoveryPartyDesc)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError


        End Try
    End Function


    ' Display all language specific captions.
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            Select Case g_lRecoveryMode
                Case MainModule.RecoveryModeEnum.RMThirdPartyReserve
                    Me.Text = "Third Party Recovery Reserve"

                    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceRecoveryParty - 1).Text = "Recovery Party"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceInitialReserve - 1).Text = "Initial Reserve"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceRevisedReserve - 1).Text = "Revised Reserve"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceThisReserve - 1).Text = "This Reserve"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceBalance - 1).Text = "Total Reserve"
                    'End (Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    ''Saurabh PN56294 - Added New Column Received to date in the listview.
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceReceivedtodateReserve).Text = "Received to Date"

                Case MainModule.RecoveryModeEnum.RMThirdPartyReceipt
                    Me.Text = "Third Party Recovery Receipt"

                Case MainModule.RecoveryModeEnum.RMSalvageReserve
                    Me.Text = "Salvage Recovery Reserve"

                    ' Set reserve captions
                    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceRecoveryParty - 1).Text = "Recovery Party"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceInitialReserve - 1).Text = "Initial Reserve"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceRevisedReserve - 1).Text = "Revised Reserve"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceThisReserve - 1).Text = "This Reserve"
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceBalance - 1).Text = "Total Reserve"
                    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    ''Saurabh PN56294 - Added New Column Received to date in the listview.
                    lvwRecovery.Columns.Item(RecoveryColumnEnum.rceReceivedtodateReserve).Text = "Received to Date"
                Case MainModule.RecoveryModeEnum.RMSalvageReceipt
                    Me.Text = "Salvage Party Recovery Receipt"


            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Updates the data storage from the interface details.
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Populate coinsurer and reinsurer details
    Private Function PopulateInsurers() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we are in reserve mode we don't display anything here
            ' PN 44597
            If Not (m_iTask = gPMConstants.PMEComponentAction.PMView) Then
                If (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReserve) Or (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReserve) Then
                    Return result
                End If
            End If

            ' Clear any current details
            lvwCoinsurance.Items.Clear()
            lvwReinsurance.Items.Clear()

            ' Populate coinsurance
            If Information.IsArray(m_vCoinsurance) Then
                For lCount As Integer = m_vCoinsurance.GetLowerBound(1) To m_vCoinsurance.GetUpperBound(1)
                    ' Create item

                    oListItem = lvwCoinsurance.Items.Add(CStr(m_vCoinsurance(ACCIDescription, lCount)).Trim(), ACFindImage)

                    With oListItem
                        ' Coinsurance share

                        .SubItems.Add(1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vCoinsurance(ACCISharePercent, lCount))
                        ' Received to date

                        .SubItems.Add(2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vCoinsurance(ACCIPaidToDate, lCount))
                        ' This receipt

                        .SubItems.Add(3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vCoinsurance(ACCIThisPaymentLoss, lCount))
                    End With
                Next lCount
            End If

            ' Populate reinsurance
            If Information.IsArray(m_vReinsurance) Then
                For lCount As Integer = m_vReinsurance.GetLowerBound(1) To m_vReinsurance.GetUpperBound(1)
                    ' Create item

                    oListItem = lvwReinsurance.Items.Add(CStr(m_vReinsurance(ACRIDescription, lCount)).Trim(), ACFindImage)

                    With oListItem
                        ' Reinsurance share

                        .SubItems.Add(1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vReinsurance(ACRISharePercent, lCount))
                        ' Received to date

                        .SubItems.Add(2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vReinsurance(ACRIPaidToDate, lCount))
                        ' This receipt

                        .SubItems.Add(3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vReinsurance(ACRIThisPayment, lCount))
                    End With
                Next lCount
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateInsurers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateInsurers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Updates the interface details from the property members.
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.
            txtClaimNumber.Text = ClaimNumber
            txtPerilType.Text = PerilType
            txtLossCurrency.Text = LossCurrency
            txtReceiptCurrency.Text = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Refresh the interface
    Private Function RefreshInterface() As Integer

        Dim result As Integer = 0
        Dim oRecovery As ListViewItem
        Dim cRecoveryAmount As Decimal

        Const kMethodName As String = "RefreshInterface"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCount As Integer

        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get active item
            oRecovery = lvwRecovery.FocusedItem

            ' Enable controls when an item is selected
            cmdAdd.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView)
            cmdEdit.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView) And Not (oRecovery Is Nothing)
            cmdDelete.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView) And Not (oRecovery Is Nothing)

            ' Check for recovery and load details
            ' Check we have current data
            If Not (oRecovery Is Nothing) Then
                ' Store unique id
                m_sUniqueID = oRecovery.Name

                ' Get proper data for this record, do not rely on reversing the displayed details
                'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

                lReturn = g_oBusiness.GetNext(vGetUniqueID:=m_sUniqueID, vRecoveryId:=m_lRecoveryID, vRecoveryType:=m_sRecoveryType, vRecoveryTypeID:=m_lRecoveryTypeID, vInitialReserve:=m_cInitialReserve, vRevisedReserve:=m_cRevisedReserve, vThisReserve:=m_cThisReserve, vReceiptCurrency:=m_sReceiptCurrency, vReceiptCurrencyID:=m_lReceiptCurrencyID, vCurrencyRate:=m_dCurrencyRate, vReceivedToDate:=m_cReceivedToDate, vThisReceipt:=m_cThisReceipt, vBalance:=m_cBalance, vTaxType:=m_sTaxType, vTaxTypeID:=m_lTaxTypeID, vTaxBand:=m_sTaxBand, vTaxBandID:=m_lTaxBandID, vTaxAmount:=m_cTaxAmount, vCoinsurance:=m_vCoinsurance, vReinsurance:=m_vReinsurance, vIsPostTaxes:=m_bIsPostTaxes, lRecoveryPartyTypeId:=m_lRecoveryPartyTypeId, lRecoveryPartyCnt:=m_lRecoveryPartyCnt, sRecoveryParty:=m_sRecoveryParty, sRecoveryPartyDesc:=m_sRecoveryPartyDesc)
                'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)


                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("g_oBusiness.GetNext()", "Failed to get recovery details")
                End If
            Else
                ' Clear all variables
                m_sUniqueID = ""
                m_lRecoveryID = 0
                m_sRecoveryType = ""
                m_lRecoveryTypeID = 0
                m_cInitialReserve = 0
                m_cRevisedReserve = 0
                m_cThisReserve = 0
                m_sReceiptCurrency = ""
                m_lReceiptCurrencyID = 0
                m_dCurrencyRate = 1
                m_cReceivedToDate = 0
                m_cThisReceipt = 0
                m_cBalance = 0
                m_sTaxType = ""
                m_lTaxTypeID = 0
                m_sTaxBand = ""
                m_lTaxBandID = 0
                m_cTaxAmount = 0
                m_vCoinsurance = VB6.CopyArray(Nothing)
                m_vReinsurance = VB6.CopyArray(Nothing)
                m_bIsPostTaxes = True
            End If

            ' Calculate net receipt
            m_cNetReceipt = m_cThisReceipt - m_cTaxAmount

            ' Determine the recovery amount for coinsurance/reinsurance
            cRecoveryAmount = IIf(m_bIsPostTaxes, m_cNetReceipt, m_cThisReceipt)

            ' Refresh information fields
            If (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReceipt) Or (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReceipt) Then
                txtRecoveryType(0).Text = m_sRecoveryType
                txtRecoveryType(1).Text = m_sRecoveryType
                txtRecoveryAmount(0).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cRecoveryAmount * m_dCurrencyRate)
                txtRecoveryAmount(1).Text = txtRecoveryAmount(0).Text
            End If

            If m_bIsRecoveriesReadOnly Then
                cmdAdd.Enabled = False
                cmdDelete.Enabled = False
                cmdEdit.Text = "View"
            Else
                cmdEdit.Text = "Edit"
            End If

            ' Refresh coinsurance and reinsurance splits
            lReturn = PopulateInsurers()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            ' This is for debugging only



        End Try
        Return result
    End Function

    ' Sets all of the interface default values.
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface and select main tab.
            iPMFunc.CenterForm(Me)
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Are we in recovery (reserve) or receipt mode
            If (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReserve) Or (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReserve) Then
                ' Don't display the tax information.
                lvwRecovery.Columns.RemoveAt(RecoveryColumnEnum.rceNetAmount - 1)
                lvwRecovery.Columns.RemoveAt(RecoveryColumnEnum.rceTaxAmount - 1)
                lvwRecovery.Columns.RemoveAt(RecoveryColumnEnum.rceTaxBand - 1)

                'PN #33276
                ' Remove coinsurance and reinsurance tabs
                'tabMainTab.TabVisible(1) = False
                'tabMainTab.TabVisible(2) = False

                ' All buttons visible, set enabled states
                cmdAdd.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView)
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False

                ' Set receipt currency
                lblReceiptCurrency.Visible = False
                txtReceiptCurrency.Visible = False
            Else
                ' If we are in receipt mode don't allow adding or deletion of reserves
                cmdAdd.Visible = False
                cmdDelete.Visible = False
                cmdEdit.Top = cmdAdd.Top
            End If

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the interface details with the property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check currency override flag, if necessary
            If (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReceipt) Or (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReceipt) Then
                m_bAllowCurrencyRateOverride = False
                m_lReturn = CType(GetUserAuthorities(r_bAllowOverride:=m_bAllowCurrencyRateOverride), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetUserAuthorities failed")
                End If
            End If

            If m_bIsRecoveriesReadOnly Then
                cmdAdd.Enabled = False
                cmdDelete.Enabled = False
                cmdEdit.Text = "View"
            Else
                cmdEdit.Text = "Edit"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '                         CONTROL EVENTS
    ' ***************************************************************** '

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Const kMethodName As String = "cmdAdd_Click"
        Dim lReturn As Integer

        Dim oDetail As frmRecoveryReserve
        Dim sUniqueID As String = ""

        Try

            ' Check we are in reserve mode
            If (g_lRecoveryMode <> MainModule.RecoveryModeEnum.RMSalvageReserve) And (g_lRecoveryMode <> MainModule.RecoveryModeEnum.RMThirdPartyReserve) Then
                RefreshInterface()
                Exit Sub
            End If

            ' Create an instance of the form
            oDetail = New frmRecoveryReserve()

            ' Set properties
            oDetail.Mode = gPMConstants.PMEComponentAction.PMAdd
            'Added the code for populating the combobox
            oDetail.cboPartyType.FirstItem = "(None)"

            ' Load and show dialog
            oDetail.ShowDialog()

            ' Check result
            If oDetail.Status = gPMConstants.PMEReturnCode.PMOK Then

                'Start Girija - PN 54404
                If oDetail.PartyCnt = ACPartyTypeEmpty Then
                    oDetail.PartyName = ACPartyTypeDefaultCaption
                End If
                'End Girija - PN 54404
                'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

                m_lReturn = g_oBusiness.EditAdd(vClaimId:=ClaimId, vPerilId:=PerilID, vRecoveryType:=oDetail.RecoveryType, vRecoveryTypeID:=oDetail.RecoveryTypeID, vLossCurrency:=LossCurrency, vLossCurrencyID:=LossCurrencyID, vInitialReserve:=oDetail.InitialReserve, rUniqueId:=sUniqueID, v_lRecoveryPartyTypeId:=oDetail.PartyTypeID, v_lRecoveryPartyCnt:=oDetail.PartyCnt, v_sRecoveryParty:=oDetail.PartyCode.Trim(), v_sRecoveryPartyDesc:=oDetail.PartyName.Trim())
                'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                ' Add the new item to the list
                Dim olistItem As ListViewItem


                olistItem = lvwRecovery.Items.Add(sUniqueID, oDetail.RecoveryType, ACFindImage)
                ' Set sub items
                'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

                'developer guide no. 101
                ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceRecoveryParty - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, oDetail.PartyName.Trim())

                ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceInitialReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oDetail.InitialReserve)

                ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceRevisedReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oDetail.RevisedReserve)

                ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceThisReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oDetail.ThisReserve)

                ListViewHelper.GetListViewSubItem(olistItem, RecoveryColumnEnum.rceBalance - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oDetail.TotalReserve)
                'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                ' Set id and select the item


                olistItem.Tag = CStr(oDetail.RecoveryTypeID)

                olistItem.Selected = True


                ' Unload and release the form
                oDetail.Close()
                oDetail = Nothing
            End If

            ' Refresh the interface
            RefreshInterface()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Or (m_lReturn = gPMConstants.PMEReturnCode.PMError) Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim oRecovery As ListViewItem

        Const kMethodName As String = "cmdDelete_Click"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            ' Check we are in reserve mode
            If (g_lRecoveryMode <> MainModule.RecoveryModeEnum.RMSalvageReserve) And (g_lRecoveryMode <> MainModule.RecoveryModeEnum.RMThirdPartyReserve) Then
                RefreshInterface()
                Exit Sub
            End If

            ' Get active item
            oRecovery = lvwRecovery.FocusedItem

            ' If we don't have a selected item somethings wrong, refresh the interface
            If oRecovery Is Nothing Then
                RefreshInterface()
                Exit Sub
            End If

            ' Ask to delete the item

            lReturn = g_oBusiness.EditDelete(lvwRecovery.FocusedItem.Name)
            Select Case lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Everything was okay, remove the list item
                    lvwRecovery.Items.RemoveAt(lvwRecovery.FocusedItem.Index)
                    RefreshInterface()
                Case gPMConstants.PMEReturnCode.PMFalse
                    ' Item has processed receipts we can't delete it
                    DisplayMessage(ACMessageInvalidDelete, "Invalid Delete Operation")
                Case Else
                    ' An error occured raise it
                    gPMFunctions.RaiseError("g_oBusiness.EditDelete", "Failed to delete recovery")
            End Select


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Const kMethodName As String = "cmdEdit_Click"
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oReserveDetail As frmRecoveryReserve
        Dim oReceiptDetail As frmRecoveryReceipt
        Dim oListItem As ListViewItem

        Try

            ' Get active item
            oListItem = lvwRecovery.FocusedItem

            ' If we don't have a selected item somethings wrong, refresh the interface
            If oListItem Is Nothing Then
                RefreshInterface()
                Exit Sub
            End If

            ' Check we have current data
            If oListItem.Name <> m_sUniqueID Then
                ' Refresh the interface, this will load the active recovery
                RefreshInterface()
            End If

            ' Check we are in reserve mode
            If (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReserve) Or (g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReserve) Then
                ' Create an instance of the form
                oReserveDetail = New frmRecoveryReserve()
                'Added the code for populating the combobox
                oReserveDetail.cboPartyType.FirstItem = "(None)"
                ' Set properties
                oReserveDetail.Mode = gPMConstants.PMEComponentAction.PMEdit
                oReserveDetail.RecoveryType = m_sRecoveryType
                oReserveDetail.RecoveryTypeID = m_lRecoveryTypeID
                oReserveDetail.InitialReserve = m_cInitialReserve
                oReserveDetail.RevisedReserve = m_cRevisedReserve
                oReserveDetail.ThisReserve = m_cThisReserve
                'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                oReserveDetail.PartyTypeID = m_lRecoveryPartyTypeId
                oReserveDetail.PartyCnt = m_lRecoveryPartyCnt
                oReserveDetail.PartyCode = m_sRecoveryParty
                oReserveDetail.PartyName = m_sRecoveryPartyDesc
                If Not cmdAdd.Enabled And Not cmdDelete.Enabled And Not cmdEdit.Enabled Then
                    oReserveDetail.cboPartyType.Enabled = False
                    oReserveDetail.cmdGetParty.Enabled = False
                End If
                oReserveDetail.IsRecoveriesReadOnly = m_bIsRecoveriesReadOnly
                'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                ' Load and show dialog

                oReserveDetail.ShowDialog()

                ' Check result
                If oReserveDetail.Status = gPMConstants.PMEReturnCode.PMOK Then
                    'Start Girija - PN 54404
                    If oReserveDetail.PartyCnt = ACPartyTypeEmpty Then
                        oReserveDetail.PartyName = ACPartyTypeDefaultCaption
                    End If
                    'End Girija - PN 54404

                    ' We only need to update the reserve
                    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

                    lReturn = g_oBusiness.EditUpdate(vUniqueID:=m_sUniqueID, vRecoveryId:=m_lRecoveryID, vThisReserve:=oReserveDetail.ThisReserve, v_lRecoveryPartyTypeId:=oReserveDetail.PartyTypeID, v_lRecoveryPartyCnt:=oReserveDetail.PartyCnt, v_sRecoveryParty:=oReserveDetail.PartyCode, v_sRecoveryPartyDesc:=oReserveDetail.PartyName)
                    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("g_oBusiness.EditUpdate()", "Failed to update recovery details")
                    End If

                    ' Update the list
                    With oListItem
                        ' Set sub items
                        'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                        'developer guide no.101
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceRecoveryParty - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEDataType.PMString, oReserveDetail.PartyName.Trim())
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceInitialReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReserveDetail.InitialReserve)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceRevisedReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReserveDetail.RevisedReserve)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceThisReserve - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReserveDetail.ThisReserve)
                        ''Saurabh
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceBalance - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReserveDetail.TotalReserve - m_cReceivedToDate)
                        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                    End With

                    ' Unload and release the form
                    oReserveDetail.Close()
                    oReserveDetail = Nothing
                End If
            Else
                ' We are in receipt mode
                ' Create an instance of the form
                oReceiptDetail = New frmRecoveryReceipt()

                ' Set properties
                oReceiptDetail.PerilID = PerilID
                oReceiptDetail.ClaimCompanyID = CompanyID

                oReceiptDetail.RecoveryType = m_sRecoveryType
                oReceiptDetail.InitialReserve = m_cInitialReserve
                oReceiptDetail.TotalReserve = m_cInitialReserve + m_cRevisedReserve
                oReceiptDetail.ReceivedToDate = m_cReceivedToDate

                oReceiptDetail.ReceiptCurrencyID = m_lReceiptCurrencyID
                oReceiptDetail.ReceiptCurrency = m_sReceiptCurrency
                oReceiptDetail.CurrencyRate = m_dCurrencyRate
                oReceiptDetail.LossCurrency = LossCurrency
                oReceiptDetail.LossCurrencyID = LossCurrencyID

                oReceiptDetail.ThisReceipt = m_cThisReceipt

                oReceiptDetail.TaxTypeID = m_lTaxTypeID
                oReceiptDetail.TaxBandID = m_lTaxBandID
                oReceiptDetail.TaxAmount = m_cTaxAmount

                oReceiptDetail.ReceiptCurrencySet = m_bReceiptCurrencySet

                ' Load and show dialog
                oReceiptDetail.ShowDialog()

                ' Check result
                If oReceiptDetail.Status = gPMConstants.PMEReturnCode.PMOK Then
                    ' The first receipt sets the receipt currency and rate
                    If Not m_bReceiptCurrencySet Then
                        txtReceiptCurrency.Text = oReceiptDetail.ReceiptCurrency
                        m_bReceiptCurrencySet = True


                        lReturn = g_oBusiness.SetReceiptCurrency(vReceiptCurrency:=oReceiptDetail.ReceiptCurrency, vReceiptCurrencyID:=oReceiptDetail.ReceiptCurrencyID, vCurrencyRate:=oReceiptDetail.CurrencyRate)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("g_oBusiness.SetReceiptCurrency()", "Failed to set receipt currency")
                        End If
                    End If

                    ' We only need to update the reserve

                    lReturn = g_oBusiness.EditUpdate(vUniqueID:=m_sUniqueID, vRecoveryId:=m_lRecoveryID, vThisReceipt:=oReceiptDetail.ThisReceipt, vTaxType:=oReceiptDetail.TaxType, vTaxTypeID:=oReceiptDetail.TaxTypeID, vTaxTypeCode:=oReceiptDetail.TaxTypeCode, vTaxBand:=oReceiptDetail.TaxBand, vTaxBandID:=oReceiptDetail.TaxBandID, vTaxAmount:=oReceiptDetail.TaxAmount)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("g_oBusiness.EditUpdate()", "Failed to update recovery details")
                    End If

                    ' Update the list
                    With oListItem
                        ' Set sub items
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReceiptDetail.TotalReserve)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceReceivedToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReceiptDetail.ReceivedToDate)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceThisReceipt).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReceiptDetail.ThisReceiptLoss)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceBalance).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReceiptDetail.Balance)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceTaxBand).Text = oReceiptDetail.TaxBand.Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceTaxAmount).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReceiptDetail.TaxAmountLoss)
                        ListViewHelper.GetListViewSubItem(oListItem, RecoveryColumnEnum.rceNetAmount).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, oReceiptDetail.NetReceiptLoss)
                    End With

                    ' Unload and release the form
                    oReceiptDetail.Close()
                    oReceiptDetail = Nothing
                End If
            End If

            ' Refresh the interface
            RefreshInterface()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        End Try
    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bForceLostFocus As Boolean

        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Ensure we have processed all data and events
            bForceLostFocus = iPMFunc.ForceLostFocus(cmdOK)
            If Not bForceLostFocus Then
                txtClaimNumber.Focus()
                Exit Sub
            End If

            ' This has to be done first as some of the values saved will be used when posting.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMCancel
                    ' Do nothing user cancelled
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                Case gPMConstants.PMEReturnCode.PMError
                    ' Not okay, return error and still hide
                    m_lStatus = gPMConstants.PMEReturnCode.PMError
                    Me.Hide()
                Case Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function ProcessCommand failed.")
            End Select

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'developer guide no.11
    Private Sub lvwRecovery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRecovery.Click
        If Not IsNothing(lvwRecovery.FocusedItem) Then
            RefreshInterface()
        End If
    End Sub


    Private Sub lvwRecovery_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRecovery.DoubleClick
        ' Trigger an edit on double click
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If
    End Sub

    ' ***************************************************************** '
    '                           FORM EVENTS
    ' ***************************************************************** '

    ' Initialise all required details of the form
    Private Sub Form_Initialize_Renamed()

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMRecovery.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmRecovery:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)

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



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' Loads all required details of the form

    Private Sub frmRecovery_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            ' Check if we have had an error so far.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults()", "Failed to set interface defaults.")
            End If

            ' Gets the interface details to be displayed.
            lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.GetInterfaceDetails()", "Failed to get interface details.")
            End If

            lvwRecovery.SelectedItems.Clear()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lErrorNumber, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Sub

    ' Store all Property Details before unloading form
    Private Sub frmRecovery_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide no. 7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()


            ' Destroy the instance of the general object from memory.
            m_oGeneral = Nothing

            Dispose()
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

    ' Resize the the controls on form
    Private isInitializingComponent As Boolean
    Private Sub frmRecovery_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ' This is only visual formatting ignore errors

        Try

            If VB6.PixelsToTwipsX(Me.Width) < 9300 Then Me.Width = VB6.TwipsToPixelsX(9300)
            If VB6.PixelsToTwipsY(Me.Height) < 6930 Then Me.Height = VB6.TwipsToPixelsY(6930)

            ' Move recovery command buttons
            cmdAdd.SetBounds(Me.ClientRectangle.Width - VB6.TwipsToPixelsX(1410), 0, 0, 0, BoundsSpecified.X)
            cmdEdit.SetBounds(cmdAdd.Left, 0, 0, 0, BoundsSpecified.X)
            cmdDelete.SetBounds(cmdAdd.Left, 0, 0, 0, BoundsSpecified.X)

            ' Main tab
            tabMainTab.Width = VB6.ToPixelsUserWidth(Me.ClientRectangle.Width - 195, 9210, 614)
            tabMainTab.Height = VB6.ToPixelsUserHeight(Me.ClientRectangle.Height - 1500, 6555, 437)

            ' List views
            lvwRecovery.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(1605)
            lvwRecovery.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(2040)

            lvwCoinsurance.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(405)
            lvwCoinsurance.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(2490)

            lvwReinsurance.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(405)
            lvwReinsurance.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(2490)

            ' Main command buttons
            cmdOK.SetBounds(VB6.ToPixelsUserX(Me.ClientRectangle.Width - 3540, 0, 9210, 614), VB6.ToPixelsUserY(Me.ClientRectangle.Height - 435, 0, 6555, 437), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdCancel.SetBounds(VB6.ToPixelsUserX(Me.ClientRectangle.Width - 2370, 0, 9210, 614), VB6.ToPixelsUserY(VB6.FromPixelsUserY(cmdOK.Top, 0, 6555, 437), 0, 6555, 437), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdHelp.SetBounds(VB6.ToPixelsUserX(Me.ClientRectangle.Width - 1200, 0, 9210, 614), VB6.ToPixelsUserY(VB6.FromPixelsUserY(cmdOK.Top, 0, 6555, 437), 0, 6555, 437), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Private Sub lvwRecovery_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwRecovery.MouseUp
        RefreshInterface()
    End Sub

    Private Sub frmRecovery_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293
        'start
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            tabMainTab.SelectedIndex = 2
        End If

        'end
    End Sub

    Private Sub lvwRecovery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRecovery.SelectedIndexChanged

    End Sub
End Class