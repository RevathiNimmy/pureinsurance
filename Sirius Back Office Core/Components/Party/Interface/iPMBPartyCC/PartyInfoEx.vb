Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class ExtraPartyInfo 
	'====================================================================
	'   Class/Module: ExtraPartyInfo
	'   Description : Common routines required to process additional Party data
	'                 (User Definable Fields)
	'
	'
	'====================================================================
	'   Maintenance History
	'
	'    23 April 2003    Paul Cunningham    Created.
	'
	'====================================================================
	' RAW 20/08/2003 : CQ1263 : replace CreateObject with g_oObjectManager.GetInstance when creating iPMURisk.interface
	
	
	Private Const ACClass As String = "ExtraPartyInfo"
	
	'#Region " Public Enums "
	Public Enum ePartyScreen
		PCScreen = 0
		CCScreen
		GCScreen
	End Enum
	'#End Region
	
	
	
	'#Region " Public Member Variables "
	
	'#End Region
	
	
	'#Region " Private Member Variables "
	Private m_lStatus As gPMConstants.PMEReturnCode
	'#End Region
	
	
	'#Region " Public Properties "
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	'#End Region
	
	'#Region " Public Methods "
	
	Public Function Start(ByVal v_ePartyScreen As ePartyScreen, ByVal v_iTask As Integer, ByVal v_lPartyCnt As Integer) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Start
		' PURPOSE: Process the additional party data request
		' AUTHOR: Paul Cunningham
		' DATE: 23 April 2003, 11:30:39
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Const ACMethod As String = "Start"
		
		Dim bAvailable As Boolean 'Indicates whether the Additional Party Info functionality (product option) is set to On or Off
		Dim sScreenCode As String = "" 'The GIS_Screen.code value from the db
		
		Dim bScreenExists As Boolean 'Whether the screen exists (there is a record on the db for the screen code)
		Dim lScreenId As Integer 'The PK of the screen (ie GIS_Screen.gis_screen_id)
		Dim bScreenDeleted As Boolean 'Indicates whether the requested screen is deleted (ie GIS_Screen.is_deleted = 0)
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMFalse
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		'Test the Product Option to determine if the additional info functionality
		'is available
		If GetAdditionalInfoAvailable(r_bAvailable:=bAvailable) <> gPMConstants.PMEReturnCode.PMTrue Then
			
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to test if additional party functionality is available")
		End If
		
		If bAvailable Then
			'Ensure that the screen required is valid
			Select Case v_ePartyScreen
				Case ePartyScreen.PCScreen
					sScreenCode = "PCScreen"
				Case ePartyScreen.GCScreen
					sScreenCode = "GCScreen"
				Case ePartyScreen.CCScreen
					sScreenCode = "CCScreen"
				Case Else
					Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unknown party screen requested: " & v_ePartyScreen)
			End Select
			
			'Get the details of the screen from the db based on the screen code
			
			Select Case GetGISScreenByCode(v_sScreenCode:=sScreenCode, r_lScreenId:=lScreenId, r_bIsDeleted:=bScreenDeleted)
				Case gPMConstants.PMEReturnCode.PMTrue
					'Ensure that the screen is active (ie GIS_Screen.is_deleted = 0)
					If Not bScreenDeleted Then
						If ShowAdditionalScreen(v_lScreenId:=lScreenId, v_iTask:=v_iTask, v_lPartyCnt:=v_lPartyCnt, r_lStatus:=m_lStatus) <> gPMConstants.PMEReturnCode.PMTrue Then
							
							Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to show additional party info screen")
						End If
						
					End If
				Case gPMConstants.PMEReturnCode.PMNotFound
					'Can't find this ScreenCode so NAR
				Case Else
					Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to test if additional party functionality is available")
			End Select
		End If
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Return result
		
		'------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'------------------------------------------------------------------------

		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Constants.vbObjectError
				' Log internal failure.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
				Return result
				
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
				Return result
				
		End Select
		
		Finally
	
		End Try
		Return result
	End Function
	'#End Region
	
	
	'#Region " Private Properties "
	
	'#End Region
	
	'#Region " Private Routines "
	Private Function GetAdditionalInfoAvailable(ByRef r_bAvailable As Boolean) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetAdditionalInfoAvailable
		' PURPOSE: Determine whether the product option that allows us to use the
		'          Additional Info has been set
		' AUTHOR: Paul Cunningham
		' DATE: 23 April 2003, 14:39:18
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Const ACMethod As String = "GetAdditionalInfoAvailable"
        'developer guide no. 17
        Dim vValue As Object
		
		
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUserDefinablePartyFields, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vValue)) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to get product option: " & gPMConstants.SIRHiddenOptions.SIROPTUserDefinablePartyFields)
            End If

            'Available if value of option = 1
            r_bAvailable = (vValue = 1)

            result = gPMConstants.PMEReturnCode.PMTrue
        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select
        Finally

        End Try
        Return result

	End Function
	
	Private Function GetGISScreenByCode(ByVal v_sScreenCode As String, ByRef r_lScreenId As Integer, ByRef r_bIsDeleted As Boolean) As Integer
		Dim result As Integer = 0
		Dim bSIRParty As Object
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetGISScreenByCode
		' PURPOSE: Get the details of the screen from the db based on the screen code
		' AUTHOR: Paul Cunningham
		' DATE: 23 April 2003, 14:39:07
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Const ACMethod As String = "GetGISScreenByCode"
		

		Dim oSIRPartyBusiness As bSIRParty.Business
		
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
        Dim temp_oSIRPartyBusiness As Object
        Try


            If MainModule.g_oObjectManager.GetInstance(temp_oSIRPartyBusiness, "bSIRParty.Business", "ClientManager") <> gPMConstants.PMEReturnCode.PMTrue Then
                oSIRPartyBusiness = temp_oSIRPartyBusiness

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", g_oObjectManager failed to create business object: bSIRParty.Business")
            Else
                oSIRPartyBusiness = temp_oSIRPartyBusiness
            End If


            'NIIT - Replaced with the Migrated code 1144 
            Select Case ReflectionHelper.Invoke(oSIRPartyBusiness, "GetGISScreenByCode", New Object() {v_sScreenCode, r_lScreenId, r_bIsDeleted})
                'Select Case oSIRPartyBusiness.GetGISScreenByCode(v_sScreenCode:=v_sScreenCode, r_lScreenId:=r_lScreenId, r_bIsDeleted:=r_bIsDeleted)
                Case gPMConstants.PMEReturnCode.PMTrue
                    result = gPMConstants.PMEReturnCode.PMTrue

                Case gPMConstants.PMEReturnCode.PMNotFound
                    result = gPMConstants.PMEReturnCode.PMNotFound

                Case Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get additional party screen details")
            End Select

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select
        Finally
            If Not (oSIRPartyBusiness Is Nothing) Then

		oSIRPartyBusiness.Dispose()
                oSIRPartyBusiness = Nothing
            End If

        End Try

        Return result

    End Function
	
	Private Function ShowAdditionalScreen(ByVal v_lScreenId As Integer, ByVal v_iTask As Integer, ByVal v_lPartyCnt As Integer, ByRef r_lStatus As Integer) As Integer
		Dim result As Integer = 0
		Dim iPMURisk As Object
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: ShowAdditionalScreen
		' PURPOSE: Show the additional info screen
		' AUTHOR: Paul Cunningham
		' DATE: 23 April 2003, 14:41:44
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Const ACMethod As String = "ShowAdditionalScreen"
		

		Dim oPMURisk As iPMURisk.Interface_Renamed
		
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		' RAW 20/08/2003 : CQ1263 : replace CreateObject with g_oObjectManager.GetInstance
		'Set oPMURisk = CreateObject("iPMURisk.Interface")
        Dim temp_oPMURisk As Object
        Try
            If MainModule.g_oObjectManager.GetInstance(temp_oPMURisk, "iPMURisk.Interface_Renamed", gPMConstants.PMGetLocalInterface) <> gPMConstants.PMEReturnCode.PMTrue Then
                oPMURisk = temp_oPMURisk
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", failed to create iPMURisk.Interface")
            Else
                oPMURisk = temp_oPMURisk
            End If
            ' RAW 20/08/2003 : CQ1263 : end

            With oPMURisk
                'Initialise the interface

                If .Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to initialise iPMURisk.Interface")
                End If


                If .SetProcessModes(v_iTask, 0, 0, "", DateTime.Now) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to initialise iPMURisk.Interface")
                End If


                .ScreenId = v_lScreenId

                .PartyCnt = v_lPartyCnt
                '.ShortName = ""

                'Defaults

                .ProductId = 0

                .RiskTypeId = 0

                .InsuranceFolderCnt = 0

                .InsuranceFileCnt = 0

                .RiskId = 0

                'Start the Risk processing (this will show our additional party info screen)

                .Start()

                'Return the response from the additional info screen

                r_lStatus = .Status
            End With

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally
            'Free references
            If Not (oPMURisk Is Nothing) Then

		oPMURisk.Dispose()
                oPMURisk = Nothing
            End If
        End Try

        Return result

    End Function
	
	
	'#End Region
	
	
	'#Region " Private Class Functions "
	
	'#End Region
End Class
