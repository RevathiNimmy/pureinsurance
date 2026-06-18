Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Sudhanshu Behera on 6/24/2010 11:27:36 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Add_NET.Add")> _
Public NotInheritable Class Add
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Add"

    'General
    Dim m_lItemNumber As Integer
    Dim m_dtDateEntered As Date
    Dim m_sItemClaimed As String = ""
    Dim m_sItemDescription As String = ""
    Dim m_sSettlementMethod As String = ""
    Dim m_dStartingValue As Double
    Dim m_lAge As Integer
    Dim m_lLife As Integer
    Dim m_dDepreciationPercent As Double
    Dim m_sDepreciation As String = ""
    Dim m_dGST As Double
    Dim m_dItemAmount As Double
    Dim m_dPaymentAmount As Double
    Dim m_dExcess As Double
    Dim m_lPayeeOrSupplier As Integer
    Dim m_sItemStatus As String = ""
    Dim m_dtPODate As Date
    Dim m_dtDatePaid As Date
    Dim m_lSalvage As Integer

    Private m_lReturn As gPMConstants.PMEReturnCode



    Public Property iItemNumber() As Integer
        Get
            Return m_lItemNumber
        End Get
        Set(ByVal Value As Integer)
            m_lItemNumber = Value
        End Set
    End Property


    Public Property dtDateEntered() As Date
        Get
            Return m_dtDateEntered
        End Get
        Set(ByVal Value As Date)
            m_dtDateEntered = Value
        End Set
    End Property


    Public Property sItemClaimed() As String
        Get
            Return m_sItemClaimed
        End Get
        Set(ByVal Value As String)
            m_sItemClaimed = Value
        End Set
    End Property


    Public Property sItemDescription() As String
        Get
            Return m_sItemDescription
        End Get
        Set(ByVal Value As String)
            m_sItemDescription = Value
        End Set
    End Property


    Public Property sSettlementMethod() As String
        Get
            Return m_sSettlementMethod
        End Get
        Set(ByVal Value As String)
            m_sSettlementMethod = Value
        End Set
    End Property


    Public Property dStartingValue() As Double
        Get
            Return m_dStartingValue
        End Get
        Set(ByVal Value As Double)
            m_dStartingValue = Value
        End Set
    End Property


    Public Property lAge() As Integer
        Get
            Return m_lAge
        End Get
        Set(ByVal Value As Integer)
            m_lAge = Value
        End Set
    End Property


    Public Property lLife() As Integer
        Get
            Return m_lLife
        End Get
        Set(ByVal Value As Integer)
            m_lLife = Value
        End Set
    End Property


    Public Property dDepreciationPercent() As Double
        Get
            Return m_dDepreciationPercent
        End Get
        Set(ByVal Value As Double)
            m_dDepreciationPercent = Value
        End Set
    End Property


    Public Property sDepreciation() As String
        Get
            Return m_sDepreciation
        End Get
        Set(ByVal Value As String)
            m_sDepreciation = Value
        End Set
    End Property


    Public Property dGST() As Double
        Get
            Return m_dGST
        End Get
        Set(ByVal Value As Double)
            m_dGST = Value
        End Set
    End Property


    Public Property dItemAmount() As Double
        Get
            Return m_dItemAmount
        End Get
        Set(ByVal Value As Double)
            m_dItemAmount = Value
        End Set
    End Property


    Public Property dPaymentAmount() As Double
        Get
            Return m_dPaymentAmount
        End Get
        Set(ByVal Value As Double)
            m_dPaymentAmount = Value
        End Set
    End Property


    Public Property dExcess() As Double
        Get
            Return m_dExcess
        End Get
        Set(ByVal Value As Double)
            m_dExcess = Value
        End Set
    End Property


    Public Property lPayeeOrSupplier() As Integer
        Get
            Return m_lPayeeOrSupplier
        End Get
        Set(ByVal Value As Integer)
            m_lPayeeOrSupplier = Value
        End Set
    End Property


    Public Property sItemStatus() As String
        Get
            Return m_sItemStatus
        End Get
        Set(ByVal Value As String)
            m_sItemStatus = Value
        End Set
    End Property


    Public Property dtPODate() As Date
        Get
            Return m_dtPODate
        End Get
        Set(ByVal Value As Date)
            m_dtPODate = Value
        End Set
    End Property


    Public Property dtDatePaid() As Date
        Get
            Return m_dtDatePaid
        End Get
        Set(ByVal Value As Date)
            m_dtDatePaid = Value
        End Set
    End Property


    Public Property lSalvage() As Integer
        Get
            Return m_lSalvage
        End Get
        Set(ByVal Value As Integer)
            m_lSalvage = Value
        End Set
    End Property

    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise



        ' ***************************************************************** '
        '
        ' Name: Initialise
        '
        ' Description:
        '
        ' History: 16092002 CMG/PB - Created.
        '
        ' ***************************************************************** '

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
            End With

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bObjectManager.ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



        End Try
    End Function


    Public Function Start() As Integer
        ' ***************************************************************** '
        '
        ' Name: Start
        '
        ' Description:
        '
        ' History: 26/10/1999 CTAF - Created.
        '
        ' ***************************************************************** '

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Function ProcessInterface() As Integer
        ' ***************************************************************** '
        '
        ' Name: ProcessInterface
        '
        ' Description:
        '
        ' History: 16092002 CMG/PB - Created.
        '
        ' ***************************************************************** '

        Dim result As Integer = 0
        Dim oAdd As frmAdd



        result = gPMConstants.PMEReturnCode.PMTrue

        oAdd = New frmAdd()

        With oAdd
            '.ItemDetailId = m_lItemDetailId
        End With

        ' Load the form

        'Modified by Sudhanshu Behera on 6/24/2010 11:28:26 AM refer developer guide no. 68
        'Load(oAdd)

        ' Show the form
        oAdd.ShowDialog()

        ' Remove the instance
        oAdd.Close()
        oAdd = Nothing

        Return result

    End Function
End Class

