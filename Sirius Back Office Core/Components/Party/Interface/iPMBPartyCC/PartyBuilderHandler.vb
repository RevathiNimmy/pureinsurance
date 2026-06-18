Option Strict Off
Option Explicit On
'Modified by Archana Tokas on 4/30/2010 12:42:20 PM refer developer guide no. 129
Imports SharedFiles
NotInheritable Class PartyBuilderHandler 'deepak changed module to class
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
    Sub New() 'deepak changes: added the constructor logic
        ' Create an instance of the object manager.
        g_oObjectManager = New bObjectManager.ObjectManager

        ' Call the initialise method.
        g_oObjectManager.Initialise(sCallingAppName:=ACApp)
    End Sub

    Private g_oObjectManager As bObjectManager.ObjectManager 'deepak changes

    Private Const ACClass As String = "PartyBuilderHandler"

    Public Function OpenPartyBuilderScreen(ByVal iTask As Short, ByVal lPartyCnt As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: OpenPartyBuilderScreen
        ' PURPOSE: Process the additional party data request
        ' AUTHOR: Paul Cunningham
        ' DATE: 23 April 2003, 11:30:39
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "OpenPartyBuilderScreen"

        Dim lReturn As Integer
        Dim lScreenId As Integer 'GIS_Screen.gis_screen_id
        Dim oBusiness As Object
        Dim bShowCustomScreen As Boolean ' PN23607
        Dim sUnderwriting As String
        Dim m_lReturn As Integer
        Dim r_lPreviousDataModelId As Integer
        Dim r_lGISPolicyLinkID As Integer

        Try
            'Modified by Archana Tokas on 4/22/2010 5:41:31 PM changes to be checked at run time
            'If (g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business", vInstanceManager_optional:=PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue) Then
            If (g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business") <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(ACMethod, "failed to create bSIRParty.Business", vbObjectError)
            End If

            'Get the details of the screen from the db based on the screen code

            Select Case oBusiness.GetGISScreenForParty(lPartyCnt:=lPartyCnt, r_lGISScreenID:=lScreenId)

                Case gPMConstants.PMEReturnCode.PMTrue
                    ' If adding a new client, confirm with the user if they want to
                    ' add custom info. PN23607.

                    ' add custom info. PN23607.
                    m_lReturn = iPMFunc.getUnderwritingOrAgency(sUnderwriting)

                    If sUnderwriting = "U" Then
                        If GetPreviousPartyBuilderDataModel(lPartyCnt:=lPartyCnt, r_lPreviousDataModelId:=r_lPreviousDataModelId, r_lGISPolicyLinkID:=r_lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then

                            RaiseError(ACMethod, "Unable to get previous screen data model info.", vbObjectError)
                        End If

                        If r_lPreviousDataModelId > 0 Then
                            If MsgBox("Warning: the data model screen has been changed for this type of Party," & vbCrLf & "continuing will reset the custom data.", MsgBoxStyle.Critical + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2, "Custom Data") = MsgBoxResult.Cancel Then

                                bShowCustomScreen = False
                            Else
                                'Delete all Party Builder GIS data for the policy link
                                If DeleteCustomData(lGISPolicyLinkID:=r_lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bShowCustomScreen = False
                                Else
                                    bShowCustomScreen = True
                                End If
                            End If
                        Else
                            bShowCustomScreen = True
                        End If
                    Else
                        If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                            If MsgBox("Do you wish to add custom data for this Client?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Custom Data") = MsgBoxResult.No Then
                                lReturn = gPMConstants.PMEReturnCode.PMTrue
                            Else
                                bShowCustomScreen = True
                            End If
                        Else
                            bShowCustomScreen = True
                        End If
                    End If

                    If bShowCustomScreen Then
                        'Ensure that the screen is active (ie GIS_Screen.is_deleted = 0)
                        If ShowAdditionalScreen(lGISScreenID:=lScreenId, iTask:=iTask, lPartyCnt:=lPartyCnt, r_lStatus:=lReturn) <> gPMConstants.PMEReturnCode.PMTrue Then

                            RaiseError(ACMethod, "Unable to show additional party info screen", vbObjectError)
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
                    MsgBox("An incorrect Screen is assigned to this type of Party." & vbCrLf & vbCrLf & "Please correct the setting for the Party Type in Lookup Maintenance or" & vbCrLf & "contact your System Administrator.", MsgBoxStyle.Critical, "Incorrect Screen assigned to Party Type")
                    lReturn = gPMConstants.PMEReturnCode.PMFalse
                Case gPMConstants.PMEReturnCode.PMCancel
                    'Nothing defined so move on silently
                    lReturn = gPMConstants.PMEReturnCode.PMTrue
                Case gPMConstants.PMEReturnCode.PMFalse
                    RaiseError(ACMethod, "Error opening Party Builder Screen for PartyCnt " & lPartyCnt, vbObjectError)
            End Select

            OpenPartyBuilderScreen = lReturn


            '------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Err.Number
                Case vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Err.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Err.Source, excep:=ex)

                    OpenPartyBuilderScreen = gPMConstants.PMEReturnCode.PMFalse


                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

                    OpenPartyBuilderScreen = gPMConstants.PMEReturnCode.PMError



            End Select

        Finally
            If Not oBusiness Is Nothing Then

                oBusiness.Dispose()

                oBusiness = Nothing
            End If

        End Try

    End Function

    'AR20050824 - PN24332
    'Retrieve whether the party type is associated with a Party Builder screen and
    'whether the party is linked to a GIS policy binder
    Public Function GetPartyBuilderFlags(ByVal lPartyCnt As Integer, ByRef r_bHasAssociatedModel As Boolean, ByRef r_bHasData As Boolean) As Integer

        Const ACMethod As String = "GetPartyBuilderFlags"

        Dim lReturn As Integer
        Dim lScreenId As Integer
        Dim lPolicyLinkID As Integer
        Dim oBusiness As Object

        Try

            'Modified by Archana Tokas on 4/22/2010 5:41:31 PM changes to be checked at run time
            'If (g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business", vInstanceManager_optional:=PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue) Then
            If (g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business") <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(ACMethod, "Failed to create bSIRParty.Business", vbObjectError)
            End If

            'Get the details of the screen from the db based on the screen code

            lReturn = oBusiness.GetGISScreenForParty(lPartyCnt:=lPartyCnt, r_lGISScreenID:=lScreenId)

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                r_bHasAssociatedModel = True


                lReturn = oBusiness.GetGISPolicyLinkForParty(lPartyCnt:=lPartyCnt, r_lGISPolicyLinkID:=lPolicyLinkID)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                    lReturn = oBusiness.GetGISCustomDataForParty(lPartyCnt:=lPartyCnt, lGISScreenID:=lScreenId, lGISPolicyLinkID:=lPolicyLinkID)
                    If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        r_bHasData = True
                    Else
                        r_bHasData = False
                    End If

                Else
                    r_bHasData = False
                End If
            End If


            oBusiness.Dispose()

            oBusiness = Nothing

            GetPartyBuilderFlags = gPMConstants.PMEReturnCode.PMTrue

            Exit Function

        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            GetPartyBuilderFlags = gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    Private Function ShowAdditionalScreen(ByVal lGISScreenID As Integer, ByVal iTask As Short, ByVal lPartyCnt As Integer, ByRef r_lStatus As Integer) As Integer
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

            ShowAdditionalScreen = gPMConstants.PMEReturnCode.PMFalse

            'Modified by Archana Tokas on 4/22/2010 5:41:31 PM changes to be checked at run time
            'If (g_oObjectManager.GetInstance(oObject:=oPMURisk, sClassName:="iPMURisk.Interface", vInstanceManager_optional:=PMGetLocalInterface) <> gPMConstants.PMEReturnCode.PMTrue) Then
            If (g_oObjectManager.GetInstance(oObject:=oPMURisk, sClassName:="iPMURisk.Interface") <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(ACMethod, "failed to create iPMURisk.Interface", vbObjectError)
            End If

            With oPMURisk
                'Initialise the interface

                If .Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(ACMethod, "unable to initialise iPMURisk.Interface", vbObjectError)
                End If


                If .SetProcessModes(iTask, 0, 0, "", Now) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(ACMethod, "unable to initialise iPMURisk.Interface", vbObjectError)
                End If


                .ScreenId = lGISScreenID

                .PartyCnt = lPartyCnt

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

            ShowAdditionalScreen = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            Select Case Err.Number
                Case vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Err.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Err.Source, excep:=ex)

                    ShowAdditionalScreen = gPMConstants.PMEReturnCode.PMFalse


                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

                    ShowAdditionalScreen = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally
            If Not oPMURisk Is Nothing Then

                oPMURisk.Dispose()

                oPMURisk = Nothing
            End If

        End Try
    End Function

    ' ************************************************************************************** '
    ' Name: GetPreviousPartyBuilderDataModel
    ' Description: Returns previous data model Id if screen data model has changed else zero
    '              Returns GIS Policy Link Id if there is any
    ' ************************************************************************************** '
    Public Function GetPreviousPartyBuilderDataModel(ByVal lPartyCnt As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer

        Const kMethodName As String = "GetPreviousPartyBuilderDataModel"


        Dim lReturn As Integer
        Dim oBusiness As Object

        Try


            GetPreviousPartyBuilderDataModel = gPMConstants.PMEReturnCode.PMTrue
            'Modified by Archana Tokas on 4/22/2010 5:41:31 PM changes to be checked at run time
            'lReturn = g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business", vInstanceManager_optional:=PMGetViaClientManager)
            lReturn = g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(CStr(lReturn), "Failed to create bSIRParty.Business")
            End If

            'Get the details of the screen from the db based on the screen code

            lReturn = oBusiness.GetPreviousDataModel(lPartyCnt:=lPartyCnt, r_lPreviousDataModelId:=r_lPreviousDataModelId, r_lGISPolicyLinkID:=r_lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(CStr(lReturn), "GetPreviousDataModel failed.")
            End If

            If r_lPreviousDataModelId > 0 And r_lGISPolicyLinkID <= 0 Then
                RaiseError(CStr(lReturn), "Failed to get GIS Policy Link.")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetPreviousPartyBuilderDataModel, excep:=ex)

        Finally


            oBusiness.Dispose()

            oBusiness = Nothing

        End Try
    End Function

    ' **************************************************************************** '
    ' Name: DeleteCustomData
    ' Description: Deletes all corresponding GIS data for a GIS Policy Link Id
    ' **************************************************************************** '
    Public Function DeleteCustomData(ByVal lGISPolicyLinkID As Integer) As Integer

        Const kMethodName As String = "DeleteCustomData"



        Dim lReturn As Integer
        Dim oBusiness As Object

        Try

            DeleteCustomData = gPMConstants.PMEReturnCode.PMTrue

            'Modified by Archana Tokas on 4/22/2010 5:41:31 PM changes to be checked at run time
            'lReturn = g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business", vInstanceManager_optional:=PMGetViaClientManager)
            lReturn = g_oObjectManager.GetInstance(oObject:=oBusiness, sClassName:="bSIRParty.Business")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(CStr(lReturn), "Failed to create bSIRParty.Business")
            End If

            'Get the details of the screen from the db based on the screen code

            lReturn = oBusiness.DeleteCustomData(lGISPolicyLinkID:=lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(CStr(lReturn), "DeleteCustomData failed.")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DeleteCustomData, excep:=ex)

        Finally


            oBusiness.Dispose()

            oBusiness = Nothing

        End Try
    End Function
End Class
