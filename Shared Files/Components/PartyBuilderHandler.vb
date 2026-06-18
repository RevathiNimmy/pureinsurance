Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("PartyBuilderHandler_NET.PartyBuilderHandler")> _
 Public Module PartyBuilderHandler
	' ***************************************************************** '
	' Class Name:   Party Builder Handler
	'
	' Date:         19/02/2005
	'
	' Description:  Generic code to display the Party Builder screen
	'               for a particular PartyCnt.
	'
	'               THIS MODULE IS ONLY FOR AN INTERFACE COMPONENT
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "PartyBuilderHandler"
    'Added by Deepak Sharma on 4/20/2010 6:46:23 PM refer developer guide no. 

    <ThreadStatic()> _
    Public g_oObjectManager As Object


    Public Function OpenPartyBuilderScreen(ByVal iTask As Integer, ByVal lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRParty As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: OpenPartyBuilderScreen
        ' PURPOSE: Process the additional party data request
        ' AUTHOR: Paul Cunningham
        ' DATE: 23 April 2003, 11:30:39
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "OpenPartyBuilderScreen"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lScreenId As Integer 'GIS_Screen.gis_screen_id

        Dim oBusiness As Object
        Dim bShowCustomScreen As Boolean ' PN23607
        Dim sUnderwriting As String = ""
        Dim m_lReturn, r_lPreviousDataModelId, r_lGISPolicyLinkID As Integer


        Try

        Dim temp_oBusiness As Object = Nothing
        If g_oObjectManager.GetInstance(temp_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
            oBusiness = temp_oBusiness
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", failed to create bSIRParty.Business")
        Else
            oBusiness = temp_oBusiness
        End If

        'Get the details of the screen from the db based on the screen code


        Select Case oBusiness.GetGISScreenForParty(lPartyCnt:=lPartyCnt, r_lGISScreenID:=lScreenId)
            Case gPMConstants.PMEReturnCode.PMTrue
                If GetPreviousPartyBuilderDataModel(lPartyCnt:=lPartyCnt, r_lPreviousDataModelId:=r_lPreviousDataModelId, r_lGISPolicyLinkID:=r_lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get previous screen data model info.")
                End If

                If r_lPreviousDataModelId > 0 Then
                    If MessageBox.Show("Warning: the data model screen has been changed for this type of Party," & _
                                       Strings.Chr(13) & Strings.Chr(10) & "continuing will reset the custom data.", "Custom Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Cancel Then

                        bShowCustomScreen = False
                    Else
                        'Delete all Party Builder GIS data for the policy link
                        bShowCustomScreen = Not (DeleteCustomData(lGISPolicyLinkID:=r_lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue)
                    End If
                Else
                    bShowCustomScreen = True
                End If


                If bShowCustomScreen Then
                    'Ensure that the screen is active (ie GIS_Screen.is_deleted = 0)
                    If ShowAdditionalScreen(lGISScreenID:=lScreenId, iTask:=iTask, lPartyCnt:=lPartyCnt, r_lStatus:=lReturn) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to show additional party info screen")
                    End If

                    'DC260106 PN27053 specifically check for Cancel
                    If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        lReturn = gPMConstants.PMEReturnCode.PMCancel
                    Else
                        lReturn = gPMConstants.PMEReturnCode.PMTrue 'PMFalse
                    End If

                End If
            Case gPMConstants.PMEReturnCode.PMError
                'Wrong Screen is defined
                MessageBox.Show("An incorrect Screen is assigned to this type of Party." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Please correct the setting for the Party Type in Lookup Maintenance or" & Strings.Chr(13) & Strings.Chr(10) & _
                                "contact your System Administrator.", "Incorrect Screen assigned to Party Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                lReturn = gPMConstants.PMEReturnCode.PMFalse
            Case gPMConstants.PMEReturnCode.PMCancel
                'Nothing defined so move on silently
                lReturn = gPMConstants.PMEReturnCode.PMTrue
            Case gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error opening Party Builder Screen for PartyCnt " & lPartyCnt)
        End Select

        result = lReturn
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
        If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
                oBusiness = Nothing
        End If

       

        End Try
	Return result
    End Function

    'AR20050824 - PN24332
    'Retrieve whether the party type is associated with a Party Builder screen and
    'whether the party is linked to a GIS policy binder
    Public Function GetPartyBuilderFlags(ByVal lPartyCnt As Integer, ByRef r_bHasAssociatedModel As Boolean, ByRef r_bHasData As Boolean) As Integer
        Const ACMethod As String = "GetPartyBuilderFlags"
        Try


            Dim lReturn As gPMConstants.PMEReturnCode
            Dim lScreenId, lPolicyLinkID As Integer

            Dim oBusiness As Object

            Dim temp_oBusiness As Object = Nothing
            If g_oObjectManager.GetInstance(temp_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                oBusiness = temp_oBusiness
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to create bSIRParty.Business")
            Else
                oBusiness = temp_oBusiness
            End If

            'Get the details of the screen from the db based on the screen code

            lReturn = oBusiness.GetGISScreenForParty(lPartyCnt:=lPartyCnt, r_lGISScreenID:=lScreenId)

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                r_bHasAssociatedModel = True


                lReturn = oBusiness.GetGISPolicyLinkForParty(lPartyCnt:=lPartyCnt, r_lGISPolicyLinkID:=lPolicyLinkID)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                    lReturn = oBusiness.GetGISCustomDataForParty(lPartyCnt:=lPartyCnt, lGISScreenID:=lScreenId, lGISPolicyLinkID:=lPolicyLinkID)
                    r_bHasData = lReturn = gPMConstants.PMEReturnCode.PMTrue

                Else
                    r_bHasData = False
                End If
            End If


            oBusiness.Dispose()
            oBusiness = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    Private Function ShowAdditionalScreen(ByVal lGISScreenID As Integer, ByVal iTask As Integer, ByVal lPartyCnt As Integer, ByRef r_lStatus As Integer) As Integer
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

        Dim oPMURisk As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Dim temp_oPMURisk As Object = Nothing
            If g_oObjectManager.GetInstance(temp_oPMURisk, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface) <> gPMConstants.PMEReturnCode.PMTrue Then
                oPMURisk = temp_oPMURisk
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", failed to create iPMURisk.Interface")
            Else
                oPMURisk = temp_oPMURisk
            End If

            With oPMURisk
                'Initialise the interface

                If .Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to initialise iPMURisk.Interface")
                End If


                If .SetProcessModes(iTask, 0, 0, "", DateTime.Now) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to initialise iPMURisk.Interface")
                End If


                .ScreenId = lGISScreenID

                .PartyCnt = lPartyCnt

                'Defaults

                .ProductId = 0

                .RiskTypeId = 0

                .InsuranceFolderCnt = 0

                .InsuranceFileCnt = 0

                .RiskId = 0



                .Start()

                'Return the response from the additional info screen

                r_lStatus = .Status
            End With

            result = gPMConstants.PMEReturnCode.PMTrue

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
            ' Resume

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
            'Free references
            If Not (oPMURisk Is Nothing) Then

                oPMURisk.Dispose()
                oPMURisk = Nothing
            End If

        End Try
        Return result

    End Function
	
	' ************************************************************************************** '
	' Name: GetPreviousPartyBuilderDataModel
	' Description: Returns previous data model Id if screen data model has changed else zero
	'              Returns GIS Policy Link Id if there is any
	' ************************************************************************************** '
	Public Function GetPreviousPartyBuilderDataModel(ByVal lPartyCnt As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer
		Dim result As Integer = 0
		Dim bSIRParty As Object
		
		Const kMethodName As String = "GetPreviousPartyBuilderDataModel"
		
        Dim oBusiness As Object

        Try

            Dim lReturn As Integer




            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oBusiness As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "Failed to create bSIRParty.Business")
            End If

            'Get the details of the screen from the db based on the screen code

            lReturn = oBusiness.GetPreviousDataModel(lPartyCnt:=lPartyCnt, r_lPreviousDataModelId:=r_lPreviousDataModelId, r_lGISPolicyLinkID:=r_lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "GetPreviousDataModel failed.")
            End If

            If r_lPreviousDataModelId > 0 And r_lGISPolicyLinkID <= 0 Then
                gPMFunctions.RaiseError(CStr(lReturn), "Failed to get GIS Policy Link.")
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


		oBusiness.Dispose()


        End Try
        Return result
    End Function

    ' **************************************************************************** '
    ' Name: DeleteCustomData
    ' Description: Deletes all corresponding GIS data for a GIS Policy Link Id
    ' **************************************************************************** '
    Public Function DeleteCustomData(ByVal lGISPolicyLinkID As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRParty As Object

        Const kMethodName As String = "DeleteCustomData"

        Dim oBusiness As Object

        Try

            Dim lReturn As Integer





            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oBusiness As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "Failed to create bSIRParty.Business")
            End If

            'Get the details of the screen from the db based on the screen code

            lReturn = oBusiness.DeleteCustomData(lGISPolicyLinkID:=lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "DeleteCustomData failed.")
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


		oBusiness.Dispose()
        End Try
        Return result
    End Function
End Module