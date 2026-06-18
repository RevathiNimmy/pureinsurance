Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    Private Const ACClass As String = "frmDetails"

    Private m_lReturn As Integer
    Private m_lStatus As Integer
    Private m_iPartyRelationshipGroupId As Integer

    'developer guide no. 50
    Dim frminterface As frmInterface
    Private m_vDataArray As Object
    Private m_vPartyArray As Object

    Private m_vDocsAvailable(,) As Object
    Private m_vDocsSuppressed As Array

    Private m_sAgencyOrUnderwriting As String = ""

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRPartyAG.Business

    ' Form Fields
    Private m_oFormField As iPMFormControl.FormFields

    ' *** Public Properties (Begin) *** '
    'developer giude no. 101
    Public WriteOnly Property DocsAvailable() As Object
        Set(ByVal Value As Object)
            m_vDocsAvailable = Value
        End Set
    End Property
    Public Property DocsSuppressed() As Object
        Get
            Return m_vDocsSuppressed
        End Get
        Set(ByVal Value As Object)
            m_vDocsSuppressed = Value
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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_vDocsAvailable) Then
                lstDocsToChoose.Items.Clear()
                ' Assign the details to the interface.
                For i As Integer = m_vDocsAvailable.GetLowerBound(1) To m_vDocsAvailable.GetUpperBound(1)
                    If CStr(m_vDocsAvailable(1, i)) <> "" Then
                        Dim lstDocsToChoose_NewIndex As Integer = -1
                        lstDocsToChoose_NewIndex = lstDocsToChoose.Items.Add(CStr(m_vDocsAvailable(1, i)).Trim())
                        VB6.SetItemData(lstDocsToChoose, lstDocsToChoose_NewIndex, Conversion.Val(CStr(m_vDocsAvailable(0, i))))
                    End If
                Next

            End If

            'Move choosen docs over to right hand listbox
            PopulateSuppressedDocs()

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

            'TODOm_lReturn& = m_oFormField.AddNewFormField( _
            'ctlControl:=txtCode, _
            'lFormat:=PMFormatString, _
            'lFieldType:=PMString, _
            'lMandatory:=PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TODOm_lReturn& = m_oFormField.AddNewFormField( _
            'ctlControl:=txtDescription, _
            'lFormat:=PMFormatString, _
            'lFieldType:=PMString, _
            'lMandatory:=PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TODOm_lReturn& = m_oFormField.AddNewFormField( _
            'ctlControl:=cboPartner, _
            'lFormat:=PMFormatString, _
            'lFieldType:=PMString, _
            'lMandatory:=PMMandatory)
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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyAG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

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
            m_lReturn = SetFieldValidation()
            m_sAgencyOrUnderwriting = m_oBusiness.UnderwritingOrAgency
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Display the interface details
            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
                End If
                m_oFormField = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' PW150702
    Private Sub cmdAddDocs_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddDocs.Click

        Dim i As Integer

        Do Until i >= lstDocsToChoose.Items.Count
            If ListBoxHelper.GetSelected(lstDocsToChoose, i) Then
                Dim lstDocsChosen_NewIndex As Integer = -1
                lstDocsChosen_NewIndex = lstDocsChosen.Items.Add(VB6.GetItemString(lstDocsToChoose, i))
                VB6.SetItemData(lstDocsChosen, lstDocsChosen_NewIndex, VB6.GetItemData(lstDocsToChoose, i))
                lstDocsToChoose.Items.RemoveAt(CShort(i))
            Else
                i += 1
            End If
        Loop

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

        ' Set the properties
        m_lReturn = InterfaceToProperties()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Exit out
        Me.Hide()

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

            ' PW150702 - put the suppressed docs into an array
            If lstDocsChosen.Items.Count > 0 Then
                m_vDocsSuppressed = Array.CreateInstance(GetType(Object), New Integer() {1, lstDocsChosen.Items.Count}, New Integer() {0, 0})
                For i As Integer = 0 To lstDocsChosen.Items.Count - 1
                    m_vDocsSuppressed(0, i) = VB6.GetItemData(lstDocsChosen, i)
                Next
            Else
                m_vDocsSuppressed = Array.CreateInstance(GetType(Object), New Integer() {1, 1}, New Integer() {0, 0})
            End If
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
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets defaults for adding a new record
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Set the captions
            m_lReturn = DisplayCaptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateSuppressedDocs
    '
    ' Description: Fills the list with suppressed documents
    ' PW150702 - created
    '
    ' ***************************************************************** '
    Private Sub PopulateSuppressedDocs()

        Dim lProcessType As Integer

        Try

            If Not Information.IsArray(m_vDocsSuppressed) Then
                Exit Sub
            End If

            lstDocsChosen.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vDocsSuppressed.GetLowerBound(1) To m_vDocsSuppressed.GetUpperBound(1)
                ' Store the process type
                lProcessType = CInt(m_vDocsSuppressed(0, i))
                ' Remove the process type from the available docs and move it to the chosen docs
                For j As Integer = 0 To lstDocsToChoose.Items.Count - 1
                    If VB6.GetItemData(lstDocsToChoose, j) = lProcessType Then
                        Dim lstDocsChosen_NewIndex As Integer = -1
                        lstDocsChosen_NewIndex = lstDocsChosen.Items.Add(VB6.GetItemString(lstDocsToChoose, j))
                        VB6.SetItemData(lstDocsChosen, lstDocsChosen_NewIndex, lProcessType)
                        lstDocsToChoose.Items.RemoveAt(CShort(j))
                        Exit For
                    End If
                Next j
            Next i

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateSuppressedDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub
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

            ' Form's caption

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab

            SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Labels
            'TODO add labels to resource file

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PW150702
    Private Sub cmdRemoveDocs_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveDocs.Click

        Dim i As Integer

        Do Until i >= lstDocsChosen.Items.Count
            If ListBoxHelper.GetSelected(lstDocsChosen, i) Then
                Dim lstDocsToChoose_NewIndex As Integer = -1
                lstDocsToChoose_NewIndex = lstDocsToChoose.Items.Add(VB6.GetItemString(lstDocsChosen, i))
                VB6.SetItemData(lstDocsToChoose, lstDocsToChoose_NewIndex, VB6.GetItemData(lstDocsChosen, i))
                lstDocsChosen.Items.RemoveAt(CShort(i))
            Else
                i += 1
            End If
        Loop

    End Sub


    Private Sub lstDocsChosen_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstDocsChosen.DoubleClick

        cmdRemoveDocs_Click(cmdRemoveDocs, New EventArgs())

    End Sub


    Private Sub lstDocsToChoose_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstDocsToChoose.DoubleClick

        cmdAddDocs_Click(cmdAddDocs, New EventArgs())

    End Sub

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMain.SelectedIndex = 0
        End If
    End Sub
End Class
