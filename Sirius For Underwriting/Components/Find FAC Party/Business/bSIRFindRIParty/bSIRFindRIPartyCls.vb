Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 08/01/2004
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
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStructure As String = ""

    Private m_lDataSource As Integer
    Private m_cInvariantKeys As Collection
    Private m_oPMBusiness As Object
    Private m_vPMResultArray As Object

    Private m_lPartyCnt As Integer
    ' PM Event Business Component (Private)
    Private m_oEvent As Object
    'Private m_oEvent As bSIREvent.Business
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bEnableBranchSelectAtLogon As Boolean
    Private m_sUnderwritingType As String = ""
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    Private m_oLookup As bPMLookup.Business

    Private m_obSIRReinsurance As bSIRReinsuranceRI2007.Form
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskId As Integer
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


    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property RiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

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

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


            m_obSIRReinsurance = New bSIRReinsuranceRI2007.Form
            m_lReturn = m_obSIRReinsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                m_cInvariantKeys = Nothing
                m_oPMBusiness = Nothing
                m_obSIRReinsurance = Nothing
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
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there
    '              are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ClearParameters()
    '
    'Try 
    '
    ' Clear the Databases Parameters Collection
    'If m_oDatabase.Parameters Is Nothing Then
    ' Do Nothing
    'Else
    'm_oDatabase.Parameters.Clear()
    'End If
    '
    '
    ' Added by Scalability Update Program - 30/07/2002
    'm_cInvariantKeys = New Collection()
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Clear Parameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearParameters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        'developer guide no. 98
        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 18/10/2001    Created
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        'developer guide no. 98
        m_lReturn = CType(bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingType), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getUnderwritingType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getUnderwritingType")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result

    End Function

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



    ' ***************************************************************** '
    '
    ' Name: IsUserSystemAdministrator
    '
    ' Description:
    '
    ' History: 17/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function IsUserSystemAdministrator(ByRef r_bIsSystemAdministrator As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACIsUserSysAdminStored As Boolean = True
            Const ACIsUserSysAdminName As String = "IsUserSystemAdministrator"
            'developer guide no. 39
            Const ACIsUserSysAdminSQL As String = "spu_pmuser_is_sysadmin"

            Dim vResultArray(,) As Object

            r_bIsSystemAdministrator = False

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for user_id = " & m_iUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If
            'developer guide no. 39
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for effective_date = " & DateTime.Now, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsUserSysAdminSQL, sSQLName:=ACIsUserSysAdminName, bStoredProcedure:=ACIsUserSysAdminStored, lNumberRecords:=500, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed for " & ACIsUserSysAdminName, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Information.IsArray(vResultArray) Then

                If Conversion.Val(CStr(vResultArray(0, 0))) > 0 Then
                    r_bIsSystemAdministrator = True
                End If
            End If

            'spu_pmuser_is_sysadmin
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserSystemAdministrator Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'QBENZ005
    Public Function FindNow(ByVal vName As String, ByVal vShortName As String, ByVal vFileCode As String, ByVal vValidBranches(,) As Object, ByRef m_vSearchData(,) As Object, ByVal m_lPartyCnt As Integer, ByVal bIsFAx As Boolean, Optional ByVal bIsParticipant As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sBranch As New StringBuilder
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_lPartyCnt > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "partycnt", m_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "partycnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            End If

            If vName.Trim() <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Name", vName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "Name", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            End If

            If vShortName.Trim() <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "ShortName", vShortName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "ShortName", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            End If

            If vFileCode.Trim() <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "FileCode", vFileCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "FileCode", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            End If

            sBranch = New StringBuilder("")
            If Information.IsArray(vValidBranches) Then
                For lCount As Integer = vValidBranches.GetLowerBound(1) To vValidBranches.GetUpperBound(1)
                    If lCount <> vValidBranches.GetUpperBound(1) Then

                        'developer guide no. 162
                        sBranch.Append(CStr(vValidBranches(0, lCount)).Trim() & ",")
                    Else

                        'developer guide no. 162
                        sBranch.Append(CStr(vValidBranches(0, lCount)).Trim())
                    End If
                Next
            End If

            If sBranch.ToString().Trim() <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Branch", sBranch.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            End If

            If bIsParticipant Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "is_ri_broker", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_ri_broker", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
            End If

            If bIsFAx Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Is_FAX", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "Is_FAX", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRIPartySQL, sSQLName:=ACGetRIPartyName, bStoredProcedure:=ACGetRIPartyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vSearchData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindNow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindNow", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Public Function GetBrokerParticipants(ByVal m_lRi_Arrangement_line_id As Integer, ByVal m_iProcessId As Integer, ByRef vArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", m_lRi_Arrangement_line_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBrokerParticipantsSQL, sSQLName:=ACGetBrokerParticipantsName, bStoredProcedure:=ACGetBrokerParticipantsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBrokerParticipants Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBrokerParticipants", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Public Function DeleteBrokerParticipants(ByVal m_lRi_Arrangement_line_id As Integer, ByVal m_iProcessId As Integer, ByVal m_lPartyCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", m_lRi_Arrangement_line_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", m_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelBrokerParticipantsSQL, sSQLName:=ACDelBrokerParticipantsName, bStoredProcedure:=ACDelBrokerParticipantsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteBrokerParticipants Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBrokerParticipants", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    Public Function AddBrokerParticipants(ByVal m_lRi_Arrangement_line_id As Integer, ByVal m_iProcessId As Integer, ByVal lPartyCnt As Integer, ByVal dPart_percent As Double) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", m_lRi_Arrangement_line_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Part_percent", CType(dPart_percent * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBrokerParticipantsSQL, sSQLName:=ACAddBrokerParticipantsName, bStoredProcedure:=ACAddBrokerParticipantsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddBrokerParticipants Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBrokerParticipants", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Public Function AddPlacementRILines(ByVal RiArrangementId As Integer, ByVal vPlacementArray(,) As Object, ByVal vParticipantArray(,) As Object, ByVal bIsFAx As Boolean, ByVal m_cUpperLimit As Decimal, ByVal m_cLowerLimit As Decimal, ByVal m_iProcessId As Integer, ByRef m_lGroupingId As Integer, Optional ByVal ClaimId As Integer = 0, Optional ByRef vAddedFindRIPartyLines() As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try

            Dim sType As String = ""
            Dim iRetained, dPremium_percent As Double
            Dim cTotalPremium As Decimal
            Dim lRi_Arrangement_lineId, lGrouping_id As Integer
            Dim sRiIds As New StringBuilder
            Dim cPremiumTax, cCommTax As Decimal



            result = gPMConstants.PMEReturnCode.PMTrue

            If bIsFAx Then
                sType = "FX"
            Else
                sType = "F"
            End If

            'Cal total premium
            cTotalPremium = 0
            For iRow As Integer = 0 To vPlacementArray.GetUpperBound(0)

                cTotalPremium += CDbl(vPlacementArray(iRow, ACIRI2007Premium))
            Next

            'Begin Transaction
            sRiIds = New StringBuilder("")
            For iRow As Integer = 0 To vPlacementArray.GetUpperBound(0)


                If CStr(vPlacementArray(iRow, ACIRI2007AccType)) = "Retained" Then
                    iRetained = 1
                Else
                    iRetained = 0
                End If


                If m_iProcessId = 1 Then

                    If cTotalPremium > 0 Then

                        dPremium_percent = (CDbl(vPlacementArray(iRow, ACIRI2007Premium)) / cTotalPremium)
                    Else
                        dPremium_percent = 0
                    End If

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_id", RiArrangementId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "type", sType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", vPlacementArray(iRow, ACIRI2007PartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", dPremium_percent, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", vPlacementArray(iRow, ACIRI2007Comm_percent), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vPlacementArray(iRow, ACIRI2007AgreementCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", m_cUpperLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vPlacementArray(iRow, ACIRI2007SumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", vPlacementArray(iRow, ACIRI2007Premium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", vPlacementArray(iRow, ACIRI2007Commission), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", vPlacementArray(iRow, ACIRI2007Tax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", vPlacementArray(iRow, ACIRI2007CommTax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", CType(CDbl(vPlacementArray(iRow, ACIRI2007Participation_percent)) * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "retained_percentage", iRetained, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", m_cLowerLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "priority", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", lRi_Arrangement_lineId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, False)

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPlacementRiLinesSQL, sSQLName:=ACAddPlacementRiLinesName, bStoredProcedure:=ACAddPlacementRiLinesStored)
                ElseIf m_iProcessId = 2 Then  'claims

                    bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", ClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", RiArrangementId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "type", sType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", vPlacementArray(iRow, ACIRI2007PartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "xol_arrangement_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vPlacementArray(iRow, ACIRI2007AgreementCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "priority", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", m_cUpperLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vPlacementArray(iRow, ACIRI2007SumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "reserve", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "payment", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "this_reserve", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "this_payment", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", m_cLowerLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "retained", iRetained, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", CType(CDbl(vPlacementArray(iRow, ACIRI2007Participation_percent)) * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", lRi_Arrangement_lineId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, False)

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClaimPlacementRiLinesSQL, sSQLName:=ACAddClaimPlacementRiLinesName, bStoredProcedure:=ACAddClaimPlacementRiLinesStored)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lRi_Arrangement_lineId = m_oDatabase.Parameters.Item("ri_arrangement_line_id").Value

                If iRow = 0 Then lGrouping_id = lRi_Arrangement_lineId

                If iRow <> vPlacementArray.GetUpperBound(0) Then
                    sRiIds.Append(CStr(lRi_Arrangement_lineId).Trim() & ",")
                Else
                    sRiIds.Append(CStr(lRi_Arrangement_lineId).Trim())
                End If

                'START PN 44646
                If Information.IsArray(vAddedFindRIPartyLines) Then
                    ReDim Preserve vAddedFindRIPartyLines(vAddedFindRIPartyLines.GetUpperBound(0) + 1)
                Else
                    ReDim vAddedFindRIPartyLines(0)
                End If

                vAddedFindRIPartyLines(vAddedFindRIPartyLines.GetUpperBound(0)) = lRi_Arrangement_lineId
                'END PN 44646



                If CStr(vPlacementArray(iRow, ACIRI2007AccType)) = "Broker" Then
                    'Add Broker Participants details according to process id
                    If lRi_Arrangement_lineId > 0 Then 'Row has been added successfully
                        For lCount As Integer = 0 To vParticipantArray.GetUpperBound(0)


                            If ToSafeString(vPlacementArray(iRow, ACIRI2007PartyCnt)) = ToSafeString(vParticipantArray(lCount, ACIBrokerAssociationPartyCnt)) Then
                                'Add broker Participants

                                m_lReturn = AddBrokerParticipants(m_lRi_Arrangement_line_id:=lRi_Arrangement_lineId, m_iProcessId:=m_iProcessId, lPartyCnt:=ToSafeInteger(vParticipantArray(lCount, ACIBrokerPartyCnt)), dPart_percent:=ToSafeDouble(Convert.ToString(vParticipantArray(lCount, ACIBrokerParticipant_percent)).Replace("%"c, "").Trim))

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next
                    Else
                        'Roll Back Transaction
                    End If
                End If
                'Add Fac XOL Tax Values
                If m_iProcessId = 1 Then

                    m_obSIRReinsurance.InsuranceFileCnt = m_lInsuranceFileCnt

                    m_obSIRReinsurance.RiskId = m_lRiskId

                    m_lReturn = m_obSIRReinsurance.CalculateFacTax(v_lArrangementLineID:=lRi_Arrangement_lineId, v_lPartyCnt:=gPMFunctions.ToSafeLong(vPlacementArray(iRow, ACIRI2007PartyCnt)), v_cPremium:=gPMFunctions.ToSafeCurrency(vPlacementArray(iRow, ACIRI2007Premium)), v_cCommission:=gPMFunctions.ToSafeCurrency(vPlacementArray(iRow, ACIRI2007Commission)), r_cPremiumTax:=cPremiumTax, r_cCommissionTax:=cCommTax)
                End If
            Next

            'Update the grouping field of ri_arrangement_line table
            If sRiIds.ToString() <> "" Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", lGrouping_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "sRiIds", sRiIds.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRiArrangementLineGroupingSQL, sSQLName:=ACUpdRiArrangementLineGroupingName, bStoredProcedure:=ACUpdRiArrangementLineGroupingStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lGroupingId = lGrouping_id

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPlacementRILines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPlacementRILines", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function DeletePlacementRILine(ByVal m_lRi_Arrangement_line_id As Integer, ByVal m_iProcessId As Integer, Optional ByVal ClaimId As Integer = 0, Optional ByRef GroupingId As Integer = 0) As Integer


        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_Arrangement_Line_id", m_lRi_Arrangement_line_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Process_Id", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_id", ClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "NewgroupingId", GroupingId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, False)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPlacementRiLinesSQL, sSQLName:=ACDelPlacementRiLinesName, bStoredProcedure:=ACDelPlacementRiLinesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            GroupingId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("NewgroupingId").Value)
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePlacementRILine Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePlacementRILine", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function
    Public Function GetGroupedRiLines(ByVal m_lRi_Arrangement_id As Integer, ByVal m_iProcessId As Integer, ByVal m_lGroupingId As Integer, ByRef vArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "RiArrangementId", m_lRi_Arrangement_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "GroupingId", m_lGroupingId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGroupedRiLinesSQL, sSQLName:=ACGetGroupedRiLinesName, bStoredProcedure:=ACGetGroupedRiLinesStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGroupedRiLines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupedRiLines", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    Public Function GetClaimGroupedRiLines(ByVal m_lClaim_id As Integer, ByVal m_iProcessId As Integer, ByVal m_lGroupingId As Integer, ByRef vArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaim_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "GroupingId", m_lGroupingId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimGroupedRiLinesSQL, sSQLName:=ACGetClaimGroupedRiLinesName, bStoredProcedure:=ACGetClaimGroupedRiLinesStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimGroupedRiLines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimGroupedRiLines", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Public Function UpdatePlacementRILines(ByVal RiArrangementId As Integer, ByVal vPlacementArray(,) As Object, ByVal vParticipantArray(,) As Object, ByVal bIsFAx As Boolean, ByVal m_cUpperLimit As Decimal, ByVal m_cLowerLimit As Decimal, ByVal m_iProcessId As Integer, ByVal m_lGroupingId As Integer, Optional ByVal ClaimId As Integer = 0, Optional ByRef lNewGroupingId As Integer = 0, Optional ByRef vAddedFindRIPartyLines() As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try

            Dim sType As String = ""
            Dim iRetained, dPremium_percent As Double
            Dim cTotalPremium As Decimal
            Dim lRi_Arrangement_lineId, lGrouping_id As Integer
            Dim sRiIds As New StringBuilder
            Dim cPremiumTax, cCommTax As Decimal


            result = gPMConstants.PMEReturnCode.PMTrue

            If bIsFAx Then
                sType = "FX"
            Else
                sType = "F"
            End If

            'Cal total premium
            cTotalPremium = 0
            For iRow As Integer = 0 To vPlacementArray.GetUpperBound(0)

                cTotalPremium += gPMFunctions.ToSafeDouble(vPlacementArray(iRow, ACIRI2007Premium))
            Next

            'Begin Transaction
            sRiIds = New StringBuilder("")
            For iRow As Integer = 0 To vPlacementArray.GetUpperBound(0)


                If CStr(vPlacementArray(iRow, ACIRI2007AccType)) = "Retained" Then
                    iRetained = 1
                Else
                    iRetained = 0
                End If


                If m_iProcessId = 1 Then

                    If cTotalPremium > 0 Then

                        dPremium_percent = (CDbl(vPlacementArray(iRow, ACIRI2007Premium)) / cTotalPremium)
                    Else
                        dPremium_percent = 0
                    End If


                    If gPMFunctions.ToSafeDouble(vPlacementArray(iRow, ACIRI2007RIArrangementLineId)) > 0 Then
                        bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", dPremium_percent, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", vPlacementArray(iRow, ACIRI2007Comm_percent), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vPlacementArray(iRow, ACIRI2007AgreementCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", m_cUpperLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vPlacementArray(iRow, ACIRI2007SumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", vPlacementArray(iRow, ACIRI2007Premium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", vPlacementArray(iRow, ACIRI2007Commission), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", vPlacementArray(iRow, ACIRI2007Tax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", vPlacementArray(iRow, ACIRI2007CommTax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", CType(CDbl(vPlacementArray(iRow, ACIRI2007Participation_percent)) * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "retained_percentage", iRetained, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", m_cLowerLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", vPlacementArray(iRow, ACIRI2007RIArrangementLineId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePlacementRiLinesSQL, sSQLName:=ACUpdatePlacementRiLinesName, bStoredProcedure:=ACUpdatePlacementRiLinesStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        lRi_Arrangement_lineId = CInt(vPlacementArray(iRow, ACIRI2007RIArrangementLineId))
                    Else
                        'Add
                        bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_id", RiArrangementId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "type", sType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", vPlacementArray(iRow, ACIRI2007PartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "premium_percent", dPremium_percent, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "commission_percent", vPlacementArray(iRow, ACIRI2007Comm_percent), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vPlacementArray(iRow, ACIRI2007AgreementCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", m_cUpperLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vPlacementArray(iRow, ACIRI2007SumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "premium_value", vPlacementArray(iRow, ACIRI2007Premium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "commission_value", vPlacementArray(iRow, ACIRI2007Commission), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "premium_tax", vPlacementArray(iRow, ACIRI2007Tax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "commission_tax", vPlacementArray(iRow, ACIRI2007CommTax), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", CType(CDbl(vPlacementArray(iRow, ACIRI2007Participation_percent)) * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "retained_percentage", iRetained, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", m_cLowerLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "is_commission_modified", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "priority", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", lRi_Arrangement_lineId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, False)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPlacementRiLinesSQL, sSQLName:=ACAddPlacementRiLinesName, bStoredProcedure:=ACAddPlacementRiLinesStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        lRi_Arrangement_lineId = m_oDatabase.Parameters.Item("ri_arrangement_line_id").Value

                        'START PN 44646
                        If Information.IsArray(vAddedFindRIPartyLines) Then
                            ReDim Preserve vAddedFindRIPartyLines(vAddedFindRIPartyLines.GetUpperBound(0) + 1)
                        Else
                            ReDim vAddedFindRIPartyLines(0)
                        End If

                        vAddedFindRIPartyLines(vAddedFindRIPartyLines.GetUpperBound(0)) = lRi_Arrangement_lineId
                        'END PN 44646

                    End If
                ElseIf m_iProcessId = 2 Then  'claims

                    If gPMFunctions.ToSafeDouble(vPlacementArray(iRow, ACIRI2007RIArrangementLineId)) > 0 Then
                        bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vPlacementArray(iRow, ACIRI2007AgreementCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", m_cUpperLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vPlacementArray(iRow, ACIRI2007SumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", CType(CDbl(vPlacementArray(iRow, ACIRI2007Participation_percent)) * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "retained", iRetained, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", m_cLowerLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", vPlacementArray(iRow, ACIRI2007RIArrangementLineId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                        'Changed as per VB Code
                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", ClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateClaimPlacementRiLinesSQL, sSQLName:=ACUpdateClaimPlacementRiLinesName, bStoredProcedure:=ACUpdateClaimPlacementRiLinesStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        lRi_Arrangement_lineId = CInt(vPlacementArray(iRow, ACIRI2007RIArrangementLineId))
                    Else
                        'Add

                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", ClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", RiArrangementId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "type", sType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "Treaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", vPlacementArray(iRow, ACIRI2007PartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "xol_arrangement_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", CType(0.0#, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vPlacementArray(iRow, ACIRI2007AgreementCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "priority", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", m_cUpperLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vPlacementArray(iRow, ACIRI2007SumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "reserve", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "this_reserve", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "this_payment", gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", m_cLowerLimit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "retained", iRetained, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", CType(CDbl(vPlacementArray(iRow, ACIRI2007Participation_percent)) * 100, gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", lRi_Arrangement_lineId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, False)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClaimPlacementRiLinesSQL, sSQLName:=ACAddClaimPlacementRiLinesName, bStoredProcedure:=ACAddClaimPlacementRiLinesStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        lRi_Arrangement_lineId = m_oDatabase.Parameters.Item("ri_arrangement_line_id").Value

                        'START PN 44646
                        If Information.IsArray(vAddedFindRIPartyLines) Then
                            ReDim Preserve vAddedFindRIPartyLines(vAddedFindRIPartyLines.GetUpperBound(0) + 1)
                        Else
                            ReDim vAddedFindRIPartyLines(0)
                        End If

                        vAddedFindRIPartyLines(vAddedFindRIPartyLines.GetUpperBound(0)) = lRi_Arrangement_lineId
                        'END PN 44646

                    End If
                End If

                If iRow = 0 And m_lGroupingId = 0 Then
                    lNewGroupingId = lRi_Arrangement_lineId
                End If

                If iRow <> vPlacementArray.GetUpperBound(0) Then
                    sRiIds.Append(CStr(lRi_Arrangement_lineId).Trim() & ",")
                Else
                    sRiIds.Append(CStr(lRi_Arrangement_lineId).Trim())
                End If


                If CStr(vPlacementArray(iRow, ACIRI2007AccType)) = "Broker" Then
                    'Add Broker Participants details according to process id
                    If lRi_Arrangement_lineId > 0 And Not gPMFunctions.IsArrayEmpty(vParticipantArray) Then 'Row has been added successfully
                        For lCount As Integer = 0 To vParticipantArray.GetUpperBound(0)


                            If vPlacementArray(iRow, ACIRI2007PartyCnt).Equals(vParticipantArray(lCount, ACIBrokerAssociationPartyCnt)) Then
                                'Delete if exists

                                m_lReturn = DeleteBrokerParticipants(m_lRi_Arrangement_line_id:=lRi_Arrangement_lineId, m_lPartyCnt:=CInt(vParticipantArray(lCount, ACIBrokerPartyCnt)), m_iProcessId:=m_iProcessId)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                'Add broker Participants


                                m_lReturn = AddBrokerParticipants(m_lRi_Arrangement_line_id:=lRi_Arrangement_lineId, m_iProcessId:=m_iProcessId, lPartyCnt:=CInt(vParticipantArray(lCount, ACIBrokerPartyCnt)), dPart_percent:=CDbl(vParticipantArray(lCount, ACIBrokerParticipant_percent)))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next
                    Else
                        'Roll Back Transaction
                    End If
                End If

                'Add Fac XOL Tax Values
                If m_iProcessId = 1 Then

                    m_obSIRReinsurance.InsuranceFileCnt = m_lInsuranceFileCnt

                    m_obSIRReinsurance.RiskId = m_lRiskId

                    m_lReturn = m_obSIRReinsurance.CalculateFacTax(v_lArrangementLineID:=lRi_Arrangement_lineId, v_lPartyCnt:=gPMFunctions.ToSafeLong(vPlacementArray(iRow, ACIRI2007PartyCnt)), v_cPremium:=gPMFunctions.ToSafeCurrency(vPlacementArray(iRow, ACIRI2007Premium)), v_cCommission:=gPMFunctions.ToSafeCurrency(vPlacementArray(iRow, ACIRI2007Commission)), r_cPremiumTax:=cPremiumTax, r_cCommissionTax:=cCommTax)
                End If
            Next

            'Update the grouping field of ri_arrangement_line table
            If sRiIds.ToString() <> "" Then

                If m_lGroupingId <> 0 Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", m_lGroupingId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", lNewGroupingId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                End If
                bPMAddParameter.AddParameterLite(m_oDatabase, "sRiIds", sRiIds.ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
                bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRiArrangementLineGroupingSQL, sSQLName:=ACUpdRiArrangementLineGroupingName, bStoredProcedure:=ACUpdRiArrangementLineGroupingStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            If m_lGroupingId = 0 Then
                m_lGroupingId = lGrouping_id
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePlacementRILines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePlacementRILines", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

