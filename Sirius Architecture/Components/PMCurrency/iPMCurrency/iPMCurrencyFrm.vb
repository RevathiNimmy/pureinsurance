Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Public Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDateProperty As Date
    Private m_oGeneral As iPMCurrencyMaintenance.General
    Private m_oBusiness As Object
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_iCurrencyID As Integer
    Private m_lCaptionID As Integer
    Private m_sIsoCode As New FixedLengthString(4)
    Private m_sDescription As New FixedLengthString(255)
    Private m_bIsBase As Boolean
    Private m_sMinorPart As String = ""
    Private m_sCode As New FixedLengthString(10)
    Private m_sSymbol As New FixedLengthString(4)
    Private m_sAlignment As New FixedLengthString(1)
    Private m_iDecimalPlaces As Integer
    Private m_iIsDeleted As Integer
    Private m_dtEffectiveDate As Date
    Private m_sFormatString As New FixedLengthString(255)
    Private m_iRoundToPlaces As Integer

    Private m_iLastRow As Integer
    Private m_iLastValue As Integer = 2 'using for validating DecimalPlaces & RoundToPlaces
    Private newRowNeeded As Boolean
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    Public Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
        Set(ByVal Value As Integer)
            m_lErrorNumber = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    Public ReadOnly Property Status() As Integer
        Get
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

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDateProperty = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property

    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = m_oBusiness.GetDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                Return result
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function BusinessToData() As Integer
        Dim result As Integer = 0
        Dim nRow As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim g_vGridData(13, 0)
            While m_oBusiness.GetNext(vCurrencyID:=m_iCurrencyID, vCaptionID:=m_lCaptionID, vIsoCode:=m_sIsoCode.Value, vDescription:=m_sDescription.Value, vIsBase:=m_bIsBase, vMinorPart:=m_sMinorPart, vCode:=m_sCode.Value, vSymbol:=m_sSymbol.Value, vAlignment:=m_sAlignment.Value, vDecimalPlaces:=m_iDecimalPlaces, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dtEffectiveDate, vFormatString:=m_sFormatString.Value, vRoundToPlaces:=m_iRoundToPlaces) = gPMConstants.PMEReturnCode.PMTrue
                nRow = g_vGridData.GetUpperBound(1)

                g_vGridData(ACGridIsoCode, nRow) = m_sIsoCode.Value
                g_vGridData(ACGridDescription, nRow) = m_sDescription.Value
                g_vGridData(ACGridIsBase, nRow) = m_bIsBase
                g_vGridData(ACGridMinorPart, nRow) = m_sMinorPart
                g_vGridData(ACGridSymbol, nRow) = m_sSymbol.Value
                g_vGridData(ACGridAlignment, nRow) = m_sAlignment.Value
                g_vGridData(ACGridDecimalPlaces, nRow) = m_iDecimalPlaces
                g_vGridData(ACGridIsDeleted, nRow) = m_iIsDeleted
                g_vGridData(ACGridEffectiveDate, nRow) = DateTime.Parse(m_dtEffectiveDate).ToString("d")
                g_vGridData(ACGridFormatString, nRow) = m_sFormatString.Value
                g_vGridData(ACGridRoundToPlaces, nRow) = m_iRoundToPlaces
                g_vGridData(ACGridCurrencyId, nRow) = m_iCurrencyID
                g_vGridData(ACGridCaptionId, nRow) = m_lCaptionID
                g_vGridData(ACGridCode, nRow) = m_sCode.Value
                g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1)) = g_vGridData.GetUpperBound(1) + 1

                ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) + 1)
            End While

            ' Check if we have data in the grid array.
            If Information.IsArray(g_vGridData) Then
                If g_vGridData.GetUpperBound(1) > 0 Then
                    ' Decrement the data array.
                    ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) - 1)
                End If
                ' Store the new index value, for when we wish start
                ' adding data.
                m_oGeneral.NewIndex = g_vGridData.GetUpperBound(1) + 1
                ' Update the approxamate number of rows.
                grdMainData.RowsCount = g_vGridData.GetUpperBound(1)
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function DataToBusiness(ByRef lMode As Integer, ByRef lDataID As Integer) As Integer
        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If String.IsNullOrEmpty(grdMainData.Rows(lDataID).Cells(ACGridIsoCode).Value.ToString()) Or String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridDescription + 1).Value.ToString())) Then
                MessageBox.Show("Codes and description are mandatory." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                     "Please check data and try again.", "iPMCurrencyMaintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                g_ToMessage = True
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_sIsoCode.Value = grdMainData.Rows(lDataID).Cells(ACGridIsoCode).Value.ToString() 'Convert.ToString(g_vGridData(ACGridIsoCode, lDataID))
            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridDescription + 1).Value.ToString())) Then
                m_sDescription.Value = CStr(grdMainData.Rows(lDataID).Cells(ACGridDescription + 1).Value) ' Convert.ToString(g_vGridData(ACGridDescription, lDataID))
            Else
                m_sDescription.Value = ""
            End If

            If String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridIsBase + 1).Value.ToString)) Then
                m_bIsBase = False
            Else
                m_bIsBase = CBool(grdMainData.Rows(lDataID).Cells(ACGridIsBase + 1).Value)
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridMinorPart + 1).Value.ToString())) Then
                m_sMinorPart = CStr((grdMainData.Rows(lDataID).Cells(ACGridMinorPart + 1)).Value)
            Else
                m_sMinorPart = ""
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridSymbol + 1)).Value.ToString) Then
                m_sSymbol.Value = Convert.ToString(grdMainData.Rows(lDataID).Cells(ACGridSymbol + 1).Value)
            Else
                m_sSymbol.Value = ""
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridAlignment + 1).Value.ToString)) Then
                m_sAlignment.Value = grdMainData.Rows(lDataID).Cells(ACGridAlignment + 1).Value.ToString()
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridDecimalPlaces + 1).Value.ToString)) Then
                m_iDecimalPlaces = CInt(grdMainData.Rows(lDataID).Cells(ACGridDecimalPlaces + 1).Value)
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridIsDeleted + 1).Value.ToString)) Then
                If grdMainData.Rows(lDataID).Cells(ACGridIsDeleted + 1).Value.ToString.Trim = "No" Then
                    m_iIsDeleted = 0
                Else
                    m_iIsDeleted = 1
                End If
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridEffectiveDate + 1).Value.ToString)) Then
                m_dtEffectiveDate = CDate(grdMainData.Rows(lDataID).Cells(ACGridEffectiveDate + 1).Value)
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridFormatString + 1).Value.ToString)) Then
                m_sFormatString.Value = Convert.ToString(grdMainData.Rows(lDataID).Cells(ACGridFormatString + 1).Value)
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridRoundToPlaces + 1).Value.ToString)) Then
                m_iRoundToPlaces = CInt(grdMainData.Rows(lDataID).Cells(ACGridRoundToPlaces + 1).Value)
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridCurrencyId + 1).Value.ToString)) Then
                m_iCurrencyID = CInt(grdMainData.Rows(lDataID).Cells(ACGridCurrencyId + 1).Value)
            Else

                m_iCurrencyID = 0
            End If

            If Not String.IsNullOrEmpty((grdMainData.Rows(lDataID).Cells(ACGridCurrencyId + 1).Value.ToString)) Then
                m_lCaptionID = CInt(grdMainData.Rows(lDataID).Cells(ACGridCurrencyId + 1).Value)
            Else
                m_lCaptionID = 0
            End If

            If Not String.IsNullOrEmpty(grdMainData.Rows(lDataID).Cells(ACGridIsoCode).Value) Then
                m_sCode.Value = CStr(grdMainData.Rows(lDataID).Cells(ACGridIsoCode).Value)
            End If

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If
            m_sScreenHierarchy = $"Currency({m_sIsoCode.Value.Trim()})/{m_sDescription.Value.Trim()}"
            ' Check the task.
            Select Case (lMode) '(m_iTask)<-- as generated. Wrong? JY
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    If Not IsDBNull(grdMainData.Rows(lDataID - 1).Cells(14).Value) Then
                        lBusinessDataID = CInt(grdMainData.Rows(lDataID - 1).Cells(14).Value) + 1
                    Else
                        lBusinessDataID = CInt(grdMainData.Rows(m_iLastRow - 1).Cells(14).Value) + 1
                    End If

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vCurrencyID:=m_iCurrencyID, vCaptionID:=m_lCaptionID, vIsoCode:=m_sIsoCode.Value, vDescription:=m_sDescription.Value, vIsBase:=m_bIsBase, vMinorPart:=m_sMinorPart, vCode:=m_sCode.Value, vSymbol:=m_sSymbol.Value, vAlignment:=m_sAlignment.Value, vDecimalPlaces:=m_iDecimalPlaces, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dtEffectiveDate, vFormatString:=m_sFormatString.Value, vRoundToPlaces:=m_iRoundToPlaces, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)
                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    lBusinessDataID = CInt(grdMainData.Rows(lDataID).Cells(14).Value)
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vCurrencyID:=m_iCurrencyID, vCaptionID:=m_lCaptionID, vIsoCode:=m_sIsoCode.Value, vDescription:=m_sDescription.Value, vIsBase:=m_bIsBase, vMinorPart:=m_sMinorPart, vCode:=m_sCode.Value, vSymbol:=m_sSymbol.Value, vAlignment:=m_sAlignment.Value, vDecimalPlaces:=m_iDecimalPlaces, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dtEffectiveDate, vFormatString:=m_sFormatString.Value, vRoundToPlaces:=m_iRoundToPlaces, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)
                Case gPMConstants.PMEComponentAction.PMDelete
                    ' Inform the business object with a deleted data item.
                    lBusinessDataID = CInt(grdMainData.Rows(lDataID - 1).Cells(14).Value)
                    m_lReturn = m_oBusiness.EditDelete(lBusinessDataID, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the data details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness")
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function DisplayLookupDetails() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: SetGridInterfaceDefaults
    '
    ' Description: Sets all of the grid default values.
    '
    ' ***************************************************************** '
    Public Function SetGridInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            gridData = New XArrayHelper
            bPMFunc.TransposeArray(g_vGridData)
            gridData.LoadRows(g_vGridData)
            For Each dr As DataRow In gridData.Rows
                If CInt(dr("Column8")) = 0 Then
                    dr("Column8") = "No"
                Else
                    dr("Column8") = "Yes"
                End If
            Next
            gridData.Columns(2).DefaultValue = False
            gridData.AcceptChanges()
            grdMainData.AutoGenerateColumns = False
            grdMainData.ScrollBars = ScrollBars.Horizontal

            With grdMainData
                .Columns(ACGridIsoCode).DataPropertyName = "Column1"
                .Columns(1).DefaultCellStyle.BackColor = Color.Black
                .Columns(1).Width = 2
                .Columns(1).HeaderCell.Style.BackColor = Color.Black
                .Columns(ACGridDescription + 1).DataPropertyName = "Column2"
                .Columns(ACGridIsBase + 1).DataPropertyName = "Column3"
                .Columns(ACGridMinorPart + 1).DataPropertyName = "Column4"
                .Columns(ACGridSymbol + 1).DataPropertyName = "Column5"
                .Columns(ACGridAlignment + 1).DataPropertyName = "Column6"
                .Columns(ACGridDecimalPlaces + 1).DataPropertyName = "Column7"
                .Columns(ACGridIsDeleted + 1).DataPropertyName = "Column8"
                .Columns(ACGridEffectiveDate + 1).DataPropertyName = "Column9"
                .Columns(ACGridFormatString + 1).DataPropertyName = "Column10"
                .Columns(ACGridRoundToPlaces + 1).DataPropertyName = "Column11"
                .Columns(ACGridCurrencyId + 1).DataPropertyName = "Column12"
                .Columns(ACGridCurrencyId + 1).Visible = False
                .Columns(ACGridCaptionId + 1).DataPropertyName = "Column13"
                .Columns(ACGridCaptionId + 1).Visible = False
                .Columns(ACGridCode + 1).DataPropertyName = "Column14"
                .Columns(ACGridCode + 1).Visible = False
                .DataSource = gridData
                .ReBind()
                .Refresh()
            End With
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the grid defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetGridInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    
    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' Set grid to view mode.
                grdMainData.AllowUserToAddRows = False
                grdMainData.ReadOnly = False
                grdMainData.AllowUserToDeleteRows = False
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            grdMainData.Columns(ACGridIsoCode).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridIsoCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridDescription + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridDescriptionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridIsBase + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridIsBaseCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridMinorPart + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridMinorPartCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridSymbol + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridSymbolCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridAlignment + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridAlignmentCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridDecimalPlaces + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridDecimalPlacesCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridIsDeleted + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridIsDeletedCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridEffectiveDate + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridEffectiveDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridFormatString + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridFormatStringCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            grdMainData.Columns(ACGridRoundToPlaces + 1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGridRoundToPlacesCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            'JK011298

            With uctPMResizer1
                .SetControlResizeOption("grdMainData", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdNavigate", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("imgIcon", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
            End With
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()
        Dim sMessage, sTitle As String

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMCurrency.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMCurrencyMaintenance.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Public Sub frmInterfaceLoad()
        Try
            iPMFunc.ShowFormInTaskBar_Detach()
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.
            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

             m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            grdMainData.RowSel = grdMainData.RowsCount
            grdMainData.ScrollBars = ScrollBars.Both
            m_iLastRow = grdMainData.Rows.Count - 1
        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If UnloadMode <> vbFormCode Then
                grdMainData.UpdateCurrentRow()
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            m_oGeneral = Nothing
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Try
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Call the update method to flush any
            ' new data currently being added.
            'grdMainData_CellEndEdit(grdMainData, New DataGridViewCellEventArgs(grdMainData.CurrentCell.ColumnIndex, 0))
            If (IsDBNull(grdMainData.CurrentRow.Cells(14).Value)) Then
                grdMainData_NewRowNeeded(grdMainData, New DataGridViewRowEventArgs(grdMainData.CurrentRow))
            Else
                grdMainData_CellValuePushed(grdMainData, New DataGridViewCellValueEventArgs(grdMainData.CurrentColumnIndex, grdMainData.CurrentRowIndex))
            End If

            For Each dr As DataRow In gridData.Rows
                If IsDBNull(dr(ACGridIsoCode)) Or IsDBNull(dr(ACGridDescription)) Then
                    If g_ToMessage = False Then
                        MessageBox.Show("Codes and description are mandatory." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                         "Please check data and try again.", "iPMCurrencyMaintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        g_ToMessage = True
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            Next
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Call the update method to flush any
            ' new data currently being added.
            grdMainData.UpdateCurrentRow()

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Function ValidateOK() As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        'Update the array
        Dim bHasIsBase As Boolean = False
        For ILoop As Integer = 0 To gridData.Rows.Count - 1
            If gPMFunctions.NullToBoolean(CStr(gridData.Rows(ILoop)(2))) Then
                bHasIsBase = True
                Exit For
            End If
        Next

        If Not bHasIsBase Then
            MessageBox.Show("At least one currency must be allowed to be a base currency.", "Currency Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    Private Sub grdMainData_DefaultValuesNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles grdMainData.DefaultValuesNeeded
        With grdMainData
            e.Row.Cells(ACGridAlignment + 1).Value = "R"
            e.Row.Cells(ACGridDecimalPlaces + 1).Value = "2"
            e.Row.Cells(ACGridIsDeleted + 1).Value = "No"
            e.Row.Cells(ACGridEffectiveDate + 1).Value = CDate(DateTime.Today).ToShortDateString
            e.Row.Cells(ACGridFormatString + 1).Value = "#,##0.00 ISO"
            e.Row.Cells(ACGridRoundToPlaces + 1).Value = "2"
        End With
    End Sub

    Private Sub grdMainData_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdMainData.CellEndEdit
        Dim vNewVal As String
        Dim bOK As Boolean
        Dim bCheck As Boolean = True
        Try
            If e.RowIndex = 0 AndAlso e.ColumnIndex = 0 Then
                Exit Sub
            End If
            If e.RowIndex <= gridData.Rows.Count - 1 And e.ColumnIndex = ACGridRoundToPlaces + 1 Then
                If Not IsNumeric(grdMainData.Rows(e.RowIndex).Cells(ACGridRoundToPlaces + 1).Value) Then
                    bCheck = False
                    grdMainData.Rows(e.RowIndex).Cells(ACGridRoundToPlaces + 1).Value = m_iLastValue
                    gridData.Rows(e.RowIndex)(ACGridRoundToPlaces) = m_iLastValue
                    gridData.AcceptChanges()
                End If
            End If
            If e.RowIndex <= gridData.Rows.Count - 1 And e.ColumnIndex = ACGridDecimalPlaces + 1 Then
                If Not IsNumeric(grdMainData.Rows(e.RowIndex).Cells(ACGridDecimalPlaces + 1).Value) Then
                    bCheck = False
                    grdMainData.Rows(e.RowIndex).Cells(ACGridRoundToPlaces + 1).Value = m_iLastValue
                    gridData.Rows(e.RowIndex)(ACGridDecimalPlaces) = m_iLastValue
                    gridData.AcceptChanges()
                End If
            End If


            If (e.ColumnIndex = 0) And e.RowIndex > gridData.Rows.Count - 1 Then
                If Not grdMainData.Rows(e.RowIndex).Cells(0).Value Is Nothing Then
                    If Not String.IsNullOrEmpty(grdMainData.Rows(e.RowIndex).Cells(0).Value.ToString) Then
                        vNewVal = CStr(grdMainData.Rows(e.RowIndex).Cells(0).Value).Trim()
                        If vNewVal <> "" Then

                            bOK = True
                            'Does this one already exist?
                            For Each dr As DataRow In gridData.Rows
                                If CStr(dr(0).ToString.Trim().ToUpper() = vNewVal.Trim().ToUpper()) Then
                                    bOK = False
                                End If
                            Next

                            ' Step through all of the columns of the row

                            If Not bOK Then
                                ' Get new Currency Code
                                m_lReturn = CType(m_oGeneral.GetAVal(vNewVal), gPMConstants.PMEReturnCode)

                                If System.Windows.Forms.DialogResult.Yes = MessageBox.Show("Code already exists?" & Strings.Chr(13) & Strings.Chr(10) & " Renamed to " & _
                                                                                           vNewVal, "Currency Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) Then
                                    grdMainData.Rows(e.RowIndex).Cells(0).Value = vNewVal
                                Else
                                    grdMainData.Rows(e.RowIndex).Cells(0).Value = ""
                                End If

                            End If
                        End If

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception
            ' Error Section.
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="grdMainData_CellEndEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub


    Private Sub grdMainData_NewRowNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles grdMainData.NewRowNeeded
        'Dim dr As DataRow = Nothing
        Dim indx As Integer = -1
        If e.Row.Index > 0 Then
            If e.Row.Index > gridData.Rows.Count - 1 Then
                Exit Sub
            End If
            '   dr = gridData.Rows(e.Row.Index)
            For Each dr As DataRow In gridData.Rows
                indx = indx + 1
                If (dr.RowState = DataRowState.Added) Or (dr.RowState = DataRowState.Modified) Then
                    m_lReturn = CType(DataToBusiness(gPMConstants.PMEComponentAction.PMAdd, indx), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                End If
            Next
            gridData.AcceptChanges()
        End If
    End Sub
    Private Function CheckValues(ByVal rowIndex As Integer) As Boolean
        Dim bOk As Boolean = True
        For Each c As DataGridViewCell In grdMainData.Rows(rowIndex).Cells
            If c.ColumnIndex <> 1 And c.ColumnIndex <> 12 And c.ColumnIndex <> 13 And c.ColumnIndex <> 14 Then
                If c.Value Is Nothing Then
                    bOk = False
                End If
            End If
        Next
        Return bOk
    End Function

    Private Sub grdMainData_CellValuePushed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellValueEventArgs) Handles grdMainData.CellValuePushed
        Dim index As Integer = -1
        If e.RowIndex = grdMainData.RowsCount - 1 Then
            Exit Sub
        End If
        For Each dr As DataRow In gridData.Rows
            index = index + 1
            If (dr.RowState = DataRowState.Added) Or (dr.RowState = DataRowState.Modified) Then
                m_lReturn = CType(DataToBusiness(gPMConstants.PMEComponentAction.PMEdit, index), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If
        Next
        gridData.AcceptChanges()
    End Sub
End Class
