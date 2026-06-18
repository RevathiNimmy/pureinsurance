Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    Private Const ACClass As String = "frmDetails"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lStatus As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lDeleteClientId As Integer
    Private m_iTask As Integer
    Private m_bInUse As Boolean
    'Developer Guide No. 17
    Private m_vDataArray As Object

    ' Form Fields
    Private m_oFormField As iPMFormControl.FormFields

    ' *** Public Properties (Begin) *** '

    Public WriteOnly Property InUse() As Boolean
        Set(ByVal Value As Boolean)
            m_bInUse = Value
        End Set
    End Property

    Public WriteOnly Property DataArray() As Object(,)
        Set(ByVal Value As Object(,))
            m_vDataArray = Value
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
    Public Property DeleteClientID() As Integer
        Get
            Return m_lDeleteClientId
        End Get
        Set(ByVal Value As Integer)
            m_lDeleteClientId = Value
        End Set
    End Property

    ' *** Public Properties (End) *** '


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 28/08/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise form control
            m_oFormField = New iPMFormControl.FormFields()

            m_lReturn = CType(m_oFormField, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMFormControl.FormFields", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
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
    ' History: 28/08/2002 CMG/PB - Created.
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


            End If
        End If
        Me.disposedValue = True
    End Sub



    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Cancel
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Exit out
        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click


        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        ' Set the properties
        m_lReturn = CType(InterfaceToProperties(), gPMConstants.PMEReturnCode)
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
    ' History: 28/08/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '
    Private Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lead Client
            m_lDeleteClientId = Convert.ToString(lvwClients.FocusedItem.Tag)

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
    ' History: 28/08/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the captions
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Ensure we have an array of clients
            If Information.IsArray(m_vDataArray) Then
                ' Get array bounds
                lLower = m_vDataArray.GetLowerBound(1)
                lUpper = m_vDataArray.GetUpperBound(1)

                ' Walk the array
                For lCount As Integer = lLower To lUpper
                    ' Create the listitem (with shortname), but dont show the current lead
                    If CDbl(m_vDataArray(0, lCount)) <> m_lDeleteClientId Then
                        With lvwClients.Items.Add(CStr(m_vDataArray(3, lCount)).Trim())
                            ' Set party_cnt for matching later and duplicate checks

                            .Tag = CStr(m_vDataArray(0, lCount))

                            ' Set item properties
                            ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vDataArray(3, lCount)).Trim()), 1).Text = CStr(m_vDataArray(4, lCount)).Trim()
                            ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vDataArray(3, lCount)).Trim()), 2).Text = CStr(m_vDataArray(5, lCount)).Trim()
                            ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vDataArray(3, lCount)).Trim()), 3).Text = IIf(CBool(CStr(m_vDataArray(1, lCount)).Trim()), "Y", "N")
                            ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vDataArray(3, lCount)).Trim()), 4).Text = IIf(CBool(CStr(m_vDataArray(2, lCount)).Trim()), "Y", "N")
                        End With
                    End If
                Next

                m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwClients.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Select the first item.
            lvwClients.Items.Item(0).Selected = True


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
    ' Name: DisplayCaptions
    '
    ' Description:
    '
    ' History: 28/08/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '
    Public Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Labels

            lblInstruct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPSetLead, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Set listview column headers


            lvwClients.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPLVCol0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwClients.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPLVCol1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwClients.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPLVCol2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwClients.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPLVCol3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwClients.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCPLVCol4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: lvwClients_ColumnClick
    '
    ' Description: Sorts the list view when column headers are clicked
    '
    ' History: 28/08/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '

    Private Sub lvwClients_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwClients.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwClients.Columns(eventArgs.Column)

        Try

            ' Check current sort order
            If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwClients) Then
                ' Current column, reverse order
                ListViewHelper.SetSortOrderProperty(lvwClients, (ListViewHelper.GetSortOrderProperty(lvwClients) + 1) Mod 2)
            Else
                ' Different column, change ordering
                ListViewHelper.SetSortedProperty(lvwClients, False)
                ListViewHelper.SetSortOrderProperty(lvwClients, SortOrder.Ascending)
                ListViewHelper.SetSortKeyProperty(lvwClients, ColumnHeader.Index + 1 - 1)
                ListViewHelper.SetSortedProperty(lvwClients, True)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwFees_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    Private Sub lvwClients_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwClients.DoubleClick
        'Call the OK event
        cmdOK_Click(cmdOK, New EventArgs())
    End Sub
End Class