Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: bSIRProductOptions
    '
    ' Date: 6th June 2002
    '
    ' Description: Business objects to retrieve product option
    '
    ' Edit History:
    '   11062002 SJP - Created
    '   27062005 CJB - PN21983 In UpdateHandlerData & UpdateExecutiveData also pass
    '                  insurance_folder_cnt to WritePolicyHandlerEvent & WritePolicyExecutiveEvent
    '                  (and in those to m_oSIREvent.DirectAdd call) as it is rqd for policy level notes.
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "Business"
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_oSIREvent As bSIREvent.Business
    Private m_oPMLookup As bPMLookup.Business


    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lClientEventTypeId As Integer
    Private m_lPolicyEventTypeId As Integer



    Private m_lOldHandlerCnt As Integer
    Private m_lNewHandlerCnt As Integer
    Private m_sOldHandlerRef As String = ""
    Private m_sNewHandlerRef As String = ""

    Private m_lOldExecutiveCnt As Integer
    Private m_lNewExecutiveCnt As Integer
    Private m_sOldExecutiveRef As String = ""
    Private m_sNewExecutiveRef As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property


    Public Property OldHandlerCnt() As Integer
        Get

            Return m_lOldHandlerCnt

        End Get
        Set(ByVal Value As Integer)

            m_lOldHandlerCnt = Value

        End Set
    End Property
    Public Property NewHandlerCnt() As Integer
        Get

            Return m_lNewHandlerCnt

        End Get
        Set(ByVal Value As Integer)

            m_lNewHandlerCnt = Value

        End Set
    End Property
    Public Property OldHandlerRef() As String
        Get

            Return m_sOldHandlerRef

        End Get
        Set(ByVal Value As String)

            m_sOldHandlerRef = Value

        End Set
    End Property
    Public Property NewHandlerRef() As String
        Get

            Return m_sNewHandlerRef

        End Get
        Set(ByVal Value As String)

            m_sNewHandlerRef = Value

        End Set
    End Property

    Public Property OldExecutiveCnt() As Integer
        Get

            Return m_lOldExecutiveCnt

        End Get
        Set(ByVal Value As Integer)

            m_lOldExecutiveCnt = Value

        End Set
    End Property
    Public Property NewExecutiveCnt() As Integer
        Get

            Return m_lNewExecutiveCnt

        End Get
        Set(ByVal Value As Integer)

            m_lNewExecutiveCnt = Value

        End Set
    End Property
    Public Property OldExecutiveRef() As String
        Get

            Return m_sOldExecutiveRef

        End Get
        Set(ByVal Value As String)

            m_sOldExecutiveRef = Value

        End Set
    End Property
    Public Property NewExecutiveRef() As String
        Get

            Return m_sNewExecutiveRef

        End Get
        Set(ByVal Value As String)

            m_sNewExecutiveRef = Value

        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property



    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    ' Create an instance of the object manager.
            '    Set g_oObjectManager = New bObjectManager.ObjectManager
            '
            '    ' Call the initialise method.
            '    m_lReturn& = g_oObjectManager.Initialise( _
            ''        sCallingAppName:=ACApp)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to call the initialise method.
            '        Initialise = PMFalse
            '
            '        ' Set the object manager to nothing.
            '        Set g_oObjectManager = Nothing
            '
            '        ' Log Error.
            '        LogMessagePopup _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to initialise the object manager", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Initialise"
            '
            '        Exit Function
            '    End If



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

            ' Get an instance of the party object

            m_oSIREvent = New bSIREvent.Business
            m_lReturn = m_oSIREvent.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create PM Lookup Business Object

            m_oPMLookup = New bPMLookup.Business
            m_lReturn = m_oPMLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lReturn = g_oObjectManager.GetInstance( _
            ''        oObject:=m_oSIREvent, _
            ''        sClassName:="bSIREvent.Business", _
            ''        vInstanceManager:=PMGetViaClientManager)
            '
            '    If (m_lReturn <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If
            '
            '   m_lReturn = g_oObjectManager.GetInstance( _
            ''        oObject:=m_oPMLookup, _
            ''        sClassName:="bPMLookup.Business", _
            ''        vInstanceManager:=PMGetViaClientManager)
            '
            '    If (m_lReturn <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If


            m_lReturn = m_oPMLookup.GetEffectiveIDFromCode(v_sTableName:="event_type", v_sCode:="CLICHANGE", v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lId:=m_lClientEventTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oPMLookup.GetEffectiveIDFromCode(v_sTableName:="event_type", v_sCode:="POLCHANGE", v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lId:=m_lPolicyEventTypeId)

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
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lNewExecutiveCnt <> 0 Then
                m_lReturn = UpdateExecutiveData()
            End If
            If m_lNewHandlerCnt <> 0 Then
                m_lReturn = UpdateHandlerData()
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 06/06/2002 SJP - Created.
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_oSIREvent IsNot Nothing Then
                    m_oSIREvent.Dispose()
                    m_oSIREvent = Nothing
                End If
                If m_oPMLookup IsNot Nothing Then
                    m_oPMLookup.Dispose()
                    m_oPMLookup = Nothing
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
    Public Function UpdateExecutiveData() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UpdateClients
        ' PURPOSE: Function To update the account executive on all
        '          matching clients
        ' AUTHOR: Elaine Knott
        ' DATE: MAR 2005
        ' REMARKS: 2005
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vClients(,) As Object = Nothing
        Dim vPolicies(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(GetClients(vClients), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateExecutiveData - SelectClientsSQL Failed")
            End If


            If Information.IsArray(vClients) Then


                For lCount As Integer = 0 To vClients.GetUpperBound(1)


                    m_lReturn = CType(UpdateClientAccountExecutive(CInt(vClients(0, lCount))), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateExecutiveData - Update Account Executive Failed")
                    End If



                    m_lReturn = CType(WriteClientEvent(CInt(vClients(0, lCount))), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateClients - Write Client Event Failed")
                    End If

                Next lCount

            End If



            m_lReturn = CType(GetPoliciesForExecutive(vPolicies), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateExecutiveData - SelectPOlciesForExecutiveSQL Failed")
            End If

            m_lReturn = CType(AuditForTransferData(m_sUniqueId, m_sScreenHierarchy), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - AuditForTransferData - Failed")
            End If

            If Not Information.IsArray(vPolicies) Then
                Return result
            End If


            For lCount As Integer = 0 To vPolicies.GetUpperBound(1)


                m_lReturn = CType(UpdatePolicyAccountExecutive(CInt(vPolicies(0, lCount))), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdatePolicyExecutiveData - Update Policy Account Executive Failed")
                End If





                m_lReturn = CType(WritePolicyExecutiveEvent(CInt(vPolicies(0, lCount)), CInt(vPolicies(1, lCount)), CInt(vPolicies(2, lCount))), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdatePolicies - Write POlicy Executive Event Failed")
                End If

            Next lCount
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateExecutiveData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    Public Function UpdateHandlerData() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UpdateHandlerData
        ' PURPOSE: Function To update the account handler on all
        '          matching policies
        ' AUTHOR: Elaine Knott
        ' DATE: MAR 2005
        ' REMARKS: 2005
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim vPolicies(,) As Object = Nothing



        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(GetPoliciesForHandler(vPolicies), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateHandlerData - SelectPoliciesSQL Failed")
            End If


            If Information.IsArray(vPolicies) Then

                For lCount As Integer = 0 To vPolicies.GetUpperBound(1)

                    m_lReturn = CType(UpdateAccountHandler(CInt(vPolicies(0, lCount))), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateHandlerData - Update Account Handler Failed")
                    End If

                    m_lReturn = CType(WritePolicyHandlerEvent(CInt(vPolicies(0, lCount)), CInt(vPolicies(1, lCount)), CInt(vPolicies(2, lCount))), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateHandlerData - Write Policy Event Failed")
                    End If

                Next lCount

            End If

            m_lReturn = CType(AuditForTransferData(m_sUniqueId, m_sScreenHierarchy), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - AuditForTransferData - Failed")
            End If

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateHandlerData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClients (Private)
    '
    ' Description: Get Relevent Clients
    '
    ' ***************************************************************** '
    Public Function GetClients(ByRef vClients(,) As Object) As Integer


        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="oldexecutive_cnt", vValue:=CStr(m_lOldExecutiveCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLSelect(sSQL:=SelectClientsSQL, sSQLName:=SelectClientsName, bStoredProcedure:=SelectClientsStored, vResultArray:=vClients, lNumberRecords:=gPMConstants.PMAllRecords)


                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - GetClients - SelectClientsSQL Failed")
                End If

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClients", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' Name: GetPoliciesForExecutive (Private)
    '
    ' Description: Get Relevent Policies
    '
    ' ***************************************************************** '
    Public Function GetPoliciesForExecutive(ByRef vPolicies(,) As Object) As Integer


        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="oldexecutive_cnt", vValue:=CStr(m_lOldExecutiveCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLSelect(sSQL:=SelectPoliciesForExecutiveSQL, sSQLName:=SelectPoliciesForExecutiveName, bStoredProcedure:=SelectPoliciesForExecutiveStored, vResultArray:=vPolicies, lNumberRecords:=gPMConstants.PMAllRecords)


                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - GetPoliciesForExecutive Failed")
                End If

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesForExecutive", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetPoliciesForHandler (Private)
    '
    ' Description: Get Relevent Policies
    '
    ' ***************************************************************** '
    Public Function GetPoliciesForHandler(ByRef vPolicies(,) As Object) As Integer


        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="oldhandler_cnt", vValue:=CStr(m_lOldHandlerCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLSelect(sSQL:=SelectPoliciesForHandlerSQL, sSQLName:=SelectPoliciesForHandlerName, bStoredProcedure:=SelectPoliciesForHandlerStored, vResultArray:=vPolicies, lNumberRecords:=gPMConstants.PMAllRecords)


                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - GetPoliciesForHandler Failed")
                End If

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesForHandler", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateClientAccountExec (Private)
    '
    ' Description: Update the Client with a New Account Executive
    '
    ' ***************************************************************** '
    Public Function UpdateClientAccountExecutive(ByVal lPartyCnt As Integer) As Integer


        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)



                m_lReturn = .Parameters.Add(sName:="newexecutive_cnt", vValue:=CStr(m_lNewExecutiveCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLAction(sSQL:=UpdateClientSQL, sSQLName:=UpdateClientName, bStoredProcedure:=UpdateClientStored)


                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateClientAccountExecutive - UpdateClientSQL Failed")
                End If

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClientAccountExecutive", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: UpdateAccountExec (Private)
    '
    ' Description: Update the Client with a New Account Executive
    '
    ' ***************************************************************** '
    Public Function UpdatePolicyAccountExecutive(ByVal lInsuranceFileCnt As Integer) As Integer


        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)



                m_lReturn = .Parameters.Add(sName:="newexecutive_cnt", vValue:=CStr(m_lNewExecutiveCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLAction(sSQL:=UpdatePolicyExecutiveSQL, sSQLName:=UpdatePolicyExecutiveName, bStoredProcedure:=UpdatePolicyExecutiveStored)


                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdatePolicyAccountExecutive - UpdateClientSQL Failed")
                End If

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyAccountExecutive", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function



    ' ***************************************************************** '


    ' ***************************************************************** '
    ' Name: UpdateAccountHandler (Private)
    '
    ' Description: Update the Client with a New Account Executive
    '
    ' ***************************************************************** '
    Public Function UpdateAccountHandler(ByVal lInsuranceFileCnt As Integer) As Integer


        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)



                m_lReturn = .Parameters.Add(sName:="newhandler_cnt", vValue:=CStr(m_lNewHandlerCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLAction(sSQL:=UpdatePolicyHandlerSQL, sSQLName:=UpdatePolicyHandlerName, bStoredProcedure:=UpdatePolicyHandlerStored)


                ' Database error encountered

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - UpdateAccountHandler - UpdatePolicySQL Failed")
                End If

            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAccountHandler", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: WriteClientEvent (Private)
    '
    ' Description: Write Change of Client Event
    '
    ' ***************************************************************** '
    Public Function WriteClientEvent(ByVal lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""


        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            sDescription = "Changed Client Account Executive from " & m_sOldExecutiveRef.Trim() & " to " & m_sNewExecutiveRef.Trim()



            m_lReturn = m_oSIREvent.DirectAdd(vPartyCnt:=lPartyCnt, vEventType:=m_lClientEventTypeId, vDescription:=sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - WriteClientEvent Failed")
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteClientEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: WritePolicyHandlerEvent (Private)
    '
    ' Description: Write Change of Policy Event for Account Handler
    '
    ' ***************************************************************** '
    Public Function WritePolicyHandlerEvent(ByVal lInsuranceFileCnt As Integer, ByVal lPartyCnt As Integer, ByVal lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""


        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            sDescription = "Changed Policy Account Handler from " & m_sOldHandlerRef.Trim() & " to " & m_sNewHandlerRef.Trim()


            m_lReturn = m_oSIREvent.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vEventType:=m_lPolicyEventTypeId, vDescription:=sDescription, vInsuranceFolderCnt:=lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - WritePolicyHandlerEvent Failed")
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="WritePolicyEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: WritePolicyExecutiveEvent (Private)
    '
    ' Description: Write Change of Policy Event for Account Executive
    '
    ' ***************************************************************** '
    Public Function WritePolicyExecutiveEvent(ByVal lInsuranceFileCnt As Integer, ByVal lPartyCnt As Integer, ByVal lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""


        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            sDescription = "Changed Policy Account Executive from " & m_sOldExecutiveRef.Trim() & " to " & m_sNewExecutiveRef.Trim()


            m_lReturn = m_oSIREvent.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vEventType:=m_lPolicyEventTypeId, vDescription:=sDescription, vInsuranceFolderCnt:=lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - WritePolicyExecutiveEvent Failed")
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="WritePolicyEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    Private Function AuditForTransferData(Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Dim code As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_lNewHandlerCnt <> 0 Then
                code = "AH"
            ElseIf m_lNewExecutiveCnt <> 0 Then
                code = "CO"
            End If
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="Code", vValue:=CStr(code), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If code = "AH" Then
                    m_lReturn = .Parameters.Add(sName:="OldReference", vValue:=CStr(m_sOldHandlerRef.Trim()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = .Parameters.Add(sName:="NewReference", vValue:=CStr(m_sNewHandlerRef.Trim()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="OldReferenceCnt", vValue:=m_lOldHandlerCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                ElseIf code = "CO" Then
                    m_lReturn = .Parameters.Add(sName:="OldReference", vValue:=CStr(m_sOldExecutiveRef.Trim()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = .Parameters.Add(sName:="NewReference", vValue:=CStr(m_sNewExecutiveRef.Trim()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="OldReferenceCnt", vValue:=m_lOldExecutiveCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=InsertConfigurationAuditDetailsSQL, sSQLName:=InsertConfigurationAuditDetailsName, bStoredProcedure:=InsertConfigurationAuditDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - AuditForTransferData - InsertConfigurationAuditDetailsSQL Failed")
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bSIRHandlerUpdate.Business - AuditForTransferData Failed")
            End If
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AuditForTransferData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
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
