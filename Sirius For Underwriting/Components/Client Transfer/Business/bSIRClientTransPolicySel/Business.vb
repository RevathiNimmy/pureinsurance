Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles
Imports Artinsoft.VB6.Utils

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ''Start(Saurabh Agrawal) Tech spec Client portfolio transfer()

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 03/10/2008
    '
    ' Description: Creatable Bussiness class which contains all the
    '              methods, business rules required for the
    '               bSIRCLientTranspolicy
    '
    ' Edit History:Saurabh Agrawal
    ' ***************************************************************** '

    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    Private Const ACClass As String = "Business"

    '***Event Type Constants***
    Private Const kEventTypePolicyTrans As String = "POLTRANS"
    '**************************

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    Private m_lReturn As Integer


    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date


    Private m_oEvent As bSIREvent.Business

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



    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable To Initalise the business component", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_lReturn = CreateRequiredObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable To Initalise the business component", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"
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

        Catch ex As Exception






            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

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
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    Private Function CreateRequiredObject() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateRequiredObject"
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oEvent = New bSIREvent.Business
        If m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Unable To Create Business Object", gPMConstants.PMELogLevel.PMLogError)
        End If





        Return result

    End Function


    Private Sub CloseRequiredObject()

        Const kMethodName As String = "CloseRequiredName"


        If Not (m_oEvent Is Nothing) Then

            m_oEvent.Dispose()
            m_oEvent = Nothing
        End If




    End Sub


    ' ***************************************************************** '
    ' Name: CreateBusinessObject
    '
    ' Description: Creates an instance of the class name passed.
    '
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' create an instance of specified object
    ' ***************************************************************** '
    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateBusinessObject"
        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Unable To Create Business object", gPMConstants.PMELogLevel.PMLogError)
        End If




        Return result

    End Function
    ' ***************************************************************** '
    ' Name:TransferClientPolicies
    '
    ' Description: Function to Transfer Policies from one client to another
    '
    ' ***************************************************************** '
    Public Function TransferClientPolicies(ByRef r_vPolicies() As Object, ByRef r_sFromClientCode As String, ByRef r_sToClientCode As String, ByRef v_lFromClientCnt As Integer, ByRef v_lToClientCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "TransferClientPolicies"

        result = gPMConstants.PMEReturnCode.PMTrue



        Try

            Dim sMessage As String = ""

            ''Event Log Message
            sMessage = "Client Portfolio Transfer " & r_sFromClientCode & " To Client " & r_sToClientCode




            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLBeginTransactiopn Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ''Check for the policies
            If Not Information.IsArray(r_vPolicies) Then
                gPMFunctions.RaiseError(kMethodName, "Policy Transfer Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '' Transfer the policies.

            For Each r_vPolicies_item As Object In r_vPolicies
                m_oDatabase.Parameters.Clear()

                bPMAddParameter.AddParameterLite(m_oDatabase, "from_party_cnt", v_lFromClientCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "to_party_cnt", v_lToClientCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", r_vPolicies_item, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)



                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACTransferpolicies, sSQLName:=ACTransferPoliciesName, bStoredProcedure:=ACTransferPoliciesStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Policy Transfer Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ''''' 68999

                m_oEvent.m_oDatabase = m_oDatabase
                m_lReturn = m_oEvent.DirectAdd(vInsuranceFileCnt:=r_vPolicies_item, vPartyCnt:=v_lToClientCnt, vEventTypeCode:=kEventTypePolicyTrans, vUserId:=m_iUserID, vDescription:=sMessage)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Policy Transfer Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Next r_vPolicies_item
            ''Create Event Log for each Client

            m_lReturn = m_oEvent.DirectAdd(vPartyCnt:=v_lToClientCnt, vEventTypeCode:=kEventTypePolicyTrans, vUserId:=m_iUserID, vDescription:=sMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Policy Transfer Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            m_lReturn = m_oEvent.DirectAdd(vPartyCnt:=v_lFromClientCnt, vEventTypeCode:=kEventTypePolicyTrans, vUserId:=m_iUserID, vDescription:=sMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Policy Transfer Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oDatabase.SQLCommitTrans()


        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            m_oDatabase.SQLRollbackTrans()
        Finally



        End Try
        Return result
    End Function
    ''End(Saurabh Agrawal) Tech spec Client portfolio transfer
End Class
