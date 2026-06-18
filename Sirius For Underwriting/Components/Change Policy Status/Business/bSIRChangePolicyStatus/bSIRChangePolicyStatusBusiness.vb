Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'Developer Guide No. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRiskData.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/12/2003
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    Private m_iMode As Integer

    Private m_sOldPolicyNumber As String = ""
    Private m_sNewPolicyNumber As String = ""
    Private m_m_oInsuranceFile As Object
    Private m_oPolicyNumMaint As Object
    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public ReadOnly Property OldPolicyNumber() As String
        Get
            Return m_sOldPolicyNumber
        End Get
    End Property
    Public ReadOnly Property NewPolicyNumber() As String
        Get
            Return m_sNewPolicyNumber
        End Get
    End Property
    'JMK 01/08/2001
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public Property Mode() As Integer
        Get
            Return m_iMode
        End Get
        Set(ByVal Value As Integer)
            m_iMode = Value
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************
    ' Name : UpdatePolicyPremium
    '
    ' Desc : update policy premium from Risk
    '
    '****************************************************************
    Public Function UpdatePolicyPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()
            'Add the parameters
            If Informations.IsNothing(v_lInsuranceFileCnt) Or CStr(v_lInsuranceFileCnt) = "" Or v_lInsuranceFileCnt = 0 Then
                v_lInsuranceFileCnt = 0
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdPolicyPremiumSQL, sSQLName:=ACUpdPolicyPremiumName, bStoredProcedure:=ACUpdPolicyPremiumStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = RollbackTrans()


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_id", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_id", vValue:=2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsFileTypeSQL, sSQLName:=ACUpdateInsFileTypeName, bStoredProcedure:=ACUpdateInsFileTypeStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_id", vValue:=4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_id", vValue:=5, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsFileTypeSQL, sSQLName:=ACUpdateInsFileTypeName, bStoredProcedure:=ACUpdateInsFileTypeStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_id", vValue:=7, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_id", vValue:=6, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsFileTypeSQL, sSQLName:=ACUpdateInsFileTypeName, bStoredProcedure:=ACUpdateInsFileTypeStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = RollbackTrans()


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If CommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyPremium", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' update policy type
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_sSelectedPolicyStatus"></param>
    ''' <param name="v_bBackDatedMTAsAllowed"></param>
    ''' <param name="v_bSetAsWritten"></param>
    ''' <param name="v_sOverriddenPolicyNumber"></param>
    ''' <param name="v_bSkipNewPolicyNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ChangePolicyStatus(ByVal v_lInsuranceFileCnt As Integer,
                                       Optional ByVal v_sSelectedPolicyStatus As String = "",
                                       Optional ByVal v_bBackDatedMTAsAllowed As Boolean = False,
                                       Optional ByVal v_bSetAsWritten As Boolean = False,
                                       Optional ByVal v_sOverriddenPolicyNumber As String = "",
                                       Optional ByVal v_bSkipNewPolicyNumber As Boolean = False) As Integer
        'End -Written Status
        Dim result As Integer = 0
        Dim vDeferredInd As Object
        Dim iIsAssignPolicyNumber As Integer
        Dim dtCoverStartDate As Date
        Dim iPartyCnt As Integer

        Const lPolicyBusinessType As Integer = 2
        Dim m_oInsuranceFile As New bSIRInsuranceFile.Services

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'create an instance of bSirInsuranceFile












            m_oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername$, sPassword:=m_sPassword$, iUserID:=m_iUserID%, iSourceID:=m_iSourceID%, iLanguageID:=m_iLanguageID%, iCurrencyID:=m_iCurrencyID%, iLogLevel:=m_iLogLevel%, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ChangePolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt

            'get details of current policy

            If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sOldPolicyNumber = m_oInsuranceFile.InsuranceRef
            m_sNewPolicyNumber = m_oInsuranceFile.InsuranceRef
            dtCoverStartDate = gPMFunctions.ToSafeDate(m_oInsuranceFile.CoverStartDate, DateTime.Parse(DateTime.Now))
            iPartyCnt = gPMFunctions.ToSafeInteger(m_oInsuranceFile.InsuredCnt, 0)

            If m_iMode = 1 Then

                'Nowt to do if we're not in new business

                If m_oInsuranceFile.InsuranceFileType <> "QUOTE" Then
                    Return result
                End If

                'Has this one already been done?

                vDeferredInd = m_oInsuranceFile.DeferredInd

                If Convert.IsDBNull(vDeferredInd) Or Informations.IsNothing(vDeferredInd) Then
                    vDeferredInd = 0
                End If

                If vDeferredInd = 1 Then
                    Return result
                End If

                'should we do this product?

                m_lReturn = CheckProduct(v_lProductId:=m_oInsuranceFile.ProductID, r_iIsAssignPolicyNumber:=iIsAssignPolicyNumber)

                If iIsAssignPolicyNumber = 1 Then
                    Return result
                End If
                If Not v_bSkipNewPolicyNumber Then

                    m_lReturn = GetNewPolicyNumber(v_lBusinessType:=lPolicyBusinessType, v_iBranch:=m_oInsuranceFile.SourceID, v_lProductId:=m_oInsuranceFile.ProductID, v_lAgent:=If(Convert.IsDBNull(m_oInsuranceFile.LeadAgentCnt) Or Informations.IsNothing(m_oInsuranceFile.LeadAgentCnt), 0, m_oInsuranceFile.LeadAgentCnt), r_sGeneratedPolicyNumber:=m_sNewPolicyNumber, v_dtTransactionDate:=dtCoverStartDate, v_iPartyCnt:=iPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'do we have new policy number
                    If m_sNewPolicyNumber <> "" Then

                        m_oInsuranceFile.InsuranceRef = m_sNewPolicyNumber
                        m_oInsuranceFile.DeferredInd = 1
                        m_lReturn = m_oInsuranceFile.UpdatePolicy()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
                Return result
            Else
                If m_sTransactionType = "MTC" Then
                    m_lReturn = CancelAllVersions(v_lInsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_oInsuranceFile.EventDescription = "Policy Cancelled"

                    m_oInsuranceFile.InsuranceFileTypeID = 8

                    m_oInsuranceFile.InsuranceFileStatusID = 1
                ElseIf m_sTransactionType = "MTCA" Then
                    m_oInsuranceFile.EventDescription = "Endorsement Cancelled"
                Else

                    Select Case m_oInsuranceFile.InsuranceFileType.ToUpper()
                        'WPR 75 Added
                        ' Case "QUOTE"
                        Case "QUOTE", "RENEWAL"
                            'WPR 75 END

                            If ((m_sTransactionType).ToUpper() <> "REN" And (m_oInsuranceFile.InsuranceFileType).ToString().ToUpper() = "RENEWAL") Or (m_oInsuranceFile.InsuranceFileType).ToString().ToUpper() = "QUOTE" Then
                                'Start- Written Status
                                If Not v_bSetAsWritten Then
                                    m_oInsuranceFile.EventDescription = "Policy made live"
                                    m_oInsuranceFile.InsuranceFileType = "POLICY"
                                Else
                                    m_oInsuranceFile.EventDescription = "Written record created"
                                    m_oInsuranceFile.InsuranceFileType = "WRITTEN"
                                End If
                                'End- Written Status

                                vDeferredInd = m_oInsuranceFile.DeferredInd

                                If Convert.IsDBNull(vDeferredInd) Or Informations.IsNothing(vDeferredInd) Then
                                    vDeferredInd = 0
                                End If

                                If vDeferredInd = 0 And m_sTransactionType.ToUpper() <> "MTA" And m_sTransactionType.ToUpper() <> "REN" And m_sTransactionType.ToUpper() <> "MTR" Then

                                    m_lReturn = CheckProduct(v_lProductId:=m_oInsuranceFile.ProductID, r_iIsAssignPolicyNumber:=iIsAssignPolicyNumber)

                                    If iIsAssignPolicyNumber = 0 And v_bSkipNewPolicyNumber = False Then
                                        If v_bSetAsWritten = True And v_sOverriddenPolicyNumber <> "" Then

                                            m_sNewPolicyNumber = v_sOverriddenPolicyNumber
                                        Else
                                            m_lReturn = GetNewPolicyNumber(v_lBusinessType:=lPolicyBusinessType, v_iBranch:=m_oInsuranceFile.SourceID, v_lProductId:=m_oInsuranceFile.ProductID, v_lAgent:=If(Convert.IsDBNull(m_oInsuranceFile.LeadAgentCnt) Or Informations.IsNothing(m_oInsuranceFile.LeadAgentCnt), 0, m_oInsuranceFile.LeadAgentCnt), r_sGeneratedPolicyNumber:=m_sNewPolicyNumber, v_dtTransactionDate:=dtCoverStartDate, v_iPartyCnt:=iPartyCnt)
                                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                ChangePolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                                                Return ChangePolicyStatus
                                            End If
                                        End If
                                        'do we have new policy number
                                        If m_sNewPolicyNumber <> "" Then
                                            m_oInsuranceFile.InsuranceRef = m_sNewPolicyNumber
                                        End If

                                    End If
                                    'm_m_oInsuranceFile.QuoteExpiryDate = Nothing
                                    'WPR 75 Added
                                End If
                            End If
                            'WPR 75 END
                            If (v_bSetAsWritten And (m_oInsuranceFile.InsuranceFileType).ToString().ToUpper() = "RENEWAL") Then
                                m_oInsuranceFile.EventDescription = "Written Renewal - " + m_sNewPolicyNumber
                                m_oInsuranceFile.InsuranceFileType = "WRITTEN"
                            End If
                        Case "MTAQUOTE"
                            If m_sTransactionType = "MTCA" Then
                                m_oInsuranceFile.EventDescription = "Endorsement Cancelled"
                            Else
                                'WPR 75 END
                                m_oInsuranceFile.EventDescription = "Endorsement made live"
                            End If
                            m_oInsuranceFile.InsuranceFileType = "MTA PERM"
                            'WPR 75 Added
                            If Convert.ToInt32(m_oInsuranceFile.InsuranceFileStatusID) = 1 And m_sTransactionType = "MTC" Then
                                'Set insurance file type & status to MTA Cancellation before calling update
                                m_oInsuranceFile.EventDescription = "Policy Cancelled"
                                m_oInsuranceFile.InsuranceFileTypeID = 8
                            End If
                            'WPR 75 END
                        Case "MTAQTETEMP"

                            m_oInsuranceFile.EventDescription = "Endorsement made live"
                            m_oInsuranceFile.InsuranceFileType = "MTA TEMP"

                        Case "MTAQREINS"

                            m_lReturn = ResetAllVersions(v_lInsuranceFileCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            m_oInsuranceFile.EventDescription = "Reinstatement made live"
                            m_oInsuranceFile.InsuranceFileType = "MTAREINS"
                            'Start Written Status
                        Case "WRITTEN"
                            m_oInsuranceFile.EventDescription = "Policy made live"
                            m_oInsuranceFile.InsuranceFileType = "POLICY"
                            'End Written Status
                    End Select
                End If

                If m_sTransactionType = "MTA" Then
                    If v_sSelectedPolicyStatus = "Cancelled" Then
                        m_oInsuranceFile.InsuranceFileStatusID = 1
                    End If

                    If v_sSelectedPolicyStatus = "Lapsed" Then
                        m_oInsuranceFile.InsuranceFileStatusID = 2
                    End If

                    If v_sSelectedPolicyStatus = "Replaced" Then
                        m_oInsuranceFile.InsuranceFileStatusID = 4
                    End If
                End If

                'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)


                m_lReturn = m_oInsuranceFile.UpdatePolicy()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oInsuranceFile.MakeEvent()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangePolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        Finally
            'destroy m_oInsuranceFile
            If Not m_oInsuranceFile Is Nothing Then
                m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

        End Try

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckProduct
    '
    ' Description:
    '
    ' History: 05/11/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CheckProduct(ByVal v_lProductId As Integer, ByRef r_iIsAssignPolicyNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        r_iIsAssignPolicyNumber = 0

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckProductSQL, sSQLName:=ACCheckProductName, bStoredProcedure:=ACCheckProductStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vArray) Then
            '29/07/2003 Tracy Richards - Protecting against the array returning a blank string

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                r_iIsAssignPolicyNumber = CInt(vArray(0, 0))
            Else
                r_iIsAssignPolicyNumber = 0
            End If

        End If


        'Developer Guide No. 12
        vArray = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name : GetNewPolicyNumber
    '
    ' Desc : generate new policy number
    '
    ' ***************************************************************** '
    'Renuka - (WPR87 Paralleling) - Added optional parameter v_dtTransactionDate
    Private Function GetNewPolicyNumber(ByVal v_lBusinessType As Integer, ByVal v_iBranch As Integer, ByVal v_lProductId As Integer, ByVal v_lAgent As Integer, ByRef r_sGeneratedPolicyNumber As String, Optional ByVal v_dtTransactionDate As Date = #12/30/1899#, Optional ByVal v_iPartyCnt As Integer = 0) As Integer


        Dim result As Integer = 0
        Dim oPolicyNumMaint As New bSIRPolicyNumMaint.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'create an instance of bSirPolicyNumMaint












            oPolicyNumMaint = New bSIRPolicyNumMaint.Business
            m_lReturn = oPolicyNumMaint.Initialise(sUsername:=m_sUsername$, sPassword:=m_sPassword$, iUserID:=m_iUserID%, iSourceID:=m_iSourceID%, iLanguageID:=m_iLanguageID%, iCurrencyID:=m_iCurrencyID%, iLogLevel:=m_iLogLevel%, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetNewPolicyNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'Start - Renuka - (WPR87 Paralleling)

            'End - Renuka - (WPR87 Paralleling)
            Return oPolicyNumMaint.GeneratePolicyNumber(v_lBusinessType:=v_lBusinessType, v_iBranch:=v_iBranch, v_lProductId:=v_lProductId, v_lAgent:=v_lAgent, r_sGeneratedPolicyNumber:=r_sGeneratedPolicyNumber, v_dtTransactionDate:=v_dtTransactionDate, v_lPartyCnt:=v_iPartyCnt)

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNewPolicyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewPolicyNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        Finally

            'destroy oPolicyNumMaint
            If Not oPolicyNumMaint Is Nothing Then
                oPolicyNumMaint.Dispose()
                oPolicyNumMaint = Nothing
            End If
        End Try

    End Function


    ' ***************************************************************** '
    ' Name : CancelAllVersions
    '
    ' Desc : Cancel all versions of a policy
    '
    ' JMK 03/08/2001 - Cancel Policy (MTC)
    ' ***************************************************************** '
    Private Function CancelAllVersions(ByVal v_lInsuranceFileCnt As Integer) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCancelAllVersionsSQL, sSQLName:=ACCancelAllVersionsName, bStoredProcedure:=ACCancelAllVersionsStored)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenumberRisks
    '
    ' Description: This function processes all risks for the policy relating
    '              to the passed InsuranceFileCnt parameter. It sets the
    '              variation_number on each risk to 0, and renumbers all the
    '              risk_number fields to be sequential starting from 1.
    '
    ' History: PW311002 - created.
    '
    ' ***************************************************************** '
    Public Function RenumberRisks(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameter collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure that renumbers the risks
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenumberRisksSQL, sSQLName:=ACRenumberRisksName, bStoredProcedure:=ACRenumberRisksStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenumberRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenumberRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: DeleteRisks
    '
    ' Description: Deletes the Risk link records for unselected risks
    '
    ' History: PW151102 - created.
    '
    ' ***************************************************************** '
    Public Function DeleteRisks(ByVal v_vrisks(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round all risks
            For i As Integer = v_vrisks.GetLowerBound(1) To v_vrisks.GetUpperBound(1)

                ' Check if risk is not selected

                Dim dTemp As Double = 0D
                Double.TryParse(v_vrisks(1, i), dTemp)
                If dTemp <> 1 Then

                    ' Clear parameters collection
                    m_oDatabase.Parameters.Clear()

                    ' Add the insurance file cnt parameter

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(CInt(v_vrisks(2, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add the risk cnt parameter

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(CInt(v_vrisks(3, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute the stored procedure to delete the risk link
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRiskSQL, sSQLName:=ACDeleteRiskName, bStoredProcedure:=ACDeleteRiskStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRisksByStatus
    '
    ' Description:
    '
    ' History: 24/09/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetRisksByStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vRisks(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRisksByStatusSQL, sSQLName:=ACGetRisksByStatusName, bStoredProcedure:=ACGetRisksByStatusStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vRisks)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisksByStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisksByStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' private Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetPolicySummary
    ' Description:
    ' ***************************************************************** '
    Public Function GetPolicySummary(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicySummarySQL, sSQLName:=ACGetPolicySummaryName, bStoredProcedure:=ACGetPolicySummaryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicySummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicySummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ResetAllVersions(ByVal v_lInsuranceFileCnt As Integer) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACResetAllVersionsSQL, sSQLName:=ACResetAllVersionsName, bStoredProcedure:=ACResetAllVersionsStored)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    'Start - Renuka - (WPR87 Paralleling)
    Public Function CheckPeriodStatus(ByVal v_lBusinessType As Integer, ByVal v_iBranch As Integer, ByVal v_lProductId As Integer, ByVal v_lAgent As Integer, ByRef r_sGeneratedPolicyNumber As String, ByVal v_dtInitialCoverStartDate As Date, ByVal v_dtCurrentStartDate As Date) As Integer
        Dim result As Integer = 0
        Dim oACTPeriod As bACTPeriod.Form
        Dim iPartyCnt As Integer
        Const kMethodName As String = "CheckPeriodStatus"
        Dim vResultArray(,) As Object = Nothing
        Dim lNumberingScheme As Integer
        Dim sCode As String = ""
        Dim bGenerate As Boolean
        Dim sMaskCode As String = ""
        Const iNUM_SCHEME_IS_GEN As Integer = 8
        Const iNUM_SCHEME_MASK As Integer = 9
        Const lQuoteBusinessType As Integer = 1
        Const lPolicyBusinessType As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_m_oInsuranceFile Is Nothing Then
                iPartyCnt = gPMFunctions.ToSafeInteger(m_m_oInsuranceFile.InsuredCnt, 0)
            End If
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the Parameter (product_id) Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromProductSQL, sSQLName:=ACGetNumberingSchemeIdsFromProductName, bStoredProcedure:=ACGetNumberingSchemeFromProductStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed in execution of the SP " & ACGetNumberingSchemeIdsFromProductSQL & "", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Not (Informations.IsArray(vResultArray)) Then
                lNumberingScheme = 0
                sCode = ""
            Else

                If CDbl(vResultArray(5, 0)) = 1 And v_lBusinessType = lQuoteBusinessType Then
                    v_lBusinessType = lPolicyBusinessType
                End If
                Dim auxVar As Object = vResultArray(v_lBusinessType, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    lNumberingScheme = CInt(vResultArray(v_lBusinessType, 0))

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If
            End If



            If lNumberingScheme = 0 Or Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme) Then
                bGenerate = False
                sMaskCode = ""

                Return result
            End If

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the Parameter (m_iLanguageID) Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(lNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the Parameter (lNumberingScheme) Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed in execution of the SP " & ACGetNumberingSchemeSQL & "", gPMConstants.PMELogLevel.PMLogError)
            ElseIf Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError(kMethodName, "The SP " & ACGetNumberingSchemeSQL & " doesn't return any values", gPMConstants.PMELogLevel.PMLogError)
            End If


            bGenerate = CBool(vResultArray(iNUM_SCHEME_IS_GEN, 0))

            sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))

            If Not bGenerate Then '

                r_sGeneratedPolicyNumber = ""
                Return result
            End If


            Dim lInitialPeriodId, lCurrentPeriodId As Integer
            Dim sInitialYearName As String = ""
            Dim sCurrentYearName As String = ""
            If sMaskCode.IndexOf("U"c) >= 0 Then


                oACTPeriod = New bACTPeriod.Form
                m_lReturn = oACTPeriod.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to create an instance of business object bACTPeriod.Form.", gPMConstants.PMELogLevel.PMLogError)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oACTPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtCurrentStartDate, lPeriodId:=lInitialPeriodId, vYearName:=sInitialYearName, v_bIncludeClosed:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed get the Period details ", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = oACTPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtCurrentStartDate, lPeriodId:=lCurrentPeriodId, vYearName:=sCurrentYearName, v_bIncludeClosed:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get the Period details ", gPMConstants.PMELogLevel.PMLogError)
                End If

                If sInitialYearName.Trim() <> "" And sCurrentYearName.Trim() <> "" And (sInitialYearName.Trim() <> sCurrentYearName.Trim()) Then
                    m_lReturn = GetNewPolicyNumber(v_lBusinessType:=v_lBusinessType, v_iBranch:=v_iBranch, v_lProductId:=v_lProductId, v_lAgent:=v_lAgent, r_sGeneratedPolicyNumber:=r_sGeneratedPolicyNumber, v_dtTransactionDate:=v_dtCurrentStartDate, v_iPartyCnt:=iPartyCnt)

                End If
            End If




        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CheckPeriodStatus", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
    'End - Renuka - (WPR87 Paralleling)
    'Start - Renuka - (WPR87 Paralleling)
    Public Function GetAccountingPeriodForCoverStartDate(ByVal v_lBusinessType As Integer, ByVal v_lProductId As Integer, ByVal v_dtCoverStartDate As Date, ByVal v_iSubBranchID As Integer, ByRef r_lPeriodID As Integer) As Integer
        Dim result As Integer = 0
        Dim oACTPeriod As bACTPeriod.Form

        Const kMethodName As String = "GetAccountingPeriodForCoverStartDate"
        Const lQuoteBusinessType As Integer = 1
        Const lPolicyBusinessType As Integer = 2
        Dim vResultArray(,) As Object = Nothing
        Dim lNumberingScheme As Integer
        Dim sCode As String = ""
        Dim bGenerate As Boolean
        Dim sMaskCode As String = ""
        Const iNUM_SCHEME_IS_GEN As Integer = 8
        Const iNUM_SCHEME_MASK As Integer = 9


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the Parameter (product_id) Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromProductSQL, sSQLName:=ACGetNumberingSchemeIdsFromProductName, bStoredProcedure:=ACGetNumberingSchemeFromProductStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed in execution of the SP " & ACGetNumberingSchemeIdsFromProductSQL & "", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Not (Informations.IsArray(vResultArray)) Then
                lNumberingScheme = 0
                sCode = ""
            Else

                If CDbl(vResultArray(5, 0)) = 1 And v_lBusinessType = lQuoteBusinessType Then
                    v_lBusinessType = lPolicyBusinessType
                End If
                Dim auxVar As Object = vResultArray(v_lBusinessType, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    lNumberingScheme = CInt(vResultArray(v_lBusinessType, 0))

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If
            End If


            If lNumberingScheme = 0 Or Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme) Then
                bGenerate = False
                sMaskCode = ""
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the Parameter (m_iLanguageID) Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(lNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the Parameter (lNumberingScheme) Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:=ACGetNumberingSchemeName, bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed in execution of the SP " & ACGetNumberingSchemeSQL & "", gPMConstants.PMELogLevel.PMLogError)
            ElseIf Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError(kMethodName, "The SP " & ACGetNumberingSchemeSQL & " doesn't return any values", gPMConstants.PMELogLevel.PMLogError)
            End If


            bGenerate = CBool(vResultArray(iNUM_SCHEME_IS_GEN, 0))

            sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))

            If sMaskCode.IndexOf("U"c) >= 0 Then

                oACTPeriod = New bACTPeriod.Form
                m_lReturn = oACTPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to create an instance of business object bACTPeriod.Form.", gPMConstants.PMELogLevel.PMLogError)

                End If

                m_lReturn = oACTPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtCoverStartDate, lPeriodID:=r_lPeriodID, vSubBranchID:=v_iSubBranchID, v_bIsPeriodNotExist:=True, v_bIncludeClosed:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed get the Period details ", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If



        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAccountingPeriodForCoverStartDate", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    'End - Renuka - (WPR87 Paralleling)
End Class
