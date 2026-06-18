Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 30/08/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMPaymentMethod
    '
    ' Edit History:Pandu
    '' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/12/2003
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

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sUnderwritingOrAgency As String = ""

    '**************************************************************
    'Name:          GetMediaTypeId
    'Description:   Return the Id of a Media Type given it's code
    'History:       AR20050426 - PN10582 Created.
    '**************************************************************
    Public Function GetMediaTypeId(ByVal sMediaTypeCode As String, ByRef r_lMediaTypeId As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase


                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="Code", vValue:=sMediaTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectMediaTypeSQL, sSQLName:=ACSelectMediaTypeName, bStoredProcedure:=ACSelectMediaTypeStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResultArray) Then
                    r_lMediaTypeId = gPMFunctions.ToSafeLong(vResultArray(0, 0))
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypeId failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypeId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetPaymentDetails(ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByVal v_lSequenceNo As Integer, ByVal v_lClaimPaymentId As Integer, ByRef r_vPaymentDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimCnt", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPerilID", vValue:=v_lClaimPerilId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="SequenceNo", vValue:=v_lSequenceNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPaymentId", vValue:=v_lClaimPaymentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPaymentDetailsSQL, sSQLName:=ACSelectPaymentDetailsName, bStoredProcedure:=ACSelectPaymentDetailsStored, vResultArray:=r_vPaymentDetailsArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetInsurerDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : E Knott : 11-2005 : Datasure
    ' ***************************************************************** '
    Public Function GetInsurerDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsurerDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceFileCnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            'DC090606
            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimId", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetCoInsurerDetailsSQL, sSQLName:=kGetCoInsurerDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCoInsurerDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: UpdateClaimPayment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : E Knott : 11-2005 :
    ' ***************************************************************** '
    Public Function UpdateClaimPayment(ByVal v_lDocumentId As Integer, ByVal v_lClaimPaymentId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsurerDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "document_Id", v_lDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_payment_id", v_lClaimPaymentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            ' Execute selection Query

            If m_oDatabase.SQLAction(sSQL:=kUpdatePaymentDocumentIdSQL, sSQLName:=kUpdatePaymentDocumentIdName, bStoredProcedure:=kUpdatePaymentDocumentIdStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePaymentDocumentIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function RejectPayment(ByVal v_lClaimPerilId As Integer, ByVal v_lSequenceNo As Integer, ByVal v_lClaimPaymentId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "RejectPayment"
        Const kErrorCode As Integer = Constants.vbObjectError

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPerilID", vValue:=v_lClaimPerilId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", m_oDatabase.Parameters.Add - 'ClaimPerilID' failed")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="SequenceNo", vValue:=v_lSequenceNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", m_oDatabase.Parameters.Add - 'SequenceNo' failed")
            End If

            'eck 11/2005

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPaymentId", vValue:=v_lClaimPaymentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", m_oDatabase.Parameters.Add - 'SequenceNo' failed")
            End If


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRejectPaymentSQL, sSQLName:=ACRejectPaymentName, bStoredProcedure:=ACRejectPaymentStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", m_oDatabase.SQLSelect - ACRejectPaymentSQL failed")
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property


    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)
            '
            '    Select Case lCurrentRecord&
            '    Case Is < 1
            '        m_lCurrentRecord& = 0
            '    Case Is > m_oCLMPerilTypes.Count
            '        m_lCurrentRecord& = m_oCLMPerilTypes.Count
            '    Case Else
            '        m_lCurrentRecord& = lCurrentRecord&
            '    End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            'RecordCount = m_oCLMPerilTypes.Count

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                End If
                m_oDatabase = Nothing
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub New()
        MyBase.New()


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer

        Dim result As Integer = 0
        Dim m_oSystemOption As bSIROptions.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then
                m_oSystemOption = New bSIROptions.Business()

                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000) For Claims Numbering.
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         IsAgentCancelled
    ' Description:  Returns true if agent is cancelled
    ' Created By:   Alix - 12/02/2004
    ' ***************************************************************** '
    Public Function IsAgentCancelled(ByVal v_lAgentID As Integer, ByRef r_bIsCancelled As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            ' Assume agent is okay
            r_bIsCancelled = False
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters and add input paramter

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_id", vValue:=v_lAgentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Exec SQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckAgentCancelledSQL, sSQLName:=ACCheckAgentCancelledName, bStoredProcedure:=ACCheckAgentCancelledSP, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Get result
            If Information.IsArray(vArray) Then

                If CStr(vArray(0, 0)) = "1" Then
                    r_bIsCancelled = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAgentCancelled failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAgentCancelled", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOptionAgent
    ' Description:
    ' ***************************************************************** '
    Public Function GetOptionAgent(ByVal v_lProductID As Integer, ByRef r_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_bValue = False


            m_oDatabase.Parameters.Clear()

            ' Pass product ID

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Exec SQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelProductSQL, sSQLName:=ACSelProductName, bStoredProcedure:=ACSelProductStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Get result
            If Information.IsArray(vResultArray) Then

                If CStr(vResultArray(31, 0)) = "1" Then
                    r_bValue = True
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOptionAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptionAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetOptionMediaTypeMandatory(ByVal v_lProductID As Integer, ByRef r_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_bValue = False


            m_oDatabase.Parameters.Clear()

            ' Pass product ID

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Exec SQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelProductSQL, sSQLName:=ACSelProductName, bStoredProcedure:=ACSelProductStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Get result
            If Information.IsArray(vResultArray) Then

                If CStr(vResultArray(33, 0)) = "1" Then
                    r_bValue = True
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOptionMediaTypeMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptionMediaTypeMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBaseCurrencyID
    '
    ' Description: get branch's base currency using method in bPMFunc
    '
    ' History: RDC 15062004 created
    ' ***************************************************************** '
    Public Function GetBaseCurrencyID(ByVal v_lSourceID As Integer, ByRef r_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.GetBranchBaseCurrency(v_lSourceID:=v_lSourceID, v_oDatabase:=m_oDatabase, r_iCurrencyID:=r_iCurrencyID)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrencyID failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrencyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimCurrency
    '
    ' Description: get claim currency from policy and claim number
    '
    ' History: RDC 15062004 created
    ' ***************************************************************** '
    Public Function GetClaimCurrencyByRefs(ByVal v_sClaimRef As String, ByVal v_sPolicyNumber As String, ByRef r_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase


                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="policy_number", vValue:=v_sPolicyNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                m_lReturn = .Parameters.Add(sName:="claim_number", vValue:=v_sClaimRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                m_lReturn = .SQLSelect(sSQL:=ACClaimCurrencySQL, sSQLName:=ACClaimCurrencyName, bStoredProcedure:=ACClaimCurrencyStored)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            r_iCurrencyID = m_oDatabase.Records.Fields("currency_id").value


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCurrencyByRefs failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCurrencyByRefs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetUserCurrencyAuthorities(ByVal v_iUserID As Integer, ByRef r_bChangeDate As Boolean, ByRef r_bChangeRate As Boolean) As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.GetCurrencyAuthorities(v_iUserID:=v_iUserID, v_oDatabase:=m_oDatabase, r_bChangeDate:=r_bChangeDate, r_bChangeRate:=r_bChangeRate)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserCurrencyAuthorities failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserCurrencyAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function UpdateOverrideRates(ByVal v_lScreenMethod As Integer, ByVal v_lClaimId As Integer, ByVal v_lOverrideID As Integer, ByVal v_dtRateDate As Date, ByVal v_dCurrencyBaseRate As Double, ByVal v_dAccountBaseRate As Double, ByVal v_dSystemBaseRate As Double) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for claim_id.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="exchange_rate_override_reason_id", vValue:=v_lOverrideID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for exchange_rate_override_reason_id.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_base_xrate", vValue:=v_dCurrencyBaseRate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for currency_base_xrate.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_base_date", vValue:=v_dtRateDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for currency_base_date.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_base_xrate", vValue:=v_dAccountBaseRate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for account_base_xrate.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_base_date", vValue:=v_dtRateDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for account_base_date.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="system_base_xrate", vValue:=v_dSystemBaseRate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for system_base_xrate.")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="system_base_date", vValue:=v_dtRateDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.Parameters.Add failed for system_base_date.")
            End If

            If v_lScreenMethod = ACPaymentMethod Then

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePaymentRatesSQL, sSQLName:=ACUpdatePaymentRatesName, bStoredProcedure:=ACUpdatePaymentRatesStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.SQLAction failed.")
                End If
            Else

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateReceiptRatesSQL, sSQLName:=ACUpdateReceiptRatesName, bStoredProcedure:=ACUpdateReceiptRatesStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.SQLAction failed.")
                End If

                ' A receipt (salvage & recovery) will be accompanied by payments to reinsurers


                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePaymentRatesSQL, sSQLName:=ACUpdatePaymentRatesName, bStoredProcedure:=ACUpdatePaymentRatesStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oDatabase.SQLAction failed.")
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOverrideRates Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOverrideRates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetClaimPayableAccountID(ByRef r_lAccountID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetClaimPayableAccountID
        ' PURPOSE: Returns the account id of the CLMPAYABLE account for use
        '          by multi-currency
        ' AUTHOR: Danny Davis
        ' DATE: 11 November 2004, 10:18:16
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                .Parameters.Add("company_id", m_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                .Parameters.Add("shortcode", "CLMPAYABLE", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


                .Parameters.Add("accountid", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .SQLSelect("spu_ACT_Get_AccountID_From_ShortCode", "Get Account ID from Short Code", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to get account id for CLMPAYABLE")
                End If


                r_lAccountID = gPMFunctions.NullToLong(.Parameters.Item("accountid").Value)
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPayableAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimBaseCurrencyDetails
    '
    ' Parameters: n/a
    '
    ' Description: Returns the base currecy id associated with the
    '               specified claim
    '
    ' History:
    '           Created : MEvans : 18-04-2005 : 20233
    ' ***************************************************************** '
    Public Function GetClaimBaseCurrencyDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimBaseCurrencyDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' claim id
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimBaseCurrencyDetailsSQL, sSQLName:=kGetClaimBaseCurrencyDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimBaseCurrencyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName & _
                                          ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

        End If

        Return result

    End Function
End Class

