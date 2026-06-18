Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Globalization

<System.Runtime.InteropServices.ProgId("uctClaimRIControlRI2007_NET.uctClaimRIControlRI2007")> _
Partial Public Class uctClaimRIControlRI2007
    Inherits System.Windows.Forms.UserControl

    ' Event Declared to set the controls on the interface
    ' on the basis of Selected RI Type
    Public Event ResetControls(ByVal Sender As Object, ByVal e As ResetControlsEventArgs)

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctClaimRIControl"


    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    Private m_bIsDirty As Boolean
    Private m_bReadOnly As Boolean

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Internal object for ri arrangement information
    Private m_oRIArrangement As ClaimRIArrangement

    ' Internal array for grid population
    Private m_oXA As XArrayHelper

    ' Flag to say we have edited a cell
    Private m_bCellChanged As Boolean

    ' RI array pointers
    Private m_bRIEmpty As Boolean
    Private m_lASumBand As Integer
    Private m_lASumTreaty As Integer
    Private m_lASumFAC As Integer
    Private m_lASumXOL As Integer
    Private m_lASumTotal As Integer
    Private m_lASumAlloc As Integer
    Private m_lASumUnalloc As Integer
    Private m_lUnallocatedSumInsured As Integer 'PN 76602
    Private m_lNoofTreatyRows As Integer
    Private m_lNoofFACRows As Integer

    Private m_sSelectedRIType As String = ""
    Private m_SelectedRow As Integer
    ' Variable Declared to keep track of No of Fac Prop and Fac Xol Rows
    Dim m_iRowCountFAC As Integer

    ' Variable Declared to keep track of No of Treaty Prop and Treaty Xol Rows
    Dim m_iRowCountTreaty As Integer
    Private m_lASumOriginal As Integer
    Private m_vDeletedRIArragementIds() As Object
    Private m_lSelRIArrangementLine As Integer
    Private m_lASumNet As Integer
    Private m_lunAllocatedReserve As Integer
    Private m_lUnallocatedPayment As Integer

    Private m_lGroupingID As Integer
    Private m_cUpperLimit As Decimal
    Private m_cLowerLimit As Decimal
    Private m_vStoreLimits As Object
    Private m_bisRiBroker As Boolean
    Private m_lPartyCnt As Integer
    Private m_iRowCountRetained As Integer
    Private m_sTransactionType As String = ""
    Private m_bTreatyAllocationforDeletion As Boolean

    Private m_bRecovery As Boolean

    Dim m_bIsFacPropExists As Boolean
    Dim m_bNewFACPropORFACXOL As Boolean
    Private m_bIsFacxolExists As Boolean
    Dim bIsFacPropExists As Boolean
    Private m_bCanEditClaimRI As Boolean
    Private m_lASumObligatoryTreaty As Integer
    Private m_oBusiness As Object     'E016
    Dim fsColumn As ArrayList
    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '

    <Browsable(False)> _
    Public WriteOnly Property Recovery() As Boolean
        Set(ByVal Value As Boolean)
            m_bRecovery = Value
        End Set
    End Property
    Public ReadOnly Property UnallocatedSumInsured() As Long ' PN 76602
        Get
            Return m_lUnallocatedSumInsured
        End Get
    End Property
    <Browsable(True)> _
    Public Property ExistingLimits() As Object
        Get
            Return m_vStoreLimits
        End Get
        Set(ByVal Value As Object)


            m_vStoreLimits = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    'Gaurav
    <Browsable(True)> _
    Public Property DeletedRIArragementIds() As Object
        Get
            Return VB6.CopyArray(m_vDeletedRIArragementIds)
        End Get
        Set(ByVal Value As Object)
            m_vDeletedRIArragementIds = Value
        End Set
    End Property

    'Gaurav

    <Browsable(True)> _
    Public Property SelRIArrangementLine() As Integer
        Get
            Return m_lSelRIArrangementLine
        End Get
        Set(ByVal Value As Integer)
            m_lSelRIArrangementLine = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property SelectedRIType() As String
        Get
            Return m_sSelectedRIType
        End Get
        Set(ByVal Value As String)
            m_sSelectedRIType = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property SelectedRow() As Integer
        Get
            Return m_SelectedRow
        End Get
        Set(ByVal Value As Integer)
            m_SelectedRow = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return grdRI.Enabled
        End Get
        Set(ByVal Value As Boolean)
            grdRI.Enabled = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property IsDirty() As Boolean
        Get
            Return m_bIsDirty
        End Get
        Set(ByVal Value As Boolean)
            m_bIsDirty = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property ReadOnly_Renamed() As Boolean
        Get
            Return m_bReadOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
            fsColumn = New ArrayList
            For i As Integer = 0 To grdRI.ColumnsCount - 1
                fsColumn.Add(False)
            Next
            ' If we can't edit turn of per cell formatting
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = True
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode) = Not m_bReadOnly
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = Not m_bReadOnly
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) = True
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) = True
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = Not m_bReadOnly
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare) = True
            fsColumn(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = True

            ' Set base styles
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode).ReadOnly = True

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement).ReadOnly = True

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name).ReadOnly = True
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained).ReadOnly = True
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve).ReadOnly = True
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).ReadOnly = True
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight

        End Set
    End Property

    <Browsable(True)> _
    Public Property RowCountFAC() As Integer
        Get
            Return m_iRowCountFAC
        End Get
        Set(ByVal Value As Integer)
            m_iRowCountFAC = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property RowCountTreaty() As Integer
        Get
            Return m_iRowCountTreaty
        End Get
        Set(ByVal Value As Integer)
            m_iRowCountTreaty = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property GroupingID() As Integer
        Get
            Return m_lGroupingID
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property UpperLimit() As Decimal
        Get
            Return m_cUpperLimit
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property LowerLimit() As Decimal
        Get
            Return m_cLowerLimit
        End Get
    End Property

    <Browsable(True)> _
    Public Property RowCountRetained() As Integer
        Get
            Return m_iRowCountRetained
        End Get
        Set(ByVal Value As Integer)
            m_iRowCountRetained = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property ThisReserve() As Decimal
        Get
            Return m_oRIArrangement.ThisReserve
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property ThisPayment() As Decimal
        Get
            Return m_oRIArrangement.ThisPayment
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property UnallocatedReserve() As Integer
        Get
            Return m_lunAllocatedReserve
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property UnallocatedPayment() As Integer
        Get
            Return m_lUnallocatedPayment
        End Get
    End Property

    <Browsable(True)> _
      Public Property ShowPayments() As Boolean
        Get
            'ShowPayments = Not grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).Locked
        End Get
        Set(ByVal Value As Boolean)
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).ReadOnly = Not Value
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).Visible = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property IsRIBroker() As Boolean
        Get
            Return m_bisRiBroker
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
    End Property

    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    'Start Arul -Bug Fixing(PN 66155)
    Public Function IsExists_NetLineRow(ByRef bRowExists As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "IsExists_NetLineRow"
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Loop through an array to find where type equals to 'NL'
            For lCount As Integer = 1 To m_oXA.Rows.Count - 1 - 1
                If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
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
    'End Arul -Bug Fixing(PN 66155)
    'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
    Public Function AllocateObligatory() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AllocateObligatory"
        Dim lReturn, lPosition, lCount, lPriority As Integer
        '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN66155)
        Dim bIsExistNetLineRow As Boolean

        Try

        result = gPMConstants.PMEReturnCode.PMTrue
        lPosition = 1
        ' Insert new row on to that position

        m_oXA.Rows.InsertAt(m_oXA.NewRow, 1)
       
        For lCount = 0 To m_oXA.Rows.Count - 1
            Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory))
                Case "1"
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)
                    m_oXA(lPosition, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oRIArrangement.SumInsured) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = gPMFunctions.ToSafeDouble(m_oRIArrangement.ThisReserve) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = gPMFunctions.ToSafeDouble(m_oRIArrangement.ThisPayment) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = gPMFunctions.ToSafeDouble(m_oRIArrangement.PaymentToDate) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = gPMFunctions.ToSafeDouble(m_oRIArrangement.ReserveToDate) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = gPMFunctions.ToSafeDouble(m_oRIArrangement.RecoveredToDate) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = gPMFunctions.ToSafeDouble(m_oRIArrangement.Balance) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = gPMFunctions.ToSafeDouble(m_oRIArrangement.Incurred) * gPMFunctions.ToSafeDouble(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare))
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsRIBroker) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsRIBroker)
                    ' Supporting fields
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "F"
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority) = lPriority + 1
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)
                    m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)
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

        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
            m_oXA.Rows.RemoveAt(lCount)
            m_oXA.AcceptChanges()
        End If

        m_lReturn = CType(IsExists_NetLineRow(bIsExistNetLineRow), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("IsExists_NetLineRow", "Failed to locate  Net Line row", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' if it does'nt then Insert the New row and populate the
        ' the amounts accordingly
        If Not bIsExistNetLineRow Then

            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition + 1)
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "Net Line"
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "NL"

            ' Increment total lines...
            m_lASumFAC += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

            m_lReturn = RollupXArray()

        End If

        ' Rebind data
        grdRI.ReBind()

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

       
        End Try
        Return result	
    End Function

    Public Function AddFacultative(ByVal lPartyCnt As Integer, ByVal sDescription As String, ByVal bIsRIBroker As Boolean) As Integer

        Dim result As Integer = 0
        Dim lPriority, lPosition As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bIsExistGrossNetRow As Boolean
        Const kMethodName As String = "AddFacultative"
        Dim bIsExistNetLineRow As Boolean
        'E016
        Dim sReinsuranceApproved As String

        Try

        result = gPMConstants.PMEReturnCode.PMTrue
        ' Set defaults
        lPriority = 65536

        ' Check for duplicate and get priority
        'For lCount = m_lASumTreaty + 1 To m_lASumFAC - 1
        For lCount As Integer = m_lASumBand + 1 To m_lASumFAC - 1
            ' Validate
            If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt)) = lPartyCnt Then
                result = gPMConstants.PMEReturnCode.PMRecordInUse
        	Return result
            End If

            ' Check priority
            If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority)) > lPriority Then
                lPriority = gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority), 0)
            End If
        Next

        Dim lObligatoryIndex As Integer
        lObligatoryIndex = 0
        For lCount As Integer = 0 To m_oXA.Rows.Count - 1
            Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                Case "T", "F"
                    If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                        lObligatoryIndex = lCount
                        Exit For
                    End If
            End Select
        Next
        If lObligatoryIndex <> 0 Then
            If Not m_bIsFacxolExists And Not m_bIsFacPropExists Then
                For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                    If gPMFunctions.ToSafeString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                        m_oXA.Rows.RemoveAt(lCount)
                        m_oXA.AcceptChanges()
                        Exit For
                    End If
                Next
                lReturn = AllocateObligatory()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first,PMLogError")
                Else
                    m_bNewFACPropORFACXOL = True
                End If
            End If
        End If

        ' ***********************************************************
        ' Determine the position after looking at the current RI arrangement
        ' and get the position at which we need to Insert the New Row
        m_lReturn = CType(IsExists_NetLineRow(bIsExistNetLineRow), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("IsExists_NetLineRow", "Failed to locate  Net Line row", gPMConstants.PMELogLevel.PMLogError)
        End If
        If lObligatoryIndex <> 0 And bIsExistNetLineRow Then
            m_lReturn = CType(FindInsertPosition("NL", lPosition, bIsObligatory:=True), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(FindInsertPosition("F", lPosition), gPMConstants.PMEReturnCode)
        End If
        ' Insert new row
        m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition)

        'Start E016
        If m_oBusiness IsNot Nothing Then
            m_lReturn = g_oObjectManager.GetInstance( _
                    oObject:=m_oBusiness, _
                    sClassName:="bSIRReinsuranceRI2007.Form", _
                    vInstanceManager:=PMGetViaClientManager)

            m_lReturn = m_oBusiness.GetInsurerApprovedStatus(r_sInsurerApprovedStatus:=sReinsuranceApproved, _
                    v_lPartyCnt:=lPartyCnt)

        End If
        'End E016
        ' Populate new line
        ' Grid fields
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "FAC Prop"
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = sDescription
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode) = ""
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsRIBroker) = bIsRIBroker
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode) = "MA"
        ' Supporting fields
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "F"

        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID) = Nothing
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt) = lPartyCnt

        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007XolID) = Nothing
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority) = lPriority + 1
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines) = 1
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit) = 0
        'E016
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsReinsurerApproved) = sReinsuranceApproved
        ' Increment total lines...
        m_lASumTreaty += 1
        m_lASumFAC += 1
        m_lASumTotal += 1
        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)

        'Increase the count of FAC row
        RowCountFAC += 1

        ' ***********************************************************
        ' We need to insert Gross Net Row if it does'nt exists
        ' So find out whether Gross Net exists or not
        m_lReturn = CType(IsExists_GrossNetRow(bIsExistGrossNetRow), gPMConstants.PMEReturnCode)

        ' if it does'nt then Insert the New row and populate the
        ' the amounts accordingly
        If Not bIsExistGrossNetRow Then
            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition + 1)

            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "GROSS NET"
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Net of FAC"
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "FT"

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
            If m_bIsFacxolExists Or m_bIsFacPropExists Then
                If Convert.ToString(m_oXA(lPosition + 2, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                    m_oXA.Rows.RemoveAt(lPosition + 2)
                    m_oXA.AcceptChanges()
                End If
            End If
        End If
        ' Rebind data
        grdRI.ReBind()

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

      
        End Try
        Return result	
    End Function

    Public Function AddTreaty(ByVal lTreatyID As Integer, ByVal sDescription As String, ByVal sAgreementCode As String, ByVal bIsRetained As Boolean, Optional ByRef sTransactionType As String = "T") As Integer

        Dim result As Integer = 0
        Dim lPriority As Integer
        Dim cPremiumTax, cCommTax As Decimal
        Dim lPosition As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddTreaty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set defaults
            lPriority = 0

            ' Check for duplicate and get priority
            For lCount As Integer = m_lASumFAC + 1 To m_lASumTreaty - 1
                ' Validate
                If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID)) = lTreatyID Then
                    result = gPMConstants.PMEReturnCode.PMRecordInUse
                    Return result
                End If

                ' Check priority
                If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority)) > lPriority Then
                    lPriority = gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority), 0)
                End If
            Next

            Dim lObligatoryIndex As Integer
            lObligatoryIndex = 0
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                    Case "T", "F"
                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                            lObligatoryIndex = lCount
                            Exit For
                        End If
                End Select
            Next

            If lObligatoryIndex <> 0 Then
                If Not m_bIsFacPropExists And Not m_bIsFacxolExists Then

                    For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                        If gPMFunctions.ToSafeString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                            m_oXA.Rows.RemoveAt(lCount)
                            m_oXA.AcceptChanges()
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

                    lReturn = AllocateObligatory()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first,PMLogError")
                    Else
                        m_bNewFACPropORFACXOL = True
                    End If
                End If
            End If

            ' ***********************************************************
            ' Determine the position after looking at the current RI arrangement
            ' and get the position at which we need to Insert the New Row
            If Not m_bIsFacPropExists And Not m_bIsFacxolExists And lObligatoryIndex <> 0 Then
                m_lReturn = CType(FindInsertPosition("NL", lPosition, bIsObligatory:=(AllocateObligatory() > 0)), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(FindInsertPosition(sTransactionType, lPosition), gPMConstants.PMEReturnCode)
            End If

            ' Insert new row
            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition)

            ' Populate new line
            ' Grid fields
            If sTransactionType = "T" Then
                m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "Treaty QSH"
            Else
                m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "Treaty XOL"
            End If
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = sDescription
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode) = sAgreementCode
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode) = "MA"
            ' Supporting fields
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = IIf(bIsRetained, "R", sTransactionType)
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID) = lTreatyID

            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID) = Nothing

            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007XolID) = Nothing
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority) = lPriority + 1
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines) = 1
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit) = 0
            m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = 0

            ' Increment total lines...
            m_lASumTreaty += 1
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)

            If lObligatoryIndex <> 0 Then
                If Not m_bIsFacxolExists Or Not m_bIsFacPropExists Then
                    If Convert.ToString(m_oXA(lPosition + 2, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                        m_oXA.Rows.RemoveAt(lPosition + 2)
                        m_oXA.AcceptChanges()
                        m_lASumFAC -= 1
                        m_lASumTreaty -= 1
                        m_lASumTotal -= 1
                        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc - 1)
                        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc - 1)
                        m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal - 1)
                        m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet - 1)
                    End If
                End If
            End If

            ' Rebind data
            grdRI.ReBind()

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
    Public Function DeleteRow(Optional ByVal iAction As Integer = 0, Optional ByVal v_bOnEditFX As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteRow"

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lReturn As Integer
        Dim bRowExists As Boolean
        Try
            If SelectedRIType = "F" Or SelectedRIType = "FX" Or SelectedRIType = "T" Or SelectedRIType = "TX" Or SelectedRIType = "R" Then

                m_bIsDirty = True
                m_bTreatyAllocationforDeletion = True
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

                ' Store an RiArrangementLine Id's into an array

                If SelectedRIType = "FX" And iAction = 3 Then
                    'do nothing
                Else
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
                'Restore the Limits Again

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
                            If Convert.ToString(m_oXA(lCount, RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007type)) = "FT" Then
                                Try
                                    lReturn = gPMConstants.PMEReturnCode.PMTrue
                                    RemoveHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                                    m_oXA.Rows.RemoveAt(lCount)
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
                If v_bOnEditFX = False Then
                    m_lReturn = RollupXArray()
                End If
                m_lReturn = CType(SortGrid("FX"), gPMConstants.PMEReturnCode)
                m_lReturn = StoreLimits()

                ' Rebind data
                grdRI.ReBind()

            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

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
                If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FT" Then
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
        m_lASumXOL = 0
        m_lASumTotal = 0
        m_lASumAlloc = 0
        m_lASumUnalloc = 0

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

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

     
        End Try
        Return result	
    End Function

    Public Sub FinaliseEdit()

        Try

            ' Certain function won't trigger an edit to commit. Force this by changing column
            Dim lLastCol As Integer = grdRI.CurrentCell.ColumnIndex
            If Not IsNothing(grdRI.CurrentRow) Then
                grdRI.CurrentCell = grdRI.CurrentRow.Cells((grdRI.CurrentCell.ColumnIndex + 1) Mod grdRI.Columns.Count)
            End If
            If Not IsNothing(grdRI.CurrentRow) Then
                grdRI.CurrentCell = grdRI.CurrentRow.Cells(lLastCol)
            End If

        Catch exc As System.Exception

        End Try

    End Sub

    Public Function GetProperties(ByRef vRILines(,) As Object) As Integer

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


    Public Function SetProperties(ByVal oRIArrangement As cSIRRIControls.ClaimRIArrangement) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_bRecovery Then
            grdRI.Columns(11).HeaderText = "This Recovery"
        Else
            If TransactionType = "C_CR" Then
                grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).Visible = False
            End If
            grdRI.Columns(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
        lReturn = Clear()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Clear", "Unable to clear previous reinsurance details")
        End If

        ' Set appropriate read only state based on what we have to allocate
        ' Store the arrangement objects
        m_oRIArrangement = oRIArrangement

        If m_bRecovery Then
            m_oRIArrangement.bRecovery = True
        End If

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
            m_oXA.RedimXArray(New Integer() {0, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max + 1}, New Integer() {0, 0})
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
    Private Function CollapseXArray(ByRef vRI(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lDest As Integer

        Dim lReturn As Integer
        Const kMethodName As String = "CollapseXArray"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' We need to walk the XArray and remove any genuine RI rows
        For lSource As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
            ' Check row type
            If iPMFunc.IsIn(Convert.ToString(m_oXA(lSource, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)), "R", "T", "F", "TX", "FX", "TFS", "TC") Then
                ' Prep main array
                If lDest = 0 Then
                    ReDim vRI(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max + 1, lDest)
                Else
                    ReDim Preserve vRI(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max + 1, lDest)
                End If

                ' Copy all row data
                'For lCol = 0 To (ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max + 1)
                For lCol As Integer = 0 To (ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max)

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
        Dim bWriteSummary As Boolean
        Dim lCount, lReturn As Integer
        Const kMethodName As String = "ExpandXArray"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' When we get here we should have an array containing just
        ' the raw treaty and fac lines. We need to expand this to
        ' include all of the summary lines.

        ' ***********************************************************
        ' Insert band total at the top
        ' Note: If ri was empty we already have an empty band row!
        m_lReturn = CType(SortGrid("TC"), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SortGrid("TX"), gPMConstants.PMEReturnCode)

        If Not m_bRIEmpty Then
            m_oXA.Rows.InsertAt(m_oXA.NewRow, 0)
            m_oXA.AppendRows()
        End If

        ' Populate row
        m_lASumBand = 0
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "GROSS"
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Band Total"
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = m_oRIArrangement.SumInsured
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = m_oRIArrangement.ReserveToDate
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = m_oRIArrangement.ThisReserve
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = m_oRIArrangement.PaymentToDate
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = m_oRIArrangement.ThisPayment
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = m_oRIArrangement.RecoveredToDate
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = m_oRIArrangement.Balance
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = m_oRIArrangement.Incurred
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IncurredToDate) = m_oRIArrangement.IncurredToDate
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "BT"

        ' ***********************************************************
        ' We must now walk the treaty rows add add the treaty total
        m_lASumTreaty = 0
        m_lNoofFACRows = 0
        m_lNoofTreatyRows = 0

        For lCount = 1 To m_oXA.Rows.Count - 1
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then
                m_lASumFAC = lCount + 1
                m_lNoofFACRows += 1

                If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then
                    If Not m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID) Is DBNull.Value AndAlso _
                    gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID), 0) = 0 Then
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Multiple Acts"
                    End If
                End If
            ElseIf Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Then  'R line's
                m_lNoofTreatyRows += 1
                If gPMFunctions.ToSafeString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                    m_lASumObligatoryTreaty = lCount
                End If
            End If
        Next

        If m_lASumObligatoryTreaty > 0 Then

            m_oXA.Rows.InsertAt(m_oXA.NewRow, gPMConstants.kRINetLineObligatory)
            m_oXA.AppendRows()
            m_oXA(gPMConstants.kRINetLineObligatory, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "Net Line"
            m_oXA(gPMConstants.kRINetLineObligatory, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "NL"
            If m_lASumFAC <> 0 Then
                m_lASumFAC += 1
            End If
            m_lASumTotal += 1
            m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
            m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
            m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
            m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

        End If

        ' If we reached the end of the array add total at end
        If m_lASumFAC > 0 Then

            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumFAC)
            m_oXA.AppendRows()

            m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "GROSS NET"
            m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Net of FAC"

            m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "FT" 'previously treaty total now Fac total
        End If

        m_lASumTreaty = m_oXA.Rows.Count - 1 + 1

        ' Add allocated total
        m_oXA.AppendRows()

        ' Set allocated line
        m_lASumAlloc = m_oXA.Rows.Count - 1
        m_oXA(m_lASumAlloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Allocated"
        m_oXA(m_lASumAlloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "AT"

        ' ***********************************************************
        ' We must now walk the xol rows add add the totals..possibly lots of them TODO
        lCount = m_lASumFAC + 1


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

      
        End Try
        Return result
    End Function

    Private Function RollupXArray() As Integer

        Dim result As Integer = 0
        Dim oTtyTotal, oFacTotal, oFACPropTotal, oGrossTotal, oGrossNet, oAllocated, oUnAllocated, oRetained, oSurTotal As ClaimTotalizerRI2007

        Dim iPositionRetainedLine As Integer
        ' Keeps the line limit of the retained line
        Dim cRetainedLineLimit As Decimal
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bNeedsTreatyAllocation As Boolean
        Dim cTotalSumInsuredFacProp, cTotalReserveFacProp As Decimal

        Dim lTQSHReserveTotal, lTQSHPaymentTotal, lTQSHSumInsuredTotal As Double
        Dim cCurrentRetained As Decimal
        Dim dParticipationPercent As Double
        Dim iCedePremiumMark As Integer  'E005 Part1
        Dim lTreatyRecoveredToDate As Integer

        Const kMethodName As String = "RollupXArray"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create totalizers
            oTtyTotal = New ClaimTotalizerRI2007()
            oFacTotal = New ClaimTotalizerRI2007() ' For Both FAC Prop and FAC XOL
            oFACPropTotal = New ClaimTotalizerRI2007()
            oGrossTotal = New ClaimTotalizerRI2007()
            oGrossNet = New ClaimTotalizerRI2007()
            oAllocated = New ClaimTotalizerRI2007()
            oUnAllocated = New ClaimTotalizerRI2007()
            oRetained = New ClaimTotalizerRI2007()
            oSurTotal = New ClaimTotalizerRI2007()

            RowCountFAC = 0
            RowCountTreaty = 0
            RowCountRetained = 0
            cTotalSumInsuredFacProp = 0
            cTotalReserveFacProp = 0

            ' Put the Totals in oGrossTotal object
            oGrossTotal.SumInsured = m_oRIArrangement.SumInsured
            oGrossTotal.ReserveToDate = m_oRIArrangement.ReserveToDate
            oGrossTotal.ThisPayment = m_oRIArrangement.ThisPayment
            oGrossTotal.RecoveredToDate = m_oRIArrangement.RecoveredToDate
            oGrossTotal.ThisReserve = m_oRIArrangement.ThisReserve
            oGrossTotal.Balance = m_oRIArrangement.Balance
            oGrossTotal.Incurred = m_oRIArrangement.Incurred
            oGrossTotal.PaymentToDate = m_oRIArrangement.PaymentToDate
            oGrossTotal.IncurredToDate = m_oRIArrangement.IncurredToDate
            'To Verify that a Line is Manually added during Claim
            bNeedsTreatyAllocation = False
            'm_bReadOnly = True
            If Not m_bReadOnly Then
                For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                    If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode)) = "MA" Then
                        bNeedsTreatyAllocation = True
                    End If
                Next
            End If

            'Calculations need to occur if the this% field can be edited
            If TransactionType = "C_CO" Then
                bNeedsTreatyAllocation = True
            End If

            If m_bTreatyAllocationforDeletion Then bNeedsTreatyAllocation = True
            m_bTreatyAllocationforDeletion = False
            ' ***********************************************************
            ' Walk through the array and determine the Facultive and Treaty Rows
            ' and put their totals in corresponding objects. We will be using it
            ' to calculate Gross Net, Allocated and Unallocated amounts
            bIsFacPropExists = False
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                    Case "F"
                        '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN 66155)
                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) <> "1" Then
                            m_bIsFacPropExists = True
                        End If

                    Case "FX"
                        m_bIsFacxolExists = True

                End Select
            Next
            Dim lObligatoryIndex As Integer
            If Not m_bNewFACPropORFACXOL Then
                For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                    Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                        Case "T"
                            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                                lObligatoryIndex = lCount
                                Exit For
                            End If
                    End Select
                Next

                If lObligatoryIndex <> 0 Then
                    If m_bIsFacxolExists Or m_bIsFacPropExists Then
                        lReturn = CType(AllocateObligatory(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        m_lReturn = CType(FindInsertPosition("NL", 2), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to Find the Position to insert Netline Obligatory ", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        m_oXA.Rows.InsertAt(m_oXA.NewRow, gPMConstants.kRINetLineObligatory)
                        m_oXA.AppendRows()

                        m_oXA(gPMConstants.kRINetLineObligatory, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "Net Line"
                        m_oXA(gPMConstants.kRINetLineObligatory, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "NL"

                        For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                            If gPMFunctions.ToSafeString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                                If lCount <> 2 Then
                                    m_oXA.Rows.RemoveAt(lCount)
                                    m_oXA.AcceptChanges()
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                    Case "F"
                        RowCountFAC += 1
                        oFACPropTotal.Add(m_oXA, lCount, m_bRecovery)
                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) <> "1" Then
                            m_bIsFacPropExists = True
                        End If
                      
                    Case "FX"
                        RowCountFAC += 1
                        If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained), 0) > 0 Then
                            bNeedsTreatyAllocation = True
                        End If
                        m_bIsFacxolExists = True
                    Case "T", "TX"
                        RowCountTreaty += 1
                    Case "R"
                        If RowCountRetained = 0 Then
                            iPositionRetainedLine = lCount
                            cRetainedLineLimit = gPMFunctions.ToSafeCurrency(m_oXA(iPositionRetainedLine, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0)
                        End If
                        RowCountRetained += 1
                        oRetained.Add(m_oXA, lCount, m_bRecovery)
                End Select
            Next

            Dim dGrossFACXOLTotal As Double

            'If a Line is Added Manually then
            If Not m_bReadOnly And bNeedsTreatyAllocation Then

                dGrossFACXOLTotal = 0
                For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                    Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                        Case "F"
                            'Do not Need to Re-Allocate this Line

                        Case "FX"
                            If TransactionType = "C_CO" Then
                                'Non Retained Participation Percentage
                                dParticipationPercent = 1 - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained), 0)
                                If dParticipationPercent = 0 Then dParticipationPercent = 1

                                If (oGrossTotal.ThisReserve - oFACPropTotal.ThisReserve) >= m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * dParticipationPercent
                                ElseIf (oGrossTotal.ThisReserve - oFACPropTotal.ThisReserve) < m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) And (oGrossTotal.ThisReserve - oFACPropTotal.ThisReserve) > m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = (gPMFunctions.ToSafeDouble(oGrossTotal.ThisReserve - oFACPropTotal.ThisReserve, 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * dParticipationPercent
                                ElseIf (oGrossTotal.ThisReserve - oFACPropTotal.ThisReserve) <= m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = 0
                                End If

                                If dParticipationPercent > 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate)) > 0 Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) * dParticipationPercent
                                End If

                                'This Payment Calculations (For Open Claim No Trans)
                                If (oGrossTotal.ThisPayment - oFACPropTotal.ThisPayment) >= m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * dParticipationPercent
                                ElseIf (oGrossTotal.ThisPayment - oFACPropTotal.ThisPayment) < m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) And (oGrossTotal.ThisPayment - oFACPropTotal.ThisPayment) > m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = (gPMFunctions.ToSafeDouble(oGrossTotal.ThisPayment - oFACPropTotal.ThisPayment, 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * dParticipationPercent
                                ElseIf (oGrossTotal.ThisPayment - oFACPropTotal.ThisPayment) <= m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = 0
                                End If

                                'Incured Calculations
                                If oGrossTotal.Incurred - oFACPropTotal.Incurred <= gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = 0
                                ElseIf oGrossTotal.Incurred - oFACPropTotal.Incurred > gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * (1 - m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained))
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = ((oGrossTotal.Incurred - oFACPropTotal.Incurred) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * (1 - m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained))
                                End If
                                'This FACXOLTotal can be used for calculating Treaty Quota share and Treaty XOL
                                dGrossFACXOLTotal += gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0)

                                If m_bRecovery Then
                                    'm_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0) - (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0) + gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0) + gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0))
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0) '- gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0)
                                End If

                                bNeedsTreatyAllocation = True
                            End If
                    End Select
                Next
            End If

            'Calculate the FAC Prop/FX Totals
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                    oFacTotal.Add(m_oXA, lCount, m_bRecovery)
                End If
            Next

            'Calculate the Gross Net Totals
            oGrossNet.SumInsured = m_oRIArrangement.SumInsured - oFacTotal.SumInsured
            oGrossNet.ReserveToDate = m_oRIArrangement.ReserveToDate - oFacTotal.ReserveToDate
            oGrossNet.ThisReserve = m_oRIArrangement.ThisReserve - oFacTotal.ThisReserve
            oGrossNet.RecoveredToDate = m_oRIArrangement.RecoveredToDate - oFacTotal.RecoveredToDate
            oGrossNet.Balance = m_oRIArrangement.Balance - oFacTotal.Balance
            oGrossNet.Incurred = m_oRIArrangement.Incurred - oFacTotal.Incurred
            oGrossNet.PaymentToDate = m_oRIArrangement.PaymentToDate - oFacTotal.PaymentToDate
            oGrossNet.ThisPayment = m_oRIArrangement.ThisPayment - oFacTotal.ThisPayment
            oGrossNet.IncurredToDate = m_oRIArrangement.IncurredToDate - oFacTotal.IncurredToDate

          

             For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)
                    Case "TFS"
                        If Not m_bReadOnly Then
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * oGrossNet.ThisReserve
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * oGrossNet.ThisPayment

                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = oGrossNet.SumInsured * ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            If ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)) > (ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) * m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines))) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = (ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) * m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines))
                            End If

                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * (oGrossNet.Incurred)
                            If m_bRecovery Then
                                'This_payment column denotes recovery for Recovery Process.And Balance does not include recovery
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0)
                            Else
                                'm_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0) '- ToSafeDouble(m_oXA(lCount, DBCRI2007RecoveredToDate), 0)
                            End If
                        End If
                        If m_bRecovery Then
                        Else
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = Math.Round(gPMFunctions.ToSafeCurrency(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate)), 4) + Math.Round(gPMFunctions.ToSafeCurrency(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve)), 4) - Math.Round(gPMFunctions.ToSafeCurrency(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)), 4) - Math.Round(gPMFunctions.ToSafeCurrency(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate)), 4)
                        End If
                        oSurTotal.Add(m_oXA, lCount, m_bRecovery)
                End Select
            Next

            If RowCountFAC > 0 Then
                oGrossNet.Store(m_oXA, m_lASumFAC)
                m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = ""
            End If

            'Recalculate Treaty This Reserve ------- PN 56635
            If Not m_bReadOnly And TransactionType <> "C_CP" Then
                For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                    Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                        Case "T"
                            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) <> "1" Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = gPMFunctions.ToSafeDouble(oGrossNet.ThisReserve) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            Else
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = gPMFunctions.ToSafeDouble(oGrossNet.ThisReserve - oSurTotal.ThisReserve) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If
                            If TransactionType = "C_SA" Or TransactionType = "C_RV" Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = gPMFunctions.ToSafeDouble(oGrossNet.ThisPayment - oSurTotal.ThisPayment) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If
                    End Select
                Next
            End If

            Dim bThisPercentageIsChanged As Boolean
            bThisPercentageIsChanged = False

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                    Case "T"

                        If TransactionType = "C_CO" And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) <= 0 Then
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)
                        ElseIf (TransactionType = "C_CR" Or TransactionType = "C_SA" Or TransactionType = "C_RV" Or TransactionType = "C_CP") And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) <= 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) <> gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare), 0) Then
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)
                        End If

                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * (oGrossNet.ThisReserve)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * (oGrossNet.ThisPayment)

                        If TransactionType = "C_CO" And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare), 0) > 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare), 0) Then
                            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = oGrossTotal.SumInsured * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare), 0)
                            Else
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = oGrossNet.SumInsured * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare), 0)
                            End If
                        Else
                            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = oGrossTotal.SumInsured * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            Else
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = oGrossNet.SumInsured * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If
                        End If

                        If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured), 0) > gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit)) And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit)) > 0 Then
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit))
                        End If

                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                            If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured), 0) > (oGrossTotal.SumInsured - oTtyTotal.SumInsured) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = (oGrossTotal.SumInsured - oTtyTotal.SumInsured) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If
                        Else
                            If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured), 0) > (oGrossNet.SumInsured - oTtyTotal.SumInsured) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = (oGrossNet.SumInsured - oSurTotal.SumInsured) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If

                        End If
                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                            If (oGrossTotal.SumInsured) > 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured), 0) > 0 Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)) / (oGrossTotal.SumInsured)
                            End If
                        Else
                            If (oGrossNet.SumInsured) > 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured), 0) > 0 Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)) / (oGrossNet.SumInsured)
                            End If
                        End If

                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * oGrossNet.ThisReserve
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * oGrossNet.ThisPayment

                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * oGrossNet.ThisPayment
                        If Not m_bReadOnly Then
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = oGrossNet.RecoveredToDate * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0) > (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)))
                            End If
                            If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0) > gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit)
                            End If
                            If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0) > (oGrossNet.RecoveredToDate - oTtyTotal.RecoveredToDate) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = (oGrossNet.RecoveredToDate - oTtyTotal.RecoveredToDate) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If
                        End If

                        'm_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = oGrossNet.PaymentToDate * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)

                        'If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0) > (oGrossNet.PaymentToDate - oTtyTotal.PaymentToDate) Then
                        '    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = (oGrossNet.PaymentToDate - oTtyTotal.PaymentToDate) * gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                        'End If

                        If gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) <> gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare), 0) And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) > 0 Then
                            bThisPercentageIsChanged = True
                        End If

                        lTQSHSumInsuredTotal += m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)
                        If m_bRecovery Then
                            'm_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0) + gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0)
                        Else
                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = Math.Round(gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0), 4) + Math.Round(gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0), 4) - Math.Round(gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0), 4) - Math.Round(gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0), 4)
                        End If
                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) <> "1" Then
                            oTtyTotal.Add(m_oXA, lCount, m_bRecovery)
                        End If
                        If Not m_bReadOnly And Not bNeedsTreatyAllocation Then
                            lTQSHReserveTotal += m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve)
                            lTQSHPaymentTotal += m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)
                        End If

                    Case "TX"
                        If m_bReadOnly Then

                        ElseIf (m_bReadOnly And Not bNeedsTreatyAllocation) Or bThisPercentageIsChanged Then
                            iCedePremiumMark = ToSafeInteger(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007CedePremiumOnly))
                            If (oGrossNet.SumInsured - oTtyTotal.SumInsured) >= gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) Then
                                If iCedePremiumMark = 1 Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                                End If
                            ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured) < gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) And (oGrossNet.SumInsured - oTtyTotal.SumInsured) > gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) Then
                                If iCedePremiumMark = 1 Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = (gPMFunctions.ToSafeDouble(oGrossNet.SumInsured, 0) - gPMFunctions.ToSafeDouble(oTtyTotal.SumInsured)) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                                End If
                            ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured - oSurTotal.SumInsured) <= gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
                            End If
                        End If

                    Case "NL"
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) - m_oXA(m_lASumBand + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) - m_oXA(m_lASumBand + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) - m_oXA(m_lASumBand + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) - m_oXA(m_lASumBand + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) - m_oXA(m_lASumBand + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) - m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate)
                        m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) - m_oXA(m_lASumBand + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance)
                End Select

            Next

            'Adjust the Treaty XOL Totals
            If Not m_bReadOnly And bNeedsTreatyAllocation Then
                For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                    Select Case m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)
                        Case "T"
                            If m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) <> "1" Then

                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = (ToSafeDouble(oGrossNet.ThisReserve)) * ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0)
                            End If
                            If m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) <> "1" Then
                                lTQSHReserveTotal = lTQSHReserveTotal + m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve)
                                lTQSHPaymentTotal = lTQSHPaymentTotal + m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)
                            End If
                        Case "TX"
                            iCedePremiumMark = ToSafeInteger(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007CedePremiumOnly))
                            If oGrossNet.ThisReserve - oSurTotal.ThisReserve - lTQSHReserveTotal >= ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                            ElseIf oGrossNet.ThisReserve - oSurTotal.ThisReserve - lTQSHReserveTotal <= ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = 0
                            Else
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = ToSafeDouble(oGrossNet.ThisReserve - oSurTotal.ThisReserve - lTQSHReserveTotal) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                            End If
                            If oGrossNet.ThisPayment - oSurTotal.ThisPayment - lTQSHPaymentTotal >= ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                            ElseIf oGrossNet.ThisPayment - oSurTotal.ThisPayment - lTQSHPaymentTotal <= ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = 0
                            Else
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = ToSafeDouble(oGrossNet.ThisPayment - oSurTotal.ThisPayment - lTQSHPaymentTotal) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                            End If

                            If (oGrossNet.SumInsured - oTtyTotal.SumInsured - oSurTotal.SumInsured) >= ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) Then

                                If iCedePremiumMark = 1 Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                                End If
                            ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured - oSurTotal.SumInsured) < ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) And (oGrossNet.SumInsured - oTtyTotal.SumInsured - oSurTotal.SumInsured) > m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) Then
                                ' # changes for Cede Premium Only

                                If iCedePremiumMark = 1 Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = (ToSafeDouble(oGrossNet.SumInsured, 0) - ToSafeDouble(oTtyTotal.SumInsured)) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                                End If
                            ElseIf (oGrossNet.SumInsured - oTtyTotal.SumInsured) <= m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = 0
                            End If

                            If m_bRecovery = True Then
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0) + ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0)
                            Else
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0) + ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0) - ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0)
                            End If

                    End Select


                Next
            End If

            'Recalculate the Treaty Totals
            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                    Case "TX" ', "T"
                        oTtyTotal.Add(m_oXA, lCount, m_bRecovery)
                End Select
            Next
            If Not m_bReadOnly Then
                ' Store allocated total
                If TransactionType = "C_CR" Or TransactionType = "C_CP" Or TransactionType = "C_SA" Or TransactionType = "C_RV" Then
                    If oGrossTotal.ReserveToDate > (oSurTotal.ReserveToDate + oTtyTotal.ReserveToDate + oFacTotal.ReserveToDate) Then
                        oRetained.ReserveToDate = oGrossTotal.ReserveToDate - oTtyTotal.ReserveToDate - oFacTotal.ReserveToDate - oSurTotal.ReserveToDate
                    Else
                        oRetained.ReserveToDate = oGrossTotal.ReserveToDate - oTtyTotal.ReserveToDate - oFacTotal.ReserveToDate - oSurTotal.ReserveToDate
                    End If
                    If oGrossTotal.PaymentToDate > (oTtyTotal.PaymentToDate + oFacTotal.PaymentToDate) Then
                        oRetained.PaymentToDate = oGrossTotal.PaymentToDate - oTtyTotal.PaymentToDate - oFacTotal.PaymentToDate - oSurTotal.PaymentToDate
                    End If
                    If oGrossTotal.RecoveredToDate * -1 > (oTtyTotal.RecoveredToDate + oFacTotal.RecoveredToDate) * -1 Then
                        oRetained.RecoveredToDate = oGrossTotal.RecoveredToDate - oTtyTotal.RecoveredToDate - oFacTotal.RecoveredToDate - oSurTotal.RecoveredToDate
                    End If

                End If
            End If

            'Calculate Retained for This Reserve
            If Math.Abs(oGrossTotal.ThisReserve) > Math.Abs(oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oSurTotal.ThisReserve) Then
                oRetained.ThisReserve = oGrossTotal.ThisReserve - oFacTotal.ThisReserve - oTtyTotal.ThisReserve - oSurTotal.ThisReserve
            Else
                oRetained.ThisReserve = oGrossTotal.ThisReserve - oFacTotal.ThisReserve - oTtyTotal.ThisReserve - oSurTotal.ThisReserve
            End If

            'Calculate Retained for Sum Insured
            cCurrentRetained = oGrossNet.SumInsured - oTtyTotal.SumInsured - oSurTotal.SumInsured
            oRetained.SumInsured = cCurrentRetained
            'If cCurrentRetained < cRetainedLineLimit Then
            '    oRetained.SumInsured = cCurrentRetained
            'Else
            '    oRetained.SumInsured = cRetainedLineLimit
            'End If

            'Calculate Retained for This Payment
            If oGrossTotal.ThisPayment > 0 Then
                If oGrossTotal.ThisPayment > (oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oSurTotal.ThisPayment) Then
                    oRetained.ThisPayment = oGrossTotal.ThisPayment - oFacTotal.ThisPayment - oTtyTotal.ThisPayment - oSurTotal.ThisPayment
                Else
                    oRetained.ThisPayment = 0
                End If
            ElseIf m_bRecovery Then
                If (oGrossTotal.ThisPayment - (oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oSurTotal.ThisPayment)) <= cRetainedLineLimit Then
                    oRetained.ThisPayment = oGrossTotal.ThisPayment - oFacTotal.ThisPayment - oTtyTotal.ThisPayment - oSurTotal.ThisPayment
                Else
                    oRetained.ThisPayment = cRetainedLineLimit
                End If
            End If

            'Set the Balances

            dGrossFACXOLTotal = 0
            lTreatyRecoveredToDate = 0
            'Note:This portion will calculate the Treaty Qouta share and XOL Incurred
            Dim dGrossQuotaShare As Double

            Dim dGrossTreatyXOL As Double

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                    Case "TX", "FX", "T", "F", "TFS"

                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Then
                            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) <> "1" Then
                                dGrossQuotaShare += gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * (oGrossNet.Incurred)
                                m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare), 0) * (oGrossNet.Incurred - lTreatyRecoveredToDate)
                            End If

                            lTreatyRecoveredToDate = CInt(lTreatyRecoveredToDate + gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0))

                        End If

                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Then
                            If gPMFunctions.ToSafeBoolean(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) And m_sTransactionType = "C_CO" Then
                                If oGrossNet.Incurred - oSurTotal.Incurred - dGrossQuotaShare <= gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0) Then 'Parallel PN70990 of PN64438
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = 0
                                    If m_bRecovery Then
                                        If gPMFunctions.ToSafeInteger(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)) = 0 Then
                                            m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) '- oRetained.ThisPayment
                                        End If
                                    End If
                                ElseIf oGrossNet.Incurred - oSurTotal.Incurred - dGrossQuotaShare > gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) Then  'Parallel PN70990 of PN64438
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = (oGrossNet.Incurred - oSurTotal.Incurred - dGrossQuotaShare) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0) 'Parallel PN70990 of PN64438
                                End If

                            End If
                            dGrossTreatyXOL += gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0)
                        End If
                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Then
                        End If

                        If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then
                            If m_sTransactionType = "C_CO" Then
                                If oGrossTotal.Incurred - oFACPropTotal.Incurred < gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0) Then 'PN64438
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = 0
                                ElseIf oGrossTotal.Incurred - oFACPropTotal.Incurred >= gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) Then
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit), 0) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * (1 - m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained))
                                Else
                                    m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = ((oGrossTotal.Incurred - oFACPropTotal.Incurred) - gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit), 0)) * (1 - m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained))
                                End If
                            End If
                            'This FACXOLTotal can be used for calculating Treaty Quota share and Treaty XOL
                            dGrossFACXOLTotal += gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred), 0)
                        End If

                    Case "R"
                        oRetained.Balance = oGrossNet.Balance - oTtyTotal.Balance - oSurTotal.Balance
                End Select
            Next

            If iPositionRetainedLine > 0 Then
                oRetained.Store(m_oXA, iPositionRetainedLine)
            End If

            If TransactionType <> "C_CP" Then
                If oGrossTotal.ThisReserve >= (oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oRetained.ThisReserve + oSurTotal.ThisReserve) Then
                    oAllocated.ThisReserve = oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oRetained.ThisReserve + oSurTotal.ThisReserve
                    oAllocated.SumInsured = oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured + oSurTotal.SumInsured
                    oAllocated.ReserveToDate = oTtyTotal.ReserveToDate + oFacTotal.ReserveToDate + oRetained.ReserveToDate + oSurTotal.ReserveToDate
                    oAllocated.PaymentToDate = oTtyTotal.PaymentToDate + oFacTotal.PaymentToDate + oRetained.PaymentToDate + oSurTotal.PaymentToDate
                    oAllocated.ThisPayment = oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment
                    oAllocated.RecoveredToDate = oTtyTotal.RecoveredToDate + oFacTotal.RecoveredToDate + oRetained.RecoveredToDate + oSurTotal.RecoveredToDate
                    oAllocated.Balance = oTtyTotal.Balance + oFacTotal.Balance + oRetained.Balance + oSurTotal.Balance
                    oAllocated.IncurredToDate = oGrossTotal.IncurredToDate
                ElseIf oGrossTotal.SumInsured < (oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured + oSurTotal.SumInsured) Then
                    oAllocated.SumInsured = oGrossTotal.SumInsured
                    oAllocated.ThisReserve = oGrossTotal.ThisReserve
                    oAllocated.ReserveToDate = oGrossTotal.ReserveToDate
                    oAllocated.PaymentToDate = oGrossTotal.PaymentToDate
                    oAllocated.RecoveredToDate = oGrossTotal.RecoveredToDate
                    oAllocated.Balance = oGrossTotal.Balance
                    oAllocated.IncurredToDate = oGrossTotal.IncurredToDate
                End If
            ElseIf TransactionType = "C_CP" Then
                If oGrossTotal.ThisPayment >= (oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment) Then
                    oAllocated.ThisReserve = oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oRetained.ThisReserve + oSurTotal.ThisReserve
                    oAllocated.ThisPayment = oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment
                    oAllocated.ReserveToDate = oTtyTotal.ReserveToDate + oFacTotal.ReserveToDate + oRetained.ReserveToDate + oSurTotal.ReserveToDate
                    oAllocated.PaymentToDate = oTtyTotal.PaymentToDate + oFacTotal.PaymentToDate + oRetained.PaymentToDate + oSurTotal.PaymentToDate
                    oAllocated.RecoveredToDate = oTtyTotal.RecoveredToDate + oFacTotal.RecoveredToDate + oRetained.RecoveredToDate + oSurTotal.RecoveredToDate
                    oAllocated.SumInsured = oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured + oSurTotal.SumInsured
                    oAllocated.Balance = oTtyTotal.Balance + oFacTotal.Balance + oRetained.Balance + oSurTotal.Balance
                    oAllocated.IncurredToDate = oGrossTotal.IncurredToDate
                ElseIf oGrossTotal.ThisPayment < (oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment) Then
                    oAllocated.SumInsured = oGrossTotal.SumInsured
                    oAllocated.ThisReserve = oGrossTotal.ThisReserve
                    oAllocated.ReserveToDate = oGrossTotal.ReserveToDate
                    oAllocated.PaymentToDate = oGrossTotal.PaymentToDate
                    oAllocated.RecoveredToDate = oGrossTotal.RecoveredToDate
                    oAllocated.ThisPayment = oGrossTotal.ThisPayment
                    oAllocated.Balance = oGrossTotal.Balance
                    oAllocated.IncurredToDate = oGrossTotal.IncurredToDate
                End If

            End If

            ' Store the Allocated amounts
            oAllocated.Store(m_oXA, m_lASumAlloc)

            ' Don't show allocated sum insured
            ' Check unallocated and add/remove as necessary
            If Math.Abs(oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oRetained.ThisReserve + oSurTotal.ThisReserve - m_oRIArrangement.ThisReserve) > 0.005 Or Math.Abs(oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment - m_oRIArrangement.ThisPayment) > 0.005 Or Math.Abs(oTtyTotal.SumInsured + oFacTotal.SumInsured + oRetained.SumInsured + oSurTotal.SumInsured - m_oRIArrangement.SumInsured) > 0.005 Then

                ' Display an allocated line
                If m_lASumUnalloc = 0 Then
                    ' Append to grid
                    m_oXA.Rows.InsertAt(m_oXA.NewRow, m_oXA.Rows.Count)
                    m_oXA.AppendRows()
                    m_lASumUnalloc = m_oXA.Rows.Count - 1
                End If

                ' Populate

                m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Unallocated"
                m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = 1 - oTtyTotal.SharePercent - oFacTotal.SharePercent - oRetained.SharePercent - oSurTotal.SharePercent
                m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = m_oRIArrangement.ThisReserve - oTtyTotal.ThisReserve - oFacTotal.ThisReserve - oRetained.ThisReserve - oSurTotal.ThisReserve
                m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = m_oRIArrangement.ThisPayment - oTtyTotal.ThisPayment - oFacTotal.ThisPayment - oRetained.ThisPayment - oSurTotal.ThisPayment
                m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = m_oRIArrangement.SumInsured - oTtyTotal.SumInsured - oFacTotal.SumInsured - oRetained.SumInsured - oSurTotal.SumInsured

                m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "UT"
                m_lUnallocatedSumInsured = m_oRIArrangement.SumInsured - oTtyTotal.SumInsured - oFacTotal.SumInsured - oRetained.SumInsured - oSurTotal.SumInsured ' PN 76602
                m_lunAllocatedReserve = CInt(m_oRIArrangement.ThisReserve - (oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oRetained.ThisReserve + oSurTotal.ThisReserve))
                m_lUnallocatedPayment = CInt(m_oRIArrangement.ThisPayment - (oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment))

            Else
                m_lUnallocatedSumInsured = m_oRIArrangement.SumInsured - oTtyTotal.SumInsured - oFacTotal.SumInsured - oRetained.SumInsured - oSurTotal.SumInsured ' PN 76602
                m_lunAllocatedReserve = CInt(m_oRIArrangement.ThisReserve - (oTtyTotal.ThisReserve + oFacTotal.ThisReserve + oRetained.ThisReserve + oSurTotal.ThisReserve))
                m_lUnallocatedPayment = CInt(m_oRIArrangement.ThisPayment - (oTtyTotal.ThisPayment + oFacTotal.ThisPayment + oRetained.ThisPayment + oSurTotal.ThisPayment))
                ' If line exists remove it
                If m_lASumUnalloc > 0 Then
                    'changed because DeleteRows is deleting more rows instead of the specified
                    Try
                        lReturn = gPMConstants.PMEReturnCode.PMTrue
                        RemoveHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                        m_oXA.Rows.RemoveAt(m_lASumUnalloc)
                        m_oXA.AcceptChanges()
                        AddHandler grdRI.CellEnter, AddressOf grdRI_CellEnter
                    Catch ex As Exception
                        lReturn = gPMConstants.PMEReturnCode.PMFalse
                    End Try

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oXA.DeleteRows(m_lASumUnalloc, 1)", "Unable to remove unallocated totals")
                    End If
                    m_lASumUnalloc = 0
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        End Try
        Return result
    End Function

    Private Sub grdRI_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdRI.CellFormatting
        If Not e.Value Is DBNull.Value Then
            Dim dTempVal As Double = 0D
            If e.ColumnIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained _
             Or e.ColumnIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare Then
                If (Double.TryParse(e.Value, dTempVal)) Then
                    e.Value = dTempVal.ToString("P2")
                End If
            ElseIf e.ColumnIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare Then
                If (Double.TryParse(e.Value, dTempVal)) Then
                    e.Value = dTempVal.ToString("P4")
                End If
            ElseIf e.CellStyle.Format = "0.00" Or e.CellStyle.Format = "N2" Then
                e.Value = Math.Round(gPMFunctions.ToSafeDouble(e.Value), 2)
            ElseIf e.CellStyle.Format = "N2" Then
                e.Value = Math.Round(gPMFunctions.ToSafeDouble(e.Value), 2)
            Else
                e.Value = e.Value
            End If
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
        Dim dObligatorySumInsured, dObligatoryReserve As Double
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

            dObligatorySumInsured = 0
            dObligatoryReserve = 0

            For lCount As Integer = 0 To m_oXA.Rows.Count - 1
                If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                    dObligatorySumInsured = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)), 0)
                    dObligatoryReserve = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve)), 0)
                    Exit For
                End If
            Next

            Dim dTempThisShare As Double = 0D
            If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare Then
                If Not m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) Is DBNull.Value Then
                    If gPMFunctions.ToSafeString(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)).Contains("%") Then
                        dTempThisShare = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare).ToString.Replace("%", "").Trim), 0) / 100
                    Else
                        dTempThisShare = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)), 0) / 100
                    End If
                    RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                    RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = dTempThisShare
                    AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                    AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                End If
            End If

            Dim dTempDefaultShare As Double = 0D
            If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare Then
                If Not m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare) Is DBNull.Value Then
                    If gPMFunctions.ToSafeString(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)).Contains("%") Then
                        dTempDefaultShare = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare).ToString.Replace("%", "").Trim), 0) / 100
                    Else
                        dTempDefaultShare = gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)), 0) / 100
                    End If
                    RemoveHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                    RemoveHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare) = dTempDefaultShare
                    AddHandler grdRI.CellUpdating, AddressOf grdRI_CellUpdating
                    AddHandler grdRI.CellUpdated, AddressOf grdRI_CellUpdated
                End If
            End If

            If iPMFunc.IsIn(Convert.ToString(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)), "F") Then
                ' Sum insured will affect share and premiums
                If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare Then

                    ' Recalculate share if we can
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = (gPMFunctions.ToSafeDouble(m_oRIArrangement.ThisReserve, 0) - dObligatoryReserve) * gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)), 0)
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = (gPMFunctions.ToSafeDouble(m_oRIArrangement.SumInsured, 0) - dObligatorySumInsured) * gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)), 0)
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = gPMFunctions.ToSafeDouble(m_oRIArrangement.ThisPayment, 0) * gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)), 0)
                    If m_bRecovery Then
                        'm_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0) + gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0) - gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0)
                    Else
                        m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0) + gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0) - gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0) - gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0)
                    End If
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Incurred) = gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate), 0) + gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve), 0) - gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment), 0) - gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate), 0) + gPMFunctions.ToSafeDouble(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate), 0)
                End If
            End If

            If iPMFunc.IsIn(Convert.ToString(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)), "T") And TransactionType = "C_CO" Then
                If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare Then
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = gPMFunctions.ToSafeDouble(m_oRIArrangement.SumInsured, 0) * gPMFunctions.ToSafeDouble(Conversion.Val(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)), 0)
                End If
            End If

            ' Check for appropriate row type
            If iPMFunc.IsIn(Convert.ToString(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)), "T", "R", "F", "FX", "TX") Then
                ' Reserve will affect share percentage
                If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve Then
                    ' Recalculate premium share, if we can
                    If m_oRIArrangement.Reserve <> 0 Then
                        m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve))) / m_oRIArrangement.Reserve
                    End If
                End If

                If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve Then
                    ' Recalculate premium share, if we can
                    If m_oRIArrangement.Reserve <> 0 Then
                        m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve))) / m_oRIArrangement.Reserve
                    End If
                End If


                'calculate This percent if this Recovery changed - PN 58905
                If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment And (m_sTransactionType = "C_RV" Or m_sTransactionType = "C_SA") Then
                    If m_oRIArrangement.ThisPayment <> 0 Then
                        m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment))) / m_oRIArrangement.ThisPayment
                    End If
                End If

                ' Sum insured will affect share and premiums
                If iPMFunc.IsIn(CStr(ColIndex), ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) Then
                    ' Recalculate balance
                    m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve))) - (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment)))
                End If

                ' Future: If col is payment then check XOL
                If ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment Then
                    ' Currently XOL is only available in automated claim processing.
                    ' Manual claims processing will not trigger XOL, if it did this
                    ' is where you should check the XOL limits.
                End If

                If iPMFunc.IsIn(CStr(ColIndex), ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) Then
                    If iPMFunc.IsIn(Convert.ToString(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)), "TX", "FX") Then
                        m_lReturn = CType(SortGrid(m_oXA(lRow, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)), gPMConstants.PMEReturnCode)
                    End If
                End If

            End If

            ' Rollup and rebind
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
        If Not grdRI.CurrentCell Is Nothing Then
            If Not grdRI.CurrentCell.Equals(grdRI.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim OldValue As Object = grdRI.CurrentRow.Cells(ColIndex).Value
        Dim Cancel As Integer = 0

        Dim NewValue As Object
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
            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured
                'Editable When
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                    ' Simple numeric validation
                    ' Set changed state

                    m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                Else
                    sMessage = "Invalid Sum Insured Entered"
                End If
            Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007ThisShare
                If Convert.ToString(NewValue).IndexOf("%"c) >= 0 Then
                    NewValue = Convert.ToString(NewValue).Replace("%", "").Trim
                End If

                ' Validate range
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                    ' Simple numeric validation
                    If Conversion.Val(gPMFunctions.ToSafeString(NewValue)) >= 0.0001 And Conversion.Val(gPMFunctions.ToSafeString(NewValue)) <= 100 Then
                        ' Set changed state

                        m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                        NewValue = Conversion.Val(gPMFunctions.ToSafeString(NewValue)) / 100
                        'If Treaty Type is FAC and Transcation type is Open Claim then we can change ThisShare %
                        'So we need to set AddMode to "MA" to re-allocate whole RI

                        If m_bCellChanged Then
                            grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode).Value = "MA"
                        End If
                    Else
                        sMessage = "Invalid This Rate Entered"
                    End If
                Else
                    sMessage = "Invalid This Rate Entered"
                End If
            Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007DefaultShare
                If Convert.ToString(NewValue).IndexOf("%"c) >= 0 Then
                    NewValue = Convert.ToString(NewValue).Replace("%", "").Trim
                End If

                ' Validate range
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                    ' Simple numeric validation
                    If Conversion.Val(gPMFunctions.ToSafeString(NewValue)) >= 0.01 And Conversion.Val(gPMFunctions.ToSafeString(NewValue)) <= 100 Then
                        ' Set changed state
                        m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                        NewValue = Conversion.Val(gPMFunctions.ToSafeString(NewValue)) / 100
                    Else
                        sMessage = "Invalid This Rate Entered"
                    End If
                Else
                    sMessage = "Invalid This Rate Entered"
                End If

            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment
                ' Validate range
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                    ' Set changed state
                    m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                Else
                    sMessage = "This " & (IIf(ColIndex = ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve, "reserve", "payment")) & " must be a valid currency value"
                End If

            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit
                ' Lower Limit can never be less then 0
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeDouble(eventArgs.Value) < 0 Then
                    sMessage = "Invalid Lower Limit Entered"
                    Cancel = 1
                    m_bCellChanged = False
                Else

                    ' Loop through the arrangement to check for clashes in
                    ' Lower and Upper Limit
                    For lCounter As Integer = 1 To m_oXA.Rows.Count - 1
                        If Convert.ToString(m_oXA(lCounter, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or Convert.ToString(m_oXA(lCounter, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then


                            NewValue = eventArgs.Value
                            m_bCellChanged = True
                            If grdRI.CurrentRowIndex <> lCounter Then
                                ' if    New Value falls in the intervel of other Placements then displays
                                '       an Error message as it is not allowed
                                ' Else
                                ' Lower Limit entered > Lower Limit of other placements and
                                ' Upper Limit entered < Upper Limit of other placements then
                                ' DISPLAy and error message as their is a CLASH
                                If gPMFunctions.ToSafeDouble(NewValue) > gPMFunctions.ToSafeDouble(m_oXA(lCounter, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) And gPMFunctions.ToSafeDouble(NewValue) < gPMFunctions.ToSafeDouble(m_oXA(lCounter, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)) Then
                                    sMessage = "Treaty XOl lower limit overlaps with another Treaty XOL Layer"
                                    Cancel = 1
                                    m_bCellChanged = False
                                    Exit For
                                ElseIf gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit).Value) <= m_oXA(lCounter, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) And gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit).Value) > m_oXA(lCounter, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) Then
                                    sMessage = "Treaty XOl limit overlaps with another Treaty XOL Layer"
                                    Cancel = 1
                                    m_bCellChanged = False
                                    Exit For

                                Else
                                    m_bCellChanged = True
                                End If
                            Else
                                If Convert.ToString(grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit).Value) = "" Or Convert.ToString(grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit).Value) = "" Then
                                    'Do nothing
                                Else
                                    ' Check that Upper Limit should always be greater than
                                    ' the Lower Limit for the same Placement
                                    If gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit).Value) >= gPMFunctions.ToSafeDouble(grdRI.CurrentRow().Cells(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit).Value) Then
                                        sMessage = "Treaty XOL Upper Limit must be greater than Treart XOL Lower Limit"
                                        Cancel = 1
                                        m_bCellChanged = False
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Case RiskRIArrangement.RiskReinsuranceEnumRI2007.DBRI2007CommPercent
                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(NewValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If gPMFunctions.ToSafeString(NewValue).IndexOf("%"c) >= 0 Then

                        NewValue = gPMFunctions.ToSafeDouble(gPMFunctions.ToSafeString(NewValue).Substring(0, Marshal.SizeOf(NewValue) - 1).TrimEnd())
                    End If
                End If

                ' Validate range
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                    ' Simple numeric validation
                    If Conversion.Val(gPMFunctions.ToSafeString(NewValue)) >= 1 And Conversion.Val(gPMFunctions.ToSafeString(NewValue)) <= 100 Then
                        ' Set changed state

                        m_bCellChanged = (NewValue <> gPMFunctions.ToSafeDouble(OldValue))
                        NewValue = Conversion.Val(gPMFunctions.ToSafeString(NewValue)) / 100
                    Else
                        sMessage = "Invalid This Rate Entered"
                    End If
                Else
                    sMessage = "Invalid This Rate Entered"
                End If

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

                eventArgs.Cancel = True
                eventArgs.Value = OldValue
            Else
                eventArgs.Value = NewValue
            End If
        End If


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
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
            SelectedRIType = m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)
            SelectedRow = grdRI.CurrentRowIndex
            If m_sSelectedRIType = "FX" Then
                If Not m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007GroupingID) Is DBNull.Value Then
                    SelRIArrangementLine = gPMFunctions.ToSafeLong(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007GroupingID))
                End If
            Else
                If Not m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID) Is DBNull.Value Then
                    SelRIArrangementLine = m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID)
                End If
            End If
            m_lGroupingID = gPMFunctions.ToSafeLong(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007GroupingID))
            m_cUpperLimit = gPMFunctions.ToSafeCurrency(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit))
            m_cLowerLimit = gPMFunctions.ToSafeCurrency(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit))
            m_bisRiBroker = gPMFunctions.ToSafeBoolean(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsRIBroker))
            m_lPartyCnt = gPMFunctions.ToSafeLong(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt))
            If Convert.ToString(m_oXA(grdRI.CurrentRowIndex, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                RaiseEvent ResetControls(Me, New ResetControlsEventArgs("T"))
            Else
                RaiseEvent ResetControls(Me, New ResetControlsEventArgs(SelectedRIType))
            End If
        End If

    End Sub

    ' ***************************************************************** '
    '                       USERCONTROL EVENTS
    ' ***************************************************************** '

    Private Sub UserControl_InitProperties()
        ' Initialise read only state
        ReadOnly_Renamed = False
    End Sub

    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        ' Load read only state
        ReadOnly_Renamed = CBool(PropBag.ReadProperty("ReadOnly", False))
    End Sub
    Private Sub uctClaimRIControlRI2007_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim vIsEditRI As Object
        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableClaimRIEditing, v_vBranch:=g_iSourceID, r_vUnderwriting:=vIsEditRI)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("getProductOptionValue", "Unable to call getProductOptionValue")
        End If
        If vIsEditRI = "" Or vIsEditRI = "0" Then
            m_bCanEditClaimRI = False
        Else
            m_bCanEditClaimRI = True
        End If
    End Sub


    Private Sub uctClaimRIControlRI2007_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            grdRI.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)
        Catch
        End Try
    End Sub
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        ' Save read only state
        PropBag.WriteProperty("ReadOnly", ReadOnly_Renamed, False)
    End Sub

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

                    If m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = sType Then
                        lPosition = lCount
                    Else
                        lPosition = lCount
                        Exit For
                    End If
                Next
            Case "FX"
                For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                    If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Then
                        lPosition = lCount + 1
                    ElseIf Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                        lPosition = lCount + 1
                    ElseIf Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then
                        If m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) < cLowerLimit Then
                            lPosition = lCount
                        ElseIf gPMFunctions.ToSafeDecimal(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) >= cLowerLimit Then
                            lPosition = lCount
                            Exit For
                        End If
                    End If
                Next
            Case "T"
                For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                    If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FT" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then 'PN 71699
                        lPosition = lCount
                    Else
                        lPosition = lCount
                        Exit For
                    End If
                Next
            Case "TX"
                For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                    If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FT" Or Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                        lPosition = lCount + 1
                    ElseIf Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Then
                        If gPMFunctions.ToSafeDecimal(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) < cLowerLimit Then
                            lPosition = lCount
                        ElseIf gPMFunctions.ToSafeDecimal(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) >= cLowerLimit Then
                            lPosition = lCount
                            Exit For
                        End If
                    End If
                Next
            Case "NL"
                If bIsObligatory Then
                    lPosition = 3
                End If
        End Select

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
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)).ToLower() = sRIType.ToLower() Then
                For innercount As Integer = m_oXA.GetLowerBound(0) To lCount
                    If Convert.ToString(m_oXA(innercount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)).ToLower() = sRIType.ToLower() And gPMFunctions.ToSafeCurrency(m_oXA(innercount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) > gPMFunctions.ToSafeCurrency(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)) Then
                        m_lReturn = CType(SwapRows(innercount, lCount), gPMConstants.PMEReturnCode)
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

        ReDim vSaveRow1(ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max + 1)

        For lCount As Integer = 0 To (ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Max)

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
        m_vStoreLimits = VariantType.Empty

        Dim lCountFXRows As Integer
        lIndex = 0

        For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then
                lCountFXRows += 1
            End If
        Next

        For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Then
                If Not Information.IsArray(m_vStoreLimits) Then
                    ReDim m_vStoreLimits(lCountFXRows - 1, 2)

                    m_vStoreLimits(0, 0) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)

                    m_vStoreLimits(0, 1) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)

                    m_vStoreLimits(0, 2) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007GroupingID)

                Else

                    m_vStoreLimits(lIndex, 0) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit)

                    m_vStoreLimits(lIndex, 1) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit)

                    m_vStoreLimits(lIndex, 2) = m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007GroupingID)
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

    Public Function AddFacultativeXOL(ByVal lPartyCnt As Integer, ByVal sDescription As String, ByVal dRetained As Double, ByVal cLowerLimit As Decimal, ByVal cUpperLimit As Decimal, ByVal cSumInsured As Decimal, ByVal lGroupingId As Integer, ByVal sTransactionType As String) As Integer

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
            If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt)) = lPartyCnt And sDescription <> "Multiple Acts" Then
                result = gPMConstants.PMEReturnCode.PMRecordInUse
            End If

            ' Check priority
            If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority)) > lPriority Then
                lPriority = gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority), 0)
            End If
        Next

        Dim lObligatoryIndex As Integer
        lObligatoryIndex = 0
        For lCount As Integer = 0 To m_oXA.Rows.Count - 1
            Select Case Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
                Case "T", "F"
                    If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                        lObligatoryIndex = lCount
                        Exit For
                    End If
            End Select
        Next

        If lObligatoryIndex <> 0 Then
            If Not m_bIsFacPropExists Then

                For lCount As Integer = 1 To m_oXA.Rows.Count - 1
                    If gPMFunctions.ToSafeString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                        m_oXA.Rows.RemoveAt(lCount)
                        m_oXA.AcceptChanges()
                        Exit For
                    End If
                Next

                lReturn = AllocateObligatory()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, "Failed to allocate Obligatory first", gPMConstants.PMELogLevel.PMLogError)
                Else
                    m_bNewFACPropORFACXOL = True
                End If
            End If
        End If
    
        ' Determine the position after looking at the current RI arrangement
        ' and get the position at which we need to Insert the New Row

        m_lReturn = CType(IsExists_NetLineRow(bIsExistNetLineRow), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to locate Net line Obligatory ", gPMConstants.PMELogLevel.PMLogError)
        End If

        If lObligatoryIndex <> 0 And bIsExistNetLineRow Then
            m_lReturn = CType(FindInsertPosition("FX", lPosition), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to find the position to insert Net line Obligatory ", gPMConstants.PMELogLevel.PMLogError)
            End If
        Else
            m_lReturn = CType(FindInsertPosition("FX", lPosition, cLowerLimit), gPMConstants.PMEReturnCode)
        End If
        ' Insert new row on to that position
        m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition)
       
        ' Populate new line
        ' Grid fields
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "FAC XOL"
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = sDescription
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Retained) = dRetained / 100
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit) = cLowerLimit
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit) = cUpperLimit
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare) = ""
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured) = cSumInsured
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ReserveToDate) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PaymentToDate) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate) = 0
        'E016
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsReinsurerApproved) = "U"
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Balance) = 0
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode) = ""
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode) = "MA"
        ' Supporting fields
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007GroupingID) = lGroupingId
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineID) = lGroupingId
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "FX"

        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007TreatyID) = Nothing
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007PartyCnt) = lPartyCnt

        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007XolID) = Nothing
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Priority) = lPriority + 1
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Lines) = 1
        m_oXA(lPosition, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LineLimit) = 0

        ' Increment total lines...
        m_lASumTreaty += 1
        m_lASumFAC += 1
        m_lASumTotal += 1
        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)
        m_lASumOriginal = IIf(m_lASumOriginal = 0, 0, m_lASumOriginal + 1)
        m_lASumNet = IIf(m_lASumNet = 0, 0, m_lASumNet + 1)

        ' Rebind data
        grdRI.ReBind()

        ' ***********************************************************
        ' We need to insert Gross Net Row if it does'nt exists
        ' So find out whether Gross Net exists or not
        m_lReturn = CType(IsExists_GrossNetRow(bIsExistGrossNetRow), gPMConstants.PMEReturnCode)

        ' if it does'nt then Insert the New row and populate the
        ' the amounts accordingly
        If Not bIsExistGrossNetRow Then
            m_oXA.Rows.InsertAt(m_oXA.NewRow, lPosition + 1)
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement) = "GROSS NET"
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name) = "Net of FAC"
            m_oXA(lPosition + 1, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type) = "FT"

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
            If m_bIsFacPropExists Or m_bIsFacxolExists Then
                If Convert.ToString(m_oXA(lPosition + 2, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = "1" Then
                    m_oXA.Rows.RemoveAt(lPosition + 2)
                    m_oXA.AcceptChanges()
                End If
            End If
        End If

        m_lReturn = CType(SortGrid("FX"), gPMConstants.PMEReturnCode)
        ' Rebind data
        grdRI.ReBind()

        m_lReturn = StoreLimits()
        m_bIsDirty = True
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

        Try

        result = gPMConstants.PMEReturnCode.PMTrue
        bValid = True

        For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" And Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode)) = "MA" Then
                If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)).Trim() = "" Or gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)) = 0 Then
                    MessageBox.Show("Invalid Ceding Rate Entered", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    bValid = False
                    Exit For
                End If
            End If
        Next

        For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" And m_sTransactionType = "C_CO" Then
                If (gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)) * 100) < 0.01 Or gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)) > 1 Then
                    If (m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Placement)) = "Treaty QSH" And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare)) = 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare)) = 0 And gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured)) = 0 Then
                    Else
                        MessageBox.Show("This % must be between 0.01% and 100.00% inclusively", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        bValid = False
                        Exit For
                    End If
                End If
            End If
        Next

        Dim dThisShare As Double
        dThisShare = 0
        For lCount As Integer = m_oXA.GetLowerBound(0) To m_oXA.Rows.Count - 1
            If Convert.ToString(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" And m_sTransactionType = "C_CO" Then
                dThisShare += gPMFunctions.ToSafeDouble(m_oXA(lCount, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare))
            End If
        Next
        If dThisShare > 1 Then 'check if it is greater than 100%
            MessageBox.Show("This % for ALL Treaty QSH rows must not exceed 100.00%", Application.ProductName)
            bValid = False
        End If


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
                    Select Case gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type))
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
                            grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Italic Or FontStyle.Bold)
                            grdRI.Rows(dr.Index).ReadOnly = True
                        Case "T", "F"
                            If ToSafeInteger(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory)) = 1 Then
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = Color.SkyBlue
                                grdRI.Rows(dr.Index).ReadOnly = True
                            End If
                    End Select

                    If fsColumn(dc.Index) Then
                        Select Case dc.Index
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007RecoveredToDate
                                If grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment <> DataGridViewContentAlignment.MiddleRight Or _
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor <> SystemColors.ButtonFace Or _
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False Then
                                    If Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" _
                                        Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TC" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    End If
                                End If
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisPayment
                                If grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment <> DataGridViewContentAlignment.MiddleRight Or _
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor <> SystemColors.ButtonFace Or _
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False Then
                                    If Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" _
                                        Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Or Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TC" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    End If
                                End If
                        End Select

                        Select Case dc.Index
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Name
                                ' Set either caption alignment for summaries or RI icons
                                Select Case gPMFunctions.ToSafeString(m_oXA(dr.Index, RiskRIArrangement.RiskReinsuranceEnum.DBRIType))
                                    Case "BT", "TT", "FT", "XT", "AT", "UT", "OT", "NT", "NL"
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    Case "R", "T", "X"
                                    Case "F"
                                End Select
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007LowerLimit, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007UpperLimit
                                If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = "1" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                Else
                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode)) = "MA" And _
                                    (gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TC") Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                    ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "OT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "AT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "BT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "XT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "UT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TT" _
                                            Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                    Else
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    End If
                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "R" Or _
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or _
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or _
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                    End If
                                End If
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007SumInsured

                                'To replace the Fetchstyle property and condition.
                                If Not Me.ReadOnly_Renamed Then
                                    If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = "1" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                    Else
                                        If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "OT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "AT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "BT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "XT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "UT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TT" _
                                            Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                        Else
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        End If

                                    End If
                                End If
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007DefaultShare

                                If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = "1" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                Else

                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode)) = "MA" Then
                                        If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" _
                                        Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        Else
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        End If
                                    ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "OT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "AT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "BT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "XT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "UT" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TT" Or _
                                            gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "NL" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    Else
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    End If
                                End If

                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisShare

                                If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = "1" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                Else

                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or _
                                       gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or _
                                       gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TC" Or _
                                       gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "R" Then

                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or _
                                        gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Then

                                        If m_sTransactionType = "C_CO" And m_bCanEditClaimRI Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                        Else
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                        End If

                                    Else
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.ForeColor = grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.SelectionForeColor = Color.Transparent
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    End If
                                End If
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AgreementCode

                                If Not Me.ReadOnly_Renamed Then
                                    If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = "1" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                    Else

                                        If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or _
                                           gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or _
                                           gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or _
                                           gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "R" Or _
                                              gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TC" Or _
                                       gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Then
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                        ElseIf gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Then
                                            If (m_sTransactionType = "C_CR" Or m_sTransactionType = "C_CP") And (ThisPayment <> 0 Or ThisReserve <> 0) Then
                                                grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                            Else
                                                grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                                grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                            End If
                                        Else
                                            grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.Info
                                            grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                        End If
                                    End If
                                End If
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007ThisReserve
                                If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) Is DBNull.Value AndAlso m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsObligatory) = "1" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.SkyBlue
                                Else

                                    If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007AddMode)) = "MA" Or _
                                       gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Then
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = False
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    End If

                                End If

                                'Start E016
                            Case ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007IsReinsurerApproved
                                If gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "FX" Or _
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TX" Or _
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "T" Or _
                                    gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "R" Or _
                                   gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "F" Or gPMFunctions.ToSafeString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRI2007ArrangementLineEnum.DBCRI2007Type)) = "TFS" Then

                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

                                End If
                                'End E016
                        End Select
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

    Private Sub grdRI_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdRI.CellLeave
        grdRI_Enter()
    End Sub
End Class
