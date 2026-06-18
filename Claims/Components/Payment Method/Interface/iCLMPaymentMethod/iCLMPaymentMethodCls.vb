Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/07/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer


    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'Variables For Payment/Receipt Method
    Private m_lPartyid As Integer
    Private m_lScreenMethod As Integer
    Private m_sPartyName As String = ""
    Private m_sComments As String = ""
    Private m_lButtonClicked As Integer
    Private m_cAmount As Decimal

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
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

    ' RDC 15062004
    Private m_lInsuranceFileCnt As Integer
    Private m_sClaimRef As String = ""
    Private m_sPolicyNumber As String = ""
    Private m_iCurrencyID As Integer
    Private m_lClaimID As Integer

    Private m_iLossCurrencyID As Integer
    Private m_cLossCurrencyAmount As Decimal

    'JAS20050113 - PN18034
    Private m_lClaimPerilID As Integer
    Private m_lRiskTypeId As Integer
    Private m_bFromNavigator As Boolean
    Private m_bAuthoriseMode As Boolean
    Private m_lSequenceNo As Integer
    Private m_lClaimPaymentID As Integer 'eck 11/2005
    Private m_sScreenType As String = ""
    Private frmInterface As frmInterface

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

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)

    Public Property Amount() As Decimal
        Get

            Return m_cAmount

        End Get
        Set(ByVal Value As Decimal)

            m_cAmount = Value

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

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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



    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :15/09/2000
    '
    ' Edit History: Pandu
    ' ***************************************************************** '

    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

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
                g_lCountryID = .CountryID
                ' RDC 16062004
                g_iUserID = .UserID
            End With


            '    m_lClaimID = v_lClaimID
            '        ClaimId = v_lClaimID

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMPaymentMethod.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness



            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set variable to show we are coming from a navigator and not iCLMPeril.
            m_bFromNavigator = True

            'Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)


                Select Case CStr(vKeyArray(0, lRow)).ToLower()
                    Case "claim_ref"

                        m_sClaimRef = CStr(vKeyArray(1, lRow))

                    Case "policy_number"

                        m_sPolicyNumber = CStr(vKeyArray(1, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimID

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameClaimPerilID

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lClaimPerilID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameClaimPaymentSequence

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lSequenceNo = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameClaimPaymentAuthoriseMode

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_bAuthoriseMode = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lPartyid = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameClaimReference

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sClaimRef = (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                        End If

                    Case PMNavKeyConst.PMKeyNameRiskTypeID
                        'JAS2   0050113 - PN18034

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lRiskTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameClaimPaymentId
                        'eck 11/2005

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lClaimPaymentID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameScreenType.ToLower()

                        m_sScreenType = gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                End Select

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

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
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        'Dim lRow As Long
        '
        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
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

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
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
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Carry on without defaults set
            End If

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()


        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

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

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    '***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 69
        frmInterface = New frmInterface
        ' THIS FUNCTION IS SHARED BY UNDERWRITING AND BROKING
        ' PLEASE BE CAREFUL WHEN ALTERING IT

        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .ScreenMethod = m_lScreenMethod
            .CurrencyID = m_iCurrencyID
            .Amount = m_cAmount
            .Comments = m_sComments

            'It is also reqd. in case of Broking in GetClaimCompany of the interface PN 17761
            .InsuranceFileCnt = m_lInsuranceFileCnt

            .ClientID = m_lClientID
            .ClientName = m_sClientName

            .LossCurrencyID = m_iLossCurrencyID
            .LossCurrencyAmount = m_cLossCurrencyAmount

            .ClaimID = m_lClaimID

            .AgentID = m_lAgentID
            .AgentName = m_sAgentName
            .ProductID = m_lProductID
            .AuthoriseMode = m_bAuthoriseMode
            .FromNavigator = True
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmInterface

            m_lStatus = .Status
            m_lPartyid = .Partyid
            m_sPartyName = .PartyName
            m_cAmount = .Amount
            m_sComments = .Comments
            m_lButtonClicked = .ButtonClicked
            m_lPayeeMediaType = .PayeeMediaType
            m_sPayeeName = .PayeeName
            m_sPayeeBankName = .PayeeBankName
            m_sPayeeSortCode = .PayeeSortCode
            m_sPayeeAccountNo = .PayeeAccountNo
            m_lPayeeCountry = .PayeeCountry
            m_sPayeeComments = .PayeeComments


        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' Date :15/07/2000
    '
    ' Edit History :SK
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

