Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System

'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "Business"


    Private m_oDatabase As dPMDAO.Database

    Private m_bCloseDatabase As Boolean

    Private m_lError As Integer
    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTypeOfBusiness As New FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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

    Public ReadOnly Property TypeOfBusiness() As String
        Get

            Return m_sTypeOfBusiness.Value

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

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


            If Not Information.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness.Value = CStr(vTypeOfBusiness)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetProcessModes", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values as defined by vTableArray.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing

            ' Get the Lookup items from the Business Component

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetLookupValues", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

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

            ' Set User ID
            m_iUserID = iUserID


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Initialise", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()

                End If
                m_oLookup = Nothing

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: CheckResults (Private)
    '
    ' Description: Checks the result array after a query
    '              If records found returns PMTrue
    '              If no records found returns PMNotFound
    '
    ' ***************************************************************** '
    Private Function CheckResults(ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' If NO records were found return PMNotFound
        If Not Information.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If


        Return result
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise


        Catch ex As Exception

            ' Error Section.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BeginTrans"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLBeginTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute SQLBeginTrans", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CommitTrans"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Commit the Transaction

            m_lReturn = m_oDatabase.SQLCommitTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute SQLCommitTrans", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackTrans"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Rollback the Transaction

            m_lReturn = m_oDatabase.SQLRollbackTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute SQLRollbackTrans", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
        End Try
        Return result
    End Function

    '****************************************************************** '
    ' Name: GetPolicyStatusForMediaTypeStatusMaintenance (Public)
    '
    ' Description: Checks whether the policy has been cancelled and
    '               any claim payment has been initiated.
    '
    '****************************************************************** '
    Public Function GetPolicyStatusForMediaTypeStatusMaintenance(ByVal v_lInsuranceFileID As Integer, ByRef r_bIsPolicyCancelled As Boolean, ByRef r_bIsClaimPaymentInitiated As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyStatusForMediaTypeStatusMaintenance"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="IsPolicyCancelled", vValue:=r_bIsPolicyCancelled, idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter IsPolicyCancelled to " & ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="IsClaimPaymentInitiated", vValue:=r_bIsClaimPaymentInitiated, idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter IsClaimPaymentInitiated to " & ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=v_lInsuranceFileID, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter v_lInsuranceFileID to " & ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL, sSQLName:=ACGetPolicyStatusForMediaTypeStatusMaintenanceName, bStoredProcedure:=ACGetPolicyStatusForMediaTypeStatusMaintenanceStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "Failed to execute " & ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            r_bIsPolicyCancelled = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("IsPolicyCancelled").Value)

            r_bIsClaimPaymentInitiated = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("IsClaimPaymentInitiated").Value)


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
    '****************************************************************** '
    ' Name: GetReceiptsForMediaTypeStatusMaintenance (Public)
    '
    ' Description: Finds the details of policy receipts based on given filters
    '
    '****************************************************************** '
    'Start Renuka PN 63396 - changed a parameter
    'developer guide no. 101
    Public Function GetReceiptsForMediaTypeStatusMaintenance(ByVal v_iUserID As Object, ByRef r_vResultArray(,) As Object, Optional ByVal v_vBranchID As Object = Nothing, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vInsurance_Ref As Object = Nothing, Optional ByVal v_vCollectionDateFrom As Object = Nothing, Optional ByVal v_vCollectionDateTo As Object = Nothing, Optional ByVal v_vMediaReference As Object = Nothing, Optional ByVal v_vMediaTypeStatusID As Object = Nothing, Optional ByVal v_vDrawnBankID As Object = Nothing, Optional ByVal v_vDocumentRef As Object = Nothing, Optional ByVal v_vMaxRowsToFetch As Object = Nothing) As Integer
        'End Renuka PN 63396

        Dim result As Integer = 0
        Const kMethodName As String = "GetReceiptsForMediaTypeStatusMaintenance"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            If Information.IsNothing(v_vBranchID) Or (v_vBranchID = -1) Then

                v_vBranchID = Nothing
            End If

            If Information.IsNothing(v_vBankAccountID) Or (v_vBankAccountID = -1) Then

                v_vBankAccountID = DBNull.Value
            End If
            'Start Renuka PN 63396
            'If IsMissing(v_vPartyID) Or (v_vPartyID = -1) Then
            '    v_vPartyID = Null
            'End If

            If Information.IsNothing(v_vShortName) Or (v_vShortName = "") Then

                v_vShortName = DBNull.Value
            End If
            'End Renuka PN 63396

            If Information.IsNothing(v_vInsurance_Ref) Or (v_vInsurance_Ref = "") Then

                v_vInsurance_Ref = DBNull.Value
            End If

            'developer guide no. 44
            If Information.IsNothing(v_vCollectionDateFrom) OrElse (v_vCollectionDateFrom = #12/30/1899#) Then

                v_vCollectionDateFrom = DBNull.Value
            End If

            'developer guide no. 44
            If Information.IsNothing(v_vCollectionDateTo) OrElse (v_vCollectionDateTo = #12/30/1899#) Then

                v_vCollectionDateTo = DBNull.Value
            End If

            If Information.IsNothing(v_vMediaReference) Or (v_vMediaReference = "") Then

                v_vMediaReference = DBNull.Value
            End If

            If Information.IsNothing(v_vMediaTypeStatusID) Or (v_vMediaTypeStatusID = -1) Then

                v_vMediaTypeStatusID = DBNull.Value
            End If

            If Information.IsNothing(v_vDrawnBankID) Or (v_vDrawnBankID = -1) Then

                v_vDrawnBankID = DBNull.Value
            End If

            If Information.IsNothing(v_vDocumentRef) Or (v_vDocumentRef = "") Then

                v_vDocumentRef = DBNull.Value
            End If

            If Information.IsNothing(v_vMaxRowsToFetch) Then
                v_vMaxRowsToFetch = 0
            End If

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_Id", vValue:=v_vBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Branch_Id to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=v_vBankAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter BankAccount_Id to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            'Start Renuka PN 63396

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ShortName", vValue:=v_vShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'Start Renuka PN 63396

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Party_Cnt to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_Ref", vValue:=v_vInsurance_Ref, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Insurance_Ref to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="CollectionDate_From", vValue:=v_vCollectionDateFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter CollectionDate_From to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="CollectionDate_To", vValue:=v_vCollectionDateTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter CollectionDate_To to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaReference", vValue:=v_vMediaReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter MediaReference to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaTypeStatus_Id", vValue:=v_vMediaTypeStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter MediaTypeStatus_Id to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="DrawnBank_Id", vValue:=v_vDrawnBankID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter DrawnBank_Id to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Ref", vValue:=v_vDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Document_Ref to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="MaxRowsToFetch", vValue:=v_vMaxRowsToFetch, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter MaxRowsToFetch to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="User_Id", vValue:=v_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter User_Id to " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, sSQLName:=ACGetPolicyReceiptsForMediaTypeStatusMaintenanceName, bStoredProcedure:=ACGetPolicyReceiptsForMediaTypeStatusMaintenanceStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "Failed to execute " & ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            result = CheckResults(r_vResultArray)

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    '****************************************************************** '
    ' Name: UpdateMediaTypeStatusForPolicyReciept (Public)
    '
    ' Description: Updates the media type status of a receipt item
    '
    '****************************************************************** '
    Public Function UpdateMediaTypeStatusForPolicyReciept(ByVal v_lCashListItemID As Integer, ByVal v_lMediaTypeID As Integer, ByVal v_lMediaTypeStatusID As Integer, ByVal v_sComments As String, ByVal v_iUserID As Integer, ByVal v_dtDateModified As Date, ByVal v_lInsuranceFileID As Integer, ByVal v_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateMediaTypeStatusForPolicyReciept"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            'check value for nothing or 0 befor calling parameters.add
            If v_lCashListItemID = 0 OrElse IsNothing(v_lCashListItemID) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListItem_Id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListItem_Id", vValue:=v_lCashListItemID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter CashListItem_Id to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            If v_lMediaTypeID = 0 OrElse IsNothing(v_lMediaTypeID) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaType_Id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaType_Id", vValue:=v_lMediaTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter MediaType_Id to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            If v_lMediaTypeStatusID = 0 OrElse IsNothing(v_lMediaTypeStatusID) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaTypeStatus_Id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaTypeStatus_Id", vValue:=v_lMediaTypeStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter MediaTypeStatus_Id to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            If v_sComments = "" OrElse IsNothing(v_sComments) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comments", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comments", vValue:=v_sComments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Comments to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            If v_iUserID = 0 OrElse IsNothing(v_iUserID) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="User_Id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="User_Id", vValue:=v_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter User_Id to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            If v_dtDateModified = #12/30/1899# OrElse IsNothing(v_dtDateModified) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_Modified", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_Modified", vValue:=v_dtDateModified, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Date_Modified to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If



            If v_lInsuranceFileID = 0 OrElse IsNothing(v_lInsuranceFileID) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=v_lInsuranceFileID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Insurance_File_Cnt to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            If v_sDocumentRef = "" OrElse IsNothing(v_sDocumentRef) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Ref", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Document_Ref to " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateReceiptMediaTypeStatusSQL, sSQLName:=ACUpdateReceiptMediaTypeStatusName, bStoredProcedure:=ACUpdateReceiptMediaTypeStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "Failed to execute " & ACUpdateReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    '****************************************************************** '
    ' Name: UpdateMediaTypeStatusForPolicyReciepts (Public)
    '
    ' Description: Updates the media type status of receipt items
    '
    '****************************************************************** '
    Public Function UpdateMediaTypeStatusForPolicyReciepts(ByRef v_vUpdateArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateMediaTypeStatusForPolicyReciepts"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(v_vUpdateArray) Then
                Return result
            End If

            ' begin transaction
            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            For iRecordCnt As Integer = v_vUpdateArray.GetLowerBound(1) To v_vUpdateArray.GetUpperBound(1)
                m_lReturn = CType(UpdateMediaTypeStatusForPolicyReciept(v_lCashListItemID:=gPMFunctions.ToSafeLong(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSF_CASHLISTITEM_ID, iRecordCnt)), v_lMediaTypeID:=gPMFunctions.ToSafeLong(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_MEDIATYPE_ID, iRecordCnt)), v_lMediaTypeStatusID:=gPMFunctions.ToSafeLong(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_MEDIATYPESTATUS_ID, iRecordCnt)), v_sComments:=gPMFunctions.ToSafeString(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_COMMENTS, iRecordCnt)), v_iUserID:=gPMFunctions.ToSafeLong(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_USER_ID, iRecordCnt)), v_dtDateModified:=gPMFunctions.ToSafeDate(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_DATE_MODIFIED, iRecordCnt)), v_lInsuranceFileID:=gPMFunctions.ToSafeLong(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_INSURANCEFILE_ID, iRecordCnt)), v_sDocumentRef:=gPMFunctions.ToSafeString(v_vUpdateArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_DOCUMENT_REF, iRecordCnt))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute UpdateMediaTypeStatusForPolicyReciept", gPMConstants.PMELogLevel.PMLogError)
                End If
            Next iRecordCnt

            ' commit transaction
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CommitTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

            ' rollback transaction
            m_lReturn = RollbackTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Public Function CreateTask(ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer, ByVal v_sCustomer As String, ByVal v_lUserGroupId As Integer, ByVal v_sDescription As String, ByVal v_iIsVisible As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "CreateTask"

        Const kTaskStatusNew As Integer = 0
        Const kTaskIsNotUrgent As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTaskInstance As bPMWrkTaskInstance.FormClass
        Dim lTaskInstanceCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of business object

            oTaskInstance = New bPMWrkTaskInstance.FormClass
            lReturn = oTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject bPMWrkTaskInstance.Form Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create new task

            lReturn = oTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=v_lTaskGroupId, v_lPMWrkTaskID:=v_lTaskId, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=v_lUserGroupId, v_sDescription:=v_sDescription, v_iTaskStatus:=kTaskStatusNew, v_iIsUrgent:=kTaskIsNotUrgent, v_dtDateCreated:=DateTime.Now, v_iCreatedByID:=m_iUserID, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iIsVisible:=v_iIsVisible)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bPMWrkTaskInstance.TaskControl.CreateNew Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oTaskInstance = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
