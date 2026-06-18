Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmSelectPeril
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmSelectPeril
    '
    ' Date: 15/07/00
    '
    ' Description: Select peril for salvage or third-party recovery
    ' ***************************************************************** '

    Private Const ACClass As String = "frmSelectPeril"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    'Developer Guide no. 7
    Private Const vbFormCode As Integer = 0
    ' Variables for Select Peril Claim
    Private m_lInsuranceFileCnt As Integer
    Private m_lCompanyID As Integer
    Private m_lClaimId As Integer
    Private m_sClaimNumber As String = ""
    Private m_lPerilId As Integer
    Private m_lCurrencyID As Integer
    Private m_sCurrency As String = ""
    Private m_sPerilType As String = ""
    Private m_lPerilTypeID As Integer
    Private m_sClientName As String = ""
    Private m_sPolicyNumber As String = ""
    Private m_lClassOfBusinessID As Integer
    Private m_sClassOfBusinessCode As String = ""

    ' Salvage or third party recovery?
    Private m_bIsSalvage As Boolean

    ' Array to Store Peril Details
    Private m_vPerilDetails(,) As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_ofrmRecoveryReceipting As frmRecoveryReceipting
    Private m_bIsRecoveriesReadOnly As Boolean = False

    ' ***************************************************************** '
    '                       STANDARD PROPERTIES
    ' ***************************************************************** '
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property



    ' ***************************************************************** '
    '                        CUSTOM PROPERTIES
    ' ***************************************************************** '
    Public Property ClaimId() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public Property ClaimNumber() As String
        Get
            Return m_sClaimNumber
        End Get
        Set(ByVal Value As String)
            m_sClaimNumber = Value
        End Set
    End Property

    Public Property ClientName() As String
        Get
            Return m_sClientName
        End Get
        Set(ByVal Value As String)
            m_sClientName = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property IsSalvage() As Boolean
        Get
            Return m_bIsSalvage
        End Get
        Set(ByVal Value As Boolean)
            m_bIsSalvage = Value
        End Set
    End Property

    Public Property PerilID() As Integer
        Get
            Return m_lPerilId
        End Get
        Set(ByVal Value As Integer)
            m_lPerilId = Value
        End Set
    End Property

    Public Property PerilType() As String
        Get
            Return m_sPerilType
        End Get
        Set(ByVal Value As String)
            m_sPerilType = Value
        End Set
    End Property

    Public Property PolicyNumber() As String
        Get
            Return m_sPolicyNumber
        End Get
        Set(ByVal Value As String)
            m_sPolicyNumber = Value
        End Set
    End Property


    ' ***************************************************************** '
    '                          PUBLIC FUNCTIONS
    ' ***************************************************************** '

    ' Updates all interface details from the search data storage.
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwSelectPeril.Items.Clear()

            ' Check that search details are valid before continuing.
            If Not Information.IsArray(m_vPerilDetails) Then
                Return result
            End If
            Dim olistitem As ListViewItem
            ' Assign the details to the interface.
            For lRow As Integer = m_vPerilDetails.GetLowerBound(1) To m_vPerilDetails.GetUpperBound(1)
                ' Create new item with Peril Code

                olistitem = lvwSelectPeril.Items.Add(CStr(m_vPerilDetails(ACPAPeril, lRow)).Trim(), "FindImage")

                ' Column 2 Peril Description

                ListViewHelper.GetListViewSubItem(olistitem, 1).Text = CStr(m_vPerilDetails(ACPADescription, lRow)).Trim()

                ' Set the tag property with the index of the search data storage.


                olistitem.Tag = CStr(lRow)
                'End With

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSelectPeril.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSelectPeril.Refresh()
                End If
            Next lRow

            ' Select the first item.
            lvwSelectPeril.Items.Item(0).Selected = True
            cmdSelect.Enabled = True

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Updates the property member from the search data storage.
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToInt32(lvwSelectPeril.Items.Item(lvwSelectPeril.FocusedItem.Index).Tag)

            ' Update the property members.
            m_lPerilId = gPMFunctions.ToSafeLong(CStr(m_vPerilDetails(ACPAPerilId, lSelectedItem)))
            m_sPerilType = CStr(m_vPerilDetails(ACPAPeril, lSelectedItem)).Trim()
            m_lPerilTypeID = gPMFunctions.ToSafeLong(CStr(m_vPerilDetails(ACPAPerilTypeId, lSelectedItem)))

            'S4B Claim Enhancements R&D 2005

            m_sCurrency = CStr(m_vPerilDetails(ACPACurrency, lSelectedItem)).Trim()
            m_lCurrencyID = gPMFunctions.ToSafeLong(CStr(m_vPerilDetails(ACPACurrencyID, lSelectedItem)))
            m_lCompanyID = gPMFunctions.ToSafeLong(CStr(m_vPerilDetails(ACPACompanyID, lSelectedItem)))
            m_lClassOfBusinessID = gPMFunctions.ToSafeLong(CStr(m_vPerilDetails(ACPACOBID, lSelectedItem)))
            m_sClassOfBusinessCode = CStr(m_vPerilDetails(ACPACOBCode, lSelectedItem)).Trim()

            m_bIsRecoveriesReadOnly = GetIsRecoveriesReadOnly(m_lInsuranceFileCnt)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Determines which action to take on the details
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String
        Dim iMsgResult As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    ' If we are maintaining an existing claim unlock it.
                    If m_sTransactionType <> "C_CO" Then
                        m_lReturn = CType(UnlockClaim(v_lClaimId:=m_lClaimId), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    'S4B Claim Enhancements R&D 2005


                    If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                        ' Set claim id and delete  claim

                        m_lReturn = g_oBusiness.DeleteClaim(v_lClaimId:=m_lClaimId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If
                    End If

                End If
            Else
                ' Check if there are any items available.
                If lvwSelectPeril.FocusedItem Is Nothing Then
                    MessageBox.Show("No peril selected", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'Modified,return false,so the form will not close
                    'Return m_lReturn
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the property member from the interface.
                m_lReturn = CType(DataToProperties(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                ' Process the recovery for the selected peril
                m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Ensure form is fully shutdown
                    m_ofrmRecovery = Nothing
                    result = m_lReturn
                End If

                ' If we error unlock the claim and prepare to exit
                If m_lStatus = gPMConstants.PMEReturnCode.PMError Then
                    ' If we are maintaining an existing claim unlock it.
                    If m_sTransactionType <> "C_CO" Then
                        m_lReturn = CType(UnlockClaim(v_lClaimId:=m_lClaimId), gPMConstants.PMEReturnCode)
                    End If

                    'S4B Claim Enhancements R&D 2005
                    If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                        ' Set claim id and delete  claim

                        m_lReturn = g_oBusiness.DeleteClaim(v_lClaimId:=m_lClaimId)
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '                       PRIVATE FUNCTIONS
    ' ***************************************************************** '

    ' Display all language specific captions.
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=g_lRecoveryMode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & _
             " - " & _
             CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPFormCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = " - " Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSelectPeril.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPColumnPeril, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwSelectPeril.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPColumnDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Load the recovery interface
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no.50
            m_ofrmRecovery = New frmRecovery
            ' Assign the parameters to the interface properties.

            With m_ofrmRecovery
                .CallingAppName = m_sCallingAppName
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                .Task = m_iTask

                .InsuranceFileCnt = m_lInsuranceFileCnt
                .CompanyID = m_lCompanyID

                .ClaimId = m_lClaimId
                .ClaimNumber = m_sClaimNumber
                .ClientName = m_sClientName

                .PerilID = m_lPerilId
                .PerilType = m_sPerilType
                .PerilTypeID = m_lPerilTypeID

                .LossCurrency = m_sCurrency
                .LossCurrencyID = m_lCurrencyID

                .ClassOfBusiness = m_sClassOfBusinessCode
                .ClassOfBusinessID = m_lClassOfBusinessID

                .IsSalvage = m_bIsSalvage
                .IsRecoveriesReadOnly = m_bIsRecoveriesReadOnly
            End With

            ' Load the instance of the interface into memory.
            Dim tempLoadForm As frmRecovery = m_ofrmRecovery
            ' Check if we have had an error so far.
            If m_ofrmRecovery.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST return the error.
                result = m_ofrmRecovery.ErrorNumber
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface into memory", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Process the recovery form
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReserve Or g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReserve Then

                ' Load the interface into memory.
                m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Display the interface.
                m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)
                m_lStatus = m_ofrmRecovery.Status

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Destroy the interface from memory.
                m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                ' load receipting interface
                m_lReturn = LoadReceiptingInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' show receipting interface
                m_lReturn = CType(ShowReceiptingInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' unload the interface
                m_lReturn = UnloadReceiptingInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Updates the interface details from the property members.
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.
            txtClaimNumber.Text = m_sClaimNumber.Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Sets all of the interface default values.
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the interface details with the property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Show the recovery interface
    Private Function ShowInterface(ByVal lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display the interface.
            VB6.ShowForm(m_ofrmRecovery, lDisplayState)

            If lDisplayState = FormShowConstants.Modal Then
                ' Check for any form errors.
                If m_ofrmRecovery.ErrorNumber <> 0 Then
                    result = m_ofrmRecovery.ErrorNumber
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Unload the Recovery form
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the property members from the interface parameters.
            With m_ofrmRecovery
                m_lStatus = .Status
                m_lClaimId = .ClaimId
                m_sClaimNumber = .ClaimNumber
            End With

            ' Unload and destroy the instance of the interface from memory.
            m_ofrmRecovery.Close()
            m_ofrmRecovery = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload the interface from memory", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    '                           CONTROL EVENTS
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Hide the interface.
            Me.Hide()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' If in receipting mode we can only process 1 at a time!!
            If g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReceipt Or g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReceipt Then
                If m_lStatus = gPMConstants.PMEReturnCode.PMError Then
                    ' We errored so return appropriately
                    m_lStatus = gPMConstants.PMEReturnCode.PMError
                    Me.Hide()
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue And m_lStatus = gPMConstants.PMEReturnCode.PMOK Then
                    ' Everything OK or Error so we can hide the interface.
                    Me.Hide()
                Else
                    ' Reset status to cancel
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSelectPeril_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectPeril.DoubleClick
        ' Delegate this call to the OK button click
        cmdSelect_Click(cmdSelect, New EventArgs())
    End Sub

    Private Sub lvwSelectPeril_ItemClick(ByVal Item As ListViewItem)
        cmdSelect.Enabled = True
    End Sub


    ' ***************************************************************** '
    '                          FORM EVENTS
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        Try

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmSelectPeril_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'Get recovery type Details

            m_lReturn = g_oBusiness.GetRecoveryTypes(v_bIsSalvage:=m_bIsSalvage, r_vResultArray:=g_vRecoveryTypes)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'Get Peril Details

            m_lReturn = g_oBusiness.GetPerilDetails(m_vPerilDetails, ClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMNotFound
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            Exit Sub


        End Try
    End Sub

    ' Store all Property Details before unloading form
    Private Sub frmSelectPeril_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending upon the interface task etc.
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide no. 7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: LoadReceiptingInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function LoadReceiptingInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadReceiptingInterface"

        Dim lReturn As Integer


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create a new instance of form
            m_ofrmRecoveryReceipting = New frmRecoveryReceipting()

            ' assign the parameters to the interface properties
            With m_ofrmRecoveryReceipting
                .RecoveryMode = g_lRecoveryMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                .Task = m_iTask
                .ClaimId = m_lClaimId
                .ClaimPerilId = m_lPerilId
                '.frmRecoveryReceipting_Load(Nothing, Nothing)
            End With


            ' Check if we have had an error so far.
            If m_ofrmRecoveryReceipting.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST return the error.
                result = m_ofrmRecoveryReceipting.ErrorNumber
            End If


        Catch ex As Exception

            m_ofrmRecoveryReceipting = Nothing

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
    ' Name: ShowReceiptingInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ShowReceiptingInterface(ByVal lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowReceiptingInterface"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Display the interface.
            '
            VB6.ShowForm(m_ofrmRecoveryReceipting, lDisplayState)

            If lDisplayState = FormShowConstants.Modal Then
                ' Check for any form errors.
                If m_ofrmRecoveryReceipting.ErrorNumber <> 0 Then
                    result = m_ofrmRecoveryReceipting.ErrorNumber
                End If
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

    ' ***************************************************************** '
    ' Name: UnloadReceiptingInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UnloadReceiptingInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnloadReceiptingInterface"

        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the property members from the interface parameters.
            With m_ofrmRecoveryReceipting
                m_lStatus = .Status
            End With

            ' Unload and destroy the instance of the interface from memory.
            m_ofrmRecoveryReceipting.Close()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            m_ofrmRecoveryReceipting = Nothing

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' This rroutine is used to get the recovery script configuration from product maintenance.
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns></returns>
    Private Function GetIsRecoveriesReadOnly(ByVal nInsuranceFileCnt As Integer) As Boolean
        Dim nProductID As Integer
        Dim oProduct As bSIRProduct.Business = Nothing
        Dim oIsRecoveriesReadonly As Object = Nothing

        m_lReturn = g_oObjectManager.GetInstance(oProduct, "bSIRProduct.Business", PMGetViaClientManager)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Product Business  object", ACApp, ACClass, "GetIsRecoveriesReadOnly", Information.Err().Number, Information.Err().Description)
            Return False
        End If

        m_lReturn = oProduct.GetProductid(ifilecnt:=nInsuranceFileCnt, vProduct_id:=nProductID)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oProduct.GetProductValue(nProductID, "is_recoveries_read_only", oIsRecoveriesReadonly)
        End If

        If IsArray(oIsRecoveriesReadonly) AndAlso gPMFunctions.ToSafeString(oIsRecoveriesReadonly(0, 0)).trim() = "1" Then
            Return True
        End If

        oProduct.Dispose()
        oProduct = Nothing
        oIsRecoveriesReadonly = Nothing

    End Function
End Class