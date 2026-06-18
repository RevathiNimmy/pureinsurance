Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Imports Artinsoft.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctPBSearchField_NET.uctPBSearchField")> _
Partial Public Class uctPBSearchField
    Inherits System.Windows.Forms.UserControl

    Private Const PMProcessModeGeneric As Integer = 110

    Private m_lMinimumWidth As Integer
    Private m_lMinimumHeight As Integer
    Private m_vDataArray As Object

    'flag for having found an item to return to the screen
    Private m_bFoundValues As Boolean
    Private m_lReturn As Integer

    Private m_oBusiness As bGISMaintainDataDictionary.Business
    Private m_bIsLoaded As Integer

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    Private m_vResultArray(,) As Object
    Private m_lDataModelType As Integer
    Private m_lDataModelTypeID As Integer
    Private m_sGISModelCode As String = ""
    Private _m_GridArray As XArrayHelper = Nothing
    Private Property m_GridArray() As XArrayHelper
        Get
            If _m_GridArray Is Nothing Then
                _m_GridArray = New XArrayHelper()
            End If
            Return _m_GridArray
        End Get
        Set(ByVal Value As XArrayHelper)
            _m_GridArray = value
        End Set
    End Property

    Event SearchFieldEdited(ByVal Sender As Object, ByVal e As SearchFieldEditedEventArgs)


    Private Function Initialise() As Object




        Dim result As Object = Nothing
        Try
            Me.cboGISDataModel.FirstItem = "(ALL)"
            result = gPMConstants.PMEReturnCode.PMFalse
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACAPP)

            'get business object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGISMaintainDataDictionary.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness


            Return m_lReturn

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cboGISDataModel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISDataModel.Click

        If Not m_bIsLoaded Then
            Exit Sub
        End If

        If cboGISDataModel.ListIndex = 0 Then
            grdSearchCriteria.Visible = False
            lblRiskIndex.Visible = True
            txtRiskIndex.Visible = True
        Else
            grdSearchCriteria.Visible = True
            lblRiskIndex.Visible = False
            txtRiskIndex.Visible = False
            m_lReturn = GetBusiness()
            m_lReturn = DataToInterface()
        End If

        m_lDataModelTypeID = cboGISDataModel.ListIndex
        RaiseEvent SearchFieldEdited(Me, New SearchFieldEditedEventArgs(""))
    End Sub

    Private Sub grdSearchCriteria_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdSearchCriteria.CellEndEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex

        'm_GridArray(grdSearchCriteria.Bookmark, ColIndex) = grdSearchCriteria.Columns(ColIndex).Text
        'm_GridArray(grdSearchCriteria.CurrentRowIndex, ColIndex) = grdSearchCriteria.Columns(ColIndex).ToString()
    End Sub


    Private Sub grdSearchCriteria_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdSearchCriteria.ButtonClick
        Dim ColIndex As Integer = eventArgs.ColumnIndex

        Dim lPartyCnt As Integer
        Dim sPartyname As String = ""


        'developer guide no.53(no solution)
        'If CDbl(m_vResultArray(ACSpecialType, grdSearchCriteria.Bookmark)) = GISSharedPropertyConstants.ACOPartyTypeID And ColIndex = kTDBGridCol_Value Then

        m_lReturn = SelectParty(r_lPartyCnt:=lPartyCnt, r_vResolvedName:=sPartyname, v_sSpecialParty:="IN")


        ' developer guide no.53(no solution)
        'm_GridArray(grdSearchCriteria.Bookmark, ColIndex) = sPartyname
        Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
        grdSearchCriteria.DataSource = bindingSource
        grdSearchCriteria.ReBind()
        grdSearchCriteria.Refresh()
        'End If
    End Sub

    Private Sub grdSearchCriteria_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdSearchCriteria.CellEnter
        Dim LastRow As DataGridViewRow = Nothing
        Dim LastCol As Integer = -1
        If Not IsNothing(grdSearchCriteria.PreviousCell) Then
            If grdSearchCriteria.PreviousCell.RowIndex > grdSearchCriteria.Rows.Count Then
                LastRow = grdSearchCriteria.Rows(grdSearchCriteria.PreviousCell.RowIndex)
            End If
            LastCol = grdSearchCriteria.PreviousCell.ColumnIndex
        End If

        Dim sTableName As String = ""
        Dim oCol As DataGridViewColumn

        With grdSearchCriteria
            If LastCol = kTDBGridCol_Value And Information.IsArray(m_vResultArray) Then

                'developer guide no.53(no solution)
                'If CDbl(m_vResultArray(ACDataType, gPMFunctions.ToSafeLong(.Bookmark))) = iGISSharedConstants.GISDataTypeDate Then
                If CDbl(m_vResultArray(ACDataType, gPMFunctions.ToSafeLong(LastRow))) = iGISSharedConstants.GISDataTypeDate Then
                    'developer guide no.55(no solution)
                    'CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).ButtonImage = -1
                    'developer guide no.198
                    '.Columns(LastCol).Button = False
                    'CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).Button = False
                    CType(.Columns(LastCol), DataGridViewExtendedColumn).Button = False
                    'CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DropDownList = False
                    CType(.Columns(LastCol), DataGridViewExtendedColumn).DropDownList = False
                    CType(.Columns(LastCol), DataGridViewExtendedColumn).DisplayValues.Clear()


                    oCol = .Columns(LastCol)
                    cboDate.Left = .Left + oCol.DataGridView.GetColumnDisplayRectangle(oCol.Index, False).Left
                    'developer guide no.53(no solution)
                    'cboDate.Top = .Top + .RowTop(CShort(.CurrentRowIndex))
                    cboDate.Width = oCol.Width
                    cboDate.Height = .RowTemplate.Height
                    cboDate.Visible = True

                    'developer guide no.53(no solution)
                    'ElseIf CDbl(m_vResultArray(ACSpecialType, gPMFunctions.ToSafeLong(.Bookmark))) = GISSharedPropertyConstants.ACOPMLookupTableName Then
                ElseIf CDbl(m_vResultArray(ACSpecialType, gPMFunctions.ToSafeLong(LastRow))) = GISSharedPropertyConstants.ACOPMLookupTableName Then


                    'developer guide no.53(no solution)
                    'sTableName = gPMFunctions.ToSafeString(CInt(m_vResultArray(ACSpecialTypeRef, gPMFunctions.ToSafeLong(.Bookmark))))
                    CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues.Clear()
                    m_lReturn = GetLookupValues(CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues, sTableName)
                    'developer guide no.55(no solution)
                    'CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).ButtonImage = -1
                    'developer guide no.198
                    '.Columns(LastCol).Button = False
                    CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).Button = False
                    CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DropDownList = True

                    cboDate.Visible = False
                    ' developer guide no.53(no solution)
                    ' ElseIf gPMFunctions.ToSafeLong(CInt(m_vResultArray(ACSpecialType, gPMFunctions.ToSafeLong(.Bookmark)))) = GISSharedPropertyConstants.ACOPartyTypeID Then
                ElseIf gPMFunctions.ToSafeLong(CInt(m_vResultArray(ACSpecialType, gPMFunctions.ToSafeLong(LastRow)))) = GISSharedPropertyConstants.ACOPartyTypeID Then

                    CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).DisplayValues.Clear()
                    CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).ButtonImage = pctDots.Image
                    'developer guide no.198
                    '.Columns(LastCol).Button = True
                    CType(.Columns(LastCol), Artinsoft.Windows.Forms.DataGridViewExtendedColumn).Button = True
                    cboDate.Visible = False

                    ' developer guide no.53(no solution)
                Else
                    'CType(.Columns(LastCol), DataGridViewExtendedColumn).DisplayValues.Clear()
                    'developer guide no.198
                    '.Columns(LastCol).Button = False
                    'CType(.Columns(LastCol), DataGridViewExtendedColumn).Button = False
                    'CType(.Columns(LastCol), DataGridViewExtendedColumn).DropDownList = False
                    ' CType(.Columns(LastCol), DataGridViewExtendedColumn).ValueType = dbgNormal
                    'cboDate.Visible = False
                    'developer guide no.53(no solution)
                End If
            End If
        End With

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtRiskIndex_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRiskIndex.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        RaiseEvent SearchFieldEdited(Me, New SearchFieldEditedEventArgs(txtRiskIndex.Text))
    End Sub

    Private Sub uctPBSearchField_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Dim vRiskIndex As String = ""

        If VB6.PixelsToTwipsY(MyBase.Height) < m_lMinimumHeight Then
            MyBase.Height = VB6.TwipsToPixelsY(m_lMinimumHeight)
        End If

        If VB6.PixelsToTwipsX(MyBase.Width) < m_lMinimumWidth Then
            MyBase.Width = VB6.TwipsToPixelsX(m_lMinimumWidth)
        End If

        'set to default size
        fraSearch.Height = MyBase.Height
        fraSearch.Width = MyBase.Width

        grdSearchCriteria.Width = VB6.PixelsToTwipsX(fraSearch.Width) - 300
        grdSearchCriteria.Height = VB6.PixelsToTwipsY(fraSearch.Height) - 800

        m_lMinimumWidth = 5340
        m_lMinimumHeight = 1950

    End Sub

    Public Sub Load_Renamed()

        Try
            If m_bIsLoaded Then
                Exit Sub
            End If


            m_lReturn = CInt(Initialise())
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Exit Sub
            End If
            m_lReturn = SetInterfaceDefaults()

            m_bIsLoaded = True

        Catch excep As System.Exception


            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start", vApp:=ACAPP, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"

        Try


            m_lReturn = m_oBusiness.GetDataModelSearchFields(v_lGisDataModelID:=gPMFunctions.ToSafeLong(cboGISDataModel.ItemId), r_vResultArray:=m_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("SearchFields control not configured", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("No search field available in '" & gPMFunctions.ToSafeString(cboGISDataModel.ItemCaption) & "' Data Model", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"

        Try



            If Information.IsArray(m_vResultArray) Then
                m_lReturn = ArrayToXArray()
            Else
                m_GridArray.RedimXArray(New Integer() {-1, 6}, New Integer() {0, 0})
            End If

            Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
            grdSearchCriteria.DataSource = bindingSource
            grdSearchCriteria.ReBind()
            grdSearchCriteria.Refresh()



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lWidth As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_GridArray = New XArrayHelper()
            m_GridArray.RedimXArray(New Integer() {-1, 6}, New Integer() {0, 0})

            lWidth = CInt((VB6.TwipsToPixelsX(grdSearchCriteria.Width) - 200) / 2)
            'grdSearchCriteria.Columns(kTDBGridCol_PropertName).Width = lWidth
            'grdSearchCriteria.Columns(kTDBGridCol_Value).Width = lWidth

            With grdSearchCriteria

                Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
                .DataSource = bindingSource
                .ReBind()
                .Refresh()

                .Columns(kTDBGridCol_GISPropertyId).Visible = False
                .Columns(kTDBGridCol_PropertName).HeaderText = "Field"
                .Columns(kTDBGridCol_PropertName).ReadOnly = True
                .Columns(kTDBGridCol_PropertName).Visible = True
                .Columns(kTDBGridCol_PropertName).Width = lWidth
                .Columns(kTDBGridCol_Value).HeaderText = "Search Value"
                .Columns(kTDBGridCol_Value).Width = lWidth
                .Columns(kTDBGridCol_Value).Visible = True
                .Columns(kTDBGridCol_GISObjectId).Visible = False
                .Columns(kTDBGridCol_DataType).Visible = False
                .Columns(kTDBGridCol_SpecialType).Visible = False
                .Columns(kTDBGridCol_SpecialTypeRef).Visible = False

            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACAPP, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SelectParty
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef r_lPartyCnt As Object, ByRef r_vResolvedName As Object, Optional ByVal v_sSpecialParty As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectParty"


        ' developer guide no.108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
      
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            'Set oFindParty = CreateObject("iPMBFindParty.Interface")

            'Set appropriate key if agent only
            If (Not False) And (v_sSpecialParty <> "") Then

                Dim vKeyArray(1, 0) As Object

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = v_sSpecialParty


                m_lReturn = oFindParty.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If


            m_lReturn = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            oFindParty.CallingAppName = ACAPP


            m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            oFindParty.NotEditable = 1


            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then



                r_lPartyCnt = oFindParty.PartyCnt

                '       If (IsMissing(vName) <> True) Then
                '           vName = oFindParty.LongName
                '       End If
                '       r_vResolvedName = oFindParty.ResolvedName



                r_vResolvedName = oFindParty.longname
                'r_vResolvedName = oFindParty.ShortCode
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            oFindParty.Dispose()
        End Try
        Return result
    End Function


    <Browsable(True)> _
    Public Property DataModelType() As Integer
        Get
            Return m_lDataModelType
        End Get
        Set(ByVal Value As Integer)
            m_lDataModelType = Value
            cboGISDataModel.WhereClause = "gis_data_model_type_id = " & Value
            cboGISDataModel.Refresh()
        End Set
    End Property

    ''' <summary>
    ''' SearchFields
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property SearchFields() As Collection
        Get

            Const kGrdArrGISObjectTableName As Integer = 7
            Const kArrGISObjectTableName As Integer = 6
            Const kArrGISDataModelCode As Integer = 8

            Dim oSearchFields As New Collection
            Dim oGisSearchFields(,) As Object

            'For each
            'When user uses the Enter Key, FindNow_Click is called before grdSearchCriteria_AfterColEdit

            If (grdSearchCriteria.CurrentCell IsNot Nothing) Then
                grdSearchCriteria_CellEndEdit(grdSearchCriteria, New DataGridViewCellEventArgs(grdSearchCriteria.CurrentCell.ColumnIndex, 0))
            End If

            For iRow As Integer = m_GridArray.GetLowerBound(0) To m_GridArray.GetUpperBound(0)

                ReDim Preserve oGisSearchFields(8, iRow)

                oGisSearchFields(kTDBGridCol_GISPropertyId, iRow) = gPMFunctions.ToSafeLong(m_GridArray(iRow, kTDBGridCol_GISPropertyId))

                oGisSearchFields(kTDBGridCol_PropertName, iRow) = gPMFunctions.ToSafeString(m_GridArray(iRow, kTDBGridCol_PropertName))

                oGisSearchFields(kTDBGridCol_Value, iRow) = gPMFunctions.ToSafeString(m_GridArray(iRow, kTDBGridCol_Value))

                oGisSearchFields(kTDBGridCol_GISObjectId, iRow) = gPMFunctions.ToSafeLong(m_GridArray(iRow, kTDBGridCol_GISObjectId))

                oGisSearchFields(kTDBGridCol_DataType, iRow) = gPMFunctions.ToSafeLong(m_GridArray(iRow, kTDBGridCol_DataType))

                oGisSearchFields(kTDBGridCol_SpecialType, iRow) = gPMFunctions.ToSafeLong(m_GridArray(iRow, kTDBGridCol_SpecialType))

                oGisSearchFields(kTDBGridCol_SpecialTypeRef, iRow) = gPMFunctions.ToSafeString(m_GridArray(iRow, kTDBGridCol_SpecialTypeRef))

                If Information.IsArray(m_vResultArray) Then

                    oGisSearchFields(kGrdArrGISObjectTableName, iRow) = gPMFunctions.ToSafeString(m_vResultArray(kArrGISObjectTableName, iRow))
                End If

                oGisSearchFields(kArrGISDataModelCode, iRow) = gPMFunctions.ToSafeString(cboGISDataModel.ItemCode).Trim()

                oSearchFields.Add(oGisSearchFields, "R" & iRow)

            Next iRow

            Return oSearchFields

        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property RiskIndex() As String
        Get
            Return txtRiskIndex.Text
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property DataModelTypeID() As Integer
        Get
            Return m_lDataModelTypeID
        End Get
    End Property



    <Browsable(True)> _
    Public Property GISModelCode() As String
        Get
            Return m_sGISModelCode
        End Get
        Set(ByVal Value As String)
            m_sGISModelCode = Value
        End Set
    End Property

    Private Function ArrayToXArray() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ArrayToXArray"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_GridArray.Clear()
            m_GridArray.RedimXArray(New Integer() {m_vResultArray.GetUpperBound(1), kTDBGridCol_UPPER}, New Integer() {m_vResultArray.GetLowerBound(1), kTDBGridCol_LOWER})

            For lRow As Integer = m_vResultArray.GetLowerBound(1) To m_vResultArray.GetUpperBound(1)
                m_GridArray(lRow, kTDBGridCol_GISPropertyId) = gPMFunctions.ToSafeLong(m_vResultArray(ACGISPropertyId, lRow))
                m_GridArray(lRow, kTDBGridCol_PropertName) = gPMFunctions.ToSafeString(m_vResultArray(ACPropertName, lRow))
                m_GridArray(lRow, kTDBGridCol_Value) = ""
                m_GridArray(lRow, kTDBGridCol_GISObjectId) = gPMFunctions.ToSafeLong(m_vResultArray(ACGISObjectId, lRow))
                m_GridArray(lRow, kTDBGridCol_DataType) = gPMFunctions.ToSafeString(m_vResultArray(ACDataType, lRow))
                m_GridArray(lRow, kTDBGridCol_SpecialType) = gPMFunctions.ToSafeString(m_vResultArray(ACSpecialType, lRow))
                m_GridArray(lRow, kTDBGridCol_SpecialTypeRef) = gPMFunctions.ToSafeString(m_vResultArray(ACSpecialTypeRef, lRow))
            Next lRow


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


    Private Function GetLookupValues(ByRef r_oValueItems As Artinsoft.Windows.Forms.ValueCollection, ByVal v_sTableName As String) As Integer
        Dim result As Integer = 0


        Dim vTableArray(,) As Object
        Dim sCaption, sID As String


        Dim vLookupItems(,) As Object

        Dim oPMLookup As bPMLookup.Business
        Dim oValueItem As New Artinsoft.Windows.Forms.DisplayValue

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Business
            Dim temp_oPMLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookup = temp_oPMLookup

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Format the Input Array
            ReDim vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0)

            ' Set the Table Name

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = v_sTableName

            ' Get the lookup values

            m_lReturn = oPMLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllWithDeleted, vTableArray:=vTableArray, iLanguageID:=1, dtEffectiveDate:=DateTime.Now, vResultArray:=vLookupItems)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetLookupValues")
                Return result
            End If

            ' Add to the drop down list box.
            ' As we are only working with one Lookup table at a time,
            ' we do not need to bother with the start and no of items
            If Information.IsArray(vLookupItems) Then

                For lRow As Integer = vLookupItems.GetLowerBound(1) To vLookupItems.GetUpperBound(1)

                    ' extract details from result set

                    sCaption = CStr(vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lRow))

                    sID = CStr(vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lRow))

                    ' add entry
                    oValueItem.Value = CInt(sID)
                    oValueItem.Key = sCaption
                    r_oValueItems.Add(oValueItem)

                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetLookupValues", excep:=excep)

            Return result

        End Try
    End Function


    Public Function ClearInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ClearInterface"

        Try



            cboGISDataModel.ItemId = 0
            txtRiskIndex.Text = ""


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

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (Not mentioned in the spec)
    Public Sub RiskIndex_setfocus()
        txtRiskIndex.Focus()
    End Sub
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (Not mentioned in the spec)
End Class
