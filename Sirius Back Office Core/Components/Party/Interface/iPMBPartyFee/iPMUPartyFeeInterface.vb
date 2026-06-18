Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("UInterface_NET.UInterface")> _
Public NotInheritable Class UInterface
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: UInterface
    '
    ' Date: 20/05/04
    '
    ' Description: Main public class to accompany the UInterface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.

    Dim objfrmInterfaceUW As frmInterfaceUW
    Private Const ACClass As String = "UInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_lTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sNavigatorTitle As String = ""
    Private m_sStepStatus As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPartyCnt As Integer
    Private m_dFeePercentage As Double
    Private m_cFeeAmount As Decimal
    Private m_lTransactionTypeID As Integer
    Private m_lProductType As Integer
    Private m_sProductTypeDesc As String = ""
    Private m_nIsTaxable As Integer
    Private m_lFeeAmountID As Integer
    'is this for being called from the fee charges screen
    'or the road map
    Private m_lRMStepEdit As Integer

    'persist currency values
    Private m_lCurrencyID As Integer
    Private m_sCurrencyName As String = ""
    Private m_lCurrencyID_Item As Integer
    Private m_sCurrencyName_Item As String = ""

    'variables to check for mutiple entries if the same
    Private m_vFeeDetails As Object

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lIsAmmended As Integer
    Public m_bDeleteClick As Boolean
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String
    Public Property IsAmmended() As Integer
        Get
            Return m_lIsAmmended
        End Get
        Set(ByVal Value As Integer)
            m_lIsAmmended = Value
        End Set
    End Property
    Public Property FeeDetails() As Object
        Get
            Return m_vFeeDetails
        End Get
        Set(ByVal Value As Object)


            m_vFeeDetails = Value
        End Set
    End Property
    Public Property RMStepEdit() As Integer
        Get
            Return m_lRMStepEdit
        End Get
        Set(ByVal Value As Integer)
            m_lRMStepEdit = Value
        End Set
    End Property
    Public Property FeeAmountID() As Integer
        Get
            Return m_lFeeAmountID
        End Get
        Set(ByVal Value As Integer)
            m_lFeeAmountID = Value
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

    Public Property CurrencyID_Item() As Integer
        Get
            Return m_lCurrencyID_Item
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID_Item = Value
        End Set
    End Property

    Public Property CurrencyName() As String
        Get
            Return m_sCurrencyName
        End Get
        Set(ByVal Value As String)
            m_sCurrencyName = Value
        End Set
    End Property
    Public Property CurrencyName_Item() As String
        Get
            Return m_sCurrencyName_Item
        End Get
        Set(ByVal Value As String)
            m_sCurrencyName_Item = Value
        End Set
    End Property

    Public Property TransactionType() As Integer
        Get
            Return m_lTransactionTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionTypeID = Value
        End Set
    End Property
    Public Property TransactionTypeDesc() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public Property ProductType() As Integer
        Get
            Return m_lProductType
        End Get
        Set(ByVal Value As Integer)
            m_lProductType = Value
        End Set
    End Property
    Public Property ProductTypeDesc() As String
        Get
            Return m_sProductTypeDesc
        End Get
        Set(ByVal Value As String)
            m_sProductTypeDesc = Value
        End Set
    End Property
    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
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
    Public Property FeeAmount() As Decimal
        Get
            Return m_cFeeAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cFeeAmount = Value
        End Set
    End Property
    Public Property FeePercentage() As Double
        Get
            Return m_dFeePercentage
        End Get
        Set(ByVal Value As Double)
            m_dFeePercentage = Value
        End Set
    End Property
    Public Property IsTaxable() As Integer
        Get
            Return m_nIsTaxable
        End Get
        Set(ByVal Value As Integer)
            m_nIsTaxable = Value
        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property


    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property
    Public Property DeleteClick() As Boolean
        Get
            Return m_bDeleteClick
        End Get
        Set(ByVal Value As Boolean)
            m_bDeleteClick = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get

            Return m_sUniqueId

        End Get
        Set(ByVal Value As String)

            m_sUniqueId = Value

        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get

            Return m_sScreenHierarchy

        End Get
        Set(ByVal Value As String)

            m_sScreenHierarchy = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub
    ''' <summary>
    ''' Start
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function Start() As Integer

        Return Start(v_bDeleteClick:=False, r_bCancelDelete:=False)

    End Function
    ''' <summary>
    ''' Start
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <param name="r_bCancelDelete"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function Start(ByVal v_bDeleteClick As Boolean, ByRef r_bCancelDelete As Boolean) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface(v_bDeleteClick, r_bCancelDelete)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessInterface() As Integer

        Return ProcessInterface(v_bDeleteClick:=False, r_bConfirmDelete:=False)

    End Function
    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <param name="r_bConfirmDelete"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessInterface(ByVal v_bDeleteClick As Boolean, ByRef r_bConfirmDelete As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface(v_bDeleteClick)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        r_bConfirmDelete = objfrmInterfaceUW.DeleteClick

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ''' <summary>
    ''' LoadInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function LoadInterface() As Integer

        Return LoadInterface(v_bDeleteClick:=False)

    End Function
    ''' <summary>
    ''' LoadInterface
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function LoadInterface(ByVal v_bDeleteClick As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        objfrmInterfaceUW = New frmInterfaceUW
        ' Assign the parameters to the interface properties.

        With objfrmInterfaceUW
            .FeeAmountID = m_lFeeAmountID
            .PartyCnt = m_lPartyCnt
            .Task = m_lTask
            .DeleteClick = v_bDeleteClick
            .UniqueId = m_sUniqueId
            .ScreenHierarchy = m_sScreenHierarchy
        End With

        ' Load the instance of the interface into memory.
        'commented as object created but never used
        'Dim tempLoadForm As frmInterfaceUW = frmInterfaceUW


        ' Check if we have had an error so far.

        If objfrmInterfaceUW.Error_Renamed Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.

        VB6.ShowForm(objfrmInterfaceUW, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.

            If objfrmInterfaceUW.Error_Renamed <> 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)



    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.

        With objfrmInterfaceUW
            m_lStatus = .Status
            '        m_sStepStatus$ = .StepStatus
            'Added 991008 MK
            '        If m_lStatus <> PMCancel Then
            ''            m_dFeePercentage = .FeePercentage
            ''            m_cFeeAmount = .FeeAmount
            ''            m_lProductType = .cboProduct.ItemData(.cboProduct.ListIndex)
            ''            m_sProductTypeDesc = .cboProduct
            ''            m_lTransactionTypeID = .TransactionType
            ''            m_sTransactionType = .cboTransType
            ''            m_lFeeAmountID = .FeeAmountID
            ''            m_nIsTaxable = .IsTaxable
            ''            m_lCurrencyID_Item = .CurrencyID
            ''            m_sCurrencyName_Item = .CurrencyName
            '        End If

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.

        objfrmInterfaceUW.Close()

        If Information.IsReference(objfrmInterfaceUW) Then


            objfrmInterfaceUW = Nothing
        End If


        Return result

    End Function
    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_lTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                End Select
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, 0)

            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

