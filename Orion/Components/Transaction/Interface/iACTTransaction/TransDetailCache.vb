Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class TransDetailCache
    Implements IDisposable
    ' **************************************************************************
    '
    ' CTAF 20030410 - Class created to cache the information stored on screen
    '
    ' This is a TPU requirement and controlled via a System Option.
    ' At present this is for 1.8.6 only.
    '
    ' The specific system option is SIROPTCacheTransDetail (0 or 1)
    '
    ' Most code is contained in this class to aid in the migration to 1.9
    ' when that happens.
    '
    ' To find the code in the rest of the project search for:
    '       CTAF 20030410
    '       CTAF 20030411
    '       etc...
    '
    ' **************************************************************************

    Private Const ACClass As String = "TransDetailCache"

    ' Tab 1 of the display
    Private m_lAccountID As Integer
    Private m_lCurrencyID As Integer
    Private m_dAmount As Double
    Private m_sComment As String = ""
    Private m_dBaseAmount As Double
    Private m_dExchangeRate As Double
    Private m_dAmountInEuros As Double
    Private m_lDepartmentID As Integer

    ' Tab 2 of the display
    Private m_sInsuranceRef As String = ""
    Private m_sPurchaseOrderNo As String = ""
    Private m_sPurchaseInvoiceNo As String = ""

    ' Extra information required
    Private m_lUserID As Integer

    ' Private variables
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lInstanceCount As Integer
    Private m_bLoaded As Boolean

    ' TransDetail
    Private m_oTransDetail As Object

    Public Property TransDetail() As Object
        Get
            Return m_oTransDetail
        End Get
        Set(ByVal Value As Object)
            m_oTransDetail = Value
        End Set
    End Property

    Private Property Loaded() As Boolean
        Get
            Return m_bLoaded
        End Get
        Set(ByVal Value As Boolean)
            m_bLoaded = Value
        End Set
    End Property

    Private Property InstanceCount() As Integer
        Get
            Return m_lInstanceCount
        End Get
        Set(ByVal Value As Integer)
            m_lInstanceCount = Value
        End Set
    End Property

    Public Property AccountID() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property

    Public Property Amount() As Double
        Get
            Return m_dAmount
        End Get
        Set(ByVal Value As Double)
            m_dAmount = Value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return m_sComment
        End Get
        Set(ByVal Value As String)
            m_sComment = Value
        End Set
    End Property

    Public Property BaseAmount() As Double
        Get
            Return m_dBaseAmount
        End Get
        Set(ByVal Value As Double)
            m_dBaseAmount = Value
        End Set
    End Property

    Public Property ExchangeRate() As Double
        Get
            Return m_dExchangeRate
        End Get
        Set(ByVal Value As Double)
            m_dExchangeRate = Value
        End Set
    End Property

    Public Property AmountInEuros() As Double
        Get
            Return m_dAmountInEuros
        End Get
        Set(ByVal Value As Double)
            m_dAmountInEuros = Value
        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return m_lDepartmentID
        End Get
        Set(ByVal Value As Integer)
            m_lDepartmentID = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

    Public Property PurchaseOrderNo() As String
        Get
            Return m_sPurchaseOrderNo
        End Get
        Set(ByVal Value As String)
            m_sPurchaseOrderNo = Value
        End Set
    End Property

    Public Property PurchaseInvoiceNo() As String
        Get
            Return m_sPurchaseInvoiceNo
        End Get
        Set(ByVal Value As String)
            m_sPurchaseInvoiceNo = Value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return m_lUserID
        End Get
        Set(ByVal Value As Integer)
            m_lUserID = Value
        End Set
    End Property

    ' **************************************************************************
    '
    ' Property:    CacheScreens
    '
    ' Description: Called by the rest of the module to decide if we are
    '              caching them or not
    '
    ' History:     CTAF 20030410 - Created
    '
    ' **************************************************************************
    Public ReadOnly Property CacheScreens() As Boolean
        Get

            Dim result As Boolean = False
            Dim vUnderwriting As String = "" ' Weird name for the Value but oh well...

            Try

                ' CTAF 20030410 - Apparently we use SIRBCHHeadOffice as the branch...

                ' Get the product option
                m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCacheTransDetail, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vUnderwriting), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log an error and return false
                    result = False
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheScreens", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' Decipher the return value

                Return vUnderwriting = "1"

            Catch excep As System.Exception




                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CacheScreens Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheScreens", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End Try
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description: Standard initialise method for the class
    '
    ' History: 10/04/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_iUserID As Integer, ByRef r_oTransDetail As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Increase the instances
            InstanceCount += 1

            ' Store the UserID
            UserID = v_iUserID

            TransDetail = r_oTransDetail

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description: Standard terminate function
    '
    ' History: 10/04/2003 CTAF - Created.
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


    ' ***************************************************************** '
    '
    ' Name: FlushCache
    '
    ' Description: Flushes the cache back to the database
    '
    ' History: 10/04/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function FlushCache() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save the data back to the database
            With Me

                m_lReturn = m_oTransDetail.SaveTransCache(v_lUserID:=.UserID, v_lSourceID:=g_iSourceID, v_lAccountID:=.AccountID, v_lCurrencyID:=.CurrencyID, v_dAmount:=.Amount, v_sComment:=.Comment, v_dBaseAmount:=.BaseAmount, v_dExchangeRate:=.ExchangeRate, v_dAmountInEuros:=.AmountInEuros, v_lDepartmentID:=.DepartmentID, v_sInsuranceRef:=.InsuranceRef, v_sPurchaseOrderNo:=.PurchaseOrderNo, v_sPurchaseInvoiceNo:=.PurchaseInvoiceNo)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save cache values to database", vApp:=ACApp, vClass:=ACClass, vMethod:="FlushCache", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FlushCache Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FlushCache", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadCache
    '
    ' Description: Loads the cache from the database (if needed)
    '
    ' History: 10/04/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function LoadCache() As Integer

        Dim result As Integer = 0
        Dim lUserID, lAccountID, lCurrencyID As Integer
        Dim dAmount As Double
        Dim sComment As String = ""
        Dim dBaseAmount, dExchangeRate, dAmountInEuros As Double
        Dim lDepartmentID As Integer
        Dim sInsuranceRef, sPurchaseOrderNo, sPurchaseInvoiceNo As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Loaded Then
                ' Exit - we've loaded already
                Return result
            End If

            ' Load the data from the database

            m_lReturn = m_oTransDetail.LoadTransCache(v_lUserID:=lUserID, v_lSourceID:=g_iSourceID, r_lAccountID:=lAccountID, r_lCurrencyID:=lCurrencyID, r_dAmount:=dAmount, r_sComment:=sComment, r_dBaseAmount:=dBaseAmount, r_dExchangeRate:=dExchangeRate, r_dAmountInEuros:=dAmountInEuros, r_lDepartmentID:=lDepartmentID, r_sInsuranceRef:=sInsuranceRef, r_sPurchaseOrderNo:=sPurchaseOrderNo, r_sPurchaseInvoiceNo:=sPurchaseInvoiceNo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Get the return values
            With Me
                .AccountID = lAccountID
                .CurrencyID = lCurrencyID
                .Amount = dAmount
                .Comment = sComment
                .BaseAmount = dBaseAmount
                .ExchangeRate = dExchangeRate
                .AmountInEuros = dAmountInEuros
                .DepartmentID = lDepartmentID
                .InsuranceRef = sInsuranceRef
                .PurchaseOrderNo = sPurchaseOrderNo
                .PurchaseInvoiceNo = sPurchaseInvoiceNo
            End With

            ' So we dont do any more un-needed loaded...
            Loaded = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadCache Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadCache", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
