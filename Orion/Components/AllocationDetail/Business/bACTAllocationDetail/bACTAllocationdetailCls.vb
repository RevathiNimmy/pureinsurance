Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Allocationdetail_NET.Allocationdetail")>
Public NotInheritable Class Allocationdetail
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Allocationdetail
    '
    ' Date: 22/01/1998
    '
    ' Description: Describes the Allocationdetail attributes.
    '
    ' Edit History: TF191198 - amendments for EMU database changes
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "AllocationDetail"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lAllocationdetailID As Integer
    Private m_lCashlistitemID As Integer
    Private m_lAllocationID As Integer
    Private m_iOriginalCurrency As Integer
    Private m_lTransdetailID As Integer
    Private m_iDocumenttypeID As Integer
    Private m_dtAccountingDate As Date
    Private m_sDocumentRef As New StringsHelper.FixedLengthString(25)
    Private m_dtOriginalDate As Date
    Private m_iAllocateToBase As Integer
    Private m_cOrigBaseAmount As Decimal
    'developer guide no. 101
    Private m_vOrigBaseAmountUnrounded As Object
    Private m_cOrigCcyAmount As Decimal
    Private m_vOrigCcyAmountUnrounded As Object
    Private m_vdOrigXrate As Decimal
    Private m_vdEffectiveXrate As Decimal
    Private m_cOsBaseAmount As Decimal
    Private m_cOsCcyAmount As Decimal
    Private m_cAllocBaseAmount As Decimal
    Private m_cAllocCcyAmount As Decimal
    Private m_iFullyMatched As Integer
    Private m_cWriteOffAmount As Decimal
    Private m_lWriteOffReasonID As Integer
    Private m_cNewOsCcyAmount As Decimal
    Private m_cNewOsBaseAmount As Decimal
    Private m_cLossGainAmount As Decimal
    Private m_iIsPrimary As Integer
    Private m_lEuroCurrencyID As Integer

    Private m_vEuroAmount As Object
    Private m_vEuroBaseXRate As Object
    Private m_vEuroCcyXRate As Object

    Private m_vdAllocBaseAmountUnrounded As Double

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_crAllocAccountAmount As Decimal = 0
    Private m_crAllocSystemAmount As Decimal = 0
    Private m_nTransdetailExID As Integer = 0
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property
    Public Property TransdetailExID() As Integer
        Get

            Return m_nTransdetailExID

        End Get
        Set(ByVal Value As Integer)

            m_nTransdetailExID = Value

        End Set
    End Property

    Public Property AllocationdetailID() As Integer
        Get

            Return m_lAllocationdetailID

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationdetailID = Value

        End Set
    End Property

    Public Property CashlistitemID() As Integer
        Get

            Return m_lCashlistitemID

        End Get
        Set(ByVal Value As Integer)

            m_lCashlistitemID = Value

        End Set
    End Property

    Public Property AllocationID() As Integer
        Get

            Return m_lAllocationID

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationID = Value

        End Set
    End Property

    Public Property OriginalCurrency() As Integer
        Get

            Return m_iOriginalCurrency

        End Get
        Set(ByVal Value As Integer)

            m_iOriginalCurrency = Value

        End Set
    End Property

    Public Property TransdetailID() As Integer
        Get

            Return m_lTransdetailID

        End Get
        Set(ByVal Value As Integer)

            m_lTransdetailID = Value

        End Set
    End Property

    Public Property DocumenttypeID() As Integer
        Get

            Return m_iDocumenttypeID

        End Get
        Set(ByVal Value As Integer)

            m_iDocumenttypeID = Value

        End Set
    End Property

    Public Property AccountingDate() As Date
        Get

            Return m_dtAccountingDate

        End Get
        Set(ByVal Value As Date)

            m_dtAccountingDate = Value

        End Set
    End Property

    Public Property DocumentRef() As String
        Get

            Return m_sDocumentRef.Value

        End Get
        Set(ByVal Value As String)

            m_sDocumentRef.Value = Value

        End Set
    End Property

    Public Property OriginalDate() As Date
        Get

            Return m_dtOriginalDate

        End Get
        Set(ByVal Value As Date)

            m_dtOriginalDate = Value

        End Set
    End Property

    Public Property AllocateToBase() As Integer
        Get

            Return m_iAllocateToBase

        End Get
        Set(ByVal Value As Integer)

            m_iAllocateToBase = Value

        End Set
    End Property

    Public Property OrigBaseAmount() As Decimal
        Get

            Return m_cOrigBaseAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cOrigBaseAmount = Value

        End Set
    End Property
    'developer guide no. 101
    Public Property OrigBaseAmountUnrounded() As Object
        Get

            Return m_vOrigBaseAmountUnrounded

        End Get
        Set(ByVal Value As Object)


            m_vOrigBaseAmountUnrounded = Value

        End Set
    End Property

    Public Property OrigCcyAmount() As Decimal
        Get

            Return m_cOrigCcyAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cOrigCcyAmount = Value

        End Set
    End Property
    'developer guide no. 101
    Public Property OrigCcyAmountUnrounded() As Object
        Get

            Return m_vOrigCcyAmountUnrounded

        End Get
        Set(ByVal Value As Object)

            m_vOrigCcyAmountUnrounded = Value

        End Set
    End Property

    Public Property OrigXrate() As Double
        Get

            Return m_vdOrigXrate

        End Get
        Set(ByVal Value As Double)


            m_vdOrigXrate = CDec(Value)

        End Set
    End Property

    Public Property EffectiveXrate() As Double
        Get

            Return m_vdEffectiveXrate

        End Get
        Set(ByVal Value As Double)


            m_vdEffectiveXrate = CDec(Value)

        End Set
    End Property

    Public Property OsBaseAmount() As Decimal
        Get

            Return m_cOsBaseAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cOsBaseAmount = Value

        End Set
    End Property

    Public Property OsCcyAmount() As Decimal
        Get

            Return m_cOsCcyAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cOsCcyAmount = Value

        End Set
    End Property

    Public Property AllocBaseAmount() As Decimal
        Get

            Return m_cAllocBaseAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cAllocBaseAmount = Value

        End Set
    End Property

    Public Property AllocCcyAmount() As Decimal
        Get

            Return m_cAllocCcyAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cAllocCcyAmount = Value

        End Set
    End Property

    Public Property FullyMatched() As Integer
        Get

            Return m_iFullyMatched

        End Get
        Set(ByVal Value As Integer)

            m_iFullyMatched = Value

        End Set
    End Property

    Public Property WriteOffAmount() As Decimal
        Get

            Return m_cWriteOffAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cWriteOffAmount = Value

        End Set
    End Property


    Public Property WriteOffReasonID() As Integer
        Get

            Return m_lWriteOffReasonID

        End Get
        Set(ByVal Value As Integer)

            m_lWriteOffReasonID = Value

        End Set
    End Property


    Public Property NewOsCcyAmount() As Decimal
        Get

            Return m_cNewOsCcyAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cNewOsCcyAmount = Value

        End Set
    End Property

    Public Property NewOsBaseAmount() As Decimal
        Get

            Return m_cNewOsBaseAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cNewOsBaseAmount = Value

        End Set
    End Property

    Public Property LossGainAmount() As Decimal
        Get

            Return m_cLossGainAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cLossGainAmount = Value

        End Set
    End Property

    Public Property IsPrimary() As Integer
        Get

            Return m_iIsPrimary

        End Get
        Set(ByVal Value As Integer)

            m_iIsPrimary = Value

        End Set
    End Property

    Public Property EuroCurrencyID() As Integer
        Get

            Return m_lEuroCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_lEuroCurrencyID = Value

        End Set
    End Property
    'developer guide no. 101
    Public Property EuroAmount() As Object
        Get

            Return m_vEuroAmount

        End Get
        Set(ByVal Value As Object)


            m_vEuroAmount = Value

        End Set
    End Property
    'developer guide no. 101
    Public Property EuroBaseXRate() As Object
        Get

            Return m_vEuroBaseXRate

        End Get
        Set(ByVal Value As Object)


            m_vEuroBaseXRate = Value

        End Set
    End Property
    'developer guide no. 101
    Public Property EuroCcyXRate() As Object
        Get

            Return m_vEuroCcyXRate

        End Get
        Set(ByVal Value As Object)

            m_vEuroCcyXRate = Value

        End Set
    End Property

    Public Property AllocAccountAmount() As Decimal
        Get

            Return m_crAllocAccountAmount

        End Get
        Set(ByVal Value As Decimal)

            m_crAllocAccountAmount = Value

        End Set
    End Property

    Public Property AllocSystemAmount() As Decimal
        Get

            Return m_crAllocSystemAmount

        End Get
        Set(ByVal Value As Decimal)

            m_crAllocSystemAmount = Value

        End Set
    End Property

    Public ReadOnly Property AllocBaseAmountUnrounded() As Double
        Get

            Dim result As Double = 0
            Dim vBaseRoundingDifference As Double
            Dim cAllocBaseAmount As Decimal
            Dim lReturn As gPMConstants.PMEReturnCode

            Try

                If m_vdAllocBaseAmountUnrounded <> 0 Then
                    result = m_vdAllocBaseAmountUnrounded
                Else

                    If AllocateToBase = gPMConstants.PMEReturnCode.PMTrue Then
                        m_vdAllocBaseAmountUnrounded = AllocBaseAmount
                    Else

                        lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=OriginalCurrency, lCompanyID:=m_iSourceID, cBaseAmount:=cAllocBaseAmount, cCurrencyAmount:=AllocCcyAmount, vConversionDate:=Nothing, vBaseRoundingDifference:=vBaseRoundingDifference, vConversionRate:=EffectiveXrate)

                        m_vdAllocBaseAmountUnrounded = cAllocBaseAmount + vBaseRoundingDifference

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        result = m_vdAllocBaseAmountUnrounded
                    End If
                End If

                Return result

            Catch excep As System.Exception




                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get AllocBaseAmountUnrounded", vApp:=ACApp, vClass:=ACClass, vMethod:="Property Get AllocBaseAmountUnrounded", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End Try
        End Get
    End Property

    Public WriteOnly Property CurrencyConvert() As bACTCurrencyConvert.Form
        Set(ByVal Value As bACTCurrencyConvert.Form)

            m_oCurrencyConvert = Value

        End Set
    End Property
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    Friend Sub New()
        MyBase.New()

        ' Class Initialise
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class