Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
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
    ' Date: 07/10/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: TF071098 - Created from iFindInsurance
    ' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document

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

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lInsFileCnt As Integer
    Private m_sInsReference As String = ""
    Private m_lInsHolderCnt As Integer
    Private m_sShortName As String = "" 'JW290498
    Private m_sLongName As String = ""
    Private m_lInsuranceFolderCnt As Integer 'TF100398
    'TF211298
    Private m_lPartyUIK As Integer
    Private m_lPolicyUIK As Integer
    Private m_lLeadAgentCnt As Integer

    Private m_sRegistration As String = "" 'TF031298
    'Private m_lProductID As Long

    ' TF311298 - chenged from NavProcessCode
    Private m_sInsFileType As String = ""

    Private m_lFindMode As Integer
    Private m_sRunMode As String = "" ' RAM20040226 : PN Issue 10592

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'SJ 22/04/2004 - start
    ' ShowLapsedOnly
    Private m_bShowLapsedOnly As Boolean
    'SJ 22/04/2004 - end

    ' Alix
    Private m_bIncludeClosedBranches As Boolean
    'developer guide no. 51
    Dim objfrmInterface As frmInterface
    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.1)


    Public WriteOnly Property IncludeClosedBranches() As Boolean
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranches = Value
        End Set
    End Property

    'SJ 22/04/2004 - start
    Public WriteOnly Property ShowLapsedOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowLapsedOnly = Value
        End Set
    End Property
    'SJ 22/04/2004 - end

    ' PRIVATE Data Members (End)

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

    ' {* USER DEFINED CODE (Begin) *}

    Public Property InsFileType() As String
        Get

            ' Return the Insurance File Type.
            Return m_sInsFileType

        End Get
        Set(ByVal Value As String)

            ' Return the Insurance File Type.
            m_sInsFileType = Value

        End Set
    End Property

    Public Property InsFileCnt() As Integer
        Get

            Return m_lInsFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsFileCnt = Value

        End Set
    End Property

    Public Property InsReference() As String
        Get

            Return m_sInsReference

        End Get
        Set(ByVal Value As String)

            m_sInsReference = Value

        End Set
    End Property

    Public Property InsHolderCnt() As Integer
        Get

            Return m_lInsHolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsHolderCnt = Value

        End Set
    End Property

    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property

    Public Property LongName() As String
        Get

            Return m_sLongName

        End Get
        Set(ByVal Value As String)

            m_sLongName = Value

        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value

        End Set
    End Property

    Public Property VehicleRegistration() As String
        Get

            Return m_sRegistration

        End Get
        Set(ByVal Value As String)

            m_sRegistration = Value

        End Set
    End Property

    Public Property FindMode() As Integer
        Get
            Return m_lFindMode
        End Get
        Set(ByVal Value As Integer)
            m_lFindMode = Value
        End Set
    End Property

    'Public Property Get ProductID() As Long
    '
    '    ProductID = m_lProductID&
    '
    'End Property
    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                MainModule.g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With MainModule.g_oObjectManager
                MainModule.g_iLanguageID = .LanguageID
                MainModule.g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Insurance Holder Count in case it isn't set
            m_lInsHolderCnt = 0

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindInsurance.Form", "ClientManager")
            MainModule.g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFailTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(MainModule.g_iLanguageID, MainModule.ACBusinessFail, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If


            g_bPMGeminiLink = g_oBusiness.GeminiLink

            '-----------------------------------------
            ' ED 05082002 : Is Registration Search Activated

            g_bRegSearch = g_oBusiness.RegSearch

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
                If Not (MainModule.g_oBusiness Is Nothing) Then
                    ' Terminate the business object

                    MainModule.g_oBusiness.Dispose()
                    MainModule.g_oBusiness = Nothing
                End If
                If MainModule.g_oObjectManager IsNot Nothing Then
                    MainModule.g_oObjectManager.Dispose()
                    MainModule.g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    ' Edit History  :
    ' RAM20040226   : Added the PMKeyNameRunMode Key (Set from uctListEventControl.ocx)
    '                  Ref. PN Issue 10592
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

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsReference

                        m_sInsReference = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lInsHolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameShortName
                        m_sShortName = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case MainModule.PMKeyNameVehicleRegNo
                        m_sRegistration = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case PMNavKeyConst.PMKeyNameInsFileType

                        m_sInsFileType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameRunMode

                        m_sRunMode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) '
                End Select

                ' {* USER DEFINED CODE (End) *}
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
            ReDim vKeyArray(1, 10)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsHolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFolderCnt
            '    vKeyArray(PMKeyName, 3) = PMKeyNameProductID
            '    vKeyArray(PMKeyValue, 3) = m_lProductID&

            'TF211298 - Pass back Insurance Holder keys

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameClientCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lInsHolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameClientUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lPartyUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClientName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_sLongName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNamePolicyUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lPolicyUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameInsReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_sInsReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNamePolicyNo

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_sInsReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameAgentCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lLeadAgentCnt

            ' {* USER DEFINED CODE (End) *}

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
            Select Case InsFileType
                Case gSIRLibrary.SIRInsFileTypeQuote, gSIRLibrary.SIRInsFileTypeMTAQuote

                    vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Quote Reference"
                Case gSIRLibrary.SIRInsFileTypeMTAQuote

                    vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "MTA Quote Reference"
                Case Else

                    vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Policy Reference"
            End Select

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sInsReference

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
            If MainModule.g_oBusiness Is Nothing Then

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
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim vInsuranceFileCnt As Integer
        Dim vPolicyHolderCnt As Integer
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.2)
        Dim sValue As String = ""
        Const kMethodName As String = "Start"
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.2)
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.2)
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
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.2)


            ' Populate search fields from supplied IDs
            If m_lInsFileCnt > 0 Then
                vInsuranceFileCnt = m_lInsFileCnt
            End If
            If m_lInsHolderCnt > 0 Then
                vPolicyHolderCnt = m_lInsHolderCnt
            End If


            m_lReturn = g_oBusiness.SetDefaultSearchFields(r_sInsRef:=m_sInsReference, r_sShortName:=m_sShortName, v_lInsuranceFileCnt:=vInsuranceFileCnt, v_lInsuranceHolderCnt:=vPolicyHolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Carry on without defaults set
            End If

            ' Check if the InsuranceFileCnt is greater than zero. If
            ' so, there is no need to proceed with the interface. We
            ' can therefore return straight back out.
            If m_lInsFileCnt > 0 Then
                ' ID is greater than zero.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

            Else
                ' ID is not greater than zero.

                ' Starts the interface processing.
                m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Check if the interface is already open
                    If m_lReturn = gPMConstants.PMEReturnCode.PMMoveStatusBack Then
                        result = 400
                    Else
                        ' Failed to process the interface.
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
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
    ' Name: GetID (Standard Method)
    '
    ' Description: Gets the ID for the search parameter from the
    '              business object.
    '
    ' ***************************************************************** '
    Public Function GetID() As Integer

        Dim result As Integer = 0

        Try


            ' Get the ID from the busines object.
            ' TF031298 - This function does not exist on Business
            '    m_lReturn& = g_oBusiness.GetID( _
            'vSearch:=CVar(m_sInsReference$), _
            'vID:=CVar(m_lInsFileCnt&))

            ' Check for errors
            '    If (m_lReturn& = PMFalse Or _
            ''    m_lReturn& = PMError) Then
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to get the ID from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetID"
            '    End If

            ' Return the value.
            '    GetID = m_lReturn&

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0

        objfrmInterface = New frmInterface


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
            'Check if the interface is already open
            If m_lReturn = gPMConstants.PMEReturnCode.PMMoveStatusBack Then
                Return 400
            Else
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'developer guide no. 51

        m_sShortName = objfrmInterface.ShortName
        m_sLongName = objfrmInterface.LongName

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
    ' Edit History  :
    ' RAM20040226   : PN Issue 10592 Changes
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        'developer guide no. 51
        With objfrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}

            .InsReference = m_sInsReference
            .ShortName = m_sShortName
            .VehicleRegistration = m_sRegistration
            .InsFileType = m_sInsFileType
            .FindMode = m_lFindMode
            .InsHolderCnt = m_lInsHolderCnt ' RAM20040226 : Set the PartyCnt if we have
            .RunMode = m_sRunMode ' RAM20040226 : Set the RunMode
            'SJ 22/04/2004 - start
            .ShowLapsedOnly = m_bShowLapsedOnly
            'SJ 22/04/2004 - end
            ' {* USER DEFINED CODE (End) *}
            .IncludeClosedBranches = m_bIncludeClosedBranches

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.3)
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.4.1.3)
        End With

        ' Load the instance of the interface into memory.
        'developer guide no.  commented refer guide no. 51 
        'Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        'developer guide no. 51 
        If objfrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = objfrmInterface.ErrorNumber
        End If

        Return result

    End Function

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
        'developer guide no. 51
        With objfrmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}
            m_lInsFileCnt = .InsFileCnt
            m_sInsReference = .InsReference
            '        m_lProductID& = .ProductID
            m_lInsHolderCnt = .InsHolderCnt
            m_lInsuranceFolderCnt = .InsuranceFolderCnt
            m_lPartyUIK = .PartyUIK
            m_sLongName = .LongName
            m_sShortName = .ShortName
            m_lPolicyUIK = .PolicyUIK
            m_lLeadAgentCnt = .LeadAgentCnt
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        'developer guide no. 51

        objfrmInterface.Close()
        objfrmInterface = Nothing


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
        'Modified by Sumeet Singh on 6/1/2010 6:49:48 PM commented the code because it has to be global so that the objfrmInterface object is available to all functions.
        'objfrmInterface = New frmInterface
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For Each frm As Object In Application.OpenForms
                If Not frm Is Nothing Then
                    If frm.Text = "Find: Insurance File" OrElse frm.GetType().ToString() = objfrmInterface.GetType().ToString() Then
                        Return 400
                    End If
                End If
            Next
            ' Display the interface.
            ' developer guide no. 51
            VB6.ShowForm(objfrmInterface, lDisplayState)

            If lDisplayState = FormShowConstants.Modal Then
                ' Check for any form errors.
                'developer guide no. 51
                If objfrmInterface.ErrorNumber <> 0 Then
                    result = objfrmInterface.ErrorNumber
                End If
            End If

            Return result
        Catch excep As System.Exception
            'Check if the interface is already open
            If Information.Err().Number = 400 Then
                Return 400
            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to display the interface", MainModule.ACApp, ACClass, "ShowInterface", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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