Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name     : frmInterface
    ' Description   : Main interface.
    ' Date          : 30/08/2000
    ' Author        : Pandu
    ' Edit History  :
    ' CJB 280905 PN24371 : Changed CreditClient to cater for claim payments
    '                      that may be +ve or -ve. Just assumed credit sign before.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    'Variables For Payment/Receipt Method
    Private m_lPartyid As Integer
    Private m_lScreenMethod As Integer
    Private m_sPartyName As String = ""
    Private m_sComments As String = ""
    Private m_lButtonClicked As gPMConstants.PMEReturnCode
    Private m_cCurrency As Decimal
    Private m_iCurrencyID As Integer
    Private m_iLossCurrencyID As Integer
    Private m_cLossCurrencyAmount As Decimal

    Private m_nOptionNumber As Integer
    Private m_lOptionValue As Integer
    Private m_cAmount As Decimal
    Private m_nTypeofParty As Integer

    Private m_sUnderWritingOrAgency As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMPaymentMethod.General

    ' Private instance of the business object.
    Private m_oBusiness As Object
    Private m_oInsuranceFile As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Stores the data from the GetPerilsForReserve method of the business object.
    Public m_vPartyArray As Object
    'JMK 21/08/2001 extra properties for UW
    ' AgentID, AgentName, ClientID,  ClientName
    Private m_lAgentID As Integer
    Private m_sAgentName As String = ""
    Private m_lClientID As Integer
    Private m_sClientName As String = ""

    Private m_lProductID As Integer

    'DJM 22/03/2004
    Private m_lPayeeMediaType As Integer
    Private m_sPayeeName As String = ""
    Private m_sPayeeBankName As String = ""
    Private m_sPayeeSortCode As String = ""
    Private m_sPayeeAccountNo As String = ""
    Private m_lPayeeCountry As Integer
    Private m_sPayeeComments As String = ""
    'eck 11/2005
    Private m_vInsurerSplit(,) As Object
    Private m_vInsurerPayments(,) As Object


    Private m_bMediaTypeMandatory As Boolean

    ' RDC 15062004
    Private m_lCurrencyChange As Integer
    ' constants for above
    Private Const CHANGECURRENCYTRUE As Integer = 1
    Private Const CHANGECURRENCYFALSE As Integer = 0
    Private m_sClaimRef As String = ""
    Private m_sPolicyNumber As String = ""
    Private m_lInsuranceFileCnt As Integer

    Private m_lClaimCompanyID As Integer
    Private m_lClaimID As Integer

    Private m_lRiskTypeId As Integer 'JAS20050113 - PN18034
    Private m_vPaymentDetailsArray(,) As Object
    Private m_bFromNavigator As Boolean
    Private m_lClaimPerilID As Integer
    Private m_lSequenceNo As Integer
    Private m_lClaimPaymentID As Integer 'eck 11/2005
    Private m_bAuthoriseMode As Boolean

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_lAccountID As Integer

    Private m_bPayeeTab As Boolean 'AR20050111 PN17973
    Private m_sScreenType As String = ""
    Private Const vbFormCode As Integer = 0
    'developer guide no. 129
    Dim m_frmInterface As frmInterface



    Public WriteOnly Property AuthoriseMode() As Boolean
        Set(ByVal Value As Boolean)
            m_bAuthoriseMode = Value
        End Set
    End Property

    'JAS20050113 - PN18034
    'JAS20050113 - PN18034
    Public Property RiskTypeId() As Integer
        Get

            Return m_lRiskTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskTypeId = Value

        End Set
    End Property

    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public Property AgentID() As Integer
        Get
            Return m_lAgentID
        End Get
        Set(ByVal Value As Integer)
            m_lAgentID = Value
        End Set
    End Property
    Public Property AgentName() As String
        Get
            Return m_sAgentName
        End Get
        Set(ByVal Value As String)
            m_sAgentName = Value
        End Set
    End Property
    Public Property ClientID() As Integer
        Get
            Return m_lClientID
        End Get
        Set(ByVal Value As Integer)
            m_lClientID = Value
        End Set
    End Property
    Public Property ClientName() As String
        Get
            Return m_sClientName
        End Get
        Set(ByVal Value As String)
            m_sClientName = Value
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

    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return m_cAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cAmount = Value
        End Set
    End Property

    Public WriteOnly Property LossCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iLossCurrencyID = Value
        End Set
    End Property

    Public WriteOnly Property LossCurrencyAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cLossCurrencyAmount = Value
        End Set
    End Property


    Public Property Partyid() As Integer
        Get
            Return m_lPartyid
        End Get
        Set(ByVal Value As Integer)

            m_lPartyid = Value

        End Set
    End Property

    Public Property ScreenMethod() As Integer
        Get

            Return m_lScreenMethod

        End Get
        Set(ByVal Value As Integer)

            m_lScreenMethod = Value

        End Set
    End Property


    Public Property PartyName() As String
        Get

            Return m_sPartyName

        End Get
        Set(ByVal Value As String)

            m_sPartyName = Value

        End Set
    End Property


    Public Property Comments() As String
        Get

            Return m_sComments

        End Get
        Set(ByVal Value As String)

            m_sComments = Value

        End Set
    End Property

    Public Property ButtonClicked() As Integer
        Get

            Return m_lButtonClicked

        End Get
        Set(ByVal Value As Integer)

            m_lButtonClicked = Value

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

    'DJM 23/03/2004

    Public Property PayeeMediaType() As Integer
        Get

            Return m_lPayeeMediaType

        End Get
        Set(ByVal Value As Integer)

            m_lPayeeMediaType = Value

        End Set
    End Property


    Public Property PayeeName() As String
        Get

            Return m_sPayeeName

        End Get
        Set(ByVal Value As String)

            m_sPayeeName = Value

        End Set
    End Property


    Public Property PayeeBankName() As String
        Get

            Return m_sPayeeBankName

        End Get
        Set(ByVal Value As String)

            m_sPayeeBankName = Value

        End Set
    End Property


    Public Property PayeeSortCode() As String
        Get

            Return m_sPayeeSortCode

        End Get
        Set(ByVal Value As String)

            m_sPayeeSortCode = Value

        End Set
    End Property


    Public Property PayeeAccountNo() As String
        Get

            Return m_sPayeeAccountNo

        End Get
        Set(ByVal Value As String)

            m_sPayeeAccountNo = Value

        End Set
    End Property


    Public Property PayeeCountry() As Integer
        Get

            Return m_lPayeeCountry

        End Get
        Set(ByVal Value As Integer)

            m_lPayeeCountry = Value

        End Set
    End Property


    Public Property PayeeComments() As String
        Get

            Return m_sPayeeComments

        End Get
        Set(ByVal Value As String)

            m_sPayeeComments = Value

        End Set
    End Property

    ' RDC 15042004
    Public WriteOnly Property ClaimRef() As String
        Set(ByVal Value As String)
            m_sClaimRef = Value
        End Set
    End Property
    Public WriteOnly Property PolicyNumber() As String
        Set(ByVal Value As String)
            m_sPolicyNumber = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public Property ClaimPaymentID() As Integer
        Get
            Return m_lClaimPaymentID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPaymentID = Value
        End Set
    End Property


    Public Property FromNavigator() As Boolean
        Get
            Return m_bFromNavigator
        End Get
        Set(ByVal Value As Boolean)
            m_bFromNavigator = Value
        End Set
    End Property

    Public Property ClaimPerilID() As Integer
        Get
            Return m_lClaimPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPerilID = Value
        End Set
    End Property

    Public Property SequenceNo() As Integer
        Get
            Return m_lSequenceNo
        End Get
        Set(ByVal Value As Integer)
            m_lSequenceNo = Value
        End Set
    End Property


    Public Property ScreenType() As String
        Get
            Return m_sScreenType
        End Get
        Set(ByVal Value As String)
            m_sScreenType = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name:         GetBusiness
    '
    ' Description:  Retrieves the details from the business object.
    '
    ' Date:         11/07/00
    '
    ' Edit History: SK

    '
    ' ***************************************************************** '

    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            Dim sOptionValue As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            m_nOptionNumber = ACOptionNumber



            m_lReturn = g_oBusiness.getOption(v_iOptionNumber:=m_nOptionNumber, r_sOptionValue:=sOptionValue)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lOptionValue = CInt(sOptionValue)


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
            'eck 11/2005 Pass payment id
            If m_bFromNavigator Then

                m_lReturn = g_oBusiness.GetPaymentDetails(v_lClaimID:=m_lClaimID, v_lClaimPerilId:=m_lClaimPerilID, v_lSequenceNo:=m_lSequenceNo, v_lClaimPaymentId:=m_lClaimPaymentID, r_vPaymentDetailsArray:=m_vPaymentDetailsArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetPaymentDetails Failed.")
                End If
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
        Dim lReserveid As Integer
        Dim cReserveAmount As Decimal


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bFromNavigator And Information.IsArray(m_vPaymentDetailsArray) Then
                'eck 11/2000 this must be accumulated as it is now grouped by reserve_id
                For lRow As Integer = 0 To m_vPaymentDetailsArray.GetUpperBound(1)
                    m_cAmount += gPMFunctions.NullToCurrency(m_vPaymentDetailsArray(ACPaymentDetailsAmount, lRow))
                Next lRow
                m_sPartyName = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsPartyCode, 0))
                m_sComments = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsComments, 0))
                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(m_vPaymentDetailsArray(ACPaymentDetailsPayeeMediaType, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    m_lPayeeMediaType = gPMFunctions.NullToLong(m_vPaymentDetailsArray(ACPaymentDetailsPayeeMediaType, 0))
                Else
                    m_lPayeeMediaType = 0
                End If
                m_sPayeeName = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsPayeeName, 0))
                m_sPayeeBankName = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsPayeeBankName, 0))
                m_sPayeeSortCode = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsPayeeSortCode, 0))
                m_sPayeeAccountNo = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsPayeeAccountNo, 0))
                If Strings.Len(CStr(m_vPaymentDetailsArray(ACPaymentDetailsPayeeCountry, 0))) = 0 Then
                    m_lPayeeCountry = 0
                Else
                    m_lPayeeCountry = gPMFunctions.NullToLong(m_vPaymentDetailsArray(ACPaymentDetailsPayeeCountry, 0))
                End If
                m_sPayeeComments = gPMFunctions.NullToString(m_vPaymentDetailsArray(ACPaymentDetailsPayeeComments, 0))

                For lRow As Integer = 0 To m_vPaymentDetailsArray.GetUpperBound(1)
                    cReserveAmount = gPMFunctions.NullToCurrency(m_vPaymentDetailsArray(ACPaymentDetailsAmount, lRow))
                    lReserveid = CInt(gPMFunctions.NullToCurrency(m_vPaymentDetailsArray(ACPaymentDetailsReserveID, lRow)))
                    'DC090606

                    m_lReturn = g_oBusiness.GetInsurerDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimID:=m_lClaimID, r_vResults:=m_vInsurerSplit)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    m_lReturn = CType(CalculateInsurerPostings(v_cAmount:=cReserveAmount), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim bIsCancelled, bOptionValue As Boolean
        Dim sOption As String = ""
        Dim lMediaTypeId As Integer

        Const AC_MEDIA_TYPE_CHEQUE As String = "CQ"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            VB6.SetDefault(cmdOK, True)


            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR20050427 - PN19582 Default MediaType dropdown to Cheque

            m_lReturn = g_oBusiness.GetMediaTypeId(AC_MEDIA_TYPE_CHEQUE, lMediaTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Me.cboMediaType.ItemId = lMediaTypeId

            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oGeneral.GetInterfaceDetails Failed.")
            End If

            m_lReturn = PropertiesToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function PropertiesToInterface Failed.")
            End If


            'AJM 12/03/01 - set up correct payment options depending on system option 2002

            Select Case m_lOptionValue
                Case ACOptionValueSuspense
                    OptClaimAccount.Checked = True
                    cmdParty.Enabled = False

                    txtParty.Text = IIf(m_lScreenMethod = ACPaymentMethod, "CLMPAYABLE", "CLMRECEIVABLE")
                    m_nTypeofParty = PMBPartyNone

                Case ACOptionValueThirdParty

                    cmdParty.Enabled = True
                    OptPartyAccount.Checked = True
                    m_nTypeofParty = PMBPartyTypeCorporateClientText

                Case ACOptionValueNominal
                    txtParty.Text = ""

                    cmdParty.Enabled = False

                Case ACOptionValueClient
                    OptPartyAccount.Checked = True
                    txtParty.Text = ClientName
                    cmdParty.Enabled = False
                    m_lPartyid = ClientID
                    m_nTypeofParty = PMBPartyTypePersonalClientText

            End Select



            cmdAuthorise.Visible = m_bAuthoriseMode
            cmdReject.Visible = m_bAuthoriseMode
            cmdOK.Visible = Not m_bAuthoriseMode


            If AgentID = 0 Then
                ' Disable Agent payment option if direct business
                OptAgentAccount.Enabled = False
                ' Alix - 12/02/2004
                ' Also change label so user knows why it is disabled

                OptAgentAccount.Text = OptAgentAccount.Text & " " & _
                                       CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDirectBusiness, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                ' We check if we need to verify agent for this product

                m_lReturn = g_oBusiness.GetOptionAgent(v_lProductID:=m_lProductID, r_bValue:=bOptionValue)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' We log an error but don't exit, there's no need for it
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get agent product option for product:" & m_lProductID, vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                If bOptionValue Then
                    ' Alix - 12/02/2004 - PS075
                    ' We need to check if the agent is still active

                    m_lReturn = g_oBusiness.IsAgentCancelled(v_lAgentID:=m_lAgentID, r_bIsCancelled:=bIsCancelled)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' We log an error but don't exit, there's no need for it
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if agent is cancelled", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    If bIsCancelled Then

                        ' If agent cancelled, disable agent payment option
                        OptAgentAccount.Enabled = False

                        ' Alix - 12/02/2004 - Also change label so user knows why it is disabled

                        OptAgentAccount.Text = OptAgentAccount.Text & " " & _
                                               CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentCancelled, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    End If
                End If
                ' /Alix
            End If



            txtAmount.Enabled = False
            txtParty.Enabled = False

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'Private Function DisableForm() As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Set all of the forms controls to the disable state.

    'For	Each ctlFormControl As Control In ContainerHelper.Controls(Me)
    ' Check the type of the control.
    'If TypeOf ctlFormControl Is TextBox Then
    'ControlHelper.SetEnabled(ctlFormControl, False)
    'ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, False)
    'ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, False)
    'ElseIf (TypeOf ctlFormControl Is RadioButton) Then 
    'ControlHelper.SetEnabled(ctlFormControl, False)
    'ElseIf (TypeOf ctlFormControl Is PMLookupControl.cboPMLookup) Then 
    'ControlHelper.SetEnabled(ctlFormControl, False)
    'End If
    'Next ctlFormControl
    '
    'Now the command buttons...
    'cmdParty.Enabled = False
    '
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    Private Sub cmdAuthorise_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAuthorise.Click
        Const kMethodName As String = "cmdAuthorise_Click"
        Const kErrorCode As Integer = Constants.vbObjectError
        Try

            ' pay client
            m_lReturn = CType(CreditClient(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", CreditClient failed")
            End If

            ' create work manager task 4 cash/cheque payment
            m_lReturn = CType(CreateChequePaymentTask(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", CreateChequePaymentTask failed")
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' sucess so close
            Me.Hide()



        Catch ex As Exception

            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes

        End Try
    End Sub



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

            If m_lScreenMethod = ACPaymentMethod Then


                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitlePayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Else


                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitleReceipt, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End If

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


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DJM 22/03/2004

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Select Case m_lScreenMethod
                Case ACPaymentMethod


                    OptClientAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientPayableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    OptClaimAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimPaymentAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    OptAgentAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentPayableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    OptPartyAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyPayableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    FraSelectMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case ACReceiptMethod


                    OptClientAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientReceivableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    OptClaimAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimReceivableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    OptAgentAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentReceivableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    OptPartyAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyReceivableAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    FraSelectMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectReceiptMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            End Select


            lblParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lblComments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACComments, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DJM 22/03/2004

            fraPaymentInformation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentInformation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraPayee.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPayee, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPayeeName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPayeeName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSortCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSortCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCountry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCountry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPayeeComments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACComments, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            'Form Width-7665 Height-5640

            If VB6.PixelsToTwipsX(Me.Width) < 7665 Then Me.Width = VB6.TwipsToPixelsX(7695)
            If VB6.PixelsToTwipsY(Me.Height) < 5640 Then Me.Height = VB6.TwipsToPixelsY(4785)


            Return result

        Catch





            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cboMediaType_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaType.GotFocus
        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
    End Sub

    Private Sub cmdReject_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReject.Click
        Const kMethodName As String = "cmdReject_Click"
        Const kErrorCode As Integer = Constants.vbObjectError
        Try


            m_lReturn = g_oBusiness.RejectPayment(m_lClaimPerilID, m_lSequenceNo, m_lClaimPaymentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Function m_oBusiness.RejectPayment failed")
            End If

            'AR20050113 - PN18029
            m_lReturn = CreatePaymentRejectionTask()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Function CreatePaymentRejectionTask failed")
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'All done, close screen.
            Me.Hide()



        Catch ex As Exception

            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes


        End Try
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

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID



            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMPaymentMethod.General()


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

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
    ' Name:         FormLoad
    ' Description:  Loads all required details of the form
    ' Date:         15/07/00
    ' Edit History: SK
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim sOption As String = ""

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


            m_bPayeeTab = True

            ' Validate fields using Forms Control

            m_lReturn = SetFieldValidation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            m_sUnderWritingOrAgency = g_oBusiness.UnderwritingOrAgency

            m_lReturn = CType(GetClaimCompany(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

    'AR20050113 - PN18029
    '*******************************************************************
    '*                                                                 *
    '* CreatePaymentRejectionTask                                      *
    '*                                                                 *
    '*******************************************************************

    Private Function CreatePaymentRejectionTask() As Integer
        Dim result As Integer = 0
        Dim bCLMPeril, iPMWrkTaskInstanceTemp As Object

        Dim vKeyArray(,) As Object
        Dim sTaskGroupCode, sTaskNarrative, sClaimReference As String

        Dim oWorkTask As Object

        Dim oPeril As bCLMPeril.Business

        Const c_sTASK_CODE As String = "MAINCLM"
        Const c_lBASE_ERROR As Integer = Constants.vbObjectError
        Const c_sFUNCTION_NAME As String = "CreatePaymentRejectionTask"

        Dim temp_oWorkTask As Object
        Try

            m_lReturn = g_oObjectManager.GetInstance(temp_oWorkTask, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWorkTask = temp_oWorkTask

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 1).ToString() + ", " + c_sFUNCTION_NAME + ", Failed to get instance of iPMWrkTaskInstanceTemp.Interface")
            End If

            Dim temp_oPeril As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPeril, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPeril = temp_oPeril

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 2).ToString() + ", " + c_sFUNCTION_NAME + ", Failed to get instance of bCLMPeril.Business")
            End If


            m_lReturn = oPeril.GetTaskGroupCode(v_sTaskCode:=c_sTASK_CODE, r_sTaskGroupCode:=sTaskGroupCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 3).ToString() + ", " + c_sFUNCTION_NAME + ", Function oPeril.GetTaskGroupCode failed.")
            End If


            m_lReturn = oPeril.GetClaimNumberFromClaim(v_lClaimID:=m_lClaimID, r_sClaimRef:=sClaimReference)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 4).ToString() + ", " + c_sFUNCTION_NAME + ", Function oPeril.GetClaimNumber failed.")
            End If

            sTaskNarrative = "CLAIM: " & sClaimReference.Trim() & " - Payment rejected"

            ReDim vKeyArray(1, 11)

            'Populate the key array with all of the keys required to create a work task.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sTaskGroupCode.Trim()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = c_sTASK_CODE


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskDescription

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = sTaskNarrative

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskCustomer

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sPartyName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameUseExtraKeys

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = True

            'Populate the key array with all of the extra keys required to run the authorise navigator.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameRiskTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lRiskTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "insurancefile_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameOperateMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = gPMConstants.PMEComponentAction.PMEdit

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lPartyid

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameClaimReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = sClaimReference.Trim()


            m_lReturn = oWorkTask.SetKeys(vKeyArray:=vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 5).ToString() + ", " + c_sFUNCTION_NAME + ", Function oWorkTask.SetKeys failed.")
            End If


            m_lReturn = oWorkTask.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 6).ToString() + ", " + c_sFUNCTION_NAME + ", Function oWorkTask.SetProcessModes failed.")
            End If


            oWorkTask.CallingAppName = "Authorise Claim Payment Task"


            m_lReturn = oWorkTask.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((c_lBASE_ERROR + 7).ToString() + ", " + c_sFUNCTION_NAME + ", Function oWorkTask.Start failed.")
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            Return result

        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=c_sFUNCTION_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=c_sFUNCTION_NAME, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally
            If Not (oWorkTask Is Nothing) Then

                oWorkTask.Dispose()
                oWorkTask = Nothing
            End If
            If Not (oPeril Is Nothing) Then

                oPeril.Dispose()
                oPeril = Nothing
            End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: Form_Query Unload
    '
    ' Description: Store all Property Details before unloading form
    '
    ' Date:30/08/2000
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

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventARgs.cancel = True
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

            If Not (m_oCurrencyConvert Is Nothing) Then

                m_oCurrencyConvert.Dispose()
                m_oCurrencyConvert = Nothing
            End If
            

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



    Private Function CreateChequePaymentTask() As Integer
        Dim result As Integer = 0
        Dim bCLMPeril, iPMWrkTaskInstanceTemp As Object
        Dim vKeyArray(,) As Object
        Dim lPMWrkTaskInstanceCnt As Integer
        Dim sTaskGroupCode, sTemp As String

        Dim oWrkTaskInstanceTemp As Object

        Dim oPeril As bCLMPeril.Business

        Const c_sTaskCode As String = "ACTPAYV2"
        Const kMethodName As String = "CreditClient"
        Const kErrorCode As Integer = Constants.vbObjectError
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Create instances of the objects we need to use.
            Dim temp_oWrkTaskInstanceTemp As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkTaskInstanceTemp, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWrkTaskInstanceTemp = temp_oWrkTaskInstanceTemp
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get instance of iPMWrkTaskInstanceTemp.Interface")
            End If

            Dim temp_oPeril As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPeril, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPeril = temp_oPeril
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get instance of bCLMPeril.Business")
            End If


            m_lReturn = oPeril.GetTaskGroupCode(v_sTaskCode:=c_sTaskCode, r_sTaskGroupCode:=sTaskGroupCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Function g_oBusiness.GetTaskGroupCode failed.")
            End If

            sTemp = "CLAIM: " & m_sClaimRef.Trim() & " - Payment authorised - cheque requested"

            'Resize the key array.
            ReDim vKeyArray(1, 13)


            'Populate the key array with all of the keys required to create a work task.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sTaskGroupCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = c_sTaskCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskDescription

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = sTemp

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskCustomer

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sPartyName.Trim()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameUseExtraKeys

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = True

            'Populate the key array with all of the extra keys required to run the authorise navigator.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameCashListRoadmap

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = "PAYMENTS"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lPartyid

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNamePayeeName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = txtPayeeName.Text.Trim()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNamePayeeAccountCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = txtAccountNo.Text.Trim()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNamePayeeSortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = txtSortCode.Text.Trim()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNamePayeeComments

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = txtPayeeComments.Text.Trim()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNameClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_cAmount

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.ACTKeyNameMediaTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = cboMediaType.ItemId 'm_lPayeeMediaType
            'Pass the keys into the object.

            m_lReturn = oWrkTaskInstanceTemp.SetKeys(vKeyArray:=vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Function oWrkTaskInstance.SetKeys failed.")
            End If


            m_lReturn = oWrkTaskInstanceTemp.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)


            oWrkTaskInstanceTemp.CallingAppName = "Authorise Claim Payment Task"

            'Start the object.

            m_lReturn = oWrkTaskInstanceTemp.Start
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Function oWrkTaskInstance.Start failed.")
            End If



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateChequePaymentTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateChequePaymentTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes
            If Not (oWrkTaskInstanceTemp Is Nothing) Then

                oWrkTaskInstanceTemp.Dispose()
                oWrkTaskInstanceTemp = Nothing
            End If

            If Not (oPeril Is Nothing) Then

                oPeril.Dispose()
                oPeril = Nothing
            End If





        End Try
        Return result
    End Function

    Function CreditClient() As Integer
        Dim result As Integer = 0
        Dim bACTImportSiriusTrans, bSirParty, bSirInsuranceFile, bACTAutoNumber, bACTDocumentPost As Object

        Const kMethodName As String = "CreditClient"
        Const kErrorCode As Integer = Constants.vbObjectError


        Dim oDocumentPost As bACTDocumentPost.Form

        Dim oPMAutoNumber As bACTAutoNumber.Business

        Dim oInsuranceFile As bSirInsuranceFile.Business

        Dim oParty As bSirParty.Business

        Dim oSiriusTrans As bACTImportSiriusTrans.Business

        Dim lDocumentType As Integer
        Dim sGroupCode, sRangeCode As String
        Dim lNumberRangeID As Integer
        Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign
        Dim iCompanyID As Integer
        Dim lNumber As Integer
        Dim sDocumentRef As String = ""
        Dim dtAccountingDate As Date
        Dim lDocumentID As Integer
        Dim vDrawerSubBranchId As Object
        Dim iCurrencyID As Integer

        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Object
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroCcyXrate, vdEuroBaseXrate As Object
        Dim vdCurrencyAmountUnrounded As Double
        Dim vdBaseAmountUnrounded As Double
        Dim lAccountID, lTransDetailID As Integer
        Dim v_Details As Object
        Dim sInsurer, sInsuranceFileRef As String

        'eck 11/2005
        Dim lTransCount As Integer
        Dim iRiskTransferAgreement, iRiskTransfer As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oDocumentPost As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oDocumentPost, "bACTDocumentPost.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oDocumentPost = temp_oDocumentPost

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create instance of bACTDocumentPost.Form.")
            End If

            Dim temp_oPMAutoNumber As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMAutoNumber, "bACTAutoNumber.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMAutoNumber = temp_oPMAutoNumber

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create instance of bACTAutoNumber.Business.")
            End If

            Dim temp_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInsuranceFile, "bSirInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oInsuranceFile = temp_oInsuranceFile

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create instance of bSirInsuranceFile.Business.")
            End If

            Dim temp_oParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oParty, "bSirParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oParty = temp_oParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create instance of bSirParty.Business.")
            End If

            Dim temp_oSiriusTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSiriusTrans, "bACTImportSiriusTrans.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSiriusTrans = temp_oSiriusTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create instance of oSiriusTrans.Business.")
            End If

            iCompanyID = g_oObjectManager.SourceID
            dtAccountingDate = DateTime.Now
            iCurrencyID = g_oObjectManager.CurrencyID
            cCurrencyAmount = m_cAmount
            m_lPayeeMediaType = cboMediaType.ItemId

            ' Check if we are creating a credit or debit for the client account PN24371
            If cCurrencyAmount > 0 Then
                eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit
                lDocumentType = gACTLibrary.ACTDocTypeClaimPayment
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef28
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClp
            Else
                eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit
                lDocumentType = gACTLibrary.ACTDocTypeClaimReceipt
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef29
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClr
            End If

            'S4B Claims Enhancements - get the Insurance File reference

            m_lReturn = oInsuranceFile.GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", oInsuranceFile.GetDetails failed")
            End If


            m_lReturn = oInsuranceFile.GetNext(r_vFieldArray:=v_Details)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", oInsuranceFile.GetNext failed")
            End If


            sInsuranceFileRef = gPMFunctions.ToSafeString(CStr(v_Details(7))).Trim()

            ' Get the number range

            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", GetNumberRange failed")
            End If

            ' Generate the next number
            'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = oPMAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=g_oObjectManager.UserID, v_iCompanyID:=iCompanyID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=sRangeCode)
            'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", GetNumber failed")
            End If

            ' Format the number
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'sDocumentRef = Format(lNumber, "00000000")
            sDocumentRef = sRangeCode & sDocumentRef


            m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeId:=lDocumentType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="Cash", r_vDocumentId:=lDocumentID, r_vDocSourceID:=iCompanyID, v_vBatchId:=0, r_vSubBranchId:=vDrawerSubBranchId, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_vClaimID:=m_lClaimID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", AddDocument failed")
            End If




            m_lReturn = CType(GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=CDec(vdCurrencyBaseXRate), v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=CByte(vdEuroCcyXrate), r_vEuroBaseXRate:=CByte(vdEuroBaseXrate), r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", GetBaseAmountFromCurrency failed")
            End If


            m_lReturn = oParty.GetAccountID(vPartyRef:=m_sPartyName, vAccountID:=m_lAccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", GetAccountID failed")
            End If

            cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
            cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)
            vdBaseAmountUnrounded = gACTLibrary.ACTSigned(vdBaseAmountUnrounded, eCreditOrDebit)
            vdCurrencyAmountUnrounded = gACTLibrary.ACTSigned(vdCurrencyAmountUnrounded, eCreditOrDebit)
            lTransDetailID = 0

            ' now create the credit transaction (to the client)

            m_lReturn = oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=1, v_lAccountId:=m_lAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCCyXrate:=vdEuroCcyXrate, v_vComment:=txtComments.Text.Trim(), v_vAccountingDate:=dtAccountingDate, v_vSpare:="", v_vSubBranchId:=vDrawerSubBranchId, v_vDocSourceID:=iCompanyID, v_vInsuranceRef:=sInsuranceFileRef, v_vClaimReference:=m_sClaimRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", oDocumentPost.AddTransaction failed for client")
            End If
            'eck 11/2005 Insurer Details
            ' get the party id for the insurer
            lTransCount = 1
            If eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit Then
                eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit
            Else
                eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit
            End If

            If Information.IsArray(m_vInsurerPayments) Then

                For lInsurers As Integer = 0 To m_vInsurerPayments.GetUpperBound(1)

                    '            lAccountID = v_Details(9)
                    '
                    '            ' find the shortcode for the insurer

                    '            m_lReturn = oParty.GetDetails(vPartyCnt:=lAccountID)
                    '            If (m_lReturn& <> PMTrue) Then
                    '                Err.Raise kErrorCode, kMethodName, _
                    ''                    "oParty.GetDetails failed"
                    '            End If
                    '
                    '
                    '            m_lReturn = oParty.GetNext(vShortname:=sInsurer)
                    '            If (m_lReturn& <> PMTrue) Then
                    '                Err.Raise kErrorCode, kMethodName, _
                    ''                    "oParty.GetNext failed"
                    '            End If
                    If CDec(m_vInsurerPayments(1, lInsurers)) <> 0 Then
                        sInsurer = CStr(m_vInsurerPayments(0, lInsurers))


                        m_lReturn = oParty.GetAccountID(vPartyRef:=sInsurer, vAccountID:=lAccountID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", oParty.GetAccountID failed")
                        End If

                        cCurrencyAmount = CDec(m_vInsurerPayments(1, lInsurers))



                        m_lReturn = CType(GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=CDec(vdCurrencyBaseXRate), v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=CByte(vdEuroCcyXrate), r_vEuroBaseXRate:=CByte(vdEuroBaseXrate), r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", GetBaseAmountFromCurrency failed")
                        End If

                        cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
                        cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)
                        vdBaseAmountUnrounded = gACTLibrary.ACTSigned(vdBaseAmountUnrounded, eCreditOrDebit)
                        vdCurrencyAmountUnrounded = gACTLibrary.ACTSigned(vdCurrencyAmountUnrounded, eCreditOrDebit)

                        lTransDetailID = 0
                        lTransCount += 1


                        iRiskTransferAgreement = oSiriusTrans.GetInsurerRiskTransferAgreement(lDocumentID:=lDocumentID, lAccountID:=lAccountID)
                        If iRiskTransferAgreement = 0 Then ' No Agreement
                            iRiskTransfer = 1 ' RT status Raised
                        Else
                            iRiskTransfer = 0 ' RT - 0 for Insurer with RT Agreement
                        End If

                        ' now create the debit transaction (from the insurer)

                        m_lReturn = oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=lTransCount, v_lAccountId:=lAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCCyXrate:=vdEuroCcyXrate, v_vComment:=txtComments.Text.Trim(), v_vAccountingDate:=dtAccountingDate, v_vSpare:="", v_vSubBranchId:=vDrawerSubBranchId, v_vDocSourceID:=iCompanyID, v_vInsuranceRef:=sInsuranceFileRef, v_vClaimReference:=m_sClaimRef, v_vRiskTransfer:=iRiskTransfer)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", oDocumentPost.AddTransaction failed for insurer")
                        End If
                    End If
                Next lInsurers
            End If


            m_lReturn = g_oBusiness.UpdateClaimPayment(v_lDocumentId:=lDocumentID, v_lClaimPaymentId:=m_lClaimPaymentID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", oDocumentPost.AddTransaction failed to update claim payment")
            End If

        Catch ex As Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes
            If Not (oDocumentPost Is Nothing) Then

                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If

            If Not (oPMAutoNumber Is Nothing) Then

                oPMAutoNumber.Dispose()
                oPMAutoNumber = Nothing
            End If

            If Not (oInsuranceFile Is Nothing) Then

                oInsuranceFile.Dispose()
                oInsuranceFile = Nothing
            End If

            If Not (m_oCurrencyConvert Is Nothing) Then

                m_oCurrencyConvert.Dispose()
                m_oCurrencyConvert = Nothing
            End If

            If Not (oParty Is Nothing) Then

                oParty.Dispose()
                oParty = Nothing
            End If

        End Try

        Return result
    End Function

    Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Decimal, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Byte, ByRef r_vEuroBaseXRate As Byte, ByRef r_vCCyAmountUnrounded As Decimal, ByRef r_vBaseAmountUnrounded As Decimal) As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "CreditClient"
        Const kErrorCode As Integer = Constants.vbObjectError
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Calculate Base Amount if Currency


            ' CF141298 - Commented as shouldn't be hardcoded to wrong value!
            'EK 100100 This should do it
            If v_iCurrencyID <> gACTLibrary.CompanyBaseCurrency() Then


                'TODO LIST
                'm_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID cBaseAmount:=r_cBaseAmount, cCurrencyAmount:=v_cCurrencyAmount, vConversionDate:=v_dtAccountingDate, vRounded:=True, lEuro:=r_lEuro, cEuroAmount:=r_cEuroAmount, vEuroCCyXrate:=r_vEuroCCyXrate, vEuroBaseXRate:=r_vEuroBaseXRate, vCCyAmountUnRounded:=r_vCCyAmountUnrounded, vBaseAmountUnRounded:=r_vBaseAmountUnrounded)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If


                'TODO::
                'r_vdCurrencyBaseXRate = m_oCurrencyConvert.ConversionRate

            Else
                ' Home currency transaction
                r_cBaseAmount = v_cCurrencyAmount
                r_vdCurrencyBaseXRate = 1
                r_lEuro = 0
                r_cEuroAmount = 0
                r_vEuroCCyXrate = 0
                r_vEuroBaseXRate = 0
                ' RAW 12/03/2003 : ISS2893 : added
                If r_vCCyAmountUnrounded = 0 Then
                    r_vCCyAmountUnrounded = v_cCurrencyAmount
                    r_vBaseAmountUnrounded = r_cBaseAmount
                Else
                    r_vBaseAmountUnrounded = r_vCCyAmountUnrounded
                End If
            End If



        Catch ex As Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name:Form_Resize
    '
    ' Description: Resize the the controls on form
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch


            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: cmdOK_Click
    '
    ' Description:Set Properties of the form on clicking OK Button from the
    '               relevant list item under focus or clicked
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lButtonClicked = gPMConstants.PMEReturnCode.PMOK

            m_sComments = txtComments.Text.Trim()

            m_lPayeeMediaType = cboMediaType.ItemId
            m_sPayeeName = txtPayeeName.Text
            m_sPayeeBankName = txtBankName.Text
            m_sPayeeSortCode = txtSortCode.Text
            m_sPayeeAccountNo = txtAccountNo.Text
            m_lPayeeCountry = cboCountry.ItemId
            m_sPayeeComments = txtPayeeComments.Text

            'Always check the mandatory controls... PN 18113
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then Exit Sub

            If m_bMediaTypeMandatory And cboMediaType.ItemId = 0 And (m_bPayeeTab) Then
                MessageBox.Show("This is a mandatory field. You must enter data in this field.", "Mandatory Field - Media Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                cboMediaType.Focus()
                Exit Sub
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: cmdCancel_Click
    '
    ' Description:Unload the Form
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lButtonClicked = gPMConstants.PMEReturnCode.PMCancel

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
    ' Name: DisplayMessage
    '
    ' Description: Display the Suitable Message
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu

    ' ***************************************************************** '

    'Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    '

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    '
    ' Display the status message.
    '
    'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub cmdParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdParty.Click

        Dim oFindParty As iPMBFindParty.Interface_Renamed

        ' Create Find Party object
        Dim temp_oFindParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oFindParty = temp_oFindParty

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End If

        ' Set component properties and start interface

        oFindParty.CallingAppName = ACApp


        oFindParty.AgentOnly = m_nTypeofParty

        'TN20010412 Start

        oFindParty.SpecialParty = "4"
        'TN20010412 End


        'DC190602 -broking now does the same


        oFindParty.IgnoreDriversAndWitnesses = True



        oFindParty.NotEditable = gPMConstants.PMEReturnCode.PMTrue

        If m_nTypeofParty = PMBPartyTypeCorporateClientText Then

            oFindParty.SpecialParty = "OT"
        End If

        If m_nTypeofParty = PMBPartyNone Then

            oFindParty.SpecialParty = ""
        End If


        m_lReturn = oFindParty.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End If


        If oFindParty.PartyCnt > 0 Then

            txtParty.Text = oFindParty.LongName


            m_lPartyid = oFindParty.PartyCnt

        End If

        ' Destroy Find Party object

        oFindParty.Dispose()
        oFindParty = Nothing
    End Sub

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

            txtAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cAmount))
            txtComments.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sComments)
            cboCurrency.ItemId = m_iCurrencyID
            If m_bFromNavigator Then

                txtParty.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sPartyName)

                If m_lPayeeMediaType > 0 Then
                    cboMediaType.ItemId = m_lPayeeMediaType
                End If
                If m_lPayeeCountry > 0 Then
                    cboCountry.ItemId = m_lPayeeCountry
                End If

                txtPayeeName.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sPayeeName)
                txtBankName.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sPayeeBankName)
                txtSortCode.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sPayeeSortCode)
                txtAccountNo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sPayeeAccountNo)
                txtPayeeComments.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_sPayeeComments)

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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

            'Party
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtParty, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'MKR PN 18085 Make Media Type Field Mandatory for Broking.
            m_bMediaTypeMandatory = False


            m_lReturn = g_oBusiness.GetOptionMediaTypeMandatory(m_lProductID, m_bMediaTypeMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_bMediaTypeMandatory) And (m_bPayeeTab) Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboMediaType, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

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

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        If Not (m_oInsuranceFile Is Nothing) Then

            m_oInsuranceFile.Dispose()
            m_oInsuranceFile = Nothing
        End If
    End Sub

    Private Sub OptAgentAccount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptAgentAccount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            m_nTypeofParty = PMBPartyTypeAgentText
            'JMK 21/08/2001

            cmdParty.Enabled = False
            txtParty.Text = AgentName
            m_lPartyid = AgentID
        End If
    End Sub

    Private Sub OptAgentAccount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptAgentAccount.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    End Sub

    Private Sub OptClaimAccount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClaimAccount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            m_nTypeofParty = PMBPartyNone

            'party button should be disabled if this option selected
            txtParty.Text = IIf(m_lScreenMethod = ACPaymentMethod, "CLMPAYABLE", "CLMRECEIVABLE")
            cmdParty.Enabled = False

            ' Clear party id
            m_lPartyid = 0
        End If
    End Sub

    Private Sub OptClaimAccount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClaimAccount.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    End Sub

    Private Sub OptClientAccount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClientAccount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            m_nTypeofParty = PMBPartyTypePersonalClientText
            'JMK 21/08/2001

            cmdParty.Enabled = False
            txtParty.Text = ClientName
            m_lPartyid = ClientID
        End If
    End Sub

    Private Sub OptClientAccount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClientAccount.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    End Sub

    Private Sub OptPartyAccount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptPartyAccount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            m_nTypeofParty = PMBPartyTypeCorporateClientText
            cmdParty.Enabled = True
            'JAS 13012005 PN18033
            txtParty.Text = ""

        End If
    End Sub

    Private Sub OptPartyAccount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptPartyAccount.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    End Sub

    'DJM 23/03/2004 : Validation copied from iPMBFinancePlanMaint.dll
    Private Sub txtAccountNo_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtAccountNo.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim oSirMediaTypeValidation As bSirMediaTypeValidation.business
        Dim bValid As Boolean
        Dim sStrippedString As String = ""

        Try

            If Strings.Len(txtAccountNo.Text) > 0 Then

                Dim temp_oSirMediaTypeValidation As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSirMediaTypeValidation.business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oSirMediaTypeValidation = temp_oSirMediaTypeValidation

                sStrippedString = txtSortCode.Text.Replace(" ", "") & _
                                  txtAccountNo.Text.Replace(" ", "")


                oSirMediaTypeValidation.ValidateNumber(cboMediaType.ItemId, g_lCountryID, sStrippedString, bValid)
                If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                    MessageBox.Show("Failed to validate Account No", "Validate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Cancel = True
                Else
                    If Not bValid Then
                        MessageBox.Show("This is not a valid bank account", "Invalid Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Cancel = True
                    End If
                    oSirMediaTypeValidation = Nothing
                End If
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Account No Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAccountNo_Validate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
            eventArgs.Cancel = Cancel
        End Try
    End Sub

    Private Sub txtComments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComments.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
        'PN 14477 Fixed Jitendra
        VB6.SetDefault(cmdOK, False)
    End Sub

    Private Sub txtComments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComments.Leave
        'PN 14477 Fixed Jitendra
        VB6.SetDefault(cmdOK, True)
    End Sub

    Private Sub txtPayeeComments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayeeComments.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
        'PN 14477 Fixed Jitendra
        VB6.SetDefault(cmdOK, False)
    End Sub

    Private Sub txtPayeeComments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayeeComments.Leave
        'PN 14477 Fixed Jitendra
        VB6.SetDefault(cmdOK, True)
    End Sub

    Private Sub txtPayeeName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayeeName.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
    End Sub


    'DJM 23/03/2004 : Validation copied from iPMBFinancePlanMaint.dll
    Private Sub txtSortCode_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtSortCode.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Const Valid As String = "0123456789"

        For iCount As Integer = 1 To Strings.Len(txtSortCode.Text)
            If (Valid.IndexOf(Mid(txtSortCode.Text, iCount, 1)) + 1) = 0 Then
                MessageBox.Show("Sort Code can only be made up of numbers.", "Invalid Sort Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Cancel = True
                Exit For
            End If
        Next iCount

        If (txtSortCode.Text.Trim().Length > 0) And (g_lCountryID = 1) And (Not Cancel) Then
            If txtSortCode.Text.Trim().Length <> 6 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                txtSortCode.Focus()
                txtSortCode.SelectionStart = 0
                txtSortCode.SelectionLength = Strings.Len(txtSortCode.Text)
                MessageBox.Show("Sort Code Invalid Length", "Sort Code", MessageBoxButtons.OK)
                Cancel = True
            End If
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim m_ctlTabFirstLast(1, 1) As Object

            m_ctlTabFirstLast(ACControlStart, 0) = OptClaimAccount
            m_ctlTabFirstLast(ACControlEnd, 0) = txtComments
            m_ctlTabFirstLast(ACControlStart, 1) = cboMediaType
            m_ctlTabFirstLast(ACControlEnd, 1) = txtPayeeComments

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function CalculateInsurerPostings(ByRef v_cAmount As Decimal) As Integer
        Dim result As Integer = 0
        Dim cRunningTotal As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC090606 change the way claims coinsurers are processed for datasure
            If Information.IsArray(m_vInsurerSplit) Then

                For lInsurers As Integer = 0 To m_vInsurerSplit.GetUpperBound(1)
                    If Not Information.IsArray(m_vInsurerPayments) Then
                        ReDim m_vInsurerPayments(1, m_vInsurerSplit.GetUpperBound(1))
                        m_vInsurerPayments(0, 0) = m_vInsurerSplit(3, 0)
                        If CDbl(m_vInsurerSplit(1, 0)) = 0 Then
                            m_vInsurerPayments(1, 0) = 0
                        Else
                            m_vInsurerPayments(1, 0) = Math.Round(v_cAmount * CDbl(m_vInsurerSplit(1, 0)) / 100, 2)
                        End If
                    Else
                        For lPayments As Integer = 0 To m_vInsurerPayments.GetUpperBound(1)
                            If m_vInsurerPayments(0, lPayments).Equals(m_vInsurerSplit(3, lInsurers)) Then
                                m_vInsurerPayments(1, lPayments) = CDbl(m_vInsurerPayments(1, lPayments)) + Math.Round(v_cAmount * CDbl(m_vInsurerSplit(1, lInsurers)) / 100, 2)
                                Exit For
                            Else
                                If CStr(m_vInsurerPayments(0, lPayments)) = "" Then
                                    m_vInsurerPayments(0, lPayments) = m_vInsurerSplit(3, lInsurers)
                                    If CDbl(m_vInsurerSplit(1, lInsurers)) = 0 Then
                                        m_vInsurerPayments(1, lPayments) = 0
                                    Else
                                        m_vInsurerPayments(1, lPayments) = Math.Round(v_cAmount * CDbl(m_vInsurerSplit(1, lInsurers)) / 100, 2)
                                    End If
                                    Exit For
                                End If
                            End If
                        Next lPayments
                    End If
                    cRunningTotal += Math.Round(v_cAmount * CDbl(m_vInsurerSplit(1, lInsurers)) / 100, 2)
                Next lInsurers
                'get rid of any rounding errors
                If v_cAmount - cRunningTotal <> 0 Then
                    m_vInsurerPayments(1, m_vInsurerPayments.GetUpperBound(1)) = CDbl(m_vInsurerPayments(1, m_vInsurerPayments.GetUpperBound(1))) + v_cAmount - cRunningTotal
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateInsurerPostings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************************
    ' Name : ShowMultiCurrencyDialogue
    '
    ' Description : Displays the multi-currency dialogue if required:
    '               If policy and base currency are different AND ((User cannot
    '               change rates AND System Option 156 enabled) OR user
    '               can change rates)
    '
    ' History :
    ' 12052004 RDC created
    '*******************************************************************************
    Public Function ShowMultiCurrencyDialogue() As Integer

        Dim result As Integer = 0
        Dim bChangeDate, bChangeRate, bCanChangeCurrency As Boolean
        Dim iBaseCurrencyID As Integer
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim sResult As String = ""
        Dim oForm As frmMultiCurrency
        Dim vClaimBaseCurrencyDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetUserCurrencyAuthorities(v_iUserID:=g_iUserID, r_bChangeDate:=bChangeDate, r_bChangeRate:=bChangeRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get user currency authorities from business")
            End If

            ' either option set to true will do
            bCanChangeCurrency = (bChangeDate Or bChangeRate)


            m_lReturn = g_oBusiness.GetClaimBaseCurrencyDetails(v_lClaimID:=m_lClaimID, r_vResults:=vClaimBaseCurrencyDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get claim base branch currency from business")
            Else
                If Information.IsArray(vClaimBaseCurrencyDetails) Then

                    iBaseCurrencyID = CInt(vClaimBaseCurrencyDetails(0, 0))
                Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get claim base branch currency from business")
                End If

            End If


            ' option 157
            m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=157, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)

            oForm = New frmMultiCurrency()


            'Developer Guide No. 68
            'Load(oForm)

            'Set up multi-currency screen
            oForm.TransactionCurrencyID = m_iCurrencyID
            oForm.TransactionAmount = m_cAmount
            oForm.SourceID = m_lClaimCompanyID
            oForm.PartyCnt = m_lPartyid
            oForm.ClaimID = m_lClaimID
            oForm.ScreenMethod = m_lScreenMethod
            oForm.LossCurrencyAmount = m_cLossCurrencyAmount
            oForm.LossCurrencyID = m_iLossCurrencyID

            m_lReturn = CType(CType(oForm, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to initialise the multi-currency dialogue")
            End If

            'If m_iCurrencyID = iBaseCurrencyID Or (Trim(sResult) <> "1" And Not bCanChangeCurrency) Then
            If (bCanChangeCurrency Or sResult.Trim() = "1") And Not (m_iCurrencyID = iBaseCurrencyID) Then
                'show the form
                oForm.ShowDialog()
                lStatus = oForm.Status
            Else
                ' claim currency is base currency, so silently save the rates
                lStatus = oForm.InterfaceToProperties()
                oForm.Status = gPMConstants.PMEReturnCode.PMOK
            End If

            oForm.Dispose()

            oForm.Close()

            oForm = Nothing


            Return lStatus

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowMultiCurrencyDialogue failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Private Function CreateInsuranceFile() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oInsuranceFile Is Nothing Then
                'Get Insurance File Object.
                Dim temp_m_oInsuranceFile As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oInsuranceFile = temp_m_oInsuranceFile
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create instance of bSIRInsuranceFile.Business")
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateInsuranceFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInsuranceFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function GetClaimCompany() As Integer
        Dim result As Integer = 0
        Dim vInsuranceFile As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get insurance file object
            m_lReturn = CType(CreateInsuranceFile(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateInsuranceFile failed.")
            End If

            'Get company id from insurance file

            m_lReturn = m_oInsuranceFile.GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oInsuranceFile.GetDetails failed.")
            End If


            m_lReturn = m_oInsuranceFile.GetNext(r_vFieldArray:=vInsuranceFile)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oInsuranceFile.GetNext failed.")
            End If


            m_lClaimCompanyID = CInt(vInsuranceFile(5))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCompany failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293
        'start
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
        'end
    End Sub
End Class
