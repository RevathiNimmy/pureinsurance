Option Strict Off
Option Explicit On
Imports System.Text
'developer guide no. 129 (guide)
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

            Dim vDatabase As New dPMDAO.Database

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


    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History:
    '
    ' ***************************************************************** '
    'developer guide no. 33
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray As Object, ByRef vResultArray(,) As Object, Optional ByRef sWhereClause As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PickListLoad"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)



                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                Next iParam


                'developer guide no. To Check 39

                m_lReturn = .SQLSelect("spu_BankGuarantee_PLL" &
                   sPickListType, sPickListType & " PickList Load", True, , vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeProductsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End With

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
    '
    ' Name: PickListSave
    '
    ' Description:
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "PickListSave"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            BeginTrans()

            If vFKArray.GetUpperBound(1) > 2 And sPickListType.Trim().ToUpper() = "SOURCE" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam

                'developer guide no. 39
                m_lReturn = .SQLAction("spe_PFScheme_PLD" &
                   sPickListType, sPickListType & " PickList Delete", True)

                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()

                        'Load the FK parameters
                        For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                            .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Next iParam


                        .Parameters.Add("Key", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        'Call the SP
                        'developer guide no. 39
                        m_lReturn = .SQLAction("spu_BankGuarantee_PLS" & sPickListType, sPickListType & " PickList Load", True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next iKey
                End If
            End With

            CommitTrans()

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
    '
    ' Name: PickListParams
    '
    ' Description: Returns a string of question marks for the SP definition
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '

    Private Function PickListParams(ByRef vParams As Object) As String

        Dim sComma As String = ""
        Dim sParam As New StringBuilder
        'developer guide no. 101
        For iParam As Integer = vParams.GetLowerBound(0) To vParams.GetUpperBound(0)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam

        Return sParam.ToString()

    End Function

    Public Function GetValidBGsOnPolicy(ByRef lProductId As Integer, ByRef lSourceId As Integer, ByRef dtCoverFromDate As Date, ByRef lInsuranceFileCnt As Integer, ByRef lTransactionCurrencyId As Integer, ByRef lPartyCnt As Integer, ByRef crTotalPremium As Decimal, ByRef vBankGuaranteeDetails(,) As Object, ByRef r_dtDueDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetValidBGsOnPolicy"
        'Dim vBranches As Object
        'Dim lCount As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", lProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", lSourceId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "cover_from_date", dtCoverFromDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Total_Premium", crTotalPremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'Start - Sankar - Bank Guarantee Bug Fixing
            bPMAddParameter.AddParameterLite(m_oDatabase, "TranCurrency_Id", lTransactionCurrencyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "DueDate", r_dtDueDate, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)
            'End - Sankar - Bank Guarantee Bug Fixing

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELValidBGsForPolicySQL, sSQLName:=ACSELValidBGsForPolicyName, bStoredProcedure:=True, vResultArray:=vBankGuaranteeDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                r_dtDueDate = m_oDatabase.Parameters.Item("DueDate").Value
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

    Public Function UpdateBGForPolicy(ByRef lInsuranceFileCnt As Integer, ByRef crAmount As Decimal, ByRef lBgId As Integer, ByRef dtCoverFromDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateBGForPolicy"
        'Dim vBranches As Object
        'Dim lCount As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Amount", crAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            bPMAddParameter.AddParameterLite(m_oDatabase, "bg_id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "cover_from_date", dtCoverFromDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDPolicyBankGuaranteeSQL, sSQLName:=ACADDPolicyBankGuaranteeName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function GetAttachedBranches(ByRef vAttachBranches(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAttachedBranches"
        Dim vBranches(,) As Object = Nothing
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

                    If Informations.IsArray(vBranches) Then


                        vAttachBranches(MainModule.ENBankGuarantee.Branches, lCount) = vBranches
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

                    If Informations.IsArray(vProducts) Then


                        vAttachProducts(MainModule.ENBankGuarantee.Products, lCount) = vProducts
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

    'Public Function GetPartyDetails(ByRef vPartyDetails As Variant) As Long
    'Const kMethodName As String = "GetPartyDetails"
    '
    '
    ' Dim vDetails As Variant
    ' Dim lCount As Long
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    GetPartyDetails = PMTrue
    '
    '    If Not IsEmpty(vPartyDetails) Then
    '    For lCount = 0 To UBound(vAttachProducts, 2)
    '
    '            ' Add Input parameters for both PartyCnt and AccountId
    '
    '                ' Clear Down Database Parameters
    '                m_oDatabase.Parameters.Clear
    '
    '                ' Add Required Stored Procedure Parameters
    '                Call AddParameterLite(m_oDatabase, "BG_Number", vAttachProducts(ENBankGuarantee.BGId, lCount), PMParamInput, PMLong)
    '
    '                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteeProductsSQL, _
    ''                                                    sSQLName:=ACSELBankGuaranteeProductsName, _
    ''                                                    bStoredProcedure:=True, _
    ''                                                    vResultArray:=vProducts)
    '
    '                If m_lReturn <> PMTrue Then
    '                    RaiseError kMethodName, ACSELBankGuaranteeProductsSQL & " Failed", PMLogError
    '                End If
    '
    '                If IsArray(vProducts) Then
    '                    vPartyDetails(ENBankGuarantee.Products, lCount) = vDetails
    '                End If
    '
    '    Next
    '    End If
    '    GoTo Finally
    '
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sUsername:=m_sUsername, _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=GetAttachedProducts
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function


    Public Function GetBankGuaranteeDetails(ByRef vBankGuaranteeDetails(,) As Object, Optional ByVal vPartyCnt As Object = Nothing, Optional ByVal vBGId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBankGuaranteeDetails"


        'Dim vResult As Object =nothing = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (Convert.IsDBNull(vPartyCnt) Or Informations.IsNothing(vPartyCnt)) Or Not (Convert.IsDBNull(vBGId) Or Informations.IsNothing(vBGId)) Then
                ' Add Input parameters for both PartyCnt and AccountId

                ' Clear Down Database Parameters
                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters

                If Not Informations.IsNothing(vPartyCnt) Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vBGId) Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", vBGId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteeDetailsSQL, sSQLName:=ACSELBankGuaranteeDetailsName, bStoredProcedure:=True, vResultArray:=vBankGuaranteeDetails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not Informations.IsArray(vBankGuaranteeDetails) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else

                    m_lReturn = CType(GetBankdetails(vResultArray:=vBankGuaranteeDetails), gPMConstants.PMEReturnCode)


                    m_lReturn = CType(GetAttachedProducts(vAttachProducts:=vBankGuaranteeDetails), gPMConstants.PMEReturnCode)


                    m_lReturn = CType(GetAttachedBranches(vAttachBranches:=vBankGuaranteeDetails), gPMConstants.PMEReturnCode)


                    m_lReturn = CType(MergeAllLookups(vResultArray:=vBankGuaranteeDetails), gPMConstants.PMEReturnCode)
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

    Public Function GetPolicyBGForReceipt(ByRef r_vGetPoliciesForReceipt(,) As Object, ByVal vPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyBGForReceipt"

        'Dim vResult As Object =nothing = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Input parameters for both PartyCnt and AccountId

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteePolicyForReceiptSQL, sSQLName:=ACSELBankGuaranteePolicyForReceiptName, bStoredProcedure:=True, vResultArray:=r_vGetPoliciesForReceipt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteePolicyForReceiptSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(r_vGetPoliciesForReceipt) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
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

    Public Function GetAttachedPolicies(ByRef vGetAttachedPolicies(,) As Object, ByVal vBG_Id As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAttachedPolicies"


        'Dim vResult As Object =nothing = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Input parameters for both PartyCnt and AccountId

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "bg_id", vBG_Id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteePolicySQL, sSQLName:=ACSELBankGuaranteePolicyName, bStoredProcedure:=True, vResultArray:=vGetAttachedPolicies)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vGetAttachedPolicies) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
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

        'Dim lReturn As Integer
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
            ElseIf Informations.IsArray(r_vResults) Then
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

        Dim vLookUp(,) As Object = Nothing
        Dim vID As Object = Nothing
        Dim vMergeLookup(1) As Object


        result = gPMConstants.PMEReturnCode.PMTrue
        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

            m_lReturn = CType(MergeLookupValues(ENMergeColumn:=MainModule.ENBankGuarantee.BGCurrencyId, vIdValue:=vResultArray(MainModule.ENBankGuarantee.BGCurrencyId, lCount), vLookUp:=vLookUp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "MergeAllLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                vMergeLookup(kLookId) = vLookUp(kLookId, 0)


                vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)

                vResultArray(MainModule.ENBankGuarantee.BGCurrencyId, lCount) = vMergeLookup
            End If


        Next
        Return result
    End Function

    Private Function MergeLookupValues(ByVal ENMergeColumn As MainModule.ENBankGuarantee, ByVal vIdValue As Object, ByRef vLookUp As Object) As Integer
        Dim result As Integer = 0
        'Const kMethodName As String = "MergeLookupValues"
        Dim sTableName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        If ENMergeColumn = MainModule.ENBankGuarantee.BGCurrencyId Then
            sTableName = "Currency"
        End If

        m_lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=sTableName, r_vResults:=vLookUp, v_Id:=vIdValue), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

        End If
        Return result
    End Function

    'Public Function GetPartyBankHistory(ByRef vPartyBankHistory As Variant, _
    ''                                        Optional vPartyCnt As Variant, _
    ''                                        Optional vAccountId As Variant) As Long
    '
    'Const kMethodName As String = "GetPartyBankHistory"
    '
    '
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    GetPartyBankHistory = PMTrue
    '    If Not IsNull(vPartyCnt) Or Not IsNull(vAccountId) Then
    '            ' Add Input parameters for both PartyCnt and AccountId
    '
    '                ' Clear Down Database Parameters
    '                m_oDatabase.Parameters.Clear
    '
    '                ' Add Required Stored Procedure Parameters
    '                Call AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, PMParamInput, PMLong)
    '                Call AddParameterLite(m_oDatabase, "account_id", vAccountId, PMParamInput, PMDouble)
    '
    '                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteeHistorySQL, _
    ''                                                    sSQLName:=ACSELBankGuaranteeHistoryName, _
    ''                                                    bStoredProcedure:=True, _
    ''                                                    vResultArray:=vPartyBankHistory)
    '
    '                If m_lReturn <> PMTrue Then
    '                    RaiseError kMethodName, ACSELBankGuaranteeDetailsSQL & " Failed", PMLogError
    '                End If
    '
    '                If Not IsArray(vPartyBankHistory) Then
    '                    GetPartyBankHistory = PMNotFound
    '                Else
    '                    m_lReturn = MergeAllLookupsHistory(vResultArray:=vPartyBankHistory)
    '                End If
    '
    '                GoTo Finally
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
    ''          r_lFunctionReturn:=GetPartyBankHistory
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    'End Function

    'Private Function MergeAllLookupsHistory(ByRef vResultArray As Variant) As Long
    'Const kMethodName As String = "MergeAllLookupsHistory"
    '
    '    On Error GoTo Catch
    '    Dim lCount As Long
    '    Dim vLookUp As Variant
    '    Dim vID As Variant
    '    Dim vMergeLookup(1) As Variant
    'Try:
    '
    '    MergeAllLookupsHistory = PMTrue
    '    For lCount = 0 To UBound(vResultArray, 2)
    '
    '            m_lReturn = MergeLookupValuesHistory(ENMergeColumn:=ENBankGuaranteeHistory.BankPaymentTypeId, _
    ''                                            vIdValue:=vResultArray(ENBankGuaranteeHistory.BankPaymentTypeId, lCount), _
    ''                                            vLookUp:=vLookUp)
    '            If m_lReturn <> PMTrue Then
    '                RaiseError kMethodName, "MergeAllLookupsHistory Failed", PMLogError
    '            ElseIf m_lReturn = PMTrue Then
    '                  vMergeLookup(kLookId) = vLookUp(kLookId, 0)
    '                  vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)
    '                  vResultArray(ENBankGuaranteeHistory.BankPaymentTypeId, lCount) = vMergeLookup
    '            End If
    '
    '
    '            m_lReturn = MergeLookupValuesHistory(ENMergeColumn:=ENBankGuaranteeHistory.BankAccountTypeId, _
    ''                                            vIdValue:=vResultArray(ENBankGuaranteeHistory.BankAccountTypeId, lCount), _
    ''                                            vLookUp:=vLookUp)
    '            If m_lReturn <> PMTrue Then
    '                RaiseError kMethodName, "MergeAllLookupsHistory Failed", PMLogError
    '            ElseIf m_lReturn = PMTrue Then
    '                  vMergeLookup(kLookId) = vLookUp(kLookId, 0)
    '                  vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)
    '                  vResultArray(ENBankGuaranteeHistory.BankAccountTypeId, lCount) = vMergeLookup
    '            End If
    '
    '            m_lReturn = MergeLookupValuesHistory(ENMergeColumn:=ENBankGuaranteeHistory.BankNameId, _
    ''                                            vIdValue:=vResultArray(ENBankGuaranteeHistory.BankNameId, lCount), _
    ''                                            vLookUp:=vLookUp)
    '            If m_lReturn <> PMTrue Then
    '                RaiseError kMethodName, "MergeAllLookupsHistory Failed", PMLogError
    '            ElseIf m_lReturn = PMTrue Then
    '                  vMergeLookup(kLookId) = vLookUp(kLookId, 0)
    '                  vMergeLookup(kLookDesc) = vLookUp(kLookDesc, 0)
    '                  vResultArray(ENBankGuaranteeHistory.BankNameId, lCount) = vMergeLookup
    '            End If
    '
    '
    '    Next
    'GoTo Finally
    '
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sUsername:=m_sUsername, _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=MergeAllLookupsHistory
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function

    'Private Function MergeLookupValuesHistory(ByVal ENMergeColumn As ENBankGuaranteeHistory, _
    ''                                    ByVal vIdValue As Variant, _
    ''                                    ByRef vLookUp As Variant) As Long
    'Const kMethodName As String = "MergeLookupValuesHistory"
    '    Dim sTableName As String
    '    On Error GoTo Catch
    'Try:
    '    MergeLookupValuesHistory = PMTrue
    '    If ENMergeColumn = ENBankGuaranteeHistory.BankPaymentTypeId Then
    '        sTableName = "bank_payment_type"
    '    ElseIf ENMergeColumn = ENBankGuaranteeHistory.BankAccountTypeId Then
    '        sTableName = "bank_account_type"
    '    ElseIf ENMergeColumn = ENBankGuaranteeHistory.BankNameId Then
    '        sTableName = "CashListItem_Bank"
    '    End If
    '
    '    m_lReturn = GetLookupsByEffectiveDate(v_sTableName:=sTableName, _
    ''                                            r_vResults:=vLookUp, _
    ''                                            v_Id:=vIdValue)
    '    If m_lReturn <> PMTrue Then
    '
    '    End If
    '    GoTo Finally
    '
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sUsername:=m_sUsername, _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=MergeLookupValuesHistory
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function

    'Developer Guide No: 33
    Public Function UpdateBankGuaranteeDetails(ByRef vPartyCnt As Object, Optional ByRef vBankGuaranteeDetails As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateBankGuaranteeDetails"

        'Dim lReturn As Integer
        'Dim vResults As Object
        Dim lEventCnt, lBankGuaranteeId As Integer
        Dim sActionCode As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sDescription As New StringBuilder
            'Dim vOldPartyBankDet As Object
            'Dim lPartyBankIdForHistory As Integer

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add new contacts for address if supplied
            Dim vGetPrevDetails(,) As Object = Nothing
            If True Then

                For i As Integer = vBankGuaranteeDetails.GetLowerBound(1) To vBankGuaranteeDetails.GetUpperBound(1)


                    If vBankGuaranteeDetails(0, i) = gPMConstants.PMEComponentAction.PMAdd Then
                        sActionCode = "Setup"

                        ' Make a call to AddBankGuarantee function supplying the relevant input parameters






                        m_lReturn = CType(AddBankGuarantee(r_lBGId:=lBankGuaranteeId, vBankNameId:=vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Id), sBankBranch:=CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, i)), vPartyCnt:=gPMFunctions.ToSafeInteger(vPartyCnt), sBGRef:=CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), lCurrencyId:=gPMFunctions.ToSafeInteger(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGCurrencyId, i)(MainModule.ENPMLookups.Id)), dBGLimit:=gPMFunctions.ToSafeCurrency(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, i)), dAvailableBal:=gPMFunctions.ToSafeCurrency(vBankGuaranteeDetails(MainModule.ENBankGuarantee.AvailableBal, i)), lCustodyBranchId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.CustodyBranchId, i)), dtIssueDate:=CDate(vBankGuaranteeDetails(MainModule.ENBankGuarantee.IssueDate, i)), dtExpiryDate:=CDate(vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)), lIsPolicyLock:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsPolicyLock, i))), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Add BankGuarantee Failed for BG Ref" & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If


                        For lCountBranches As Integer = 0 To vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i).GetUpperBound(0)
                            'Start - Sankar - Bank Guarantee Bug Fixing

                            m_lReturn = CType(AddBranches(lBgId:=lBankGuaranteeId, lSourceId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i)(lCountBranches, 0))), gPMConstants.PMEReturnCode)
                            'End - Sankar - Bank Guarantee Bug Fixing
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                gPMFunctions.RaiseError(kMethodName, "Add Branches Failed for BG Ref" & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Next


                        For lCountProducts As Integer = 0 To vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i).GetUpperBound(0)
                            'Start - Sankar - Bank Guarantee Bug Fixing

                            m_lReturn = CType(AddProducts(lBgId:=lBankGuaranteeId, lProductId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i)(lCountProducts, 0))), gPMConstants.PMEReturnCode)
                            'End - Sankar - Bank Guarantee Bug Fixing
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                gPMFunctions.RaiseError(kMethodName, "Add Products Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Next

                        'Start - Sankar - Bank Guarantee Bug Fixing - Added BG Limit
                        sDescription = New StringBuilder("Bank Guarantee Details Created -")






                        sDescription.Append( _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Description)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)) & ", " & _
                                            GetBGStatusDescription(CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, i))))


                        m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=CInt(vPartyCnt), v_lEventTypeId:=PMBConst.PMBEventNewClient, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Create Event Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf vBankGuaranteeDetails(0, i) = gPMConstants.PMEComponentAction.PMEdit Then
                        sActionCode = "Amendment"

                        'Since we need to feed old bank details along with new bank details in order to generate event log at the time of editing bank details so retrieve old bank details prior to editing it in the database
                        m_lReturn = CType(GetBankGuaranteeDetails(vBankGuaranteeDetails:=vGetPrevDetails, vBGId:=vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i)), gPMConstants.PMEReturnCode)









                        m_lReturn = CType(EditBankGuarantee(lBgId:=vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i), vBankNameId:=vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Id), sBankBranch:=CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, i)), vPartyCnt:=vBankGuaranteeDetails(MainModule.ENBankGuarantee.PartyCnt, i), sBGRef:=CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), lCurrencyId:=gPMFunctions.ToSafeInteger(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGCurrencyId, i)(MainModule.ENPMLookups.Id)), dBGLimit:=CDbl(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, i)), dAvailableBal:=CDbl(vBankGuaranteeDetails(MainModule.ENBankGuarantee.AvailableBal, i)), lCustodyBranchId:=CDbl(vBankGuaranteeDetails(MainModule.ENBankGuarantee.CustodyBranchId, i)), dtIssueDate:=CDate(vBankGuaranteeDetails(MainModule.ENBankGuarantee.IssueDate, i)), dtExpiryDate:=CDate(vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)), lIsPolicyLock:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsPolicyLock, i))), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "EditBankGuarantee Failed for BG Ref" & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If



                        m_lReturn = CType(DeleteBranches(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i))), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Delete Branches Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If


                        For lCountBranches As Integer = 0 To vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i).GetUpperBound(0)



                            m_lReturn = CType(AddBranches(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i)), lSourceId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i)(lCountBranches, 0))), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                gPMFunctions.RaiseError(kMethodName, "Add Branches Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Next


                        m_lReturn = CType(DeleteProducts(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i))), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Delete Products Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If


                        For lCountProducts As Integer = 0 To vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i).GetUpperBound(0)



                            m_lReturn = CType(AddProducts(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i)), lProductId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i)(lCountProducts, 0))), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                gPMFunctions.RaiseError(kMethodName, "Add Products Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Next


                        'Start - Sankar - Bank Guarantee Bug Fixing - Added BG Limit
                        sDescription = New StringBuilder("Bank Guarantee Details Edited from -")






                        sDescription.Append( _
                                            CStr(vGetPrevDetails(MainModule.ENBankGuarantee.BankNameId, 0)(MainModule.ENPMLookups.Description)) & ", " & _
                                            CStr(vGetPrevDetails(MainModule.ENBankGuarantee.BankBranch, 0)) & ", " & _
                                            CStr(vGetPrevDetails(MainModule.ENBankGuarantee.BGRef, 0)) & ", " & _
                                            CStr(vGetPrevDetails(MainModule.ENBankGuarantee.BGLimit, 0)) & ", " & _
                                            CStr(vGetPrevDetails(MainModule.ENBankGuarantee.ExpiryDate, 0)) & ", " & _
                                            GetBGStatusDescription(CInt(vGetPrevDetails(MainModule.ENBankGuarantee.BGStatusId, 0))))

                        sDescription.Append(" TO ")






                        sDescription.Append( _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Description)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)) & ", " & _
                                            GetBGStatusDescription(CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, i))))
                        '

                        m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=CInt(vPartyCnt), v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Create Event Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf vBankGuaranteeDetails(0, i) = gPMConstants.PMEComponentAction.PMDelete Then
                        sActionCode = "Deleted"
                        ' Since we can't rely on the details coming from
                        ' front end as user can always edit data prior to its deletion so for generating event log we will pick data from DB
                        'm_lReturn = GetBankGuaranteeDetails(vBankGuaranteeDetails:=vGetPrevDetails, _
                        ''                                 vBGId:=vBankGuaranteeDetails(ENBankGuarantee.BGId, i))



                        m_lReturn = CType(DelUnDelBankGuarantee(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i)), lDeleted:=1), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Delete Bank Guarantee Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If


                        'Start - Sankar - Bank Guarantee Bug Fixing - Added BG Limit
                        sDescription = New StringBuilder("Bank Guarantee Details Deleted from -")






                        sDescription.Append( _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Description)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, i)) & ", " & _
                                            CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)) & ", " & _
                                            GetBGStatusDescription(CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, i))))


                        m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=CInt(vPartyCnt), v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription.ToString()), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Create Event Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf vBankGuaranteeDetails(0, i) = gPMConstants.PMEComponentAction.PMReverse Then
                        sActionCode = "Un Deleted"


                        m_lReturn = CType(DelUnDelBankGuarantee(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i)), lDeleted:=0), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "UnDelete BankGuarantee Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    ElseIf CDbl(vBankGuaranteeDetails(0, i)) = 4 Then  ' Invoked


                        m_lReturn = CType(InvokeBankGuarantee(lBgId:=CInt(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, i)), dtInvokedDate:=CDate(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, i))), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Invoke Bank Guarantee Failed for BG Ref " & CStr(vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If
                Next i
            End If
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    'Start - Sankar - Bank Guarantee Bug Fixing
    Public Function GetBGStatusDescription(ByVal iBGStatusId As Integer) As String
        Dim result As String = String.Empty
        If iBGStatusId = MainModule.ENBGStatus.Active Then
            result = kBGStatusActive
        ElseIf iBGStatusId = MainModule.ENBGStatus.Issued Then
            result = kBGStatusIssued
        ElseIf iBGStatusId = MainModule.ENBGStatus.Invoked Then
            result = kBGStatusInvoked
        ElseIf iBGStatusId = MainModule.ENBGStatus.Deleted Then
            result = kBGStatusDeleted
        ElseIf iBGStatusId = MainModule.ENBGStatus.Expired Then
            result = kBGStatusExpired
        End If
        Return result
    End Function
    'End - Sankar - Bank Guarantee Bug Fixing

    Private Function InvokeBankGuarantee(ByVal lBgId As Integer, ByVal dtInvokedDate As Date) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "InvokeBankGuarantee"




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "Status_Id", 3, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "Status_Date", dtInvokedDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDBGStatusSQL, sSQLName:=ACUPDBGStatusName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACUPDBGStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function

    Private Function DeleteBranches(ByVal lBgId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DeleteBranches"




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELBranchSQL, sSQLName:=ACDELBranchName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACDELBranchSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function

    Private Function DeleteProducts(ByVal lBgId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DeleteProducts"




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELProductSQL, sSQLName:=ACDELProductName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACDELProductSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function

    Private Function AddBranches(ByVal lBgId As Integer, ByVal lSourceId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddBranches"




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "branch_id", lSourceId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDBranchSQL, sSQLName:=ACADDBranchName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACADDBranchSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    Private Function AddProducts(ByVal lBgId As Integer, ByVal lProductId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddProducts"




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", lProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDProductSQL, sSQLName:=ACADDProductName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACADDProductSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    Public Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults(,) As Object, Optional ByRef v_Id As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupsByEffectiveDate"

        'Dim lReturn As Integer

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


    Public Function GetBankGuaranteeDetailsById(ByVal lPartyBankId As Integer, ByRef vOldPartyBankDet(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBankGuaranteeDetailsById"


        'Dim vResult As Object =nothing = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_bank_id", lPartyBankId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELBankGuaranteeDetailsByIdSQL, sSQLName:=ACSELBankGuaranteeDetailsByIdName, bStoredProcedure:=True, vResultArray:=vOldPartyBankDet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeDetailsByIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

        End Try
        Return result
    End Function

    Public Function GetCashListItemForBG(ByVal lCashListId As Integer, ByRef vCashListItemsForBg(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetCashListItemForBG"


        'Dim vResult As Object =nothing = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "cashlist_id", lCashListId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELCashListItemIdsForBGSQL, sSQLName:=ACSELCashListItemIdsForBGName, bStoredProcedure:=True, vResultArray:=vCashListItemsForBg)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeDetailsByIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vCashListItemsForBg) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
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

    Public Function UpdateCashListItemForBG(ByVal lBgId As Integer, ByVal lCashListId As Integer, ByVal lCashListitemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal cAmtToBePosted As Decimal) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCashListItemForBG"


        ''Dim vResult As Object =nothing = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "bg_id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "cashlist_id", lCashListId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "cashlistitem_id", lCashListitemId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'Start - Sankar - Bank Guarantee Bug Fixing - Changed PMLong to PMCurrency
            bPMAddParameter.AddParameterLite(m_oDatabase, "amt_to_be_posted", cAmtToBePosted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDCashListItemIdsForBGSQL, sSQLName:=ACUPDCashListItemIdsForBGName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBankGuaranteeDetailsByIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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
    Public Function AddBankGuarantee(ByRef r_lBGId As Integer, ByVal vBankNameId As Object, ByVal sBankBranch As String, ByVal vPartyCnt As Object, ByVal sBGRef As String, ByVal lCurrencyId As Integer, ByVal dBGLimit As Double, ByVal dAvailableBal As Double, ByVal lCustodyBranchId As Integer, ByVal dtIssueDate As Date, ByVal dtExpiryDate As Date, ByVal lIsPolicyLock As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddBankGuarantee"
        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (Convert.IsDBNull(vPartyCnt) Or Informations.IsNothing(vPartyCnt)) Then 'Or Not IsNull(vAccountId) Then
                ' Add Input parameters for both PartyCnt and AccountId

                ' Clear Down Database Parameters
                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", r_lBGId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "bank_name_id", vBankNameId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "bank_branch", sBankBranch, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "custody_branch_id", lCustodyBranchId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_ref", sBGRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Currency_Id", lCurrencyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Limit", dBGLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                bPMAddParameter.AddParameterLite(m_oDatabase, "issue_date", dtIssueDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                bPMAddParameter.AddParameterLite(m_oDatabase, "available_bal", dAvailableBal, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", dtExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                bPMAddParameter.AddParameterLite(m_oDatabase, "is_policy_lock", lIsPolicyLock, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "bg_status_id", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDBankGuaranteeDetailsSQL, sSQLName:=ACADDBankGuaranteeDetailsName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACADDBankGuaranteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Else
                    r_lBGId = m_oDatabase.Parameters.Item("bg_id").Value
                End If
                Return result
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

    Public Function EditBankGuarantee(ByRef lBgId As Object, ByVal vBankNameId As Object, ByVal sBankBranch As String, ByVal vPartyCnt As Object, ByVal sBGRef As String, ByVal lCurrencyId As Integer, ByVal dBGLimit As Double, ByVal dAvailableBal As Double, ByVal lCustodyBranchId As Double, ByVal dtIssueDate As Date, ByVal dtExpiryDate As Date, ByVal lIsPolicyLock As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "EditBankGuarantee"
        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (Convert.IsDBNull(vPartyCnt) Or Informations.IsNothing(vPartyCnt)) Then 'Or Not IsNull(vAccountId) Then
                ' Add Input parameters for both PartyCnt and AccountId

                ' Clear Down Database Parameters
                m_oDatabase.Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "bank_name_id", vBankNameId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "bank_branch", sBankBranch, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                'Call AddParameterLite(m_oDatabase, "Party_Cnt", vPartyCnt, PMParamInput, PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_ref", sBGRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Currency_Id", lCurrencyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "BG_Limit", dBGLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                bPMAddParameter.AddParameterLite(m_oDatabase, "available_bal", dAvailableBal, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                bPMAddParameter.AddParameterLite(m_oDatabase, "custody_branch_id", lCustodyBranchId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "issue_date", dtIssueDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", dtExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                bPMAddParameter.AddParameterLite(m_oDatabase, "is_policy_lock", lIsPolicyLock, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDBankGuaranteeDetailsSQL, sSQLName:=ACUPDBankGuaranteeDetailsName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACUPDBankGuaranteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                Return result
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

    Public Function GetSystemCurrency(ByRef r_lCurrencyId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetSystemCurrency"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", r_lCurrencyId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetSystemCurrencySQL, sSQLName:=ACGetSystemCurrencyName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDELBankGuaranteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                r_lCurrencyId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("currency_id").Value)
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

    Public Function DelUnDelBankGuarantee(ByVal lBgId As Integer, ByVal lDeleted As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DelUnDelBankGuarantee"
        'Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "bg_id", lBgId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "delete", lDeleted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELBankGuaranteeDetailsSQL, sSQLName:=ACDELBankGuaranteeDetailsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDELBankGuaranteeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    'Public Function DeletePartyBank(ByVal lPartyBankId As Long) As Long
    '    Const kMethodName As String = "DeletePartyBank"
    '    Dim lReturn     As Long
    '
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    DeletePartyBank = PMTrue
    '
    '
    '                ' Clear Down Database Parameters
    '                m_oDatabase.Parameters.Clear
    '
    '                ' Add Required Stored Procedure Parameters
    '                Call AddParameterLite(m_oDatabase, "party_bank_id", lPartyBankId, PMParamInput, PMLong)
    '
    '                m_lReturn& = m_oDatabase.SQLAction(sSQL:=ACDELPartyBankDetailsSQL, _
    ''                                                    sSQLName:=ACDELPartyBankDetailsName, _
    ''                                                    bStoredProcedure:=True)
    '
    '                If m_lReturn <> PMTrue Then
    '                    RaiseError kMethodName, ACDELPartyBankDetailsSQL & " Failed", PMLogError
    '                End If
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
    ''          r_lFunctionReturn:=DeletePartyBank
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function

    'Public Function AddBankGuaranteeHistory(ByVal lPartyBankId As Long, _
    ''                                        ByVal sActionCode As String, _
    ''                                        ByVal vPartyCnt As Variant, _
    ''                                        ByVal vAccountId As Variant, _
    ''                                        ByVal sAccHolderName As String, _
    ''                                        ByVal sAccNumber As String, _
    ''                                        ByVal lBankPaymentTypeId As Long, _
    ''                                        ByVal lBankAccountTypeId As Long, _
    ''                                        ByVal vBankNameId As Variant, _
    ''                                        ByVal sBankBranch As String, _
    ''                                        ByVal sBankBranchCode As String, _
    ''                                        ByVal sBankAdd1 As String, _
    ''                                        ByVal sBankAdd2 As String, _
    ''                                        ByVal sBankAdd3 As String, _
    ''                                        ByVal sBankTown As String, _
    ''                                        ByVal sBankPCode As String, _
    ''                                        ByVal sBankRegion As String, _
    ''                                        ByVal sBankCountry As String, _
    ''                                        ByVal sCCNum As String, _
    ''                                        ByVal sCCStartDate As String, _
    ''                                        ByVal sCCExpiryDate As String, _
    ''                                        ByVal sCCIssueNum As String, _
    ''                                        ByVal sCCPin As String, _
    ''                                        ByVal lIsRegistered As Long, ByVal sCCAdd1 As String, ByVal sCCAdd2 As String, ByVal sCCAdd3 As String, ByVal sCCTown As String, ByVal sCCPCode As String, ByVal sCCCountry As String, ByVal lUserId As Long) As Long
    '
    '
    '    Const kMethodName As String = "AddBankGuaranteeHistory"
    '    Dim lReturn     As Long
    '
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    AddBankGuaranteeHistory = PMTrue
    '
    '    If Not IsNull(vPartyCnt) Or Not IsNull(vAccountId) Then
    '            ' Add Input parameters for both PartyCnt and AccountId
    '
    '                ' Clear Down Database Parameters
    '                m_oDatabase.Parameters.Clear
    '
    '                ' Add Required Stored Procedure Parameters
    '                Call AddParameterLite(m_oDatabase, "party_bank_id", lPartyBankId, PMParamInput, PMLong)
    '                Call AddParameterLite(m_oDatabase, "action_code", sActionCode, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "party_cnt", vPartyCnt, PMParamInput, PMLong)
    '                Call AddParameterLite(m_oDatabase, "account_id", vAccountId, PMParamInput, PMLong)
    '                Call AddParameterLite(m_oDatabase, "account_holder_name", sAccHolderName, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "account_number", sAccNumber, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_payment_type_id", lBankPaymentTypeId, PMParamInput, PMLong)
    '                Call AddParameterLite(m_oDatabase, "bank_account_type_id", lBankAccountTypeId, PMParamInput, PMLong)
    '                Call AddParameterLite(m_oDatabase, "bank_name_id", vBankNameId, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_branch", sBankBranch, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_branch_code", sBankBranchCode, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_add1", sBankAdd1, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_add2", sBankAdd2, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_add3", sBankAdd3, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_town", sBankTown, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_PCode", sBankPCode, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_region", sBankRegion, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "bank_country", sBankCountry, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_num", sCCNum, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_start_date", sCCStartDate, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_expiry_date", sCCExpiryDate, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_issue_num", sCCIssueNum, PMParamInput, PMString)
    '
    '                Call AddParameterLite(m_oDatabase, "cc_pin", sCCPin, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "is_registered", lIsRegistered, PMParamInput, PMInteger)
    '                Call AddParameterLite(m_oDatabase, "cc_add1", sCCAdd1, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_add2", sCCAdd2, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_add3", sCCAdd3, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_town", sCCTown, PMParamInput, PMString)
    '
    '                Call AddParameterLite(m_oDatabase, "cc_pcode", sCCPCode, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "cc_country", sCCCountry, PMParamInput, PMString)
    '                Call AddParameterLite(m_oDatabase, "user_id", 1, PMParamInput, PMLong)
    '
    '
    '                m_lReturn& = m_oDatabase.SQLAction(sSQL:=ACAddBankGuaranteeHistorySQL, _
    ''                                                    sSQLName:=ACAddBankGuaranteeHistoryName, _
    ''                                                    bStoredProcedure:=True)
    '
    '                If m_lReturn <> PMTrue Then
    '                    RaiseError kMethodName, ACAddBankGuaranteeHistorySQL & " Failed", PMLogError
    '                End If
    '
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
    ''          r_lFunctionReturn:=AddBankGuaranteeHistory
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function


    'developer guide no.109
    Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        'Const kMethodName As String = "EditBankGuarantee"
        'Dim lReturn As Integer



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

            lReturn = m_oDatabase.SQLBeginTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLBeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
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

            lReturn = m_oDatabase.SQLRollbackTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLRollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
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

            lReturn = m_oDatabase.SQLCommitTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
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
End Class
