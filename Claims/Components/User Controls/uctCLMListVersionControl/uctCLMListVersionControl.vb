Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMVersions_NET.uctCLMVersions")> _
Partial Public Class uctCLMVersions
    Inherits System.Windows.Forms.UserControl

    Private Const ACClass As String = "uctCLMListVersions"
    ' objects
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oBusiness As bCLMFindClaim.Business

    ' generic interface details
    Private m_iTask As Integer
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sTransactionType As String = ""
    Private m_sUsername As String = ""

    ' custom interface details
    Private m_lClaimId As Integer
    Private m_sShortName As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_sClaimNumber As String = ""

    ' selected items details
    Private m_lSelectedClaimID As Integer
    Private m_lSelectedInsuranceFileCnt As Integer
    Private m_sSelectedClaimNumber As String = ""
    Private m_sSelectedInsuranceRef As String = ""
    Private m_lSelectedRiskCnt As Integer
    Private m_sSelectedClientShortName As String = ""
    Private m_dtSelectedLossFromDate As Date
    Private m_sSelectedInsuranceHolderShortname As String = ""
    Private m_lSelectedInsuranceFolderCnt As Integer

    ' interface flags
    Private m_bUserControlIsResizing As Boolean

    ' internal data arrays
    Private m_vClaimDetails(,) As Object
    Private m_vClaimVersionDetails As Object

    ' internal collection - holds retrieved claim version details
    Private m_colClaimVersionDetails As Collection

    Public Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)

    ''PLICO RFC-9 - Amit
    Private m_lPartyCnt As Integer
    Private m_vOtherClaimDetails As Object


    Private m_oBusinessCase As bCLMCase.Business

    Private msX As Integer
    Private msY As Integer
    Private m_sSelectedTransTypeCode As String = ""

    <Browsable(False)> _
    Public WriteOnly Property ClaimNumber() As String
        Set(ByVal Value As String)
            m_sClaimNumber = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ShortName() As String
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property InsuranceRef() As String
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property SelectedClaimId() As Integer
        Get
            Return m_lSelectedClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lSelectedClaimID = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Static bIsInitialised As Boolean

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            ' initialise collection
            m_colClaimVersionDetails = New Collection()

            ' Create an instance of the object manager.
            m_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oObjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If m_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With m_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMFindClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRPartyFee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get an instance of the business object of Case
            Dim temp_m_oBusinessCase As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oBusinessCase, "bCLMCase.Business", vInstanceManager:="ClientManager")
            m_oBusinessCase = temp_m_oBusinessCase

            ' Check for errors.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bCLMCase", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' hold Initialised status
            bIsInitialised = True


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' setup user control
            lReturn = SetUpUserControl()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


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


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ResizeControl
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-03-2006 : Claima Versioning Changes
    ' ***************************************************************** '
    Private Function ResizeControl() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ResizeControl"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bUserControlIsResizing Then

                m_bUserControlIsResizing = True

                fraMain.Height = MyBase.Height - VB6.TwipsToPixelsY(120)
                fraMain.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
                fraMain.Left = VB6.TwipsToPixelsX(60)
                fraMain.Top = VB6.TwipsToPixelsY(60)

                tvwClaims.Top = VB6.TwipsToPixelsY(330)
                tvwClaims.Width = fraMain.Width / 4
                tvwClaims.Height = fraMain.Height - VB6.TwipsToPixelsY(330) - VB6.TwipsToPixelsY(120)

                fraClaimInformation.Top = VB6.TwipsToPixelsY(240)
                fraClaimInformation.Width = ((fraMain.Width / 4) * 3) - VB6.TwipsToPixelsX(360)
                fraClaimInformation.Left = tvwClaims.Left + tvwClaims.Width + VB6.TwipsToPixelsX(120)

                fraClaimVersions.Top = fraClaimInformation.Top + fraClaimInformation.Height + VB6.TwipsToPixelsY(120)
                fraClaimVersions.Left = tvwClaims.Left + tvwClaims.Width + VB6.TwipsToPixelsX(120)
                fraClaimVersions.Width = ((fraMain.Width / 4) * 3) - VB6.TwipsToPixelsX(360)
                fraClaimVersions.Height = fraMain.Height - fraClaimVersions.Top - VB6.TwipsToPixelsY(120)

                lvwClaimVersions.Width = fraClaimVersions.Width - VB6.TwipsToPixelsX(240)
                lvwClaimVersions.Height = fraClaimVersions.Height - VB6.TwipsToPixelsY(360)

                m_bUserControlIsResizing = False

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub cmdViewCase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewCase.Click

        Const ACMethod As String = "cmdViewCase_Click"

        Dim bShowCustomScreen As Boolean
        Dim lPreviousDataModelId, lGISPolicyLinkID As Integer

        Dim sSQL As String = ""
        Dim vSearchData As Object
        Dim lReturn As Integer

        Try

            lReturn = m_oBusinessCase.GenerateSQL(r_sSQL:=sSQL, v_sCaseNumber:=txtCaseNumber.Text)


            ' Get the Case details from the business object.

            lReturn = m_oBusinessCase.FindCase(v_sSQL:=sSQL, r_vResultArray:=vSearchData)

            lReturn = m_oBusinessCase.GetPreviousDataModel(v_lCaseId:=vSearchData(0, 0), r_lPreviousDataModelId:=lPreviousDataModelId, r_lGISPolicyLinkID:=lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "GetPreviousDataModel failed.")
            End If

            If lPreviousDataModelId > 0 And lGISPolicyLinkID <= 0 Then
                gPMFunctions.RaiseError(CStr(lReturn), "Failed to get GIS Policy Link.")
            End If

            If lPreviousDataModelId > 0 Then
                If MessageBox.Show("Warning: the data model screen has been changed for Case," & _
                                   Strings.Chr(13) & Strings.Chr(10) & "continuing will reset the custom data.", "Custom Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Cancel Then

                    bShowCustomScreen = False
                Else

                    'Delete all Party Builder GIS data for the policy link
                    'Get the details of the screen from the db based on the screen code

                    lReturn = m_oBusinessCase.DeleteCustomData(v_lGISPolicyLinkID:=lGISPolicyLinkID)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(CStr(lReturn), "DeleteCustomData failed.")
                    End If

                    bShowCustomScreen = Not (lReturn <> gPMConstants.PMEReturnCode.PMTrue)

                End If
            Else
                bShowCustomScreen = True
            End If

            If bShowCustomScreen Then


                lReturn = ShowCaseScreen(v_lTask:=gPMConstants.PMEComponentAction.PMView, v_sTransactionType:="C_VC", v_lCaseID:=CInt(vSearchData(0, 0)), v_lBaseCaseID:=CInt(vSearchData(9, 0)))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(CStr(lReturn), "Failed to populate the Case details")
                End If

            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdEditCase_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub lvwClaimVersions_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwClaimVersions.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub lvwClaimVersions_ItemClick(ByVal Item As ListViewItem)
        SetSelectedItemsDetails(Item)
    End Sub
    Private Sub tvwClaims_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwClaims.DoubleClick

        Dim HitNode As TreeNode
        Dim oCMManager As Object
        Dim lReturn As Integer

        Try

            HitNode = tvwClaims.GetNodeAt(msX, msY)

            If Not (HitNode Is Nothing) Then
                If HitNode.Name.Trim().Length <> 0 And Convert.ToString(HitNode.Tag) <> 0 Then
                    If Information.IsArray(m_vClaimDetails) Then
                        If m_vClaimDetails.GetUpperBound(0) = kClaimDetailsIsOtherClaim Then
                            For iLoop As Integer = m_vClaimDetails.GetLowerBound(1) To m_vClaimDetails.GetUpperBound(1)
                                If gPMFunctions.ToSafeInteger(m_vClaimDetails(kClaimDetailsClaimId, iLoop)) = Convert.ToString(HitNode.Tag) And CDbl(m_vClaimDetails(kClaimDetailsIsOtherClaim, iLoop)) = 1 Then

                                    ' Get an instance of the wrapper
                                    Dim temp_oCMManager As Object
                                    lReturn = g_oObjectManager.GetInstance(temp_oCMManager, sClassName:="iPMBClientManagerWrapper.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                                    oCMManager = temp_oCMManager
                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ' Log Error Message
                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBClientManagerWrapper.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwClaims_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                        Exit Sub
                                    End If

                                    oCMManager.SkipFindParty = True

                                    oCMManager.ShortName = CStr(m_vClaimVersionDetails(kClaimVersionDetailsClientShortName, 0)).Trim()
                                    ' Start it

                                    lReturn = oCMManager.Start()
                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        ' Log Error Message
                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start oCMManager", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwClaims_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                        oCMManager = Nothing
                                        Exit Sub
                                    End If
                                    ' Terminate it

                                    oCMManager.Dispose()

                                    oCMManager = Nothing


                                    Exit Sub
                                End If
                            Next
                        End If
                    End If
                End If
            End If

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="tvwClaims_DblClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwClaims_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub tvwClaims_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwClaims.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        msX = CInt(x)
        msY = CInt(y)
    End Sub

    Private Sub tvwClaims_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwClaims.AfterSelect
        Dim Node As TreeNode = eventArgs.Node
        If Convert.ToString(Node.Tag) <> 0 Then
            ProcessNodeClick(Convert.ToString(Node.Tag), Node.Text)
        End If
    End Sub

    Private Sub uctCLMVersions_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        ResizeControl()
    End Sub

    ' ***************************************************************** '
    ' Name: SetUpUserControl
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Private Function SetUpUserControl() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpUserControl"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get and populate the claim details for the search criteria
            lReturn = LoadClaimDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if a claim id has been passed
            If m_lClaimId <> 0 Then

                ' get claim version details
                lReturn = CType(GetClaimVersions(m_lClaimId, m_sClaimNumber), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClaimVersions Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate claim version details
                lReturn = PopulateClaimVersionDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateClaimVersionDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                SetSelectedRow()
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimVersions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-03-2006 : Claims Versioning
    ' ***************************************************************** '
    Private Function GetClaimVersions(ByVal v_lClaimID As Integer, ByVal v_sClaimNumber As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimVersions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bItemExistsInCollection As Boolean
        Dim oClaimDetails As cClaimDetails



        result = gPMConstants.PMEReturnCode.PMTrue

        bItemExistsInCollection = True

        ' reset claim version details array

        m_vClaimVersionDetails = Nothing

        ' attempt to retrieve the
        oClaimDetails = m_colClaimVersionDetails.Item(v_sClaimNumber)

        If Not Information.IsArray(oClaimDetails.ClaimVersionDetails) Then

            ' get the claim version details for the specified claim id

            result = m_oBusiness.GetClaimVersions(v_lClaimId:=v_lClaimID, r_vResults:=m_vClaimVersionDetails)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimVersions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' add the claim version details into the collection

            oClaimDetails.ClaimVersionDetails = m_vClaimVersionDetails

        End If
        m_vClaimVersionDetails = oClaimDetails.ClaimVersionDetails
        oClaimDetails = Nothing
        Return result
       
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetClaimDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim vOtherClaimDetails(,) As Object
        Dim vMergedArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.FindClaim(v_sShortname:=m_sShortName, v_sInsuranceRef:=m_sInsuranceRef, v_lClaimID:=m_lClaimId, r_vResults:=m_vClaimDetails, v_bViaClaimVersionList:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMFindClaim.FindClaim Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_lPartyCnt <> 0 Then

                lReturn = m_oBusiness.FindOtherClaims(v_lPartyCnt:=m_lPartyCnt, r_vOtherClaimDetails:=vOtherClaimDetails)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bCLMFindClaim.FindOtherClaim Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            lReturn = CType(MergeArrays(vArray:=vOtherClaimDetails, vOriginalArray:=m_vClaimDetails, vMergedArray:=vMergedArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_vClaimDetails = vMergedArray


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: PopulateClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PopulateClaimDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClaimDetails"

        Dim lReturn As Integer
        Dim bClaimDetailsFound As Boolean

        Dim oNode, oOpenNode, oInfoNode, oSettledNode, oParentNode, oMainNode As TreeNode

        Dim llBound, lUBound, lClaimId, lClaimStatusId, lInfoOnly As Integer
        Dim sClaimNumber As String = ""
        Dim bInfoOnly As Boolean
        Dim sClaimDescription, sInsuranceRef As String
        Dim lProductId As Integer
        Dim sProductDescription, sNodeKey, sCaseNumber As String

        Dim oClaimDetails As cClaimDetails
        Dim sTempNodeKey As String = String.Empty

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if any claim details have been found for the specified search criteria
            bClaimDetailsFound = Information.IsArray(m_vClaimDetails)

            ' set up the initial tree view nodes
            lReturn = SetupInitialTreeView(bClaimDetailsFound)

            ' if there is claim data
            If bClaimDetailsFound Then
                ' get the relevant parent nodes
                oOpenNode = tvwClaims.Nodes.Item(0).Nodes(0).Nodes(ktvwNodeKeyOPEN)
                oInfoNode = tvwClaims.Nodes.Item(0).Nodes(0).Nodes(ktvwNodeKeyINFOONLY)
                oSettledNode = tvwClaims.Nodes.Item(0).Nodes(0).Nodes(ktvwNodeKeySETTLED)

                ' determine array bounds
                llBound = m_vClaimDetails.GetLowerBound(1)
                lUBound = m_vClaimDetails.GetUpperBound(1)

                For lClaimItem As Integer = llBound To lUBound

                    ' get the details for the claim
                    lClaimStatusId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailsClaimStatusId, lClaimItem), 0)
                    bInfoOnly = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailsInfoOnly, lClaimItem), False)
                    sClaimNumber = CStr(m_vClaimDetails(kClaimDetailsClaimNumber, lClaimItem))
                    lClaimId = CInt(m_vClaimDetails(kClaimDetailsClaimId, lClaimItem))
                    sInsuranceRef = CStr(m_vClaimDetails(kClaimDetailsInsuranceRef, lClaimItem))
                    sClaimDescription = CStr(m_vClaimDetails(kClaimDetailsDescription, lClaimItem))
                    lProductId = CInt(m_vClaimDetails(kClaimDetailsProductId, lClaimItem))
                    sProductDescription = CStr(m_vClaimDetails(kClaimDetailsProductDescription, lClaimItem))
                    sCaseNumber = gPMFunctions.ToSafeString(m_vClaimDetails(kClaimDetailsCaseNumber, lClaimItem))

                    ' determine which node is to be the parent
                    If lClaimStatusId = kClaimStatusIdClosed Or lClaimStatusId = kClaimStatusIdReClosed Then
                        oParentNode = oSettledNode
                    ElseIf bInfoOnly Then
                        oParentNode = oInfoNode
                    Else
                        oParentNode = oOpenNode
                    End If

                    If m_vClaimDetails.GetUpperBound(0) >= kClaimDetailsIsOtherClaim Then
                        If gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailsIsOtherClaim, lClaimItem)) Then
                            sNodeKey = oParentNode.Name & "OtherClaims"
                            If FindTreeviewNode(sNodeKey) = gPMConstants.PMEReturnCode.PMFalse Then
                                oNode = tvwClaims.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sNodeKey, "Other Claims", kImageFolderClosed)
                                'developer guide no. 210
                                'oNode.SelectedImageIndex = kImageFolderOpen
                                oNode.Tag = CStr(0)
                            End If
                            oParentNode = tvwClaims.Nodes.Find(sNodeKey, True)(0)
                        End If
                    End If
                    ' add product node
                    ' create a new key

                    sNodeKey = oParentNode.Name.ToString() & "Product" & CStr(lProductId)
                    If sTempNodeKey <> sNodeKey Then
                        sTempNodeKey = sNodeKey
                        oMainNode = Nothing
                        If tvwClaims.Nodes.Find(sNodeKey, True).Length > 0 Then
                            oMainNode = tvwClaims.Nodes.Find(sNodeKey, True)(0)
                        End If
                        If oMainNode Is Nothing Then
                            ' add the node
                            oMainNode = oParentNode.Nodes.Add(sNodeKey, sProductDescription, kImageFolderClosed)
                            'oNode.SelectedImageIndex = kImageFolderOpen
                            oMainNode.Tag = CStr(0)
                        End If
                    End If
                    ' check the node doesn't already exist
                    'If FindTreeviewNode(sNodeKey) = gPMConstants.PMEReturnCode.PMFalse Then
                    '    ' add the node
                    '    oNode = tvwClaims.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sNodeKey, sProductDescription, kImageFolderClosed)
                    '    'oNode.SelectedImageIndex = kImageFolderOpen
                    '    oNode.Tag = CStr(0)
                    'End If

                    'Changed to pick up the correct node.
                    'oParentNode = tvwClaims.Nodes.Find(oParentNode.Name, True)(0).Nodes.Item(sNodeKey)
                    ' add claims node
                    If oMainNode IsNot Nothing Then
                        oNode = oMainNode.Nodes.Add(sClaimNumber & "/s", sClaimNumber, kImageFolderClosed, kImageFolderSelected)
                        oNode.Tag = CStr(lClaimId)
                    End If
                    ' create claim details container
                    oClaimDetails = New cClaimDetails()
                    oClaimDetails.ClaimDescription = sClaimDescription
                    oClaimDetails.InsuranceRef = sInsuranceRef
                    oClaimDetails.CaseNumber = sCaseNumber

                    m_colClaimVersionDetails.Add(oClaimDetails, sClaimNumber)

                    ' if there is only one claim node
                    If lUBound = 0 Then
                        ' expand and select it automatically
                        oParentNode.ExpandAll()

                        'Added to select the claim node by default.
                        tvwClaims.SelectedNode = oNode

                        ' and set the description and insurance ref details
                        fraClaimInformation.Text = "Claim Information - " & sClaimDescription
                        'txtDescription.Text = sClaimDescription
                        txtInsuranceRef.Text = sInsuranceRef
                        If sCaseNumber <> "" Then
                            lblCaseNumber.Visible = True
                            txtCaseNumber.Visible = True
                            txtCaseNumber.Text = sCaseNumber
                            cmdViewCase.Visible = True
                        Else
                            lblCaseNumber.Visible = False
                            txtCaseNumber.Visible = False
                            cmdViewCase.Visible = False
                        End If
                    End If

                Next
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateClaimVersionDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Private Function PopulateClaimVersionDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClaimVersionDetails"

        Dim lReturn As Integer
        Dim oItem As ListViewItem
        Dim llBound, lUBound, lClaimId, lVersionId As Integer
        Dim dtCreateDate As Date
        Dim sTransactionType, sVersionDescription As String
        Dim crTotalIncurred, crTotalPaid, crThisReserveRevision, crThisReservePayment, crThisSalvageRecovery, crThisThirdPartyRecovery, crCurrentReserve As Decimal
        Dim sInsuranceFileCurrency, sClaimCurrency, sCreatedBy, sClaimDescription, sInsuranceRef As String
        Dim lInsuranceFileCnt As Integer
        Dim sClaimNumber As String = ""
        Dim lRiskCnt As Integer
        Dim sClientShortName As String = ""
        Dim dtLossFromDate As Date
        Dim sInsuranceHolderShortname As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 178
            lReturn = ListViewFunc.ListViewBatchStart(lvwClaimVersions)

            ' clear all items in the list view
            lvwClaimVersions.Items.Clear()

            ' for each of the claim versions for this claims
            If Information.IsArray(m_vClaimVersionDetails) Then

                ' get array boundaries

                llBound = m_vClaimVersionDetails.GetLowerBound(1)

                lUBound = m_vClaimVersionDetails.GetUpperBound(1)

                ' for each claim version
                For lClaimItem As Integer = llBound To lUBound

                    ' get the claim versions details from the array
                    lClaimId = gPMFunctions.ToSafeLong(m_vClaimVersionDetails(kClaimVersionDetailsClaimId, lClaimItem), 0)
                    lVersionId = gPMFunctions.ToSafeLong(m_vClaimVersionDetails(kClaimVersionDetailsVersionId, lClaimItem), 0)
                    dtCreateDate = gPMFunctions.ToSafeDate(m_vClaimVersionDetails(kClaimVersionDetailsCreateDate, lClaimItem), CDate("00:00:00"))

                    sTransactionType = CStr(m_vClaimVersionDetails(kClaimVersionDetailsTransactionType, lClaimItem))

                    sVersionDescription = CStr(m_vClaimVersionDetails(kClaimVersionDetailsVersionDescription, lClaimItem))
                    crTotalIncurred = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsTotalIncurred, lClaimItem), 0)
                    crTotalPaid = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsTotalPaid, lClaimItem), 0)
                    crThisReserveRevision = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsThisReserveRevision, lClaimItem), 0)
                    crThisReservePayment = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsThisReservePayment, lClaimItem), 0)
                    crThisSalvageRecovery = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsThisSalvageRecovery, lClaimItem), 0)
                    crThisThirdPartyRecovery = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsThisThirdPartyRecovery, lClaimItem), 0)
                    crCurrentReserve = gPMFunctions.ToSafeCurrency(m_vClaimVersionDetails(kClaimVersionDetailsCurrentReserve, lClaimItem), 0)

                    sInsuranceFileCurrency = CStr(m_vClaimVersionDetails(kClaimVersionDetailsInsuranceFileCurrency, lClaimItem))

                    sClaimCurrency = CStr(m_vClaimVersionDetails(kClaimVersionDetailsClaimCurrency, lClaimItem))

                    sCreatedBy = CStr(m_vClaimVersionDetails(kClaimVersionDetailsCreatedBy, lClaimItem))

                    ' add a list item to the claims list view
                    oItem = lvwClaimVersions.Items.Add("")

                    ' setup the list item
                    oItem.Text = CStr(lClaimId)

                    ' save the array position against the selected item
                    oItem.Tag = CStr(lClaimItem)

                    oItem.SubItems.Add(kClaimVersionDetailsVersionId).Text = CStr(lVersionId)
                    oItem.SubItems.Add(kClaimVersionDetailsCreateDate).Text = gPMFunctions.ToSafeDate(dtCreateDate).ToString("d")
                    oItem.SubItems.Add(kClaimVersionDetailsTransactionType).Text = sTransactionType
                    oItem.SubItems.Add(kClaimVersionDetailsVersionDescription).Text = sVersionDescription
                    oItem.SubItems.Add(kClaimVersionDetailsTotalIncurred).Text = StringsHelper.Format(crTotalIncurred, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsTotalPaid).Text = StringsHelper.Format(crTotalPaid, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsThisReserveRevision).Text = StringsHelper.Format(crThisReserveRevision, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsThisReservePayment).Text = StringsHelper.Format(crThisReservePayment, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsThisSalvageRecovery).Text = StringsHelper.Format(crThisSalvageRecovery, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsThisThirdPartyRecovery).Text = StringsHelper.Format(crThisThirdPartyRecovery, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsCurrentReserve).Text = StringsHelper.Format(crCurrentReserve, "0.00")
                    oItem.SubItems.Add(kClaimVersionDetailsInsuranceFileCurrency).Text = sInsuranceFileCurrency
                    oItem.SubItems.Add(kClaimVersionDetailsClaimCurrency).Text = sClaimCurrency
                    oItem.SubItems.Add(kClaimVersionDetailsCreatedBy).Text = sCreatedBy

                Next

            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            'developer guide no. 178
            lReturn = ListViewFunc.ListViewBatchEnd()




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupClaimVersionListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Private Function SetupClaimVersionListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupClaimVersionListView"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwClaimVersions.Columns.Clear()

            lvwClaimVersions.Columns.Insert(klvwIndexClaimId - 1, klvwKeyClaimId, klvwTextClaimId, CInt(VB6.TwipsToPixelsX(0)))
            lvwClaimVersions.Columns.Insert(klvwIndexVersion - 1, klvwKeyVersion, klvwTextVersion, CInt(VB6.TwipsToPixelsX(900)), HorizontalAlignment.Center, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexTransactionDate - 1, klvwKeyTransactionDate, klvwTextTransactionDate, CInt(VB6.TwipsToPixelsX(1700)))
            lvwClaimVersions.Columns.Insert(klvwIndexTransactionType - 1, klvwKeyTransactionType, klvwTextTransactionType, CInt(VB6.TwipsToPixelsX(1700)))
            lvwClaimVersions.Columns.Insert(klvwIndexVersionDescription - 1, klvwKeyVersionDescription, klvwTextVersionDescription, CInt(VB6.TwipsToPixelsX(2800)))
            lvwClaimVersions.Columns.Insert(klvwIndexTotalIncurred - 1, klvwKeyTotalIncurred, klvwTextTotalIncurred, CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexTotalPaid - 1, klvwKeyTotalPaid, klvwTextTotalPaid, CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexThisRevision - 1, klvwKeyThisRevision, klvwTextThisRevision, CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexThisPayment - 1, klvwKeyThisPayment, klvwTextThisPayment, CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexThisSalvageRecovery - 1, klvwKeyThisSalvageRecovery, klvwTextThisSalvageRecovery, CInt(VB6.TwipsToPixelsX(2150)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexThisThirdPartyRecovery - 1, klvwKeyThisThirdPartyRecovery, klvwTextThisThirdPartyRecovery, CInt(VB6.TwipsToPixelsX(2450)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexCurrentReserve - 1, klvwKeyCurrentReserve, klvwTextCurrentReserve, CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimVersions.Columns.Insert(klvwIndexPolicyCurrency - 1, klvwKeyPolicyCurrency, klvwTextPolicyCurrency, CInt(VB6.TwipsToPixelsX(1750)))
            lvwClaimVersions.Columns.Insert(klvwIndexLossCurrency - 1, klvwKeyLossCurrency, klvwTextLossCurrency, CInt(VB6.TwipsToPixelsX(1750)))
            lvwClaimVersions.Columns.Insert(klvwIndexUser - 1, klvwKeyUser, klvwTextUser, CInt(VB6.TwipsToPixelsX(1750)))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub UserControl_Terminate()

        ' initialise collection
        m_colClaimVersionDetails = Nothing

    End Sub


    ' ***************************************************************** '
    ' Name: SetupInitialTreeView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Private Function SetupInitialTreeView(ByVal v_bClaimDetailsFound As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupInitialTreeView"

        Dim lReturn As Integer

        Dim oRealRootNode, oRootNode, oOpenNode, oInfoOnlyNode, oSettledNode As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' setup tree view appearence
            tvwClaims.Nodes.Clear()
            tvwClaims.BorderStyle = BorderStyle.Fixed3D


            'to do list,It might not be required
            'tvwClaims = mscomctl.TreeStyleConstants.tvwTreelinesPlusMinusText

            tvwClaims.Indent = VB6.TwipsToPixelsX(5)

            ' add real root node
            oRealRootNode = tvwClaims.Nodes.Add("REALROOT", "", kImageFolderClosed)

            'developer guide no. 210
            'oRealRootNode.SelectedImageIndex = kImageFolderOpen
            oRealRootNode.Tag = CStr(0)

            ' if no claim details found for the specified search criteria
            If Not v_bClaimDetailsFound Then

                ' indicate to the user that no claims have been found
                oRootNode = tvwClaims.Nodes.Find(oRealRootNode.Name, True)(0).Nodes.Add(ktvwNodeKeyALL, "NO CLAIMS FOUND", kImageFolderClosed)
                oRootNode.Tag = CStr(0)
            Else
                ' add root node "ALL"
                oRootNode = tvwClaims.Nodes.Find(oRealRootNode.Name, True)(0).Nodes.Add(ktvwNodeKeyALL, "All", kImageFolderClosed)
                'developer guide no. 210
                'oRootNode.SelectedImageIndex = kImageFolderOpen
                oRootNode.Tag = CStr(0)

                ' add base nodes "OPEN"
                oOpenNode = tvwClaims.Nodes.Find(oRootNode.Name, True)(0).Nodes.Add(ktvwNodeKeyOPEN, "Open", kImageFolderClosed)
                'developer guide no. 210
                'oOpenNode.SelectedImageIndex = kImageFolderOpen
                oOpenNode.Tag = CStr(0)

                ' add base nodes "INFOONLY"
                oInfoOnlyNode = tvwClaims.Nodes.Find(oRootNode.Name, True)(0).Nodes.Add(ktvwNodeKeyINFOONLY, "Info Only", kImageFolderClosed)
                'developer guide no. 210
                'oInfoOnlyNode.SelectedImageIndex = kImageFolderOpen
                oInfoOnlyNode.Tag = CStr(0)


                ' add base nodes "SETTLED"
                oSettledNode = tvwClaims.Nodes.Find(oRootNode.Name, True)(0).Nodes.Add(ktvwNodeKeySETTLED, "Settled", kImageFolderClosed)
                'developer guide no. 210
                'oSettledNode.SelectedImageIndex = kImageFolderOpen
                oSettledNode.Tag = CStr(0)

            End If
            oRealRootNode.Expand()
            oRootNode.Expand()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessNodeClick
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Private Function ProcessNodeClick(ByVal v_lClaimID As Integer, ByVal v_sClaimNumber As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessNodeClick"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oClaimDetails As cClaimDetails

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down selected claim details
            lReturn = ClearSelectedItemsDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ClearSelectedItemsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the claim versions for the specified claim details
            lReturn = CType(GetClaimVersions(v_lClaimID, v_sClaimNumber), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimVersions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate the claim versions list view
            lReturn = PopulateClaimVersionDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClaimVersionDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate the additional claims details on screen
            oClaimDetails = m_colClaimVersionDetails.Item(v_sClaimNumber)
            fraClaimInformation.Text = "Claim Information - " & oClaimDetails.ClaimDescription
            'txtDescription.Text = oClaimDetails.ClaimDescription
            txtInsuranceRef.Text = oClaimDetails.InsuranceRef

            If oClaimDetails.CaseNumber <> "" Then
                lblCaseNumber.Visible = True
                txtCaseNumber.Visible = True
                txtCaseNumber.Text = oClaimDetails.CaseNumber
                cmdViewCase.Visible = True
            Else
                lblCaseNumber.Visible = False
                txtCaseNumber.Visible = False
                cmdViewCase.Visible = False
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oClaimDetails = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSelectedClaimsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function GetSelectedClaimsDetails(Optional ByRef r_lClaimId As Integer = 0, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByRef r_sClaimNumber As String = "", Optional ByRef r_sInsuranceRef As String = "", Optional ByRef r_lRiskCnt As Integer = 0, Optional ByRef r_sClientShortname As String = "", Optional ByRef r_dtLossFromDate As Date = #12/30/1899#, Optional ByRef r_sInsuranceHolderShortname As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_bRecovery As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedClaimsDetails"

        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            r_lClaimId = m_lSelectedClaimID
            r_lInsuranceFileCnt = m_lSelectedInsuranceFileCnt
            r_sClaimNumber = m_sSelectedClaimNumber
            r_sInsuranceRef = m_sInsuranceRef
            r_lRiskCnt = m_lSelectedRiskCnt
            r_sClientShortname = m_sSelectedClientShortName
            r_dtLossFromDate = m_dtSelectedLossFromDate
            r_sInsuranceHolderShortname = m_sSelectedInsuranceHolderShortname
            r_lInsuranceFolderCnt = m_lSelectedInsuranceFolderCnt
            r_bRecovery = m_sSelectedTransTypeCode = "C_SA" Or m_sSelectedTransTypeCode = "C_RV"


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetSelectedItemsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function SetSelectedItemsDetails(ByVal oItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetSelectedItemsDetails"

        Dim lReturn, lSelectedItemIndex As Integer
        Dim sClaimDescription As String = ""
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lSelectedClaimID = CInt(oItem.Text)

            lSelectedItemIndex = Convert.ToString(oItem.Tag)


            m_sSelectedClaimNumber = CStr(m_vClaimVersionDetails(kClaimVersionDetailsClaimNumber, lSelectedItemIndex))
            m_lSelectedInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vClaimVersionDetails(kClaimVersionDetailsInsuranceFileCnt, lSelectedItemIndex), 0)

            m_sSelectedInsuranceRef = CStr(m_vClaimVersionDetails(kClaimVersionDetailsInsuranceRef, lSelectedItemIndex))
            m_lSelectedRiskCnt = gPMFunctions.ToSafeLong(m_vClaimVersionDetails(kClaimVersionDetailsRiskCnt, lSelectedItemIndex), 0)

            m_sSelectedClientShortName = CStr(m_vClaimVersionDetails(kClaimVersionDetailsClientShortName, lSelectedItemIndex))
            m_dtSelectedLossFromDate = gPMFunctions.ToSafeDate(m_vClaimVersionDetails(kClaimVersionDetailsLossFromDate, lSelectedItemIndex), CDate("00:00:00"))

            m_sSelectedInsuranceHolderShortname = CStr(m_vClaimVersionDetails(kClaimVersionDetailsInsuranceHolderShortname, lSelectedItemIndex))

            m_lSelectedInsuranceFolderCnt = CInt(m_vClaimVersionDetails(kClaimVersionDetailsInsuranceFolderCnt, lSelectedItemIndex))

            sClaimDescription = CStr(m_vClaimVersionDetails(kClaimDetailsDescription, lSelectedItemIndex))

            m_sSelectedTransTypeCode = CStr(m_vClaimVersionDetails(kClaimVersionDetailsTransactionTypeCode, lSelectedItemIndex))

            fraClaimInformation.Text = "Claim Information - " & sClaimDescription

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ClearSelectedItemsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ClearSelectedItemsDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ClearSelectedItemsDetails"

        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lSelectedClaimID = 0
            m_sSelectedClaimNumber = ""
            m_lSelectedInsuranceFileCnt = 0
            m_sSelectedInsuranceRef = ""
            m_lSelectedRiskCnt = 0
            m_sSelectedClientShortName = ""
            m_dtSelectedLossFromDate = CDate("00:00:00")
            m_lSelectedInsuranceFolderCnt = 0


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Shadows Function Refresh() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Refresh"

        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            LoadClaimDetails()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LoadClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function LoadClaimDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' reset the claims collection
            m_colClaimVersionDetails = New Collection()

            ' setup claim version list view
            lReturn = SetupClaimVersionListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimVersionListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get all claims details for claim lookup details
            lReturn = GetClaimDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' clear all version related details
            lvwClaimVersions.Items.Clear()
            fraClaimInformation.Text = "Claim Information"
            'txtDescription = ""
            txtInsuranceRef.Text = ""

            ' populate tree view with claim details
            lReturn = PopulateClaimDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Function FindTreeviewNode(ByVal sNodeKey As String) As Integer
        ' returns PMTrue if a node with a key of sNodeKey exists
        ' PMFalse otherwise


        Try
            'Below commented code was unable to find in the whole treeview.
            If tvwClaims.Nodes.Find(sNodeKey, True).Length > 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            'For Each nSearchNode As TreeNode In tvwClaims.Nodes
            '    If nSearchNode.Name = sNodeKey Then
            '        Return gPMConstants.PMEReturnCode.PMTrue
            '    End If
            'Next nSearchNode

            Return gPMConstants.PMEReturnCode.PMFalse

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error searching treeview", vApp:=ACApp, vClass:=ACClass, vMethod:="FindTreeviewNode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    Private Sub SetSelectedRow()
        Try

            For iIndex As Integer = 0 To lvwClaimVersions.Items.Count - 1
                If CDbl(lvwClaimVersions.Items.Item(iIndex).Text) = m_lSelectedClaimID Then
                    lvwClaimVersions.Items.Item(iIndex).Selected = True
                    Exit For
                End If
            Next
        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute SetSelectedRow", vApp:=ACApp, vClass:=ACClass, vMethod:="SetSelectedRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub


    ' ***************************************************************** '
    '
    ' Name: MergeArrays
    '
    ' Description:
    '
    ' History: 25/08/2008 Created. - Amit
    '
    ' ***************************************************************** '
    Private Function MergeArrays(ByRef vArray(,) As Object, ByRef vOriginalArray(,) As Object, ByRef vMergedArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lHowMany As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(vArray) Then
                If Information.IsArray(vOriginalArray) Then
                    'We need to merge
                Else
                    'Not to do
                    vMergedArray = VB6.CopyArray(vArray)
                    Return result
                End If
            Else
                If Information.IsArray(vOriginalArray) Then
                    'Use this one
                    vMergedArray = VB6.CopyArray(vOriginalArray)
                    Return result
                Else
                    'Not to do
                    Return result
                End If
            End If

            'Put the original first
            ReDim vMergedArray(vArray.GetUpperBound(0), vArray.GetUpperBound(1) + vOriginalArray.GetUpperBound(1) + 1)

            For lTemp As Integer = vOriginalArray.GetLowerBound(1) To vOriginalArray.GetUpperBound(1)
                For lTemp2 As Integer = vOriginalArray.GetLowerBound(0) To vOriginalArray.GetUpperBound(0)


                    vMergedArray(lTemp2, lTemp) = vOriginalArray(lTemp2, lTemp)
                Next

            Next lTemp

            lHowMany = vOriginalArray.GetUpperBound(1) + 1

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                For lTemp2 As Integer = vArray.GetLowerBound(0) To vArray.GetUpperBound(0)


                    vMergedArray(lTemp2, lHowMany + lTemp) = vArray(lTemp2, lTemp)
                Next

            Next lTemp


            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeArrays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeArrays", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ShowCaseScreen
    ' Parameters:
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function ShowCaseScreen(ByVal v_lTask As Integer, ByVal v_sTransactionType As String, Optional ByVal v_lCaseID As Integer = 0, Optional ByVal v_lBaseCaseID As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowCaseScreen"

        Dim vResultArray As Object

        'developer guide no.88
        Dim oObject As Object

        Dim sCaseScreenID As String = ""

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            lReturn = m_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = CType(oObject, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oObject.SetProcessModes(vTask:=v_lTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=v_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            iPMFunc.GetSystemOption(5035, sCaseScreenID)

            If sCaseScreenID.Trim() = "" Or sCaseScreenID = "0" Then
                MessageBox.Show("Please select the Case Screen from System Option", "Find Case", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If


            oObject.ScreenId = gPMFunctions.ToSafeLong(sCaseScreenID)

            oObject.CallingAppName = ACApp

            If v_lCaseID > 0 Then

                oObject.CaseID = v_lCaseID

                oObject.BaseCaseID = v_lBaseCaseID

                oObject.CaseNumber = txtCaseNumber.Text.Trim()
            End If


            lReturn = oObject.Start()



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
            End If




        End Try
        Return result
    End Function

    Private Sub lvwClaimVersions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwClaimVersions.SelectedIndexChanged
        'SetSelectedItemsDetails(Item)
        If lvwClaimVersions.SelectedItems.Count <> 0 Then
            SetSelectedItemsDetails(lvwClaimVersions.SelectedItems(0))
        End If
    End Sub
    'Added by Sumeet-To provide GUI same as VB
    Private Sub tvwClaims_AfterExpand(ByVal sender As Object, ByVal EventArgs As System.Windows.Forms.TreeViewEventArgs) Handles tvwClaims.AfterExpand
        Dim Node As TreeNode = EventArgs.Node
        Node.ImageIndex = kImageFolderOpen
        Node.SelectedImageIndex = kImageFolderOpen
        'start
        Dim cNode As TreeNode = Node.FirstNode
        If Not IsNothing(cNode) Then
            Do
                If cNode.GetNodeCount(False) > 0 And cNode.IsExpanded Then
                    cNode.ImageIndex = kImageFolderOpen
                    cNode.SelectedImageIndex = kImageFolderOpen
                ElseIf cNode.GetNodeCount(False) > 0 Then
                    cNode.ImageIndex = kImageFolderClosed
                    cNode.SelectedImageIndex = kImageFolderClosed
                Else
                    cNode.SelectedImageIndex = cNode.ImageIndex
                End If
                cNode = cNode.NextNode
            Loop While Not IsNothing(cNode)
        End If
        'end
    End Sub
    'Added by Sumeet-To provide GUI same as VB
    Private Sub tvwClaims_AfterCollapse(ByVal sender As Object, ByVal EventArgs As System.Windows.Forms.TreeViewEventArgs) Handles tvwClaims.AfterCollapse
        Dim Node As TreeNode = EventArgs.Node
        Node.ImageIndex = kImageFolderClosed
        Node.SelectedImageIndex = kImageFolderClosed
        tvwClaims.Refresh()
    End Sub
End Class
