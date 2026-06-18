Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No 129. 
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
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

    ' PW170702 - Declare an instance of the Business object.

    Private m_oBusiness As bSIRGetDocument.Business

    'TN20001128 (Start)

    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    'TN20001128 (End)

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer  'MSB301001
    Private m_lPartyCnt As Integer
    Private m_lAgentCnt As Integer
    Private m_lProcessType As Integer
    Private m_lProductid As Integer
    Private m_sShortCode As String = ""
    Private m_bFormlessDocumentPrint As Boolean
    Private m_bIsNonBatchProcess As Boolean
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iFuntionalArea As Integer
    Private m_bSilentMode As Boolean
    Private m_iPrintMode As Integer
    Private m_sDocumentDescription As String = ""

    Private m_sProcessTypeDocuments As String = ""
    Private Const ACSpooldocumentOrNot As Integer = 8
    Private Const ACEditableAfterMerging As Integer = 9
    Private Const ACProcessTypeDocuments As Integer = 10

    Private m_lUserChoice As Integer
    Private m_lIsEditableMerging As Integer
    Private sTransactionType As String = ""
    Private Const ACUserChoice As Integer = 15  ''to support User Choice

    Private Const ACShortFormOfNewBusiness As String = "NB"
    Private Const ACShortFormOfAdditionalPremium As String = "AP"
    Private Const ACShortFormOfReturnPremium As String = "RP"
    Private Const ACShortFormOfZeroPremium As String = "ZP"
    Private Const ACShortFormOfReInstatement As String = "RI"
    Private Const ACShortFormOfRenewalInvite As String = "RNI"
    Private Const ACShortFormOfRenewalAccept As String = "RN"

    Private Const ACExpandedFormOfNB As String = "New Business"
    Private Const ACExpandedFormOfAP As String = "MTA Additional Premium"
    Private Const ACExpandedFormOfRP As String = "MTA Return Premium"
    Private Const ACExpandedFormOfZP As String = "MTA Zero Premium"
    Private Const ACExpandedFormOfRI As String = "MTA Reinstatement"
    Private Const ACExpandedFormOfRNI As String = "Renewal Invite"
    Private Const ACExpandedFormOfRN As String = "Renewal Accept"
    Private Const kYesNoButton As Integer = 1

    Private Const kDocumentTemplateId As Integer = 0
    Private Const kDocumentTypeId As Integer = 1
    Private Const kDocumentTemplateDesc As Integer = 3
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

    Public WriteOnly Property FunctionalArea() As Integer
        Set(ByVal Value As Integer)

            m_iFuntionalArea = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property PrintMode() As Integer
        Set(ByVal Value As Integer)

            m_iPrintMode = Value

        End Set
    End Property
    Public WriteOnly Property DocumentDescription() As String
        Set(ByVal Value As String)

            m_sDocumentDescription = Value

        End Set
    End Property

    Public WriteOnly Property SlientMode() As Integer
        Set(ByVal Value As Integer)

            m_bSilentMode = Value

        End Set
    End Property

    Public WriteOnly Property IsNonBatchProcess() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsNonBatchProcess = Value

        End Set
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
            Dim temp_m_oInsuranceFile As Object
            If g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oInsuranceFile = temp_m_oInsuranceFile


                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                m_oInsuranceFile = temp_m_oInsuranceFile
            End If

            ' PW170702 - Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRGetDocument.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW170702 - Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oInsuranceFile IsNot Nothing Then
                    m_oInsuranceFile.Dispose()
                    m_oInsuranceFile = Nothing
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


                'Developer Guide No 149
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'MSB301001
                    Case PMNavKeyConst.PMKeyNameInsFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyCnt


                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "document_id"

                        m_lProcessType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameProductID

                        m_lProductid = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameShortCode

                        m_sShortCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeynameFormlessInterface
                        m_bFormlessDocumentPrint = Convert.ToBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))


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
        Dim m_oProductBusiness As bSIRProduct.Business
        Dim vIsProduced As Object  'Declare new variable
        Dim bIsproduced As Boolean


        'Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Default status to OK
        m_lStatus = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_m_oProductBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oProductBusiness = temp_m_oProductBusiness

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

            Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lProductid = 0 Then

                m_lReturn = m_oProductBusiness.GetProductid(m_lInsuranceFileCnt, m_lProductid)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrive the function from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oProductBusiness.CheckIfProduced(v_lProductId:=m_lProductid, r_vProducedArray:=vIsProduced)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrive the function from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Dim bIsProduceClauseDocument As Boolean = True
        If Information.IsArray(vIsProduced) Then
            'Retrieve the value from the array on the basis of above condition.
            If m_sShortCode.ToLower() = "schedule" Then

                bIsproduced = CBool(vIsProduced.GetValue(0, 0))
                bIsProduceClauseDocument = True
            ElseIf m_sShortCode.ToLower() = "certificate" Then
                bIsproduced = CBool(vIsProduced.GetValue(1, 0))
            ElseIf m_sShortCode.ToLower() = "debitnote" Then
                bIsproduced = CBool(vIsProduced.GetValue(2, 0))
            ElseIf m_sShortCode.ToLower() = "" Or m_sShortCode.ToLower() = "reinstate" Then
                bIsproduced = True
                If (m_sTransactionType = "RNI" Or m_sTransactionType = "RN") AndAlso CBool(vIsProduced.GetValue(0, 0)) Then
                    bIsProduceClauseDocument = True
                End If
            End If
        End If

        ' Starts the interface processing.
        If bIsproduced Then
            'Process interface if bIsproduced is 1 else skip this Step
            m_lReturn = ProcessInterface(bIsProduceClauseDocument)
        Else
            'Prohibit the generation of the documents and move to next Step
            result = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

        'Catch excep As System.Exception



        '    result = gPMConstants.PMEReturnCode.PMError

        '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '    Return result

        'End Try
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
    Private Function ProcessInterface(Optional ByVal v_bProduceClauseDocument As Boolean = False) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        'In case of Renewal Invite and Renewal Accept process type has already been set
        ' PN 75635
        If GetTransactionType(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_sTransactionType:=m_sTransactionType, r_dtEffectiveDate:=m_dtEffectiveDate) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = GetDocument(v_bProduceClauseDocument)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
            Return result
        End If

        m_lReturn = UNLOCKPOLICY()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

        'Catch excep As System.Exception




        '    result = gPMConstants.PMEReturnCode.PMError

        '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '    Return result

        'End Try
    End Function

    '******************************************************************
    ' Name : GetTransactionType
    '
    ' Desc : get transaction type back depending on insurance file type
    '
    ' Hist : 28/11/2000 Tinny - Created
    '******************************************************************
    Private Function GetTransactionType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sTransactionType As String, ByRef r_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Dim sInsuranceFileType As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue


        m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt


        If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sInsuranceFileType = m_oInsuranceFile.InsuranceFileType

        'In case of Renewal Invite and Renewal Accept process type has already been set
        If UCase$(m_sTransactionType) = "RN" Or UCase$(m_sTransactionType) = "RNI" Then
            r_dtEffectiveDate = m_oInsuranceFile.InceptionTPI
            Return result
        End If

        Select Case sInsuranceFileType.Trim().ToUpper()
            Case "RENEWAL"
                If r_sTransactionType <> "RNI" Then
                    r_sTransactionType = "RN"  'renewal
                End If

                r_dtEffectiveDate = m_oInsuranceFile.InceptionTPI
            Case "QUOTE", "POLICY"
                r_sTransactionType = "NB"  'new business

                r_dtEffectiveDate = m_oInsuranceFile.CoverStartDate
            Case "MTAQUOTE", "MTA PERM", "MTA TEMP", "MTAQTETEMP", "MTACAN"

                If m_oInsuranceFile.ThisPremium > 0 Then
                    r_sTransactionType = "AP"  'additional premium
                ElseIf m_oInsuranceFile.ThisPremium < 0 Then
                    r_sTransactionType = "RP"  'return premium
                Else
                    r_sTransactionType = "ZP"  'zero premium
                End If

                r_dtEffectiveDate = m_oInsuranceFile.InceptionTPI
            Case "MTAREINS", "MTAQREINS"
                r_sTransactionType = "RI"

                r_dtEffectiveDate = m_oInsuranceFile.InceptionDate
                'StartWritten Status
            Case "WRITTEN"
                If r_sTransactionType <> "WRN" Then
                    r_sTransactionType = "WNB"
                End If

                r_dtEffectiveDate = m_oInsuranceFile.CoverStartDate
                'End- Written Status-
        End Select

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDocument
    '
    ' Description:
    '
    ' History: 11/09/2000 Tomo - Created.
    '        : PW170702 - check if the document is suppressed through
    '                      the agent
    '
    ' ***************************************************************** '
    Private Function GetDocument(Optional ByVal v_bProduceClauseDocument As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lDocId, lDocTypeId As Integer
        Dim bSuppressed As Boolean
        Dim vDocumentArray(,) As Object

        Dim bDocSpooled As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = m_oBusiness.CheckIfSuppressed(lProcessType:=m_lProcessType, lInsuranceFileCnt:=m_lInsuranceFileCnt, bSuppressed:=bSuppressed)

        If bSuppressed Then
            Return result
        End If


        m_lReturn = GetTheTemplate(r_lDocumentTemplateId:=lDocId, r_lDocumentTypeId:=lDocTypeId, r_vDocumentArray:=vDocumentArray)

        If Information.IsArray(vDocumentArray) Then

            For lCount As Integer = vDocumentArray.GetLowerBound(1) To vDocumentArray.GetUpperBound(1)
                bDocSpooled = False

                m_lUserChoice = ToSafeInteger(vDocumentArray(ACSpooldocumentOrNot, lCount))

                m_lIsEditableMerging = (vDocumentArray(ACEditableAfterMerging, lCount))

                m_sProcessTypeDocuments = ToSafeString(vDocumentArray(ACProcessTypeDocuments, lCount))


                If gPMFunctions.ToSafeInteger((vDocumentArray(2, lCount)), 0) = 1 Then



                    m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong((vDocumentArray(0, lCount)), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong((vDocumentArray(1, lCount)), 0), r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iIsClient:=gPMFunctions.ToSafeInteger((vDocumentArray(2, lCount))), v_iProductionOrder:=gPMFunctions.ToSafeInteger((vDocumentArray(5, lCount))), v_sSpoolDesc:=CStr(vDocumentArray(7, lCount)), v_lPartyCnt:=m_lPartyCnt)
                    bDocSpooled = True

                End If

                'If there is not Lead Agent means its a direct business
                'So agent copy should not be spooled


                If gPMFunctions.ToSafeInteger((vDocumentArray(3, lCount)), 0) = 1 And gPMFunctions.ToSafeLong((vDocumentArray(6, lCount)), 0) > 0 Then



                    m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong((vDocumentArray(0, lCount)), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong((vDocumentArray(1, lCount)), 0), r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iIsAgent:=gPMFunctions.ToSafeInteger((vDocumentArray(3, lCount))), v_iProductionOrder:=gPMFunctions.ToSafeInteger((vDocumentArray(5, lCount))), v_sSpoolDesc:=CStr(vDocumentArray(7, lCount)), v_lPartyCnt:=ToSafeLong(vDocumentArray(6, lCount), 0))
                    bDocSpooled = True
                End If


                If gPMFunctions.ToSafeInteger((vDocumentArray(4, lCount)), 0) = 1 Then



                    m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong((vDocumentArray(0, lCount)), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong((vDocumentArray(1, lCount)), 0), r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iIsOffice:=gPMFunctions.ToSafeInteger((vDocumentArray(4, lCount))), v_iProductionOrder:=gPMFunctions.ToSafeInteger((vDocumentArray(5, lCount))), v_sSpoolDesc:=CStr(vDocumentArray(7, lCount)), v_lPartyCnt:=m_lPartyCnt)
                    bDocSpooled = True
                End If

                If Not bDocSpooled Then


                    m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong((vDocumentArray(0, lCount)), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong((vDocumentArray(1, lCount)), 0), r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iProductionOrder:=gPMFunctions.ToSafeInteger((vDocumentArray(5, lCount))), v_sSpoolDesc:=CStr(vDocumentArray(7, lCount)), v_lPartyCnt:=m_lPartyCnt)

                End If

            Next lCount
        Else
            If lDocId = 0 Then
                Return result
            End If

            m_lReturn = UseTheTemplate(r_lDocId:=lDocId, r_lDocTypeId:=lDocTypeId, r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPartyCnt:=m_lPartyCnt)
        End If

        If m_lInsuranceFileCnt <> 0 AndAlso v_bProduceClauseDocument AndAlso (m_sProcessTypeDocuments.ToUpper = "SCHEDULE" _
                                                                                  OrElse m_sProcessTypeDocuments.ToUpper = "RENEWAL NOTICE") Then
            Dim oDoNotMergeClauseArray As Object = Nothing
            Dim nResultDoNotMergeDocument As Integer = 0
            m_oInsuranceFile.InsuranceFileCnt = m_lInsuranceFileCnt

            nResultDoNotMergeDocument = m_oInsuranceFile.GetDoNotMergeClauses(oDoNotMergeClauseArray)
            If nResultDoNotMergeDocument <> gPMConstants.PMEReturnCode.PMTrue AndAlso nResultDoNotMergeDocument <> PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oDoNotMergeClauseArray IsNot Nothing AndAlso Information.IsArray(oDoNotMergeClauseArray) Then
                Dim nDocumentId As Integer
                Dim nDocumentTypeId As Integer
                For iCount As Integer = oDoNotMergeClauseArray.GetLowerBound(1) To oDoNotMergeClauseArray.GetUpperBound(1)
                    nDocumentId = ToSafeInteger(oDoNotMergeClauseArray.GetValue(kDocumentTemplateId, iCount))
                    nDocumentTypeId = ToSafeInteger(oDoNotMergeClauseArray.GetValue(kDocumentTypeId, iCount))
                    m_lReturn = UseTheTemplate(r_lDocId:=nDocumentId, r_lDocTypeId:=nDocumentTypeId, r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sSpoolDesc:=ToSafeString(oDoNotMergeClauseArray.GetValue(kDocumentTemplateDesc, iCount)), v_lPartyCnt:=m_lPartyCnt)
                Next
            End If

        End If
        Return result

        'Catch excep As System.Exception



        '    result = gPMConstants.PMEReturnCode.PMError

        '    ' Log Error Message
        '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        '    Return result

        'End Try
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
    Private Function GetTheTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer, Optional ByRef r_vDocumentArray(,) As Object = Nothing) As Integer
        Dim result As Integer = 0




        Dim obPMBDocLink As bPMBDocLink.Business

        'Initialize object

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
        m_iFuntionalArea = 1

        m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=m_iFuntionalArea, v_lInsurance_File_Cnt:=m_lInsuranceFileCnt, v_lProcessType_Docs_ID:=m_lProcessType, v_lProcess_Type_Code:=m_sTransactionType, v_dtEffectiveDate:=m_dtEffectiveDate, r_vResultarray:=r_vDocumentArray, v_bCalledFromSAM:=False)


        '    'Returning the values
        If Information.IsArray(r_vDocumentArray) Then

            r_lDocumentTemplateId = ToSafeInteger(r_vDocumentArray(0, 0))

            r_lDocumentTypeId = ToSafeInteger(r_vDocumentArray(1, 0))
            'Start-(Arul Stephen)-(Document Configuration)
            'Start Arul PN56858

            m_lUserChoice = ToSafeInteger(r_vDocumentArray(ACSpooldocumentOrNot, 0))

            m_lIsEditableMerging = ToSafeInteger(r_vDocumentArray(ACEditableAfterMerging, 0))

            m_sProcessTypeDocuments = ToSafeString(r_vDocumentArray(ACProcessTypeDocuments, 0))
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
    Private Function UseTheTemplate(ByRef r_lDocId As Integer, ByRef r_lDocTypeId As Integer, ByRef r_lInsuranceFileCnt As Integer, Optional ByVal v_iIsClient As Integer = 0, Optional ByVal v_iIsAgent As Integer = 0, Optional ByVal v_iIsOffice As Integer = 0, Optional ByVal v_iProductionOrder As Integer = 1, Optional ByVal v_sSpoolDesc As String = "", Optional ByVal v_lPartyCnt As Integer = 0) As Integer
        Dim result As Integer = 0

        Dim oObject As iPMBDocTemplate.Interface_Renamed
        Dim option1 As String = String.Empty

        ' Try

        result = gPMConstants.PMEReturnCode.PMTrue
        m_lReturn = iPMFunc.GetSystemOption(5097, option1)

        'Dim temp_oObject As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        'oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If

        m_sDocumentDescription = v_sSpoolDesc
        oObject.PartyCnt = v_lPartyCnt
        oObject.InsuranceFileCnt = r_lInsuranceFileCnt

        oObject.InsuranceFolderCnt = m_lInsuranceFolderCnt  'MSB301001


        oObject.DocumentTemplateId = r_lDocId
        oObject.DocumentTypeId = r_lDocTypeId


        Select Case m_lUserChoice
            Case 0
                oObject.FormlessDocument = m_bFormlessDocumentPrint
                If m_bFormlessDocumentPrint Then
                    oObject.Mode = gSIRLibrary.ACPrintSilentMode
                Else
                    oObject.Mode = gSIRLibrary.ACPrintMode
                End If
            Case 1
                oObject.Mode = gSIRLibrary.ACSpoolDocMode
                oObject.SpoolDesc = m_sDocumentDescription
                oObject.FormlessDocument = m_bFormlessDocumentPrint
            Case 2
                oObject.Mode = ACUserChoice
                m_bFormlessDocumentPrint = False
        End Select

        'm_bSlientMode is only ever passed in from calling component
        'so we *could* set this from iPMURenInvitePrint if it's always the case that it's silent from that process
        'however we'd need to set oObject.FormlessDocument too - this configuration needs thorough regression testing
        If m_bSilentMode Then
            oObject.FormlessDocument = True
            oObject.Mode = gSIRLibrary.ACSpoolDocMode
            m_iPrintMode = gSIRLibrary.ACSpoolDocMode
        End If

        oObject.IsClient = v_iIsClient
        oObject.IsAgent = v_iIsAgent
        oObject.IsOffice = v_iIsOffice
        oObject.ProductionOrder = v_iProductionOrder

        If m_iPrintMode = gSIRLibrary.ACSpoolDocMode Then
            oObject.SpoolDesc = m_sDocumentDescription
        End If

        oObject.FormlessDocument = m_bFormlessDocumentPrint

        'Start-(Arul Stephen)-(Document Configuration)
        sTransactionType = FindTransactionType(m_sTransactionType.Trim())
        'Start Arul PN 56858


        oObject.CalledFromGetDocument = True 'PN 66227
        oObject.IsNonBatchProcess = m_bIsNonBatchProcess

        If ToSafeInteger(option1) <> 1 Then   'Enhamcement done for PLICO as they don't want to see any message during printing 68514
            If oObject.Mode = ACUserChoice Then
                m_lReturn = MessageBox.Show("Do you want to produce " & sTransactionType & " " & m_sProcessTypeDocuments & " template?", sTransactionType, MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                'End Arul PN 56858
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
            Case ACShortFormOfReInstatement
                result = ACExpandedFormOfRI
            Case ACShortFormOfNewBusiness
                result = ACExpandedFormOfNB
            Case ACShortFormOfZeroPremium
                result = ACExpandedFormOfZP
            Case ACShortFormOfAdditionalPremium
                result = ACExpandedFormOfAP
            Case ACShortFormOfReturnPremium
                result = ACExpandedFormOfRP
            Case ACShortFormOfRenewalAccept
                result = ACExpandedFormOfRN
            Case ACShortFormOfRenewalInvite
                result = ACExpandedFormOfRNI
            Case Else
                result = ACExpandedFormOfNB
        End Select



        Return result
    End Function
    'End-(Arul Stephen)-(Document Configuration)
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
    ' Name: UNLOCKPOLICY
    '
    ' Description: UnLock Policy  'PN35753 --RC
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 18 Jul 2007
    '' ***************************************************************** '
    Private Function UNLOCKPOLICY() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.UnLockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the policy", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UNLOCKPOLICY Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

End Class

