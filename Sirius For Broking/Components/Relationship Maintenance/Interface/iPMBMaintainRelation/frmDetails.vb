Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developers guide no. 129
Imports SharedFiles
Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form

    Implements IDisposable
    Private Const ACClass As String = "frmDetails"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lStatus As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lPartnerID As Integer
    Private m_iPartyRelationshipGroupId As Integer
    Private m_lRelationshipTypeID As Integer
    Private m_iTask As Integer
    Private m_bInUse As Boolean
    'developer guide no. 50
    Dim frminterface As frmInterface
    Private m_vDataArray(,) As Object
    Private m_vPartyArray(,) As Object
    Private m_vLookupArray As Object
    Private Const vbFormCode As Integer = 0
    ' Form Fields
    Private m_oFormField As iPMFormControl.FormFields

    ' *** Public Properties (Begin) *** '

    Public WriteOnly Property InUse() As Boolean
        Set(ByVal Value As Boolean)
            m_bInUse = Value
        End Set
    End Property
    'developer guide no. 33
    Public WriteOnly Property DataArray() As Object

        'developer guide no. 33
        Set(ByVal Value As Object)
            m_vDataArray = Value
        End Set
    End Property

    Public WriteOnly Property LookupArray() As Object
        Set(ByVal Value As Object)


            m_vLookupArray = Value
        End Set
    End Property
    'developer guide no. 33
    Public WriteOnly Property PartyArray() As Object
        'developer guide no. 33
        Set(ByVal Value As Object)
            m_vPartyArray = Value
        End Set
    End Property

    Public WriteOnly Property PartyRelationshipGroupId() As Integer
        Set(ByVal Value As Integer)
            m_iPartyRelationshipGroupId = Value
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

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property PartnerID() As Integer
        Get
            Return m_lPartnerID
        End Get
        Set(ByVal Value As Integer)
            m_lPartnerID = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public Property RelationshipTypeID() As Integer
        Get
            Return m_lRelationshipTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lRelationshipTypeID = Value
        End Set
    End Property



    ' *** Public Properties (End) *** '

    ' ***************************************************************** '
    '
    ' Name: DataToInterface
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the code
            m_lReturn = m_oFormField.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the description
            m_lReturn = m_oFormField.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the partner
            m_lReturn = CType(GetIndexFromID(v_lID:=m_lPartnerID, v_iIndex:=iIndex), gPMConstants.PMEReturnCode)
            cboPartner.SelectedIndex = iIndex


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetFieldValidation
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormField.AddNewFormField(ctlControl:=cboPartner, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFieldValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateCombo
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '          11/07/2002 CMG/PB - Restrict combo to only
    '                              show client party relationships
    ' ***************************************************************** '
    Private Function PopulateCombo() As Integer

        Dim result As Integer = 0
        Dim iItemLoop As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate the combo box
            cboPartner.Items.Insert(0, "(none)")
            VB6.SetItemData(cboPartner, 0, 0)

            If Information.IsArray(m_vDataArray) Then
                iItemLoop = 1
                For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
                    'Only add if client party relationship
                    If CDbl(m_vDataArray(ACArrayPartyRelationshipGroupID, iLoop1)) = m_iPartyRelationshipGroupId Then
                        cboPartner.Items.Add(CStr(m_vDataArray(ACArrayDescription, iLoop1)))
                        VB6.SetItemData(cboPartner, iItemLoop, CInt(m_vDataArray(ACArrayRelationShipTypeID, iLoop1)))
                        iItemLoop += 1
                    End If
                Next iLoop1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise form control
            m_oFormField = New iPMFormControl.FormFields()
            'developer guide no. 9
            m_lReturn = m_oFormField.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMFormControl.FormFields", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface details if we're in edit mode
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
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
                If m_oFormField IsNot Nothing Then
                    m_oFormField.Dispose()
                    m_oFormField = Nothing
                End If
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Sub cboPartner_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartner.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=cboPartner)

    End Sub

    Private Sub cboPartner_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartner.Leave

        m_lReturn = m_oFormField.LostFocus(ctlControl:=cboPartner)

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Cancel
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_lReturn = m_oFormField.CheckMandatoryControls()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = CType(ValidateOurControls(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Set the properties
        m_lReturn = CType(InterfaceToProperties(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try



            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            End If

        Catch excep As System.Exception



            ' Do Not Call any functions before here or the error will be lost
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_QueryUnload Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' If you want to rollback a transaction or something, do it here



            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=txtCode)

    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave

        m_lReturn = m_oFormField.LostFocus(ctlControl:=txtCode)

    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        m_lReturn = m_oFormField.GotFocus(ctlControl:=txtDescription)

    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave

        m_lReturn = m_oFormField.LostFocus(ctlControl:=txtDescription)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: InterfaceToProperties
    '
    ' Description: Set the values of the properties from the interface
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Code

            m_sCode = CStr(m_oFormField.UnformatControl(ctlControl:=txtCode))
            ' Description

            m_sDescription = CStr(m_oFormField.UnformatControl(ctlControl:=txtDescription))
            ' Partner's ID
            m_lPartnerID = VB6.GetItemData(cboPartner, cboPartner.SelectedIndex)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckUniqueCode
    '
    ' Description: Checks that the user has entered a valid code
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckUniqueCode() As Integer

        Dim result As Integer = 0
        Dim sCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the code

            sCode = CStr(m_oFormField.UnformatControl(ctlControl:=txtCode))

            ' Check it isnt the same as we started with
            If sCode.Trim() = m_sCode.Trim() Then
                Return result
            End If

            ' Loop through the array and check if its the same as any other
            ' PW290702 - only do if records initially existed
            If Information.IsArray(m_vDataArray) Then
                For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
                    If CStr(m_vDataArray(ACArrayCode, iLoop1)).Trim().ToUpper() = sCode.ToUpper() Then
                        ' Found a match so PMFalse outta here
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If
                Next iLoop1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUniqueCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUniqueCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: ValidateOurControls
    '
    ' Description: Validates control values that arent covered by Form Fields
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateOurControls() As Integer

        Dim result As Integer = 0
        Dim lPartnerID As Integer
        Dim sMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the ID of the parent isnt the same as itself
            lPartnerID = VB6.GetItemData(cboPartner, cboPartner.SelectedIndex)

            If lPartnerID <> 0 Then

                If lPartnerID = m_lRelationshipTypeID Then
                    MessageBox.Show("You cannot relate a type to itself.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Check the Code is unique
            m_lReturn = CType(CheckUniqueCode(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("The code '" & txtCode.Text & "' is already in use.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that the partnership hasnt been changed
            If m_bInUse Then

                If m_lPartnerID <> VB6.GetItemData(cboPartner, cboPartner.SelectedIndex) Then
                    sMsg = "Unable to change the partner for this relationship as it is in use " & _
                           "by one or more parties." & Environment.NewLine & _
                           "Either change it back or cancel off this screen."
                    MessageBox.Show(sMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Check that it wont break any relationships that are in use by creating this
            ' relationship.
            m_lReturn = CType(CheckNotInUse(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMsg = "Cannot select this relation as it is in use by one or more parties." & Environment.NewLine & _
                       "Please select another."
                MessageBox.Show(sMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOurControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOurControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets defaults for adding a new record
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the captions
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Fill the combo box with valid partners
            m_lReturn = CType(PopulateCombo(), gPMConstants.PMEReturnCode)


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd

                    ' Default the combo to (none)
                    m_lReturn = CType(GetIndexFromID(v_lID:=0, v_iIndex:=iIndex), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    cboPartner.SelectedIndex = iIndex

                Case gPMConstants.PMEComponentAction.PMEdit

                    ' Lock code text box
                    txtCode.Enabled = False

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetIndexFromID
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetIndexFromID(ByVal v_lID As Integer, ByRef v_iIndex As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop1 As Integer = 0 To cboPartner.Items.Count - 1
                If VB6.GetItemData(cboPartner, iLoop1) = v_lID Then
                    v_iIndex = iLoop1
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIndexFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIndexFromID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckInUse
    '
    ' Description: Check if a relation is associated with a party.
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckInUse(ByVal v_iTypeID As Integer, ByRef r_bInUse As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default to not in use
            r_bInUse = False

            ' Check that there's some in use
            If Not Information.IsArray(m_vPartyArray) Then
                Return result
            End If

            ' Loop through the array
            For iLoop1 As Integer = 0 To m_vPartyArray.GetUpperBound(1)
                If CInt(m_vPartyArray(0, iLoop1)) = v_iTypeID Then
                    r_bInUse = True
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInUse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInUse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckNotInUse
    '
    ' Description:
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckNotInUse() As Integer

        Dim result As Integer = 0
        Dim iNewID As Integer
        Dim bInUse As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the selected partner id
            iNewID = VB6.GetItemData(cboPartner, cboPartner.SelectedIndex)

            ' Only check if it's been changed
            If iNewID <> m_lPartnerID Then
                m_lReturn = CType(CheckInUse(v_iTypeID:=iNewID, r_bInUse:=bInUse), gPMConstants.PMEReturnCode)
                If bInUse Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckNotInUse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckNotInUse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DisplayCaptions
    '
    ' Description:
    '
    ' History: 25/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        frminterface = New frmInterface
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'iPMFunc.GetResData is commented as GetResData is added to the same project
            ' Form's caption

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterface2Title, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab

            SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Labels

            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCodeLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescriptionLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPartner.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartnerLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMain.SelectedIndex = 0
        End If
    End Sub
End Class