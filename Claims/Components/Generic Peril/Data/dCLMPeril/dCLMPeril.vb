Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129 (guide)
<System.Runtime.InteropServices.ProgId("Data_NET.Data")> _
Public NotInheritable Class Data
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMPeril
    '
    ' Date: 24/8/2000
    '
    ' Description: Describes the CLMPeril attributes.
    '
    ' Edit History:
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
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
    Private Const ACClass As String = "CLMPeril"

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lClaimID As Integer
    Private m_lPerilID As Integer
    Private m_lPerilTypeID As Integer
    Private m_lpartycnt As Integer

    Private m_sUnderwritingOrAgency As String = ""

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public Function GetTaskGroupCode(ByVal v_sTaskCode As String, ByRef r_sTaskGroupCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "SELECT MAX(TG.code)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM PMWrk_Task T" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN PMWrk_Task_Group_Task TGT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON TGT.pmwrk_task_id = T.pmwrk_task_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN PMWrk_Task_Group TG" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON TG.pmwrk_task_group_id = TGT.pmwrk_task_group_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE T.code = {task_code}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ISNULL(TG.is_deleted,0) = 0" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_code", vValue:=v_sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskGroupCode", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'pass back code

            r_sTaskGroupCode = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskGroupCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property


    Public Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property


    Public Property PerilTypeID() As Integer
        Get
            Return m_lPerilTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeID = Value
        End Set
    End Property


    Public Property Partycnt() As Integer
        Get
            Return m_lpartycnt
        End Get
        Set(ByVal Value As Integer)
            m_lpartycnt = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '

    'Private Function AddKeyInputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '    With m_oDatabase
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="periltypeid", vValue:=CStr(PerilTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(Partycnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '    End With
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
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyInputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyInputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Public Sub New()
        MyBase.New()

        ' Class Initialise


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetControls (Public)
    '
    ' Description: Gets the list of user defined controls for the particular
    '              PerilTypeID
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    Public Function GetControls(ByRef r_vControlsArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            '    With m_oDatabase
            m_oDatabase.Parameters.Clear()

            '        m_lReturn = AddKeyInputParam

            '        If (m_lReturn <> PMTrue) Then
            '            GetControls = PMFalse
            '            Exit Function
            '        End If

            If PerilID <> 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetControlsSQL, sSQLName:=ACGetControlsName, bStoredProcedure:=ACGetControlsStored, vResultArray:=r_vControlsArray)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="periltypeid", vValue:=CStr(PerilTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetControlsViewSQL, sSQLName:=ACGetControlsViewName, bStoredProcedure:=ACGetControlsViewStored, vResultArray:=r_vControlsArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetControls", vApp:=ACApp, vClass:=ACClass, vMethod:="GetControls", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveType (Public)
    '
    ' Description: Gets all the types of Reserve's
    '
    ' Author: Ranjit R
    '
    ' ***************************************************************** '
    Public Function GetReserveType(ByRef r_vReserveTypeArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            '    m_lReturn = AddKeyInputParam()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        GetReserveType = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="periltypeid", vValue:=CStr(PerilTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReserveTypeSQL, sSQLName:=ACGetReserveTypeName, bStoredProcedure:=ACGetReserveTypeStored, vResultArray:=r_vReserveTypeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetReserveType", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveDetails(Public)
    '
    ' Description: Gets the details regarding a Reserve by passing the reserve_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetReserveDetails(ByVal v_vInsuranceFilecnt As Object, ByVal v_vRiskID As Object, ByVal v_vsiriusproduct As Object, ByRef r_vReserveDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase
            m_oDatabase.Parameters.Clear()

            '    m_lReturn = AddKeyInputParam
            '
            '    If (m_lReturn <> PMTrue) Then
            '        GetReserveDetails = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="siriusproduct", vValue:=CStr(v_vsiriusproduct), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="policyid", vValue:=CStr(v_vInsuranceFilecnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="riskid", vValue:=CStr(v_vRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReserveDetailsSQL, sSQLName:=ACGetReserveDetailsName, bStoredProcedure:=ACGetReserveDetailsStored, vResultArray:=r_vReserveDetailsArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetReserveDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPaymentDetails (Public)
    '
    ' Description: Gets the details regarding a Paymnet by passing the payment_id
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    'AK 210503 - added another parameter (user id) for PS 237

    Public Function GetPaymentDetails(ByRef r_vPaymentDetailsArray(,) As Object, Optional ByRef r_iUserId As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            '    m_lReturn = AddKeyInputParam
            '
            '    If (m_lReturn <> PMTrue) Then
            '        GetPaymentDetails = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'AK 210503 - another parameter to insert 'created_by' column in Payments
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CreatedBy", vValue:=CStr(r_iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentDetailsSQL, sSQLName:=ACGetPaymentDetailsName, bStoredProcedure:=ACGetPaymentDetailsStored, vResultArray:=r_vPaymentDetailsArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyDetails (Public)
    '
    ' Description: Gets the details regarding a Party_id
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByRef r_vPartyDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            '    m_lReturn = AddKeyInputParam
            '
            '    If (m_lReturn <> PMTrue) Then
            '        GetPartyDetails = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL, sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, vResultArray:=r_vPartyDetailsArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPartyDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddParty(Public)
    '
    ' Description: Adds a Party to the database
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    Public Function AddParty(ByVal v_vPartyIDArray() As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Object.Equals(v_vPartyIDArray, Nothing) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(v_vPartyIDArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    With m_oDatabase

            For iCount As Integer = v_vPartyIDArray.GetLowerBound(0) To v_vPartyIDArray.GetUpperBound(0) - 1
                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="partyclaimid", vValue:=CStr(v_vPartyIDArray(iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '            m_lReturn = AddKeyInputParam
                '
                '            If (m_lReturn <> PMTrue) Then
                '                AddParty = PMFalse
                '                Exit Function
                '            End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=AcAddPartySQL, sSQLName:=ACAddPartyName, bStoredProcedure:=ACAddPartyStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iCount
            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddParty", vApp:=ACApp, vClass:=ACClass, vMethod:="Add Party", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteParty(Public)
    '
    ' Description: Deletes a Party
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function DeleteParty(ByVal v_vPartyIDArray() As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Object.Equals(v_vPartyIDArray, Nothing) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(v_vPartyIDArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    With m_oDatabase

            For iCount As Integer = v_vPartyIDArray.GetLowerBound(0) To v_vPartyIDArray.GetUpperBound(0) - 1
                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="partyclaimid", vValue:=CStr(v_vPartyIDArray(iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeletePartySQL, sSQLName:=ACDeletePartyName, bStoredProcedure:=ACDeletePartyStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iCount
            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to DeleteParty", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateReserveDetails (Public)
    '
    ' Description: Updates the details for a reserve
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function UpdateReserveDetails(ByVal v_vReserveDetailsArray(,) As Object, Optional ByRef sTransactionType As String = "") As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim dSumInsured As Decimal
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Object.Equals(v_vReserveDetailsArray, Nothing) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(v_vReserveDetailsArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '  DC051201 - added to chekc whether run via Underwriting Or Broking
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = CType(getUnderwritingOrAgency(r_sUnderwritingOrAgency:=sTemp), gPMConstants.PMEReturnCode)
            End If


            For iCount As Integer = v_vReserveDetailsArray.GetLowerBound(1) To v_vReserveDetailsArray.GetUpperBound(1)
                m_oDatabase.Parameters.Clear()

                dSumInsured = v_vReserveDetailsArray(7, iCount)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="reserveid", vValue:=CStr(v_vReserveDetailsArray(g_ciRDAReserveID, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="initialreserve", vValue:=CStr(v_vReserveDetailsArray(g_ciRDAInitialReserve, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="revisedreserve", vValue:=CStr(v_vReserveDetailsArray(g_ciRDARevisedReserve, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="average", vValue:=CStr(v_vReserveDetailsArray(g_ciRDAAverage, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="paidtodate", vValue:=CStr(v_vReserveDetailsArray(g_ciRDAPaidtoDate, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                m_lReturn = m_oDatabase.Parameters.Add(sName:="this_payment", vValue:=CStr(v_vReserveDetailsArray(g_ciRDAThisPayment, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="this_revision", vValue:=CStr(v_vReserveDetailsArray(g_ciRDAThisRevision, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="revised_entered", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sTransactionType <> "" Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured", vValue:=CStr(dSumInsured), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateReserveDetailsSQL, sSQLName:=ACUpdateReserveDetailsName, bStoredProcedure:=ACUpdateReserveDetailsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateReserveDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserveDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePaymentDetails (Public)
    '
    ' Description: Updates the details for a payment
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    'Public Function UpdatePaymentDetails(ByVal v_vPaymentDetailsArray As Variant, ByRef r_lSequenceNo As Long) As Long
    '
    'Dim iCount As Integer
    '
    'On Error GoTo Err_UpdatePaymentDetails
    '
    '    UpdatePaymentDetails = PMTrue
    '
    '    If IsEmpty(v_vPaymentDetailsArray) Then
    '        UpdatePaymentDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    If Not IsArray(v_vPaymentDetailsArray) Then
    '        UpdatePaymentDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    r_lSequenceNo = 0
    '
    '    For iCount = LBound(v_vPaymentDetailsArray, 2) To UBound(v_vPaymentDetailsArray, 2)
    '
    '
    '        AddParameterLite m_oDatabase, "Paymentid", v_vPaymentDetailsArray(g_ciPDAPaymentID, iCount), PMParamInput, PMLong, True
    '        AddParameterLite m_oDatabase, "amount", v_vPaymentDetailsArray(g_ciPDAAmount, iCount), PMParamInput, PMCurrency
    '        AddParameterLite m_oDatabase, "partycnt", v_vPaymentDetailsArray(g_ciPDAPartycnt, iCount), PMParamInput, PMLong
    '        AddParameterLite m_oDatabase, "comments", v_vPaymentDetailsArray(g_ciPDAComments, iCount), PMParamInput, PMString
    '        AddParameterLite m_oDatabase, "tax_amount", v_vPaymentDetailsArray(g_ciPDATaxAmount, iCount), PMParamInput, PMCurrency
    '
    '        If Val(v_vPaymentDetailsArray(g_ciPDAPayeeMediaType, iCount)) = 0 Then
    '            AddParameterLite m_oDatabase, "PayeeMediaType", Null, PMParamInput, PMLong
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeMediaType", v_vPaymentDetailsArray(g_ciPDAPayeeMediaType, iCount), PMParamInput, PMLong
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDAPayeeName, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "PayeeName", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeName", v_vPaymentDetailsArray(g_ciPDAPayeeName, iCount), PMParamInput, PMString
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDAPayeeBankName, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "PayeeBankName", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeBankName", v_vPaymentDetailsArray(g_ciPDAPayeeBankName, iCount), PMParamInput, PMString
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDAPayeeSortCode, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "PayeeSortCode", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeSortCode", v_vPaymentDetailsArray(g_ciPDAPayeeSortCode, iCount), PMParamInput, PMString
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDAPayeeAccountNo, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "PayeeAccountNo", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeAccountNo", v_vPaymentDetailsArray(g_ciPDAPayeeAccountNo, iCount), PMParamInput, PMString
    '        End If
    '
    '        If Val(v_vPaymentDetailsArray(g_ciPDAPayeeCountry, iCount)) = 0 Then
    '            AddParameterLite m_oDatabase, "PayeeCountry", Null, PMParamInput, PMLong
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeCountry", v_vPaymentDetailsArray(g_ciPDAPayeeCountry, iCount), PMParamInput, PMLong
    '
    '
    '
    '
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDAPayeeComments, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "PayeeComments", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "PayeeComments", v_vPaymentDetailsArray(g_ciPDAPayeeComments, iCount), PMParamInput, PMString
    '        End If
    '
    '        If r_lSequenceNo = 0 Then
    '            AddParameterLite m_oDatabase, "SequenceNo", Null, PMParamInputOutput, PMLong
    '        Else
    '            AddParameterLite m_oDatabase, "SequenceNo", r_lSequenceNo, PMParamInputOutput, PMLong
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDACurrencyID, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "Currency_id", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "Currency_id", v_vPaymentDetailsArray(g_ciPDACurrencyID, iCount), PMParamInput, PMString
    '        End If
    '
    '        If Trim(v_vPaymentDetailsArray(g_ciPDAPaymentLossXrate, iCount)) = "" Then
    '            AddParameterLite m_oDatabase, "payment_loss_xrate", Null, PMParamInput, PMString
    '        Else
    '            AddParameterLite m_oDatabase, "payment_loss_xrate", v_vPaymentDetailsArray(g_ciPDAPaymentLossXrate, iCount), PMParamInput, PMString
    '        End If
    '
    '
    '        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePaymentDetailsSQL, _
    ''                                          sSQLName:=ACUpdatePaymentDetailsName, _
    ''                                          bStoredProcedure:=ACUpdatePaymentDetailsStored)
    '
    '            If (m_lReturn <> PMTrue) Then
    '            UpdatePaymentDetails = PMFalse
    '            Exit Function
    '        End If
    '
    '        r_lSequenceNo = m_oDatabase.Parameters.Item("SequenceNo").value
    '
    '    Next iCount
    '
    '
    '    Exit Function
    '
    'Err_UpdatePaymentDetails:
    '
    '    UpdatePaymentDetails = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to UpdatePaymentDetails", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Update Payment details", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    'End Function

    ' ***************************************************************** '
    ' Name: UpdateGeneral(Public)
    '
    ' Description: Updates the details for a user defined field
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function UpdateGeneral(ByVal v_vGeneralDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If v_vGeneralDetailsArray Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(v_vGeneralDetailsArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    With m_oDatabase
            For iCount As Integer = v_vGeneralDetailsArray.GetLowerBound(1) To v_vGeneralDetailsArray.GetUpperBound(1)

                If CStr(v_vGeneralDetailsArray(g_ciGDAUserdefinedperildataid, iCount)) <> "" Then
                    m_oDatabase.Parameters.Clear()


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="userdefinedperildataid", vValue:=CStr(v_vGeneralDetailsArray(g_ciGDAUserdefinedperildataid, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'ED 07102002 - Added ClaimId to ensure correct records updated
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'ED 07102002 - End


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Value", vValue:=CStr(v_vGeneralDetailsArray(g_ciGDAValue, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateGeneralDetailsSQL, sSQLName:=ACUpdateGeneralDetailsName, bStoredProcedure:=ACUpdateGeneralDetailsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next iCount
            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateGeneral", vApp:=ACApp, vClass:=ACClass, vMethod:="Update General", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimLookup (Public)
    '
    ' Description: Gets the list of values for a given lookup table
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetClaimLookup(ByVal v_vclaimlookupid As Object, ByRef r_vLookupArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimlookupid", vValue:=CStr(v_vclaimlookupid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACClaimLookupSQL, sSQLName:=ACClaimLookupName, bStoredProcedure:=ACClaimLookupStored, vResultArray:=r_vLookupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetClaimLookup", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimLookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRecoveryDetails(Public)
    '
    ' Description: Gets the details regarding a Recovery by passing the Recovery_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetRecoveryDetails(ByVal v_vRecoveryType As Object, ByRef r_vRecoveryDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            '    m_lReturn = AddKeyInputParam
            '
            '    If (m_lReturn <> PMTrue) Then
            '       GetRecoveryDetails = PMFalse
            '       Exit Function
            '    End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:=CStr(v_vRecoveryType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRecoveryDetailsSQL, sSQLName:=ACGetRecoveryDetailsName, bStoredProcedure:=ACGetRecoveryDetailsStored, vResultArray:=r_vRecoveryDetailsArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetRecoveryDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecoveryDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '' **************************************************************************** '
    '' Name: GetReceiptDetails(Public)
    ''
    '' Description: Gets the details regarding a Receipt by passing the Receipt_id
    ''
    '' Author: Ranjit R
    ''
    '' **************************************************************************** '
    'Public Function GetReceiptDetails(ByVal v_vRecoveryType As Variant, _
    ''                                  ByRef r_vReceiptDetailsArray As Variant) As Long
    '
    'On Error GoTo Err_GetReceiptDetails
    '
    '    GetReceiptDetails = PMTrue
    '
    '    m_oDatabase.Parameters.Clear
    '
    ''    m_lReturn = AddKeyInputParam
    ''
    ''    If (m_lReturn <> PMTrue) Then
    ''        GetReceiptDetails = PMFalse
    ''        Exit Function
    ''    End If
    '
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''          sName:="PerilID", _
    ''          vValue:=PerilID, _
    ''          iDirection:=PMParamInput, _
    ''          iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetReceiptDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''          sName:="ClaimID", _
    ''          vValue:=ClaimID, _
    ''          iDirection:=PMParamInput, _
    ''          iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetReceiptDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''             sName:="type", _
    ''             vValue:=v_vRecoveryType, _
    ''             iDirection:=PMParamInput, _
    ''             iDataType:=PMInteger)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetReceiptDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReceiptDetailsSQL, _
    ''                                      sSQLName:=ACGetReceiptDetailsName, _
    ''                                      bStoredProcedure:=ACGetReceiptDetailsStored, _
    ''                                      vResultArray:=r_vReceiptDetailsArray)
    '
    '    If (m_lReturn <> PMTrue) Then
    '        GetReceiptDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_GetReceiptDetails:
    '
    '    GetReceiptDetails = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to GetReceiptDetails", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetReceiptDetails", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    'End Function

    ' **************************************************************************** '
    ' Name: AddComments(Public)
    '
    ' Description: Gets the details regarding a Receipt by passing the Receipt_id
    '
    ' Author: Ranjit R
    '
    ' **************************************************************************** '
    Public Function AddComments(ByVal v_vComments As String) As Integer

        'DC240402 -Start
        Dim result As Integer = 0
        Dim vClaimComments(,) As Object
        Dim iTxtPointer As Integer
        Dim sTextLine As String = ""
        Dim iCount As Integer
        'DC240402 -End

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DELETE COMMENT LINES
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimCommentsSQL, sSQLName:=ACDeleteClaimCommentsName, bStoredProcedure:=ACDeleteClaimCommentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'New collection of text
            ReDim vClaimComments(0, 0)

            'Break up the entered comments into a collection of TextLines
            iTxtPointer = 1
            iCount = 0
            While iTxtPointer < v_vComments.Length
                sTextLine = v_vComments.Substring(iTxtPointer - 1, Math.Min(v_vComments.Length, 255))
                ReDim Preserve vClaimComments(0, iCount)

                vClaimComments(0, iCount) = sTextLine
                iTxtPointer += 255
                iCount += 1
            End While

            'ADD COMMENT LINES

            For iCount = vClaimComments.GetLowerBound(1) To vClaimComments.GetUpperBound(1)

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Comment_Id", vValue:=CStr(iCount + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Line", vValue:=CStr(vClaimComments(0, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClaimCommentsSQL, sSQLName:=ACAddClaimCommentsName, bStoredProcedure:=ACAddClaimCommentsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddComments", vApp:=ACApp, vClass:=ACClass, vMethod:="AddComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' **************************************************************************** '
    ' Name: AddCommentsUW(Public)
    '
    ' Description: Gets the details regarding a Receipt by passing the Receipt_id
    '
    ' Author: Ranjit R
    '
    ' **************************************************************************** '
    Public Function AddCommentsUW(ByVal v_vComments As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="comments", vValue:=CStr(v_vComments), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddCommentsSQLUW, sSQLName:=ACAddCommentsNameUW, bStoredProcedure:=ACAddCommentsStoredUW)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddCommentsUW", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCommentsUW", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' **************************************************************************** '
    ' Name: GetComments(Public)
    '
    ' Description: Get the comments for a specific Claim Peril ID
    '
    ' Author: Ranjit R
    '
    ' **************************************************************************** '
    Public Function GetComments(ByRef r_vComments As Object) As Integer

        'DC240402 -Start
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        'DC240402 -End

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AddInputParam = PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="comment_type", vValue:=CStr(2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AddInputParam = PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="entity_id", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AddInputParam = PMFalse
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimCommentsSQL, sSQLName:=ACGetClaimComments, bStoredProcedure:=ACGetClaimCommentsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            r_vComments = vArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetComments", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' **************************************************************************** '
    ' Name: GetComments(Public)
    '
    ' Description: Get the comments for a specific Claim Peril ID
    '
    ' Author: Ranjit R
    '
    ' **************************************************************************** '
    Public Function GetCommentsUW(ByRef r_vComments(,) As Object) As Integer

        'DC240402 -Start
        Dim result As Integer = 0
        'DC240402 -End

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            '    m_lReturn = AddKeyInputParam
            '
            '    If (m_lReturn <> PMTrue) Then
            '        GetComments = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCommentsSQLUW, sSQLName:=ACGetCommentsNameUW, bStoredProcedure:=ACGetCommentsStoredUW, vResultArray:=r_vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetCommentsUW", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' RWH(07/02/2001) (U/W hidden option)
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency(ByRef r_sUnderwritingOrAgency As String) As Integer

        Dim result As Integer = 0
        Try

            If m_sUnderwritingOrAgency <> "" Then
                r_sUnderwritingOrAgency = m_sUnderwritingOrAgency
                Return result
            End If

            result = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_sUnderwritingOrAgency)

            m_sUnderwritingOrAgency = r_sUnderwritingOrAgency

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name : GetPartyName
    '
    ' Desc : get name using party count
    '
    ' Hist : 20/02/2001 Created - Tinny
    '********************************************************************************
    Public Function GetPartyName(ByVal v_lPartyCnt As Integer, ByVal v_sFieldName As String, ByRef r_sResult As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT " & v_sFieldName & " FROM Party WHERE party_cnt = {party_cnt}"
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyName", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'pass back name

            r_sResult = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name : GetClaimPerilDetails
    ' Desc : Get all ClaimPeril details
    '
    ' Hist : 10/01/2006 Created - A.Robinson
    '********************************************************************************
    Public Function GetClaimPerilDetails(ByRef r_vClaimPerilDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURE_NAME As String = "GetClaimPerilDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPerilId", vValue:=CStr(m_lPerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=AC_CLAIM_PERIL_SELECT_SQL, sSQLName:=AC_CLAIM_PERIL_SELECT_NAME, bStoredProcedure:=AC_CLAIM_PERIL_SELECT_SP, vResultArray:=r_vClaimPerilDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vClaimPerilDetails) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_PROCEDURE_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURE_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    '********************************************************************************
    ' Name : GetSystemOption
    '
    ' Desc : get system option value
    '
    ' Hist : 10/05/2001 Created - Tinny
    '********************************************************************************
    Public Function GetSystemOption(ByVal v_lOptionNumber As Integer, ByRef r_sReturn As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(v_lOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetSystemOptionSQL, sSQLName:=ACGetSystemOptionName, bStoredProcedure:=ACGetSystemOptionStored, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sReturn = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
