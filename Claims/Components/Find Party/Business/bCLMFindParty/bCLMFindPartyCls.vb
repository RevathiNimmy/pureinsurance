Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ***************************************************************** '
    ' Class Name: FindParty
    '
    ' Date: 27th September 1996
    '
    ' Description: Creatable FindParty class used by the Find Party
    '              lookup.
    '
    ' Edit History:
    ' SP011298 - changes to support new business roadmap
    ' ***************************************************************** '

    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

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
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lDataSource As Integer
    Private m_cInvariantKeys As Collection
    Private m_oPMBusiness As Object
    Private m_vPMResultArray As Object

    ' Primary Keys to work with
    Private m_lPartyClaimID As Integer

    ' PM Event Business Component (Private)
    Private m_oEvent As Object

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
    Public Property PartyClaimID() As Integer
        Get
            Return m_lPartyClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyClaimID = Value
        End Set
    End Property
    Public WriteOnly Property DataSource() As Integer
        Set(ByVal Value As Integer)
            m_lDataSource = Value
        End Set
    End Property

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

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

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
               
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                m_cInvariantKeys = Nothing
                m_oPMBusiness = Nothing
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

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
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

    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
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


    'DC120701 add extra check for Underwriting Or Broking
    Public Function SearchByQuery(ByRef r_vRefArray(,) As Object, Optional ByRef vName As String = "", Optional ByRef vAddress As String = "", Optional ByRef vPhoneNumber As String = "", Optional ByRef vClaimPartyType As String = "", Optional ByRef vBrokingOrUnderwriting As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vTypeID As Object
        Dim sAddSql, sSqlSelect As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC120701 added extra address lines for broking
            sSqlSelect = "select a.Party_Claim_id,a.Claim_Party_type_id, a.Name, a.Address, " & _
                         "a.License_type ,License_type.description," & _
                         "a.License_Number, a.Date_of_Birth, a.Sex, a.party_status,  driver_status.description, a.Phone_Number, a.Fax_Number,a.Reference_Number," & _
                         "a.Reg_Number from party_claim a left outer join License_type on a.License_type = License_type.License_type_id," & _
                         "party_claim b left outer join Driver_Status on b.Party_Status = Driver_Status.driver_status_id where a.party_claim_id = b.party_claim_id"


            If vName <> "" Then
                If (vName.IndexOf("%"c) + 1) = 0 Then
                    vName = vName & "%"
                End If
                sSqlSelect = sSqlSelect & " AND a.name Like '" & vName & "'"
            End If

            If vAddress <> "" Then
                If (vAddress.IndexOf("%"c) + 1) = 0 Then
                    vAddress = vAddress & "%"
                End If

                'DC120701 -start -cater for different address field
                sSqlSelect = sSqlSelect & " AND a.Address Like '" & vAddress & "'"


            End If

            If vPhoneNumber <> "" Then
                If (vPhoneNumber.IndexOf("%"c) + 1) = 0 Then
                    vPhoneNumber = vPhoneNumber & "%"
                End If

                sSqlSelect = sSqlSelect & " And a.Phone_Number Like '" & vPhoneNumber & "'"
            End If

            If vClaimPartyType <> "" And vClaimPartyType <> "<ALL>" Then
                sAddSql = "Select Claim_Party_type_id from Claim_Party_Type where description='" & vClaimPartyType & "'"
                m_lReturn = m_oDatabase.SQLSelect(sAddSql, ACFindTypeName, ACFindTypeStored, , vTypeID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vTypeID) Then

                    sSqlSelect = sSqlSelect & " AND a.Claim_party_type_id=" & CStr(vTypeID(0, 0))
                End If
            End If



            r_vRefArray = Nothing
            m_lReturn = m_oDatabase.SQLSelect(sSqlSelect, ACFindPartyName, ACFindPartyStored, , r_vRefArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
