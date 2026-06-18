Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
' developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Renewals_NET.Renewals")> _
Public NotInheritable Class Renewals
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Renewals"


    Private m_oBusiness As bGIS.Renewals

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' Process modes
    Private m_vTask As Object
    Private m_vNavigate As Object
    Private m_vProcessMode As Object
    Private m_vTransactionType As Object
    Private m_vEffectiveDate As Object

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.pmtrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                ' Failed to call the initialise method.
                result = m_lReturn

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Create the GIS Business Object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGIS.Renewals", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            ' Dont really want to do anything in here

            Return gPMConstants.PMEReturnCode.pmtrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.pmtrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeys
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.pmtrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.pmtrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByVal vTask As Object = Nothing, Optional ByVal vNavigate As Object = Nothing, Optional ByVal vProcessMode As Object = Nothing, Optional ByVal vTransactionType As Object = Nothing, Optional ByVal vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.pmtrue


            If Not Information.IsNothing(vTask) Then


                m_vTask = vTask
            End If


            If Not Information.IsNothing(vNavigate) Then


                m_vNavigate = vNavigate
            End If


            If Not Information.IsNothing(vProcessMode) Then


                m_vProcessMode = vProcessMode
            End If


            If Not Information.IsNothing(vTransactionType) Then


                m_vTransactionType = vTransactionType
            End If


            If Not Information.IsNothing(vEffectiveDate) Then


                m_vEffectiveDate = vEffectiveDate
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewal
    '
    ' Description:
    '
    ' History: 23/05/2001 CTAF - Created.
    '          26/10/2001 SJ - Add IsWhatIfQ parameter
    ' ***************************************************************** '
    Public Function ConfirmRenewal(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, Optional ByVal v_bIsWhatIfQ As Boolean = False) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmRenewal")

        Try

            result = gPMConstants.PMEReturnCode.pmtrue

            ' Call the business object

            m_lReturn = m_oBusiness.ConfirmRenewal(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSchemeID:=v_lSchemeID, v_lPartyCnt:=v_lPartyCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_bIsWhatIfQ:=v_bIsWhatIfQ, v_bAutoConfirm:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".ConfirmRenewal")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".ConfirmRenewal")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPolicyRenewalVersions
    '
    ' Description:
    '
    ' History: 21/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyRenewalVersions(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetPolicyRenewalVersions")

        Try

            result = gPMConstants.PMEReturnCode.pmtrue

            ' Call bGIS

            m_lReturn = m_oBusiness.GetPolicyRenewalVersion(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                Return m_lReturn
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetPolicyRenewalVersions")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetPolicyRenewalVersions")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyRenewalVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRenewalVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ConfirmLapse
    '
    ' Description:
    '
    ' History: 29/05/2001 SSL - Created.
    '
    ' ***************************************************************** '
    Public Function ConfirmLapse(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmLapse")

        Try

            result = gPMConstants.PMEReturnCode.pmtrue

            ' Call the business object

            m_lReturn = m_oBusiness.ConfirmLapse(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSchemeID:=v_lSchemeID, v_lPartyCnt:=v_lPartyCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".ConfirmLapse")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".ConfirmLapse")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmLapse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmLapse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewal
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenMtaAtRenewal(ByVal v_bTransactMtaRenewal As Boolean, ByVal v_lGisSchemeId As Integer, ByVal v_lGisPolicyLinkId As Integer, ByVal v_cOldAnnualPremium As Decimal, ByVal v_cNewAnnualPremium As Decimal, ByVal v_dtEffectiveDate As Date, ByVal v_sGisDataModelCode As String, ByRef r_bIsInRenewalCycle As Boolean, ByRef r_bResetRenewalRecord As Boolean, ByRef r_cNewRenewalPremiumIncIpt As Decimal, ByRef r_cOldRenewalPremiumIncIpt As Decimal) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.pmtrue



            m_lReturn = m_oBusiness.RenMtaAtRenewal(v_bTransactMtaRenewal:=v_bTransactMtaRenewal, v_lGisSchemeId:=v_lGisSchemeId, v_lGisPolicyLinkId:=v_lGisPolicyLinkId, v_cOldAnnualPremium:=v_cOldAnnualPremium, v_cNewAnnualPremium:=v_cNewAnnualPremium, v_dtEffectiveDate:=v_dtEffectiveDate, v_sGisDataModelCode:=v_sGisDataModelCode, r_bIsInRenewalCycle:=r_bIsInRenewalCycle, r_bResetRenewalRecord:=r_bResetRenewalRecord, r_cNewRenewalPremiumIncIpt:=r_cNewRenewalPremiumIncIpt, r_cOldRenewalPremiumIncIpt:=r_cOldRenewalPremiumIncIpt)

            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewal")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
