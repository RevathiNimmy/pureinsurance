Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
    Public Const ACApp As String = "bSIRFutureAddressUpdate"
    Private m_oBatch As bSIRParty.Business
	Private m_lReturn As gPMConstants.PMEReturnCode
	
    Public Sub Main()

        Try

            Dim g_sUserName As New FixedLengthString(12)
            Dim g_sPassword As New FixedLengthString(30)
            Dim g_iUserID As Integer
            Dim g_sCallingAppName As String = ""
            Dim g_iSourceID, g_iLanguageID, g_iCurrencyID, g_iLogLevel As Integer

            Dim dtEffectiveDate As Date

            g_sUserName.Value = "sirius"
            g_sPassword.Value = "sirius"
            g_iUserID = 1
            g_sCallingAppName = ACApp
            g_iSourceID = 1
            g_iLanguageID = 1
            g_iCurrencyID = 26
            g_iLogLevel = 6

            m_oBatch = New bSIRParty.Business()

            m_lReturn = CType(m_oBatch, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_sUserName.Value, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=ACApp)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oBatch.Dispose()
                m_oBatch = Nothing

                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBatch.Initialise failed.", vApp:=ACApp, vClass:="mainModule", vMethod:="Main")
                Exit Sub
            End If

            dtEffectiveDate = DateTime.Now

            m_lReturn = m_oBatch.CommitFutureDatedAddress(v_dtEffectiveDate:=CInt(dtEffectiveDate.ToOADate))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oBatch.Dispose()
                m_oBatch = Nothing

                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBatch.Start failed.", vApp:=ACApp, vClass:="mainModule", vMethod:="Main")
                Exit Sub
            End If

            m_oBatch.Dispose()
            m_oBatch = Nothing

        Catch excep As System.Exception


            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Sub Main failed.", vApp:=ACApp, vClass:="mainModule", vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (m_oBatch Is Nothing) Then
		m_oBatch.Dispose()
                m_oBatch = Nothing
            End If

            Exit Sub

        End Try

    End Sub
End Module
