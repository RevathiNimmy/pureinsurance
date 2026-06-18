Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no.129
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


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


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

    'Find Insurance Variables
    Private m_nInsuranceFilecnt As Integer
    Private m_sPolicyNumber As String = ""
    Private m_sPolicyCode As String = ""
    Private m_sRiskIndex As String = ""
    Private m_dtClaimDate As Object
    Private m_sShortName As String = ""
    Private m_sPostCode As String = ""
    Private m_dtFromDate As Object
    Private m_dtToDate As Object
    Private m_sPolicyHolder As String = ""
    Private m_sProductCode As String = ""

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    Private m_ofrmInterface As iSIRFindInsurance.frmInterface

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    Private m_vSourceArray As Object ' MKW 190503 PN2032

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.1)

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

            Return m_lStatus

        End Get
    End Property
    'DC020701 was Integer now Long
    'DC020701 was Integer now Long
    Public Property InsuranceFilecnt() As Integer
        Get

            Return m_nInsuranceFilecnt

        End Get
        Set(ByVal Value As Integer)

            m_nInsuranceFilecnt = Value

        End Set
    End Property

    Public Property PolicyNumber() As String
        Get

            Return m_sPolicyNumber

        End Get
        Set(ByVal Value As String)

            m_sPolicyNumber = Value

        End Set
    End Property

    Public Property PolicyHolder() As String
        Get

            Return m_sPolicyHolder

        End Get
        Set(ByVal Value As String)

            m_sPolicyHolder = Value

        End Set
    End Property

    Public Property RiskIndex() As String
        Get

            Return m_sRiskIndex


        End Get
        Set(ByVal Value As String)

            m_sRiskIndex = Value


        End Set
    End Property

    Public Property ProductCode() As String
        Get

            Return m_sProductCode

        End Get
        Set(ByVal Value As String)

            m_sProductCode = Value

        End Set
    End Property


    Public Property FromDate() As Object
        Get

            Return m_dtFromDate

        End Get
        Set(ByVal Value As Object)



            m_dtFromDate = Value

        End Set
    End Property
    Public Property ClaimDate() As Object
        Get

            'Return Claim  Date
            Return m_dtClaimDate

        End Get
        Set(ByVal Value As Object)

            'Set Claim  Date


            m_dtClaimDate = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date  :10/07/2000
    '
    ' Edit HIstory :Pandu
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

                g_iUserID = .UserID ' MKW 190503 PN2032 START
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            'Set g_oBusiness = New bBackOfficeLink.bBOLink

            '***************Change Made For Client/Server Model*******
            '********Author - Pandu Date :12-10-2000********************

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bBackOfficeLink.bBOLink", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            '***************Change Made For Client/Server Model*******
            '********Author - Pandu  Date :12-10-2000 ********************



            m_lSiriusUnderWritingBroking = g_oBusiness.Sirius_Product



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
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
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
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    'Set Insurance File cnt
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        'DC290601 was using CInt now using CLng

                        m_nInsuranceFilecnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        '*****************
                        ' MEvans : 12-03-2003 : Issue 2357
                        ' Bug Clng being used instead of CStr
                    Case PMNavKeyConst.PMKeyNamePolicyHolder

                        m_sPolicyHolder = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        '*****************

                        'Set Policy Number
                    Case PMNavKeyConst.PMKeyNamePolicyNumber

                        m_sPolicyNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'Set Risk Index
                    Case PMNavKeyConst.PMKeyNameRiskIndex

                        m_sRiskIndex = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'Set Product Code
                    Case PMNavKeyConst.PMKeyNameProductCode

                        m_sProductCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'Set Caim Loss From Date
                    Case PMNavKeyConst.PMKeyNameFromDate


                        m_dtFromDate = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

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
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.

            ReDim vKeyArray(1, 7)

            ' Assign the key array with the parameter members into navigator

            'vKeyArray(PMKeyName, 0) = PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePolicyID 'MSB091101 - Use this other constant instead

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_nInsuranceFilecnt

            'vKeyArray(PMKeyName, 1) = PMKeyNamePolicyHolder

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientHolder 'MSB091101 - Use this other constant instead

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sPolicyHolder


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNamePolicyNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sPolicyNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameRiskIndex

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sRiskIndex


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameProductCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sProductCode


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameFromDate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_dtFromDate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClaimDate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_dtClaimDate

            Return result

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.
            ' 200199 - Display appropriate heading according to InsFileType
            '    Select Case InsFileType
            '        Case SIRInsFileTypeQuote, SIRInsFileTypeMTAQuote
            '            vSummaryArray(PMNavSummHeading, 0) = "Quote Reference"
            '        Case SIRInsFileTypeMTAQuote
            '            vSummaryArray(PMNavSummHeading, 0) = "MTA Quote Reference"
            '        Case Else
            '            vSummaryArray(PMNavSummHeading, 0) = "Policy Reference"
            '    End Select
            '    vSummaryArray(PMNavSummValue, 0) = m_sInsReference$

            ' {* USER DEFINED CODE (End) *}

            Return result

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
    ' Date: 10/07/2000
    '
    ' Edit History :Pandu
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
            If g_oBusiness Is Nothing Then

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
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.2)
        Dim sValue As String = ""
        Const kMethodName As String = "Start"
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.2)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' MKW 190503 PN2032
            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Carry on without defaults set
            End If

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.2)

            ' Get System Option for Disable Wildcard Search
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bDisableWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = "1")
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.2)



            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'End If

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
    ' Date : 10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

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
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmInterface = New iSIRFindInsurance.frmInterface()

        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            'Set the Interface properties to Form properties

            ' MKW 190503 PN2032


            'Developer Guide no24
            .SourceArray = m_vSourceArray

            .PolicyNumber = m_sPolicyNumber
            .PolicyHolder = m_sPolicyHolder
            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.3)
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.5.1.3)



            .RiskIndex = m_sRiskIndex
            .ProductCode = m_sProductCode





            'Developer Guide no24
            .FromDate = m_dtFromDate
            .InsuranceFilecnt = m_nInsuranceFilecnt

        End With


        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface
            m_lStatus = .Status

            'Set the Properties of Interface from the corresponding
            'ones of the form
            m_sPolicyNumber = .PolicyNumber
            m_sPolicyHolder = .PolicyHolder



            m_sRiskIndex = .RiskIndex
            m_sProductCode = .ProductCode





            m_dtFromDate = .FromDate
            m_nInsuranceFilecnt = .InsuranceFilecnt


            m_dtClaimDate = .ClaimDate

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmInterface.Close()

        m_ofrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' Date :10/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_ofrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_ofrmInterface.ErrorNumber <> 0 Then
                result = m_ofrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function



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


    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    ' Added : MKW 190503 PN2032
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_g_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        '    ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse

            '        ' Display error stating the problem.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If


        m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If

        '    ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            g_oPMUser.Dispose()
            g_oPMUser = Nothing


        End If

        Return result

    End Function
End Class

