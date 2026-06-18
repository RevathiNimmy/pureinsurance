Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "PMSiriusSupport"
	Private Const ACClass As String = "MainModule"
	
	Public Const ACPMHomePage As String = "www.pmgroup.co.uk/gemini"
	


    Public Sub Main()
        Dim oPMSiriusSupport As iPMSiriusSupport.PMSiriusSupport
        Dim oServerRegistry As bPMServerRegistry.PMServerRegistry
        Dim oForm As frmPMSiriusSupport
        Dim lReturn As Integer
        Dim sPMSupportWebAddress As String = ""


        Try

            ' Display waht's happening
            oForm = New frmPMSiriusSupport()
            oForm.Show()

            ' Get the web address from the server registry
            oServerRegistry = New bPMServerRegistry.PMServerRegistry()

            'developer guide no. 9
            lReturn = oServerRegistry.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'Terminate the form
                oForm.Close()
                oForm = Nothing

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot read Server registry settings ", vApp:=ACApp, vClass:=ACClass, vMethod:="Main")

                Exit Sub

            End If

            lReturn = oServerRegistry.GetServerRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACWrkManRegSupportWebAddress, r_sSettingValue:=sPMSupportWebAddress, v_sSubKey:=gPMConstants.ACWrkManRegSubKey)

            oServerRegistry = Nothing

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sPMSupportWebAddress = "" Then

                lReturn = MessageBox.Show("PM Sirius Support Web Address not found." & Strings.Chr(13) & Strings.Chr(10) & _
                          "Linking to Policy Master Gemini home page, " & ACPMHomePage, Application.ProductName, MessageBoxButtons.OKCancel)

                If lReturn = System.Windows.Forms.DialogResult.Cancel Then
                    'Terminate the form and exit
                    oForm.Close()
                    oForm = Nothing

                    Exit Sub

                End If

                sPMSupportWebAddress = ACPMHomePage

            End If

            ' Launch Internet Explorer
            oPMSiriusSupport = New iPMSiriusSupport.PMSiriusSupport()

            'developer guide no. 9
            oPMSiriusSupport.Initialise()


            oPMSiriusSupport.PMSiriusSupportURL = sPMSupportWebAddress

            oPMSiriusSupport.PMSiriusSupport()

            'Terminate the IE launcher object
            oPMSiriusSupport.Dispose()
            oPMSiriusSupport = Nothing

            'Terminate the form
            oForm.Close()
            oForm = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Main Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub
End Module
