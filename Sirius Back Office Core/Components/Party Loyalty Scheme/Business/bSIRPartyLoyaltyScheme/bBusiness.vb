Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 03/10/2002
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PartyLoyaltyScheme.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 27/11/2003
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
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As gPMConstants.PMEComponentAction
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    'SP to Add a Party_Loyalty_Scheme
    Private Const ACDirectAddName As String = "AddPartyLoyaltyScheme"
    ' Note the '? =' at the start which is required to access the return value
    'developer guide no. 39(latest guide)
    Private Const ACDirectAddSQL As String = "spu_SIR_Add_PartyLoyaltyScheme"

    'SP to Edit a Party_Loyalty_Scheme
    Private Const ACDirectEditName As String = "UpdatePartyLoyaltyScheme"

    'developer guide no. 39(latest guide)
    Private Const ACDirectEditSQL As String = "spu_SIR_Update_PartyLoyaltyScheme"

    'SP to Delete a Party_Loyalty_Scheme
    Private Const ACDirectDeleteName As String = "DeletePartyLoyaltyScheme"

    'developer guide no. 39(latest guide)
    Private Const ACDirectDeleteSQL As String = "spu_SIR_Delete_PartyLoyaltyScheme"

    'SP to Select a single PartyLoyaltyScheme
    Private Const ACSelectPartyLoyaltySchemeName As String = "SelectPartyLoyaltyScheme"

    'developer guide no. 39(latest guide)
    Private Const ACSelectPartyLoyaltySchemeSQL As String = "spu_SIR_Select_PartyLoyaltyScheme"

    'Result Array columns for GetDetails
    Private Const ACPartyLoyaltySchemeId As Integer = 0
    Private Const ACPartyCnt As Integer = 1
    Private Const ACLoyaltySchemeId As Integer = 2
    Private Const ACLoyaltySchemeName As Integer = 3
    Private Const ACMemberNumber As Integer = 4
    Private Const ACOtherRef As Integer = 5
    Private Const ACStartDate As Integer = 6
    Private Const ACEndDate As Integer = 7
    Private Const ACMainMemberNumber As Integer = 8
    Private Const ACIsActive As Integer = 9

    ' These are constants to supplement the standard function return constants
    ' Not an ideal approach I know but this way more information can be
    ' passed back to the caller without adding a new entry for PMEReturnCode enum
    Private Const ACAlreadyExists As gPMConstants.PMEReturnCode = -1234




    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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
        Const ACMethod As String = "Initialise"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername.Value = sUsername
            g_sPassword.Value = sPassword

            ' Set UserID
            g_iUserID = iUserID

            ' Set Calling Application
            g_sCallingAppName = sCallingAppName

            ' Set Language ID
            g_iLanguageID = iLanguageID

            ' Set Source ID
            g_iSourceID = iSourceID

            ' Set Currency ID
            g_iCurrencyID = iCurrencyID

            ' Set Log Level
            g_iLogLevel = iLogLevel

            ' Get Reference to Database


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="gPMComponentServices.CheckDatabase")
                Return result
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const ACMethod As String = "SetProcessModes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Select a single PartyLoyaltyScheme record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef r_vPartyLoyaltySchemeId As Object = Nothing, Optional ByRef r_vPartyCnt As Object = Nothing, Optional ByRef r_vLoyaltySchemeId As Object = Nothing, Optional ByRef r_vMemberNumber As Object = Nothing, Optional ByRef r_vMainMemberNumber As Object = Nothing, Optional ByRef r_vOtherRef As Object = Nothing, Optional ByRef r_vStartDate As Object = Nothing, Optional ByRef r_vEndDate As Object = Nothing, Optional ByRef r_vIsActive As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetDetails"

        Dim vResultArray(,) As Object = Nothing
        Const klFirstRow As Integer = 0
        Dim vSearchPartyLoyaltySchemeId, vSearchPartyCnt, vSearchLoyaltySchemeId As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not Information.IsNothing(r_vPartyLoyaltySchemeId)) And (Not (Convert.IsDBNull(r_vPartyLoyaltySchemeId) Or IsNothing(r_vPartyLoyaltySchemeId))) Then
                ' Use PartyLoyaltySchemeId to locate the row and set


                vSearchPartyLoyaltySchemeId = r_vPartyLoyaltySchemeId


                vSearchPartyCnt = DBNull.Value


                vSearchLoyaltySchemeId = DBNull.Value
            Else


                If (Not Information.IsNothing(r_vPartyCnt)) And (Not (Convert.IsDBNull(r_vPartyCnt) Or IsNothing(r_vPartyCnt))) And (Not Information.IsNothing(r_vLoyaltySchemeId)) And (Not (Convert.IsDBNull(r_vLoyaltySchemeId) Or IsNothing(r_vLoyaltySchemeId))) Then
                    ' use PartyCnt and LoyaltyScheme id to locate the row


                    vSearchPartyLoyaltySchemeId = DBNull.Value


                    vSearchPartyCnt = r_vPartyCnt


                    vSearchLoyaltySchemeId = r_vLoyaltySchemeId
                Else
                    ' error
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid combination of search parameters", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
                    Return result
                End If

            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the PartyLoyaltySchemeID INPUT parameter

            'Developer Guide 101
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="party_loyalty_scheme_id", vValue:=CStr(vSearchPartyLoyaltySchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_loyalty_scheme_id", vValue:=vSearchPartyLoyaltySchemeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add the PartyCnt INPUT parameter

            'Developer Guide 101
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vSearchPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=vSearchPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add the LoyaltySchemeID INPUT parameter

            'Developer Guide 101
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="loyalty_scheme_id", vValue:=CStr(vSearchLoyaltySchemeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="loyalty_scheme_id", vValue:=vSearchLoyaltySchemeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPartyLoyaltySchemeSQL, sSQLName:=ACSelectPartyLoyaltySchemeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.SQLSelect")
                Return result
            End If

            'Populate the return params
            If Information.IsArray(vResultArray) Then

                If ConvertToTrueDataType(vResultArray(ACPartyLoyaltySchemeId, klFirstRow), r_vPartyLoyaltySchemeId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACPartyCnt, klFirstRow), r_vPartyCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACLoyaltySchemeId, klFirstRow), r_vLoyaltySchemeId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (LoyaltySchemeId)")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACMemberNumber, klFirstRow), r_vMemberNumber) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (MemberNumber)")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACMainMemberNumber, klFirstRow), r_vMainMemberNumber) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (MainMemberNumber)")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACOtherRef, klFirstRow), r_vOtherRef) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (OtherRef)")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACStartDate, klFirstRow), r_vStartDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (StartDate)")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACEndDate, klFirstRow), r_vEndDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (EndDate)")
                    Return result
                End If

                If ConvertToTrueDataType(vResultArray(ACIsActive, klFirstRow), r_vIsActive) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="ConvertToTrueDataType (IsActive)")
                    Return result
                End If

            End If



            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Party Loyalty Scheme ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a Party_Loyalty_Scheme record to the database
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef r_lPartyLoyaltySchemeId As Integer = 0, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vLoyaltySchemeId As Object = Nothing, Optional ByVal v_vMemberNumber As Object = Nothing, Optional ByVal v_vMainMemberNumber As Object = Nothing, Optional ByVal v_vOtherRef As Object = Nothing, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vIsActive As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "DirectAdd"

        Dim lRecordsAffected As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Business Rules...

            'Ensure that the PartyLoyaltyScheme contains valid data


            'Developer Guide No 33 
            m_lReturn = SetDefaultAndValidate(r_vPartyLoyaltySchemeId:=r_lPartyLoyaltySchemeId, r_vPartyCnt:=v_vPartyCnt, r_vLoyaltySchemeId:=v_vLoyaltySchemeId, r_vMemberNumber:=v_vMemberNumber, r_vMainMemberNumber:=v_vMainMemberNumber, r_vOtherRef:=v_vOtherRef, r_vStartDate:=v_vStartDate, r_vEndDate:=v_vEndDate, r_vIsActive:=v_vIsActive)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="SetDefaultAndValidate")
                Return result
            End If


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add a param for the return value from the stored proc
            m_lReturn = m_oDatabase.Parameters.Add(sName:="return_value", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add Party_Loyalty_Scheme_id as an OUTPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_loyalty_scheme_id", vValue:=CStr(r_lPartyLoyaltySchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add party_cnt as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add loyalty_scheme_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="loyalty_scheme_id", vValue:=CStr(v_vLoyaltySchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add member_number as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="membership_number", vValue:=CStr(v_vMemberNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add other_ref as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="other_reference", vValue:=CStr(v_vOtherRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add start_date as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="start_date", vValue:=CStr(v_vStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add end_date as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="end_date", vValue:=CStr(v_vEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add main_member_number as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="main_membership_number", vValue:=CStr(v_vMainMemberNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add is_active as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_active", vValue:=CStr(v_vIsActive), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddSQL, sSQLName:=ACDirectAddName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.SQLAction")
                Return result
            End If

            ' Check if insert failed because row already exists
            If m_oDatabase.Parameters.Item("return_value").Value = -1 Then
                Return ACAlreadyExists
            End If

            ' If record wasn't added, error
            If lRecordsAffected > 0 Then
                ' Added, so return Id of new record if required
                If Not False Then
                    r_lPartyLoyaltySchemeId = m_oDatabase.Parameters.Item("party_loyalty_scheme_id").Value
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="0 rows were added ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectEdit (Public)
    '
    ' Description: Updates a Party_Loyalty_Scheme record to the database
    '
    ' ***************************************************************** '
    Public Function DirectEdit(Optional ByVal v_vPartyLoyaltySchemeId As Object = Nothing, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vLoyaltySchemeId As Object = Nothing, Optional ByVal v_vMemberNumber As Object = Nothing, Optional ByVal v_vMainMemberNumber As Object = Nothing, Optional ByVal v_vOtherRef As Object = Nothing, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vIsActive As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "DirectEdit"

        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Ensure that the PartyLoyaltyScheme contains valid data


            'Developer Guide No 33 
            m_lReturn = SetDefaultAndValidate(r_vPartyLoyaltySchemeId:=v_vPartyLoyaltySchemeId, r_vPartyCnt:=v_vPartyCnt, r_vLoyaltySchemeId:=v_vLoyaltySchemeId, r_vMemberNumber:=v_vMemberNumber, r_vMainMemberNumber:=v_vMainMemberNumber, r_vOtherRef:=v_vOtherRef, r_vStartDate:=v_vStartDate, r_vEndDate:=v_vEndDate, r_vIsActive:=v_vIsActive)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="SetDefaultAndValidate")
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Party_Loyalty_Scheme_id as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_loyalty_scheme_id", vValue:=CStr(v_vPartyLoyaltySchemeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add member_number as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="membership_number", vValue:=CStr(v_vMemberNumber), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add other_ref as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="other_reference", vValue:=CStr(v_vOtherRef), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add start_date as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="start_date", vValue:=CStr(v_vStartDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add end_date as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="end_date", vValue:=CStr(v_vEndDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add main_member_number as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="main_membership_number", vValue:=CStr(v_vMainMemberNumber), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Add cash_float as an input param for an edit

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_active", vValue:=CStr(v_vIsActive), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditSQL, sSQLName:=ACDirectEditName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.SQLAction")
                Return result
            End If

            ' If record wasn't edited, error
            If lRecordsAffected > 0 Then
                ' Updated, No action required

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="0 rows were updated ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a Party_Loyalty_Scheme record and any child
    '              (banking / security) records
    '
    '(NOTE - record will not be deleted if drawer is used)
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByVal v_lPartyLoyaltySchemeId As Integer) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "DirectDelete"

        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Party_Loyalty_Scheme_id as an input param for a delete
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_loyalty_scheme_id", vValue:=CStr(v_lPartyLoyaltySchemeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.Parameters.Add")
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteSQL, sSQLName:=ACDirectDeleteName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.SQLAction")
                Return result
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' deleted, No action required

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="0 rows were deleted ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetMandatoryStatus (Public)
    '
    ' Description: returns the Mandatory fields required.
    '
    ' ***************************************************************** '
    Public Function GetMandatoryStatus(Optional ByRef r_bMandatoryPartyLoyaltySchemeId As Boolean = False, Optional ByRef r_bMandatoryPartyCnt As Boolean = False, Optional ByRef r_bMandatoryLoyaltySchemeId As Boolean = False, Optional ByRef r_bMandatoryMemberNumber As Boolean = False, Optional ByRef r_bMandatoryMainMemberNumber As Boolean = False, Optional ByRef r_bMandatoryOtherRef As Boolean = False, Optional ByRef r_bMandatoryStartDate As Boolean = False, Optional ByRef r_bMandatoryEndDate As Boolean = False, Optional ByRef r_bMandatoryIsActive As Boolean = False) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetMandatoryStatus"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Mandatory status of each variable.

            If Not False Then
                r_bMandatoryPartyLoyaltySchemeId = ACMandatoryPartyLoyaltySchemeId
            End If
            If Not False Then
                r_bMandatoryPartyCnt = ACMandatoryPartyCnt
            End If
            If Not False Then
                r_bMandatoryLoyaltySchemeId = ACMandatoryLoyaltySchemeId
            End If
            If Not False Then
                r_bMandatoryMemberNumber = ACMandatoryMemberNumber
            End If
            If Not False Then
                r_bMandatoryMainMemberNumber = ACMandatoryMainMemberNumber
            End If
            If Not False Then
                r_bMandatoryOtherRef = ACMandatoryOtherRef
            End If
            If Not False Then
                r_bMandatoryStartDate = ACMandatoryStartDate
            End If
            If Not False Then
                r_bMandatoryEndDate = ACMandatoryEndDate
            End If
            If Not False Then
                r_bMandatoryIsActive = ACMandatoryIsActive
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMandatoryStatus Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: SetDefaultAndValidate
    ' PURPOSE: This component has responsibility for writing to the database so
    '          lets check that the data is correct.
    '          Can we rely completely upon the interface  - lets assume not???
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    'Developer Guide No 33
    Private Function SetDefaultAndValidate(ByRef r_vPartyLoyaltySchemeId As String, ByRef r_vPartyCnt As String, ByRef r_vLoyaltySchemeId As String, ByRef r_vMemberNumber As String, ByRef r_vMainMemberNumber As String, ByRef r_vOtherRef As String, ByRef r_vStartDate As Object, ByRef r_vEndDate As Object, ByRef r_vIsActive As String) As Integer
        Dim result As Integer = 0
        Const ACMethod As String = "SetDefaultAndValidate"

        Dim bMandatoryPartyLoyaltySchemeId, bMandatoryPartyCnt, bMandatoryLoyaltySchemeId, bMandatoryMemberNumber, bMandatoryMainMemberNumber, bMandatoryOtherRef, bMandatoryStartDate, bMandatoryEndDate, bMandatoryIsActive As Boolean

        Dim sMessage As String = ""



        result = gPMConstants.PMEReturnCode.PMFalse

        'set default value to "" for parameters that are empty or null
        ' simplifies tests for missing values later on



        If String.IsNullOrEmpty(r_vPartyLoyaltySchemeId) Or Convert.IsDBNull(r_vPartyLoyaltySchemeId) Or IsNothing(r_vPartyLoyaltySchemeId) Then
            r_vPartyLoyaltySchemeId = ""
        End If


        If String.IsNullOrEmpty(r_vPartyCnt) Or Convert.IsDBNull(r_vPartyCnt) Or IsNothing(r_vPartyCnt) Then
            r_vPartyCnt = ""
        End If


        If String.IsNullOrEmpty(r_vLoyaltySchemeId) Or Convert.IsDBNull(r_vLoyaltySchemeId) Or IsNothing(r_vLoyaltySchemeId) Then
            r_vLoyaltySchemeId = ""
        End If


        If String.IsNullOrEmpty(r_vMemberNumber) Or Convert.IsDBNull(r_vMemberNumber) Or IsNothing(r_vMemberNumber) Then
            r_vMemberNumber = ""
        End If


        If String.IsNullOrEmpty(r_vMainMemberNumber) Or Convert.IsDBNull(r_vMainMemberNumber) Or IsNothing(r_vMainMemberNumber) Then
            r_vMainMemberNumber = ""
        End If


        If String.IsNullOrEmpty(r_vOtherRef) Or Convert.IsDBNull(r_vOtherRef) Or IsNothing(r_vOtherRef) Then
            r_vOtherRef = ""
        End If


        If String.IsNullOrEmpty(r_vStartDate) Or Convert.IsDBNull(r_vStartDate) Or IsNothing(r_vStartDate) Then
            r_vStartDate = ""
        ElseIf r_vStartDate <= #12/30/1899# Then
            r_vStartDate = ""
        End If


        If String.IsNullOrEmpty(r_vEndDate) Or Convert.IsDBNull(r_vEndDate) Or IsNothing(r_vEndDate) Then
            r_vEndDate = ""
        ElseIf r_vEndDate <= #12/30/1899# Then
            r_vEndDate = ""
        End If


        If String.IsNullOrEmpty(r_vIsActive) Or Convert.IsDBNull(r_vIsActive) Or IsNothing(r_vIsActive) Then
            r_vIsActive = ""
        End If


        ' Set defaults

        'If main member number is missing then default to the member number - only when adding a new row
        If r_vMainMemberNumber = "" Then
            r_vMainMemberNumber = r_vMemberNumber
        End If

        If r_vIsActive <> "" Then
            Dim dbNumericTemp As Double
            If Double.TryParse(r_vIsActive, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Select Case r_vIsActive
                    Case CStr(TriState.False)
                        r_vIsActive = CStr(0)
                    Case Else
                        r_vIsActive = CStr(1)
                End Select
            End If
        End If


        ' get mandatory status of each control from business object
        m_lReturn = GetMandatoryStatus(r_bMandatoryPartyLoyaltySchemeId:=bMandatoryPartyLoyaltySchemeId, r_bMandatoryPartyCnt:=bMandatoryPartyCnt, r_bMandatoryLoyaltySchemeId:=bMandatoryLoyaltySchemeId, r_bMandatoryMemberNumber:=bMandatoryMemberNumber, r_bMandatoryMainMemberNumber:=bMandatoryMainMemberNumber, r_bMandatoryOtherRef:=bMandatoryOtherRef, r_bMandatoryStartDate:=bMandatoryStartDate, r_bMandatoryEndDate:=bMandatoryEndDate, r_bMandatoryIsActive:=bMandatoryIsActive)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="GetMandatoryStatus")
            Return result
        End If


        ' Error if mandatory field is missing
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd And bMandatoryPartyLoyaltySchemeId Then
            If r_vPartyLoyaltySchemeId = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vPartyLoyaltySchemeId is missing"
            End If
        End If
        If bMandatoryPartyCnt Then
            If r_vPartyCnt = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vPartyCnt is missing"
            End If
        End If
        If bMandatoryLoyaltySchemeId Then
            If r_vLoyaltySchemeId = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vLoyaltySchemeId is missing"
            End If
        End If
        If bMandatoryMemberNumber Then
            If r_vMemberNumber = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vMemberNumber is missing"
            End If
        End If
        If bMandatoryMainMemberNumber Then
            If r_vMainMemberNumber = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vMainMemberNumber is missing"
            End If
        End If
        If bMandatoryOtherRef Then
            If r_vOtherRef = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vOtherRef is missing"
            End If
        End If
        If bMandatoryStartDate Then
            'Developer Guide No. 161
            If (IsNothing(r_vStartDate)) Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vStartDate is missing"
            End If
        End If
        If bMandatoryEndDate Then
            If r_vEndDate = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vEndDate is missing"
            End If
        End If
        If bMandatoryIsActive Then
            If r_vIsActive = "" Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vIsActive is missing"
            End If
        End If


        'Validate contents
        'If (r_vStartDate <> #12/30/1899#) And (r_vEndDate <> #12/30/1899#) Then
        If ((Information.IsDate(r_vStartDate) AndAlso r_vStartDate <> #12/30/1899#) And (Information.IsDate(r_vEndDate) AndAlso r_vEndDate <> #12/30/1899#)) Then
            'If (Information.IsDate(r_vStartDate)) And (Information.IsDate(r_vEndDate)) Then
            If r_vEndDate < r_vStartDate Then
                sMessage = sMessage & Strings.Chr(13) & Constants.vbLf & "r_vEndDate must be >= r_vStartDate"
            End If
            'End If
        End If

        'Handle any errors detected
        If sMessage <> "" Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
            Return result
        End If


        ' Now set all optional params to null if they contain an empty string

        'Developer Guide No:115
        'Starts
        If Not bMandatoryPartyLoyaltySchemeId AndAlso r_vPartyLoyaltySchemeId = "" Then

            r_vPartyLoyaltySchemeId = Nothing
        End If
        If Not bMandatoryPartyCnt AndAlso r_vPartyCnt = "" Then

            r_vPartyCnt = Nothing
        End If
        If Not bMandatoryLoyaltySchemeId AndAlso r_vLoyaltySchemeId = "" Then

            r_vLoyaltySchemeId = Nothing
        End If
        If Not bMandatoryMemberNumber AndAlso r_vMemberNumber = "" Then

            r_vMemberNumber = Nothing
        End If
        If Not bMandatoryMainMemberNumber AndAlso r_vMainMemberNumber = "" Then

            r_vMainMemberNumber = Nothing
        End If
        If Not bMandatoryOtherRef AndAlso r_vOtherRef = "" Then

            r_vOtherRef = Nothing
        End If
        If Not bMandatoryStartDate AndAlso r_vStartDate = "" Then

            r_vStartDate = Nothing
        End If
        If Not bMandatoryEndDate And r_vEndDate.ToString() = "" Then

            r_vEndDate = Nothing
        End If
        If Not bMandatoryIsActive AndAlso r_vIsActive = "" Then

            r_vIsActive = Nothing
        End If
        'Ends

        'All params are valid
        result = gPMConstants.PMEReturnCode.PMTrue


        Return result


    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Const ACMethod As String = "BeginTrans"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.SQLBeginTrans")
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Const ACMethod As String = "CommitTrans"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.CommitTrans")
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Const ACMethod As String = "RollbackTrans"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'LogFailedCall(vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vChild:="m_oDatabase.RollbackTrans")
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'Const ACMethod As String = "Class_Initialize"

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class