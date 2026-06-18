Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/09/00
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPMAuthorityLevel As Integer

    'TN20001128 (Start)

    Private m_oGetDocument As bCLMGetClaimDocument.Business
    'TN20001128 (End)

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_lClaimID As Integer
    Private m_lPartyCnt As Integer
    Private m_lProcessType As Integer
    Private m_lPolicycnt As Integer
    Private m_sDocDescription As Object
    Private m_bGenerateClaimDocument As Boolean
    Private m_iClaimWorkFlowId As Integer
    Private m_iDecisionResult As gPMConstants.PMEReturnCode
    Private m_sDocumentRef As String = ""

    'Start Arul PN56858
    Private m_sProcessTypeDocuments As String = ""
    Private Const ACSpooldocumentOrNot As Integer = 8
    Private Const ACEditableAfterMerging As Integer = 9
    Private Const ACProcessTypeDocuments As Integer = 10
    Private Const ACUserChoice As Integer = 15 ' added new option to handle User Choice
    'End Arul PN56858
    '(Start)-(Arul Stephen)-(Document Configuration)
    'Arul PN56858
    Private m_lUserChoice As Integer
    Private m_lIsEditableMerging As Integer
    Private Const kUserChoice As Integer = 8
    Private Const kEditableMerging As Double = 9.0#
    Private sTransationType As String = ""
    Private Const kYesNoButton As Integer = 1

    Private Const ACExpandedFormOfCJ As String = "Claim Jacket"
    Private Const ACExpandedFormOfCC As String = "Claim notification to Client"
    Private Const ACExpandedFormOfCA As String = "Claim notification to Agent"
    Private Const ACExpandedFormOfCI As String = "Claim notification to Insurer"
    Private Const ACExpandedFormOfEH As String = "External Handler Notification"
    Private Const ACExpandedFormOfLL As String = "Large Loss Advice"
    Private Const ACExpandedFormOfLA As String = "Loss Advice"
    Private Const ACExpandedFormOfAS As String = "Claim Advice to Agent"
    Private Const ACExpandedFormOfCQ As String = "Cheque Requisition"
    Private Const ACExpandedFormOfAF As String = "Claim Acceptance Form"
    Private Const ACExpandedFormOfNT As String = "Advice to Reinsurer"
    Private Const ACExpandedFormOfCP As String = "Claim Payment Advice"

    Private Const ACShortFormOfClaimAdviceToAgent As String = "AS"
    Private Const ACShortFormOfChequeRequisition As String = "CQ"
    Private Const ACShortFormOfClaimAcceptanceForm As String = "AF"
    Private Const ACShortFormOfAdviceToReinsurer As String = "NT"
    Private Const ACShortFormOfClaimPaymentAdvice As String = "CP"
    Private Const ACShortFormOfClaimJacket As String = "CJ"
    Private Const ACShortFormOfClaimNotificationToClient As String = "CC"
    Private Const ACShortFormOfClaimNotificationToAgent As String = "CA"
    Private Const ACShortFormOfClaimNotificationToInsurer As String = "CI"
    Private Const ACShortFormOfExternalHandlerNotification As String = "EN"
    Private Const ACShortFormOfLargeLossAdvice As String = "LL"
    Private Const ACShortFormOfLossAdvice As String = "LA"

    '(End)-(Arul Stephen)-(Document Configuration)

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iFuntionalArea As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
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
    ' {* USER DEFINED CODE (Begin) *}
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

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oGetDocument As Object
            If g_oObjectManager.GetInstance(temp_m_oGetDocument, "bCLMGetClaimDocument.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oGetDocument = temp_m_oGetDocument


                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                m_oGetDocument = temp_m_oGetDocument
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
                If m_oGetDocument IsNot Nothing Then
                    m_oGetDocument.Dispose()

                End If
                m_oGetDocument = Nothing
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

                ' {* USER DEFINED CODE (Begin) *}


                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameClaimID

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' AJM 16/02/01 - do not need to pass following parameters in
                        '            Case PMKeyNamePartyCnt
                        '                m_lPartyCnt = CLng(vKeyArray(PMKeyValue, lRow&))

                        '            Case "document_id"
                        '                 m_lProcessType = CLng(vKeyArray(PMKeyValue, lRow&))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId

                        m_iClaimWorkFlowId = gPMFunctions.ToSafeInteger(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameGenerateClaimDocument

                        m_bGenerateClaimDocument = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameDecisionResult

                        m_iDecisionResult = CType(gPMFunctions.ToSafeInteger(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))), gPMConstants.PMEReturnCode)
                    Case PMNavKeyConst.ACTKeyNameDocumentRef

                        m_sDocumentRef = gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

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

            ' {* USER DEFINED CODE (Begin) *}



            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

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

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessInterface() As Integer
        Dim nResult As Integer
        Dim sMessage As String = ""
        Dim oResultArray(,) As Object
        Dim oDocumentArray() As Object
        Dim cClaimLimit As Decimal
        Dim oSpoolArray(,) As Object
        Dim bMessageAdded As Boolean
        Dim bDocumentStatus As Boolean

        Dim o_ProductBusiness As bSIRProduct.Business



        nResult = gPMConstants.PMEReturnCode.PMTrue
        'Check the Decision
        If (m_iClaimWorkFlowId >= 1 AndAlso m_iClaimWorkFlowId <= 3 AndAlso Not m_bGenerateClaimDocument) OrElse m_iDecisionResult = gPMConstants.PMEReturnCode.PMCancel Then
            Return nResult
        End If

        ReDim oDocumentArray(0)
        ReDim m_sDocDescription(0)
        ' AJM 16/02/01 - get missing parameters for document production
        ' i.e. insured id & policy id

        m_lReturn = m_oGetDocument.GetClientAndPolicyID(m_lClaimID, oResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '------------------------------------------------------------------------------------------------------------
        'To get the value of document generated status if it 1, document has already been generated for this claim id
        'else it need to produce the document.(SAM Generate Claim Document)
        '------------------------------------------------------------------------------------------------------------
        'Get Docuemnt Generated Status

        bDocumentStatus = gPMFunctions.ToSafeBoolean(CStr(oResultArray(7, 0)))

        If bDocumentStatus Then
            MessageBox.Show("Document has already been generated for this claim ID:-" & m_lClaimID, Application.ProductName)
            Return nResult
        Else
            ' Get the client ID

            m_lPolicycnt = CInt(oResultArray(0, 0))

            ' Get the policy ID

            m_lPartyCnt = CInt(oResultArray(1, 0))

            ' Set document type required
            m_lProcessType = gSIRLibrary.SIRDocTypeClaims 'claim documents CLM

            'Check transaction type
            Select Case m_sTransactionType.Trim().Substring(m_sTransactionType.Trim().Length - 2)
                Case gSIRLibrary.SIRTransactionCodeClaimOpen, gSIRLibrary.SIRTransactionCodeClaimRevision

                    ' Check if we have a lead agent cnt, if not then must be direct business
                    'Start Renuka PN 61791
                    ' AGENT so notify agent Sumit K PM028570
                    If IsNumeric(oResultArray(2, 0)) Then

                        oDocumentArray(oDocumentArray.GetUpperBound(0)) = "CA"
                    Else
                        oDocumentArray(oDocumentArray.GetUpperBound(0)) = "CC"
                    End If

                    ' Now add transaction type for the claim jacket, always required

                    ReDim Preserve oDocumentArray(oDocumentArray.GetUpperBound(0) + 1)

                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "CJ"

                    ' Only remains for the reinsurance documentation to be checked.
                    ' A loss advice needs to be produced for reinsurers and if claim is for
                    ' over a specified amount then a large loss advice is produced instead
                    'AJM (20/07/2001) if '100% RETAINED' reinsurance, do not produce reinsurance documents
                    'If IsNumeric(vResultArray(3, 0)) Then

                    If CStr(oResultArray(3, 0)).TrimEnd() <> "" Then 'We have a reinsurer who is not RETAINED

                        'This means reinsurance records exist for claim on claim_party table
                        'other than 100% RETAINED

                        ReDim Preserve oDocumentArray(oDocumentArray.GetUpperBound(0) + 1)

                        ' We have reinsurance on the claim, now need to check if total
                        ' claim exceeds specified limit

                        Dim temp_o_ProductBusiness As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        o_ProductBusiness = temp_o_ProductBusiness

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("ProcessInterface", "Failed to Get bSIRProduct.Business")
                        End If


                        m_lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimID, r_cClaim_Value_For_Large_Loss_Advice:=cClaimLimit)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("ProcessInterface", "Failed to Get bSIRProduct.Business")
                        End If

                        o_ProductBusiness.Dispose()

                        Dim dbNumericTemp As Double
                        If Not Double.TryParse(CStr(cClaimLimit), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            cClaimLimit = 0
                        End If

                        If gPMFunctions.ToSafeCurrency(cClaimLimit) > 0 Then

                            If CStr(oResultArray(4, 0)) <> "" Then
                                'AJM 26/09/2001 - check if value is greater than or equal to
                                '                  not just greater than

                                If CInt(oResultArray(4, 0)) >= gPMFunctions.ToSafeCurrency(cClaimLimit) Then

                                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "LL"
                                Else

                                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "LA"
                                End If
                            Else

                                oDocumentArray(oDocumentArray.GetUpperBound(0)) = "LA"
                            End If
                        Else

                            oDocumentArray(oDocumentArray.GetUpperBound(0)) = "LA"
                        End If
                    End If

                Case gSIRLibrary.SIRTransactionCodeClaimPaid

                    ' Claim payment advice - always required

                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "CP"

                    ReDim Preserve oDocumentArray(oDocumentArray.GetUpperBound(0) + 1)

                    ' Cheque requisition form - always required

                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "CQ"

                    ReDim Preserve oDocumentArray(oDocumentArray.GetUpperBound(0) + 1)

                    ' Claim Acceptance form - always required

                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "AF"

                    ' Check if reinsurer payment advice is required
                    'AJM (20/07/2001) if '100% RETAINED' reinsurance, do not produce reinsurance documents
                    'If IsNumeric(vResultArray(3, 0)) Then

                    If CStr(oResultArray(3, 0)).TrimEnd() <> "" Then 'We a have reinsurer who is not RETAINED
                        'This means reinsurance records exist for claim on claim_party table

                        ReDim Preserve oDocumentArray(oDocumentArray.GetUpperBound(0) + 1)

                        oDocumentArray(oDocumentArray.GetUpperBound(0)) = "NT"
                    End If

                    ' Check if we have a lead agent cnt
                    ' AGENT so produce advice slip to agent

                    ReDim Preserve oDocumentArray(oDocumentArray.GetUpperBound(0) + 1)

                    oDocumentArray(oDocumentArray.GetUpperBound(0)) = "AS"

                Case Else
                    Return nResult

            End Select
        End If
        ' Now loop through the array producing a document for each required
        ' transaction type

        For i As Integer = 0 To oDocumentArray.GetUpperBound(0)

            m_sTransactionType = CStr(oDocumentArray(i))

            m_lReturn = GetDocument()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                    Return nResult
                End If
            End If

        Next

        If IsArray(m_sDocDescription) Then
            Dim ncount As Integer = 0
            ncount = m_sDocDescription.GetUpperBound(0)
            ReDim oSpoolArray(0, ncount)
            For istep As Integer = 0 To m_sDocDescription.GetUpperBound(0)
                oSpoolArray(0, istep) = RTrim(m_sDocDescription(istep))
            Next

        End If


        bMessageAdded = False

        m_lReturn = m_oGetDocument.GetClaimSpooledDesc(m_lClaimID, oSpoolArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        sMessage = "The following claim documents have been automatically spooled:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

        'Now build up the message to display the description of the documents that have been spooled
        If Information.IsArray(oSpoolArray) Then

            For i As Integer = 0 To oSpoolArray.GetUpperBound(1)
                If CStr(oSpoolArray(0, i)) <> "" Then
                    sMessage = sMessage & CStr(oSpoolArray(0, i)) & Strings.Chr(13) & Strings.Chr(10)
                    bMessageAdded = True
                End If
            Next

        End If

        If Not bMessageAdded Then
            sMessage = sMessage & "NONE"
        End If

        'Display message
        MessageBox.Show(sMessage, "Documents Spooled", MessageBoxButtons.OK)

        Return nResult

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDocument
    '
    ' Description:
    '
    ' History: 11/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocument() As Integer

        Dim result As Integer = 0
        Dim lDocId, lDocTypeId As Integer
        Dim iIsAgentCopy, iIsClientCopy, iIsOfficeCopy As Integer
        Dim lLeadAgentCnt As Integer
        Dim vDocumentArray(,) As Object
        Dim iCopies, lCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = GetTheTemplate(r_lDocumentTemplateId:=lDocId, r_lDocumentTypeId:=lDocTypeId, r_vDocumentArray:=vDocumentArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_sDocDescription = ""
            Return m_lReturn
        End If

        If IsArray(vDocumentArray) Then
            For lCount = 0 To vDocumentArray.GetUpperBound(1)
                If IsArray(vDocumentArray) Then ReDim Preserve m_sDocDescription(UBound(m_sDocDescription) + 1)

                If Information.IsArray(vDocumentArray) Then

                    lLeadAgentCnt = gPMFunctions.ToSafeLong(CStr(vDocumentArray(6, lCount)), 0)

                    iIsClientCopy = gPMFunctions.ToSafeInteger(CStr(vDocumentArray(2, lCount)))
                    'If there is not Lead Agent means its a direct business
                    'So agent copy should not be spooled
                    If lLeadAgentCnt > 0 Then

                        iIsAgentCopy = gPMFunctions.ToSafeInteger(CStr(vDocumentArray(3, lCount)), 0)
                    Else
                        iIsAgentCopy = 0
                    End If

                    iIsOfficeCopy = gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount)), 0)
                    iCopies = iIsClientCopy + iIsAgentCopy + iIsOfficeCopy
                End If


                If lDocId = 0 Then
                    Return result
                End If

                If iCopies > 0 Then
                    For iCopyCount As Integer = 1 To iCopies

                        m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong(CStr(vDocumentArray(0, lCount)), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong(CStr(vDocumentArray(1, lCount)), 0), r_lInsuranceFileCnt:=m_lPolicycnt, v_iIsClient:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(2, lCount))), v_iIsAgent:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(3, lCount))), v_iIsOffice:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount))), v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount))), v_sDocDescription:=ToSafeString(vDocumentArray(7, lCount)))
                        m_sDocDescription(UBound(m_sDocDescription)) = vDocumentArray(7, lCount)
                    Next iCopyCount
                Else

                    m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong(CStr(vDocumentArray(0, lCount)), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong(CStr(vDocumentArray(1, lCount)), 0), r_lInsuranceFileCnt:=m_lPolicycnt, v_iIsClient:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(2, lCount))), v_iIsAgent:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(3, lCount))), v_iIsOffice:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(4, lCount))), v_iProductionOrder:=gPMFunctions.ToSafeInteger(CStr(vDocumentArray(5, lCount))), v_sDocDescription:=ToSafeString(vDocumentArray(7, lCount)))
                    m_sDocDescription(UBound(m_sDocDescription)) = ToSafeString(vDocumentArray(7, lCount))
                End If
                'm_lReturn = UseTheTemplate(r_lDocId:=lDocId, r_lDocTypeId:=lDocTypeId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_sDocDescription = Nothing
                    Return m_lReturn
                End If
            Next
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTheTemplate
    '
    ' Description:
    '
    ' History: 26/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GetTheTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer, ByRef r_vDocumentArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim obPMBDocLink As bPMBDocLink.Business
        Dim dtEffectiveDate As Date

        'Initialize object
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim temp_obPMBDocLink As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_obPMBDocLink, "bPMBDocLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        obPMBDocLink = temp_obPMBDocLink


        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Doc Link object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTheTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        'Get Values from
        'For time being funtional area is set to 1 i.e. document linking for policy
        'Start Renuka PN 61791
        If m_sTransactionType = "CP" Or m_sTransactionType = "CQ" Or m_sTransactionType = "AF" Or m_sTransactionType = "NT" Or m_sTransactionType = "AS" Then
            'End Renuka PN 61791
            m_iFuntionalArea = 4 'Payment of Claim
        Else
            m_iFuntionalArea = 2 ' Open/Maintain claim
        End If


        m_lReturn = m_oGetDocument.GetEffectiveDate(v_lClaimID:=m_lClaimID, r_dtEffectiveDate:=dtEffectiveDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get GetEffectiveDate ", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTheTemplate")
        End If


        m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=m_iFuntionalArea, v_lInsurance_File_Cnt:=m_lPolicycnt, v_lProcessType_Docs_ID:=m_lProcessType, v_lProcess_Type_Code:=m_sTransactionType, v_dtEffectiveDate:=dtEffectiveDate, r_vResultarray:=r_vDocumentArray)



        '    'Returning the values
        If Information.IsArray(r_vDocumentArray) Then

            r_lDocumentTemplateId = CInt(r_vDocumentArray(0, 0))

            r_lDocumentTypeId = CInt(r_vDocumentArray(1, 0))

            'Start-(Arul Stephen)-(Document Configuration)
            'Start Arul PN56858

            m_lUserChoice = CInt(r_vDocumentArray(ACSpooldocumentOrNot, 0))

            m_lIsEditableMerging = CInt(r_vDocumentArray(ACEditableAfterMerging, 0))

            m_sProcessTypeDocuments = CStr(r_vDocumentArray(ACProcessTypeDocuments, 0))
            'End Arul PN56858
            'End-(Arul Stephen)-(Document Configuration)
        End If

        '
        'Terminate the object
        If Not (obPMBDocLink Is Nothing) Then
            ' Terminate the business object

            obPMBDocLink.Dispose()

           

            ' Destroy the instance of the business object
            ' from memory.
            obPMBDocLink = Nothing

        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTheTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTheTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UseTheTemplate
    '
    ' Description:
    '
    ' History: 27/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function UseTheTemplate(ByRef r_lDocId As Integer, ByRef r_lDocTypeId As Integer, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByVal v_iIsClient As Integer = 0, Optional ByVal v_iIsAgent As Integer = 0, Optional ByVal v_iIsOffice As Integer = 0, Optional ByVal v_iProductionOrder As Integer = 1, Optional ByVal v_sDocDescription As String = "") As Integer
        Dim result As Integer = 0

        Dim oObject As iPMBDocTemplate.Interface_Renamed



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If


        oObject.PartyCnt = m_lPartyCnt

        oObject.ClaimCnt = m_lClaimID

        oObject.InsuranceFileCnt = m_lPolicycnt

        oObject.DocumentTemplateId = r_lDocId

        oObject.DocumentTypeId = r_lDocTypeId

        oObject.DocumentRef = m_sDocumentRef
        'AJM 08/03/01 - silent document production, spool documents straight away and
        '               give document description

        oObject.SpoolDesc = v_sDocDescription.TrimEnd()

        'Start-(Arul Stephen)-(Document Configuration)
        If m_lUserChoice = 2 Then

            oObject.Mode = ACUserChoice
        Else
            If m_lUserChoice = 0 Then

                oObject.Mode = gSIRLibrary.ACPrintSilentMode
            Else

                oObject.Mode = gSIRLibrary.ACSpoolDocMode
            End If
        End If
        'End-(Arul Stephen)-(Document Configuration)

        oObject.IsClient = v_iIsClient

        oObject.IsAgent = v_iIsAgent

        oObject.IsOffice = v_iIsOffice

        oObject.ProductionOrder = v_iProductionOrder

        sTransationType = FindTransactionType(m_sTransactionType.Trim())

        oObject.CalledFromGetDocument = True 'PN 66227



        If oObject.Mode = ACUserChoice Or (oObject.Mode = ACSpoolDocMode And m_lIsEditableMerging) Then
            m_lReturn = MessageBox.Show("Do you want to produce " & sTransationType & " " & m_sProcessTypeDocuments & " template?", sTransationType, MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If m_lReturn = kYesNoButton Then

                m_lReturn = oObject.Start()
            Else

                oObject.Dispose()
              

                oObject = Nothing
                Return result
            End If
        Else

            m_lReturn = oObject.Start()
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If


        oObject.Dispose()

       

        oObject = Nothing

        Return result

    End Function
    'Start-(Arul Stephen)-(Document Configuration)
    'Note:Old way of error handling is used since the new way of try catch cannot be applied since this function returns string
    Private Function FindTransactionType(ByRef m_lTransacationType As String) As String

        Dim result As String = String.Empty

        Select Case m_lTransacationType
            Case ACShortFormOfClaimJacket
                result = ACExpandedFormOfCJ
            Case ACShortFormOfClaimNotificationToAgent
                result = ACExpandedFormOfCA
            Case ACShortFormOfClaimNotificationToClient
                result = ACExpandedFormOfCC
            Case ACShortFormOfClaimNotificationToInsurer
                result = ACExpandedFormOfCI
            Case ACShortFormOfExternalHandlerNotification
                result = ACExpandedFormOfEH
            Case ACShortFormOfLargeLossAdvice
                result = ACExpandedFormOfLL
            Case ACShortFormOfLossAdvice
                result = ACExpandedFormOfLA
            Case ACShortFormOfClaimAdviceToAgent
                result = ACExpandedFormOfAS
            Case ACShortFormOfAdviceToReinsurer
                result = ACExpandedFormOfNT
            Case ACShortFormOfClaimAcceptanceForm
                result = ACExpandedFormOfAF
            Case ACShortFormOfChequeRequisition
                result = ACExpandedFormOfCQ
            Case ACShortFormOfClaimPaymentAdvice
                result = ACExpandedFormOfCP
            Case Else
                result = ""
        End Select



        Return result

    End Function
    'End-(Arul Stephen)-(Document Configuration)

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
