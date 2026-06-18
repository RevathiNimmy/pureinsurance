Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctClaimRIControl_NET.uctClaimRIControl")> _
Public Partial Class uctClaimRIControl
	Inherits System.Windows.Forms.UserControl
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "uctClaimRIControl"
	
    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
	Private m_bIsDirty As Boolean
	Private m_bReadOnly As Boolean
	
	' Stores the return value for the a function call.
	Private m_lReturn As Integer
	
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
	Private m_sTransactionType As String = ""
    Private m_lASumObligatoryTreaty As Integer
	Private m_lRetained As Integer

	' ***************************************************************** '
	'                        PUBLIC PROPERTIES
	' ***************************************************************** '
    <Browsable(True)> _
     Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
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
	
	<Browsable(False)> _
	Public ReadOnly Property IsDirty() As Boolean
		Get
			Return m_bIsDirty
		End Get
	End Property
	
	<Browsable(True)> _
	Public Property ReadOnly_Renamed() As Boolean
		Get
			Return m_bReadOnly
		End Get
		Set(ByVal Value As Boolean)
			m_bReadOnly = Value
			
			' Set base styles
            grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).ReadOnly = m_bReadOnly
            grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            grdRI.Columns(RiskRIArrangement.RiskReinsuranceEnum.DBRIAgreementCode).ReadOnly = m_bReadOnly

			If m_sTransactionType = "C_CV" Then
                grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve).ReadOnly = True
                grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
			End If
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property ShowPayments() As Boolean
		Get
			Return Not grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).ReadOnly
		End Get
		Set(ByVal Value As Boolean)
			grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).ReadOnly = Not Value
			grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).Visible = Value
		End Set
	End Property

	' ***************************************************************** '
	'                         PUBLIC METHODS
	' ***************************************************************** '
	
	Public Function AddFacultative(ByVal lPartyCnt As Integer, ByVal sDescription As String) As Integer
		
		Dim result As Integer = 0
		Dim lPriority As Integer
		
		Dim lReturn As Integer
		Const kMethodName As String = "AddFacultative"
		
        Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Set defaults
		lPriority = 65536
		
		' Check for duplicate and get priority
		For lCount As Integer = m_lASumTreaty + 1 To m_lASumFAC - 1
			' Validate
            If Not m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPartyCnt) Is DBNull.Value AndAlso m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPartyCnt) = lPartyCnt Then
                result = gPMConstants.PMEReturnCode.PMRecordInUse
	        Return result
            End If

            ' Check priority
            If Not m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority) Is DBNull.Value AndAlso m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority) > lPriority Then
                lPriority = m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority)
            End If
        Next

        ' Insert new row
        m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumFAC)
        'developer guide no. added to commit the change in datatable m_oXA
        m_oXA.AppendRows()

        ' Populate new line
        ' Grid fields
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = sDescription
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIDefaultShare) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIAgreementCode) = ""
        ' Supporting fields
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILineID) = 0
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "F"

        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRITreatyID) = Nothing
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPartyCnt) = lPartyCnt

        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIXolID) = Nothing
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority) = lPriority + 1
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILines) = 1
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILineLimit) = 0

        ' Increment total lines...
        m_lASumFAC += 1
        m_lASumTotal += 1
        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result	
    End Function

    Public Function AddTreaty(ByVal lTreatyID As Integer, ByVal sDescription As String, ByVal sAgreementCode As String, ByVal bIsRetained As Boolean) As Integer

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
            If Not m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRITreatyID) Is DBNull.Value AndAlso m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRITreatyID) = lTreatyID Then
                result = gPMConstants.PMEReturnCode.PMRecordInUse
        	Return result
            End If

            ' Check priority
            If Not m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority) Is DBNull.Value AndAlso m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority) > lPriority Then
                lPriority = m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority)
            End If
        Next

        ' Insert new row
        m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumTreaty)
        m_oXA.AppendRows()

        ' Populate new line
        ' Grid fields
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = sDescription
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIDefaultShare) = 0 'PN71192
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIAgreementCode) = sAgreementCode
        ' Supporting fields
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILineID) = 0
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = IIf(bIsRetained, "R", "T")
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRITreatyID) = lTreatyID
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPartyCnt) = Nothing
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIXolID) = Nothing
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPriority) = lPriority + 1
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILines) = 1
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILineLimit) = 0

        ' Increment total lines...
        m_lASumTreaty += 1
        m_lASumFAC += 1
        m_lASumTotal += 1
        m_lASumAlloc = IIf(m_lASumAlloc = 0, 0, m_lASumAlloc + 1)
        m_lASumUnalloc = IIf(m_lASumUnalloc = 0, 0, m_lASumUnalloc + 1)

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



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

    Public Function SetProperties(ByVal oRIArrangement As ClaimRIArrangement) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"
        Dim vRIValues(,) As Object '// Parallel PN74618
        Dim lCount As Integer '// Parallel PN74618

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear grid
        lReturn = Clear()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Clear", "Unable to clear previous reinsurance details")
        End If

        If TransactionType = "C_CR" Then ' Maintain claim
            grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).Visible = False
        End If
        ' Set appropriate read only state based on what we have to allocate
        ReadOnly_Renamed = (oRIArrangement.ThisReserve = 0 And oRIArrangement.ThisPayment = 0)

        ' Store the arrangement objects
        m_oRIArrangement = oRIArrangement

        ' Create the xarray and load from supplied raw array
        m_oXA = New XArrayHelper()
        If Information.IsArray(oRIArrangement.ReinsuranceLines) Then
            m_bRIEmpty = False
            '/* Parallel PN74618
            vRIValues = VB6.CopyArray(oRIArrangement.ReinsuranceLines)

            For lCount = vRIValues.GetLowerBound(1) To vRIValues.GetUpperBound(1)
                ' Recalculate premium share, if we can (Note:Only when if it is blank)
                If m_oRIArrangement.Reserve <> 0 And Convert.ToString(vRIValues(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare, lCount)) = "" Then
                        vRIValues(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare, lCount) = (gPMFunctions.ToSafeCurrency(vRIValues(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate, lCount)) + gPMFunctions.ToSafeCurrency(vRIValues(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve, lCount))) / m_oRIArrangement.Reserve
                End If
            Next

            bPMFunc.TransposeArray(vRIValues)
            m_oXA.LoadRows(vRIValues)
            bPMFunc.TransposeArray(vRIValues)

        Else
            ' Create band header row and set flag indicating we have done so
            m_bRIEmpty = True
            m_oXA.RedimXArray(New Integer() {0, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIMax}, New Integer() {0, 0})
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

        'Added the following code to remove the unwanted code
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
            If iPMFunc.IsIn(m_oXA(lSource, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType), "R", "T", "F", "X") Then
                ' Prep main array
                If lDest = 0 Then
                    ReDim vRI(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIMax, lDest)
                Else
                    ReDim Preserve vRI(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIMax, lDest)
                End If

                ' Copy all row data
                For lCol As Integer = 0 To ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIMax

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
        If Not m_bRIEmpty Then

            m_oXA.Rows.InsertAt(m_oXA.NewRow, 0)
            m_oXA.AppendRows()
        End If

        ' Populate row
        m_lASumBand = 0
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Band Total"
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) = m_oRIArrangement.SumInsured
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate) = m_oRIArrangement.ReserveToDate
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) = m_oRIArrangement.ThisReserve
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate) = m_oRIArrangement.PaymentToDate
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) = m_oRIArrangement.ThisPayment
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) = m_oRIArrangement.Balance
        m_oXA(m_lASumBand, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "BT"

        ' ***********************************************************
        ' We must now walk the treaty rows add add the treaty total
        m_lASumTreaty = 0

        m_lASumObligatoryTreaty = 0
        For lCount = 1 To m_oXA.Rows.Count - 1
            Select Case m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType)
                Case "R"
                    ' Retained store ro and keep going
                    If m_lRetained = 0 Then
                        m_lRetained = lCount
                    End If
                Case "T"
                    ' Retained or treaty keep going
                    If gPMFunctions.ToSafeString(m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIIsObligatory)) = "1" Then
                        m_lASumObligatoryTreaty = lCount
                    End If

                Case Else
                    m_lASumTreaty = lCount
                    Exit For
            End Select
        Next

        If m_lASumObligatoryTreaty > 0 Then

            m_oXA.Rows.InsertAt(m_oXA.NewRow, gPMConstants.kRINetLineObligatory)
            m_oXA.AppendRows()
            m_oXA(gPMConstants.kRINetLineObligatory, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Net Line"
            m_oXA(gPMConstants.kRINetLineObligatory, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "NL"
            If m_lASumTreaty > 0 Then
                m_lASumTreaty += 1
            End If
        End If

        ' If we reached the end of the array add total at end
        If m_lASumTreaty = 0 Then
            m_oXA.AppendRows()
            m_lASumTreaty = m_oXA.Rows.Count - 1
        Else
            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumTreaty)
            m_oXA.AppendRows()
        End If

        ' Populate treaty summary (no values, these are done elsewhere)
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Treaty Total"
        m_oXA(m_lASumTreaty, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "TT"

        ' ***********************************************************
        ' We must now walk the fac rows add add the treaty total
        m_lASumFAC = 0
        For lCount = m_lASumTreaty + 1 To m_oXA.Rows.Count - 1
            Select Case m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType)
                Case "R", "T"
                    gPMFunctions.RaiseError("Select Case m_oXA.Value(lCount, DBCRIType)", "Treaty lines found out of sequence")
                Case "F"
                    ' Fac lines continue
                Case Else
                    m_lASumFAC = lCount
                    Exit For
            End Select
        Next

        ' If we reached the end of the array add total at end
        ' Note: For FAC this is true unless xol is present
        If m_lASumFAC = 0 Then
            m_oXA.AppendRows()
            m_lASumFAC = m_oXA.Rows.Count - 1
        Else
            m_oXA.Rows.InsertAt(m_oXA.NewRow, m_lASumFAC)
            m_oXA.AppendRows()
        End If

        ' Populate fac summary (no values, these are done elsewhere)
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Facultative Total"
        m_oXA(m_lASumFAC, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "FT"

        ' ***********************************************************
        ' We must now walk the xol rows add add the totals..possibly lots of them TODO
        lCount = m_lASumFAC + 1
        Do While lCount <= m_oXA.Rows.Count - 1
            Select Case m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType)
                Case "R", "T", "F"
                    gPMFunctions.RaiseError("Select Case m_oXA.Value(lCount, DBCRIType)", "Treaty of FAC lines found out of sequence")
                Case "XT"
                    ' A new summary line skip it
                Case "X"
                    ' Xol line, check for new arrangement...on next line
                    bWriteSummary = (lCount = m_oXA.Rows.Count - 1)
                    If Not bWriteSummary Then
                        bWriteSummary = (m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIXolID) <> m_oXA(lCount + 1, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIXolID))
                    End If

                    ' Should we write out a summary?
                    If bWriteSummary Then
                        ' Insert summary for this xol layer
                        If lCount = m_oXA.Rows.Count - 1 Then
                            m_oXA.AppendRows()
                        Else
                            m_oXA.Rows.InsertAt(m_oXA.NewRow, lCount + 1)
                            m_oXA.AppendRows()
                        End If

                        ' Populate xol summary (no values, these are done elsewhere)
                        If gPMFunctions.ToSafeLong(m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRICatastropheID)) > 0 Then
                            m_oXA(lCount + 1, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRICatastrophe) & " XOL " & m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILayer) & " Total"
                        Else
                            m_oXA(lCount + 1, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Claim XOL " & m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRILayer) & " Total"
                        End If
                        m_oXA(lCount + 1, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "XT"

                        ' We want to store the highest xol summary row
                        lCount += 1
                        m_lASumXOL = lCount
                    End If

                Case Else
                    Exit Do
            End Select
            lCount += 1
        Loop

        ' ***********************************************************
        ' Add allocated total
        If Not m_bReadOnly Then
            m_oXA.AppendRows()

            ' Set allocated line
            m_lASumAlloc = m_oXA.Rows.Count - 1
            m_oXA(m_lASumAlloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Allocated"
            m_oXA(m_lASumAlloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "AT"
        End If

        ' ***********************************************************
        ' Do not add unallocated here it will be added, if
        ' necessary during totalling


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result
    End Function

    Private Function RollupXArray() As Integer

        Dim result As Integer = 0
        Dim oTtyTotal, oFacTotal, oXolTotal As ClaimTotalizer

        Dim lReturn As Integer
        Const kMethodName As String = "RollupXArray"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create totalizers
            oTtyTotal = New ClaimTotalizer()
            oFacTotal = New ClaimTotalizer()
            oXolTotal = New ClaimTotalizer()

            ' Walk the treaty rows and store summary
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
                For lCount As Integer = m_lASumBand + 1 To m_lASumTreaty - 1
                    ' Increase the totals
                    oTtyTotal.Add(m_oXA, lCount)
                Next
            End If
            oTtyTotal.Store(m_oXA, m_lASumTreaty)

            ' Walk the fac rows and store summary
            For lCount As Integer = m_lASumTreaty + 1 To m_lASumFAC - 1
                oFacTotal.Add(m_oXA, lCount)
            Next
            oFacTotal.Store(m_oXA, m_lASumFAC)

            ' Get grand total
            oTtyTotal.Combine(oFacTotal)

            ' Walk xol rows
            For lCount As Integer = m_lASumFAC + 1 To m_lASumXOL
                ' Check if this is an xol subtotal or another xol line
                If Not m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) Is DBNull.Value AndAlso m_oXA(lCount, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "XT" Then
                    ' XOL Sub total: store it, add it to grand total and clear
                    oXolTotal.Store(m_oXA, lCount)
                    oTtyTotal.Combine(oXolTotal)
                    oXolTotal = New ClaimTotalizer()
                Else
                    ' XOL line add to running total
                    oXolTotal.Add(m_oXA, lCount)
                End If
            Next

            ' Get allocation detail if we are editable
            If Not m_bReadOnly Then
                ' Store allocated total
                oTtyTotal.Store(m_oXA, m_lASumAlloc)
                ' Don't show allocated sum insured
                m_oXA(m_lASumAlloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRISumInsured) = ""

                ' Check unallocated and add/remove as necessary
                If Math.Abs(oTtyTotal.ThisReserve - m_oRIArrangement.ThisReserve) > 0.005 Or Math.Abs(oTtyTotal.ThisPayment - m_oRIArrangement.ThisPayment) > 0.005 Then
                    ' Display an allocated line
                    If m_lASumUnalloc = 0 Then
                        ' Append to grid
                        RemoveHandler grdRI.RowAdded, AddressOf grdRI_RowAdded
                        m_oXA.Rows.InsertAt(m_oXA.NewRow, m_oXA.Rows.Count)
                        m_lASumUnalloc = m_oXA.Rows.Count - 1
                        AddHandler grdRI.RowAdded, AddressOf grdRI_RowAdded
                    End If

                    ' Populate
                    m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName) = "Unallocated"
                    m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = 1 - oTtyTotal.SharePercent
                    m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve) = m_oRIArrangement.ThisReserve - oTtyTotal.ThisReserve
                    m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) = m_oRIArrangement.ThisPayment - oTtyTotal.ThisPayment
                    m_oXA(m_lASumUnalloc, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) = "UT"
                    If (m_oRIArrangement.ThisPayment - oTtyTotal.ThisPayment) <> 0 Then
                        grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).ReadOnly = False
                        grdRI.Columns(ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment).Visible = True
                    End If
                    grdRI_Enter()
                Else
                    ' If line exists remove it
                    If m_lASumUnalloc > 0 Then

                        'developer guide no. added the below line because it was missed by Artinsoft
                        'changed because DeleteRows is deleting more rows instead of the specified
                        Try
                            lReturn = gPMConstants.PMEReturnCode.PMTrue
                            RemoveHandler grdRI.RowDeleted, AddressOf grdRI_RowDeleted
                            m_oXA.Rows.RemoveAt(m_lASumUnalloc)
                            m_oXA.AcceptChanges()
                            AddHandler grdRI.RowDeleted, AddressOf grdRI_RowDeleted
                        Catch ex As Exception
                            lReturn = gPMConstants.PMEReturnCode.PMFalse
                        End Try

                        If lReturn <> 1 Then
                            gPMFunctions.RaiseError("m_oXA.DeleteRows(m_lASumUnalloc, 1)", "Unable to remove unallocated totals")
                        End If
                        m_lASumUnalloc = 0
                    End If
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        End Try
        Return result
    End Function

    Private Sub grdRI_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdRI.CellLeave
        grdRI_Enter()
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

            ' Check for appropriate row type
            If iPMFunc.IsIn(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType), "T", "R", "F", "X") Then
                ' Reserve will affect share percentage
                If ColIndex = ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve Then
                    ' Recalculate premium share, if we can
                    If m_oRIArrangement.Reserve <> 0 Then
                        m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare) = (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve))) / m_oRIArrangement.Reserve
                    End If
                End If

                ' Sum insured will affect share and premiums
                If iPMFunc.IsIn(CStr(ColIndex), ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment) Then
                    ' Recalculate balance
                    m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIBalance) = (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIReserveToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve))) - (gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIPaymentToDate)) + gPMFunctions.ToSafeCurrency(m_oXA(lRow, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment)))
                End If

                ' Future: If col is payment then check XOL
                If ColIndex = ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment Then
                    ' Currently XOL is only available in automated claim processing.
                    ' Manual claims processing will not trigger XOL, if it did this
                    ' is where you should check the XOL limits.
                End If
            End If

            ' Rollup and rebind
            RollupXArray()

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
            Case ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisPayment
                ' Validate range
                If Decimal.TryParse(NewValue, dTempValue) AndAlso gPMFunctions.ToSafeCurrency(NewValue, 0) = NewValue Then
                    ' Set changed state

                    m_bCellChanged = (NewValue <> OldValue)
                Else
                    sMessage = "This " & (IIf(ColIndex = ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve, "reserve", "payment")) & " must be a valid currency value"
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
        grdRI_Enter()
    End Sub

    Private Sub grdRI_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles grdRI.LostFocus
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

    Private Sub uctClaimRIControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            grdRI.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)

        Catch
        End Try
    End Sub
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        ' Save read only state
        PropBag.WriteProperty("ReadOnly", ReadOnly_Renamed, False)
    End Sub
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
                    If Not m_oXA(dr.Index, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType) Is DBNull.Value Then
                        Select Case m_oXA(dr.Index, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType)
                            Case "TT", "FT", "XT", "NL"
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.Info
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Bold Or FontStyle.Italic)
                                grdRI.Rows(dr.Index).ReadOnly = True
                                grdRI.Rows(dr.Index).DefaultCellStyle.ForeColor = SystemColors.InfoText
                            Case "BT", "AT", "UT"
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.Info
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Bold)
                                grdRI.Rows(dr.Index).ReadOnly = True
                                grdRI.Rows(dr.Index).DefaultCellStyle.ForeColor = SystemColors.InfoText
                            Case "OT", "NT"
                                grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = SystemColors.ButtonFace
                                grdRI.Rows(dr.Index).DefaultCellStyle.Font = New Font(grdRI.Font, FontStyle.Italic Or FontStyle.Bold)
                                grdRI.Rows(dr.Index).ReadOnly = True
                            Case "T", "F"
                                If ToSafeInteger(m_oXA(dr.Index, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIIsObligatory)) = 1 Then
                                    grdRI.Rows(dr.Index).DefaultCellStyle.BackColor = Color.SkyBlue
                                    grdRI.Rows(dr.Index).ReadOnly = True
                                End If
                        End Select
                    End If

                    Select Case dc.Index
                        Case ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIName
                            ' Set either caption alignment for summaries or RI icons
                            Select Case Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType))
                                Case "BT", "TT", "FT", "XT", "AT", "UT", "OT", "NT", "NL"
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                Case "R", "T", "X"
                                Case "F"
                            End Select
                        Case ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIAgreementCode
                            ' We can't edit retained agreement codes
                            'To replace the Fetchstyle property and condition.
                            If Not Me.ReadOnly_Renamed Then
                                Select Case Convert.ToString(m_oXA(dr.Index, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType))
                                    Case "R"
                                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                        grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                End Select
                            End If
                        Case ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisReserve
                            'Only apply style if the cell is readonly.
                            If Me.ReadOnly_Renamed Then
                                If m_sTransactionType = "C_CV" Then
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = SystemColors.ButtonFace
                                    grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly = True
                                    grdRI.Rows(dr.Index).Cells(dc.Index).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                End If
                            End If
                    End Select

                    If Not grdRI.Rows(dr.Index).Cells(dc.Index).ReadOnly Then
                        grdRI.Rows(dr.Index).Cells(dc.Index).Style.BackColor = Color.White
                    End If

                Catch ex As Exception
                    ' DO Not Call any functions before here or the error will be lost
                    iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="grdRI_Enter", r_lFunctionReturn:=m_lReturn, excep:=ex)
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
        If Not e.Value Is DBNull.Value Then
            Dim dTempVal As Double = 0D
            If e.ColumnIndex = ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIThisShare _
             Or e.ColumnIndex = ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIDefaultShare Then
                If (Double.TryParse(e.Value, dTempVal)) Then
                    e.Value = dTempVal.ToString("P2")
                End If
            ElseIf e.CellStyle.Format = "0.00" Then
                e.Value = Math.Round(gPMFunctions.ToSafeDouble(e.Value), 2)
            ElseIf e.CellStyle.Format = "N2" Then
                e.Value = Math.Round(gPMFunctions.ToSafeDouble(e.Value), 2)
            Else
                e.Value = e.Value
            End If
        End If
    End Sub

    Private Sub grdRI_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles grdRI.CellPainting
        If e.ColumnIndex = 0 And e.RowIndex > -1 Then
            If Not IsDBNull(m_oXA(e.RowIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType)) Then
                Select Case m_oXA(e.RowIndex, ClaimRIArrangement.ClaimRIArrangementLineEnum.DBCRIType)
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
        End If
    End Sub
End Class
