Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("bGEMFunc_NET.bGEMFunc")> _
 Public Module bGEMFunc
	' ***************************************************************** '
	'
	' Gemini Business general functions module. Contains all of the global
	' functions that might be useful when writing the business layer.
	'
	' ***************************************************************** '
	
	Private Const ACClass As String = "bGemFunc"
	
	Private m_lReturn As Integer
	
	' ***************************************************************** '
	' Name: GetGeminiDatabase (Public)
	'
	' Description: Opens the Gemini database
	' This function should be called by Gemini business objects
	' to either instance a new PMDAO or return one that has been instanced earlier
	'
	' Needs to be able to identify which database vDatabase is
	' New property of PMDAO required - see code .DSN=
	'
	' ***************************************************************** '
    Public Function GetGeminiDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByRef lOpenStatus As Integer, ByRef bCloseDatabase As Boolean, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Object

        Dim result As Object = Nothing
        Dim oDatabase As Object
        Dim bInstanceNew As Boolean

        Try

            ' Create a new Gemini DB Instance
            bInstanceNew = True

            ' Was a valid Database Object Reference passed?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                bInstanceNew = False
            End If

            If Not bInstanceNew Then

                oDatabase = vDatabase
                lOpenStatus = gPMConstants.PMEReturnCode.PMTrue
                ' Do NOT Close Database in Terminate() method
                ' let the instancer do it instead
                bCloseDatabase = False
            Else

                ' NO, Create new instance of the database object
                oDatabase = New dPMDAO.Database

                ' Open the Database
                'lOpenStatus = oDatabase.OpenDatabase(vDSN:=PMGeminiDSN)
                'AK 100802 - scalability
                lOpenStatus = oDatabase.OpenDatabase(vDSN:=gPMConstants.PMGeminiDSN, sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp)

                If lOpenStatus <> gPMConstants.PMEReturnCode.PMTrue Then
                    oDatabase = Nothing
                End If

                ' Close Database in Terminate() method
                bCloseDatabase = True
            End If


            Return oDatabase

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGeminiDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGeminiDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGISDatabase (Public)
    '
    ' Description: Opens the GIS database
    ' This function should be called by GIS business objects
    ' to either instance a new PMDAO or return one that has been instanced earlier
    '
    ' Needs to be able to identify which database vDatabase is
    ' New property of PMDAO required - see code .DSN=
    '
    ' ***************************************************************** '
    Public Function GetGISDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByRef lOpenStatus As Integer, ByRef bCloseDatabase As Boolean, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Object

        Dim result As Object = Nothing
        Dim oDatabase As Object
        Dim bInstanceNew As Boolean

        Try

            ' Create a new GIS DB Instance
            bInstanceNew = True

            ' Was a valid Database Object Reference passed?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                bInstanceNew = False
            End If

            If Not bInstanceNew Then

                oDatabase = vDatabase
                lOpenStatus = gPMConstants.PMEReturnCode.PMTrue
                ' Do NOT Close Database in Terminate() method
                ' let the instancer do it instead
                bCloseDatabase = False
            Else

                ' NO, Create new instance of the database object
                oDatabase = New dPMDAO.Database

                ' Open the Database
                lOpenStatus = oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp, vDSN:="GIS")

                If lOpenStatus <> gPMConstants.PMEReturnCode.PMTrue Then
                    oDatabase = Nothing
                End If

                ' Close Database in Terminate() method
                bCloseDatabase = True
            End If


            Return oDatabase

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetSiriusArchitectureDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByRef lOpenStatus As Integer, ByRef bCloseDatabase As Boolean, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Object

        Dim result As Object = Nothing
        Dim oDatabase As Object
        Dim bInstanceNew As Boolean

        Try

            ' Create a new Gemini DB Instance
            bInstanceNew = True

            ' Was a valid Database Object Reference passed?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                bInstanceNew = False
            End If

            If Not bInstanceNew Then

                oDatabase = vDatabase
                lOpenStatus = gPMConstants.PMEReturnCode.PMTrue
                ' Do NOT Close Database in Terminate() method
                ' let the instancer do it instead
                bCloseDatabase = False
            Else

                ' NO, Create new instance of the database object
                oDatabase = New dPMDAO.Database

                ' Open the Database
                '        lOpenStatus = oDatabase.OpenDatabase(vDSN:=PMSiriusArchitectureDSN)
                'AK 100802 - scalability
                lOpenStatus = oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp, vDSN:=gPMConstants.PMSiriusArchitectureDSN)

                If lOpenStatus <> gPMConstants.PMEReturnCode.PMTrue Then
                    oDatabase = Nothing
                End If

                ' Close Database in Terminate() method
                bCloseDatabase = True
            End If


            Return oDatabase

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSiriusArchitectureDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSiriusArchitectureDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	' ***************************************************************** '
	' Name: GetGeminiBusiness (Public)
	'
	' Description: Get a refernece to and initialise an object class
	'
	' ***************************************************************** '
	
	Public Function GetGeminiBusiness(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sClassName As String, Optional ByVal v_vDatabase As Object = Nothing, Optional ByVal v_vBeginTrans As Object = Nothing) As Object
		Dim result As Object = Nothing
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim oNewObject As Object
		
		Try 
			
			oNewObject = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).FullName, v_sClassName).Unwrap()
			If Not (oNewObject Is Nothing) Then

				lReturn = oNewObject.Initialise(sUserName:=v_sUsername, sPassword:=v_sPassword, iUserID:=v_iUserID, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, iCurrencyID:=v_iCurrencyID, iLogLevel:=v_iLogLevel, sCallingAppName:=ACApp, vDatabase:=v_vDatabase)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					oNewObject = Nothing
				End If
			End If
			
			
			Return oNewObject
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGeminiBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGeminiBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
    'Modified by Deepak Sharma on 4/20/2010 4:47:34 PM refer developer guide no. 29(No Solutions)
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()

End Module