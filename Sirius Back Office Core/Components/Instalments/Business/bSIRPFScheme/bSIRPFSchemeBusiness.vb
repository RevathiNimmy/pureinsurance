Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
'refer Developer Guide No. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 23/10/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PFScheme.
    '
    ' Edit History:
    ' RAW 06/02/2003 : ISS2053 : changed DB param and field name from Bank_Account_id to BankAccount_id
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 13/01/2004
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


    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
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

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            'refer Developer Guide No. 
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


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


    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)



                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                Next iParam

                'Call the SP
                'Developer Guide No 39
                m_lReturn = .SQLSelect(sSQL:="spe_PFScheme_PLL" & sPickListType, sSQLName:=sPickListType & " PickList Load", bStoredProcedure:=True, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Select Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return m_lReturn
                End If
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

        Try

            BeginTrans()

            If vFKArray.GetUpperBound(1) > 2 And sPickListType.Trim().ToUpper() = "SOURCE" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=vFKArray(2, iParam))
                Next iParam

                'Developer Guide No 39
                m_lReturn = .SQLSelect(sSQL:="spe_PFScheme_PLD" & sPickListType, sSQLName:=sPickListType & " PickList Delete", bStoredProcedure:=True)
                'See if there is anything to save
                If Information.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()

                        'Load the FK parameters
                        For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                            .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=vFKArray(2, iParam))
                        Next iParam


                        .Parameters.Add("Key", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        'Call the SP
                        'Developer Guide No 39
                        m_lReturn = .SQLAction(sSQL:="spe_PFScheme_PLS" & sPickListType, sSQLName:=sPickListType & " PickList Load", bStoredProcedure:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey
                End If
            End With

            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()
            Return result

        End Try
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
    Private Function PickListParams(ByRef vParams(,) As Object) As String

        Dim result As String = String.Empty


        Dim sComma As String = ""
        Dim sParam As New StringBuilder

        sComma = ""
        sParam = New StringBuilder("")
        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam


        Return sParam.ToString()

    End Function
    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single PFScheme directly into the database.
    '' AAB-15-Oct-2003 14:05 - Added 4 New parameters to support Re_insurance
    '                         dripping. v_vReInsuranceSuspenseAccount,
    '                         v_vSpreadReInsurance, v_vSpreadTaxes, v_vDepositAsInstalment
    '  PN12594 Handle Business Code Mandatory Field
    ' ***************************************************************** '
    Public Function DirectAdd(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal vSchemeArray(,) As Object, ByVal sUniqueId As String, ByVal sScreenHierarchy As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                AddKeyParam(lCompanyNo, lSchemeNo, lSchemeVersion)
                AddInputParam(vSchemeArray, sUniqueId, sScreenHierarchy)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Interface allows user to enter duplicate values
                    ' The primary key containt can fail
                    ' When it does, but don't want the system to crash
                    ' We just display a message on the interface to
                    ' ask user to try other values

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectEdit (Public)
    '
    ' Description: Adds a single PFScheme directly into the database.
    ' AAB-15-Oct-2003 14:05 - Added 4 New parameters to support Re_insurance
    '                         dripping. v_vReInsuranceSuspenseAccount,
    '                         v_vSpreadReInsurance, v_vSpreadTaxes, v_vDepositAsInstalment
    '  PN12594 Handle Business Code Mandatory Field
    ' ***************************************************************** '
    Public Function DirectEdit(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal vSchemeArray(,) As Object, ByVal sUniqueId As String, ByVal sScreenHierarchy As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                AddKeyParam(lCompanyNo, lSchemeNo, lSchemeVersion)
                AddInputParam(vSchemeArray, sUniqueId, sScreenHierarchy)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single PFScheme directly from the database.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, Optional sUniqueId As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ''PN-61310 Date 15/1/2010
                If GetIsPFSchemeRateExist(lCompanyNo:=lCompanyNo, lSchemeNo:=lSchemeNo, lSchemeVersion:=lSchemeVersion) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                AddKeyParam(lCompanyNo, lSchemeNo, lSchemeVersion)

                m_lReturn = .Parameters.Add(sName:="UniqueId",
                                        vValue:=sUniqueId,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="UserId",
                                        vValue:=m_iUserID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: GetCompanyNo
    '
    ' Description:
    '
    ' History: 02/10/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetCompanyNo(ByVal lPartyCnt As Integer, ByRef sCompanyNo As String) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing
        Dim str As String = ""

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_GetCompanyNo", sSQLName:="Select CompanyNo from Party", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResult)


            sCompanyNo = CStr(vResult(0, 0))

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCompanyNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompanyNo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CheckMediaType
    '
    ' Description: Check if the Media Type has an associated Media Type
    '              Validation ID
    '
    ' History: PW040303 - Created.
    '
    ' ***************************************************************** '
    Public Function CheckMediaType(ByVal vMediaTypeID As Object, ByRef r_bHasValidation As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing
        Dim str As String = ""

        Try

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="media_type_id", vValue:=CStr(vMediaTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_CheckMediaType", sSQLName:="Check Media Type validation", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResult)

            r_bHasValidation = Information.IsArray(vResult)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMediaType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMediaType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PFSchemes into a return array

    ' ***************************************************************** '
    Public Function GetDetails(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByRef r_vSchemeArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            AddKeyParam(lCompanyNo, lSchemeNo, lSchemeVersion)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, vResultArray:=r_vSchemeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetRatesList
    '
    ' Description:
    '
    ' History: 12/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetRatesList(ByVal vCompanyNo As Object, ByVal vSchemeNo As Object, ByVal vSchemeVersion As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()


            m_oDatabase.Parameters.Add("CompanyNo", CStr(vCompanyNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_oDatabase.Parameters.Add("SchemeNo", CStr(vSchemeNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_oDatabase.Parameters.Add("SchemeVersion", CStr(vSchemeVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFRF_List", sSQLName:="spu_PFRF_saa", bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If Not Information.IsArray(vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatesList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatesList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetSchemeTypes(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_Type_List", sSQLName:="spu_PFScheme_Type_List", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Public Function GetPrintTypes(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_PrintType_List", sSQLName:="spu_PFScheme_PrintType_List", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrintTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrintTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetMediaTypes(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_MediaType_List", sSQLName:="spu_PFScheme_MediaType_List", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetCurrencies(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_currency_selall", sSQLName:="spu_PFSchemeCurrency_List", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'UPGRADE_NOTE: (7001) The following declaration (GetNextPartyFP) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetNextPartyFP(ByRef v_lPartyCnt As Integer, Optional ByRef r_vPartyCode As Object = Nothing, Optional ByRef r_vPartyName As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Dim oPartyFP As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' get instance of PartyFP
    '
    'm_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oPartyFP, v_sClassName:="bSIRPartyFP.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create PartyFP.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPartyFP", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    'End If
    '
    'With oPartyFP
    '

    'm_lReturn = .GetDetails(vPartyCnt:=v_lPartyCnt)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PartyFP.GetDetails() failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPartyFP", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '

    '.Terminate()
    'oPartyFP = Nothing
    'Return result
    'End If
    '

    'm_lReturn = .GetNext(vPartyCnt:=v_lPartyCnt, vShortName:=r_vPartyCode, vName:=r_vPartyName)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PartyFP.GetNext() failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPartyFP", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '

    '.Terminate()
    'oPartyFP = Nothing
    'Return result
    'End If
    '

    '.Terminate()
    '
    'End With
    '
    'oPartyFP = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextPartyFP Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPartyFP", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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




    '********************************************************************************
    ' Name: GetDocTemplate
    '
    ' Description: Get document template details
    '
    ' History:
    '   Thinh Nguyen 04/12/2001 Created
    '   Peter Finney 13/12/2001 Pulled into SFU 1.7 Merge
    '********************************************************************************
    Public Function GetDocTemplate(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'Developer Guide No 39
            sSQL = "spu_PFScheme_GetDocTemplate"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get document template details", bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Sub AddKeyParam(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer)

        With m_oDatabase
            m_lReturn = .Parameters.Add(sName:="CompanyNo", vValue:=CStr(lCompanyNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="SchemeNo", vValue:=CStr(lSchemeNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="SchemeVersion", vValue:=CStr(lSchemeVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


        End With

    End Sub

    ''' <summary>
    ''' AddInputParam
    ''' </summary>
    ''' <param name="vSchemeArray"></param>
    ''' <remarks></remarks>
    Private Sub AddInputParam(ByVal vSchemeArray(,) As Object, ByVal sUniqueId As String, ByVal sScreenHierarchy As String)


        With m_oDatabase

            If CDbl(vSchemeArray(bSIRPremFinConst.k_PFSchemePartyCnt, 0)) < 1 Then

                m_lReturn = .Parameters.Add(sName:="Party_Cnt", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                m_lReturn = .Parameters.Add(sName:="Party_Cnt",
                                            vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemePartyCnt, 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = .Parameters.Add(sName:="DataModelCode",
                                        vValue:=
                                           gPMFunctions.ToSafeString(
                                               vSchemeArray(bSIRPremFinConst.k_PFSchemeDataModelCode, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="StartDate",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeStartDate, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)


            m_lReturn = .Parameters.Add(sName:="EndDate",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeEndDate, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)


            'Developer Guide No 85
            If Convert.IsDBNull(vSchemeArray(bSIRPremFinConst.k_PFSchemePaymentMethod, 0)) Then
                m_lReturn = .Parameters.Add(sName:="PaymentMethod_Cnt", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="PaymentMethod_Cnt",
                                            vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemePaymentMethod, 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = .Parameters.Add(sName:="SystemTag",
                                        vValue:=
                                           gPMFunctions.ToSafeString(vSchemeArray(bSIRPremFinConst.k_PFSchemeSystemTag,
                                                                                  0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="SchemeName",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeName, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="SchemeDescription",
                                        vValue:=
                                           gPMFunctions.ToSafeString(
                                               vSchemeArray(bSIRPremFinConst.k_PFSchemeSchemeDescription, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="QuoteableInd",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteableInd, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            'Developer Guide No 85
            If Convert.IsDBNull(vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0)) Then
                m_lReturn = .Parameters.Add(sName:="QuoteDocID", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="QuoteDocID",
                                            vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeQuoteDocID, 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If Convert.IsDBNull(vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0)) Then
                m_lReturn = .Parameters.Add(sName:="BankDocID", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="BankDocID",
                                            vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBankDocID, 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If Convert.IsDBNull(vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0)) Then
                m_lReturn = .Parameters.Add(sName:="CreditDocID", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="CreditDocID",
                                            vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeCreditDocID, 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = .Parameters.Add(sName:="InsrMailBoxNo",
                                        vValue:=
                                           gPMFunctions.ToSafeString(
                                               vSchemeArray(bSIRPremFinConst.k_PFSchemeInsrMailboxNo, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="EDImessagecount",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeEdiMessageCount, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="ImmediateBankDetails",
                                        vValue:=
                                           CStr(
                                               Math.Abs(
                                                   CDbl(vSchemeArray(bSIRPremFinConst.k_PFSchemeImmediateBankDetails, 0)))),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Mediatype_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeMediaTypeID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Currency_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeCurrencyID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="pfScheme_PrintType_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemePrintTypeID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Spread_Commission",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadCommission, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Suspense_Account_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeSuspenseAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Interest_Account_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeInterestAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Admin_Account_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeAdminAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Protection_Account_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProtectionAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Tax_Group_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxGroupID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Tax_Suspense_Account_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeTaxSuspenseAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Commission_Suspense_Account_ID",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeCommissionSuspenseAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="pfscheme_type_id",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemePFSchemeTypeID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Bank_Name_Mandatory",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBankNameMandatory, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Bank_Address_Mandatory",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAddressMandatory, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Branch_Name_Mandatory",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBranchNameMandatory, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Branch_Code_Mandatory",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBranchCodeMandatory, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="BankAccount_ID",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBankAccountID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            'Developer Guide No 85
            If Convert.IsDBNull(vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0)) Then
                m_lReturn = .Parameters.Add(sName:="confirmation_doc_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="confirmation_doc_id",
                                            vValue:=
                                               CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeDocConfirmationID, 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            m_lReturn = .Parameters.Add(sName:="Spread_ri",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadReInsurance, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Spread_taxes",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeSpreadTaxes, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="ri_Suspense_Account_ID",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeReInsuranceSuspenseAccount, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="deposit_as_instalment",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositAsInstalment, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="deposit_on_other_media_type",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositOnOtherMediaType, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="agent_ref_mandatory",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeAgentRefMandatory, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="pf_message",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemePFMessage, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="business_code_mandatory",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeBusinessCodeMandatory, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="receipt_difference",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeReceiptDifference, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="provider_website",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderWebsite, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="provider_username",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderUsername, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="provider_password",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderPassword, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="provider_brokerid",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderBrokerID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="provider_timeout",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderTimeout, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="financial_institution_code",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeFinancialInstitutionCode, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="direct_debit_supplier_name",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeDirectDebitSupplierName, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="direct_debit_supplier_id",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeDirectDebitSupplierID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="remitter",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeRemitter, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            'Developer Guide No 248
            m_lReturn = .Parameters.Add(sName:="processing_days",
                                        vValue:=
                                           CStr(
                                               gPMFunctions.BlankToZero(
                                                   Conversion.Val(vSchemeArray(bSIRPremFinConst.k_PFSchemeProcessingDays,
                                                                               0)))),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            'PN 38433

            m_lReturn = .Parameters.Add(sName:="provider_plbrokerid",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemePLBrokerID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="provider_clbrokerid",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeCLBrokerID, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="provider_prem_threshold",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeProviderPremThreshold, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            '(RC) PLICO 9-10

            'Developer Guide No 98
            m_lReturn = .Parameters.Add(sName:="rates_are_for_information_only",
                                        vValue:=
                                           CStr(
                                               gPMFunctions.BlankToZero(
                                                   vSchemeArray(bSIRPremFinConst.k_PFSchemeRatesForInformationOnly, 0))),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="Allow_client_fees",
                                        vValue:=CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeAllowClientFees, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Developer Guide No 85
            If Convert.IsDBNull(vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID, 0)) Then
                m_lReturn = .Parameters.Add(sName:="ColNotDocID", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="ColNotDocID",
                                            vValue:=
                                               CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDocID,
                                                                 0)),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            m_lReturn = .Parameters.Add(sName:="nColNotNumDays",
                                        vValue:=
                                           CStr(vSchemeArray(bSIRPremFinConst.k_PFSchemeCollectionNotificationDays, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)
            ''End(Saurabh Agrawal)Tech Spec PGR005 Automated Emails(5.8.2)
            ' PN74526
            m_lReturn = .Parameters.Add(sName:="is_plan_reference_editable",
                                        vValue:=
                                           CStr(
                                               gPMFunctions.BlankToZero(
                                                   vSchemeArray(bSIRPremFinConst.k_PFSchemePlanRefEditable, 0))),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)
            ''Start(Saurabh Agrawal)Tech Spec PGR005 Automated Emails(5.8.2)
            m_lReturn = .Parameters.Add(sName:="nSpread_subagent_Commission",
                                        vValue:=
                                           CStr((vSchemeArray(bSIRPremFinConst.k_PFSchemeSubAgentSpreadCommission, 0))),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="nCommission_subagent_Suspense_Account_ID",
                                        vValue:=
                                           CStr(
                                               (vSchemeArray(
                                                   bSIRPremFinConst.k_PFSchemeSubAgentCommissionSuspenseAccountID, 0))),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="UniqueId",
                                        vValue:=sUniqueId,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="UserId",
                                        vValue:=m_iUserID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="ScreenHierarchy",
                                        vValue:=sScreenHierarchy,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

        End With
    End Sub

    Public Function GetBranches(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_Branches_List", sSQLName:="spu_PFScheme_PrintType_List", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ValidateBranches(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByRef r_sInvalidBranches As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ValidateBranches
        ' PURPOSE: Validates the branches saved against a scheme to ensure the base
        ' currency matches the scheme currency
        ' AUTHOR: Danny Davis
        ' DATE: 05 August 2004, 10:50 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="CompanyNo", vValue:=CStr(lCompanyNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="SchemeNo", vValue:=CStr(lSchemeNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="SchemeVersion", vValue:=CStr(lSchemeVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Developer Guide No 39
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_ValidateBranches", sSQLName:="spu_PFScheme_ValidateBranches", bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBranches", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return m_lReturn
                End If
            End With

            r_sInvalidBranches = ""
            If Information.IsArray(vResultArray) Then

                For lItem As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    r_sInvalidBranches = r_sInvalidBranches & Strings.Chr(13) & Strings.Chr(10) & CStr(vResultArray(0, lItem)).Trim()
                Next lItem
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBranches", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally



        End Try
        Return result
    End Function

    Public Function ValidateSchemeNumber(ByVal v_lSchemeNo As Integer, ByVal v_lSchemeVersion As Integer, ByRef r_blSchemeExists As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ValidateSchemeNumber
        ' PURPOSE: Validates the Scheme number and Scheme Version to ensure they are unique
        ' AUTHOR: Richard Taylor
        ' DATE: 06 December 2004
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray As Object
        Dim lItem, lSchemeExists As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="SchemeNo", vValue:=CStr(v_lSchemeNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="SchemeVersion", vValue:=CStr(v_lSchemeVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="SchemeExists", vValue:=CStr(lSchemeExists), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Developer Guide No 39
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_ValidateSchemeNumber", sSQLName:="spu_PFScheme_ValidateSchemeNumber", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateSchemeNumber", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return m_lReturn
                End If

                r_blSchemeExists = .Parameters.Item("SchemeExists").Value

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateSchemeNumber", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function GetAllowClientFee(Optional ByVal v_lPremiumFinanceCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAllowClientFee"

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lPremiumFinanceCnt = 0 Then
                'NB - Are there any possible schemes that allow client fees?
                sSQL = ""
                sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    NULL" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "FROM pfscheme s" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "WHERE s.quoteableind = 'Y'" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "AND s.allow_client_fees = 1" & Strings.Chr(13) & Strings.Chr(10)
            Else
                'MTA - Does this scheme allow client fees?
                sSQL = ""
                sSQL = sSQL & "SELECT" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    NULL" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "FROM pfpremiumfinance pf" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "JOIN pfscheme s" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    ON s.schemeno = pf.schemeno" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND s.schemeversion = pf.schemeversion" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND s.companyno = pf.companyno" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND s.quoteableind = 'Y'" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND s.allow_client_fees = 1" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "WHERE pf.pfprem_finance_cnt = " & CStr(v_lPremiumFinanceCnt) & Strings.Chr(13) & Strings.Chr(10)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllowClientFee", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=GetAllowClientFee", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Public Function chkFinanceNetComm(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByRef r_vSchemeArray(,) As Object) As Integer

        Try

            Dim sSQL As String

            chkFinanceNetComm = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                sSQL = "select * from pfrf where companyNo=" & ToSafeString(lCompanyNo) & " and schemeNo=" & ToSafeString(lSchemeNo) + _
                       " and schemeVersion=" & ToSafeString(lSchemeVersion) & " and finance_net_commission=1"


                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=r_vSchemeArray)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    chkFinanceNetComm = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="chkFinanceNetComm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Function
                End If

            End With

            Exit Function


        Catch excep As System.Exception

            chkFinanceNetComm = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="chkFinanceNetComm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="chkFinanceNetComm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
            Exit Function
        End Try

    End Function



    ''PN-61310 Date 15/1/2010
    Private Function GetIsPFSchemeRateExist(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer) As Boolean

        Dim result As Boolean = False
        Const kMethodName As String = "GetIsPFSchemeRateExist"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing




        'Add the PF Scheme Parameter
        m_oDatabase.Parameters.Clear()
        AddKeyParam(lCompanyNo, lSchemeNo, lSchemeVersion)

        ' Execute SQL Statement
        lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsPFRateExistSQL, sSQLName:=ACIsPFRateExistName, bStoredProcedure:=ACIsPFRateExistStored, vResultArray:=vResultArray)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = False
        End If

        If Information.IsArray(vResultArray) Then
            result = gPMFunctions.ToSafeBoolean(vResultArray(0, 0))
        End If

        Return result
    End Function
End Class
