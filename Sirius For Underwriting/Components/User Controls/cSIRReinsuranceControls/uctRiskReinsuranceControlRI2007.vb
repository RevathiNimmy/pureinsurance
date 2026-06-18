Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports SharedFiles
'
<System.Runtime.InteropServices.ProgId("uctRiskRIControlRI2007_NET.uctRiskRIControlRI2007")>
Partial Public Class uctRiskRIControlRI2007
    Inherits System.Windows.Forms.UserControl
    ' Current Supported Types of RI and their priority
    ' F -  Facultative          1
    ' FX - Facultative XOL      2
    ' T -  Treaty               3
    ' TX - Treaty XOL           4


    ' Pending Tasks
    '   --> Testing if their is no Retain Line

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctRiskRIControl"

    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    ' Refresh treaty tax amounts
    Public Event RecalculateTreatyTax(ByVal Sender As Object, ByRef e As RecalculateTreatyTaxEventArgs)

    ' Refresh facultative tax amounts
    Public Event RecalculateFacTax(ByVal Sender As Object, ByRef e As RecalculateFacTaxEventArgs)

    Public Event ResetControls(ByVal Sender As Object, ByVal e As ResetControlsEventArgs)

    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    Private m_bIsDirty As Boolean
    Private m_bReadOnly As Boolean
    Private m_bAgency As Boolean

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    ' To Keep the track of Last Row at the which Treaty row exists
    Private m_lASumTreaty As Integer

    ' To Keep the track of Last Row at the which Facultative row exists
    Private m_lASumFAC As Integer
    Private m_lASumTotal As Integer
    Private m_lASumAlloc As Integer
    Private m_lASumUnalloc As Integer
    Private m_lASumOriginal As Integer
    Private m_lASumNet As Integer
    Private m_lunAllocatedRI As Decimal
    Private m_lUnallocatedPremium As Decimal

    Private m_cTotalSumInsuredFacProp As Decimal
    Private m_bIsFacxolExists As Boolean
    Private m_bIsObligatoryExists As Boolean
    Private m_bIsFacPropExists As Boolean

    Private m_sSelectedRIType As String = ""
    Private m_SelectedRow As Integer
    ' Variable Declared to keep track of No of Fac Prop and Fac Xol Rows
    Dim m_iRowCountFAC As Integer

    ' Variable Declared to keep track of No of Treaty Prop and Treaty Xol Rows
    Dim m_iRowCountTreaty As Integer
    Private m_iRowCountRetained As Integer

    Private m_vDeletedRIArragementIds() As Object
    Private m_lSelRIArrangementLine As Integer
    Private m_lGroupingID As Integer
    Private m_cUpperLimit As Decimal
    Private m_cLowerLimit As Decimal
    Private m_vStoreLimits As Object
    Private m_bisRiBroker As Boolean
    Private m_lPartyCnt As Integer
    Private m_lASumObligatoryTreaty As Integer
    Private m_bIsNetLineNeeded As Integer
    Private m_oBusiness As Object     'E016
    Private m_sTransactionType As String
    Private m_nRIVersionId As Integer
    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '

    <Browsable(True)>
    Public Property ExistingLimits() As Object
        Get
            Return m_vStoreLimits
        End Get
        Set(ByVal Value As Object)


            m_vStoreLimits = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property DeletedRIArragementIds() As Object
        Get
            Return VB6.CopyArray(m_vDeletedRIArragementIds)
        End Get
        Set(ByVal Value As Object)
            m_vDeletedRIArragementIds = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property SelRIArrangementLine() As Integer
        Get
            Return m_lSelRIArrangementLine
        End Get
        Set(ByVal Value As Integer)
            m_lSelRIArrangementLine = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property SelectedRIType() As String
        Get
            Return m_sSelectedRIType
        End Get
        Set(ByVal Value As String)
            m_sSelectedRIType = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property SelectedRow() As Integer
        Get
            Return m_SelectedRow
        End Get
        Set(ByVal Value As Integer)
            m_SelectedRow = Value
        End Set
    End Property

    <Browsable(True)>
    Public Shadows Property Enabled() As Boolean
        Get
            Return grdRI.Enabled
        End Get
        Set(ByVal Value As Boolean)
            grdRI.Enabled = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Agency() As Boolean
        Set(ByVal Value As Boolean)
            m_bAgency = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property IsDirty() As Boolean
        Get
            Return m_bIsDirty
        End Get
        Set(ByVal Value As Boolean)
            m_bIsDirty = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RIVersionId() As Integer
        Get
            Return m_nRIVersionId
        End Get
        Set(ByVal Value As Integer)
            m_nRIVersionId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ReadOnly_Renamed() As Boolean
        Get
            Return m_bReadOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value

            ' Set base styles
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode).ReadOnly = m_bReadOnly

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement).ReadOnly = True

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Retained).ReadOnly = True
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Retained).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare).ReadOnly = m_bReadOnly
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property FACPropExists() As Boolean
        Get
            Return m_bIsFacPropExists
        End Get
    End Property
    <Browsable(False)>
    Public ReadOnly Property FACXolExists() As Boolean
        Get
            Return m_bIsFacxolExists
        End Get
    End Property
    <Browsable(False)>
    Public ReadOnly Property ObligatoryExists() As Boolean
        Get
            Return m_bIsObligatoryExists
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property UnallocatedRI() As Decimal
        Get
            Return m_lunAllocatedRI
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property UnallocatedPremium() As Decimal
        Get
            Return m_lUnallocatedPremium
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalSumInsuredFacProp() As Decimal
        Get
            Return m_cTotalSumInsuredFacProp
        End Get
    End Property

    <Browsable(True)>
    Public Property RowCountFAC() As Integer
        Get
            Return m_iRowCountFAC
        End Get
        Set(ByVal Value As Integer)
            m_iRowCountFAC = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RowCountTreaty() As Integer
        Get
            Return m_iRowCountTreaty
        End Get
        Set(ByVal Value As Integer)
            m_iRowCountTreaty = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RowCountRetained() As Integer
        Get
            Return m_iRowCountRetained
        End Get
        Set(ByVal Value As Integer)
            m_iRowCountRetained = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property GroupingID() As Integer
        Get
            Return m_lGroupingID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property UpperLimit() As Decimal
        Get
            Return m_cUpperLimit
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property LowerLimit() As Decimal
        Get
            Return m_cLowerLimit
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property IsRIBroker() As Boolean
        Get
            Return m_bisRiBroker
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
    End Property
    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '

    Public Function AllocateObligatory() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AllocateObligatory"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPosition, lPriority As Integer

        Dim bIsExistNetLineRow As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lPosition = 1
            ' Insert new row on to that position

            m_oXA.Rows.InsertAt(m_oXA.NewRow, 1)

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory))
                    Case "1"
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oRIArrangement.SumInsured) * ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare))
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = ToSafeDouble(m_oRIArrangement.Premium) * ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare))
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsRIBroker) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsRIBroker)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode)
                        ' Supporting fields
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "F"
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) = lPriority + 1
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified)
                        m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)
                        Exit For
                End Select
            Next

            ' Increment total lines...
            m_lASumTreaty += 1
            m_lASumFAC += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            'Increase the count of FAC row
            RowCountFAC += 1

            m_lReturn = CType(IsExists_NetLineRow(bIsExistNetLineRow), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("IsExists_NetLineRow", "Failed to locate the NetLine row", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not bIsExistNetLineRow Then

                m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition + 1)
                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "Net Line"
                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "NL"

                ' Increment total lines...
                m_lASumFAC += 1
                m_lASumTotal += 1
                m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
                m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
                m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
                m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

                m_lReturn = RollupXArray()

                For lCount As Integer = 2 To m_oXA.Rows.Count - 1
                    Select Case gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type))
                        Case "F", "T"
                            If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                                If lCount <> 1 Then
                                    m_oXA.Rows.RemoveAt(lCount)
                                    m_oXA.AcceptChanges()
                                    m_lASumTreaty -= 1
                                    m_lASumTotal -= 1
                                    m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                                    m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                                    m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                                    m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                                    Exit For
                                End If
                            End If
                    End Select
                Next
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function IsExists_NetLineRow(ByRef bRowExists As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsExists_NetLineRow"
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Loop through an array to find where type equals to 'NL'
            For lCount As Integer = 1 To (m_oXA.Rows.Count - 1) - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                    bRowExists = True
                End If
            Next
            Return result

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function

    Public Function AddFacultative(ByVal lPartyCnt As Integer, ByVal sDescription As String, ByVal dCommission As Double, ByVal bIsRIBroker As Boolean) As Integer

        Dim result As Integer = 0
        Dim lPriority As Integer
        Dim cPremiumTax, cCommTax As Decimal

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPosition As Integer
        Dim bIsExistGrossNetRow As Boolean

        Dim bIsExistNetLineRow As Boolean
        Dim sReinsurerApproved As String      'E016
        Const kMethodName As String = "AddFacultative"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set defaults
            lPriority = 65536

            ' Check for duplicate and get priority
            For lCount As Integer = m_lASumBand + 1 To m_lASumFAC - 1
                ' Validate
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt) Is DBNull.Value AndAlso ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt)) = lPartyCnt Then
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                    'GoTo Finally
                End If

                ' Check priority
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) Is DBNull.Value AndAlso ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority)) > lPriority Then
                    lPriority = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority)
                End If
            Next

            ' Get tax values (they may be value based)
            Dim objArgs As RecalculateFacTaxEventArgs = New RecalculateFacTaxEventArgs(0, lPartyCnt, 0, 0, cPremiumTax, cCommTax)
            RaiseEvent RecalculateFacTax(Me, objArgs)

            Dim lObligatoryIndex As Integer
            lObligatoryIndex = 0
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "T", "F"
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            lObligatoryIndex = lCount
                            Exit For
                        End If
                End Select
            Next
            If lObligatoryIndex <> 0 Then
                If Not FACXolExists And Not FACPropExists Then

                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                            m_oXA.Rows.RemoveAt(lCount)
                            m_oXA.AcceptChanges()
                            m_lASumTreaty -= 1
                            m_lASumTotal -= 1
                            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                            Exit For
                        End If
                    Next

                    lReturn = AllocateObligatory()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first,PMLogError")
                    End If
                End If
            End If

            ' ***********************************************************
            ' Determine the position after looking at the current RI arrangement
            ' and get the position at which we need to Insert the New Row
            m_lReturn = CType(FindInsertPosition("F", lPosition), gPMConstants.PMEReturnCode)

            m_lReturn = CType(IsExists_NetLineRow(bIsExistNetLineRow), gPMConstants.PMEReturnCode)

            If bIsExistNetLineRow Then
                lPosition += 1
            End If

            ' Insert new row on to that position
            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition)

            'Start E016
            If m_oBusiness Is Nothing Then
                m_lReturn = g_oObjectManager.GetInstance(
                        oObject:=m_oBusiness,
                        sClassName:="bSIRReinsuranceRI2007.Form",
                        vInstanceManager:=PMGetViaClientManager)
            End If

            m_lReturn = m_oBusiness.GetInsurerApprovedStatus(r_sInsurerApprovedStatus:=sReinsurerApproved,
                           v_lPartyCnt:=lPartyCnt)

            'End E016

            ' Populate new line
            ' Grid fields
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "FAC Prop"
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = sDescription
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) = dCommission / 100
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode) = ""
            'E016
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsReinsurerApproved) = sReinsurerApproved
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsRIBroker) = bIsRIBroker
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) = "MA"
            ' Supporting fields
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "F"

            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID) = Nothing
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt) = lPartyCnt
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) = lPriority + 1
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines) = 1
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = 0

            ' Increment total lines...
            m_lASumTreaty += 1
            m_lASumFAC += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            'Increase the count of FAC row
            RowCountFAC += 1
            grdRI.ReBind()
            ' ***********************************************************
            ' We need to insert Gross Net Row if it does'nt exists
            ' So find out whether Gross Net exists or not
            m_lReturn = CType(IsExists_GrossNetRow(bIsExistGrossNetRow), gPMConstants.PMEReturnCode)

            ' if it does'nt then Insert the New row and populate the
            ' the amounts accordingly
            If Not bIsExistGrossNetRow Then

                m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition + 1)

                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "GROSS NET"
                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Net of FAC"
                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FT"

                ' Increment total lines...
                m_lASumFAC += 1
                m_lASumTotal += 1
                m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
                m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
                m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
                m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

                m_lReturn = RollupXArray()
            End If

            If lObligatoryIndex <> 0 Then
                If FACPropExists Or FACPropExists Then
                    If Not m_oXA(lPosition + 2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lPosition + 2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                        m_oXA.Rows.RemoveAt(lPosition + 2)
                        m_oXA.AcceptChanges()
                        m_lASumTreaty -= 1
                        m_lASumTotal -= 1
                        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                        m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                        m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                    End If
                End If
            End If
            grdRI.ReBind()
            m_bIsDirty = True
            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '       Name :  DeleteRow
    '
    ' Description:  Designed to Delete the Current Highlighted Row and Produce
    '               the appropriate amounts accordingly

    ' ***************************************************************** '
    Public Function DeleteRow(Optional ByVal iAction As Integer = 0, Optional ByVal bManualDeleted As Boolean = True) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteRow"

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lReturn As Integer
        Dim bRowExists As Boolean
        Try

            If SelectedRIType = "F" Or SelectedRIType = "FX" Or SelectedRIType = "T" Or SelectedRIType = "TX" Or SelectedRIType = "R" Then

                m_bIsDirty = True
                Try
                    lReturn = gPMConstants.PMEReturnCode.PMTrue
                    RemoveHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                    m_oXA.Rows.RemoveAt(m_SelectedRow)
                    m_oXA.AcceptChanges()
                    AddHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                Catch ex As Exception
                    lReturn = gPMConstants.PMEReturnCode.PMFalse
                End Try

                If lReturn <> 1 Then
                    gPMFunctions.RaiseError("m_oXA.DeleteRows(m_lASumUnalloc, 1)", "Unable to remove unallocated totals")
                End If

                'Restore the Limits Again
                m_lReturn = StoreLimits()

                If bManualDeleted Then ' if Line deleted by user
                    ' Store an RiArrangementLine Id's into an array
                    If SelRIArrangementLine <> 0 Then

                        If m_vDeletedRIArragementIds Is Nothing Then
                            ReDim m_vDeletedRIArragementIds(0)
                            m_vDeletedRIArragementIds(0) = SelRIArrangementLine
                        Else
                            ReDim Preserve m_vDeletedRIArragementIds(m_vDeletedRIArragementIds.GetUpperBound(0) + 1)
                            m_vDeletedRIArragementIds(m_vDeletedRIArragementIds.GetUpperBound(0)) = SelRIArrangementLine
                        End If

                    End If
                End If

                If SelectedRIType = "F" Or SelectedRIType = "FX" Then
                    ' Decriment total lines...
                    m_lASumFAC -= 1
                    m_lASumTreaty -= 1
                    m_lASumTotal -= 1
                    m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                    m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                    m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                    m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)

                    ' ***********************************************************
                    ' Check to see that whether Gross Net row exists or Not.
                    ' If Exist and we are removing the last FAC row then
                    ' We need to delete the Gross Net also.
                    m_lReturn = CType(IsExists_GrossNetRow(bRowExists), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("DeleteRow", "IsExists_GrossNetRow Method Failed")
                    End If

                    RowCountFAC -= 1
                    If bRowExists And RowCountFAC = 0 Then
                        For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1 - 1
                            If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FT" Then
                                'lReturn = m_oXA.DeleteRows(lCount, 1)
                                Try
                                    lReturn = gPMConstants.PMEReturnCode.PMTrue
                                    RemoveHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                                    m_oXA.Rows.RemoveAt(m_SelectedRow)
                                    m_oXA.AcceptChanges()
                                    AddHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                                Catch ex As Exception
                                    lReturn = gPMConstants.PMEReturnCode.PMFalse
                                End Try


                                If lReturn <> 1 Then
                                    gPMFunctions.RaiseError("m_oXA.DeleteRows(m_lASumUnalloc, 1)", "Unable to delete the selected row")
                                End If
                                ' Decriment total lines...
                                m_lASumFAC -= 1
                                m_lASumTreaty -= 1
                                m_lASumTotal -= 1
                                m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                                m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                                m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                                m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)

                                Exit For
                            End If
                        Next
                    End If
                ElseIf SelectedRIType = "T" Or SelectedRIType = "TX" Or SelectedRIType = "R" Then
                    ' Decriment total lines...
                    m_lASumTreaty -= 1
                    m_lASumTotal -= 1
                    m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                    m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                    m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                    m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)

                End If

                ' ***********************************************************
                ' Call RollupXArray to refresh the amounts after the deletion
                ' of the row.

                m_lReturn = RollupXArray()
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        End Try
        Return result
    End Function

    Public Function AddTreaty(ByVal lTreatyID As Integer, ByVal sDescription As String, ByVal dCommission As Double, ByVal sAgreementCode As String, ByVal bIsRetained As Boolean, Optional ByRef sTransactionType As String = "T") As Integer

        Dim result As Integer = 0
        Dim lPriority As Integer
        Dim cPremiumTax, cCommTax As Decimal

        Dim lReturn, lPosition As Integer
        Const kMethodName As String = "AddTreaty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set defaults
            lPriority = 0

            ' Check for duplicate and get priority
            For lCount As Integer = m_lASumFAC + 1 To m_lASumTreaty
                ' Validate
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID) Is DBNull.Value AndAlso ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID)) = lTreatyID Then
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                    Return result
                End If

                ' Check priority
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) Is DBNull.Value AndAlso ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority)) > lPriority Then
                    lPriority = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority)
                End If
            Next

            ' Get tax values (they may be value based)
            Dim objArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(0, lTreatyID, 0, 0, cPremiumTax, cCommTax)
            RaiseEvent RecalculateTreatyTax(Me, objArgs)

            ' ***********************************************************
            ' Determine the position after looking at the current RI arrangement
            ' and get the position at which we need to Insert the New Row
            m_lReturn = CType(FindInsertPosition(sTransactionType, lPosition), gPMConstants.PMEReturnCode)

            ' Insert new row on to that position
            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition)

            ' Populate new line
            ' Grid fields
            If sTransactionType = "T" Then
                m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "Treaty QSH"
            Else
                m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "Treaty XOL"
            End If
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = sDescription
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) = dCommission / 100
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax
            If sTransactionType = "T" Then
                m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode) = sAgreementCode
            End If
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) = "MA" ' Manually Added
            ' Supporting fields
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = IIf(bIsRetained, "R", sTransactionType)
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID) = lTreatyID
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt) = Nothing
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) = lPriority + 1
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines) = 1
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = 0

            'Increase the Count of Treaty Row
            RowCountTreaty += 1

            ' Increment total lines...
            m_lASumTreaty += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '       Name :  FindInsertPosition
    '
    ' Parameters :
    '        Name:  sType
    '        Desc:  Type of arrangement we are adding. Can have
    '                   F(Facultative Proportional
    '                   FX(Facultative Proportional)
    '                   T(Treaty)
    '                   TX(Treaty XOL)
    '
    '        Name:  lPosition
    '        Desc:  Returns True if Gross Net Rows exists in the Grid else
    '               False

    '        Name:  cLowerLimit
    '        Desc:  Returns True if Gross Net Rows exists in the Grid else
    '               False

    ' Description:  Function Designed to find the position at which we need
    '               to insert the Treaty or Facultative rows if they are
    '               attached manually.
    '       Logic:  If Type = "F"
    '                   Put at the Last position of facultaive rows
    '               ElseIf Type = "FX"
    '                   Sort on the basis of Lower Limit with other FAC XOL rows
    '               ElseIf Type = "T"
    '                   Put at the Last position of Treaty rows
    '               ElseIf Type = "TX"
    '                   Sort on the basis of Lower Limit with other Treaty XOL rows
    ' ***************************************************************** '
    Public Function FindInsertPosition(ByRef sType As String, ByRef lPosition As Integer, Optional ByRef cLowerLimit As Decimal = 0, Optional ByRef bIsObligatory As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FindInsertPosition"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign Default Position to 1 as 0'th poistion row will always
            ' be occupied by Band Total Row
            lPosition = 1
            Select Case sType
                Case "F"
                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If bIsObligatory Then
                            lPosition = 1
                            Exit For
                        End If
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = sType Then
                            lPosition = lCount
                        Else
                            lPosition = lCount
                            Exit For
                        End If
                    Next
                Case "FX"
                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value Then
                            If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                                lPosition = lCount + 1
                            ElseIf gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                                lPosition = lCount + 1
                            ElseIf gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Then
                                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Is DBNull.Value Then
                                    If m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) < cLowerLimit Then
                                        lPosition = lCount + 1
                                    ElseIf m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) >= cLowerLimit Then
                                        lPosition = lCount
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    Next
                Case "T"
                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value Then
                            If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Or m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FT" Or m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TX" Then
                                lPosition = lCount
                            Else
                                lPosition = lCount
                                Exit For
                            End If
                        End If
                    Next
                Case "TX"
                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value Then
                            If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" _
                                Or gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FT" Then
                                lPosition = lCount + 1
                            ElseIf gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Then
                                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Is DBNull.Value Then
                                    If m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) < cLowerLimit Then
                                        lPosition = lCount
                                    ElseIf m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) >= cLowerLimit Then
                                        lPosition = lCount
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    Next
            End Select

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '       Name :  IsExists_GrossNetRow
    '
    ' Parameters :
    '        Name:  bRowsExists
    '        Desc:  Returns True if Gross Net Rows exists in the Grid else
    '               False
    ' Description:  Function Designed to find whether Gross Net Row exists
    '               in the Reinsurance Grid or not.
    '
    ' ***************************************************************** '
    Public Function IsExists_GrossNetRow(ByRef bRowExists As Boolean) As Integer

        'FT is the Type Given to Gross Net Row
        Dim result As Integer = 0
        Const kMethodName As String = "IsExists_GrossNetRow"

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Loop through an array to find where type equals to 'FT'
            For lCount As Integer = 1 To m_oXA.Rows.Count - 1 - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FT" Then
                    bRowExists = True
                End If
            Next

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
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
            ''Debugger.Break()
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Sub FinaliseEdit()
        Try

            ' Certain function won't trigger an edit to commit. Force this by changing column
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
            ' Store the arrangement objects
            m_oRIArrangement = oRIArrangement
            m_oRIOriginal = oRIOriginal

            ' Create the xarray and load from supplied raw array
            m_oXA = New XArrayHelper()
            'Sort Array

            If Information.IsArray(oRIArrangement.ReinsuranceLines) Then
                m_bRIEmpty = False
                bPMFunc.TransposeArray(oRIArrangement.ReinsuranceLines)
                m_oXA.LoadRows(oRIArrangement.ReinsuranceLines)
                bPMFunc.TransposeArray(oRIArrangement.ReinsuranceLines)
            Else
                ' Create band header row and set flag indicating we have done so
                m_bRIEmpty = True
                m_oXA.RedimXArray(New Integer() {0, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Max}, New Integer() {0, 0})
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
            m_oXA.AcceptChanges()
            If grdRI.Columns.Count > 15 Then
                For colCtr As Integer = 15 To grdRI.Columns.Count - 1
                    grdRI.Columns(colCtr).Visible = False
                Next
            End If
            grdRI_Enter()
            m_lReturn = StoreLimits()

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

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
            vRI = Nothing

            ' We need to walk the XArray and remove any genuine RI rows
            For lSource As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
                ' Check row type
                If iPMFunc.IsIn(m_oXA(lSource, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type), "R", "T", "F", "FX", "TX", "TFS", "TC") Then
                    ' Prep main array
                    If lDest = 0 Then
                        ReDim vRI(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Max, lDest)
                    Else
                        ReDim Preserve vRI(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Max, lDest)
                    End If

                    ' Copy all row data
                    For lCol As Integer = 0 To RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Max
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

    ' ***************************************************************** '
    '       Name :  ExpandXArray
    '
    ' Description:  Function Designed to insert the
    '                    Band Total
    '                    Gross Net
    '
    '               Rows to the Grid
    '
    ' ***************************************************************** '
    Private Function ExpandXArray() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ExpandXArray"

        Dim bIsExistNetLineRow As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(SortGrid("TC"), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SortGrid("TX"), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SortGrid("TFS"), gPMConstants.PMEReturnCode)
            ' ***********************************************************
            ' The Row in the grid will be Band Total which will display the total
            ' of Sun Insured and Premium
            ' Below is the code to insert the new row for Band Total
            If Not m_bRIEmpty Then
                m_oXA.Rows.InsertAt(m_oXA.NewRow, 0)
                m_oXA.AppendRows()
            End If

            ' Populate row
            m_lASumBand = 0
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "GROSS"
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Band Total"
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = m_oRIArrangement.SumInsured
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = m_oRIArrangement.Premium
            m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "BT"

            ' ***********************************************************
            m_lASumTreaty = 0

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                    m_bIsObligatoryExists = True
                    Exit For
                End If
            Next
            ' ***********************************************************
            ' We must now walk the Facultative rows and put its count in
            m_lASumObligatoryTreaty = 0
            For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value Then
                    If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then
                        If m_bIsObligatoryExists Then
                            m_lASumFAC = lCount
                        Else
                            m_lASumFAC = lCount + 1
                        End If
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Then
                            If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) Is DBNull.Value AndAlso
                            gPMFunctions.ToSafeLong(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), 0) = 0 Then
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Multiple Acts"
                            End If
                        End If
                    ElseIf gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Then
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            m_lASumObligatoryTreaty = lCount
                        End If
                    End If
                End If
            Next
            If m_lASumObligatoryTreaty > 0 Then

                m_lReturn = CType(IsExists_NetLineRow(bIsExistNetLineRow), gPMConstants.PMEReturnCode)
                If Not bIsExistNetLineRow Then
                    m_oXA.Rows.InsertAt(m_oXA.NewRow, gPMConstants.kRINetLineObligatory)
                    m_oXA.AppendRows()
                    m_oXA(2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "Net Line"
                    m_oXA(2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "NL"
                    ' Increment total lines...
                    If m_lASumFAC <> 0 Then
                        m_lASumFAC += 1
                    End If
                    m_lASumTotal += 1
                    m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
                    m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
                    m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
                    m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)
                End If

            End If

            ' ***********************************************************
            ' If we found any faculative row the Insert Gross Net Row at the
            ' Specified position and if not then don't diaplay the Gross Net row
            If m_lASumFAC > 0 Then
                m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumFAC)
                m_oXA.AppendRows()
            End If

            ' Populate treaty summary (no values, these are done elsewhere)
            If m_lASumFAC > 0 Then

                If m_bAgency Then
                    m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "GROSS NET"
                    m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Net of FAC"
                Else
                    m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "GROSS NET"
                    m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Net of FAC"
                End If

                m_oXA(m_lASumFAC, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FT" 'previously treaty total now Fac total
            End If
            ' ***********************************************************
            m_lASumTreaty = (m_oXA.Rows.Count - 1) + 1

            ' Add allocated total
            If Not m_bReadOnly Then
                m_oXA.AppendRows()

                ' Set allocated line
                m_lASumAlloc = m_oXA.Rows.Count - 1
                m_oXA(m_lASumAlloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Allocated"
                m_oXA(m_lASumAlloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "AT"
            End If

            ' ***********************************************************
            ' Do not add unallocated here it will be added, if
            ' necessary during totalling
            ' ***********************************************************
            ' If original ri is present add original and net values
            If Not (m_oRIOriginal Is Nothing) And (Not m_bReadOnly) Then

                m_oXA.AppendRows()
                m_oXA.AppendRows()
                ' Set original line
                m_lASumOriginal = (m_oXA.Rows.Count - 1) - 1
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Original RI Totals"
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = m_oRIOriginal.SumInsured
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = m_oRIOriginal.Premium
                m_oXA(m_lASumOriginal, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "OT"

                ' Set allocated line
                m_lASumNet = m_oXA.Rows.Count - 1
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Net"
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = m_oRIArrangement.SumInsured + m_oRIOriginal.SumInsured
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = m_oRIArrangement.Premium + m_oRIOriginal.Premium
                m_oXA(m_lASumNet, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "NT"
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' Function Designed to insert the Band Total,Gross Net,Allocated,Unallocated Rows to the Grid
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RollupXArray() As Integer

        Dim nResult As Integer = 0
        Dim oTtyTotal, oFacTotal, oFACPropTotal, oGrossTotal, oGrossNet, oAllocated, oUnAllocated, oRetained As RiskTotalizer2007
        Dim iPositionRetainedLine As Integer
        Dim nReturn As PMEReturnCode
        Dim cPremiumTax, cCommTax As Decimal

        ' Keeps the line limit of the retained line
        Dim crRetainedLineLimit As Decimal
        Dim nTXolPremiumTotal As Double
        Dim nCATXolPremiumTotal As Double
        Dim nTCXolPremiumTotal As Double
        Dim nCedePremiumMark As Integer
        Dim lTXolUpperLimit As Double
        Const kMethodName As String = "RollupXArray"
        Dim crRunningSumInsured As Decimal

        Try

            RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated

            nResult = PMEReturnCode.PMTrue

            ' Create totalizers
            oTtyTotal = New RiskTotalizer2007()
            oFacTotal = New RiskTotalizer2007()
            oFACPropTotal = New RiskTotalizer2007()
            ' To Keep the amounts of Gross Net row
            oGrossNet = New RiskTotalizer2007()

            ' To Keep the Gross totals
            oGrossTotal = New RiskTotalizer2007()

            ' To Keep the amounts of Allocated row
            oAllocated = New RiskTotalizer2007()

            oRetained = New RiskTotalizer2007()

            RowCountFAC = 0
            RowCountTreaty = 0
            RowCountRetained = 0
            m_cTotalSumInsuredFacProp = 0
            ' ***********************************************************
            ' Walk through the array and determine the Facultive and Treaty Rows
            ' and put their totals in corresponding objects. We will be using it
            ' to calculate Gross Net, Allocated and Unallocated amounts

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "F", "FX"
                        RowCountFAC += 1
                        ' # check introduced for FX when SI or Premium already exaust
                        ' # Abs check put for Original RI lines where SI coming -ve
                        If (Math.Abs(m_oRIArrangement.SumInsured) - Math.Abs(oFacTotal.SumInsured)) <= 0 Then
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = 0
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = 0
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = 0
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = 0
                        End If
                        oFacTotal.Add(m_oXA, lCount)

                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                            oFACPropTotal.Add(m_oXA, lCount)
                            m_cTotalSumInsuredFacProp = oFACPropTotal.SumInsured
                            m_bIsFacPropExists = True
                        End If
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Then
                            m_bIsFacxolExists = True
                        End If
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            m_bIsObligatoryExists = True
                        End If
                    Case "T"
                        RowCountTreaty += 1
                        lTXolUpperLimit = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)
                        m_bIsNetLineNeeded = True
                    Case "TX", "TC"
                        RowCountTreaty += 1
                    Case "R"
                        If RowCountRetained = 0 Then
                            iPositionRetainedLine = lCount
                            crRetainedLineLimit = m_oXA(iPositionRetainedLine, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)
                        End If
                        RowCountRetained += 1
                        oRetained.Add(m_oXA, lCount)
                    Case "TFS"
                        lTXolUpperLimit = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)
                End Select
            Next
            ' ***********************************************************
            ' Put the Totals in oGrossTotal object
            oGrossTotal.SumInsured = m_oRIArrangement.SumInsured
            oGrossTotal.Premium = m_oRIArrangement.Premium

            ' Put the Totals in oGrossNet object which will always be equal to
            ' Gross Total - Fac Total
            oGrossNet.SumInsured = m_oRIArrangement.SumInsured - oFacTotal.SumInsured
            oGrossNet.Premium = m_oRIArrangement.Premium - oFacTotal.Premium

            Dim dObligatorySumInsured, dObligatoryPremium As Double

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "T" 'This is for recalculate Treaty
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" And RowCountFAC = 0 Then
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * oGrossNet.SumInsured
                            dObligatorySumInsured = oGrossNet.SumInsured - m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)
                            dObligatoryPremium = oGrossNet.Premium - m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) 'PN 71440

                        Else
                            If Math.Abs(dObligatorySumInsured - 0) > 0.005 Then
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * dObligatorySumInsured

                                If gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)) > (gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit))) Then
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)))
                                End If

                            Else
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * (oGrossNet.SumInsured)
                                If Math.Abs(oTtyTotal.SumInsured + m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)) > Math.Abs(oGrossNet.SumInsured) Then
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)) * (oGrossNet.SumInsured)
                                End If

                                If Math.Abs(gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured))) > (gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit))) Then
                                    If gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)) >= 0 Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)))
                                    Else
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit))) * -1
                                    End If
                                End If



                                RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                                RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                                If oGrossNet.SumInsured = 0 Then
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = 0
                                Else
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)) / gPMFunctions.ToSafeDouble(oGrossNet.SumInsured)
                                End If
                                AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                                AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated

                            End If
                        End If
                        oTtyTotal.Add(m_oXA, lCount)

                    Case "NL"
                        If m_bIsNetLineNeeded Then
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) - m_oXA(m_lASumBand + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = m_oXA(m_lASumBand, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) - m_oXA(m_lASumBand + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)
                        End If

                End Select
            Next

            'Adjust the Treaty XOL Totals
            If m_oRIArrangement.IsExtendedLimitApplied <> True Or m_oRIArrangement.ExtendedLimitamount >= oGrossNet.SumInsured Or m_oRIArrangement.ExtendedLimitamount = 0 Then
                If Not m_bReadOnly Then
                    For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                        Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                            Case "TX"
                                If m_oRIArrangement.RIModelId <> m_oRIArrangement.XOLRIModelId And TransactionType <> "PT" Then
                                    If m_oRIArrangement.ExtendedLimitamount = 0 Then
                                        m_oRIArrangement.ExtendedLimitamount = lTXolUpperLimit
                                    End If

                                    nCedePremiumMark = ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CedePremiumOnly), 0)
                                    If (lTXolUpperLimit - oTtyTotal.SumInsured) >= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) Then
                                        If nCedePremiumMark = 1 Then
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                        Else
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit), 0) - ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)
                                        End If
                                    ElseIf (lTXolUpperLimit - oTtyTotal.SumInsured) < m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) And (lTXolUpperLimit - oTtyTotal.SumInsured) > m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        If nCedePremiumMark = 1 Then
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                        Else
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(lTXolUpperLimit - oTtyTotal.SumInsured, 0) - ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)
                                        End If
                                    ElseIf (lTXolUpperLimit - oTtyTotal.SumInsured) <= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                    End If
                                Else
                                    nCedePremiumMark = ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CedePremiumOnly), 0)
                                    If (oGrossNet.SumInsured - oTtyTotal.SumInsured) >= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) Then
                                        If nCedePremiumMark = 1 Then
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                        Else
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit), 0) - ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)
                                        End If
                                    ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured) < m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) And
                                       (oGrossNet.SumInsured - oTtyTotal.SumInsured) > m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        If nCedePremiumMark = 1 Then
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                        Else
                                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(oGrossNet.SumInsured - oTtyTotal.SumInsured, 0) - ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)
                                        End If
                                    ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured) <= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                    End If
                                End If
                            Case "TC"
                                nCedePremiumMark = ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CedePremiumOnly), 0)
                                If nCedePremiumMark = 1 Then
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                Else
                                    If (oGrossNet.SumInsured - oTtyTotal.SumInsured) >= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)
                                    ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured) < m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) And (oGrossNet.SumInsured - oTtyTotal.SumInsured) > m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = gPMFunctions.ToSafeDouble(oGrossNet.SumInsured - oTtyTotal.SumInsured, 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)
                                    ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured) <= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                    End If
                                End If
                        End Select
                    Next
                End If
            Else
                If Not m_bReadOnly Then
                    For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                        Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                            Case "TX", "TC"
                                nCedePremiumMark = ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CedePremiumOnly), 0)
                                If nCedePremiumMark = 1 Then
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                Else

                                    If (m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured) >= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit), 0) - ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)

                                    ElseIf (m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured) < m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) And (m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured) > m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = ToSafeDouble(m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured, 0) - ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit), 0)

                                    ElseIf (m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured) <= m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0

                                    End If
                                End If
                        End Select
                    Next
                End If
            End If
            'Recalculate the Treaty Totals
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "TX", "TC"
                        oTtyTotal.Add(m_oXA, lCount)
                End Select
            Next

            ' ***********************************************************
            ' As premium for all XOL treaties will be calculated on the
            ' Gross Net Premium * Ceded rate. So reset all the premiums
            ' as soon as we are able to get the Gross Net Premium row
            oTtyTotal.Premium = 0
            RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
            If Not m_bReadOnly Then

                For countTreaty As Integer = m_lASumFAC + 1 To m_lASumFAC + RowCountTreaty + 1
                    If Not m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Then ' To Confirm
                        If Not m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode)) = "AU" Then
                            If oGrossNet.SumInsured <> 0 Then
                                If dObligatorySumInsured > 0 And gPMFunctions.ToSafeString(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) <> "1" Then
                                    m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) / dObligatorySumInsured 'PN 71440
                                Else
                                    m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) / oGrossNet.SumInsured
                                End If
                            Else
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)
                            End If
                        End If

                        If dObligatorySumInsured > 0 And gPMFunctions.ToSafeString(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) <> "1" Then
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = dObligatoryPremium * (gPMFunctions.ToSafeCurrency(Conversion.Val(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)), 0))
                        Else
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = oGrossNet.Premium * (Conversion.Val(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)))
                        End If
                        nTXolPremiumTotal += m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)

                    ElseIf m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TX" Or m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TC" Then
                        If Trim(UCase(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ReinsuranceType))) = "CAT" Then
                            If m_sTransactionType = "PT" Or m_nRIVersionId > 1 Then
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
                            Else
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round((oGrossNet.Premium - nTXolPremiumTotal - nTCXolPremiumTotal) * ToSafeDouble(Val(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)), 0), 4)
                            End If
                            nCATXolPremiumTotal = nCATXolPremiumTotal + m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)
                        Else
                            If m_sTransactionType = "PT" Or m_nRIVersionId > 1 Then
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
                            Else
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round((oGrossNet.Premium - nTXolPremiumTotal) * ToSafeDouble(Val(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)), 0), 4)
                            End If
                            nTCXolPremiumTotal = nTCXolPremiumTotal + m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)
                        End If
                        m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)

                        Dim objArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                        RaiseEvent RecalculateTreatyTax(Me, objArgs)

                        ' Store new values
                        m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
                        m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax


                    End If
                Next
            End If
            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
            ' ***********************************************************

            ' ***********************************************************
            ' Re-calculate the Treaty Total
            ' Treaty arrangeemnt will have T,TX,R reinsurance types
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "T", "TX", "TC" ', "TFS" ', "R"
                        oTtyTotal.Premium += m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)
                        m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 0) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent), 0)
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) > 0 Then
                            Dim objArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID), m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objArgs)

                            ' Store new values

                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax
                        End If
                    Case "FX"
                        If Math.Abs(gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 0) - 0) > 0.005 Then
                            m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) = gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), 0) / gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 0)
                        End If
                End Select
            Next
            ' ***********************************************************

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                End Select
            Next
            'End (Sriram P)PN52639



            ' ***********************************************************
            ' Produce the allocated and Unallocated rows on the basis scenarios
            ' of Fac and Treaty Totals
            If Not m_bReadOnly Then

                '       Set allocated = Gross Totals
                If RowCountRetained <> 0 Then
                    If oGrossTotal.SumInsured > (oTtyTotal.SumInsured + oFacTotal.SumInsured) Then
                        If (m_oRIArrangement.IsExtendedLimitApplied = False Or m_oRIArrangement.ExtendedLimitamount > oGrossNet.SumInsured) Or m_oRIArrangement.ExtendedLimitamount = 0 Then
                            If (oGrossTotal.SumInsured - oTtyTotal.SumInsured - oFacTotal.SumInsured) <= crRetainedLineLimit Then
                                oRetained.SumInsured = oGrossTotal.SumInsured - oFacTotal.SumInsured - oTtyTotal.SumInsured
                                oRetained.Premium = oGrossTotal.Premium - oFacTotal.Premium - oTtyTotal.Premium
                            Else
                                oRetained.SumInsured = crRetainedLineLimit
                                oRetained.Premium = oGrossTotal.Premium - oFacTotal.Premium - oTtyTotal.Premium
                            End If
                            'Need to calculate Sharepercent and Premiumpercent according to SUminusred and Premium allocated to Retained
                            'I am changing this as per SP calulating Sharepercent and premiumprecent
                            oRetained.SharePercent = (oRetained.SumInsured / (oGrossTotal.SumInsured - oFacTotal.SumInsured))
                        Else
                            If (m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured - oFacTotal.SumInsured) <= crRetainedLineLimit Then
                                oRetained.SumInsured = m_oRIArrangement.ExtendedLimitamount - oTtyTotal.SumInsured
                                oRetained.Premium = oGrossTotal.Premium - oFacTotal.Premium - oTtyTotal.Premium
                            Else
                                oRetained.SumInsured = crRetainedLineLimit
                                oRetained.Premium = oGrossTotal.Premium - oFacTotal.Premium - oTtyTotal.Premium
                            End If
                        End If
                    Else
                        oRetained.SumInsured = 0
                        oRetained.Premium = oGrossTotal.Premium - oFacTotal.Premium - oTtyTotal.Premium
                        oRetained.SharePercent = 0
                    End If
                End If

                Dim dSILeft As Double
                Dim dTFSPremium As Double
                Dim lCount As Integer
                dTFSPremium = 0

                dSILeft = m_oRIArrangement.SumInsured - (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured)
                For lCount = 0 To m_oXA.Rows.Count - 1
                    Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                        Case "TFS"
                            If dSILeft > 0 Then
                                If m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) * m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines) > dSILeft Then
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = dSILeft
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round(oGrossNet.Premium * (dSILeft / oGrossNet.SumInsured), 4, MidpointRounding.AwayFromZero)
                                Else
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) * m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines)
                                    m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round(oGrossNet.Premium * (m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) / oGrossNet.SumInsured), 4, MidpointRounding.AwayFromZero)
                                End If
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) / oGrossNet.SumInsured
                                dTFSPremium = dTFSPremium + m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)
                                dSILeft = dSILeft - m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = dTFSPremium * m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)
                            Else
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = 0
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = 0
                                m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = 0
                            End If
                    End Select
                Next

                Dim dTotalTSQ As Double
                Dim dTotalTX As Double
                Dim countTreaty As Integer
                Dim dTotalTC As Double
                dTotalTSQ = 0
                dTotalTX = 0
                dTotalTC = 0
                If Not m_bReadOnly Then
                    For countTreaty = 0 To m_oXA.Rows.Count - 1
                        If m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "T" Then ' To Confirm
                            dTotalTSQ = dTotalTSQ + ToSafeCurrency(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 0)
                        ElseIf m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TX" Then
                            If m_sTransactionType = "PT" Or m_nRIVersionId > 1 Then
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
                            Else
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round((oGrossNet.Premium - dTFSPremium - dTotalTSQ) * Math.Round(Val(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)), 6), 4)
                            End If
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)
                            dTotalTX = dTotalTX + ToSafeCurrency(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 0)
                            Dim objArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objArgs)
                            ' Store new values
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax

                        ElseIf m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TC" Then
                            If m_sTransactionType = "PT" Or m_nRIVersionId > 1 Then
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = 0
                            Else
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round((oGrossNet.Premium - dTFSPremium - dTotalTSQ - dTotalTX) * Val(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare)), 4)
                            End If
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)
                            dTotalTC = dTotalTC + ToSafeCurrency(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 0)
                            Dim objArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objArgs)
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax
                        ElseIf m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "R" Then
                            m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round((oGrossNet.Premium - dTotalTSQ - dTotalTX - dTFSPremium - dTotalTC), 4)
                            oRetained.Premium = Math.Round(m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), 4)
                        End If
                    Next
                End If

                oTtyTotal.Clear()
                ' Recalculate Treaty Totals again
                For lCount = 0 To m_oXA.Rows.Count - 1
                    Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                        Case "TX", "TC", "T", "TFS"
                            oTtyTotal.Add(m_oXA, lCount)
                    End Select
                Next

                ' If there is no retained line then make sure no Retained Premium and
                ' Share Percentage go - to avoid problem of more allocation to allocated line
                If RowCountRetained = 0 Then
                    oRetained.SumInsured = 0
                    oRetained.Premium = 0
                    oRetained.SharePercent = 0
                Else
                    For countTreaty = 0 To m_oXA.Rows.Count - 1
                        If m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "R" Then
                            If (oGrossTotal.Premium - (oTtyTotal.Premium + oFacTotal.Premium) > 0) And
                                        (oGrossTotal.Premium - (oTtyTotal.Premium + oFacTotal.Premium) <= crRetainedLineLimit) Then
                                m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = oGrossTotal.Premium - (oTtyTotal.Premium + oFacTotal.Premium)
                                oRetained.Premium = m_oXA(countTreaty, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)
                            End If
                        End If
                    Next
                End If
                If m_sTransactionType = "MTC" And Math.Abs(m_oRIArrangement.SumInsured - (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured)) > 0.005 Then
                    oRetained.SumInsured = m_oRIArrangement.SumInsured - (oTtyTotal.SumInsured + oFacTotal.SumInsured)
                End If
                If oGrossTotal.SumInsured >= (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured) Then
                    oAllocated.SumInsured = oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured
                    oAllocated.Premium = oTtyTotal.Premium + oFacTotal.Premium + oRetained.Premium
                    oAllocated.SharePercent = oTtyTotal.SharePercent + oFacTotal.SharePercent + oRetained.SharePercent
                ElseIf oGrossTotal.SumInsured < (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured) Then
                    oAllocated.SumInsured = oGrossTotal.SumInsured
                    oAllocated.Premium = oGrossTotal.Premium
                    oAllocated.SharePercent = oTtyTotal.SharePercent + oFacTotal.SharePercent
                End If

                If iPositionRetainedLine > 0 Then
                    oRetained.Store(m_oXA, iPositionRetainedLine)
                End If
                ' Store the Allocated amounts
                oAllocated.Store(m_oXA, m_lASumAlloc)

                ' ***********************************************************
                ' Check unallocated and add/remove as necessary
                If Math.Abs((oTtyTotal.Premium + oFacTotal.Premium + oRetained.Premium) - oGrossTotal.Premium) > 0.005 Or Math.Abs((oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured) - m_oRIArrangement.SumInsured) > 0.005 Then
                    ' Display unallocated line
                    If m_lASumUnalloc = 0 Then ' To Confirm
                        If m_lASumOriginal > 0 Then
                            ' Insert before original summary and move summaries
                            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumOriginal)

                            m_lASumUnalloc = m_lASumOriginal
                            m_lASumOriginal += 1
                            m_lASumNet += 1
                        Else
                            ' Append to grid
                            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_oXA.Rows.Count)

                            ''This check I have added because AppendRows need to be called twice at the beginning only, calling 
                            ''it once does not add any row to the m_oXA DataTable. The following check ensures that
                            ''second time the AppendRows is not called, otherwise two blank rows will get inserted.
                            m_lASumUnalloc = m_oXA.Rows.Count - 1
                        End If
                    End If
                    RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                    RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                    ' Populate
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Unallocated"
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = 1 - (oTtyTotal.SharePercent + oFacTotal.SharePercent + oRetained.SharePercent)
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = m_oRIArrangement.SumInsured - (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured)
                    'To check the unallocated RI amount
                    m_lunAllocatedRI = m_oRIArrangement.SumInsured - (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured)
                    m_lUnallocatedPremium = Math.Round(m_oRIArrangement.Premium - (oTtyTotal.Premium + oFacTotal.Premium + oRetained.Premium), 2)
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = m_oRIArrangement.Premium - (oTtyTotal.Premium + oFacTotal.Premium + oRetained.Premium)
                    m_oXA(m_lASumUnalloc, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "UT"
                Else
                    ' If line exists remove it
                    m_lunAllocatedRI = m_oRIArrangement.SumInsured - (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured)
                    m_lUnallocatedPremium = Math.Round(m_oRIArrangement.Premium - (oTtyTotal.Premium + oFacTotal.Premium + oRetained.Premium), 2)

                    If m_lASumUnalloc > 0 Then
                        Try
                            nReturn = PMEReturnCode.PMTrue
                            m_oXA.Rows.RemoveAt(m_lASumUnalloc)
                        Catch ex As Exception
                            nReturn = PMEReturnCode.PMFalse
                        End Try

                        m_oXA.AcceptChanges()

                        If nReturn <> PMEReturnCode.PMTrue Then
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

            For lCounter As Long = 1 To m_oXA.Rows.Count - 1
                If m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Or m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "F" _
                Or m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TX" Or m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "T" _
                Or m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "R" _
                    Or m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TC" And m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "TFS" Then
                    If m_oRIArrangement.Premium <> 0 Then
                        m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) / m_oRIArrangement.Premium
                    End If
                End If
            Next

            Dim lObligatoryIndex As Integer
            lObligatoryIndex = 0
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "T", "F"
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            lObligatoryIndex = lCount
                            Exit For
                        End If

                End Select
            Next
            If lObligatoryIndex <> 0 Then
                If FACPropExists And lObligatoryIndex <> 1 Or m_bIsFacxolExists And lObligatoryIndex <> 1 Then

                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                            m_oXA.Rows.RemoveAt(lCount)
                            m_oXA.AcceptChanges()
                            m_lASumTreaty -= 1
                            m_lASumTotal -= 1
                            m_lASumFAC -= 1
                            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                            Exit For
                        End If
                    Next

                    nReturn = AllocateObligatory()
                    If nReturn <> PMEReturnCode.PMTrue Then

                        gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first,PMLogError")
                    Else
                        If Not m_oXA(RowCountFAC + 2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(RowCountFAC + 2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            m_oXA.Rows.RemoveAt(RowCountFAC + 2)
                            m_oXA.AcceptChanges()
                        End If
                    End If
                Else
                    oGrossNet.Store(m_oXA, m_lASumFAC)
                End If

            Else
                oGrossNet.Store(m_oXA, m_lASumFAC)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally
            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
        End Try
        Return nResult
    End Function




    Private Sub grdRI_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdRI.CellFormatting
        If Not e.Value Is DBNull.Value Then
            Dim dTempVal As Double = 0D
            If e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Retained Then
                If (Double.TryParse(Convert.ToString(e.Value).Replace("%", "").Trim, dTempVal)) Then
                    e.Value = (dTempVal).ToString("P4")
                End If
            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare Then
                If (Double.TryParse(Convert.ToString(e.Value).Replace("%", "").Trim, dTempVal)) Then
                    e.Value = dTempVal.ToString("P4")
                End If

            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
                If (Double.TryParse(Convert.ToString(e.Value).Replace("%", "").Trim, dTempVal)) Then
                    e.Value = dTempVal.ToString("P4")
                End If
            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare Then
                If (Double.TryParse(Convert.ToString(e.Value).Replace("%", "").Trim, dTempVal)) Then
                    e.Value = (dTempVal).ToString("P2")
                End If
            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit _
            Or e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit Then
                Dim dValue As Double = 0D
                Double.TryParse(e.Value, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dValue)
                e.Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=dValue)
            ElseIf e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured Or
                   e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium Or
                   e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax Or
                   e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission Or
                   e.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax Then
                Dim dVal As Decimal = 0D
                Decimal.TryParse(e.Value, dVal)
                If dVal = 0D Then
                    e.Value = "0.00"
                Else
                    e.Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDecimal, vFieldValue:=Decimal.Round(dVal, 2).ToString)
                End If
            End If
        Else
            e.Value = e.Value
        End If
    End Sub

    ' ***************************************************************** '
    '                           GRID EVENTS
    ' ***************************************************************** '
    Private Sub grdRI_CellUpdated(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles grdRI.CellUpdated
        If Not grdRI.CurrentCell Is Nothing Then
            If Not grdRI.CurrentCell.Equals(grdRI.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        Dim ColIndex As Integer = eventArgs.ColumnIndex

        Dim lRow As Integer
        Dim cPremiumTax, cCommTax As Decimal
        Dim dObligatorySumInsured As Double
        Dim dObligatoryPremium As Double
        Dim bIsObligatory As Boolean = False
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

                'Set dirty and recalculate any affected data
                m_bIsDirty = True

                dObligatorySumInsured = m_oRIArrangement.SumInsured
                dObligatoryPremium = m_oRIArrangement.Premium
                If iPMFunc.IsIn(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type), "F") Then
                    ' Sum insured will affect share and premiums
                    If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare Then
                        ' Recalculate share if we can

                        Dim dTempThisShare As Double = 0D
                        If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) Is DBNull.Value Then
                            If gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)).Contains("%") Then
                                dTempThisShare = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare).ToString.Replace("%", "")), 0) / 100
                            Else
                                dTempThisShare = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)), 0) / 100
                            End If
                            RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                            RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = dTempThisShare
                            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                        End If
                    End If
                End If

                If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
                    Dim dTempCommPer As Double = 0D
                    If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) Is DBNull.Value Then
                        If gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).Contains("%") Then
                            dTempCommPer = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent).ToString.Replace("%", "")), 0) / 100
                        Else
                            dTempCommPer = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)), 0) / 100
                        End If
                        RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                        RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) = dTempCommPer
                        AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                        AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                    End If
                End If

                For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                    If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value Then
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            bIsObligatory = True
                            dObligatorySumInsured -= gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)), 0)
                            dObligatoryPremium -= gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)), 0)
                            Exit For
                        End If
                    End If
                Next

                If iPMFunc.IsIn(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type), "F") Then
                    ' Sum insured will affect share and premiums
                    If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare Then
                        ' Recalculate share if we can

                        If m_bIsObligatoryExists Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = Math.Round(gPMFunctions.ToSafeDouble(dObligatorySumInsured, 0) * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round(gPMFunctions.ToSafeDouble(dObligatoryPremium, 0) * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare), 4)
                        Else
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = Math.Round(gPMFunctions.ToSafeDouble(m_oRIArrangement.SumInsured, 0) * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                            ' store actual rounded value to calculate correct net offs
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round(gPMFunctions.ToSafeDouble(m_oRIArrangement.Premium, 0) * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare), 4)
                        End If

                    End If
                    ' SI, Premium or commission rate will affect commission and taxes
                    If iPMFunc.IsIn(gPMFunctions.ToSafeString(ColIndex), RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) Then
                        ' Recalculate premium share, if we can
                        If m_oRIArrangement.Premium <> 0 Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) / m_oRIArrangement.Premium
                        End If
                    End If

                    If iPMFunc.IsIn(gPMFunctions.ToSafeString(ColIndex), RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) Then
                        ' Recalculate commission
                        If gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).IndexOf("%"c) >= 0 Then
                            'PN #32170 Need to remove % sign
                            'This situation will occur in compiled exe only
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * (Conversion.Val(gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).Replace("%", "")) / 100)
                        Else
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * Conversion.Val(gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).Replace("%", ""))
                        End If

                        ' Flag commission as amended
                        If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = 1
                        End If

                        ' Recalculate taxes
                        Dim objArgs As Object
                        If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then
                            ' Get new fax taxes
                            objArgs = New RecalculateFacTaxEventArgs(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateFacTax(Me, objArgs)
                        Else
                            ' Get new treaty taxes
                            objArgs = New RecalculateTreatyTaxEventArgs(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objArgs)
                        End If

                        ' Store new values
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax
                    End If

                End If

                ' Check for appropriate row type
                If iPMFunc.IsIn(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type), "T", "R", "F", "TX", "FX") Then
                    ' Sum insured will affect share and premiums
                    If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured Then
                        ' Recalculate share if we can
                        If m_oRIArrangement.SumInsured <> 0 Then
                            If bIsObligatory AndAlso dObligatorySumInsured > 0 Then
                                m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) / dObligatorySumInsured
                            Else
                                m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) / m_oRIArrangement.SumInsured
                            End If
                        End If

                        ' Recalculate premium
                        ' Note: This will override non-prop fac premiums
                        If bIsObligatory Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round(dObligatoryPremium * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare), 4)
                        Else
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = Math.Round(m_oRIArrangement.Premium * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare), 4)
                        End If

                    End If

                    If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium And gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                        If m_oRIArrangement.Premium <> 0 Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) / m_oRIArrangement.Premium
                        End If
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = Math.Round(gPMFunctions.ToSafeDouble(m_oRIArrangement.SumInsured, 0) * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = gPMFunctions.ToSafeDouble(m_oRIArrangement.Premium, 0) * m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)
                    End If

                    ' SI, Premium or commission rate will affect commission and taxes
                    If iPMFunc.IsIn(gPMFunctions.ToSafeString(ColIndex), RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) Then
                        ' Recalculate premium share, if we can
                        If m_oRIArrangement.Premium <> 0 Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) / m_oRIArrangement.Premium
                        End If
                    End If

                    ' SI, Premium or commission rate will affect commission and taxes
                    If iPMFunc.IsIn(gPMFunctions.ToSafeString(ColIndex), RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) Then
                        ' Recalculate commission
                        If gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).IndexOf("%"c) >= 0 Then
                            'PN #32170 Need to remove % sign
                            'This situation will occur in compiled exe only
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * (Conversion.Val(gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).Replace("%", "")) / 100)
                        Else
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * Conversion.Val(gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)).Replace("%", ""))
                        End If

                        ' Flag commission as amended
                        If ColIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
                            m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = 1
                        End If

                        ' Recalculate taxes
                        Dim objEventArgs As Object
                        If Not m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then
                            ' Get new fax taxes
                            objEventArgs = New RecalculateFacTaxEventArgs(gPMFunctions.ToSafeInteger(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID)), gPMFunctions.ToSafeInteger(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt)), gPMFunctions.ToSafeDecimal(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)), gPMFunctions.ToSafeDecimal(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission)), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateFacTax(Me, objEventArgs)
                        Else
                            ' Get new treaty taxes
                            objEventArgs = New RecalculateTreatyTaxEventArgs(gPMFunctions.ToSafeInteger(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID)), gPMFunctions.ToSafeInteger(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID)), gPMFunctions.ToSafeDecimal(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)), gPMFunctions.ToSafeDecimal(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission)), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objEventArgs)
                        End If

                        ' Store new values
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objEventArgs.cPremiumTax
                        m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objEventArgs.cCommTax
                    End If

                    If iPMFunc.IsIn(gPMFunctions.ToSafeString(ColIndex), RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) Then
                        If iPMFunc.IsIn(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type), "TX", "FX") Then
                            m_lReturn = CType(SortGrid(m_oXA(lRow, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)), gPMConstants.PMEReturnCode)
                        End If
                    End If

                End If

                RollupXArray()
                grdRI.ReBind()
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
        Exit Sub
    End Sub

    Private Sub grdRI_CellUpdating(ByVal eventSender As Object, ByVal eventArgs As Artinsoft.Windows.Forms.DataGridViewCellValueCancelEventArgs) Handles grdRI.CellUpdating
        'added check to prevent recursion        
        RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
        RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated

        If Not grdRI.CurrentCell Is Nothing Then
            If Not grdRI.CurrentCell.Equals(grdRI.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        If m_bCellChanged Then
            Exit Sub
        End If
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim OldValue As Object = grdRI.CurrentRow.Cells(ColIndex).Value
        Dim Cancel As Integer = 0

        Dim NewValue As Object
        Dim cPremiumTax, cCommTax As Decimal

        Dim sMessage As String = ""
        Dim lReturn As Integer
        Const kMethodName As String = "grdRI_BeforeColUpdate"
        Dim dTempValue As Decimal = 0D

        Try

            ' Ensure we default to not changed
            m_bCellChanged = False

            ' Store new value, we'll use it lots
            NewValue = eventArgs.Value

            ' Validate the change
            Select Case ColIndex
                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured
                    ' Validate range
                    If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                        ' Simple numeric validation
                        If NewValue >= 0 Then
                            ' Set changed state

                            m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                        Else
                            sMessage = "Sum Insured must be a positive currency value"
                        End If
                    Else
                        sMessage = "Sum Insured must be a valid currency value"
                    End If
                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare
                    ' Validate range
                    If Convert.ToString(NewValue).IndexOf("%"c) >= 0 Then
                        'NewValue = gPMFunctions.ToSafeDouble(gPMFunctions.ToSafeString(NewValue).Substring(0, Marshal.SizeOf(NewValue) - 1))
                        NewValue = Convert.ToString(NewValue).Replace("%", "").Trim
                    End If

                    If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                        ' Simple numeric validation
                        If Conversion.Val(gPMFunctions.ToSafeString(NewValue)) >= 0.01 And Conversion.Val(gPMFunctions.ToSafeString(NewValue)) <= 100 Then
                            ' Set changed state

                            m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                            NewValue = Conversion.Val(gPMFunctions.ToSafeString(NewValue)) / 100

                        Else
                            sMessage = "Invalid Ceding Rate Entered"
                        End If
                    Else
                        sMessage = "Invalid Ceding Rate Entered"
                    End If
                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare
                    If Convert.ToString(NewValue).IndexOf("%"c) >= 0 Then
                        'NewValue = gPMFunctions.ToSafeDouble(gPMFunctions.ToSafeString(NewValue).Substring(0, Marshal.SizeOf(NewValue) - 1).TrimEnd())
                        NewValue = Convert.ToString(NewValue).Replace("%", "").Trim
                    End If

                    ' Validate range               
                    If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                        ' Simple numeric validation
                        If Conversion.Val(gPMFunctions.ToSafeString(NewValue)) >= 0.0001 And Conversion.Val(gPMFunctions.ToSafeString(NewValue)) <= 100 Then
                            ' Set changed state

                            m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                            NewValue = Conversion.Val(gPMFunctions.ToSafeString(NewValue)) / 100
                        Else
                            sMessage = "Invalid This Rate Entered"
                        End If
                    Else
                        sMessage = "Invalid This Rate Entered"
                    End If
                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium
                    ' Validate range
                    If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                        ' Note:
                        ' - Premium changes are only allowed for non-proportional fac
                        ' - Any change in premium should be balanced to the retained line
                        ' - We can allow negative for manual balancing within FAC lines
                        ' Set changed state

                        m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))

                        ' Check for retained
                        If m_lRetained > 0 Then
                            ' Balance to retained line (modify by adjusted value)

                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) + (gPMFunctions.ToSafeDouble(gPMFunctions.ToSafeDouble(OldValue) - NewValue))

                            ' Recalc retained percent
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) / m_oRIArrangement.Premium

                            ' Recalculate commission
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) * m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent)

                            ' Get new treaty taxes
                            Dim objArgs As RecalculateTreatyTaxEventArgs = New RecalculateTreatyTaxEventArgs(m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID), m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007TreatyID), m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium), m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission), cPremiumTax, cCommTax)
                            RaiseEvent RecalculateTreatyTax(Me, objArgs)

                            ' Store new values
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = objArgs.cPremiumTax
                            m_oXA(m_lRetained, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = objArgs.cCommTax
                        End If
                    Else
                        sMessage = "Premium must be a valid currency value"
                    End If

                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent
                    ' Simple numeric validation
                    NewValue = gPMFunctions.ToSafeDouble(gPMFunctions.ToSafeString(NewValue).Replace("%", ""))
                    Dim dbNumericTemp As Double
                    If Double.TryParse(gPMFunctions.ToSafeString(NewValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        ' Validate percentage is between 0 and 100
                        If (NewValue >= 0) And (NewValue <= 100) Then
                            ' Warn if changing treaty commission
                            If Not m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value And m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) Is DBNull.Value Then
                                If gPMFunctions.ToSafeString(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" And m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = 0 Then
                                    If MessageBox.Show("Amending the treaty commission percentage will override individual treaty party rates." & Strings.Chr(13) & Strings.Chr(10) & "Continue to amend commission percentage?", "Override Treaty Commission", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                                        Cancel = 1
                                        eventArgs.Cancel = True
                                        Exit Sub
                                    End If
                                End If
                            End If
                            ' If we use automatic grid formatting then it treats percentages as
                            ' 0..1 which is a pain to enter so scale them nicely for the user
                            NewValue = gPMFunctions.ToSafeDouble(NewValue / 100)
                            eventArgs.Value = gPMFunctions.ToSafeDouble(NewValue)
                            m_bCellChanged = True
                        Else
                            sMessage = "Commission percentage must be between 0% and 100%"
                        End If
                    Else
                        sMessage = "Commission percentage must be a valid numeric between 0 and 100"
                    End If

                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit ' To Confirm (Simplify)
                    ' Lower Limit can never be less then 0
                    If gPMFunctions.ToSafeDouble(eventArgs.Value) < 0 Then
                        sMessage = "Invalid Lower Limit Entered"
                        Cancel = 1
                        m_bCellChanged = False
                    Else

                        ' Loop through the arrangement to check for clashes in
                        ' Lower and Upper Limit
                        For lCounter As Integer = 1 To m_oXA.Rows.Count - 1
                            If Not m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then


                                NewValue = eventArgs.Value
                                m_bCellChanged = True
                                If grdRI.CurrentRowIndex <> lCounter Then
                                    ' if    New Value falls in the intervel of other Placements then displays
                                    '       an Error message as it is not allowed
                                    ' Else
                                    ' Lower Limit entered > Lower Limit of other placements and
                                    ' Upper Limit entered < Upper Limit of other placements then
                                    ' DISPLAy and error message as their is a CLASH
                                    If gPMFunctions.ToSafeDouble(NewValue) > gPMFunctions.ToSafeDouble(m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) And gPMFunctions.ToSafeDouble(NewValue) < gPMFunctions.ToSafeDouble(m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)) Then
                                        sMessage = "Treaty XOl lower limit overlaps with another Treaty XOL Layer"
                                        Cancel = 1
                                        m_bCellChanged = False
                                        Exit For
                                    ElseIf gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit).Value) < m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) And gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit).Value) > m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) And m_oXA(lCounter, RiskRIArrangement.RiskReinsuranceEnum.DBRIType) = "TX" Then
                                        sMessage = "Treaty XOl limit overlaps with another Treaty XOL Layer"
                                        Cancel = 1
                                        m_bCellChanged = False
                                        Exit For
                                    Else
                                        m_bCellChanged = True
                                    End If
                                Else
                                    If grdRI.CurrentRow().Cells(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit).Value = "" Or grdRI.CurrentRow().Cells(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit).Value = "" Then
                                        'Do nothing
                                        If gPMFunctions.ToSafeCurrency(NewValue) = NewValue Then
                                            m_bCellChanged = True
                                        Else
                                            sMessage = "Invalid Value Entered"
                                        End If
                                    Else
                                        ' Check that Upper Limit should always be greater than
                                        ' the Lower Limit for the same Placement
                                        If gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit).Value) >= gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit).Value) Then
                                            sMessage = "Treaty XOL Upper Limit must be greater than Treart XOL Lower Limit"
                                            Cancel = 1
                                            m_bCellChanged = False
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode
                    m_bCellChanged = (ToSafeString(NewValue) <> ToSafeString(OldValue))
                Case Else
                    ' Simple change check

                    m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
            End Select

            ' If we have a message then show it and set Cancel flag
            If sMessage.Length Then
                MessageBox.Show(sMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                eventArgs.Cancel = True
                eventArgs.Value = OldValue
            Else
                ' If we have not got a changed value cancel the change
                If Not m_bCellChanged Then

                    eventArgs.Value = OldValue
                Else
                    eventArgs.Value = NewValue
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            ''Debugger.Break()
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated

            If Cancel <> 0 Then
                grdRI.CancelEdit()
            End If

        End Try
        Exit Sub
    End Sub

    Private Sub grdRI_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles grdRI.GotFocus
        grdRI_Enter()
    End Sub

    Private Sub grdRI_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles grdRI.LostFocus
    End Sub

    Private Sub grdRI_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdRI.CellEnter
        Dim LastRow As DataGridViewRow = Nothing
        Dim LastCol As Integer = -1
        If Not IsNothing(grdRI.PreviousCell) Then
            If grdRI.PreviousCell.RowIndex > grdRI.Rows.Count Then
                LastRow = grdRI.Rows(grdRI.PreviousCell.RowIndex)
            End If
            LastCol = grdRI.PreviousCell.ColumnIndex
        End If

        If grdRI.CurrentRowIndex > 0 Then
            SelectedRIType = m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
            SelectedRow = grdRI.CurrentRowIndex
            If SelectedRIType <> "FX" Then
                If Not m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) Is DBNull.Value Then
                    SelRIArrangementLine = m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID)
                End If
            Else
                If Not m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007GroupingID) Is DBNull.Value Then
                    SelRIArrangementLine = m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007GroupingID)
                End If
            End If
            m_lGroupingID = gPMFunctions.ToSafeLong(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007GroupingID))
            m_cUpperLimit = gPMFunctions.ToSafeCurrency(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit))
            m_cLowerLimit = gPMFunctions.ToSafeCurrency(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit))
            m_bisRiBroker = gPMFunctions.ToSafeBoolean(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsRIBroker))
            m_lPartyCnt = gPMFunctions.ToSafeLong(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt))
            If Not m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(grdRI.CurrentRowIndex, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                RaiseEvent ResetControls(Me, New ResetControlsEventArgs("T"))
            Else
                RaiseEvent ResetControls(Me, New ResetControlsEventArgs(SelectedRIType))
            End If
        End If
    End Sub

    ' ***************************************************************** '
    '                       USERCONTROL EVENTS
    ' *************************************************************** '
    Private Sub UserControl_InitProperties()
        ' Initialise read only state
        ReadOnly_Renamed = False
    End Sub

    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        ' Load read only state
        ReadOnly_Renamed = CBool(PropBag.ReadProperty("ReadOnly", False))
    End Sub

    Private Sub uctRiskRIControlRI2007_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            grdRI.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)
        Catch
        End Try
    End Sub

    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        ' Save read only state
        PropBag.WriteProperty("ReadOnly", ReadOnly_Renamed, False)
    End Sub

    Public Function AddFacultativeXOL(ByVal lPartyCnt As Integer, ByVal sDescription As String, ByVal dRetained As Double, ByVal cLowerLimit As Decimal, ByVal cUpperLimit As Decimal, ByVal cSumInsured As Decimal, ByVal cPremium As Decimal, ByVal cPremiumTax As Decimal, ByVal dCommPercent As Double, ByVal cComm As Decimal, ByVal cCommTax As Decimal, ByVal lGroupingId As Integer, Optional ByRef sAgreementCode As String = "") As Integer

        Dim result As Integer = 0
        Dim lPriority As Integer

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPosition As Integer
        Dim bIsExistGrossNetRow As Boolean
        Dim bIsExistNetLineRow As Boolean

        Const kMethodName As String = "AddFacultativeXOL"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set defaults
            lPriority = 65536

            ' Check for duplicate and get priority
            For lCount As Integer = m_lASumBand + 1 To m_lASumFAC - 1
                ' Validate
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt) Is DBNull.Value AndAlso ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt)) = lPartyCnt And sDescription <> "Multiple Acts" Then
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                End If

                ' Check priority
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) Is DBNull.Value AndAlso ToSafeInteger(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority)) > lPriority Then
                    lPriority = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority)
                End If
            Next

            Dim lObligatoryIndex As Integer
            lObligatoryIndex = 0
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)
                    Case "T", "F"
                        If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                            lObligatoryIndex = lCount
                            Exit For
                        End If
                End Select
            Next

            If lObligatoryIndex <> 0 Then
                If Not FACPropExists Then
                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                            m_oXA.Rows.RemoveAt(lCount)
                            m_oXA.AcceptChanges()
                            m_lASumTreaty -= 1
                            m_lASumTotal -= 1
                            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                            Exit For
                        End If
                    Next
                    lReturn = AllocateObligatory()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first,PMLogError")
                    End If
                End If
            End If

            ' ***********************************************************
            ' Determine the position after looking at the current RI arrangement
            ' and get the position at which we need to Insert the New Row
            m_lReturn = CType(FindInsertPosition("FX", lPosition, cLowerLimit), gPMConstants.PMEReturnCode)

            ' Insert new row on to that position
            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition)

            ' Populate new line
            ' Grid fields
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "FAC XOL"
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = sDescription
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Retained) = dRetained / 100
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit) = cLowerLimit
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) = cUpperLimit
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured) = cSumInsured
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium) = cPremium
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax) = cPremiumTax
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent) = dCommPercent / 100
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission) = cComm
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax) = cCommTax
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode) = sAgreementCode ' Sankar - PN 50348
            'E016
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsReinsurerApproved) = "U"
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) = "MA"
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007GroupingID) = lGroupingId
            ' Supporting fields
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) = lGroupingId
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX"

            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LineID) = Nothing
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt) = lPartyCnt
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Priority) = lPriority + 1
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Lines) = 1
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PremiumPercent) = 0
            m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsCommissionModified) = 0

            ' Increment total lines...
            m_lASumTreaty += 1
            m_lASumFAC += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            grdRI.ReBind()
            ' ***********************************************************
            ' We need to insert Gross Net Row if it does'nt exists
            ' So find out whether Gross Net exists or not
            m_lReturn = CType(IsExists_GrossNetRow(bIsExistGrossNetRow), gPMConstants.PMEReturnCode)

            ' if it does'nt then Insert the New row and populate the
            ' the amounts accordingly
            If Not bIsExistGrossNetRow Then
                m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition + 1)

                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Placement) = "GROSS NET"
                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) = "Net of FAC"
                m_oXA(lPosition + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FT"

                ' Increment total lines...
                m_lASumFAC += 1
                m_lASumTotal += 1
                m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
                m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
                m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
                m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            End If
            ' ***********************************************************
            m_lReturn = RollupXArray()

            If lObligatoryIndex <> 0 Then
                If FACPropExists Or FACPropExists Then
                    If Not m_oXA(lPosition + 2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lPosition + 2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = "1" Then
                        m_oXA.Rows.RemoveAt(lPosition + 2)
                        m_oXA.AcceptChanges()
                        m_lASumTreaty -= 1
                        m_lASumTotal -= 1
                        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                        m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                        m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                    End If
                End If
            End If

            m_lReturn = CType(SortGrid("FX"), gPMConstants.PMEReturnCode)
            grdRI.ReBind()
            m_lReturn = StoreLimits()

            m_bIsDirty = True
            AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
            AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function


    Public Function GetDeletedRILines(ByRef vDeletedRILines() As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetDeletedRILines"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(DeletedRIArragementIds) Then
                For lCount As Integer = DeletedRIArragementIds.GetLowerBound(0) To DeletedRIArragementIds.GetUpperBound(0)

                    If vDeletedRILines Is Nothing Then
                        ReDim vDeletedRILines(0)

                        vDeletedRILines(0) = DeletedRIArragementIds(0)
                    Else
                        ReDim Preserve vDeletedRILines(vDeletedRILines.GetUpperBound(0) + 1)

                        vDeletedRILines(vDeletedRILines.GetUpperBound(0)) = DeletedRIArragementIds(lCount)
                    End If
                Next
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function
    ' Routine Desined to find the rows in an Array
    ' that are required to be sorted
    Private Function SortGrid(ByVal sRIType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SortGrid"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
                'developer guide no. 131
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type).ToLower() = sRIType.ToLower() AndAlso (Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ReinsuranceType) Is DBNull.Value AndAlso UCase(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ReinsuranceType)) = "XOL") Then
                    For innercount As Integer = m_oXA.GetLowerBound(0) To lCount
                        If Not m_oXA(innercount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso m_oXA(innercount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type).ToLower() = sRIType.ToLower() And gPMFunctions.ToSafeCurrency(m_oXA(innercount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) > gPMFunctions.ToSafeCurrency(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) Then
                            m_lReturn = CType(SwapRows(innercount, lCount), gPMConstants.PMEReturnCode)
                        End If
                    Next
                End If
            Next

            For lCount As Integer = m_oXA.GetLowerBound(1) To m_oXA.Rows.Count - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso LCase(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = LCase(sRIType) AndAlso
                        Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ReinsuranceType) Is DBNull.Value AndAlso Trim(UCase(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ReinsuranceType))) = "CAT" Then
                    For innercount As Integer = m_oXA.GetLowerBound(1) To lCount
                        If Not m_oXA(innercount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso LCase(m_oXA(innercount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = LCase(sRIType) AndAlso
                            ToSafeCurrency(m_oXA(innercount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) > ToSafeCurrency(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) Then
                            m_lReturn = SwapRows(innercount, lCount)
                        End If
                    Next
                End If
            Next


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' Function Designed to Swap two rows from an Array
    ' Just Pass the two indexes and it will swap these 2 rows
    Private Function SwapRows(ByVal lRowIndex1 As Integer, ByVal lRowIndex2 As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SwapRows"

        Try

            Dim vSaveRow1 As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vSaveRow1(RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Max)

            For lCount As Integer = 0 To RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Max

                vSaveRow1(lCount) = m_oXA(lRowIndex1, lCount)
                m_oXA(lRowIndex1, lCount) = m_oXA(lRowIndex2, lCount)

                m_oXA(lRowIndex2, lCount) = vSaveRow1(lCount)
            Next


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Private Function StoreLimits() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "StoreLimits"
        Dim lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Array and refresh It

            m_vStoreLimits = Nothing

            Dim lCountFXRows As Integer
            lIndex = 0

            For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Then
                    lCountFXRows += 1
                End If
            Next

            For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Then
                    If Not Information.IsArray(m_vStoreLimits) Then
                        ReDim m_vStoreLimits(lCountFXRows - 1, 2)

                        m_vStoreLimits(0, 0) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)

                        m_vStoreLimits(0, 1) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)

                        m_vStoreLimits(0, 2) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007GroupingID)

                    Else

                        m_vStoreLimits(lIndex, 0) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)

                        m_vStoreLimits(lIndex, 1) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)

                        m_vStoreLimits(lIndex, 2) = m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007GroupingID)
                    End If
                    lIndex += 1
                End If
            Next




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    Public Function ValidateRILines(ByRef bValid As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ValidateRILines"
        Dim cTotalFAXPremium As Decimal
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bValid = True
            If IsDBNull(DBNull.Value) Then

            End If

            If m_oXA.Rows.Count > 1 Then
                'check for duplicate lines
                For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1 - 1
                    For lCount2 As Integer = lCount + 1 To m_oXA.Rows.Count - 1
                        'Check here for duplicate lines on Party or Sum Insured

                        If Not (m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) Is Nothing) And Not (m_oXA(lCount2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name) Is Nothing) And gPMFunctions.ToSafeLong(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt)) = gPMFunctions.ToSafeLong(m_oXA(lCount2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007PartyCnt)) And gPMFunctions.ToSafeCurrency(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)) = gPMFunctions.ToSafeCurrency(m_oXA(lCount2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured)) And gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" And m_oXA(lCount2, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "F" Then
                            bValid = False
                            MessageBox.Show("Reinsurance Lines cannot be duplicated", "Duplicate Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit For
                        End If
                    Next
                Next
                'check for Overlap
                For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1 - 1
                    If ToSafeCurrency(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) > 0 And ToSafeCurrency(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)) > 0 And
                            ToSafeCurrency(m_oXA(lCount + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) > 0 And ToSafeCurrency(m_oXA(lCount + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)) > 0 And
                            gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" And m_oXA(lCount + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then
                        If ToSafeCurrency(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit)) > ToSafeCurrency(m_oXA(lCount + 1, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit)) Then
                            bValid = False
                            MessageBox.Show("FAC XOL limits cannot be overlapped", "Overlap Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit For
                        End If
                    End If
                Next
            End If
            For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) Is DBNull.Value Then
                    If gPMFunctions.ToSafeString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" And m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode) = "MA" Then
                        If Trim(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)) = "" Or ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare)) = 0 Then
                            MessageBox.Show("Invalid Ceding Rate Entered", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            bValid = False
                            Exit For
                        End If
                    End If
                End If
                If Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) Is DBNull.Value AndAlso Not m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) Is DBNull.Value Then
                    If (m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" And m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit) > m_oRIArrangement.SumInsured - m_cTotalSumInsuredFacProp Then
                        If Not m_bIsFacPropExists Then
                            MessageBox.Show("FAC XOL Upper Limit must be less than or equal to Gross Sum Insured", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("FAC XOL Upper Limit must be less than or equal to Gross Sum Insured less FAC Proportional Sum Insured(s)", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        bValid = False
                        Exit For
                    End If
                End If
                If m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type) = "FX" Then
                    cTotalFAXPremium = cTotalFAXPremium + m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium)

                    If cTotalFAXPremium > ToSafeCurrency(m_oRIArrangement.Premium) Then
                        MsgBox("Cumulative FAC XOL premium is more than the band premium, please change the premium to proceed further", vbInformation, "FAC XOL Check")
                        bValid = False
                        Exit For
                    End If
                End If
            Next


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    Public Sub grdRI_Enter()
        If grdRI.RowsCount = 0 Then
            Exit Sub
        End If
        If grdRI.Columns.Count > 15 Then
            For colCtr As Integer = 15 To grdRI.Columns.Count - 1
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
                    Select Case gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type))
                        Case "TT", "FT", "XT", "NL"

                            grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.Info
                            grdRI.Rows(dr.Index).DefaultCellStyle.ForeColor = SystemColors.InfoText
                            grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Italic Or FontStyle.Bold)
                            grdRI.Rows(dr.Index).ReadOnly = True

                        Case "BT", "AT", "UT"

                            grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.Info
                            grdRI.Rows(dr.Index).DefaultCellStyle.ForeColor = SystemColors.InfoText
                            grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Bold)
                            grdRI.Rows(dr.Index).ReadOnly = True

                        Case "OT", "NT"

                            grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.ButtonFace
                            grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Bold Or FontStyle.Italic)
                            grdRI.Rows(dr.Index).ReadOnly = True

                        Case "TX"

                        Case "T", "F"
                            If ToSafeInteger(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) = 1 Then

                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = Color.SkyBlue
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Bold)
                                grdRI.Rows(dr.Index).ReadOnly = True
                            End If

                    End Select

                    Select Case dc.Index
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Name
                            ' Set either caption alignment for summaries or RI icons
                            Select Case gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType))
                                Case "BT", "TT", "FT", "XT", "AT", "UT", "OT", "NT", "NL"
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Case "R", "T", "X"
                                Case "F"
                            End Select
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007LowerLimit, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007UpperLimit
                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else
                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode)) = "MA" And
                                (gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TC") Then
                                    If Not Me.ReadOnly_Renamed Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                    End If
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "AT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "BT" Or
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "XT" Or
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "UT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TT" _
                                        Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                Else
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                End If
                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" Or
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                End If
                            End If
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007SumInsured
                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else
                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" Or
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Or
                                     gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TC" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                                    If Not Me.ReadOnly_Renamed Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    End If
                                End If

                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                End If

                            End If
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Premium
                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else
                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" _
                                Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" _
                                Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TC" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                                    If (m_oRIArrangement.FACPremiumMethod = RiskRIArrangement.FACPremiumEnum.FACPremiumIsProportional) Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    End If
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "OT" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Else
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                End If
                            End If
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare

                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else
                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AddedMode)) = "MA" Then
                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" _
                                    Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    Else
                                        If Not Me.ReadOnly_Renamed Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                        End If
                                    End If
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "AT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "BT" Or
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "XT" Or
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "UT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TT" Or
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NL" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Else
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                End If
                            End If

                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare

                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else

                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" Or
                                     gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TC" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory)) <> "1" Then '(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
                                        If Not Me.ReadOnly_Renamed Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        End If
                                    Else
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    End If
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "OT" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Else
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                End If
                            End If
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent

                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else

                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" Or
                                     gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TC" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                                    If Not Me.ReadOnly_Renamed Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    End If
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "OT" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight

                                Else
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                End If
                            End If
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007AgreementCode

                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else

                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Or
                                 gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TC" Then

                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Then
                                    If Not Me.ReadOnly_Renamed Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                    End If
                                ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "NT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "OT" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Else
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                End If
                            End If
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Tax, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007Commission, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommTax

                            If Not m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsObligatory) = "1" Then
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                            Else

                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "AT" Or
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "UT" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                End If

                            End If

                            'Start E016
                        Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007IsReinsurerApproved
                            If gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FX" Or
                               gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TX" Or
                               gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "T" Or
                              gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "R" Or
                              gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "F" Or
                              gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "TFS" Then

                                grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                            End If
                            'End E016
                    End Select

                    If grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor.Name = "Info" And grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor.Name = "ButtonFace" Then
                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = SystemColors.ButtonFace
                    End If

                Catch ex As Exception

                End Try
            Next
        Next
    End Sub

    Private Sub grdRI_RowAdded(ByVal sender As Object, ByVal e As System.Data.DataTableNewRowEventArgs) Handles grdRI.RowAdded
        grdRI_Enter()
    End Sub

    Private Sub grdRI_RowDeleted(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs) Handles grdRI.RowDeleted
        grdRI_Enter()
    End Sub

    Private Sub grdRI_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdRI.CellLeave
        grdRI_Enter()
    End Sub

    Private Sub grdRI_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles grdRI.CellPainting
        If e.ColumnIndex = 0 And e.RowIndex > -1 Then
            Select Case gPMFunctions.ToSafeString(m_oXA(e.RowIndex, RiskRIArrangement.RiskReinsuranceEnum.DBRIType))
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

    Private Sub grdRI_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles grdRI.CellContentClick

    End Sub

    ''' <summary>
    ''' grdRI_EditingControlShowing Event to add handlers to editable textbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grdRI_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles grdRI.EditingControlShowing
        If grdRI.CurrentCell.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
            If TypeOf e.Control Is TextBox Then
                Dim tbCommPer As TextBox = TryCast(e.Control, TextBox)
                AddHandler tbCommPer.KeyPress, AddressOf tbCommPer_KeyPress
                AddHandler tbCommPer.KeyDown, AddressOf tbCommPer_KeyDown
                AddHandler tbCommPer.MouseDown, AddressOf tbCommPer_MouseDown
            End If
        End If
    End Sub

    ''' <summary>
    ''' tbCommPer_KeyPress Event to handle KeyPress in grid editable textbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbCommPer_KeyPress(sender As Object, e As KeyPressEventArgs)
        If grdRI.CurrentCell.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
            If Not Char.IsControl(e.KeyChar) _
                AndAlso Not Char.IsDigit(e.KeyChar) _
                AndAlso e.KeyChar <> "." Then
                e.Handled = True
            Else
                If CType(sender, TextBox).SelectionStart >= InStr(CType(sender, TextBox).Text, ".") Then
                    Dim sValue As String = gPMFunctions.ToSafeString(CType(sender, TextBox).Text).Replace("%", "").Trim
                    If InStr(sValue, ".") > 0 AndAlso Len(Mid(sValue, InStr(sValue, ".") + 1, Len(sValue))) > 3 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' tbCommPer_KeyDown Event to handle KeyDown in grid editable textbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbCommPer_KeyDown(sender As Object, e As KeyEventArgs)
        If grdRI.CurrentCell.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
            If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V Then
                If CType(sender, TextBox).SelectionLength = 0 Then
                    CType(sender, TextBox).SelectAll()
                End If
                Dim strValue As String = Clipboard.GetText(TextDataFormat.UnicodeText)
                strValue = strValue.Replace("%", "")
                If gPMFunctions.ToSafeDouble(strValue) > 0 Then
                    If InStr(strValue, ".") > 0 Then
                        Dim strBeforeDecimal As String = Mid(strValue, 1, InStr(strValue, "."))
                        Dim strAfterDecimal As String = Mid(strValue, InStr(strValue, ".") + 1)
                        If Len(strAfterDecimal) > 4 Then
                            strAfterDecimal = Mid(strAfterDecimal, 1, 4)
                        End If
                        strValue = strBeforeDecimal + strAfterDecimal
                    End If
                Else
                    strValue = ""
                End If
                Clipboard.SetText(strValue)
            End If
        End If
    End Sub

    ''' <summary>
    ''' tbCommPer_MouseDown Event to handle MouseDown in grid editable textbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbCommPer_MouseDown(sender As Object, e As MouseEventArgs)
        If grdRI.CurrentCell.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                If CType(sender, TextBox).SelectionLength = 0 Then
                    CType(sender, TextBox).SelectAll()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' WndProc windows default procedure override to capture mouse paste event 
    ''' </summary>
    ''' <param name="m"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If grdRI.CurrentCell IsNot Nothing AndAlso grdRI.CurrentCell.ColumnIndex = RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent Then
            If m.Msg = 32 Then
                Try
                    Dim strValue As String = Clipboard.GetText(TextDataFormat.UnicodeText)
                    strValue = strValue.Replace("%", "").Trim
                    If gPMFunctions.ToSafeDouble(strValue) > 0 Then
                        If InStr(strValue, ".") > 0 Then
                            Dim strBeforeDecimal As String = Mid(strValue, 1, InStr(strValue, "."))
                            Dim strAfterDecimal As String = Mid(strValue, InStr(strValue, ".") + 1)
                            If Len(strAfterDecimal) > 4 Then
                                strAfterDecimal = Mid(strAfterDecimal, 1, 4)
                            End If
                            strValue = strBeforeDecimal + strAfterDecimal
                        End If
                    Else
                        strValue = 0
                    End If
                    Clipboard.SetText(strValue)
                Catch ex As Exception
                End Try
            End If
        End If
        MyBase.WndProc(m)
    End Sub

End Class
