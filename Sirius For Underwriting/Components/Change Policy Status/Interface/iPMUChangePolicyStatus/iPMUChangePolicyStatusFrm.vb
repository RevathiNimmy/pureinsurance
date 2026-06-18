Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Description: Main interface.
    '
    ' History:
    '          VB 05/04/2005 PN19874: Added QuoteStatus Property for check status of quote.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRChangePolicyStatus.Business

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Data variables
    Private m_lInsuranceFileCnt As Integer

    Private m_sInsuredName As String = ""
    Private m_sAgentName As String = ""
    Private m_sInceptionDate As String = ""
    Private m_sCoverFromDate As String = ""
    Private m_sExpiryDate As String = ""
    Private m_cNetPremium As Decimal
    Private m_cTax As Decimal
    Private m_cFee As Decimal
    Private m_cTotalPremium As Decimal
    Private m_sCurrency As String = ""
    Private m_sQuoteStatus As String = ""

    Public WriteOnly Property BusinessObject() As bSIRChangePolicyStatus.Business
        Set(ByVal Value As bSIRChangePolicyStatus.Business)
            m_oBusiness = Value
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
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property InsuranceFileCnt() As String
        Set(ByVal Value As String)
            m_lInsuranceFileCnt = CInt(Value)
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

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public ReadOnly Property QuoteStatus() As String
        Get
            Return m_sQuoteStatus
        End Get
    End Property

    Private Sub cmdMakeLive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMakeLive.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Me.Visible Then Me.Hide()
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the MakeLive command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMakeLive_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdRequote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRequote.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Me.Visible Then Me.Hide()
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Requote command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRequote_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdSaveQuote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSaveQuote.Click

        Try

            ' Set the interface status.
            ' We set it to "fail" to allow user to cancel the current screen.
            ' The normal PMCancel status is already used to go back to the
            ' policy screen
            m_lStatus = gPMConstants.PMEReturnCode.PMFail

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Me.Visible Then Me.Hide()
                m_sQuoteStatus = "Quote Saved."
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the SaveQuote command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSaveQuote_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Display Data on screen
            m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get display details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object

        Const ARRAY_INSUREDNAME As Integer = 0
        Const ARRAY_AGENT As Integer = 1
        Const ARRAY_INCDATE As Integer = 2
        Const ARRAY_COVERFROMDATE As Integer = 3
        Const ARRAY_EXPIRYDATE As Integer = 4
        Const ARRAY_NET As Integer = 5
        Const ARRAY_TAX As Integer = 6
        Const ARRAY_FEE As Integer = 7
        Const ARRAY_TOTAL As Integer = 8
        Const ARRAY_CURRENCY As Integer = 9

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we do have a valid business object
            If m_oBusiness Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get data from business object

            m_lReturn = m_oBusiness.GetPolicySummary(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResult:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get data from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Copy from array to local variables
            If Information.IsArray(vResult) Then


                m_sInsuredName = CStr(vResult(ARRAY_INSUREDNAME, 0))

                m_sAgentName = CStr(vResult(ARRAY_AGENT, 0))

                m_sInceptionDate = CStr(vResult(ARRAY_INCDATE, 0))

                m_sCoverFromDate = CStr(vResult(ARRAY_COVERFROMDATE, 0))

                m_sExpiryDate = CStr(vResult(ARRAY_EXPIRYDATE, 0))


                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vResult(ARRAY_NET, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    m_cNetPremium = 0
                Else

                    m_cNetPremium = CDec(vResult(ARRAY_NET, 0))
                End If


                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vResult(ARRAY_TAX, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    m_cTax = 0
                Else

                    m_cTax = CDec(vResult(ARRAY_TAX, 0))
                End If


                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vResult(ARRAY_FEE, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    m_cFee = 0
                Else

                    m_cFee = CDec(vResult(ARRAY_FEE, 0))
                End If


                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vResult(ARRAY_TOTAL, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    m_cTotalPremium = 0
                Else

                    m_cTotalPremium = CDec(vResult(ARRAY_TOTAL, 0))
                End If


                m_sCurrency = CStr(vResult(ARRAY_CURRENCY, 0))

            End If

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtInsuredName.Text = m_sInsuredName
            txtAgent.Text = m_sAgentName
            txtIncDate.Text = m_sInceptionDate
            txtCoverFromDate.Text = m_sCoverFromDate
            txtExpiryDate.Text = m_sExpiryDate
            '**fixed PN-14741 *****JT*******
            'Changed the FormatCurrency to FormatField
            txtNetPolicyPremium.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cNetPremium, gPMConstants.PMEDataType.PMDecimal)
            txtTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cTax, gPMConstants.PMEDataType.PMDecimal)
            txtPolicyFee.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cFee, gPMConstants.PMEDataType.PMDecimal)
            'PN -15083 JT
            txtTotalPremium.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cTotalPremium, gPMConstants.PMEDataType.PMDecimal)
            '**END fixed PN-14741
            txtCurrency.Text = m_sCurrency

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_FormCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Buttons

            'cmdRequote.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Button_Requote, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdRequote.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Button_Requote, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdSaveQuote.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Button_Save, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdSaveQuote.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Button_Save, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'cmdMakeLive.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Button_MakeLive, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdMakeLive.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Button_MakeLive, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Labels

            'lblInsuredName.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_InsuredName, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblInsuredName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_InsuredName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblAgent.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Agent, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Agent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblIncDate.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_IncDate, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblIncDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_IncDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblCoverFromDate.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_CoverFromDate, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCoverFromDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_CoverFromDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblExpiryDate.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_ExpiryDate, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblExpiryDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_ExpiryDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblNetPolicyPremium.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_NetPremium, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblNetPolicyPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_NetPremium, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblTax.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Tax, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Tax, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblPolicyFee.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Fee, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblPolicyFee.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Fee, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblTotalPremium.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_TotalPremium, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblTotalPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_TotalPremium, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblCurrency.Text = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.AC_Currency, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_Currency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function
End Class