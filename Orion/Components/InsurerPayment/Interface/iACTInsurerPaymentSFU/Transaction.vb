Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Transaction_NET.Transaction")> _
Public NotInheritable Class Transaction 
	Implements IEnumerable
	
	' Collection for all transaction entries
    Private m_cEntries As New Collection
    Private m_cInstalmentEntries As Collection

	Private Const ACClass As String = "Transaction"

	' *******************************************************************************
	' PUBLIC FIELDS (VB WILL AUTOMATICALLY DERIVE THE PUBLIC PROPERTIES)
	' *******************************************************************************
    Private m_lDetailID As Integer

    Public DocumentID As Integer

    ' Document and policy references
    Public DocumentRef As String = ""
    Public InsuranceRef As String = ""
    Public IsConsolidateBinder As Boolean
    Public Transtype As Integer
    'PN 33593 (RC)
    Public AlternateRef As String = ""
    Public EffectiveDate As Date
    Public DueDate As Date

    ' Currency Details
    Public CurrencyID As Integer
    Public FullyPaidAmount As Decimal
    Public ClientOSAmount As Decimal
    Public CurrencyRate As Double

    ' Account Currency Details
    Public AccountCurrencyID As Integer
    Public FullyPaidAccountAmount As Decimal
    Public ClientOSAccountAmount As Decimal
    Public AccountCurrencyRate As Double
    Public ClientAccountCurrencyID As Integer

    ' Policy holder details
    Public HolderCode As String = ""
    Public HolderName As String = ""
    ' Fields duplicated from transaction entries
    Public CompanyID As Integer
    Public AccountingDate As Date
    Public Period As String = ""
    Public MediaType As String
    Public Month As Integer
    Public AllocationPeriod As String = ""
    Private m_cTransAmount As Decimal 'store the transaction's amount
    Public IsDebitOrderTransDetail As Boolean
    Public WriteOnly Property TransAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cTransAmount = m_cTransAmount + Value
        End Set
    End Property

	Public Property DetailID() As Integer
		Get
			Return m_lDetailID
		End Get
		Set(ByVal Value As Integer)
			m_lDetailID = Value
		End Set
	End Property

	' *******************************************************************************
	' PUBLIC FIELDS (VB WILL AUTOMATICALLY DERIVE THE PUBLIC PROPERTIES)
	' *******************************************************************************
	
	
	' *******************************************************************************
	' PUBLIC PROPERTIES
	' *******************************************************************************
	Public ReadOnly Property Count() As Integer
		Get
			Return m_cEntries.Count
		End Get
    End Property
    Public ReadOnly Property CountInstalment() As Integer
        Get
            Return m_cInstalmentEntries.Count
        End Get
    End Property

    ' Determine how marked out transaction is
    ' Determine how marked out transaction is
    Public ReadOnly Property IsMarkedInstalment() As TransactionInst.MarkedStatusEnumInstalment
        Get
            Dim oInstalmentEntry As TransactionInst
            Dim lStatus As TransactionInst.MarkedStatusEnumInstalment

            ' Set default state to first entry (doesn't really matter which)
            lStatus = m_cInstalmentEntries.Item(1).IsMarked

            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                ' If the status is different we are part marked, else continue
                If lStatus <> oInstalmentEntry.IsMarked Then
                    lStatus = TransactionEntry.MarkedStatusEnum.acmsePartMarked
                    Exit For
                End If
            Next

            ' Return final value
            IsMarkedInstalment = lStatus
        End Get
    End Property

	Public ReadOnly Property IsMarked() As TransactionEntry.MarkedStatusEnum
		Get						
            ' Set default state to first entry (doesn't really matter which)

            Dim lStatus As TransactionEntry.MarkedStatusEnum = m_cEntries(1).IsMarked

            ' Sum up all marked totals
            For Each oEntry As TransactionEntry In m_cEntries
                ' If the status is different we are part marked, else continue
                If lStatus <> oEntry.IsMarked Then
                    lStatus = TransactionEntry.MarkedStatusEnum.acmsePartMarked
                    Exit For
                End If
            Next oEntry

            ' Return final value
            Return lStatus
        End Get
	End Property
	
	' Return the item requested by key or index
	Default Public ReadOnly Property Item(ByVal Index As Integer) As TransactionEntry
		Get
			Return m_cEntries(Index)
		End Get
	End Property
	
	' A listview compatible string key derived from the DocumentID
	Public ReadOnly Property Key() As String
		Get
			'Start (Arul Stephen)-(Tech Spec-QBENZCR008-Insurer Payments-Display journal on multiple lines)-(4.2.2.2)
			If DocumentRef.Substring(0, 2) <> ACInsurerPaymentJournal Then
				Return "t_" & DocumentID
			Else
				Return "t_" & DocumentID & "_d_" & CStr(DetailID)
			End If
			'End (Arul Stephen)-(Tech Spec-QBENZCR008-Insurer Payments-Display journal on multiple lines)-(4.2.2.2)
		End Get
	End Property
	
	
	Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        ' Allow enumeration with the For...Each syntax
        If Transtype = 0 Then
            Return m_cEntries.GetEnumerator
        Else
            Return m_cInstalmentEntries.GetEnumerator
        End If
	End Function
	
	Public ReadOnly Property TotalCurrency() As Decimal
        Get

            Dim oEntry As TransactionEntry
            Dim oTotal As Decimal
            For Each oEntry In m_cEntries
                oTotal = oTotal + oEntry.CurrencyAmount
            Next oEntry
            oTotal = oTotal + m_cTransAmount
            TotalCurrency = oTotal + FullyPaidAmount
        End Get
	End Property
	
    Public ReadOnly Property TotalMarked() As Decimal
        Get

            Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
            ' Sum up all marked totals
            For Each oEntry As TransactionEntry In m_cEntries
                oTotal += oEntry.MarkedAmount
            Next oEntry
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.MarkedAmount
            Next
            ' Return
            Return oTotal
        End Get
    End Property
	
	Public ReadOnly Property TotalOutstanding() As Decimal
		Get
			
			Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
			' Sum up all marked totals
			For	Each oEntry As TransactionEntry In m_cEntries
				oTotal += oEntry.OutstandingAmount
			Next oEntry
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.OutstandingAmount
            Next
			' Return
			Return oTotal
		End Get
	End Property
	
    Public ReadOnly Property TotalPaid() As Decimal
        Get

            Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
            ' Sum up all marked totals
            For Each oEntry As TransactionEntry In m_cEntries
                oTotal += oEntry.PaidAmount
            Next oEntry
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.PaidAmount
            Next
            ' Return
            Return oTotal + FullyPaidAmount
        End Get
    End Property
	
	Public ReadOnly Property TotalAccountCurrency() As Decimal
		Get
			
			Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
			' Sum up all marked totals
			For	Each oEntry As TransactionEntry In m_cEntries
				oTotal += oEntry.CurrencyAccountAmount
			Next oEntry
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.CurrencyAccountAmount
            Next
			' Return
			Return oTotal + FullyPaidAccountAmount
		End Get
	End Property
	
	Public ReadOnly Property TotalAccountMarked() As Decimal
		Get
			
			Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
			' Sum up all marked totals
			For	Each oEntry As TransactionEntry In m_cEntries
				oTotal += oEntry.MarkedAccountAmount
			Next oEntry
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.MarkedAccountAmount
            Next
			' Return
			Return oTotal
		End Get
	End Property
    ' Return the item requested by key or index
    Public ReadOnly Property ItemInstalment(ByVal Index As Object) As TransactionInst
        Get
            Return m_cInstalmentEntries.Item(Index)
        End Get
    End Property
    Public ReadOnly Property NewEnumInst() As Object
        Get
            ' Allow enumeration with the For...Each syntax
            NewEnumInst = m_cInstalmentEntries.GetEnumerator
        End Get
    End Property

    

    Public ReadOnly Property TotalAccountOutstanding() As Decimal
        Get

            Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
            ' Sum up all marked totals
            For Each oEntry As TransactionEntry In m_cEntries
                oTotal += oEntry.OutstandingAccountAmount
            Next oEntry
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.OutstandingAccountAmount
            Next
            ' Return
            Return oTotal
        End Get
    End Property

    Public ReadOnly Property TotalAccountPaid() As Decimal
        Get

            Dim oTotal As Decimal
            Dim oInstalmentEntry As TransactionInst
           For Each oEntry As TransactionEntry In m_cEntries
                oTotal += oEntry.PaidAccountAmount
            Next oEntry
            '    Else
            ' Sum up all marked totals
            For Each oInstalmentEntry In m_cInstalmentEntries
                oTotal = oTotal + oInstalmentEntry.PaidAccountAmount
            Next

            '    End If
            ' Return
            Return oTotal + FullyPaidAccountAmount
        End Get
    End Property
    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************


    ' *******************************************************************************
    ' PUBLIC METHODS
    ' *******************************************************************************
    Public Sub Add(ByRef rEntry As TransactionEntry)
        ' Add the detail to the collection
        m_cEntries.Add(rEntry, rEntry.Key)
    End Sub
    Public Sub AddInstalment(ByRef rInstalmentEntry As TransactionInst)
        ' Add the detail to the collection
        Call m_cInstalmentEntries.Add(rInstalmentEntry, rInstalmentEntry.Key)
    End Sub
    Public Sub Clear()
        m_cEntries = New Collection()
    End Sub
  
    ' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc)
    Public Function GetMinTransactionId(ByRef oTransaction As Transaction, ByRef iTransID As Integer, ByRef iRow As Integer, Optional ByRef r_bInstalment As Boolean = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMinTransactionId"
        Dim bInstalment As Boolean
        Dim iCount As Integer
        Dim iTemp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IND" And _
                     Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IED" And _
                     Mid(Trim$(oTransaction.DocumentRef), 1, 3) <> "IRD" Then
                iCount = oTransaction.Count
                bInstalment = False
            Else
                iCount = oTransaction.CountInstalment
                bInstalment = True
            End If
            If iCount = 1 Then
                If Not bInstalment Then
                    iTransID = oTransaction.Item(1).DetailID
                    iRow = 1
                    Return result
                Else
                    iTransID = oTransaction.ItemInstalment(1).DetailID
                    iRow = 1
                    r_bInstalment = True
                    Return result
                End If
            ElseIf (iCount > 0) Then
                If Not bInstalment Then
                    iTransID = oTransaction.Item(1).DetailID
                    iRow = 1
                    For iTemp = 1 To iCount - 1
                        If iTransID > oTransaction.Item(iTemp + 1).DetailID Then
                            iTransID = oTransaction.Item(iTemp + 1).DetailID
                            iRow = iTemp + 1
                        End If
                    Next
                Else
                    iTransID = oTransaction.ItemInstalment(1).DetailID
                    iRow = 1
                    For iTemp = 1 To iCount - 1
                        If (iTransID > oTransaction.ItemInstalment(iTemp + 1).DetailID) Then
                            iTransID = oTransaction.ItemInstalment(iTemp + 1).DetailID
                            iRow = iTemp + 1
                        End If
                    Next
                    r_bInstalment = True
                End If
            Else

                iTransID = 0
                iRow = 0
                gPMFunctions.RaiseError("GetMinTransactionId", "Failed to find Min Transaction Key")
            End If

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result

        End Try

    End Function

    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc)


    ' *******************************************************************************
    ' PUBLIC METHODS
    ' *******************************************************************************


    ' *******************************************************************************
    ' CLASS EVENTS
    ' *******************************************************************************

    Protected Overrides Sub Finalize()
        m_cEntries = Nothing
        m_cInstalmentEntries = Nothing
    End Sub
    ' *******************************************************************************
    ' CLASS EVENTS
    ' *******************************************************************************

    Public Sub New()
        m_cEntries = New Collection
        m_cInstalmentEntries = New Collection
    End Sub
End Class
