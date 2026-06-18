Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:15/07/00
    '
    ' Description: Main interface.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'Count For No of Objects in Collection
    Public g_lCount As Integer
    Private Const Column1 As Integer = 1
    Private Const Column2 As Integer = 2
    Private Const Column3 As Integer = 3
    Private Const Column4 As Integer = 4
    Private Const Column5 As Integer = 5
    Private Const Column6 As Integer = 6
    Private Const Column7 As Integer = 7
    Private Const Column8 As Integer = 8
    Private Const Column9 As Integer = 9
    Private Const Column10 As Integer = 10

    'Constants for Defining Width of Columns in List View

    Private Const ColWidthAdd As Integer = 2000
    Private Const ColWidthEdit As Integer = 1500
    Private Const ColWidthReceipt As Integer = 1500

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Variables for Salvage Recovery
    Private m_lClaimID As Integer
    Private m_sClaimNumber As String = ""
    Private m_nFindClaimMode As Integer
    Private m_lPerilId As Integer
    Private m_sPerilType As String = ""

    'TN20010329 Start
    Private m_lInsuranceFileCnt As Integer
    Private m_lPerilTypeID As Integer
    'TN20010329 End

    Private m_lDecimalCount As Integer
    Private m_lLossCurrencyId As Integer
    Private m_lExchangeRate As Integer
    Private m_lDefaultCoinsuranceTreatment As Integer


    Private m_lRecoveryId As Integer
    Private m_lRecoveryTypeID As Integer
    Private m_cInitialReserve As Decimal
    Private m_cRevisedReserve As Decimal
    Private m_cNewreserve As Decimal
    Private m_cReceivedTodate As Decimal
    Private m_lRevisionCount As Integer
    Private m_cTaxAmount As Decimal
    Private m_sTaxTypeCode As String = ""
    Private m_sTaxTypeDesc As String = ""
    Private m_sTaxBandDesc As String = ""

    'Variables For Salvage Receipt
    Private m_lReceiptID As Integer
    Private m_lPartyClaimID As Integer
    Private m_cReceiptAmount As Decimal
    Private m_cReceiptAmountLoss As Decimal
    Private m_dtDateofReceipt As Object

    Private m_lPaymentID As Integer
    Private m_cPaymentAmount As Decimal
    Private m_dtDateofPayment As Object
    Private m_sComments As String = ""

    'Store RecoveryIds
    Private m_vRecoveryID(,) As Object
    Private m_vRecdToDate(,) As Object


    'Index of the Selected Recovery Type
    Private m_lSelectedIndex As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMSalvageRecovery.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    'RWH(06/04/2001)
    Private m_bRecordSelected As Boolean
    Private m_vCoInsurerDetails(,) As Object
    Private m_vReInsurerDetails(,) As Object

    'JMK 14/11/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""

    Private m_lReceiptCurrencyId As Integer
    Private m_dReceiptToLossRate As Double

    Private m_bDataChanged As Boolean
    Public Property DataChanged() As Boolean
        Get

            Return m_bDataChanged

        End Get
        Set(ByVal Value As Boolean)

            m_bDataChanged = Value

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
    Public Property ClaimNumber() As String
        Get

            Return m_sClaimNumber

        End Get
        Set(ByVal Value As String)

            m_sClaimNumber = Value

        End Set
    End Property

    Public Property ClaimId() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property
    Public Property ClaimMode() As Integer
        Get

            Return m_nFindClaimMode

        End Get
        Set(ByVal Value As Integer)

            m_nFindClaimMode = Value

        End Set
    End Property
    Public Property PerilType() As String
        Get

            Return m_sPerilType

        End Get
        Set(ByVal Value As String)

            m_sPerilType = Value

        End Set
    End Property

    Public Property PerilID() As Integer
        Get

            Return m_lPerilId

        End Get
        Set(ByVal Value As Integer)

            m_lPerilId = Value

        End Set
    End Property

    'TN20010329 Start
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property PerilTypeID() As Integer
        Set(ByVal Value As Integer)
            m_lPerilTypeID = Value
        End Set
    End Property
    'TN20010329 End

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

            ' Get the details from the business object.


            m_lReturn = g_oBusiness.GetDetails(vPerilId:=PerilID)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object/Details are not available for Selected Peril", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If

            End If


            g_lCount = g_oBusiness.RecordCount

            'Populate Co-Insurer Details

            m_lReturn = g_oBusiness.GetCoinsuranceRecoveries(m_vCoInsurerDetails, m_lClaimID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                Else
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Populate Re-Insurer Details

            m_lReturn = g_oBusiness.GetReinsuranceRecoveries(m_vReInsurerDetails, m_lClaimID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                Else
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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
    ' Name: GetReceiptPartyid
    '
    ' Description: Instance PaymentMethod to retrieve Policyholder
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetReceiptPartyid(ByRef lReceiptPartyId As Integer, ByVal cAmount As Decimal, ByRef sOComments As String, ByRef lButtonClicked As Integer, Optional ByVal sIComments As String = "") As Integer
        Dim result As Integer = 0
        Dim oReceiptMethod As iCLMPaymentMethod.Interface_Renamed

        'TN20010820
        Dim vResultArray(,) As Object

        Const ACReceiptMethod As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oReceiptMethod As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReceiptMethod, sClassName:="iCLMPaymentMethod.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReceiptMethod = temp_oReceiptMethod

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReceiptPartyid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oReceiptMethod.CallingAppName = ACApp

            oReceiptMethod.ScreenMethod = ACReceiptMethod

            oReceiptMethod.Amount = cAmount
            'oReceiptMethod.Comments = sIComments

            oReceiptMethod.CurrencyID = m_lReceiptCurrencyId

            oReceiptMethod.ClaimId = m_lClaimID

            oReceiptMethod.InsuranceFileCnt = m_lInsuranceFileCnt

            'TN20010823 - start


            m_lReturn = g_oBusiness.GetClientAgentID(v_lWorkClaimID:=m_lClaimID, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            oReceiptMethod.ClientName = vResultArray(0, 0)


            oReceiptMethod.AgentName = vResultArray(1, 0)


            If CStr(vResultArray(2, 0)) = "" Then

                oReceiptMethod.ClientID = 0
            Else


                oReceiptMethod.ClientID = vResultArray(2, 0)
            End If


            If CStr(vResultArray(3, 0)) = "" Then

                oReceiptMethod.AgentID = 0
            Else


                oReceiptMethod.AgentID = vResultArray(3, 0)
            End If

            ' Alix - 12/02/2004 - Also pass product id

            If CStr(vResultArray(4, 0)) = "" Then

                oReceiptMethod.ProductID = 0
            Else


                oReceiptMethod.ProductID = vResultArray(4, 0)
            End If
            ' /Alix


            'TN20010823 - end


            m_lReturn = oReceiptMethod.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReceiptPartyid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Retrieve Party Shortname and set as Agent

            lReceiptPartyId = oReceiptMethod.Partyid

            lButtonClicked = oReceiptMethod.ButtonClicked

            sOComments = oReceiptMethod.Comments

            ' Destroy Find Party object

            oReceiptMethod.Dispose()
            oReceiptMethod = Nothing


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReceiptPartyid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReceiptPartyid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd
                    'RWH(24/08/01) Make sure Claim Number & PerilType is set on all tabs.
                    cmdOK.Enabled = False
                    For ncount As Integer = 0 To 2
                        txtclaimNumber(ncount).Text = m_sClaimNumber
                        txtPerilType(ncount).Text = m_sPerilType

                    Next ncount

                Case gPMConstants.PMEComponentAction.PMEdit
                    cmdOK.Enabled = False
                    For ncount As Integer = 0 To 2
                        txtclaimNumber(ncount).Text = m_sClaimNumber
                        txtPerilType(ncount).Text = m_sPerilType

                    Next ncount

                Case gPMConstants.PMEComponentAction.PMView

                    cmdOK.Enabled = False
                    For ncount As Integer = 0 To 2
                        txtclaimNumber(ncount).Text = m_sClaimNumber
                        txtPerilType(ncount).Text = m_sPerilType

                    Next ncount

            End Select


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
        Dim iMode As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            iMode = ClaimMode

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                iMode = gPMConstants.PMEComponentAction.PMView
            End If

            '    Select Case ClaimMode

            Select Case iMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    lvwRecovery.Columns.Insert(Column1 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column2 - 1, "", 94)

                    lvwRecovery.Columns.Item(Column2 - 1).TextAlign = HorizontalAlignment.Right

                    lvwRecovery.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthAdd))
                    lvwRecovery.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthAdd))

                    SSTabHelper.SetTabVisible(tabMainTab, 1, False)
                    SSTabHelper.SetTabVisible(tabMainTab, 2, False)

                    cmdAdd.Enabled = True
                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False

                Case gPMConstants.PMEComponentAction.PMEdit
                    'RWH(27/06/01) Add extra column to show total Salvaged amount

                    lvwRecovery.Columns.Insert(Column1 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column2 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column3 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column4 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column5 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column6 - 1, "", 94) 'Blank
                    lvwRecovery.Columns.Insert(Column7 - 1, "", 94) 'Tax Amount
                    lvwRecovery.Columns.Insert(Column8 - 1, "", 94) 'Tax Type Description
                    lvwRecovery.Columns.Insert(Column9 - 1, "", 94) 'Tax Band Description
                    lvwRecovery.Columns.Insert(Column10 - 1, "", 94) 'Receipt Amount (Receipt Currency)


                    lvwRecovery.Columns.Item(Column2 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column3 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column4 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column5 - 1).TextAlign = HorizontalAlignment.Right

                    lvwRecovery.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column3 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column5 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column6 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column7 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column8 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column9 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column10 - 1).Width = CInt(0)

                    cmdAdd.Enabled = True
                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False

                Case gPMConstants.PMEComponentAction.PMView
                    lvwRecovery.Columns.Insert(Column1 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column2 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column3 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column4 - 1, "", 94)
                    lvwRecovery.Columns.Insert(Column5 - 1, "", 94) 'Initial Reserve
                    lvwRecovery.Columns.Insert(Column6 - 1, "", 94) 'Total Salvaged Amount
                    lvwRecovery.Columns.Insert(Column7 - 1, "", 94) 'Tax Amount
                    lvwRecovery.Columns.Insert(Column8 - 1, "", 94) 'Tax Type Description
                    lvwRecovery.Columns.Insert(Column9 - 1, "", 94) 'Tax Band Description
                    lvwRecovery.Columns.Insert(Column10 - 1, "", 94) 'Receipt Amount (Receipt Currency)

                    lvwRecovery.Columns.Item(Column2 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column3 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column4 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column5 - 1).TextAlign = HorizontalAlignment.Right
                    lvwRecovery.Columns.Item(Column6 - 1).TextAlign = HorizontalAlignment.Right

                    lvwRecovery.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column3 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column5 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column6 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwRecovery.Columns.Item(Column7 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column8 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column9 - 1).Width = CInt(0)
                    lvwRecovery.Columns.Item(Column10 - 1).Width = CInt(0)

                    cmdAdd.Enabled = False
                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False

            End Select


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView

                    lvwReinsurance.Columns.Insert(Column1 - 1, "", 94)
                    lvwReinsurance.Columns.Insert(Column2 - 1, "", 94)
                    lvwReinsurance.Columns.Insert(Column3 - 1, "", 94)


                    lvwReinsurance.Columns.Item(Column2 - 1).TextAlign = HorizontalAlignment.Right
                    lvwReinsurance.Columns.Item(Column3 - 1).TextAlign = HorizontalAlignment.Right
                    lvwReinsurance.Columns.Item(Column3 - 1).TextAlign = HorizontalAlignment.Right


                    lvwReinsurance.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwReinsurance.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwReinsurance.Columns.Item(Column3 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))


                    lvwCoInsurance.Columns.Insert(Column1 - 1, "", 94)
                    lvwCoInsurance.Columns.Insert(Column2 - 1, "", 94)
                    lvwCoInsurance.Columns.Insert(Column3 - 1, "", 94)

                    lvwCoInsurance.Columns.Item(Column2 - 1).TextAlign = HorizontalAlignment.Right
                    lvwCoInsurance.Columns.Item(Column3 - 1).TextAlign = HorizontalAlignment.Right

                    lvwCoInsurance.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwCoInsurance.Columns.Item(Column2 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))
                    lvwCoInsurance.Columns.Item(Column3 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))

                    'TN20010906 - add received to date
                    If ClaimMode = gPMConstants.PMEComponentAction.PMView Then
                        lvwReinsurance.Columns.Insert(Column4 - 1, "", 94)
                        lvwReinsurance.Columns.Item(Column4 - 1).TextAlign = HorizontalAlignment.Right
                        lvwReinsurance.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))

                        lvwCoInsurance.Columns.Insert(Column4 - 1, "", 94)
                        lvwCoInsurance.Columns.Item(Column4 - 1).TextAlign = HorizontalAlignment.Right
                        lvwCoInsurance.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidthEdit))

                    End If


            End Select


            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Update the interface details with the
            ' property members.
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

            'RWH(09/04/2001)
            'Made full row select on list views
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwRecovery.Handle.ToInt32(), v_vShowRowSelect:=True)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwCoInsurance.Handle.ToInt32(), v_vShowRowSelect:=True)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwReinsurance.Handle.ToInt32(), v_vShowRowSelect:=True)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            txtExchangeRate.Text = CStr(1)



            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit

                    txtclaimNumber(0).Enabled = False
                    txtPerilType(0).Enabled = False

                    txtclaimNumber(0).BackColor = SystemColors.Control
                    txtPerilType(0).BackColor = SystemColors.Control

                Case gPMConstants.PMEComponentAction.PMView

                    For ncount As Integer = 0 To 2

                        txtclaimNumber(ncount).Enabled = False
                        txtPerilType(ncount).Enabled = False

                        txtclaimNumber(ncount).BackColor = SystemColors.Control
                        txtPerilType(ncount).BackColor = SystemColors.Control

                    Next ncount

                    cboCoinsuranceTreatment.Enabled = False
                    cboCoinsuranceTreatment.BackColor = SystemColors.Control

            End Select

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            txtExchangeRate.Enabled = False

            cboCurrency.Enabled = False

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
            ReDim m_ctlTabFirstLast(1, 2)

            ' Set the first and last data entry controls for
            ' all of the tabs.


            m_ctlTabFirstLast(ACControlStart, 0) = lvwRecovery
            m_ctlTabFirstLast(ACControlEnd, 0) = lvwRecovery



            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMView


                    m_ctlTabFirstLast(ACControlStart, 1) = lvwCoInsurance
                    m_ctlTabFirstLast(ACControlEnd, 1) = lvwCoInsurance


                    m_ctlTabFirstLast(ACControlStart, 2) = lvwReinsurance
                    m_ctlTabFirstLast(ACControlEnd, 2) = lvwReinsurance

            End Select







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

            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit


                    Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                Case gPMConstants.PMEComponentAction.PMView


                    Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageReceipt, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            End Select

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit


                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                Case gPMConstants.PMEComponentAction.PMView


                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                    SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    '            'JMK 14/11/2001
                    '            tabMainTab.TabCaption(2) = iPMFunc.GetResData( _
                    ''                iLangID:=g_iLanguageID%, _
                    ''                lID:=ACTabTitle3, _
                    ''                iDataType:=PMResString)

            End Select

            'JMK 14/11/2001

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))





            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    'Salvage type


                    lvwRecovery.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Intial reserve


                    lvwRecovery.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitialReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                Case gPMConstants.PMEComponentAction.PMEdit

                    'Salvage type


                    lvwRecovery.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Intial reserve


                    lvwRecovery.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitialReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Revised Reserve


                    lvwRecovery.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRevisedReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    '            'Salvaged
                    '             lvwRecovery.ColumnHeaders(4).Text = iPMFunc.GetResData( _
                    ''                iLangID:=g_iLanguageID%, _
                    ''                lID:=ACNewReserve, _
                    ''                iDataType:=PMResString)

                    'New Reserve


                    lvwRecovery.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'TN20010906 - received to date


                    lvwRecovery.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReceivedToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                Case gPMConstants.PMEComponentAction.PMView

                    'Salvage type


                    lvwRecovery.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Salvage Amount


                    lvwRecovery.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'RWH(25/06/01) Added new column Initial Reserve in line with TPR.
                    'Intial reserve


                    lvwRecovery.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitialReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Revised Reserve


                    lvwRecovery.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRevisedReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'New Reserve


                    lvwRecovery.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'TN20010906 - received to date


                    lvwRecovery.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReceivedToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            End Select


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
                    'CoInsurer


                    lvwCoInsurance.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoInsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Share


                    lvwCoInsurance.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShare, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Salvage Amount


                    lvwCoInsurance.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'TN20010906 - received to date
                    If ClaimMode = gPMConstants.PMEComponentAction.PMView Then


                        lvwCoInsurance.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReceivedToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                        lvwReinsurance.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReceivedToDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    End If


                    'reInsurer


                    lvwReinsurance.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReInsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                    'Share


                    lvwReinsurance.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShare, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                    'Salvage Amount


                    lvwReinsurance.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSalvageAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            End Select


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit




                    lblClaimNumber(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblPerilType(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPerilType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                    lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblExchangeRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExchangeRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                Case gPMConstants.PMEComponentAction.PMView



                    lblClaimNumber(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblClaimNumber(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblClaimNumber(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblPerilType(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPerilType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblPerilType(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPerilType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))



                    lblPerilType(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPerilType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                    lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                    lblExchangeRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExchangeRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                    lblCoinsuranceTreatment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCoInsuranceTreatment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            End Select


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

            'Form Width is 9345 Height is 6645


            If VB6.PixelsToTwipsX(Me.Width) < 9345 Then Me.Width = VB6.TwipsToPixelsX(9345)
            If VB6.PixelsToTwipsY(Me.Height) < 6645 Then Me.Height = VB6.TwipsToPixelsY(6645)


            cmdAdd.Left = Me.Width - VB6.TwipsToPixelsX(1665)
            cmdDelete.Left = Me.Width - VB6.TwipsToPixelsX(1665)
            cmdEdit.Left = Me.Width - VB6.TwipsToPixelsX(1665)


            'ImgImage.Left = Me.Width - 975

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(330)
            tabMainTab.Height = Me.Height - VB6.TwipsToPixelsY(1590)


            lvwRecovery.Width = Me.Width - VB6.TwipsToPixelsX(2130)
            lvwRecovery.Height = Me.Height - VB6.TwipsToPixelsY(3150)

            If ClaimMode = gPMConstants.PMEComponentAction.PMView Then

                lvwCoInsurance.Width = Me.Width - VB6.TwipsToPixelsX(810)
                lvwCoInsurance.Height = Me.Height - VB6.TwipsToPixelsY(3510)

                lvwReinsurance.Width = Me.Width - VB6.TwipsToPixelsX(810)
                lvwReinsurance.Height = Me.Height - VB6.TwipsToPixelsY(3510)

            End If

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1305)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1365)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2505)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1365)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3705)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1365)


            Return result

        Catch





            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' PRIVATE Methods (End)

    Private Sub cboCurrency_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.SelectedIndexChanged

        If VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex) = m_lLossCurrencyId Then

            txtExchangeRate.Enabled = False

            txtExchangeRate.BackColor = SystemColors.Control

            txtExchangeRate.Text = CStr(1)


        Else

            txtExchangeRate.Enabled = True

            txtExchangeRate.BackColor = SystemColors.Window

        End If



    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        g_lButton = ACAddButton


        Dim lExchangeRate As gPMConstants.PMEReturnCode = ExchangeRateMandatory()
        Dim frmSalvageDetails As New frmSalvageDetails
        If lExchangeRate <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = DisableInterface(True)
            Exit Sub
        End If


        Select Case ClaimMode
            Case gPMConstants.PMEComponentAction.PMAdd

                frmSalvageDetails.InitialReserve = 0
                frmSalvageDetails.ClaimMode = ClaimMode
                frmSalvageDetails.RecoveryTypeID = 0
                frmSalvageDetails.RevisedReserve = 0
                frmSalvageDetails.NewReserve = 0
                frmSalvageDetails.PerilID = m_lPerilId
                frmSalvageDetails.LossCurrencyId = m_lLossCurrencyId
                frmSalvageDetails.LossCurrencyDesc = cboCurrency.Text
                frmSalvageDetails.InsuranceFileCnt = m_lInsuranceFileCnt

                Dim tempLoadForm As frmSalvageDetails = frmSalvageDetails

                ' Alix - 15/05/2003
                'PSL 25/06/2003 Iss 4969 Underwriting only

                frmSalvageDetails.FillTaxTypeCombo(g_oBusiness)

                ' /Alix

                frmSalvageDetails.ShowDialog()

                If frmSalvageDetails.Status = gPMConstants.PMEReturnCode.PMOK Then

                    m_cInitialReserve = frmSalvageDetails.InitialReserve
                    m_lRecoveryTypeID = frmSalvageDetails.RecoveryTypeID

                    'RWH(09/04/2001) Initialise amount received to enable edit.
                    m_cReceivedTodate = 0
                    g_lCount += 1

                    ' Alix
                    'PSL 25/06/2003 Iss 4969 Underwriting only

                    m_cTaxAmount = frmSalvageDetails.TaxAmount

                    ' /Alix


                    m_lReturn = g_oBusiness.EditAdd(g_lCount, vRecoveryTypeId:=m_lRecoveryTypeID, vPerilId:=m_lPerilId, vCurrencyID:=m_lLossCurrencyId, vInitialReserve:=m_cInitialReserve, vTable:=ACRecovery, vTaxAmount:=m_cTaxAmount)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    m_lReturn = AddToListView()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If


                End If

                frmSalvageDetails.Close()
                'AJM 25/04/01 add PMView
            Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView

                frmSalvageDetails.InitialReserve = 0
                frmSalvageDetails.RevisedReserve = 0
                frmSalvageDetails.NewReserve = 0
                frmSalvageDetails.ClaimMode = ClaimMode
                frmSalvageDetails.PerilID = m_lPerilId
                frmSalvageDetails.LossCurrencyId = m_lLossCurrencyId
                frmSalvageDetails.LossCurrencyDesc = cboCurrency.Text
                frmSalvageDetails.InsuranceFileCnt = m_lInsuranceFileCnt

                Dim tempLoadForm2 As frmSalvageDetails = frmSalvageDetails

                ' Alix - 15/05/2003
                'PSL 25/06/2003 Iss 4969 Underwriting only

                frmSalvageDetails.FillTaxTypeCombo(g_oBusiness)

                ' /Alix

                frmSalvageDetails.ShowDialog()

                If frmSalvageDetails.Status = gPMConstants.PMEReturnCode.PMOK Then

                    m_cInitialReserve = frmSalvageDetails.InitialReserve
                    m_cRevisedReserve = frmSalvageDetails.RevisedReserve
                    m_cNewreserve = frmSalvageDetails.NewReserve
                    m_lRecoveryTypeID = frmSalvageDetails.RecoveryTypeID
                    m_lReceiptCurrencyId = frmSalvageDetails.ReceiptCurrencyId
                    m_dReceiptToLossRate = frmSalvageDetails.ReceiptToLossRate

                    'RWH(09/04/2001) Initialise amount received to enable edit.
                    m_cReceivedTodate = 0
                    g_lCount += 1


                    m_cTaxAmount = frmSalvageDetails.TaxAmount



                    m_lReturn = g_oBusiness.EditAdd(g_lCount, vRecoveryTypeId:=m_lRecoveryTypeID, vPerilId:=m_lPerilId, vCurrencyID:=m_lLossCurrencyId, vInitialReserve:=m_cInitialReserve, vRevisedReserve:=m_cRevisedReserve, vTable:=ACRecovery, vTaxAmount:=m_cTaxAmount)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    m_lReturn = AddToListView()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                End If

                frmSalvageDetails.Close()

        End Select

        m_lReturn = DisableInterface(True)

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        If lvwRecovery.Items.Count > 0 Then

            If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Or ClaimMode = gPMConstants.PMEComponentAction.PMView Then



                m_lReturn = g_oBusiness.Checkid(m_vRecoveryID(0, Convert.ToString(lvwRecovery.FocusedItem.Tag)))

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    DisplayMessage(ACInvalidDeleteOperation, "Invalid Delete Operation")

                    Exit Sub

                End If

            End If





            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView


                    m_lReturn = g_oBusiness.EditDelete(Convert.ToString(lvwRecovery.FocusedItem.Tag))

                    lvwRecovery.Items.RemoveAt(lvwRecovery.FocusedItem.Index)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        Exit Sub

                    End If


            End Select
        End If

        m_lReturn = DisableInterface(True)

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim frmSalvageDetails As New frmSalvageDetails
        Dim nCollectionObject As Integer

        Dim cTempReceiptAmount, cTempInitialReserve, cTempNewReserve As Decimal

        Dim cTempEditRevisedReserve, cTempEditInitialReserve, cTempEditNewReserve As Decimal

        Dim bDisableCurrency As Boolean


        g_lButton = ACEditButton

        Dim lExchangeRate As gPMConstants.PMEReturnCode = ExchangeRateMandatory()

        If lExchangeRate <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = DisableInterface(True)
            Exit Sub
        End If


        If lvwRecovery.Items.Count > 0 Then


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    'frmSalvageDetails.InitialReserve = CDec(lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 1).Text.Trim())
                    frmSalvageDetails.InitialReserve = CDec(lvwRecovery.FocusedItem.SubItems(1).Text.Trim())
                    frmSalvageDetails.ClaimMode = ClaimMode

                    frmSalvageDetails.RecoveryTypeID = CInt(m_vRecoveryTypeID(0, Convert.ToString(lvwRecovery.FocusedItem.Tag)))
                    frmSalvageDetails.LossCurrencyId = m_lLossCurrencyId
                    frmSalvageDetails.LossCurrencyDesc = cboCurrency.Text
                    frmSalvageDetails.InsuranceFileCnt = m_lInsuranceFileCnt

                    Dim tempLoadForm As frmSalvageDetails = frmSalvageDetails


                    frmSalvageDetails.FillTaxTypeCombo(g_oBusiness)

                    frmSalvageDetails.ShowDialog()

                    If frmSalvageDetails.Status = gPMConstants.PMEReturnCode.PMOK Then

                        m_cInitialReserve = frmSalvageDetails.InitialReserve
                        m_lRecoveryTypeID = frmSalvageDetails.RecoveryTypeID


                        nCollectionObject = Convert.ToString(lvwRecovery.FocusedItem.Tag)

                        m_cTaxAmount = frmSalvageDetails.TaxAmount


                        m_lReturn = g_oBusiness.EditUpdate(nCollectionObject, vRecoveryTypeId:=m_lRecoveryTypeID, vPerilId:=m_lPerilId, vCurrencyID:=m_lLossCurrencyId, vInitialReserve:=m_cInitialReserve, vTable:=ACRecovery, vTaxAmount:=m_cTaxAmount)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                        m_lReturn = EditToListView(m_lSelectedIndex)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                    End If

                    frmSalvageDetails.Close()

                Case gPMConstants.PMEComponentAction.PMEdit

                    'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 2).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(2).Text.Trim() = Nothing Then
                        cTempEditInitialReserve = 0
                    Else
                        cTempEditInitialReserve = CDec(lvwRecovery.FocusedItem.SubItems(1).Text.Trim())
                    End If

                    If lvwRecovery.FocusedItem.SubItems(3).Text.Trim() = Nothing Then
                        cTempEditRevisedReserve = 0
                    Else
                        cTempEditRevisedReserve = CDec(lvwRecovery.FocusedItem.SubItems(2).Text.Trim())
                    End If

                    If lvwRecovery.FocusedItem.SubItems(3).Text.Trim() = Nothing Then
                        cTempEditNewReserve = 0
                    Else
                        cTempEditNewReserve = CDec(lvwRecovery.FocusedItem.SubItems(3).Text.Trim())
                    End If

                    'If an amount has already been selected for another peril then
                    'don't allow the user to change the receipt currency.
                    bDisableCurrency = False
                    For lLoop As Integer = 1 To lvwRecovery.Items.Count
                        If lvwRecovery.Items.Item(lLoop - 1).Text <> lvwRecovery.FocusedItem.Text Then
                            If lvwRecovery.FocusedItem.SubItems(3).Text.Trim() <> Nothing Then
                                If CDec(lvwRecovery.FocusedItem.SubItems(3).Text.Trim()) <> 0 Then
                                    bDisableCurrency = True
                                End If
                            End If
                        End If
                    Next

                    frmSalvageDetails.InitialReserve = cTempEditInitialReserve
                    frmSalvageDetails.NewReserve = cTempEditNewReserve

                    frmSalvageDetails.RecoveryTypeID = CInt(m_vRecoveryTypeID(ACIRecoveryID, Convert.ToString(lvwRecovery.FocusedItem.Tag)))

                    frmSalvageDetails.ReceivedTodate = CDec(m_vRecdToDate(ACIRecoveryID, Convert.ToString(lvwRecovery.FocusedItem.Tag)))
                    frmSalvageDetails.ClaimMode = ClaimMode
                    frmSalvageDetails.LossCurrencyId = m_lLossCurrencyId
                    frmSalvageDetails.LossCurrencyDesc = cboCurrency.Text
                    frmSalvageDetails.InsuranceFileCnt = m_lInsuranceFileCnt
                    frmSalvageDetails.DisableCurrency = bDisableCurrency


                    'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 7).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(7).Text.Trim() = Nothing Then
                        frmSalvageDetails.TaxAmount = 0
                    Else
                        frmSalvageDetails.TaxAmount = CDec(lvwRecovery.FocusedItem.SubItems(7).Text)
                    End If
                    frmSalvageDetails.TaxTypeDesc = lvwRecovery.FocusedItem.SubItems(8).Text
                    frmSalvageDetails.TaxBandDesc = lvwRecovery.FocusedItem.SubItems(9).Text


                    Dim tempLoadForm2 As frmSalvageDetails = frmSalvageDetails

                    frmSalvageDetails.FillTaxTypeCombo(g_oBusiness)

                    frmSalvageDetails.ShowDialog()

                    If frmSalvageDetails.Status = gPMConstants.PMEReturnCode.PMOK Then

                        m_cInitialReserve = frmSalvageDetails.InitialReserve
                        m_cRevisedReserve = frmSalvageDetails.RevisedReserve + cTempEditRevisedReserve
                        m_cNewreserve = frmSalvageDetails.NewReserve
                        m_lRecoveryTypeID = frmSalvageDetails.RecoveryTypeID
                        m_lReceiptCurrencyId = frmSalvageDetails.ReceiptCurrencyId
                        m_dReceiptToLossRate = frmSalvageDetails.ReceiptToLossRate



                        nCollectionObject = Convert.ToString(lvwRecovery.FocusedItem.Tag)


                        m_cTaxAmount = frmSalvageDetails.TaxAmount
                        m_sTaxTypeDesc = frmSalvageDetails.TaxTypeDesc
                        m_sTaxBandDesc = frmSalvageDetails.TaxBandDesc
                        m_sTaxTypeCode = frmSalvageDetails.TaxTypeCode



                        m_lReturn = g_oBusiness.EditUpdate(nCollectionObject, vRecoveryTypeId:=m_lRecoveryTypeID, vPerilId:=m_lPerilId, vCurrencyID:=m_lLossCurrencyId, vRevisedReserve:=m_cRevisedReserve, vTable:=ACRecovery, vTaxAmount:=m_cTaxAmount)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                        m_lReturn = EditToListView(m_lSelectedIndex)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                    End If

                    frmSalvageDetails.Close()

                Case gPMConstants.PMEComponentAction.PMView

                    'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 1).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(1).Text.Trim() = Nothing Then
                        cTempReceiptAmount = 0
                    Else
                        cTempReceiptAmount = CDec(lvwRecovery.FocusedItem.SubItems(1).Text.Trim())
                    End If

                    'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 2).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(2).Text.Trim() = Nothing Then
                        cTempInitialReserve = 0
                    Else
                        'cTempInitialReserve = CDec(lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 2).Text.Trim())
                        cTempInitialReserve = CDec(lvwRecovery.FocusedItem.SubItems(2).Text.Trim())
                    End If

                    'RWH(27/06/01)
                    ' If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 3).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(3).Text.Trim() = Nothing Then
                        cTempEditRevisedReserve = 0
                    Else
                        'cTempEditRevisedReserve = CDec(lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 3).Text.Trim())
                        cTempEditRevisedReserve = CDec(lvwRecovery.FocusedItem.SubItems(3).Text.Trim())
                    End If

                    'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 3).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(3).Text.Trim() = Nothing Then
                        cTempNewReserve = 0
                    Else
                        'cTempNewReserve = CDec(lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 4).Text.Trim())
                        cTempNewReserve = CDec(lvwRecovery.FocusedItem.SubItems(4).Text.Trim())
                    End If

                    'If an amount has already been selected for another peril then
                    'don't allow the user to change the receipt currency.
                    bDisableCurrency = False
                    For lLoop As Integer = 1 To lvwRecovery.Items.Count
                        If lvwRecovery.Items.Item(lLoop - 1).Text <> lvwRecovery.FocusedItem.Text Then
                            'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 3).Text.Trim() <> Nothing Then
                            If lvwRecovery.FocusedItem.SubItems(3).Text.Trim() <> Nothing Then
                                'If CDec(lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 3).Text.Trim()) <> 0 Then
                                If CDec(lvwRecovery.FocusedItem.SubItems(3).Text.Trim()) <> 0 Then
                                    bDisableCurrency = True
                                End If
                            End If
                        End If
                    Next

                    frmSalvageDetails.ReceiptAmount = cTempReceiptAmount
                    frmSalvageDetails.InitialReserve = cTempInitialReserve
                    frmSalvageDetails.NewReserve = cTempNewReserve + cTempReceiptAmount

                    frmSalvageDetails.RecoveryTypeID = CInt(m_vRecoveryTypeID(0, Convert.ToString(lvwRecovery.FocusedItem.Tag)))
                    frmSalvageDetails.ClaimMode = ClaimMode
                    frmSalvageDetails.LossCurrencyId = m_lLossCurrencyId
                    frmSalvageDetails.LossCurrencyDesc = cboCurrency.Text
                    frmSalvageDetails.InsuranceFileCnt = m_lInsuranceFileCnt
                    frmSalvageDetails.DisableCurrency = bDisableCurrency


                    'If lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 6).Text.Trim() = Nothing Then
                    If lvwRecovery.FocusedItem.SubItems(6).Text.Trim() = Nothing Then
                        frmSalvageDetails.TaxAmount = 0
                    Else
                        ' frmSalvageDetails.TaxAmount = CDec(lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 6).Text)
                        frmSalvageDetails.TaxAmount = CDec(lvwRecovery.FocusedItem.SubItems(6).Text)
                    End If
                    'frmSalvageDetails.TaxTypeDesc = lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 7).Text
                    frmSalvageDetails.TaxTypeDesc = lvwRecovery.FocusedItem.SubItems(7).Text
                    ' frmSalvageDetails.TaxBandDesc = lvwRecovery.listViewHelper1.GetListViewSubItem(lvwRecovery.FocusedItem, 8).Text
                    frmSalvageDetails.TaxBandDesc = lvwRecovery.FocusedItem.SubItems(8).Text


                    Dim tempLoadForm3 As frmSalvageDetails = frmSalvageDetails


                    frmSalvageDetails.FillTaxTypeCombo(g_oBusiness)


                    frmSalvageDetails.ShowDialog()

                    If frmSalvageDetails.Status = gPMConstants.PMEReturnCode.PMOK Then

                        m_cInitialReserve = frmSalvageDetails.InitialReserve
                        m_cNewreserve = frmSalvageDetails.NewReserve
                        m_cRevisedReserve = cTempEditRevisedReserve
                        m_lRecoveryTypeID = frmSalvageDetails.RecoveryTypeID
                        m_cReceiptAmount = frmSalvageDetails.ReceiptAmount
                        m_lReceiptCurrencyId = frmSalvageDetails.ReceiptCurrencyId
                        m_dReceiptToLossRate = frmSalvageDetails.ReceiptToLossRate
                        m_cReceiptAmountLoss = frmSalvageDetails.ReceiptAmountLoss


                        nCollectionObject = Convert.ToString(lvwRecovery.FocusedItem.Tag)

                        m_cTaxAmount = frmSalvageDetails.TaxAmount
                        m_sTaxTypeDesc = frmSalvageDetails.TaxTypeDesc
                        m_sTaxBandDesc = frmSalvageDetails.TaxBandDesc
                        m_sTaxTypeCode = frmSalvageDetails.TaxTypeCode


                        'RWH(26/06/1) Update vReceivedToDate in cmdOK.

                        m_lReturn = g_oBusiness.EditUpdate(nCollectionObject, vRecoveryTypeId:=m_lRecoveryTypeID, vPerilId:=m_lPerilId, vCurrencyID:=m_lLossCurrencyId, vTable:=ACRecovery, vTaxAmount:=m_cTaxAmount)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                        'RWH(27/06/01)

                        m_vRecdToDate(ACIRecoveryID, Convert.ToString(lvwRecovery.FocusedItem.Tag)) = m_cReceiptAmountLoss

                        m_lReturn = EditToListView(m_lSelectedIndex)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                    End If

                    frmSalvageDetails.Close()

            End Select

        End If

        m_lReturn = DisableInterface(True)

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

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMSalvageRecovery.General()


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            'Set m_oFormFields = New iPMFormControl.FormFields

            ' Set language
            'm_oFormFields.LanguageID = g_iLanguageID%


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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMSalvageRecovery.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            Dim sMessage, sTitle As String


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                'Initialise = PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If


            '
            '    'Validate fields using Forms Control
            '    'm_lReturn& = SetFieldValidation()
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Set the mouse pointer to normal.
            '        SetMousePointer PMMouseNormal
            '
            '        Exit Sub
            '    End If

            'JMK 14/11/2001 - get hidden option (UW Only)


            m_sUnderwritingType = g_oBusiness.UnderwritingType


            'Set the interface default values.
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
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If



            Dim vCurrencyID As Object


            m_lReturn = g_oBusiness.GetDefaultCurrencyID(vCurrencyID, m_lClaimID)



            m_lLossCurrencyId = CInt(vCurrencyID(0, 0))


            If CStr(vCurrencyID(1, 0)) <> "" Then


                m_lDefaultCoinsuranceTreatment = CInt(vCurrencyID(1, 0))

            End If

            'Set Default Currency id equal to policy currency id


            For ncount As Integer = 0 To cboCurrency.Items.Count - 1

                If VB6.GetItemData(cboCurrency, ncount) = m_lLossCurrencyId Then

                    cboCurrency.SelectedIndex = ncount

                    Exit For

                End If

            Next ncount


            If CStr(vCurrencyID(1, 0)) <> "" Then

                If ClaimMode = gPMConstants.PMEComponentAction.PMView Then

                    If cboCoinsuranceTreatment.Items.Count > 0 Then

                        For ncount As Integer = 0 To cboCoinsuranceTreatment.Items.Count - 1

                            If VB6.GetItemData(cboCoinsuranceTreatment, ncount) = m_lDefaultCoinsuranceTreatment Then

                                cboCoinsuranceTreatment.SelectedIndex = ncount

                                Exit For

                            End If

                        Next ncount

                    End If

                End If

            End If

            txtExchangeRate.Enabled = False

            txtExchangeRate.BackColor = SystemColors.Control


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
    Private Const vbFormCode As Integer = 0
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
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = ResizeInterface()

        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwRecovery_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRecovery.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'TN20010328    If ClaimMode <> PMView Then
        'RWH(06/04/2001) Just set record selected flag here.
        m_bRecordSelected = Not (Me.lvwRecovery.GetItemAt(x, y) Is Nothing)
        'TN20010328    End If
    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click
        cmdCancel_Click(cmdCancel, New EventArgs())
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
        '
        '    On Error GoTo Err_tabMainTabClick
        '
        '    With tabMainTab
        '        ' Set the default button.
        ''        If (.Tab < cmdNext.Count) Then
        ''            cmdNext(.Tab).Default = True
        ''        Else
        ''            cmdOK.Default = True
        ''        End If
        '
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        ''        DoEvents
        '
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With

        'TN20010906 - start - its better here than in the lvwRecovery click
        If lvwRecovery.Items.Count > 0 Then
            If lvwRecovery.FocusedItem Is Nothing Then
                lvwRecovery.Items.Item(0).Selected = True
            End If
            PopulateCoReInsurerOnSelection()
        End If
        'TN20010906 - end


        Exit Sub




        Exit Sub

        tabMainTabPreviousTab = tabMainTab.SelectedIndex
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

        ' Click event of the OK button.

        Try

            Const ACOptionValueSuspense As Integer = 1
            Const ACOptionNumber As Integer = 2002
            Const ACAmountAgainst As String = "Amount Against "

            Dim nOptionValue As Integer
            Dim lPartyid As Integer
            Dim sComments As String = ""
            Dim lButtonClicked As gPMConstants.PMEReturnCode
            Dim cReceiptAmount As Decimal
            Dim sCommentsOut As String = ""

            'TN20010329 Start
            Dim sShortName As String = ""
            Dim lCOBId As Integer
            Dim sCOBCode As String = ""
            'TN20010329 End

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            Dim bForceLostFocus As Boolean

            Dim nCollectionObject As Integer
            'RWH(28/06/01)
            Dim sAccountType As String = ""
            Dim lProcessCommand As gPMConstants.PMEReturnCode


            bForceLostFocus = iPMFunc.ForceLostFocus(cmdOK)
            'DoEvents
            If Not bForceLostFocus Then
                txtExchangeRate.Focus()
                Exit Sub
            End If

            'RWH(26/06/01) EditUpdate every modification could give cumulative errors for
            'Received_to_Date.
            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMView
                    For iCount As Integer = 1 To lvwRecovery.Items.Count


                        nCollectionObject = Convert.ToString(lvwRecovery.Items.Item(iCount - 1).Tag)

                        m_lReturn = g_oBusiness.EditUpdate(nCollectionObject, vReceivedToDate:=CDec(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nCollectionObject - 1), 1).Text.Trim()))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                    Next iCount
            End Select
            m_oGeneral.ClaimId = m_lClaimID
            'This has to be done first as some of the values saved will be used when posting.
            lProcessCommand = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            If lProcessCommand <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function ProcessCommand failed.")
            End If

            If ClaimMode = gPMConstants.PMEComponentAction.PMView Or m_sTransactionType.Trim() = ACTransTypeClaimSalvage Then

                m_lReturn = GetComments(sCommentsOut)

                sCommentsOut = ACAmountAgainst & sCommentsOut


                m_lReturn = g_oBusiness.GetOption(ACOptionNumber, nOptionValue)

                'get class of business

                m_lReturn = g_oBusiness.GetClassOfBusinss(v_lPerilTypeID:=m_lPerilTypeID, r_lId:=lCOBId, r_sCode:=sCOBCode)

                m_lReturn = GetReceiptAmount(cReceiptAmount)

                If cReceiptAmount <> 0 Then

                    If nOptionValue = ACOptionValueSuspense Then

                        lPartyid = 0

                        If ClaimMode = gPMConstants.PMEComponentAction.PMView Then

                            m_lReturn = AddPaymentDetails(sCommentsOut)

                            m_lReturn = AddReceiptDetails(lPartyid, sCommentsOut)

                            'post to orion - debit claim suspense and credit claim expense
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                If sCOBCode <> "" Then
                                    'RWH(28/06/01) Correct AccountCode & MappingCode.
                                    m_lReturn = PostSalvageToOrion(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimID:=m_lClaimID, v_lPerilID:=m_lPerilId, v_cTransAmount:=cReceiptAmount, v_sDebitAccountCode:="CLAIMSUS", v_sDebitMappingCode:="CLMSUS" & sCOBCode, v_lCOBId:=lCOBId, v_sCOBCode:=sCOBCode)
                                Else
                                    ' Log Error.
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get class of business." & Constants.vbLf & "Salvage transactions will not be posted to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                End If
                            End If
                            'TN20010329

                        End If

                    Else
                        'If nOptionValue <> ACOptionValueSuspense Then

                        'TN20010328 m_lReturn = GetReceiptAmount(cReceiptAmount)

                        If ClaimMode = gPMConstants.PMEComponentAction.PMView Then

                            m_lReturn = AddPaymentDetails(sComments)

                            'Adds most of the receipt details
                            m_lReturn = AddReceiptDetails(lPartyid, sComments)

                            'As well as other things, updates the receipt with overridden rates.
                            m_lReturn = GetReceiptPartyid(lPartyid, cReceiptAmount, sComments, lButtonClicked, sCommentsOut)

                            'Updates the receipt with the party id and comments
                            m_lReturn = UpdateReceiptDetails(lPartyid, sComments)

                            If lButtonClicked = gPMConstants.PMEReturnCode.PMOK Then

                                'RWH(28/06/01) Check party selected and set default if necessary.
                                If lPartyid <> 0 Then
                                    'TN20010329 Start

                                    m_lReturn = g_oBusiness.GetPartyName(v_lPartyCnt:=lPartyid, v_sFieldName:="shortname", r_sResult:=sShortName)

                                    sAccountType = ""
                                Else
                                    sAccountType = "CLAIMREC"
                                    sShortName = "CLMRECEIVABLE"

                                End If


                                'post to orion - debit punter and credit claim expenses
                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    If sCOBCode <> "" Then
                                        m_lReturn = PostSalvageToOrion(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimID:=m_lClaimID, v_lPerilID:=m_lPerilId, v_cTransAmount:=cReceiptAmount, v_sDebitAccountCode:=sAccountType, v_sDebitMappingCode:=sShortName, v_lCOBId:=lCOBId, v_sCOBCode:=sCOBCode, v_lPartyCnt:=lPartyid)
                                    Else
                                        ' Log Error.
                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get class of business." & Constants.vbLf & "Salvage transactions will not be posted to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    End If
                                Else
                                    ' Log Error.
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party name." & Constants.vbLf & "Salvage transaction will not be posted to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                End If
                                'TN20010329 End
                            End If
                        Else
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                        End If


                    End If
                End If
            End If



            'RWH(05/10/01) Check to see if claim can be closed and close it if required.
            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (m_sTransactionType = "C_SA") Then
                CheckCurrentReserve()
            End If

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




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    'Private Sub lvwSearchDetails_GotFocus()
    '
    ' GotFocus Event for the search details
    '

    'Try 
    '
    '    ' Unset any default buttons so can select with Enter key.
    '    cmdFindNow.Default = False
    '    cmdOK.Default = False
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    '
    'End Sub
    ' ***************************************************************** '
    ' Name: lvwSearchDetails_lostfocus
    '
    ' Description:Set find now as default
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    'Private Sub lvwSearchDetails_LostFocus()
    '
    ' LostFocus Event for the search details
    '

    'Try 
    '
    ' Set the default button.
    'cmdFindNow.Default = True
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

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

    Private Sub lvwRecovery_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRecovery.Click

        If (lvwRecovery.Items.Count > 0) And (m_bRecordSelected) Then

            'RWH(06/04/2001) #630 Ensure recovery types can be added and
            'edited, but deleted only if mode NOT maintain or payment.

            'AJM 25/04/01 if claim mode PMView diable add button
            If ClaimMode <> gPMConstants.PMEComponentAction.PMView Then
                If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    cmdAdd.Enabled = True
                End If
            End If

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEdit.Enabled = True
            End If

            If ClaimMode = gPMConstants.PMEComponentAction.PMAdd Then
                cmdDelete.Enabled = True
            End If

            m_lSelectedIndex = lvwRecovery.FocusedItem.Index + 1

            'TN20010906 move to tab click its better there
            'PopulateCoReInsurerOnSelection

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
    Private Sub lvwRecovery_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRecovery.DoubleClick

        ' Double click event for the search details.



        'Try 
        '
        '*****Commented By Pandu 18-10-2000 As Double clicking should call
        '*****Salvage Details Screen in Edit Mode
        ''*****Start of Code change Internal Bug 8
        '
        '
        '
        '    ' Check if there are any items available.
        '    If (lvwRecovery.ListItems.Count = 0) Then
        '        Exit Sub
        '    End If
        ''
        '    ' Set the interface status.
        '    m_lStatus& = PMOK
        ''
        '    ' Process the next set of actions.
        '    m_lReturn& = m_oGeneral.ProcessCommand()
        ''
        '    ' Check the return value.
        '    If (m_lReturn& = PMTrue) Then
        '        ' Everything OK, so we can hide the interface.
        '        Me.Hide
        '    End If
        '*****end of Code change Internal Bug 8
        '
        '******************End of Change in Comments
        '
        '*********Internal Bug 8  -Start of Change For Calling Salvage Details Screen 18-10-2000 Pandu
        '
        '    Call cmdEdit_Click
        '
        '*********Internal Bug 8 -End of Change  For Calling Salvage Details Screen 18-10-2000 Pandu
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub
    '' ***************************************************************** '
    '' Name:lvwSearchDetails_KeyDown
    ''
    '' Description:Set Command Button Ok as Not Default on Pressing Enter Key
    ''
    '' Date:11/07/00
    ''
    '' Edit History:Pandu
    '' ***************************************************************** '
    'Private Sub lvwRecovery(KeyCode As Integer, Shift As Integer)
    '
    '    If (KeyCode <> 13) Then
    '        cmdOK.Default = False
    '    End If
    '
    'End Sub

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

    'Private Sub lvwRecovery(KeyAscii As Integer)
    '
    'Dim sindex As Integer
    '
    '    If (KeyAscii = 13) Then
    '        If (lvwsearchdetails.ListItems.Count > 0) Then
    '            sindex = lvwsearchdetails.SelectedItem.Tag
    '
    '            txtClaimRef.Text = Trim(m_vSearchData(ACIClaimRef, sindex))
    '            txtPolicy.Text = Trim(m_vSearchData(ACIInsuranceRef, sindex))
    '
    '            If m_lSiriusUnderWritingBroking = ACUnderWriting Then
    '
    '                txtPolicyHolder.Text = Trim(m_vSearchData(ACIUPolicyHolder, sindex))
    '
    '            Else
    '
    '                txtPolicyHolder.Text = Trim(m_vSearchData(ACIBPolicyHolder, sindex))
    '
    '            End If
    '
    '
    '            cmdOK.Default = True
    '        End If
    '    End If
    '
    'End Sub
    ' ***************************************************************** '
    ' Name: lvwSearchDetails_ColumnClick
    '
    ' Description:Sort the Details of List View as per the column clicked
    '
    ' Date:11/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    'Private Sub lvwSearchDetails_ColumnClick(ByVal ColumnHeader As ColumnHeader)
    '
    '    ' Column click event for the search details
    '
    '    On Error GoTo Err_lvwSearchDetailsColumnClick
    '
    '    With lvwsearchdetails
    '
    '        ' If date column clicked, then sort by date sort column
    '        If (ColumnHeader.Index - 1 = 4) Then
    '            .Sorted = False
    '            If (.SortKey <> 5) Then
    '                .SortKey = 5
    '                .SortOrder = 0
    '            Else
    '                .SortOrder = (.SortOrder + 1) Mod 2
    '            End If
    '            .Sorted = True
    '
    '            ' If current sort column header is
    '            ' pressed.
    '        ElseIf (ColumnHeader.Index - 1 = .SortKey) Then
    '            ' Set sort order opposite of
    '            ' current direction.
    '            .SortOrder = (.SortOrder + 1) Mod 2
    '        Else
    '            ' Sort by this column (ascending).
    '            .Sorted = False
    '
    '            ' Turn off sorting so that the list
    '            ' is not sorted twice
    '            .SortOrder = 0
    '            .SortKey = ColumnHeader.Index - 1
    '            .Sorted = True
    '        End If
    '    End With
    '
    '    Exit Sub
    '
    '
    'Err_lvwSearchDetailsColumnClick:
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to sort the column", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="lvwSearchDetails_ColumnClick", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub


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



            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString))


            ' Display the status message.

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


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

            'Currency
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Exchange Rate
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExchangeRate, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

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
            Dim sRecoveryTypeDesc As String = ""
            Dim oListItem As ListViewItem
            Dim cNewReserve As Decimal

            'Const ACFindImage As String = "FindImage"

            Select Case ClaimMode
                'RWH(09/04/2001) Get details for Add mode as well.
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd


                    If g_oBusiness.RecordCount > 0 Then

                        cmdOK.Enabled = True

                        'PoPulate Recovery ListView

                        For ncount As Integer = 1 To g_oBusiness.RecordCount

                            ' Assign the details from the business object
                            ' to the data storage.
                            m_lReturn = BusinessToData()

                            ' Check for errors
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to assign the data.
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Assign the details to the interface.



                            m_lReturn = GetRecoveryTypeDescription(m_lRecoveryTypeID, sRecoveryTypeDesc, g_vSalvageRecoveryTypes, g_vSalvageRecoveryTypes.GetLowerBound(1), g_vSalvageRecoveryTypes.GetUpperBound(0) + 1)

                            ' Check for errors
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to assign the data.
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            oListItem = lvwRecovery.Items.Add(sRecoveryTypeDesc.Trim(), "")

                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve).Trim())

                            If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Then

                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve).Trim())

                                cNewReserve = CDec(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve + m_cInitialReserve - m_cReceivedTodate).Trim()))

                                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cNewReserve).Trim())

                                'TN20010906 - received to date
                                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cReceivedTodate))

                            End If


                            oListItem.Tag = g_oBusiness.CurrentRecord

                            ReDim Preserve m_vRecdToDate(ACIRecoveryID, Convert.ToString(oListItem.Tag))

                            ReDim Preserve m_vRecoveryID(ACIRecoveryID, Convert.ToString(oListItem.Tag))

                            ReDim Preserve m_vRecoveryTypeID(ACIRecoveryID, Convert.ToString(oListItem.Tag))

                            'Store the recoveryid against the collection object

                            m_vRecoveryID(ACIRecoveryID, Convert.ToString(oListItem.Tag)) = m_lRecoveryId


                            m_vRecoveryTypeID(ACIRecoveryID, Convert.ToString(oListItem.Tag)) = m_lRecoveryTypeID


                            m_vRecdToDate(ACIRecoveryID, Convert.ToString(oListItem.Tag)) = m_cReceivedTodate

                            ' Refresh the first X amount of rows, to
                            ' allow the user to see the results instantly.
                            If ncount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                                ' Select the first item.
                                lvwRecovery.Items.Item(0).Selected = True

                                ' Refresh the initial results.
                                lvwRecovery.Refresh()
                            End If

                        Next ncount

                        ' Select the first item.
                        lvwRecovery.Items.Item(0).Selected = True

                    End If

                Case gPMConstants.PMEComponentAction.PMView


                    If g_oBusiness.RecordCount > 0 Then

                        cmdOK.Enabled = True

                        'PoPulate Recovery ListView

                        For ncount As Integer = 1 To g_oBusiness.RecordCount

                            ' Assign the details from the business object
                            ' to the data storage.
                            m_lReturn = BusinessToData()

                            ' Check for errors
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to assign the data.
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' Assign the details to the interface.



                            m_lReturn = GetRecoveryTypeDescription(m_lRecoveryTypeID, sRecoveryTypeDesc, g_vSalvageRecoveryTypes, g_vSalvageRecoveryTypes.GetLowerBound(0), g_vSalvageRecoveryTypes.GetUpperBound(0) + 1)


                            ' Check for errors
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to assign the data.
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                            oListItem = lvwRecovery.Items.Add(sRecoveryTypeDesc.Trim(), "")

                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(0))


                            'RWH(25/06/01) Show column values including Initial Reserve.
                            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve).Trim())

                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve).Trim())

                            'It Should be addition of intial+revised-recdtodate
                            cNewReserve = CDec(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve + m_cInitialReserve - m_cReceivedTodate).Trim()))

                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cNewReserve).Trim())

                            '             oListItem.SubItems(1) = FormatField( _
                            ''                                    iFormatType:=PMFormatCurrency, _
                            ''                                    vFieldValue:=0)
                            '
                            '             'It Should be addition of intial+revised-recdtodate
                            '             oListItem.SubItems(2) = FormatField( _
                            ''                                     iFormatType:=PMFormatCurrency, _
                            ''                                     vFieldValue:=Trim$(m_cRevisedReserve + m_cInitialReserve - m_cReceivedTodate))
                            '
                            '              oListItem.SubItems(3) = FormatField( _
                            ''                                      iFormatType:=PMFormatCurrency, _
                            ''                                      vFieldValue:=0)

                            'TN20010906 - received to date
                            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cReceivedTodate))



                            oListItem.Tag = g_oBusiness.CurrentRecord

                            'oListItem.Key = m_lRecoveryTypeID

                            ReDim Preserve m_vRecoveryID(ACIRecoveryID, Convert.ToString(oListItem.Tag))


                            m_vRecoveryID(ACIRecoveryID, Convert.ToString(oListItem.Tag)) = m_lRecoveryId

                            ReDim Preserve m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(oListItem.Tag))


                            m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(oListItem.Tag)) = m_lRecoveryTypeID

                            ReDim Preserve m_vRecdToDate(ACIRecoveryID, Convert.ToString(oListItem.Tag))

                            ' Refresh the first X amount of rows, to
                            ' allow the user to see the results instantly.
                            If ncount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                                ' Select the first item.
                                lvwRecovery.Items.Item(0).Selected = True

                                ' Refresh the initial results.
                                lvwRecovery.Refresh()
                            End If

                        Next ncount

                        ' Select the first item.
                        lvwRecovery.Items.Item(0).Selected = True

                    End If
            End Select


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


            m_lReturn = g_oBusiness.Update


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
    '    ReDim vTableArray(4, 1)
    '
    '    'vTableArray(0, 0) = "Recovery_Type"
    '    vTableArray(0, 0) = "Currency"
    '    vTableArray(0, 1) = "Coinsurance_Treatment"
    '
    '    'vTableArray(1, 0) = ""
    '    vTableArray(1, 0) = ""
    '    vTableArray(1, 1) = ""
    '
    '
    '    m_lReturn& = g_oBusiness.GetLookupValues(PMLookupAll, vTableArray, g_iLanguageID, g_vLookupArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn& = g_oBusiness.GetSalvageRecoveryType(g_vSalvageRecoveryTypes)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    Select Case ClaimMode
    '
    '        Case PMEdit, PMAdd
    '
    '            Call LoadDataInCombo(cboCurrency, g_vLookupArray, vTableArray(2, 0), vTableArray(3, 0))
    '
    '        Case PMView
    '
    '            Call LoadDataInCombo(cboCurrency, g_vLookupArray, vTableArray(2, 0), vTableArray(3, 0))
    '
    '            Call LoadDataInCombo(cboCoinsuranceTreatment, g_vLookupArray, vTableArray(2, 1), vTableArray(3, 1))
    '
    '    End Select
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

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer
        Dim result As Integer = 0
        Dim lCurrency As Integer 'Always the same as loss currency

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd


                    m_lReturn = g_oBusiness.GetNext(vRecoveryId:=m_lRecoveryId, vRecoveryTypeId:=m_lRecoveryTypeID, vCurrencyID:=lCurrency, vInitialReserve:=m_cInitialReserve, vRevisedReserve:=m_cRevisedReserve, vReceivedToDate:=m_cReceivedTodate)


                Case gPMConstants.PMEComponentAction.PMView


                    m_lReturn = g_oBusiness.GetNext(vRecoveryId:=m_lRecoveryId, vRecoveryTypeId:=m_lRecoveryTypeID, vCurrencyID:=lCurrency, vInitialReserve:=m_cInitialReserve, vRevisedReserve:=m_cRevisedReserve, vReceivedToDate:=m_cReceivedTodate)

            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

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





            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRecoveryTypeDescription(ByRef lRecoveryTypeId As Integer, ByRef sRecoveryTypeDescription As String, ByVal vntData(,) As Object, ByVal vnStart As Integer, ByVal vnCount As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue




        'Check whether an array has been passed
        If Information.IsArray(vntData) Then


            'Load the data from the Array to the combobox
            For lCount As Integer = vnStart To vnStart + vnCount - 1



                If CDbl(vntData(0, lCount)) = lRecoveryTypeId Then


                    sRecoveryTypeDescription = CStr(vntData(1, lCount))

                    Exit For

                End If


            Next lCount

        End If



        Return result
    End Function
    ' ***************************************************************** '
    ' Name: LoadDataInCombo
    '
    ' Description: Fills the data from variant array into combobox
    '               INPUTS : Combo Control to be filled
    '                       2D - Array Containing the Record values
    '                       Index in the Array where the Records of the
    '                           Table Start from
    '                       Number of records to enter
    ' ***************************************************************** '

    'Private Function LoadDataInCombo(ByRef cboControl As ComboBox, ByVal vntData( ,  ) As Object, ByVal vnStart As Integer, ByVal vnCount As Integer) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Check whether an array has been passed
    'If Information.IsArray(vntData) Then
    '
    'clear the combobox
    'cboControl.Items.Clear()
    '
    'Load the data from the Array to the combobox
    'For 'lCount As Integer = vnStart To vnStart + vnCount - 1
    '
    'Dim cboControl_NewIndex As Integer = -1

    'cboControl_NewIndex = cboControl.Items.Add(CStr(vntData(1, lCount)))

    'VB6.SetItemData(cboControl, cboControl_NewIndex, CInt(vntData(0, lCount)))
    '
    'Next lCount
    '
    'End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load data in combobox", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDataInCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '

    'Private Function CheckMandatory() As Boolean
    '
    'Dim result As Boolean = False
    'Try 
    '
    '
    '
    '
    'If cboCurrency.Text = "" Then ' RecoveryType Combo box
    'DisplayMessage(ACMandatoryFieldMsg, Mid(lblCurrency.Name, 4))
    'Return False
    'Else
    '   If all the Mandatory fields are having values SET the CheckMandatory = True
    'Return True
    'End If
    '
    'Catch 
    'End Try
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for Mandatory Fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End Function


    Public Function AddToListView() As Integer
        Dim result As Integer = 0
        'Const ACFindImage As String = "FindImage"

        Dim sRecoveryTypeDesc As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        'Assign the details to the interface.



        m_lReturn = GetRecoveryTypeDescription(m_lRecoveryTypeID, sRecoveryTypeDesc, g_vSalvageRecoveryTypes, g_vSalvageRecoveryTypes.GetLowerBound(1), g_vSalvageRecoveryTypes.GetUpperBound(0) + 1)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim oListItem As ListViewItem = lvwRecovery.Items.Add(sRecoveryTypeDesc.Trim(), "")

        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve).Trim())


        If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Then

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve).Trim())

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cNewreserve).Trim())
        End If

        oListItem.Tag = CStr(g_lCount)

        ReDim Preserve m_vRecoveryTypeID(ACIRecoveryID, Convert.ToString(oListItem.Tag))

        m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(oListItem.Tag)) = m_lRecoveryTypeID

        'RWH(09/04/2001) Update local array for Amount received so far
        'so we can edit if we wish.
        ReDim Preserve m_vRecdToDate(ACIRecoveryID, Convert.ToString(oListItem.Tag))

        m_vRecdToDate(ACIRecoveryID, g_lCount) = m_cReceivedTodate

        cmdOK.Enabled = True

        Return result
    End Function

    Public Function PopulateCoReInsurer() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim cReceiptAmount As Decimal
        Dim cAmountAfterCoinsurers, cSalvageAmount, cShare As Decimal
        Const ACFirstItem As Integer = 1

        'Call ClearCoReInsurer

        If ClaimMode = gPMConstants.PMEComponentAction.PMView Then

            For nRecCount As Integer = 1 To lvwRecovery.Items.Count

                If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nRecCount - 1), 1).Text) <> 0 Then


                    cReceiptAmount = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=CDec(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nRecCount - 1), 1).Text.Trim())))

                    cAmountAfterCoinsurers = cReceiptAmount


                    If lvwCoInsurance.Items.Count > 0 Then

                        For ncount As Integer = ACFirstItem To lvwCoInsurance.Items.Count


                            cShare = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatPercent, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=ListViewHelper.GetListViewSubItem(lvwCoInsurance.Items.Item(ncount - 1), 1).Text))

                            cSalvageAmount = cShare * cReceiptAmount / 100

                            'RWH(26/06/01)
                            '                If (ClaimMode = PMEdit) Then
                            '                    lvwCoInsurance.ListItems(ncount).SubItems(2) = FormatField( _
                            ''                                                                   iFormatType:=PMFormatCurrency, _
                            ''                                                                   vFieldValue:=(UnFormatField( _
                            ''                                                                                iFormatTypein:=PMFormatCurrency, _
                            ''                                                                                iDataTypeout:=PMCurrency, _
                            ''                                                                                vFieldValue:=lvwCoInsurance.ListItems(ncount).SubItems(2)) + cSalvageAmount))
                            '                Else
                            ListViewHelper.GetListViewSubItem(lvwCoInsurance.Items.Item(ncount - 1), 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cSalvageAmount))
                            '                End If
                            cAmountAfterCoinsurers -= cSalvageAmount

                        Next

                    End If

                    If lvwReinsurance.Items.Count > 0 Then

                        For ncount As Integer = ACFirstItem To lvwReinsurance.Items.Count


                            cShare = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatPercent, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=ListViewHelper.GetListViewSubItem(lvwReinsurance.Items.Item(ncount - 1), 1).Text))

                            cSalvageAmount = cShare * cAmountAfterCoinsurers / 100

                            'RWH(26/06/01)
                            '                If (ClaimMode = PMEdit) Then
                            '                    lvwReinsurance.ListItems(ncount).SubItems(2) = FormatField( _
                            ''                                                                    iFormatType:=PMFormatCurrency, _
                            ''                                                                    vFieldValue:=(UnFormatField( _
                            ''                                                                                iFormatTypein:=PMFormatCurrency, _
                            ''                                                                                iDataTypeout:=PMCurrency, _
                            ''                                                                                vFieldValue:=lvwReinsurance.ListItems(ncount).SubItems(2)) + cSalvageAmount))
                            '                Else
                            ListViewHelper.GetListViewSubItem(lvwReinsurance.Items.Item(ncount - 1), 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cSalvageAmount))
                            '                End If
                        Next

                    End If

                End If

            Next nRecCount


        End If


        Return result
    End Function

    'Public Function UpdateReceiptDetails() As Long
    '
    'UpdateReceiptDetails = PMTrue
    'Dim sTitle As String
    'Dim sMessage As String
    'Dim iMsgResult As Integer
    'Select Case ClaimMode
    '
    'Case PMView
    '
    '             ' Check if form has been cancelled, if so,
    '            ' check if the details have changed and if
    '            ' so, prompt if they wish to cancel.
    '            If (Status = PMCancel) Then
    '
    '
    '                    sTitle$ = iPMFunc.GetResData( _
    ''                        iLangID:=g_iLanguageID%, _
    ''                        lID:=ACCancelDetailsTitle, _
    ''                        iDataType:=PMResString)
    '
    '                    sMessage$ = iPMFunc.GetResData( _
    ''                        iLangID:=g_iLanguageID%, _
    ''                        lID:=ACCancelDetails, _
    ''                        iDataType:=PMResString)
    '
    '                    iMsgResult = MsgBox(sMessage$, _
    ''                    vbYesNo + vbDefaultButton2 + vbQuestion, sTitle$)
    '
    '                    ' Check message result.
    '                    If (iMsgResult = vbNo) Then
    '                        ' Set return to false, meaning
    '                        ' don't cancel.
    '                        UpdateReceiptDetails = PMFalse
    '                    End If
    ''                End If
    '            Else
    '                'Call Payment Method Screen
    '
    '                Dim m_lReceiptID As Long
    '                Dim cReceiptAmount As Currency
    '
    '                m_lReturn = GetTotalSalvageAmount(cReceiptAmount)
    '
    '                 ' Check for errors.
    '                If (m_lReturn& <> PMTrue) Then
    '                   ' Failed to update the details
    '                   UpdateReceiptDetails = PMFalse
    '
    '                   Exit Function
    '
    '                End If
    '
    '                m_lReturn = GetReceiptDetails(m_lReceiptID, cReceiptAmount)
    '
    '                ' Check for errors.
    '                If (m_lReturn& <> PMTrue) Then
    '                   ' Failed to update the details
    '                   UpdateReceiptDetails = PMFalse
    '
    '                   Exit Function
    '
    '                End If
    '
    '                m_lReturn = AddReceiptDetails(m_lPartyClaimID)
    '
    '                ' Check for errors.
    '                If (m_lReturn& <> PMTrue) Then
    '                   ' Failed to update the details
    '                   UpdateReceiptDetails = PMFalse
    '
    '                   Exit Function
    '
    '                End If
    '
    '                ' Update the details using the business object.
    '                m_lReturn& = g_oBusiness.Update()
    '
    '                ' Check for errors.
    '                If (m_lReturn& <> PMTrue) Then
    '                   ' Failed to update the details
    '                   UpdateReceiptDetails = PMFalse
    '
    '                   ' Log Error.
    '                   LogMessage _
    ''                       iType:=PMLogError, _
    ''                       sMsg:="Failed to update the details", _
    ''                       vApp:=ACApp, _
    ''                       vClass:=ACClass, _
    ''                       vMethod:="ProcessCommand"
    '                End If
    '            End If
    'End Select
    '
    '



    'End Function

    Public Function GetReceiptDetails(ByRef lReceiptId As Integer, ByRef cReceiptAmount As Decimal) As Integer


    End Function

    Public Function GetTotalSalvageAmount(ByRef cReceiptAmount As Decimal) As Integer


        Const nFirstItem As Integer = 1
        If lvwRecovery.Items.Count > 0 Then



            For ncount As Integer = nFirstItem To lvwRecovery.Items.Count

                cReceiptAmount += CDbl(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(ncount - 1), 1).Text)

            Next ncount

        End If


    End Function

    Public Function AddReceiptDetails(ByRef lPartyClaimId As Integer, ByRef sComments As String) As Integer
        Dim result As Integer = 0
        Dim lRecoveryId, lRecoveryTypeId As Integer
        Dim cReceiptAmount As Decimal

        result = gPMConstants.PMEReturnCode.PMTrue

        If lvwRecovery.Items.Count > 0 Then

            For ncount As Integer = 1 To lvwRecovery.Items.Count

                If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(ncount - 1), 9).Text) <> 0 And ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(ncount - 1), 9).Text.Trim() <> Nothing Then


                    lRecoveryId = CInt(m_vRecoveryID(ACIRecoveryID, Convert.ToString(lvwRecovery.Items.Item(ncount - 1).Tag)))


                    lRecoveryTypeId = CInt(m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(lvwRecovery.Items.Item(ncount - 1).Tag)))


                    cReceiptAmount = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=CDec(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(ncount - 1), 9).Text.Trim())))

                    g_lCount += 1


                    m_lReturn = g_oBusiness.EditAdd(g_lCount, vTable:=ACReceipt, vRecoveryId:=lRecoveryId, vClaimId:=m_lClaimID, vRecoveryTypeId:=lRecoveryTypeId, vPerilId:=m_lPerilId, vPartyClaimID:=lPartyClaimId, vCurrencyID:=m_lReceiptCurrencyId, vComments:=sComments, vReceiptAmount:=cReceiptAmount, vDateofReceipt:=DateTime.Today, vTaxAmount:=m_cTaxAmount, vReceiptToLossRate:=m_dReceiptToLossRate)


                    m_lReturn = g_oBusiness.Update
                End If

            Next

        End If

        Return result
    End Function

    Public Function UpdateReceiptDetails(ByRef lPartyClaimId As Integer, ByRef sComments As String) As Integer

        Dim result As Integer = 0
        Dim lTable As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lCurrentRecord As Integer = 0

        g_oBusiness.CurrentRecord = lCurrentRecord


        m_lReturn = g_oBusiness.GetNext(vTable:=lTable)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.GetNext failed.")
        End If


        Do While g_oBusiness.CurrentRecord <> lCurrentRecord

            lCurrentRecord = g_oBusiness.CurrentRecord

            If lTable = ACReceipt Then

                m_lReturn = g_oBusiness.EditUpdate(lCurrentRecord, vPartyClaimID:=lPartyClaimId, vComments:=sComments)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.EditUpdate failed.")
                End If
            End If


            m_lReturn = g_oBusiness.GetNext(vTable:=lTable)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.GetNext failed.")
            End If
        Loop


        m_lReturn = g_oBusiness.Update

        Return result
    End Function


    Public Function AddPaymentDetails(ByRef sComments As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lPartyClaimId As Integer
        Dim lRecoveryId, lRecoveryTypeId As Integer
        Dim cReceiptAmount, cCoInsurerAmount As Decimal
        Dim lShare As Integer
        Dim cAmountAfterCoinsurers, cReInsurerAmount As Decimal
        If lvwRecovery.Items.Count > 0 Then


            For ncount As Integer = 1 To lvwRecovery.Items.Count

                If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(ncount - 1), 1).Text) <> 0 Then


                    lRecoveryId = CInt(m_vRecoveryID(ACIRecoveryID, Convert.ToString(lvwRecovery.Items.Item(ncount - 1).Tag)))


                    lRecoveryTypeId = CInt(m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(lvwRecovery.Items.Item(ncount - 1).Tag)))


                    cReceiptAmount = CDec(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=CDec(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(ncount - 1), 1).Text.Trim())))

                    cAmountAfterCoinsurers = cReceiptAmount

                    If lvwCoInsurance.Items.Count > 0 Then


                        For nCoInsurerCount As Integer = 1 To lvwCoInsurance.Items.Count


                            lShare = CInt(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatPercent, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=ListViewHelper.GetListViewSubItem(lvwCoInsurance.Items.Item(nCoInsurerCount - 1), 1).Text.Trim()))

                            cCoInsurerAmount = (lShare * cReceiptAmount) / 100

                            g_lCount += 1


                            lPartyClaimId = Convert.ToString(lvwCoInsurance.Items.Item(nCoInsurerCount - 1).Tag)


                            m_lReturn = g_oBusiness.EditAdd(g_lCount, vTable:=ACPayment, vRecoveryId:=lRecoveryId, vClaimId:=m_lClaimID, vRecoveryTypeId:=lRecoveryTypeId, vPerilId:=m_lPerilId, vPartyClaimID:=lPartyClaimId, vCurrencyID:=m_lReceiptCurrencyId, vComments:=sComments, vPaymentAmount:=cCoInsurerAmount, vDateofPayment:=DateTime.Today)

                            cAmountAfterCoinsurers -= cCoInsurerAmount


                        Next nCoInsurerCount

                    End If

                    If lvwReinsurance.Items.Count > 0 Then

                        For nReInsurerCount As Integer = 1 To lvwReinsurance.Items.Count


                            lShare = CInt(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatPercent, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=ListViewHelper.GetListViewSubItem(lvwReinsurance.Items.Item(nReInsurerCount - 1), 1).Text))

                            cReInsurerAmount = (lShare * cAmountAfterCoinsurers) / 100

                            g_lCount += 1


                            lPartyClaimId = Convert.ToString(lvwReinsurance.Items.Item(nReInsurerCount - 1).Tag)


                            m_lReturn = g_oBusiness.EditAdd(g_lCount, vTable:=ACPayment, vRecoveryId:=lRecoveryId, vClaimId:=m_lClaimID, vRecoveryTypeId:=lRecoveryTypeId, vPerilId:=m_lPerilId, vPartyClaimID:=lPartyClaimId, vCurrencyID:=m_lReceiptCurrencyId, vComments:=sComments, vPaymentAmount:=cReInsurerAmount, vDateofPayment:=DateTime.Today)


                        Next nReInsurerCount

                    End If
                End If

            Next ncount

        End If

        Return result
    End Function

    Public Function EditToListView(ByRef lSelectedIndex As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sRecoveryTypeDesc As String = ""

        ' Assign the details to the interface.



        m_lReturn = GetRecoveryTypeDescription(m_lRecoveryTypeID, sRecoveryTypeDesc, g_vSalvageRecoveryTypes, g_vSalvageRecoveryTypes.GetLowerBound(1), g_vSalvageRecoveryTypes.GetUpperBound(0) + 1)

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to assign the data.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        lvwRecovery.Items.Item(lSelectedIndex - 1).Text = sRecoveryTypeDesc.Trim()


        Select Case ClaimMode
            Case gPMConstants.PMEComponentAction.PMAdd

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve).Trim())


                m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(lvwRecovery.Items.Item(lSelectedIndex - 1).Tag)) = m_lRecoveryTypeID


            Case gPMConstants.PMEComponentAction.PMEdit

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cInitialReserve).Trim())

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve).Trim())

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cNewreserve).Trim())

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 6).Text = CStr(m_cTaxAmount)
                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 7).Text = m_sTaxTypeDesc
                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 8).Text = m_sTaxBandDesc


                m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(lvwRecovery.Items.Item(lSelectedIndex - 1).Tag)) = m_lRecoveryTypeID

            Case gPMConstants.PMEComponentAction.PMView

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cReceiptAmountLoss).Trim())

                'RWH(25/06/01) Shift setting of values below along one to account for introduction
                'of Initial Reserve as 3rd column.
                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cRevisedReserve).Trim())

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_cNewreserve).Trim())

                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 6).Text = CStr(m_cTaxAmount)
                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 7).Text = m_sTaxTypeDesc
                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 8).Text = m_sTaxBandDesc
                ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lSelectedIndex - 1), 9).Text = CStr(m_cReceiptAmount)


                m_vRecoveryTypeID(ACIRecoveryTypeID, Convert.ToString(lvwRecovery.Items.Item(lSelectedIndex - 1).Tag)) = m_lRecoveryTypeID


        End Select


        Return result
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

            cmdEdit.Enabled = Not bDisable

            cmdDelete.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub txtExchangeRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExchangeRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Dim dbNumericTemp As Double
        If Double.TryParse(txtExchangeRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

            m_lExchangeRate = CInt(CDec(txtExchangeRate.Text.Trim()))

        End If

        If txtExchangeRate.Text = Nothing Then

            m_lExchangeRate = 0

            'm_lDecimalCount = 0

        End If



    End Sub

    Private Sub txtExchangeRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExchangeRate.Enter
        iPMFunc.SelectText(txtExchangeRate)
    End Sub

    Private Sub txtExchangeRate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtExchangeRate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        '    If (KeyAscii = 46) Then
        '
        '        m_lDecimalCount = m_lDecimalCount + 1
        '
        '    End If
        '
        '    If m_lDecimalCount > 1 Then
        '
        '        Call DisplayMessage(ACInvalidCurrencyDecimalPointsMsg, Mid(lblExchangeRate.Name, 4))
        '
        '        KeyAscii = 0
        '
        '        m_lDecimalCount = 1
        '
        '        Exit Sub
        '
        '    End If

        If KeyAscii <> 46 Then

            If KeyAscii <> 8 Then
                If KeyAscii <> 32 Then

                    If KeyAscii < 48 Or KeyAscii > 57 Then

                        KeyAscii = 0

                        DisplayMessage(ACInvalidNumberMsg, Mid(lblExchangeRate.Name, 4))

                    End If
                End If

            End If
        End If


        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtExchangeRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExchangeRate.Leave

        If txtExchangeRate.Text <> "" Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtExchangeRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                DisplayMessage(ACInvalidCurrencyMsg, Mid(lblExchangeRate.Name, 4))

                txtExchangeRate.Text = ""

                txtExchangeRate.Focus()

            Else

                txtExchangeRate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtExchangeRate.Text.Trim())

            End If


        End If


    End Sub

    Public Function ExchangeRateMandatory() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If txtExchangeRate.Text = "" Then

            DisplayMessage(ACMandatoryFieldMsg, Mid(lblExchangeRate.Name, 4))
            Return gPMConstants.PMEReturnCode.PMFalse

        Else

            If StringsHelper.ToDoubleSafe(txtExchangeRate.Text.Trim()) = 0 Then

                DisplayMessage(ACInvalidExchangeRateMsg, Mid(lblExchangeRate.Name, 4))
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        End If


        Return result
    End Function

    Public Sub ClearCoReInsurer()


        If lvwCoInsurance.Items.Count > 0 Then

            For ncount As Integer = 1 To lvwCoInsurance.Items.Count

                ListViewHelper.GetListViewSubItem(lvwCoInsurance.Items.Item(ncount - 1), 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(0))

            Next ncount

        End If

        If lvwReinsurance.Items.Count > 0 Then

            For ncount As Integer = 1 To lvwReinsurance.Items.Count

                ListViewHelper.GetListViewSubItem(lvwReinsurance.Items.Item(ncount - 1), 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(0))

            Next ncount

        End If

    End Sub
    '***************************************************************** '
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




            m_lReturn = g_oBusiness.GetSalvageRecoveryType(g_vSalvageRecoveryTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                Else

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup details.

                    m_lReturn = GetLookupDetails(sLookupTable:="Currency", ctlLookup:=cboCurrency)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                Case gPMConstants.PMEComponentAction.PMView

                    ' Get all of the lookup details.

                    m_lReturn = GetLookupDetails(sLookupTable:="Currency", ctlLookup:=cboCurrency)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get all of the lookup details.

                    m_lReturn = GetLookupDetails(sLookupTable:="Coinsurance_Treatment", ctlLookup:=cboCoinsuranceTreatment)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


            End Select



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
            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
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
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        'Dim newIndex As Object

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False


            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.

                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.




            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))



                'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(Trim(m_vLookupDetails(ACDetailDesc, lCntr)), CInt(m_vLookupDetails(ACDetailKey, lCntr))))


                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                '        If (m_vLookupValues(ACValueID, lRow&) = _
                ''        CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
                '            ctlLookup.ListIndex = ctlLookup.NewIndex
                '        End If


                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then


                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide no.28
                        'ctlLookup.ListIndex = ctlLookup.NewIndex
                        ctlLookup.SelectedIndex = NewIndex
                    End If
                End If

            Next lCntr

            '    ' Check if the selected index is blank. If so,
            '    ' we set the controls index to zero.
            '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
            '        ctlLookup.ListIndex = 0
            '    End If
            '
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetReceiptAmount(ByRef cReceiptAmount As Decimal) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue




        For nRecCount As Integer = 1 To lvwRecovery.Items.Count

            If gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nRecCount - 1), 9).Text, 0) <> 0 And ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nRecCount - 1), 9).Text.Trim() <> Nothing Then


                cReceiptAmount += CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatCurrency, iDataTypeOut:=gPMConstants.PMEDataType.PMCurrency, vFieldValue:=CDec(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nRecCount - 1), 9).Text.Trim())))
            End If

        Next nRecCount



        Return result
    End Function

    Public Function GetComments(ByRef sComments As String) As Integer



        For nRecCount As Integer = 1 To lvwRecovery.Items.Count

            If StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(nRecCount - 1), 1).Text) <> 0 Then

                sComments = sComments & "," & lvwRecovery.Items.Item(nRecCount - 1).Text.Trim()

            End If

        Next nRecCount


    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()

                End If

                
                g_oBusiness = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    '*******************************************************************************
    ' Name : PostSalvageToOrion
    '
    ' Desc : post total amount of salvage to orion for this peril
    '
    ' Hist : 28/03/2001 Created - Tinny
    '*******************************************************************************
    Private Function PostSalvageToOrion(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimID As Integer, ByVal v_lPerilID As Integer, ByVal v_cTransAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_sDebitMappingCode As String, ByVal v_lCOBId As Object, ByVal v_sCOBCode As String, Optional ByVal v_lPartyCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oControlTrans As bControlTransClaims.Automated
        Dim lDebitAccountID As Integer
        Dim sDebitTransLedgerCode As String = ""
        Dim lCreditAccountID As Integer
        Dim sCreditAccountCode, sCreditMappingCode As String
        Dim sLedgerShortName As String = ""
        Dim lStatsFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sCreditAccountCode = "CLAIMEXP"
            sCreditMappingCode = "CLMEXP" & v_sCOBCode

            'create object to send to orion
            Dim temp_oControlTrans As Object
            result = g_oObjectManager.GetInstance(temp_oControlTrans, "bControlTransClaims.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oControlTrans = temp_oControlTrans

            If result <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bControlTransClaims object", vApp:=ACApp, vClass:=ACClass, vMethod:="PostSalvageToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            'get debit account id - use party count if we have it
            If v_lPartyCnt <> 0 Then

                result = oControlTrans.GetAccountID(r_lAccountID:=lDebitAccountID, v_lPartyCnt:=v_lPartyCnt)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then

                    oControlTrans.Dispose()
                    oControlTrans = Nothing

                    Return result
                End If

            End If

            'data which goes in stats folder/detail and transaction detail

            oControlTrans.DebitAccountID = lDebitAccountID

            oControlTrans.CreditAccountID = lCreditAccountID

            oControlTrans.TransactionTypeID = 28

            oControlTrans.TransactionTypeCode = "C_SA" 'claim maintenance

            oControlTrans.DocumentTypeID = 29 'Claim receipt

            oControlTrans.InsuranceFileCnt = v_lInsuranceFileCnt

            oControlTrans.ClaimId = v_lClaimID

            oControlTrans.PerilID = v_lPerilID

            oControlTrans.DebitCredit = "C"

            oControlTrans.DocumentComment = "Salvage for claim number "
            'RWH(14/09/01) Reverse sign of transaction so Receivable is credited.

            oControlTrans.TransactionAmount = -v_cTransAmount

            'RWH(02/07/01) Need to create stats separately now for each record to
            'account for reins and coins.

            m_lReturn = oControlTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:="C_SA")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(02/07/01) Create stats_detail for main payment.

            m_lReturn = oControlTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=v_sDebitMappingCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_cTaxAmount <> 0 Then

                ' Pass tax amount

                oControlTrans.TransactionAmount = m_cTaxAmount

                ' set tan / tag account code
                sCreditAccountCode = "NOTA" & m_sTaxTypeCode.Trim() & "IN"

                ' Create stats for TAG amount

                m_lReturn = oControlTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAG", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    oControlTrans.Dispose()
                    oControlTrans = Nothing

                    Return result
                End If
            End If



            oControlTrans.Dispose()
            oControlTrans = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSalvageToOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostSalvageToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '
    ' Name: PopulateCoReInsurerOnSelection
    '
    ' Description:
    '
    ' History: 27/06/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function PopulateCoReInsurerOnSelection() As Integer
        Dim result As Integer = 0
        Dim cAmountAfterCoins, cCoinsAmount As Decimal
        Dim oListItem As ListViewItem

        'TN20010906
        Dim cReceivedToDateCoin As Decimal

        'Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(26/06/01) Make sure Reins Coins included for View and Edit.

            Select Case ClaimMode
                Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMEdit
                    'Populate Coinsurance Tab


                    If g_oBusiness.RecordCount > 0 Then
                        '        m_lReturn = g_oBusiness.GetCoinsuranceRecoveries(m_m_vCoInsurerDetails, m_lClaimID)
                        '
                        '        If m_lReturn <> PMTrue Then
                        '
                        '            If (m_lReturn& = PMNotFound) Then
                        '
                        '            Else
                        '                ' Failed to assign the data.
                        '                PopulateCoReInsurerOnSelection = PMFalse
                        '                Exit Function
                        '            End If
                        '
                        '        End If

                        ' Clear the search details.
                        lvwCoInsurance.Items.Clear()

                        ' Check that search details are valid before
                        ' continuing.
                        If Information.IsArray(m_vCoInsurerDetails) Then

                            ' Assign the details to the interface.
                            For lRow As Integer = m_vCoInsurerDetails.GetLowerBound(1) To m_vCoInsurerDetails.GetUpperBound(1)


                                ' Assign the details to the first column.
                                ' Column 1 Claim Type

                                oListItem = lvwCoInsurance.Items.Add(CStr(m_vCoInsurerDetails(ACICoInsurerName, lRow)).Trim(), "")

                                ' Assign details to other the columns
                                ' Column 2 Claim Ref
                                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatPercent, vFieldValue:=CStr(m_vCoInsurerDetails(ACICoShare, lRow)).Trim())

                                '                If (ClaimMode = PMEdit) Then
                                If Information.IsArray(m_vRecdToDate) Then
                                    'RWH(26/06/01) Split existing received_to_date value.

                                    cCoinsAmount = CDec(m_vRecdToDate(ACIRecoveryID, Convert.ToString(lvwRecovery.FocusedItem.Tag)))

                                    cCoinsAmount = cCoinsAmount * Conversion.Val(CStr(m_vCoInsurerDetails(ACICoShare, lRow))) / 100

                                    cAmountAfterCoins = CDec(CDbl(m_vRecdToDate(ACIRecoveryID, Convert.ToString(lvwRecovery.FocusedItem.Tag))) - cCoinsAmount)

                                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cCoinsAmount))
                                    '                Else
                                    '                    oListItem.SubItems(2) = FormatField( _
                                    ''                                            iFormatType:=PMFormatCurrency, _
                                    ''                                            vFieldValue:=0)
                                    '                End If
                                End If

                                'TN20010906 - received to date (coinsurance)
                                If ClaimMode = gPMConstants.PMEComponentAction.PMView Then
                                    cReceivedToDateCoin = m_cReceivedTodate * Conversion.Val(CStr(m_vCoInsurerDetails(ACICoShare, lRow))) / 100

                                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cReceivedToDateCoin))

                                End If


                                ' Set the tag property with the index of
                                ' the search data storage.
                                oListItem.Tag = CStr(m_vCoInsurerDetails(ACICoInsurerId, lRow))


                                ' Refresh the first X amount of rows, to
                                ' allow the user to see the results instantly.
                                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                                    ' Select the first item.
                                    lvwCoInsurance.Items.Item(0).Selected = True

                                    ' Refresh the initial results.
                                    lvwCoInsurance.Refresh()
                                End If
                            Next lRow

                            ' Select the first item.
                            lvwCoInsurance.Items.Item(0).Selected = True

                        End If

                        '        'Populate Re-Insurer Details
                        '        m_lReturn = g_oBusiness.GetReinsuranceRecoveries(m_vReInsurerDetails, m_lClaimID)
                        '
                        '
                        '        If m_lReturn <> PMTrue Then
                        '
                        '        If (m_lReturn& = PMNotFound) Then
                        '
                        '        Else
                        '            ' Failed to assign the data.
                        '            PopulateCoReInsurerOnSelection = PMFalse
                        '            Exit Function
                        '        End If
                        '
                        '        End If
                        ' Clear the search details.
                        lvwReinsurance.Items.Clear()

                        ' Check that search details are valid before
                        ' continuing.
                        If Information.IsArray(m_vReInsurerDetails) Then


                            ' Assign the details to the interface.
                            For lRow As Integer = m_vReInsurerDetails.GetLowerBound(1) To m_vReInsurerDetails.GetUpperBound(1)


                                ' Assign the details to the first column.
                                ' Column 1 Claim Type

                                oListItem = lvwReinsurance.Items.Add(CStr(m_vReInsurerDetails(ACIReInsurerName, lRow)).Trim(), "")

                                ' Assign details to other the columns
                                ' Column 2 Claim Ref
                                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatPercent, vFieldValue:=CStr(m_vReInsurerDetails(ACIReShare, lRow)).Trim())

                                '                If (ClaimMode = PMEdit) Then
                                'RWH(26/06/01) Split existing received_to_date value.
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(cAmountAfterCoins * Conversion.Val(CStr(m_vReInsurerDetails(ACIReShare, lRow))) / 100))
                                '                Else
                                '                    oListItem.SubItems(2) = FormatField( _
                                ''                                            iFormatType:=PMFormatCurrency, _
                                ''                                            vFieldValue:=0)
                                '                End If

                                'TN20010906 - received to date (reinsurance)
                                If ClaimMode = gPMConstants.PMEComponentAction.PMView Then
                                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr((m_cReceivedTodate - cReceivedToDateCoin) * Conversion.Val(CStr(m_vReInsurerDetails(ACIReShare, lRow))) / 100))

                                End If

                                ' Set the tag property with the index of
                                ' the search data storage.
                                oListItem.Tag = CStr(m_vReInsurerDetails(ACIReInsurerId, lRow))


                                ' Refresh the first X amount of rows, to
                                ' allow the user to see the results instantly.
                                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                                    ' Select the first item.
                                    lvwReinsurance.Items.Item(0).Selected = True

                                    ' Refresh the initial results.
                                    lvwReinsurance.Refresh()
                                End If
                            Next lRow

                            ' Select the first item.
                            lvwReinsurance.Items.Item(0).Selected = True

                        End If

                    End If
            End Select



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateCoReInsurerOnSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCoReInsurerOnSelection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetMappingCodes
    '
    ' Description:
    '
    ' History: 28/06/2001 RWH - Created.
    '
    ' ***************************************************************** '

    'Private Function GetMappingCodes(ByRef v_sLedgerShortName As String, ByRef r_sTransLedgerCode As String, ByRef r_sLedgerMappingCode As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Select Case v_sLedgerShortName.Trim()
    'Case "NO"
    'r_sTransLedgerCode = "NO"
    'r_sLedgerMappingCode = "CLAIMREC"
    '
    'Case "SA"
    'r_sTransLedgerCode = "SL"
    'r_sLedgerMappingCode = "SALESLEDGR"
    '
    'Case "PU", "RF", "FE", "DI", "CO"
    '
    'Case "IN"
    'r_sTransLedgerCode = "IN"
    'r_sLedgerMappingCode 'REINSACC or COINSACC
    '
    'Case "AG"
    'r_sTransLedgerCode = "AG"
    'r_sLedgerMappingCode = "AGENTLEDGR"
    '
    'Case "UB"
    '
    '
    'End Select
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMappingCodes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMappingCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    '       Added a functionality. On pressing OK, in Salvage screen,
    '                             when the Sum(CurrentReserve) = 0 then a message will come up
    '                             asking the User wether the Claim can be closed.
    '                             If the reply is YES the Claim status is set to closed.
    Private Sub CheckCurrentReserve()

        Dim cCurrentReserve, cCurrentRecovery As Decimal
        Dim sTitle, sMessage As String
        Dim vDataArray(,) As Object

        '    If Task <> PMEdit Then Exit Sub


        m_lReturn = g_oBusiness.GetCurrentReserveRecovery(ClaimId, vDataArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vDataArray) Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidAction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedToGetCurrentReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
            Exit Sub
        End If



        If CStr(vDataArray(0, 0)) = "3" Then Exit Sub ' If it is already closed exit sub

        ' If the Claim is not closed the check the current reserve.
        ' If is not 0 then exit sub else close the Claim


        Dim auxVar As Object = vDataArray(1, 0)


        If (Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Or Not Object.Equals(vDataArray(1, 0), Nothing)) And CStr(vDataArray(1, 0)) <> "" Then

            cCurrentReserve = CDec(vDataArray(1, 0))
            If cCurrentReserve <> 0 Then
                Exit Sub
            End If
        End If

        ' If the Claim is not closed the check the current recovery.
        ' If is not 0 then exit sub else close the Claim


        Dim auxVar_2 As Object = vDataArray(2, 0)


        If (Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Or Not Object.Equals(vDataArray(2, 0), Nothing)) And CStr(vDataArray(2, 0)) <> "" Then

            cCurrentRecovery = CDec(vDataArray(2, 0))
            If cCurrentRecovery <> 0 Then
                Exit Sub
            End If
        End If

        'Confirmation before closing the claim

        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseClaimTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseClaimDetail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
        If MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
            Exit Sub
        End If


        m_lReturn = g_oBusiness.CloseClaim(ClaimId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseClaimTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedToCloseClaim, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
            Exit Sub
        End If

    End Sub
End Class