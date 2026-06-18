Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no 129. 
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 14/07/2000
    '
    ' Description: Creatable Bussiness class which contains all the
    '              methods, business rules required for the
    '              SIRFindClaim .
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' SET 01082002 - Removed for scalability
    'Private oComponentServices As PMServerBusinessCS

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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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
    ' Date :15/07/2000
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

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAttachedBranches(ByRef vAttachBranches(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAttachedBranches"
        Dim vBranches(,) As Object = Nothing
        Dim vMergeArray(,) As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (vAttachBranches Is Nothing) Then
                For lCount As Integer = 0 To vAttachBranches.GetUpperBound(1)

                    ' Add Input parameters for both PartyCnt and AccountId

                    ' Clear Down Database Parameters
                    m_oDatabase.Parameters.Clear()

                    ' Add Required Stored Procedure Parameters
                    bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Number", vAttachBranches(MainModule.ENBankGuarantee.BGId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteeBranchesSQL, sSQLName:=ACSELBankGuaranteeBranchesName, bStoredProcedure:=True, vResultArray:=vBranches)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Information.IsArray(vBranches) Then

                        ReDim vMergeArray(vBranches.GetUpperBound(1), 1)

                        For lMergeCount As Integer = 0 To vMergeArray.GetUpperBound(0)


                            vMergeArray(lMergeCount, 0) = vBranches(0, lMergeCount)


                            vMergeArray(lMergeCount, 1) = vBranches(1, lMergeCount)
                        Next


                        vAttachBranches(MainModule.ENBankGuarantee.Branches, lCount) = vMergeArray
                    End If

                Next
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function GetAttachedProducts(ByRef vAttachProducts(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetAttachedProducts"


        Dim vProducts(,) As Object = Nothing
        Try


            Dim vMergeArray As Object
            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (vAttachProducts Is Nothing) Then
                For lCount As Integer = 0 To vAttachProducts.GetUpperBound(1)

                    ' Add Input parameters for both PartyCnt and AccountId

                    ' Clear Down Database Parameters
                    m_oDatabase.Parameters.Clear()

                    ' Add Required Stored Procedure Parameters
                    bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Number", vAttachProducts(MainModule.ENBankGuarantee.BGId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteeProductsSQL, sSQLName:=ACSELBankGuaranteeProductsName, bStoredProcedure:=True, vResultArray:=vProducts)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeProductsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Information.IsArray(vProducts) Then

                        ReDim vMergeArray(vProducts.GetUpperBound(1), 1)

                        For lMergeCount As Integer = 0 To vProducts.GetUpperBound(1)


                            vMergeArray(lMergeCount, 0) = vProducts(0, lMergeCount)


                            vMergeArray(lMergeCount, 1) = vProducts(1, lMergeCount)
                        Next


                        vAttachProducts(MainModule.ENBankGuarantee.Products, lCount) = vMergeArray
                    End If

                Next
            End If
            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name:Get Bank Guarantee Details
    '
    ' Description:  SQL Query to Select Claim details
    '
    ' Date :
    '
    ' Edit History
    '
    '
    '
    ' ***************************************************************** '
    Public Function GetValidBGsforParty(ByVal vPartyCnt As Object, ByVal vSourceId As Object, ByVal vProductId As Object, ByVal vDate As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetValidBGsforParty"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Source_Id", vSourceId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Product_Id", vProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Date", vDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetValidPartyBGDetailsSQL, sSQLName:=ACGetValidPartyBGDetailsName, bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetBankGuarenteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else

                m_lReturn = CType(MergeAllLookups(vResultArray:=r_vResultArray), gPMConstants.PMEReturnCode)


                m_lReturn = CType(GetAttachedProducts(vAttachProducts:=r_vResultArray), gPMConstants.PMEReturnCode)


                m_lReturn = CType(GetAttachedBranches(vAttachBranches:=r_vResultArray), gPMConstants.PMEReturnCode)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name:Get Bank Guarantee Details
    '
    ' Description:  SQL Query to Select Claim details
    '
    ' Date :
    '
    ' Edit History
    '
    '
    '
    ' ***************************************************************** '
    Public Function GetBankGuaranteeDetails(ByVal vPartyCode As Object, ByVal vAgentCode As Object, ByVal vBankGuaranteeRef As Object, ByVal vInsuranceFileRef As Object, ByVal vBankName As Object, ByVal vBGStatusId As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBankGuaranteeDetails"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            bPMAddParameter.AddParameterLite(m_oDatabase, "party_code", vPartyCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "agent_code", vAgentCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_ref", vInsuranceFileRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "bank_guarantee_ref", vBankGuaranteeRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "bank_name", vBankName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "BG_status_id", vBGStatusId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetBankGuarenteeDetailsSQL, sSQLName:=kGetBankGuarenteeDetailsName, bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetBankGuarenteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                'm_lReturn = MergeAllLookups(vResultArray:=r_vResultArray)

                m_lReturn = CType(GetBankdetails(r_vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetBankdetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = CType(GetAttachedProducts(vAttachProducts:=r_vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetAttachedProducts Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = CType(GetAttachedBranches(vAttachBranches:=r_vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetAttachedBranches Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function GetBankdetails(ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBankdetails"

        Dim lReturn As Integer
        Dim vMergeLookup(1) As Object
        Dim r_vResults(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Bank_Id", vResultArray(MainModule.ENBankGuarantee.BankNameId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_Select_Bank", sSQLName:="", bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetLookupsByEffectiveDateSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf Information.IsArray(r_vResults) Then
                'developer guide no.(Added the following code to create new instance of vMergeLookUp
                ReDim vMergeLookup(1)


                vMergeLookup(kLookId) = r_vResults(0, 0)


                vMergeLookup(kLookDesc) = r_vResults(3, 0)

                vResultArray(MainModule.ENBankGuarantee.BankNameId, lCount) = vMergeLookup
            End If

        Next
        Return result
    End Function

    Private Function MergeAllLookups(ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "MergeAllLookups"

        Dim vLookUp As Object = Nothing
        Dim vID As Object = Nothing
        Dim vMergeLookup(1) As Object


        result = gPMConstants.PMEReturnCode.PMTrue
        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

            m_lReturn = CType(MergeLookupValues(ENMergeColumn:=MainModule.ENBankGuarantee.BankNameId, vIdValue:=vResultArray(MainModule.ENBankGuarantee.BankNameId, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                vResultArray(MainModule.ENBankGuarantee.BankNameId, lCount) = vMergeLookup
            End If


        Next
        Return result
    End Function

    Private Function MergeLookupValues(ByVal ENMergeColumn As MainModule.ENBankGuarantee, ByVal vIdValue As Object, ByRef vLookUp As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "MergeLookupValues"
        Dim IsLookup As Boolean
        Dim sTableName As String = ""
        Dim sSPName As String = ""
        Dim sColumnName As Object


        result = gPMConstants.PMEReturnCode.PMTrue
        If ENMergeColumn = MainModule.ENBankGuarantee.BankNameId Then
            sTableName = "Bank"
            IsLookup = False
            'sSPName = "spu_ACT_Select_Bank"
            'Dim IdField(1) As Variant
            'IdField(0) = "Bank_id"
            'IdField(1) = "bank_id"

            'ReDim sColumnName(2)
            'sColumnName(0) = IdField
            'sColumnName(1) = "Bank_Name"
            'sColumnName(2) = "Code"
        End If

        If IsLookup Then
            m_lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=sTableName, r_vResults:=vLookUp, v_Id:=vIdValue), gPMConstants.PMEReturnCode)
            'Else
            '        m_lReturn = GetNonLookupValues(v_sSPName:=sSPName, _
            'r_vResults:=vLookUp, _
            'sColumns:=sColumnName, _
            'v_Id:=vIdValue)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

        End If
        Return result
    End Function

    'Private Function GetNonLookupValues(v_sSPName As String, _
    ''                                                    r_vResults As Variant, _
    ''                                                    sColumns As Variant, _
    ''                                                    v_Id As Long) As Long
    '
    'Const kMethodName As String = "GetLookupsByEffectiveDate"
    '
    '    Dim lReturn As Long
    '
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    GetLookupsByEffectiveDate = PMTrue
    '
    '    ' Clear Down Database Parameters
    '    m_oDatabase.Parameters.Clear
    '
    '    ' Add Required Stored Procedure Parameters
    '    Call AddParameterLite(m_oDatabase, sColumns(0)(1), v_Id, PMParamInput, PMLong)
    '
    '    ' Execute selection Query
    '    m_lReturn = m_oDatabase.SQLSelect( _
    ''                            sSQL:=v_sSPName, _
    ''                            sSQLName:="", _
    ''                            bStoredProcedure:=True, _
    ''                            vResultArray:=r_vResults, _
    ''                            lNumberRecords:=PMAllRecords)
    '
    '    If IsArray(r_vResults) Then
    '
    '    If m_lReturn <> PMTrue Then
    '        RaiseError kMethodName, ACGetLookupsByEffectiveDateSQL & " Failed", PMLogError
    '    End If
    '
    '    GoTo Finally
    '
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sUsername:=m_sUsername, _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=GetLookupsByEffectiveDate
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '
    '    Resume
    '
    'End Function
    Public Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults(,) As Object, Optional ByRef v_Id As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupsByEffectiveDate"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "table", v_sTableName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Id", v_Id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

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

        End Try
        Return result
    End Function

    'Start - Sankar - Bank Guarantee Bug Fixing - 22/01/09
    Public Function GetPartyShortname(ByVal lPartyCnt As Integer, ByRef sPartyCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyShortname"

        Dim r_vResults(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLookupShortnameSQL, sSQLName:=ACGetLookupShortnameName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If Information.IsArray(r_vResults) Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACGetLookupShortnameSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Else

                    sPartyCode = CStr(r_vResults(0, 0)).Trim()
                End If
            Else
                sPartyCode = ""
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    'End - Sankar - Bank Guarantee Bug Fixing - 22/01/09
    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
