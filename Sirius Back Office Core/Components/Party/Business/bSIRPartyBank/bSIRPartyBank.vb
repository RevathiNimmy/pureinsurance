Option Strict Off
Option Explicit On
Imports System.Text
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' Handle action_code for history

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 18/06/2007
    '
    ' Description: Creatable Bussiness class which contains all the
    '              methods, business rules required for the
    '              bSIRPartyBank.
    '
    ' Edit History:Gaurav Arora
    ' ***************************************************************** '


    ' ************************************************
    ' Module variables
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"


    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oCaseNumbering As Object
    Private m_oEvent As bSIREvent.Business
    Private m_bIsBankEdited As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
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


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :18/06/2007
    '
    ' Edit History :Gaurav Arora
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            ' Set Username and Password
            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Edit History :Gaurav Arora
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
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav Arora
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub New()
        MyBase.New()

        Try

            Dim vDatabase As Object = Nothing

            ' Class Initialise
            m_oDatabase = New dPMDAO.Database()


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'Developer Guide No.33
    Public Function GetPartyBankDetails(ByRef vPartyBankDetails(,) As Object, Optional ByVal vPartyCnt As Object = Nothing, Optional ByVal vAccountID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyBankDetails"


        Dim vResult As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (Convert.IsDBNull(vPartyCnt) Or Informations.IsNothing(vPartyCnt)) Or Not (Convert.IsDBNull(vAccountID) Or Informations.IsNothing(vAccountID)) Then
                ' Add Input parameters for both PartyCnt and AccountId

                ' Clear Down Database Parameters

                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_id", vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELPartyBankDetailsSQL, sSQLName:=ACSELPartyBankDetailsName, bStoredProcedure:=True, vResultArray:=vPartyBankDetails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not Informations.IsArray(vPartyBankDetails) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else

                    m_lReturn = CType(MergeAllLookups(vResultArray:=vPartyBankDetails), gPMConstants.PMEReturnCode)
                End If

                Return result
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

    Private Function MergeAllLookups(ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "MergeAllLookups"

        Dim vLookUp As Object, vID As Object = Nothing
        Dim vMergeLookup(1) As Object


        result = gPMConstants.PMEReturnCode.PMTrue
        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

            m_lReturn = CType(MergeLookupValues(ENMergeColumn:=MainModule.ENPartyBank.BankPaymentTypeId, vIdValue:=vResultArray(MainModule.ENPartyBank.BankPaymentTypeId, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Start - Sankar - PN 54715 - Added If Condition

                If Informations.IsArray(vLookUp) Then


                    vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                    vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                    vResultArray(MainModule.ENPartyBank.BankPaymentTypeId, lCount) = CType(vMergeLookup, Object()).Clone
                End If
                'End - Sankar - PN 54715
            End If


            '            m_lReturn = MergeLookupValues(ENMergeColumn:=ENPartyBank.BankAccountTypeId, _
            ''                                            vIdValue:=vResultArray(ENPartyBank.BankAccountTypeId, lCount), _
            ''                                            vLookUp:=vLookUp)
            '            If m_lReturn <> PMTrue Then
            '                RaiseError kMethodName, "MergeAllLookups Failed", PMLogError
            '            ElseIf m_lReturn = PMTrue Then
            '                  vMergeLookup(kLookId) = vLookUp(kLookId, 0)
            '                  vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)
            '                  vResultArray(ENPartyBank.BankAccountTypeId, lCount) = vMergeLookup
            '            End If

            m_lReturn = CType(MergeLookupValues(ENMergeColumn:=MainModule.ENPartyBank.BankNameId, vIdValue:=vResultArray(MainModule.ENPartyBank.BankNameId, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Start - Sankar - PN 54715 - Added If Condition

                If Informations.IsArray(vLookUp) AndAlso Not gPMFunctions.ToSafeLong(vResultArray(MainModule.ENPartyBank.BankNameId, lCount), 0) = 0 Then


                    vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                    vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                    vResultArray(MainModule.ENPartyBank.BankNameId, lCount) = CType(vMergeLookup, Object()).Clone
                End If
                'End - Sankar - PN 54715
            End If

            m_lReturn = CType(MergeLookupValues(ENMergeColumn:=MainModule.ENPartyBank.BankCountry, vIdValue:=vResultArray(MainModule.ENPartyBank.BankCountry, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Start - Sankar - PN 54715 - Added If Condition

                If Informations.IsArray(vLookUp) Then


                    vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                    vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                    vResultArray(MainModule.ENPartyBank.BankCountry, lCount) = CType(vMergeLookup, Object()).Clone
                End If
                'End - Sankar - PN 54715
            End If

            m_lReturn = CType(MergeLookupValues(ENMergeColumn:=MainModule.ENPartyBank.CCCountry, vIdValue:=vResultArray(MainModule.ENPartyBank.CCCountry, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Start - Sankar - PN 54715 - Added If Condition

                If Informations.IsArray(vLookUp) Then


                    vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                    vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                    vResultArray(MainModule.ENPartyBank.CCCountry, lCount) = vMergeLookup
                End If
                'End - Sankar - PN 54715
            End If

        Next
        Return result
    End Function

    Private Function MergeLookupValues(ByVal ENMergeColumn As MainModule.ENPartyBank, ByVal vIdValue As Object, ByRef vLookUp As Object) As Integer
        Dim result As Integer = 0

        Dim sTableName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        If ENMergeColumn = MainModule.ENPartyBank.BankPaymentTypeId Then
            sTableName = "bank_payment_type"
        ElseIf ENMergeColumn = MainModule.ENPartyBank.BankAccountTypeId Then
            sTableName = "bank_account_type"
        ElseIf ENMergeColumn = MainModule.ENPartyBank.BankNameId Then
            sTableName = "CashListItem_Bank"
        ElseIf ENMergeColumn = MainModule.ENPartyBank.BankCountry Or ENMergeColumn = MainModule.ENPartyBank.CCCountry Then
            sTableName = "Country"
        End If


        'Developer Guide No. Done as per VB Code
        m_lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=sTableName, r_vResults:=vLookUp, v_Id:=vIdValue), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

        End If
        Return result
    End Function

    'Developer Guide No 98
    Public Function GetPartyBankHistory(ByRef vPartyBankHistory(,) As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAccountID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyBankHistory"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Developer Guide No 44
            If Not (Convert.IsDBNull(vPartyCnt) OrElse Informations.IsNothing(vPartyCnt)) OrElse Not (Convert.IsDBNull(vAccountID) OrElse Informations.IsNothing(vAccountID)) Then
                ' Add Input parameters for both PartyCnt and AccountId

                ' Clear Down Database Parameters

                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_id", vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELPartyBankHistorySQL, sSQLName:=ACSELPartyBankHistoryName, bStoredProcedure:=True, vResultArray:=vPartyBankHistory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not Informations.IsArray(vPartyBankHistory) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else

                    m_lReturn = CType(MergeAllLookupsHistory(vResultArray:=vPartyBankHistory), gPMConstants.PMEReturnCode)
                End If

                Return result
            End If

            Return result
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

    Private Function MergeAllLookupsHistory(ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "MergeAllLookupsHistory"

        Dim vLookUp As Object = Nothing
        Dim vID As Object = Nothing
        Dim vMergeLookup(1) As Object


        result = gPMConstants.PMEReturnCode.PMTrue
        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

            m_lReturn = CType(MergeLookupValuesHistory(ENMergeColumn:=MainModule.ENPartyBankHistory.BankPaymentTypeId, vIdValue:=vResultArray(MainModule.ENPartyBankHistory.BankPaymentTypeId, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookupsHistory Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Start - Sankar - PN 54715 - Added IF Condition

                If Informations.IsArray(vLookUp) Then


                    vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                    vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                    vResultArray(MainModule.ENPartyBankHistory.BankPaymentTypeId, lCount) = CType(vMergeLookup, Object()).Clone
                End If
                'End - Sankar - PN 54715
            End If

            m_lReturn = CType(MergeLookupValuesHistory(ENMergeColumn:=MainModule.ENPartyBankHistory.BankName, vIdValue:=vResultArray(MainModule.ENPartyBankHistory.BankName, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookupsHistory Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Start - Sankar - PN 54715 - Added IF Condition

                If Informations.IsArray(vLookUp) Then


                    vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                    vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                    vResultArray(MainModule.ENPartyBankHistory.BankName, lCount) = vMergeLookup
                End If
                'End - Sankar - PN 54715
            End If


        Next
        Return result
    End Function

    Private Function MergeLookupValuesHistory(ByVal ENMergeColumn As MainModule.ENPartyBankHistory, ByVal vIdValue As Object, ByRef vLookUp As Object) As Integer
        Dim result As Integer = 0

        Dim sTableName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        If ENMergeColumn = MainModule.ENPartyBankHistory.BankPaymentTypeId Then
            sTableName = "bank_payment_type"
        ElseIf ENMergeColumn = MainModule.ENPartyBankHistory.BankAccountTypeId Then
            sTableName = "bank_account_type"
        ElseIf ENMergeColumn = MainModule.ENPartyBankHistory.BankName Then
            sTableName = "CashListItem_Bank"
        End If


        m_lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=sTableName, r_vResults:=vLookUp, v_Id:=vIdValue), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

        End If
        Return result
    End Function

    ''' <summary>
    ''' UpdatePartyBankDetails
    ''' </summary>
    ''' <param name="vPartyCnt"></param>
    ''' <param name="vPartyBankDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePartyBankDetails(ByRef vPartyCnt As Object, Optional ByRef vPartyBankDetails(,) As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "UpdatePartyBankDetails"
        Dim nEventCnt As Integer
        Dim nPartyBankId As Integer
        Dim sActionCode As String = ""
        Try
            Dim sDescription As New StringBuilder
            Dim oOldPartyBankDet As Object = Nothing
            Dim nPartyBankIdForHistory As Integer
            Dim oBankNameId As Object
            Dim oBankNameDesc As Object

            'Add new contacts for address if supplied
            If True Then

                For i As Integer = vPartyBankDetails.GetLowerBound(1) To vPartyBankDetails.GetUpperBound(1)


                    If CStr(vPartyBankDetails(MainModule.ENPartyBank.IsBank, i)) <> "1" Then
                        oBankNameId = Nothing
                        oBankNameDesc = Nothing
                    Else
                        If vPartyBankDetails(MainModule.ENPartyBank.BankNameId, i).ToString <> "" Then
                            oBankNameId = vPartyBankDetails(MainModule.ENPartyBank.BankNameId, i).GetValue(MainModule.ENPMLookups.Id)
                        Else
                            oBankNameId = Nothing
                        End If
                        If vPartyBankDetails(MainModule.ENPartyBank.BankNameId, i).ToString <> "" Then
                            oBankNameDesc = vPartyBankDetails(MainModule.ENPartyBank.BankNameId, i).GetValue(MainModule.ENPMLookups.Description)
                        Else
                            oBankNameDesc = Nothing
                        End If
                    End If

                    If vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMAdd Then
                        sActionCode = "Setup"
                        Dim sScreenHierarchyName As String = ""
                        If sScreenHierarchy <> "" Then
                            sScreenHierarchyName = sScreenHierarchy & $"Bank Payment Type({vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Description)})/Account Type({vPartyBankDetails(ENPartyBank.BankAccountTypeId, i)})"
                        End If
                        ' Make a call to AddPartyBank function supplying the relevant input parameters
                        m_lReturn = AddPartyBank(r_lPartyBankId:=nPartyBankId,
                                                    vPartyCnt:=vPartyCnt,
                                                    vAccountID:=vPartyBankDetails(ENPartyBank.AccountId, i),
                                                    sAccHolderName:=vPartyBankDetails(ENPartyBank.AccountHolderName, i),
                                                    sAccNumber:=vPartyBankDetails(ENPartyBank.AccountNumber, i),
                                                    lBankPaymentTypeID:=vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Id),
                                                    sBankAccountType:=vPartyBankDetails(ENPartyBank.BankAccountTypeId, i),
                                                    lIsBank:=vPartyBankDetails(ENPartyBank.IsBank, i),
                                                        vBankNameId:=oBankNameId,
                                                    sBankBranch:=vPartyBankDetails(ENPartyBank.BankBranch, i),
                                                    sBankBranchCode:=vPartyBankDetails(ENPartyBank.BankBranchCode, i),
                                                    sBankAdd1:=vPartyBankDetails(ENPartyBank.BankAdd1, i),
                                                    sBankAdd2:=vPartyBankDetails(ENPartyBank.BankAdd2, i),
                                                    sBankAdd3:=vPartyBankDetails(ENPartyBank.BankAdd3, i),
                                                    sBankTown:=vPartyBankDetails(ENPartyBank.BankTown, i),
                                                    sBankPCode:=vPartyBankDetails(ENPartyBank.BankPCode, i),
                                                    sBankRegion:=vPartyBankDetails(ENPartyBank.BankRegion, i),
                                                    sBankCountry:=vPartyBankDetails(ENPartyBank.BankCountry, i)(ENPMLookups.Id),
                                                    sCCNum:=vPartyBankDetails(ENPartyBank.CCNum, i),
                                                    sCCStartDate:=vPartyBankDetails(ENPartyBank.CCStartDate, i),
                                                    sCCExpiryDate:=vPartyBankDetails(ENPartyBank.CCExpiryDate, i),
                                                    sCCIssueNum:=vPartyBankDetails(ENPartyBank.CCIssueNum, i),
                                                    sCCPin:=vPartyBankDetails(ENPartyBank.CCPIN, i),
                                                    lIsRegistered:=vPartyBankDetails(ENPartyBank.IsRegistered, i),
                                                        sCCAdd1:=vPartyBankDetails(ENPartyBank.CCAdd1, i),
                                                        sCCAdd2:=vPartyBankDetails(ENPartyBank.CCAdd2, i),
                                                        sCCAdd3:=vPartyBankDetails(ENPartyBank.CCAdd3, i),
                                                        sCCTown:=vPartyBankDetails(ENPartyBank.CCTown, i),
                                                        sCCPCode:=vPartyBankDetails(ENPartyBank.CCPCode, i),
                                                        sCCCountry:=vPartyBankDetails(ENPartyBank.CCCountry, i)(ENPMLookups.Id),
                                                        sCCNameOnCard:=vPartyBankDetails(ENPartyBank.CCNameOnCard, i),
                                                        sCCManaulAuthNumber:=vPartyBankDetails(ENPartyBank.CCManualAuthorisationNum, i),
                                                        sBIC:=vPartyBankDetails(ENPartyBank.BIC, i),
                                                        sIBAN:=vPartyBankDetails(ENPartyBank.IBAN, i),
                                                 v_nIsDefault:=vPartyBankDetails(ENPartyBank.IsDefault, i),
                                                 sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchyName)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "AddPartyBank Failed", gPMConstants.PMELogLevel.PMLogError)
                        Else
                            nPartyBankIdForHistory = nPartyBankId
                        End If

                        ' Make a call to AddPartyBankHistory function supplying the relevant input parameters
                        sDescription = New StringBuilder("Bank Details Created -")
                        sDescription.Append(CStr(vPartyBankDetails(MainModule.ENPartyBank.AccountHolderName, i)) & ", " &
                                            oBankNameDesc & ", " &
                                        CStr(vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, i)))

                        m_lReturn = CType(CreateEvent(r_lEventCnt:=nEventCnt, v_lPartyCnt:=vPartyCnt,
                                                      v_lEventTypeId:=PMBConst.PMBEventNewClient, v_dtEventDate:=DateTime.Today,
                                                      v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)

                        ' Generate Event Log
                        m_lReturn = CType(CreateEventLog(vPartyCnt:=vPartyCnt, sDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMEdit Then
                        sActionCode = "Amendment"
                        Dim sScreenHierarchyName As String = ""
                        If sScreenHierarchy <> "" Then
                            sScreenHierarchyName = sScreenHierarchy & $"/Bank Payment Type({vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Description)})/Account Type({vPartyBankDetails(ENPartyBank.BankAccountTypeId, i)})"
                        End If
                        'Since we need to feed old bank details along with new bank details in order to generate event log at the time of editing bank details so retrieve old bank details prior to editing it in the database
                        m_lReturn = CType(GetBankDetailsById(lPartyBankId:=vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i), vOldPartyBankDet:=oOldPartyBankDet), gPMConstants.PMEReturnCode)

                        m_lReturn = EditPartyBank(lPartyBankId:=vPartyBankDetails(ENPartyBank.PartyBankId, i),
                                                       vPartyCnt:=vPartyCnt,
                                                       vAccountID:=vPartyBankDetails(ENPartyBank.AccountId, i),
                                                       sAccHolderName:=vPartyBankDetails(ENPartyBank.AccountHolderName, i),
                                                       sAccNumber:=vPartyBankDetails(ENPartyBank.AccountNumber, i),
                                                       lBankPaymentTypeID:=vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Id),
                                                       sBankAccountType:=vPartyBankDetails(ENPartyBank.BankAccountTypeId, i),
                                                       lIsBank:=vPartyBankDetails(ENPartyBank.IsBank, i),
                                                           vBankNameId:=oBankNameId,
                                                       sBankBranch:=vPartyBankDetails(ENPartyBank.BankBranch, i),
                                                       sBankBranchCode:=vPartyBankDetails(ENPartyBank.BankBranchCode, i),
                                                       sBankAdd1:=vPartyBankDetails(ENPartyBank.BankAdd1, i),
                                                       sBankAdd2:=vPartyBankDetails(ENPartyBank.BankAdd2, i),
                                                       sBankAdd3:=vPartyBankDetails(ENPartyBank.BankAdd3, i),
                                                       sBankTown:=vPartyBankDetails(ENPartyBank.BankTown, i),
                                                       sBankPCode:=vPartyBankDetails(ENPartyBank.BankPCode, i),
                                                       sBankRegion:=vPartyBankDetails(ENPartyBank.BankRegion, i),
                                                       sBankCountry:=vPartyBankDetails(ENPartyBank.BankCountry, i)(ENPMLookups.Id),
                                                       sCCNum:=vPartyBankDetails(ENPartyBank.CCNum, i),
                                                       sCCStartDate:=vPartyBankDetails(ENPartyBank.CCStartDate, i),
                                                       sCCExpiryDate:=vPartyBankDetails(ENPartyBank.CCExpiryDate, i),
                                                       sCCIssueNum:=vPartyBankDetails(ENPartyBank.CCIssueNum, i),
                                                       sCCPin:=vPartyBankDetails(ENPartyBank.CCPIN, i),
                                                       lIsRegistered:=vPartyBankDetails(ENPartyBank.IsRegistered, i),
                                                           sCCAdd1:=vPartyBankDetails(ENPartyBank.CCAdd1, i),
                                                           sCCAdd2:=vPartyBankDetails(ENPartyBank.CCAdd2, i),
                                                           sCCAdd3:=vPartyBankDetails(ENPartyBank.CCAdd3, i),
                                                           sCCTown:=vPartyBankDetails(ENPartyBank.CCTown, i),
                                                           sCCPCode:=vPartyBankDetails(ENPartyBank.CCPCode, i),
                                                           sCCCountry:=vPartyBankDetails(ENPartyBank.CCCountry, i)(ENPMLookups.Id),
                                                           sCCNameOnCard:=vPartyBankDetails(ENPartyBank.CCNameOnCard, i),
                                                           sCCManaulAuthNumber:=vPartyBankDetails(ENPartyBank.CCManualAuthorisationNum, i),
                                                           sBIC:=vPartyBankDetails(ENPartyBank.BIC, i),
                                                           sIBAN:=vPartyBankDetails(ENPartyBank.IBAN, i),
                                                   v_nIsDefault:=vPartyBankDetails(ENPartyBank.IsDefault, i),
                                                   sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchyName)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "EditPartyBank Failed", gPMConstants.PMELogLevel.PMLogError)
                        Else

                            nPartyBankIdForHistory = CInt(vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i))
                        End If

                        ' Change PN 62283
                        sDescription = New StringBuilder("Bank Details Edited from -")
                        sDescription.Append(CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountHolderName, 0)) &
                                            (If(gPMFunctions.ToSafeString(CStr(oOldPartyBankDet.GetValue(MainModule.ENPartyBank.BankNameId, 0))).Trim().Length = 0, " ", ", ")) &
                                        CStr(oOldPartyBankDet.GetValue(MainModule.ENPartyBank.BankNameId, 0)) &
                                            (If(gPMFunctions.ToSafeString(CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountNumber, 0))).Trim().Length = 0, " ", ", ")) &
                                            CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountNumber, 0)))

                        sDescription.Append(" TO ")
                        sDescription.Append(
                             CStr(vPartyBankDetails(MainModule.ENPartyBank.AccountHolderName, i)) &
                                 (If(Convert.ToString(oBankNameDesc).Trim().Length = 0, " ", ", ")) &
                                 oBankNameDesc &
                             (If(Convert.ToString(CStr(vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, 0))).Trim().Length = 0, " ", ", ")) &
                             CStr(vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, i)))
                        'End Cahnge 62283

                        m_lReturn = CType(CreateEvent(r_lEventCnt:=nEventCnt, v_lPartyCnt:=vPartyCnt,
                                                      v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today,
                                                      v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)

                        ' Generate Event Log
                        m_lReturn = CType(CreateEventLog(vPartyCnt:=vPartyCnt, sDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMDelete Then
                        sActionCode = "Inactivated"
                        ' Since we can't rely on the details coming from
                        ' front end as user can always edit data prior to its deletion so for generating event log we will pick data from DB
                        Dim sScreenHierarchyName As String = ""
                        If sScreenHierarchy <> "" Then
                            sScreenHierarchyName = sScreenHierarchy & $"Bank Payment Type({vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Description)})/Account Type({vPartyBankDetails(ENPartyBank.BankAccountTypeId, i)})"
                        End If
                        m_lReturn = CType(GetBankDetailsById(lPartyBankId:=(vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i)), vOldPartyBankDet:=oOldPartyBankDet), gPMConstants.PMEReturnCode)
                        m_lReturn = CType(DelUnDelPartyBank(lPartyBankId:=vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i), lDeleted:=1, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchyName), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "DeletePartyBank Failed", gPMConstants.PMELogLevel.PMLogError)
                        Else
                            nPartyBankIdForHistory = CInt(vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i))
                        End If


                        sDescription = New StringBuilder("Bank Details Inactivated from -")
                        sDescription.Append(CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountHolderName, 0)) & ", " &
                                             CStr(oOldPartyBankDet(MainModule.ENPartyBank.BankNameId, 0)) & ", " &
                                            CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountNumber, 0)))

                        m_lReturn = CType(CreateEvent(r_lEventCnt:=nEventCnt, v_lPartyCnt:=vPartyCnt,
                                                      v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today,
                                                      v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)

                        ' Generate Event Log
                        m_lReturn = CType(CreateEventLog(vPartyCnt:=vPartyCnt, sDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMReverse Then
                        sActionCode = "Activated"
                        ' Since we can't rely on the details coming from
                        ' front end as user can always edit data prior to its deletion so for generating event log we will pick data from DB
                        Dim sScreenHierarchyName As String = ""
                        If sScreenHierarchy <> "" Then
                            sScreenHierarchyName = sScreenHierarchy & $"Bank Payment Type({vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Description)})/Account Type({vPartyBankDetails(ENPartyBank.BankAccountTypeId, i)})"
                        End If
                        m_lReturn = CType(GetBankDetailsById(lPartyBankId:=vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i), vOldPartyBankDet:=oOldPartyBankDet), gPMConstants.PMEReturnCode)
                        m_lReturn = CType(DelUnDelPartyBank(lPartyBankId:=vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i), lDeleted:=0, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchyName), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "DeletePartyBank Failed", gPMConstants.PMELogLevel.PMLogError)
                        Else
                            nPartyBankIdForHistory = CInt(vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i))
                        End If
                        sDescription = New StringBuilder("Bank Details Activated from -")
                        sDescription.Append(
                                            CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountHolderName, 0)) & ", " &
                                            CStr(oOldPartyBankDet(MainModule.ENPartyBank.BankNameId, 0)) & ", " &
                                            CStr(oOldPartyBankDet(MainModule.ENPartyBank.AccountNumber, 0)))

                        m_lReturn = CType(CreateEvent(r_lEventCnt:=nEventCnt, v_lPartyCnt:=vPartyCnt,
                                                      v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today,
                                                      v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)

                        ' Generate Event Log
                        m_lReturn = CType(CreateEventLog(vPartyCnt:=vPartyCnt, sDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    ElseIf vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMDeleteFromDB Then

                        m_lReturn = CType(DeletePartyBank(lPartyBankId:=vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, i)), gPMConstants.PMEReturnCode)
                        ' Change  PN  62288
                        sDescription = New StringBuilder("Bank Detail Deleted ")
                        sDescription.Append(vPartyBankDetails(MainModule.ENPartyBank.AccountHolderName, i))
                        sDescription.Append(", ")
                        sDescription.Append(vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, i))

                        m_lReturn = CType(CreateEvent(r_lEventCnt:=nEventCnt, v_lPartyCnt:=vPartyCnt,
                                                      v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today,
                                                      v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)

                        ' Generate Event Log
                        m_lReturn = CType(CreateEventLog(vPartyCnt:=vPartyCnt, sDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)

                        ' End Chnage PN 62288
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    If vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMAdd OrElse
                        (vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMEdit AndAlso m_bIsBankEdited) OrElse
                        vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMDelete OrElse
                        vPartyBankDetails(0, i) = gPMConstants.PMEComponentAction.PMReverse Then

                        If CStr(vPartyBankDetails(ENPartyBank.IsDefault, i)) = "" Then
                            vPartyBankDetails(ENPartyBank.IsDefault, i) = "0"
                        End If

                        m_lReturn = AddPartyBankHistory(lPartyBankId:=nPartyBankIdForHistory,
                                                     sActionCode:=sActionCode,
                                                     vPartyCnt:=vPartyCnt,
                                                     vAccountID:=vPartyBankDetails(ENPartyBank.AccountId, i),
                                                     sAccHolderName:=vPartyBankDetails(ENPartyBank.AccountHolderName, i),
                                                     sAccNumber:=vPartyBankDetails(ENPartyBank.AccountNumber, i),
                                                     lBankPaymentTypeID:=vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Id),
                                                     sBankAccountType:=vPartyBankDetails(ENPartyBank.BankAccountTypeId, i),
                                                         vBankNameId:=oBankNameId,
                                                     sBankBranch:=vPartyBankDetails(ENPartyBank.BankBranch, i),
                                                     sBankBranchCode:=vPartyBankDetails(ENPartyBank.BankBranchCode, i),
                                                     sBankAdd1:=vPartyBankDetails(ENPartyBank.BankAdd1, i),
                                                     sBankAdd2:=vPartyBankDetails(ENPartyBank.BankAdd2, i),
                                                     sBankAdd3:=vPartyBankDetails(ENPartyBank.BankAdd3, i),
                                                     sBankTown:=vPartyBankDetails(ENPartyBank.BankTown, i),
                                                     sBankPCode:=vPartyBankDetails(ENPartyBank.BankPCode, i),
                                                     sBankRegion:=vPartyBankDetails(ENPartyBank.BankRegion, i),
                                                     sBankCountry:=vPartyBankDetails(ENPartyBank.BankCountry, i)(ENPMLookups.Id),
                                                     sCCNum:=vPartyBankDetails(ENPartyBank.CCNum, i),
                                                     sCCStartDate:=vPartyBankDetails(ENPartyBank.CCStartDate, i),
                                                     sCCExpiryDate:=vPartyBankDetails(ENPartyBank.CCExpiryDate, i),
                                                     sCCIssueNum:=vPartyBankDetails(ENPartyBank.CCIssueNum, i),
                                                     sCCPin:=vPartyBankDetails(ENPartyBank.CCPIN, i),
                                                         lIsRegistered:=vPartyBankDetails(ENPartyBank.IsRegistered, i),
                                                         sCCAdd1:=vPartyBankDetails(ENPartyBank.CCAdd1, i),
                                                         sCCAdd2:=vPartyBankDetails(ENPartyBank.CCAdd2, i),
                                                         sCCAdd3:=vPartyBankDetails(ENPartyBank.CCAdd3, i),
                                                         sCCTown:=vPartyBankDetails(ENPartyBank.CCTown, i),
                                                         sCCPCode:=vPartyBankDetails(ENPartyBank.CCPCode, i),
                                                         sCCCountry:=vPartyBankDetails(ENPartyBank.CCCountry, i)(ENPMLookups.Id),
                                                         lUserId:=1, sCCNameOnCard:=vPartyBankDetails(ENPartyBank.CCNameOnCard, i),
                                                         sCCManaulAuthNumber:=vPartyBankDetails(ENPartyBank.CCManualAuthorisationNum, i),
                                                         sBIC:=vPartyBankDetails(ENPartyBank.BIC, i),
                                                         sIBAN:=vPartyBankDetails(ENPartyBank.IBAN, i),
                                                         v_nIsDefault:=vPartyBankDetails(ENPartyBank.IsDefault, i))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "AddPartyBankHistory Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                Next i
            End If
            Return nResult
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return nResult
    End Function
    'Developer Guide No. Done as per vb code. 
    Public Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults(,) As Object, Optional ByRef v_Id As Object = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupsByEffectiveDate"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "table", v_sTableName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "Id", CStr(gPMFunctions.ToSafeLong(v_Id, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLookupsByEffectiveDateSQL, sSQLName:=ACGetLookupsByEffectiveDateName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetLookupsByEffectiveDateSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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


    Public Function GetBankDetailsById(ByVal lPartyBankId As Integer, ByRef vOldPartyBankDet(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBankDetailsById"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "party_bank_id", CStr(lPartyBankId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELPartyBankDetailsByIdSQL, sSQLName:=ACSELPartyBankDetailsByIdName, bStoredProcedure:=True, vResultArray:=vOldPartyBankDet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vOldPartyBankDet) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result
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



    ''' <summary>
    ''' Add a record in Party_Bank Table
    ''' </summary>
    ''' <param name="r_lPartyBankId"></param>
    ''' <param name="vPartyCnt"></param>
    ''' <param name="vAccountID"></param>
    ''' <param name="sAccHolderName"></param>
    ''' <param name="sAccNumber"></param>
    ''' <param name="lBankPaymentTypeID"></param>
    ''' <param name="sBankAccountType"></param>
    ''' <param name="lIsBank"></param>
    ''' <param name="vBankNameId"></param>
    ''' <param name="sBankBranch"></param>
    ''' <param name="sBankBranchCode"></param>
    ''' <param name="sBankAdd1"></param>
    ''' <param name="sBankAdd2"></param>
    ''' <param name="sBankAdd3"></param>
    ''' <param name="sBankTown"></param>
    ''' <param name="sBankPCode"></param>
    ''' <param name="sBankRegion"></param>
    ''' <param name="sBankCountry"></param>
    ''' <param name="sCCNum"></param>
    ''' <param name="sCCStartDate"></param>
    ''' <param name="sCCExpiryDate"></param>
    ''' <param name="sCCIssueNum"></param>
    ''' <param name="sCCPin"></param>
    ''' <param name="lIsRegistered"></param>
    ''' <param name="sCCAdd1"></param>
    ''' <param name="sCCAdd2"></param>
    ''' <param name="sCCAdd3"></param>
    ''' <param name="sCCTown"></param>
    ''' <param name="sCCPCode"></param>
    ''' <param name="sCCCountry"></param>
    ''' <param name="sCCNameOnCard"></param>
    ''' <param name="sCCManaulAuthNumber"></param>
    ''' <param name="sBIC"></param>
    ''' <param name="sIBAN"></param>
    ''' <param name="v_nIsDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyBank(ByRef r_lPartyBankId As Integer,
                                 ByVal vPartyCnt As Object,
                                 ByVal vAccountID As Object,
                                 ByVal sAccHolderName As String,
                                 ByVal sAccNumber As String,
                                 ByVal lBankPaymentTypeID As Integer,
                                 ByVal sBankAccountType As String,
                                 ByVal lIsBank As Integer,
                                 ByVal vBankNameId As Object,
                                 ByVal sBankBranch As String,
                                 ByVal sBankBranchCode As String,
                                 ByVal sBankAdd1 As String,
                                 ByVal sBankAdd2 As String,
                                 ByVal sBankAdd3 As String,
                                 ByVal sBankTown As String,
                                 ByVal sBankPCode As String,
                                 ByVal sBankRegion As String,
                                 ByVal sBankCountry As String,
                                 ByVal sCCNum As String,
                                 ByVal sCCStartDate As String,
                                 ByVal sCCExpiryDate As String,
                                 ByVal sCCIssueNum As String,
                                 ByVal sCCPin As String,
                                 ByVal lIsRegistered As Integer,
                                 ByVal sCCAdd1 As String,
                                 ByVal sCCAdd2 As String,
                                 ByVal sCCAdd3 As String,
                                 ByVal sCCTown As String,
                                 ByVal sCCPCode As String,
                                 ByVal sCCCountry As String,
                                 ByVal sCCNameOnCard As String,
                                 ByVal sCCManaulAuthNumber As String,
                                 ByVal sBIC As String,
                                 ByVal sIBAN As String,
                                 Optional ByVal v_nIsDefault As Integer = 0,
                                 Optional ByVal sUniqueId As String = "",
                                 Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "AddPartyBank"

        Try
            If Not (Convert.IsDBNull(vPartyCnt) OrElse Informations.IsNothing(vPartyCnt)) OrElse Not (Convert.IsDBNull(vAccountID) OrElse Informations.IsNothing(vAccountID)) Then
                ' Add Input parameters for both PartyCnt and AccountId
                m_oDatabase.Parameters.Clear()
                If Not Informations.IsDBNull(vBankNameId) AndAlso ((Informations.IsNothing(vBankNameId)) OrElse vBankNameId = 0) Then
                    vBankNameId = DBNull.Value
                End If
                AddParameterLite(m_oDatabase, "party_bank_id", CStr(r_lPartyBankId), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_id", vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_holder_name", sAccHolderName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "account_number", sAccNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_payment_type_id", CStr(lBankPaymentTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_type", sBankAccountType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "is_bank", CStr(lIsBank), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "bank_name_id", vBankNameId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "bank_branch", sBankBranch, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_branch_code", sBankBranchCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add1", sBankAdd1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add2", sBankAdd2, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add3", sBankAdd3, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_town", sBankTown, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_PCode", sBankPCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_region", sBankRegion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_country", sBankCountry, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_num", sCCNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_start_date", sCCStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_expiry_date", sCCExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_issue_num", sCCIssueNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_pin", sCCPin, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "is_registered", CStr(lIsRegistered), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "cc_add1", sCCAdd1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_add2", sCCAdd2, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_add3", sCCAdd3, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_town", sCCTown, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_pcode", sCCPCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_country", sCCCountry, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "name_on_card", sCCNameOnCard, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "manual_auth_number", sCCManaulAuthNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "sBusinessIdentifierCode", sBIC, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "sInternationalBankAccountNumber", sIBAN, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "IsDefault", v_nIsDefault, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDPartyBankDetailsSQL, sSQLName:=ACADDPartyBankDetailsName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACADDPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Else
                    r_lPartyBankId = m_oDatabase.Parameters.Item("party_bank_id").Value
                End If
            End If

            Return nResult
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lPartyBankId"></param>
    ''' <param name="vPartyCnt"></param>
    ''' <param name="vAccountID"></param>
    ''' <param name="sAccHolderName"></param>
    ''' <param name="sAccNumber"></param>
    ''' <param name="lBankPaymentTypeID"></param>
    ''' <param name="sBankAccountType"></param>
    ''' <param name="lIsBank"></param>
    ''' <param name="vBankNameId"></param>
    ''' <param name="sBankBranch"></param>
    ''' <param name="sBankBranchCode"></param>
    ''' <param name="sBankAdd1"></param>
    ''' <param name="sBankAdd2"></param>
    ''' <param name="sBankAdd3"></param>
    ''' <param name="sBankTown"></param>
    ''' <param name="sBankPCode"></param>
    ''' <param name="sBankRegion"></param>
    ''' <param name="sBankCountry"></param>
    ''' <param name="sCCNum"></param>
    ''' <param name="sCCStartDate"></param>
    ''' <param name="sCCExpiryDate"></param>
    ''' <param name="sCCIssueNum"></param>
    ''' <param name="sCCPin"></param>
    ''' <param name="lIsRegistered"></param>
    ''' <param name="sCCAdd1"></param>
    ''' <param name="sCCAdd2"></param>
    ''' <param name="sCCAdd3"></param>
    ''' <param name="sCCTown"></param>
    ''' <param name="sCCPCode"></param>
    ''' <param name="sCCCountry"></param>
    ''' <param name="sCCNameOnCard"></param>
    ''' <param name="sCCManaulAuthNumber"></param>
    ''' <param name="bEditInstalmentPlans"></param>
    ''' <param name="sBIC"></param>
    ''' <param name="sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditPartyBank(ByVal lPartyBankId As Integer,
                                  ByVal vPartyCnt As Object,
                                  ByVal vAccountID As Object,
                                  ByVal sAccHolderName As String,
                                  ByVal sAccNumber As String,
                                  ByVal lBankPaymentTypeID As Integer,
                                  ByVal sBankAccountType As String,
                                  ByVal lIsBank As Integer,
                                  ByVal vBankNameId As Object,
                                  ByVal sBankBranch As String,
                                  ByVal sBankBranchCode As String,
                                  ByVal sBankAdd1 As String,
                                  ByVal sBankAdd2 As String,
                                  ByVal sBankAdd3 As String,
                                  ByVal sBankTown As String,
                                  ByVal sBankPCode As String,
                                  ByVal sBankRegion As String,
                                  ByVal sBankCountry As String,
                                  ByVal sCCNum As String,
                                  ByVal sCCStartDate As String,
                                  ByVal sCCExpiryDate As String,
                                  ByVal sCCIssueNum As String,
                                  ByVal sCCPin As String,
                                  ByVal lIsRegistered As Integer,
                                  ByVal sCCAdd1 As String,
                                  ByVal sCCAdd2 As String,
                                  ByVal sCCAdd3 As String,
                                  ByVal sCCTown As String,
                                  ByVal sCCPCode As String,
                                  ByVal sCCCountry As String,
                                  ByVal sCCNameOnCard As String,
                                  ByVal sCCManaulAuthNumber As String,
                                  Optional ByVal bEditInstalmentPlans As Boolean = False,
                                  Optional sBIC As String = "",
                                  Optional sIBAN As String = "",
                                  Optional v_nIsDefault As Integer = 0,
                                 Optional ByVal sUniqueId As String = "",
                                 Optional ByVal sScreenHierarchy As String = "") As Integer


        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "EditPartyBank"
        Try
            If Not (Convert.IsDBNull(vPartyCnt) Or Informations.IsNothing(vPartyCnt)) Or Not (Convert.IsDBNull(vAccountID) Or Informations.IsNothing(vAccountID)) Then
                ' Add Input parameters for both PartyCnt and AccountId

                m_oDatabase.Parameters.Clear()
                If Not Informations.IsDBNull(vBankNameId) AndAlso ((Informations.IsNothing(vBankNameId)) OrElse vBankNameId = 0) Then
                    vBankNameId = DBNull.Value
                End If
                AddParameterLite(m_oDatabase, "party_bank_id", CStr(lPartyBankId), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_id", vAccountID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_holder_name", sAccHolderName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "account_number", sAccNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_payment_type_id", CStr(lBankPaymentTypeID), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                If sBankAccountType <> "" Then
                    AddParameterLite(m_oDatabase, "account_type", sBankAccountType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                End If
                AddParameterLite(m_oDatabase, "is_bank", CStr(lIsBank), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "bank_name_id", vBankNameId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "bank_branch", sBankBranch, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_branch_code", sBankBranchCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add1", sBankAdd1, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add2", sBankAdd2, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add3", sBankAdd3, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_town", sBankTown, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_PCode", sBankPCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_region", sBankRegion, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_country", sBankCountry, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_num", sCCNum, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_start_date", sCCStartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_expiry_date", sCCExpiryDate, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_issue_num", sCCIssueNum, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_pin", sCCPin, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "is_registered", CStr(lIsRegistered), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "cc_add1", sCCAdd1, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_add2", sCCAdd2, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_add3", sCCAdd3, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_town", sCCTown, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_pcode", sCCPCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_country", sCCCountry, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "name_on_card", sCCNameOnCard, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "manual_auth_number", sCCManaulAuthNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                If bEditInstalmentPlans Then
                    AddParameterLite(m_oDatabase, "UpdatePFPremiumFinance", CStr(1), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                End If
                AddParameterLite(m_oDatabase, "IsEdited", CStr(0), PMEParameterDirection.PMParamOutput, PMEDataType.PMBoolean)
                AddParameterLite(m_oDatabase, "sBusinessIdentifierCode", sBIC, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "sInternationalBankAccountNumber", sIBAN, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "IsDefault", CStr(v_nIsDefault), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "UniqueId", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDPartyBankDetailsSQL, sSQLName:=ACUPDPartyBankDetailsName, bStoredProcedure:=True)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACUPDPartyBankDetailsSQL & " Failed", PMELogLevel.PMLogError)
                Else

                    m_bIsBankEdited = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("IsEdited").Value)
                End If

                Return nResult
            End If

            Return nResult
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            ' Return nResult
        End Try
    End Function

    Public Function DeletePartyBank(ByVal lPartyBankId As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "DeletePartyBank"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide No 44
            If Not (Convert.IsDBNull(lPartyBankId) OrElse Informations.IsNothing(lPartyBankId)) Then
                ' Add Input parameters for both PartyCnt and AccountId

                ' Clear Down Database Parameters

                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                AddParameterLite(m_oDatabase, "party_bank_id", CStr(lPartyBankId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)



                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELDBPartyBankDetailsSQL, sSQLName:=ACDELDBPartyBankDetailsName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACDELDBPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Return result
            End If

            Return result
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

    Public Function ISPartyBankActiveTransactions(ByVal lPartyBankId As Integer, ByRef lIsExists As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ISPartyBankActiveTransactions"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "Party_Bank_Id", CStr(lPartyBankId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No. 85
            AddParameterLite(m_oDatabase, "ActiveTransExists", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACPartyBankActiveTransactionsSQL, sSQLName:=ACPartyBankActiveTransactionsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACPartyBankActiveTransactionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                lIsExists = m_oDatabase.Parameters.Item("ActiveTransExists").Value
            End If

            Return result
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

    Public Function DelUnDelPartyBank(ByVal lPartyBankId As Integer, ByVal lDeleted As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DelUnDelPartyBank"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "party_bank_id", CStr(lPartyBankId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "delete", CStr(lDeleted), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "unique_id", sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "screen_hierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELPartyBankDetailsSQL, sSQLName:=ACDELPartyBankDetailsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDELPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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

    ''' <summary>
    ''' AddPartyBankHistory
    ''' </summary>
    ''' <param name="lPartyBankId"></param>
    ''' <param name="sActionCode"></param>
    ''' <param name="vPartyCnt"></param>
    ''' <param name="vAccountID"></param>
    ''' <param name="sAccHolderName"></param>
    ''' <param name="sAccNumber"></param>
    ''' <param name="lBankPaymentTypeID"></param>
    ''' <param name="sBankAccountType"></param>
    ''' <param name="vBankNameId"></param>
    ''' <param name="sBankBranch"></param>
    ''' <param name="sBankBranchCode"></param>
    ''' <param name="sBankAdd1"></param>
    ''' <param name="sBankAdd2"></param>
    ''' <param name="sBankAdd3"></param>
    ''' <param name="sBankTown"></param>
    ''' <param name="sBankPCode"></param>
    ''' <param name="sBankRegion"></param>
    ''' <param name="sBankCountry"></param>
    ''' <param name="sCCNum"></param>
    ''' <param name="sCCStartDate"></param>
    ''' <param name="sCCExpiryDate"></param>
    ''' <param name="sCCIssueNum"></param>
    ''' <param name="sCCPin"></param>
    ''' <param name="lIsRegistered"></param>
    ''' <param name="sCCAdd1"></param>
    ''' <param name="sCCAdd2"></param>
    ''' <param name="sCCAdd3"></param>
    ''' <param name="sCCTown"></param>
    ''' <param name="sCCPCode"></param>
    ''' <param name="sCCCountry"></param>
    ''' <param name="lUserId"></param>
    ''' <param name="sCCNameOnCard"></param>
    ''' <param name="sCCManaulAuthNumber"></param>
    ''' <param name="sBIC"></param>
    ''' <param name="sIBAN"></param>
    ''' <param name="v_nIsDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyBankHistory(ByVal lPartyBankId As Integer,
                                        ByVal sActionCode As String,
                                        ByVal vPartyCnt As Object,
                                        ByVal vAccountID As Object,
                                        ByVal sAccHolderName As String,
                                        ByVal sAccNumber As String,
                                        ByVal lBankPaymentTypeID As Integer,
                                        ByVal sBankAccountType As String,
                                        ByVal vBankNameId As Object,
                                        ByVal sBankBranch As String,
                                        ByVal sBankBranchCode As String,
                                        ByVal sBankAdd1 As String,
                                        ByVal sBankAdd2 As String,
                                        ByVal sBankAdd3 As String,
                                        ByVal sBankTown As String,
                                        ByVal sBankPCode As String,
                                        ByVal sBankRegion As String,
                                        ByVal sBankCountry As String,
                                        ByVal sCCNum As String,
                                        ByVal sCCStartDate As String,
                                        ByVal sCCExpiryDate As String,
                                        ByVal sCCIssueNum As String,
                                        ByVal sCCPin As String,
                                        ByVal lIsRegistered As Integer,
                                        ByVal sCCAdd1 As String,
                                        ByVal sCCAdd2 As String,
                                        ByVal sCCAdd3 As String,
                                        ByVal sCCTown As String,
                                        ByVal sCCPCode As String,
                                        ByVal sCCCountry As String,
                                        ByVal lUserId As Integer,
                                        ByVal sCCNameOnCard As String,
                                        ByVal sCCManaulAuthNumber As String,
                                        ByVal sBIC As String,
                                        ByVal sIBAN As String,
                                        Optional ByVal v_nIsDefault As Integer = 0) As Integer


        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "AddPartyBankHistory"
        Try

            If Not (Convert.IsDBNull(vPartyCnt) OrElse Informations.IsNothing(vPartyCnt)) OrElse Not (Convert.IsDBNull(vAccountID) OrElse Informations.IsNothing(vAccountID)) Then
                m_oDatabase.Parameters.Clear()

                If Not Informations.IsDBNull(vBankNameId) AndAlso ((Informations.IsNothing(vBankNameId)) OrElse vBankNameId = 0) Then
                    vBankNameId = DBNull.Value
                End If

                AddParameterLite(m_oDatabase, "party_bank_id", CStr(lPartyBankId), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "action_code", sActionCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_id", vAccountID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "account_holder_name", sAccHolderName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "account_number", sAccNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_payment_type_id", CStr(lBankPaymentTypeID), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                If sBankAccountType <> "" Then
                    AddParameterLite(m_oDatabase, "account_type", sBankAccountType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                End If
                AddParameterLite(m_oDatabase, "bank_name_id", vBankNameId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "bank_branch", sBankBranch, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_branch_code", sBankBranchCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add1", sBankAdd1, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add2", sBankAdd2, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_add3", sBankAdd3, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_town", sBankTown, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_PCode", sBankPCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_region", sBankRegion, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "bank_country", sBankCountry, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_num", sCCNum, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_start_date", sCCStartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_expiry_date", sCCExpiryDate, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_issue_num", sCCIssueNum, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

                AddParameterLite(m_oDatabase, "cc_pin", sCCPin, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "is_registered", CStr(lIsRegistered), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "cc_add1", sCCAdd1, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_add2", sCCAdd2, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_add3", sCCAdd3, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_town", sCCTown, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_pcode", sCCPCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "cc_country", sCCCountry, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "user_id", CStr(m_iUserID), PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "name_on_card", sCCNameOnCard, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "manual_auth_number", sCCManaulAuthNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "sBusinessIdentifierCode", sBIC, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "sInternationalBankAccountNumber", sIBAN, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                AddParameterLite(m_oDatabase, "IsDefault", v_nIsDefault, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDPartyBankHistorySQL, sSQLName:=ACADDPartyBankHistoryName, bStoredProcedure:=True)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACADDPartyBankHistorySQL & " Failed", PMELogLevel.PMLogError)
                End If

            End If

            Return nResult
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally
        End Try
        Return nResult
    End Function
    Public Function ISPartyBankLinkedWithInstalment(ByVal lPartyBankId As Integer, ByRef bisLinked As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ISPartyBankLinkedWithInstalment"
        Dim lReturn As Integer = 0
        Dim vResults(,) As Object = Nothing
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "party_bank_id", CStr(lPartyBankId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIspartyBankLinkedWithInstalmentsSQL, sSQLName:=ACIspartyBankLinkedWithInstalmentsName, bStoredProcedure:=True, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACIspartyBankLinkedWithInstalmentsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(vResults) Then
                bisLinked = True
            End If
            Return result
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












    'Developer Guide No. 119
    Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oEvent Is Nothing Then
            m_oEvent = New bSIREvent.Business()
            m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oEvent.Initialise", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)

        Return result
    End Function



    ' ***************************************************************** '
    ' Name: BeginTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BeginTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oDatabase.SQLBeginTrans

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLBeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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
    ' Name: RollbackTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oDatabase.SQLRollbackTrans

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLRollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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
    ' Name: CommitTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CommitTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oDatabase.SQLCommitTrans

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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
    ' Name: CreateEventLog
    '
    ' Parameters: PartyCnt, Description
    '
    ' Description: To Generate Event Log
    '
    ' History:
    ' ***************************************************************** '
    'Developer Guide no 101
    Private Function CreateEventLog(ByVal vPartyCnt As Object, ByVal sDescription As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateEventLog"

        Dim r_vResults(,) As Object = Nothing
        Dim vPublicTextId As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        If ToSafeDouble(vPartyCnt) > 0 Then

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetEventLogSQL, sSQLName:=ACGetEventLogName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetEventLogSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(r_vResults) Then

                For lCount As Integer = 0 To r_vResults.GetUpperBound(1)

                    vPublicTextId = CStr(r_vResults(1, lCount))
                Next
                vPublicTextId = CStr(CDbl(vPublicTextId) + 1)
            Else
                vPublicTextId = CStr(1)
            End If


            m_oDatabase.Parameters.Clear()
            AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "party_public_text_id", vPublicTextId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            'Developer Guide No. 40
            AddParameterLite(m_oDatabase, "text_line", "[SIRIUS " & DateTime.Now & "]" & Strings.ChrW(9) & Strings.ChrW(9), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddEventLogSQL, sSQLName:=ACAddEventLogName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACAddEventLogSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_oDatabase.Parameters.Clear()
            vPublicTextId = CStr(CDbl(vPublicTextId) + 1)
            AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "party_public_text_id", vPublicTextId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "text_line", sDescription & Strings.ChrW(9) & Strings.ChrW(9), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddEventLogSQL, sSQLName:=ACAddEventLogName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACAddEventLogSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        Return result
    End Function

    'Developer Guide No 101
    Public Function GetPartyAccountTypes(ByRef vPartyAccountsType(,) As Object, ByVal vPartyCnt As Object, ByVal vAccountID As Object, ByVal sBankPaymentTypeCode As String, Optional ByVal vIsBank As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyAccountTypes"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()


            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "AccountID", vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "PartyCnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "BankPaymentTypeCode", sBankPaymentTypeCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If Not Informations.IsNothing(vIsBank) AndAlso CStr(vIsBank) <> "" Then
                AddParameterLite(m_oDatabase, "ISBank", CStr(vIsBank), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELPartyAccountsTypeSQL, sSQLName:=ACSELPartyAccountsTypeName, bStoredProcedure:=True, vResultArray:=vPartyAccountsType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELPartyAccountsTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vPartyAccountsType) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                'Else
                '   m_lReturn = MergeAllLookups(vResultArray:=vPartyBankDetails)
            End If

            Return result

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

    Public Function isExistPaymentType(ByVal lPartyCnt As Integer, ByVal lBankPaymentTypeID As Integer, ByVal sBankAccountType As String, ByRef lIsEntryExists As Integer, ByRef lIsAccountTypeNullExists As Integer, ByRef lIsDuplicateAccountExists As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "isExistPaymentType"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "Party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "Bank_Payment_Type_Id", CStr(lBankPaymentTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "Account_type", sBankAccountType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            'Developer Guide No. 85
            AddParameterLite(m_oDatabase, "IsEntryExits", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No. 85
            AddParameterLite(m_oDatabase, "IsAccountTypeNullExists", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No. 85
            AddParameterLite(m_oDatabase, "IsDuplicateAccountExists", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSELPaymentTypeSQL, sSQLName:=ACSELPaymentTypeName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDELPartyBankDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                lIsEntryExists = m_oDatabase.Parameters.Item("IsEntryExits").Value

                lIsAccountTypeNullExists = m_oDatabase.Parameters.Item("IsAccountTypeNullExists").Value

                lIsDuplicateAccountExists = m_oDatabase.Parameters.Item("IsDuplicateAccountExists").Value
            End If


            Return result
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
    ''' <summary>
    ''' Check Manual auth code is present in database
    ''' </summary>
    ''' <param name="sManualAuthCode"></param>
    ''' <param name="nAccountId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckManualAuthCodeIsInUse(ByVal sManualAuthCode As String, ByVal nAccountId As Integer,
                                               ByRef bIsEntryExists As Boolean) As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            AddParameterLite(m_oDatabase, "nAccountId", nAccountId,
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "sManualAuthCode", sManualAuthCode,
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "bIsEntryExits", DBNull.Value,
                                             gPMConstants.PMEParameterDirection.PMParamOutput,
                                             gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kCheckManualAuthCodeSQL, sSQLName:=kCheckManualAuthCodeName,
                                              bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("CheckManualAuthCodeIsInUse", kCheckManualAuthCodeSQL & " Failed",
                                        gPMConstants.PMELogLevel.PMLogError)
            Else
                bIsEntryExists = m_oDatabase.Parameters.Item("bIsEntryExits").Value
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If
            Return nResult
        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckManualAuthCodeIsInUse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckManualAuthCodeIsInUse", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try

    End Function
    'Developer Guide No 101
    Public Function GetPartyName(ByVal vPartyCnt As Object, ByVal vAccountID As Object, ByRef vPartyName As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyName"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (Convert.IsDBNull(vPartyCnt) Or Informations.IsNothing(vPartyCnt)) Then
                ' Add Input parameter PartyCnt
                ' Clear Down Database Parameters

                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "Account_Id", vAccountID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "Party_Name", vPartyName, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELGetPartyNameSQL, sSQLName:=ACSELGetPartyName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELGetPartyNameSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Else

                    vPartyName = m_oDatabase.Parameters.Item("Party_Name").Value
                End If

                Return result
            End If

            Return result
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


End Class