Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Collections.Generic
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctRiskRIControl_NET.uctRiskRIControl")> _
Partial Public Class uctRiskRIControl
    Inherits System.Windows.Forms.UserControl

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctRiskRIControl"


    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    ' Refresh treaty tax amounts
    Public Event RecalculateTreatyTax(ByVal Sender As Object, ByRef e As RecalculateTreatyTaxEventArgs)

    ' Refresh facultative tax amounts
    Public Event RecalculateFacTax(ByVal Sender As Object, ByRef e As RecalculateFacTaxEventArgs)



    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    Private m_bIsDirty As Boolean
    Private m_bReadOnly As Boolean
    Private m_bAgency As Boolean

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Internal object for ri arrangement information
    Private m_oRIArrangement As RiskRIArrangement
    Private m_oRIOriginal As RiskRIArrangement

    ' Internal array for grid population
    Private m_oXA As XArrayHelper

    ' Flag to say we have edited a cell
    Private m_bCellChanged As Boolean

    ' RI array pointers
    Private m_bRIEmpty As Boolean
    Private m_lRetained As Integer
    Private m_lASumBand As Integer
    Private m_lASumTreaty As Integer
    Private m_lASumFAC As Integer
    Private m_lASumTotal As Integer
    Private m_lASumAlloc As Integer
    Private m_lASumUnalloc As Integer
    Private m_lASumOriginal As Integer
    Private m_lASumNet As Integer
    Private m_lunAllocatedRI As Decimal
    Private m_lUnallocatedPremium As Decimal
    '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
    Private m_lASumObligatoryTreaty As Integer

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return grdRI.Enabled
        End Get
        Set(ByVal Value As Boolean)
            grdRI.Enabled = Value
        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property Agency() As Boolean
        Set(ByVal Value As Boolean)
            m_bAgency = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property IsDirty() As Boolean
        Get
            Return m_bIsDirty
        End Get
    End Property
    'Collection that will hold the Column Indices for which style is not to be applied.
    'Dim lstColumns As New ArrayList
    <Browsable(True)> _
    Public Property ReadOnly_Renamed() As Boolean
        Get
            Return m_bReadOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value

            ' If we can't edit turn of per cell formatting

            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium).ReadOnly = Not m_bReadOnly

            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent).ReadOnly = Not m_bReadOnly

            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode).ReadOnly = Not m_bReadOnly

            'If Not m_bReadOnly Then
            '    If Not lstColumns.Contains(RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) Then
            '        lstColumns.Add(RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium)
            '    End If
            '    If Not lstColumns.Contains(RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) Then
            '        lstColumns.Add(RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent)
            '    End If
            '    If Not lstColumns.Contains(RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode) Then
            '        lstColumns.Add(RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode)
            '    End If
            'End If

            ' Set base styles
            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured).Style = IIf(m_bReadOnly, "RightAlignLocked", "RightAlign")
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium).Style = IIf(m_bReadOnly, "RightAlignLocked", "RightAlign")
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent).Style = IIf(m_bReadOnly, "RightAlignLocked", "RightAlign")
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            'grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode).Style = IIf(m_bReadOnly, "Locked", "")
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode).ReadOnly = m_bReadOnly
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property UnallocatedRI() As Decimal
        Get
            Return m_lunAllocatedRI
        End Get
    End Property
    Public ReadOnly Property UnallocatedPremium() As Decimal
        Get
            Return m_lUnallocatedPremium
        End Get
    End Property

    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '

    Public Function AddFacultative(ByVal lPartyCnt As Integer, ByVal sDescription As String, ByVal dCommission As Double) As Integer

        Dim result As Integer = 0
        Dim lPriority As Integer
        Dim cPremiumTax, cCommTax As Decimal

        Dim lReturn As Integer
        Const kMethodName As String = "AddFacultative"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set defaults
            lPriority = 65536

            ' Check for duplicate and get priority
            For lCount As Integer = m_lASumTreaty + 1 To m_lASumFAC - 1
                ' Validate
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPartyCnt) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPartyCnt) = lPartyCnt Then
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                    Return result
                End If

                ' Check priority
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority) > lPriority Then
                    lPriority = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority)
                End If
            Next

            ' Get tax values (they may be value based)
            Dim objArgs As RecalculateFacTaxEventArgs = New RecalculateFacTaxEventArgs(0, lPartyCnt, 0, 0, cPremiumTax, cCommTax)
            RaiseEvent RecalculateFacTax(Me, objArgs)

            ' Insert new row
            'lReturn = m_oXA.InsertRows(m_lASumFAC, 1)
            'If lReturn <> 1 Then
            '    gPMFunctions.RaiseError("m_oXA.InsertRows", "Unable to insert new facultative row")
            'End If
            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumFAC)
            m_oXA.AppendRows()
            ' Populate new line
            ' Grid fields
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = sDescription
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIDefaultShare) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRITax) = objArgs.cPremiumTax
            'm_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) = (dCommission / 100).ToString("P2")
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) = dCommission / 100
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRICommTax) = objArgs.cCommTax
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode) = ""
            ' Supporting fields
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRILineID) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "F"

            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID) = Nothing
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIPartyCnt) = lPartyCnt
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority) = lPriority + 1
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRILines) = 1
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRILineLimit) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremiumPercent) = 0
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsCommissionModified) = 0

            ' Increment total lines...
            m_lASumFAC += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            ' Rebind data
            'grdRI.ReBind()
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function AddTreaty(ByVal lTreatyID As Integer, ByVal sDescription As String, ByVal dCommission As Double, ByVal sAgreementCode As String, ByVal bIsRetained As Boolean) As Integer

        Dim result As Integer = 0
        Dim lPriority As Integer
        Dim cPremiumTax, cCommTax As Decimal

        Dim lReturn As Integer
        Const kMethodName As String = "AddTreaty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set defaults
            lPriority = 0

            ' Check for duplicate and get priority
            For lCount As Integer = m_lASumBand + 1 To m_lASumTreaty - 1
                ' Validate
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID) = lTreatyID Then
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                    Return result
                End If

                ' Check priority
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority) > lPriority Then
                    lPriority = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority)
                End If
            Next

            ' Get tax values (they may be value based)
            Dim objTreatyTaxEventArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(0, lTreatyID, 0, 0, cPremiumTax, cCommTax)
            RaiseEvent RecalculateTreatyTax(Me, objTreatyTaxEventArgs)

            ' Insert new row
            'lReturn = m_oXA.InsertRows(m_lASumTreaty, 1)
            'If lReturn <> 1 Then
            '    gPMFunctions.RaiseError("m_oXA.InsertRows", "Unable to insert new treaty row")
            'End If
            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumTreaty)
            m_oXA.AppendRows()
            ' Populate new line
            ' Grid fields
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = sDescription
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIDefaultShare) = 1
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRITax) = objTreatyTaxEventArgs.cPremiumTax
            'm_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) = (dCommission / 100).ToString("P2")
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) = dCommission / 100
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRICommTax) = objTreatyTaxEventArgs.cCommTax
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode) = sAgreementCode
            ' Supporting fields
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRILineID) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = IIf(bIsRetained, "R", "T")
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID) = lTreatyID

            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIPartyCnt) = Nothing
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority) = lPriority + 1
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRILines) = 1
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRILineLimit) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremiumPercent) = 0
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsCommissionModified) = 0

            ' Increment total lines...
            m_lASumTreaty += 1
            m_lASumFAC += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            ' Rebind data
            'grdRI.ReBind()

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Public Function Clear() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "Clear"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear all row references
            m_lASumBand = 0
            m_lASumTreaty = 0
            m_lASumFAC = 0
            m_lASumTotal = 0
            m_lASumAlloc = 0
            m_lASumUnalloc = 0
            m_lASumOriginal = 0
            m_lASumNet = 0
            '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
            m_lASumObligatoryTreaty = 0

            ' Clear grid
            grdRI.Close()
            m_bIsDirty = False

            ' Clear XArray
            If Not (m_oXA Is Nothing) Then
                m_oXA.Clear()
                m_oXA = Nothing
            End If

            ' Release other objects
            m_oRIArrangement = Nothing
            m_oRIOriginal = Nothing

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Sub FinaliseEdit()

        Try

            ' Certain function won't trigger an edit to commit. Force this by changing column
            'developer guide no. 131
            If Not grdRI.CurrentCell Is Nothing Then
                Dim lLastCol As Integer = grdRI.CurrentCell.ColumnIndex
                If Not IsNothing(grdRI.CurrentRow) Then
                    grdRI.CurrentCell = grdRI.CurrentRow.Cells((grdRI.CurrentCell.ColumnIndex + 1) Mod grdRI.Columns.Count)
                End If
                If Not IsNothing(grdRI.CurrentRow) Then
                    grdRI.CurrentCell = grdRI.CurrentRow.Cells(lLastCol)
                End If
            End If
        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Public Function GetProperties(ByRef vRILines As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetProperties"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for valid array
            If m_oXA Is Nothing Then
                gPMFunctions.RaiseError("m_oXA Is Nothing", "Can't read properties when grid is closed")
            End If

            ' Collapse XArray back to basic RI array
            lReturn = CType(CollapseXArray(vRILines), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CollapseXArray(vRILines)", "Unable to collapse reinsurance array")
            End If

            ' Update the ri array on our arrangement object


            m_oRIArrangement.ReinsuranceLines = vRILines

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function




    Public Function SetProperties(ByVal oRIArrangement As RiskRIArrangement, Optional ByVal oRIOriginal As RiskRIArrangement = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear grid
            lReturn = Clear()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Clear", "Unable to clear previous reinsurance details")
            End If

            ' Set appropriate read only state based on what we have to allocate
            'ReadOnly_Renamed = (oRIArrangement.Premium = 0 And oRIArrangement.SumInsured = 0)

            ' Store the arrangement objects
            m_oRIArrangement = oRIArrangement
            m_oRIOriginal = oRIOriginal

            ' Create the xarray and load from supplied raw array
            m_oXA = New XArrayHelper()
            If Information.IsArray(oRIArrangement.ReinsuranceLines) Then
                m_bRIEmpty = False
                bPMFunc.TransposeArray(oRIArrangement.ReinsuranceLines)
                m_oXA.LoadRows(oRIArrangement.ReinsuranceLines)
                bPMFunc.TransposeArray(oRIArrangement.ReinsuranceLines)
            Else
                ' Create band header row and set flag indicating we have done so
                m_bRIEmpty = True
                m_oXA.RedimXArray(New Integer() {0, RiskRIArrangement.RiskReinsuranceEnum.DBRIMax}, New Integer() {0, 0})
            End If

            ' The array is raw RI data, insert the summary rows
            lReturn = ExpandXArray()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ExpandXArray", "Unable to expand the reinsurance array")
            End If

            ' We now need to rollup all values into the summary rows
            lReturn = RollupXArray()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RollupXArray", "Unable to rollup the reinsurance array")
            End If

            ' Bind array to grid
            Dim bindingSource As BindingSource = New BindingSource(m_oXA, "")
            grdRI.DataSource = bindingSource
            'grdRI.ReBind()
            m_oXA.AcceptChanges()
            If grdRI.Columns.Count > 10 Then
                For colCtr As Integer = 10 To grdRI.Columns.Count - 1
                    grdRI.Columns(colCtr).Visible = False
                Next
            End If
            grdRI_Enter()
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function CollapseXArray(ByRef vRI As Object) As Integer

        Dim result As Integer = 0
        Dim lDest As Integer

        Dim lReturn As Integer
        Const kMethodName As String = "CollapseXArray"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear ri array

            'developer guide no. 12
            vRI = Nothing

            ' We need to walk the XArray and remove any genuine RI rows
            For lSource As Integer = m_oXA.GetLowerBound(0) To m_oXA.GetUpperBound(0)
                ' Check row type
                If iPMFunc.IsIn(m_oXA(lSource, RiskRIArrangement.RiskReinsuranceEnum.DBRIType), "R", "T", "F") Then
                    ' Prep main array
                    If lDest = 0 Then
                        ReDim vRI(RiskRIArrangement.RiskReinsuranceEnum.DBRIMax, lDest)
                    Else
                        ReDim Preserve vRI(RiskRIArrangement.RiskReinsuranceEnum.DBRIMax, lDest)
                    End If

                    ' Copy all row data
                    For lCol As Integer = 0 To RiskRIArrangement.RiskReinsuranceEnum.DBRIMax

                        vRI(lCol, lDest) = m_oXA(lSource, lCol)
                    Next

                    ' Increment destination count
                    lDest += 1
                End If
            Next


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Function ExpandXArray() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ExpandXArray"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' When we get here we should have an array containing just
            ' the raw treaty and fac lines. We need to expand this to
            ' include all of the summary lines.

            ' ***********************************************************
            ' Insert band total at the top, checking if we have a base array
            ' Note: If ri was empty we already have an empty band row!
            If Not m_bRIEmpty Then
                'lReturn = CType(m_oXA.InsertRows(0, 1), gPMConstants.PMEReturnCode)
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.InsertRows(1, 1)", "Unable to insert band totals")
                'End If
                m_oXA.Rows.InsertAt(m_oXA.NewRow, 0)
                'developer guide no. added to commit the change in datatable m_oXA
                m_oXA.AppendRows()
            End If

            ' Populate row
            m_lASumBand = 0
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Band Total"
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = m_oRIArrangement.SumInsured
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = m_oRIArrangement.Premium
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "BT"




            ' ***********************************************************
            ' We must now walk the treaty rows add add the treaty total
            m_lASumTreaty = 0
            ' -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN66155
            m_lASumObligatoryTreaty = 0
            'developer guide no. changed as per the functionality
            For lCount As Integer = 1 To m_oXA.GetUpperBound(0)
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)
                    Case "R"
                        ' Retained store ro and keep going
                        If m_lRetained = 0 Then
                            m_lRetained = lCount
                        End If
                    Case "T"
                        ' Treaty keep going
                        'Start -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsObligatorty)) = "1" Then
                            m_lASumObligatoryTreaty = lCount
                        End If
                        'End -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
                    Case Else
                        m_lASumTreaty = lCount
                        Exit For
                End Select
            Next
            'Start -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
            If m_lASumObligatoryTreaty > 0 Then
                'lReturn = CType(m_oXA.InsertRows(gPMConstants.kRINetLineObligatory, 1), gPMConstants.PMEReturnCode)
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.InsertRows", "Unable to insert new NetLine row", gPMConstants.PMELogLevel.PMLogError)
                'End If
                m_oXA.Rows.InsertAt(m_oXA.NewRow, gPMConstants.kRINetLineObligatory)
                'developer guide no. added to commit the change in datatable m_oXA
                m_oXA.AppendRows()
                m_oXA(gPMConstants.kRINetLineObligatory, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Net Line"
                m_oXA(gPMConstants.kRINetLineObligatory, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "NL"
                If m_lASumTreaty > 0 Then
                    m_lASumTreaty += 1
                End If
            End If
            'End -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
            ' If we reached the end of the array add total at end
            If m_lASumTreaty = 0 Then
                'lReturn = m_oXA.AppendRows()
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.AppendRows", "Unable to append treaty totals")
                'End If
                m_oXA.AppendRows()

                m_lASumTreaty = m_oXA.GetUpperBound(0)
            Else
                'lReturn = CType(m_oXA.InsertRows(m_lASumTreaty, 1), gPMConstants.PMEReturnCode)
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.InsertRows(m_lASumTreaty, 1)", "Unable to insert treaty totals")
                'End If
                m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumTreaty)
                'developer guide no. added to commit the change in datatable m_oXA
                m_oXA.AppendRows()
            End If

            ' Populate treaty summary (no values, these are done elsewhere)
            If m_bAgency Then
                m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Binder Total"
            Else
                m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Treaty Total"
            End If
            m_oXA(m_lASumTreaty, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "TT"



            ' ***********************************************************
            ' We must now walk the fac rows add add the treaty total
            m_lASumFAC = 0
            For lCount As Integer = m_lASumTreaty + 1 To m_oXA.GetUpperBound(0)
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)
                    Case "R", "T"
                        gPMFunctions.RaiseError("Select Case m_oXA.Value(lCount, DBRIType)", "Treaty lines found out of sequence")
                    Case "F"
                        ' Fac lines continue
                    Case Else
                        m_lASumFAC = lCount
                        Exit For
                End Select
            Next

            ' If we reached the end of the array add total at end
            ' Note: For FAC this should always be true at risk level
            If m_lASumFAC = 0 Then
                'lReturn = m_oXA.AppendRows()
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.AppendRows", "Unable to append facultative totals")
                'End If
                m_oXA.AppendRows()
                m_lASumFAC = m_oXA.GetUpperBound(0)
            Else
                'lReturn = CType(m_oXA.InsertRows(m_lASumFAC, 1), gPMConstants.PMEReturnCode)
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.InsertRows(m_lASumFAC, 1)", "Unable to insert fac totals")
                'End If
                m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumFAC)
                'developer guide no. added to commit the change in datatable m_oXA
                m_oXA.AppendRows()
            End If

            ' Populate treaty summary (no values, these are done elsewhere)
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Facultative Total"
            m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "FT"

            ' Check this was the last line
            If m_lASumFAC <> m_oXA.GetUpperBound(0) Then
                gPMFunctions.RaiseError("m_lASumFAC <> m_oXA.UpperBound(1)", "Last line is not facultative summary")
            End If


            ' ***********************************************************
            ' Add allocated total
            If Not m_bReadOnly Then
                ''lReturn = m_oXA.AppendRows()
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("m_oXA.AppendRows", "Unable to append allocated total")
                'End If
                m_oXA.AppendRows()

                ' Set allocated line
                m_lASumAlloc = m_oXA.GetUpperBound(0)
                m_oXA(m_lASumAlloc, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Allocated"
                m_oXA(m_lASumAlloc, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "AT"
            End If

            ' ***********************************************************
            ' Do not add unallocated here it will be added, if
            ' necessary during totalling


            ' ***********************************************************
            ' If original ri is present add original and net values
            If Not (m_oRIOriginal Is Nothing) And (Not m_bReadOnly) Then
                'lReturn = CType(m_oXA.AppendRows(2), gPMConstants.PMEReturnCode)
                'If lReturn <> 2 Then
                '    gPMFunctions.RaiseError("m_oXA.AppendRows(2)", "Unable to append original ri lines")
                'End If
                m_oXA.AppendRows()
                m_oXA.AppendRows()
                ' Set original line
                m_lASumOriginal = m_oXA.GetUpperBound(0) - 1
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Original RI Totals"
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = m_oRIOriginal.SumInsured
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = m_oRIOriginal.Premium
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "OT"

                ' Set allocated line
                m_lASumNet = m_oXA.GetUpperBound(0)
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Net"
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = m_oRIArrangement.SumInsured + m_oRIOriginal.SumInsured
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = m_oRIArrangement.Premium + m_oRIOriginal.Premium
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "NT"
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Function RollupXArray() As Integer

        Dim result As Integer = 0
        Dim oTtyTotal, oFacTotal As RiskTotalizer

        Dim lReturn As Integer
        Const kMethodName As String = "RollupXArray"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create totalizers
            oTtyTotal = New RiskTotalizer()
            oFacTotal = New RiskTotalizer()

            ' Walk the treaty rows and store summary
            'Start -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
            If m_lASumObligatoryTreaty > 0 Then
                For lCount As Integer = m_lASumBand + 1 To 2 - 1
                    ' Increase the totals
                    oTtyTotal.Add(m_oXA, lCount)
                Next
                oTtyTotal.StoreNetLine(m_oXA, 2)
                For lCount As Integer = 3 To m_lASumTreaty - 1
                    ' Increase the totals
                    oTtyTotal.Add(m_oXA, lCount)
                Next
            Else
                'End -(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
                For lCount As Integer = m_lASumBand + 1 To m_lASumTreaty - 1
                    ' Increase the totals
                    oTtyTotal.Add(m_oXA, lCount)
                Next
                '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
            End If
            oTtyTotal.Store(m_oXA, m_lASumTreaty)

            ' Walk the fac rows and store summary
            For lCount As Integer = m_lASumTreaty + 1 To m_lASumFAC - 1
                oFacTotal.Add(m_oXA, lCount)
            Next
            oFacTotal.Store(m_oXA, m_lASumFAC)

            ' Get allocation details if we are editable
            If Not m_bReadOnly Then
                ' Get grand total and store
                oTtyTotal.Combine(oFacTotal)
                oTtyTotal.Store(m_oXA, m_lASumAlloc)

                ' Check unallocated and add/remove as necessary
                If Math.Abs(oTtyTotal.Premium - m_oRIArrangement.Premium) > 0.005 Or Math.Abs(oTtyTotal.SumInsured - m_oRIArrangement.SumInsured) > 0.005 Then
                    ' Display an allocated line
                    If m_lASumUnalloc = 0 Then
                        If m_lASumOriginal > 0 Then
                            ' Insert before original summary and move summaries
                            'lReturn = m_oXA.InsertRows(m_lASumOriginal)
                            'If lReturn <> 1 Then
                            '    gPMFunctions.RaiseError("m_oXA.InsertRows(m_lASumOriginal)", "Unable to insert unallocated totals")
                            'End If
                            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumOriginal)
                            'developer guide no. added to commit the change in datatable m_oXA
                            m_oXA.AppendRows()
                            m_lASumUnalloc = m_lASumOriginal
                            m_lASumOriginal += 1
                            m_lASumNet += 1
                        Else
                            ' Append to grid
                            'lReturn = m_oXA.AppendRows()
                            'If lReturn <> 1 Then
                            '    gPMFunctions.RaiseError("m_oXA.AppendRows", "Unable to append unallocated totals")
                            'End If  
                            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_oXA.Rows.Count)
                            m_oXA.AppendRows()
                            'This check I have added because AppendRows need to be called twice at the beginning only, calling 
                            'it once does not add any row to the m_oXA DataTable. The following check ensures that
                            'second time the AppendRows is not called, otherwise two blank rows will get inserted.
                            If Not m_oXA.Rows(m_oXA.Rows.Count - 1)(0) Is Nothing AndAlso Not m_oXA.Rows(m_oXA.Rows.Count - 1)(0).ToString.Equals(String.Empty) Then
                                m_oXA.AppendRows()
                            End If
                            m_lASumUnalloc = m_oXA.Rows.Count - 1
                        End If
                    End If

                    ' Populate
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnum.DBRIName) = "Unallocated"
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = 1 - oTtyTotal.SharePercent
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) = m_oRIArrangement.SumInsured - oTtyTotal.SumInsured

                    'To check the unallocated RI amount
                    m_lunAllocatedRI = m_oRIArrangement.SumInsured - oTtyTotal.SumInsured
                    m_lUnallocatedPremium = m_oRIArrangement.Premium - oTtyTotal.Premium
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = m_oRIArrangement.Premium - oTtyTotal.Premium
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "UT"
                    'grdRI_Enter()
                Else
                    ' If line exists remove it
                    If m_lASumUnalloc > 0 Then
                        lReturn = m_oXA.DeleteRows(m_lASumUnalloc)
                        m_oXA.AcceptChanges()
                        If lReturn <> 1 Then
                            gPMFunctions.RaiseError("m_oXA.DeleteRows(m_lASumUnalloc, 1)", "Unable to remove unallocated totals")
                        End If
                        m_lASumUnalloc = 0

                        ' Move original summaries
                        If m_lASumOriginal > 0 Then
                            m_lASumOriginal -= 1
                            m_lASumNet -= 1
                        End If
                    End If
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub grdRI_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles grdRI.CellPainting
        If e.ColumnIndex = 0 And e.RowIndex > -1 Then
            Select Case gPMFunctions.ToSafeString(m_oXA(e.RowIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIType))
                '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)-Note:- "NL" is added in this condition
                Case "R", "T", "X"
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All And Not DataGridViewPaintParts.ContentForeground)
                    e.Graphics.DrawIconUnstretched(Global.cSIRRIControls.My.Resources.TTY, e.CellBounds)
                    e.Graphics.DrawString(e.Value, e.CellStyle.Font, Brushes.Black, e.CellBounds.X + Global.cSIRRIControls.My.Resources.TTY.Width, e.CellBounds.Y + 5)
                    e.Handled = True
                Case "F"
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All And Not DataGridViewPaintParts.ContentForeground)
                    e.Graphics.DrawIconUnstretched(Global.cSIRRIControls.My.Resources.FAC, e.CellBounds)
                    e.Graphics.DrawString(e.Value, e.CellStyle.Font, Brushes.Black, e.CellBounds.X + Global.cSIRRIControls.My.Resources.FAC.Width, e.CellBounds.Y + 5)
                    e.Handled = True
            End Select
        End If
    End Sub


    ' ***************************************************************** '
    '                           GRID EVENTS
    ' ***************************************************************** '
    Private Sub grdRI_CellUpdated(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles grdRI.CellUpdated
        'developer guide no. added check to prevent recursion
        If Not grdRI.CurrentCell Is Nothing Then
            If Not grdRI.CurrentCell.Equals(grdRI.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim lRow As Integer
        Dim cPremiumTax, cCommTax As Decimal
        Dim bCalcBasisSIChanged As Boolean
        Dim lOriginalRow As Integer

        Dim lReturn As Integer
        Const kMethodName As String = "grdRI_AfterColUpdate"


        Try

            If m_bCellChanged Then
                ' reset changed status and get the current row
                m_bCellChanged = False
                lRow = grdRI.CurrentRowIndex

                ' If we have changed row, this will be automatic, if we've
                ' changed column or hit enter we must force the update...
                grdRI.UpdateCurrentRow()

                ' Set dirty and recalculate any affected data
                m_bIsDirty = True

                If m_oRIArrangement.TransactionType = "MTA" And Not (m_oRIOriginal Is Nothing) Then

                    If Not m_oRIArrangement.ReinsuranceLines Is Nothing Then
                        If (m_oRIArrangement.ReinsuranceLines.GetUpperBound(1) = m_oRIOriginal.ReinsuranceLines.GetUpperBound(1) Or CStr(m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIType, m_oRIArrangement.ReinsuranceLines.GetUpperBound(1))) = "F") And (m_oRIArrangement.SumInsured + m_oRIOriginal.SumInsured <> 0) Then

                            For lCnt As Integer = 0 To m_oRIArrangement.ReinsuranceLines.GetUpperBound(1)

                                If Not m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority, 0).Equals(m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority, lCnt)) Then
                                    ' Check if more than one priority is there with SI value in it

                                    If gPMFunctions.ToSafeDouble(m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured, lCnt)) <> 0 Then
                                        bCalcBasisSIChanged = True
                                        Exit For
                                    End If
                                End If
                            Next lCnt

                            If bCalcBasisSIChanged Then
                                lOriginalRow = -1
                                ' match all the priorities on both tabs

                                For lCnt As Integer = 0 To m_oRIArrangement.ReinsuranceLines.GetUpperBound(1)
                                    'set it back to false (except FAC) and search the treaty on original tab for given priority and default share

                                    If gPMFunctions.ToSafeString(m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIType, lCnt)) <> "F" Then
                                        bCalcBasisSIChanged = False
                                    End If


                                    For lCntOriginal As Integer = 0 To m_oRIOriginal.ReinsuranceLines.GetUpperBound(1)
                                        If m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID, lCnt).Equals(m_oRIOriginal.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID, lCntOriginal)) And m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority, lCnt).Equals(m_oRIOriginal.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIPriority, lCntOriginal)) And m_oRIArrangement.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIDefaultShare, lCnt).Equals(m_oRIOriginal.ReinsuranceLines(RiskRIArrangement.RiskReinsuranceEnum.DBRIDefaultShare, lCntOriginal)) Then
                                            bCalcBasisSIChanged = True
                                            ' mark the matched one
                                            If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) Is DBNull.Value AndAlso m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "F" Then
                                                If lCntOriginal = lRow - 2 Then
                                                    lOriginalRow = lCntOriginal
                                                End If
                                            Else
                                                If lCntOriginal = lRow - 1 Then
                                                    lOriginalRow = lCntOriginal
                                                End If
                                            End If
                                            Exit For
                                        End If
                                    Next lCntOriginal

                                    If Not bCalcBasisSIChanged Then
                                        ' with first failure move out
                                        Exit For
                                    End If
                                Next lCnt


                                If bCalcBasisSIChanged And lOriginalRow = -1 Then
                                    ' it's an additional FAC row, over and above all priorities; let it go
                                    If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) Is DBNull.Value AndAlso m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) <> "F" Then
                                        bCalcBasisSIChanged = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If


                ' Check for appropriate row type
                If iPMFunc.IsIn(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType), "T", "R", "F") Then
                    ' Sum insured will affect share and premiums
                    If ColIndex = RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured Then
                        ' Recalculate share if we can
                        If m_oRIArrangement.SumInsured <> 0 Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured) / m_oRIArrangement.SumInsured
                        End If

                        ' Recalculate premium
                        ' Note: This will override non-prop fac premiums
                        'normal distribution on band SI : treaty SI basis
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = m_oRIArrangement.Premium _
                            * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare)
                    End If

                    If ColIndex = RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium Then
                        ' Recalculate share if we can
                        If m_oRIArrangement.SumInsured <> 0 OrElse m_oRIArrangement.Premium <> 0 Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare) = m_oXA(lRow, _
                                                                                               RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) / m_oRIArrangement.Premium
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = m_oRIArrangement.Premium * m_oXA(lRow, _
                                                                                             RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare)
                        End If
                    End If


                    ' SI, Premium or commission rate will affect commission and taxes
                    If iPMFunc.IsIn(CStr(ColIndex), RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) Then
                        ' Recalculate premium share, if we can
                        If m_oRIArrangement.Premium <> 0 AndAlso CDbl(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured)) <> 0 Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremiumPercent) = CDbl(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured)) / m_oRIArrangement.SumInsured
                        Else
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremiumPercent) = 0
                        End If
                    End If

                    ' SI, Premium or commission rate will affect commission and taxes
                    If iPMFunc.IsIn(CStr(ColIndex), RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) Then
                        ' Recalculate commission
                        If CStr(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent)).IndexOf("%"c) >= 0 Or bIsCommPercentChanged Then
                            'PN #32170 and PN #33188 Need to remove % sign
                            'This situation will occur in compiled exe only
                            RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                            RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent) = (Conversion.Val(CStr(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent)).Replace("%", "")) / 100)
                            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                        End If

                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) * ToSafeDouble(Convert.ToString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent)))

                        ' Flag commission as amended
                        If ColIndex = RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsCommissionModified) = 1
                        End If

                        ' Recalculate taxes
                        Dim objArgs As Object
                        If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) Is DBNull.Value AndAlso m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "F" Then
                            ' Get new fax taxes
                            objArgs = New RecalculateFacTaxEventArgs(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRILineID), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPartyCnt), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateFacTax(Me, objArgs)
                        Else
                            ' Get new treaty taxes
                            objArgs = New RecalculateTreatyTaxEventArgs(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRILineID), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objArgs)
                        End If

                        ' Store new values
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRITax) = objArgs.cPremiumTax
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnum.DBRICommTax) = objArgs.cCommTax
                    End If
                End If

                ' Rollup and rebind
                RollupXArray()
                'grdRI.ReBind()
                grdRI.Refresh()
                ' Set to active column before rebind
                If Not IsNothing(grdRI.CurrentRow) Then
                    grdRI.CurrentCell = grdRI.CurrentRow.Cells(ColIndex)
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub
    Private bIsCommPercentChanged As Boolean
    Private Sub grdRI_CellUpdating(ByVal eventSender As Object, ByVal eventArgs As Artinsoft.Windows.Forms.DataGridViewCellValueCancelEventArgs) Handles grdRI.CellUpdating
        'developer guide no. added check to prevent recursion
        If Not grdRI.CurrentCell Is Nothing Then
            If Not grdRI.CurrentCell.Equals(grdRI.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex)) Then
                Exit Sub
            End If
        End If

        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim OldValue As Object = grdRI.CurrentRow.Cells(ColIndex).Value
        Dim Cancel As Integer = 0
        'developer guide no. 101
        Dim NewValue As Object
        Dim cPremiumTax, cCommTax As Decimal

        Dim sMessage As String = ""
        Dim lReturn As Integer
        Const kMethodName As String = "grdRI_BeforeColUpdate"


        Try

            ' Ensure we default to not changed
            m_bCellChanged = False

            ' Store new value, we'll use it lots

            NewValue = eventArgs.Value
            ' Validate the change
            Dim dTempValue As Decimal = 0D
            Select Case ColIndex
                Case RiskRIArrangement.RiskReinsuranceEnum.DBRISumInsured
                    ' Validate range
                    If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                        ' Simple numeric validation
                        If NewValue >= 0 Then
                            ' Set changed state

                            m_bCellChanged = (NewValue <> OldValue)
                        Else
                            sMessage = "Sum Insured must be a positive currency value"
                        End If
                    Else
                        sMessage = "Sum Insured must be a valid currency value"
                    End If

                Case RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium
                    ' Validate range
                    If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                        ' Note:
                        ' - Premium changes are only allowed for non-proportional fac
                        ' - Any change in premium should be balanced to the retained line
                        ' - We can allow negative for manual balancing within FAC lines
                        ' Set changed state

                        m_bCellChanged = (NewValue <> OldValue)

                        ' Check for retained
                        If m_lRetained > 0 Then
                            If Not (m_oRIArrangement.RIManualPremiumAdjustments = 1 And grdRI.CurrentCell.ColumnIndex = 4) Then
                                ' Balance to retained line (modify by adjusted value)

                                m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium) = gPMFunctions.ToSafeDouble(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium)) + (gPMFunctions.ToSafeDouble(OldValue) - gPMFunctions.ToSafeDouble(NewValue))
                            End If

                            ' Recalc retained percent
                            If m_oRIArrangement.Premium <> 0D Then
                                m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremiumPercent) = gPMFunctions.ToSafeDouble(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium)) / m_oRIArrangement.Premium
                            End If

                            ' Recalculate commission
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission) = gPMFunctions.ToSafeDouble(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium)) * gPMFunctions.ToSafeDouble(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent))

                            ' Get new treaty taxes
                            Dim objTaxArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(gPMFunctions.ToSafeInteger(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRILineID)), gPMFunctions.ToSafeInteger(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRITreatyID)), gPMFunctions.ToSafeDecimal(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium)), gPMFunctions.ToSafeDecimal(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRICommission)), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objTaxArgs)

                            ' Store new values
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRITax) = objTaxArgs.cPremiumTax
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnum.DBRICommTax) = objTaxArgs.cCommTax
                        End If
                    Else
                        sMessage = "Premium must be a valid currency value"
                    End If

                Case RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent
                    ' Simple numeric validation
                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(NewValue).Replace("%",""), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        NewValue = gPMFunctions.ToSafeDouble(gPMFunctions.ToSafeString(NewValue).Replace("%", ""))
                        ' Validate percentage is between 0 and 100
                        If (NewValue >= 0) And (NewValue <= 100) Then
                            ' Warn if changing treaty commission
                            If (Not m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) Is DBNull.Value) And (Not m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsCommissionModified) = 0 Is DBNull.Value) Then
                                If m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "T" And m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsCommissionModified) = 0 Then
                                    If MessageBox.Show("Amending the treaty commission percentage will override individual treaty party rates." & Strings.Chr(13) & Strings.Chr(10) & "Continue to amend commission percentage?", "Override Treaty Commission", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                                        Cancel = gPMConstants.PMEReturnCode.PMFalse

                                        eventArgs.Value = OldValue

                                        eventArgs.Cancel = True
                                        Exit Sub
                                    End If
                                End If
                            End If
                            ' If we use automatic grid formatting then it treats percentages as
                            ' 0..1 which is a pain to enter so scale them nicely for the user
                            eventArgs.Value = gPMFunctions.ToSafeDouble(NewValue / 100)
                            bIsCommPercentChanged = True
                            m_bCellChanged = True
                        Else
                            sMessage = "Commission percentage must be between 0% and 100%"
                        End If
                    Else
                        sMessage = "Commission percentage must be a valid numeric between 0 and 100"
                    End If

                Case Else
                    ' Simple change check

                    m_bCellChanged = (NewValue <> OldValue)
            End Select

            ' If we have a message then show it and set Cancel flag
            If sMessage.Length Then
                MessageBox.Show(sMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                eventArgs.Cancel = True
                eventArgs.Value = OldValue
            Else
                ' If we have not got a changed value cancel the change
                If Not m_bCellChanged Then

                    eventArgs.Cancel = True
                    eventArgs.Value = OldValue
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally





        End Try
    End Sub







    Private Sub grdRI_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles grdRI.GotFocus


        'grdRI.MarqueeStyle  = MarqueeStyleConstants.dbgSolidCellBorder
        grdRI_Enter()
    End Sub

    Private Sub grdRI_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles grdRI.LostFocus


        'grdRI.MarqueeStyle  = MarqueeStyleConstants.dbgNoMarquee
    End Sub


    ' ***************************************************************** '
    '                       USERCONTROL EVENTS
    ' ***************************************************************** '

    Private Sub UserControl_InitProperties()
        ' Initialise read only state
        ReadOnly_Renamed = False
    End Sub



    'developer guide no. 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        ' Load read only state


        ReadOnly_Renamed = CBool(PropBag.ReadProperty("ReadOnly", False))
    End Sub

    Private Sub uctRiskRIControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            grdRI.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)

        Catch
        End Try
    End Sub



    'developer guide no. 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        ' Save read only state

        PropBag.WriteProperty("ReadOnly", ReadOnly_Renamed, False)
    End Sub
    Dim iStartIndexSI As Integer = -1
    Dim iEndIndexSI As Integer = -1
    Dim iStartIndexComm As Integer = -1
    Dim iEndIndexComm As Integer = -1
    Dim iStartIndexAg As Integer = -1
    Dim iEndIndexAg As Integer = -1
    Dim iStartIndexPrem As Integer = -1
    Dim iEndIndexPrem As Integer = -1

    Dim iStartIndexSIFAC As Integer = -1
    Dim iEndIndexSIFAC As Integer = -1
    Dim iStartIndexCommFAC As Integer = -1
    Dim iEndIndexCommFAC As Integer = -1
    Dim iStartIndexAgFAC As Integer = -1
    Dim iEndIndexAgFAC As Integer = -1
    Dim iStartIndexPremFAC As Integer = -1
    Dim iEndIndexPremFAC As Integer = -1



    Public Sub grdRI_Enter()
        If grdRI.RowsCount = 0 Then
            Exit Sub
        End If
        If grdRI.Columns.Count > 10 Then
            For colCtr As Integer = 10 To grdRI.Columns.Count - 1
                grdRI.Columns(colCtr).Visible = False
            Next
        End If
        For Each dr As DataGridViewRow In grdRI.Rows
            If m_oXA.Rows(dr.Index).RowState = DataRowState.Deleted Then
                m_oXA.AcceptChanges()
                Exit Sub
            End If
            For Each dc As DataGridViewColumn In grdRI.Columns
                Try
                    ' All summary rows are locked and have differing format styles
                    If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) Is DBNull.Value Then
                        Select Case m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)
                            '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)-Note:- "NL" is added in this condition
                            Case "TT", "FT", "XT", "NL"
                                'RowStyle.BackColor = vbInfoBackground
                                'RowStyle.Font.Bold = True
                                'RowStyle.Font.Italic = True
                                'RowStyle.ForeColor = vbInfoText
                                'RowStyle.Locked = True
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.Info
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Italic Or FontStyle.Bold)
                                grdRI.Rows(dr.Index).ReadOnly = True

                            Case "BT", "AT", "UT"
                                'RowStyle.BackColor = vbInfoBackground
                                'RowStyle.Font.Bold = True
                                'RowStyle.ForeColor = vbInfoText
                                'RowStyle.Locked = True
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.Info
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Bold)
                                grdRI.Rows(dr.Index).ReadOnly = True


                            Case "OT", "NT"
                                'RowStyle.BackColor = vbButtonFace
                                'RowStyle.Font.Bold = True
                                'RowStyle.Font.Italic = True
                                'RowStyle.Locked = True
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.ButtonFace
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Italic Or FontStyle.Bold)
                                grdRI.Rows(dr.Index).ReadOnly = True

                                'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
                            Case "T"
                                If ToSafeInteger(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsObligatorty)) = 1 Then
                                    'RowStyle.BackColor = RGB(135, 206, 250)
                                    ''-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
                                    'RowStyle.Locked = True

                                    grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = Color.SkyBlue
                                    'grdRI.Rows(dr.Index).ReadOnly = True
                                    If dc.Index = RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium And m_oRIArrangement.RIManualPremiumAdjustments = 1 Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                    Else
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    End If

                                End If
                                'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
                        End Select
                    End If

                    'Check if the column needs to have style or not
                    'If r.Contains(CType(dc.Index, RiskRIArrangement.RiskReinsuranceEnum)) Then
                    '    'grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                    '    Select Case m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)
                    '        Case "R", "T", "X", "F"
                    '            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                    '    End Select
                    '    Continue For
                    'End If

                    'Dim strColName As String = grdRI.Columns(e.ColumnIndex).HeaderText
                    Select Case dc.Index
                        Case RiskRIArrangement.RiskReinsuranceEnum.DBRIName
                            ' Set either caption alignment for summaries or RI icons
                            Select Case m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)
                                '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)-Note:- "NL" is added in this condition
                                Case "BT", "TT", "FT", "XT", "AT", "UT", "OT", "NT", "NL"
                                    'CellStyle.Alignment = dbgRight
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Case "R", "T", "X"
                                    'CellStyle.ForegroundPicture = grdRI.Styles("TTYHeader").ForegroundPicture
                                Case "F"
                                    'CellStyle.ForegroundPicture = grdRI.Styles("FACHeader").ForegroundPicture
                            End Select

                        Case RiskRIArrangement.RiskReinsuranceEnum.DBRIPremium
                            ' We may be able to edit FAC premiums
                            Select Case m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)

                                Case "R", "T"
                                    'Start-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.5.2.1)
                                    If m_oRIArrangement.RIManualPremiumAdjustments = 1 Then
                                        'CellStyle.BackColor = vbWhite
                                        'CellStyle.Locked = False
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                    Else
                                        'CellStyle.BackColor = vbButtonFace
                                        'CellStyle.Locked = True
                                        If ToSafeInteger(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIIsObligatorty)) = 1 Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                        Else
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        End If
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    End If
                                    'End-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.5.2.1)

                                Case "F"
                                    'Start-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.5.2.1)
                                    If m_oRIArrangement.RIManualPremiumAdjustments = 1 Then
                                        'CellStyle.BackColor = vbWhite
                                        'CellStyle.Locked = False
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                    ElseIf m_oRIArrangement.FACPremiumMethod = RiskRIArrangement.FACPremiumEnum.FACPremiumIsProportional Then
                                        'End-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.5.2.1)
                                        'CellStyle.BackColor = vbButtonFace
                                        'CellStyle.Locked = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    End If
                            End Select

                        Case RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent, RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode
                            ' We can't edit retained commission or agreement codes
                            Select Case m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType)
                                Case "R"
                                    'CellStyle.BackColor = vbButtonFace
                                    'CellStyle.Locked = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                            End Select
                    End Select

                    If Not grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly Then
                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                    End If

                Catch
                    ' DO Not Call any functions before here or the error will be lost
                    'iPMFunc.LogError( _
                    '    v_sClass:=ACClass, _
                    '    v_sMethod:="grdRI_Enter", _
                    '    r_lFunctionReturn:=m_lReturn)

                    'Finally
                    '    Exit Sub
                End Try
            Next
        Next
    End Sub

    Private Sub grdRI_RowAdded(ByVal sender As System.Object, ByVal e As System.Data.DataTableNewRowEventArgs) Handles grdRI.RowAdded
        grdRI_Enter()
    End Sub

    Private Sub grdRI_RowDeleted(ByVal sender As System.Object, ByVal e As System.Data.DataRowChangeEventArgs) Handles grdRI.RowDeleted
        grdRI_Enter()
    End Sub
    Private Sub grdRI_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdRI.CellFormatting
        Dim dTempVal As Double = 0D
        If Not e.Value Is DBNull.Value Then
            If e.CellStyle.Format = "0.00%" Then
                e.Value = Convert.ToString(Math.Round((gPMFunctions.ToSafeDouble(e.Value) * 100), 2)) + "%"
            ElseIf e.CellStyle.Format = "0.00" Then
                e.Value = Math.Round(gPMFunctions.ToSafeDouble(e.Value), 2)
            ElseIf e.CellStyle.Format = "N2" Then
                e.Value = Math.Round(gPMFunctions.ToSafeDouble(e.Value), 2)
            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnum.DBRIThisShare _
             Or e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnum.DBRIDefaultShare Then
                If (Double.TryParse(e.Value, dTempVal)) Then
                    e.Value = dTempVal.ToString("P2")
                End If
            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnum.DBRICommPercent Then
                If (Double.TryParse(e.Value.ToString.Replace("%", ""), dTempVal)) Then
                    If bIsCommPercentChanged Then
                        e.Value = (dTempVal / 100).ToString("P2")
                        bIsCommPercentChanged = False
                    Else
                        e.Value = dTempVal.ToString("P2")
                    End If
                End If
            Else
                e.Value = e.Value
            End If
        End If
    End Sub

    Private Sub grdRI_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdRI.CellLeave
        grdRI_Enter()
    End Sub
End Class
