Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'Developer Guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("CLMRecoveryTaxes_NET.CLMRecoveryTaxes")> _
Public NotInheritable Class CLMRecoveryTaxes 
	Implements IEnumerable
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "CLMRecoveryTaxs"
	
	
	' Collection
	Private m_cRecoveryTaxes As New Collection
	
	
	' Add a new tax value
	Public Function Add(ByVal TaxTypeCode As String, ByVal TaxAmount As Decimal) As Integer
		
		
		TaxAmount = CDec(CStr(TaxAmount).Trim())
		

		Try 
			' Try to get existing tax
			Dim oTax As CLMRecoveryTax = m_cRecoveryTaxes(TaxTypeCode)
			Try 
				
				If oTax Is Nothing Then
					' This tax type doesn't exist create it new
					oTax = New CLMRecoveryTax()
					oTax.TaxTypeCode = TaxTypeCode
					oTax.TaxTotal = TaxAmount
					
					' Add to collection
					m_cRecoveryTaxes.Add(oTax, oTax.TaxTypeCode)
				Else
					' This tax type already exists, add to total
					oTax.TaxTotal += TaxAmount
				End If
				
				Exit Function
			
			Catch excep As System.Exception
				
				
				
				' Log Error Message
				bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CLMRecovery to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				' Error.
				Return gPMConstants.PMEReturnCode.PMError
				
			End Try
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Count
	'
	' Description: Returns the number of CLMRecoverys in the collection.
	' ***************************************************************** '
	Public Function Count() As Integer
		Return m_cRecoveryTaxes.Count
	End Function
	
	' ***************************************************************** '
	' Name: Remove
	'
	' Description: Delete a CLMRecovery from the Collection.
	' ***************************************************************** '
	Public Sub Remove(ByVal vKey As String)
		m_cRecoveryTaxes.Remove(vKey)
	End Sub
	
	' ***************************************************************** '
	' Name: Item
	'
	' Description: Returns the selected CLMRecovery from the Collection.
	' ***************************************************************** '
	Public Function Item(ByRef vKey As String) As bCLMRecovery.CLMRecoveryTax
		Return m_cRecoveryTaxes(vKey)
	End Function
	
	' ***************************************************************** '
	' Name: Clear
	'
	' Description: Clear the CLMRecovery Collection.
	' ***************************************************************** '
	Public Sub Clear()
		' Set CLMRecovery Collection to Nothing and recreate
		m_cRecoveryTaxes = Nothing
		m_cRecoveryTaxes = New Collection()
	End Sub
	
	
	' ***************************************************************** '
	' Name: NewEnum (Posh Method :-)
	'
	' Description: Allow this collection to be enumerated with
	'   For Each...Next
	'
	' Notes:
	'   The return property from this call must be IUnknown!!
	'   The _NewEnum property of the collection is hidden
	'   For this to function the Procedure ID must be set to -4
	' ***************************************************************** '
	
	Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
		' Pass through to collection class
		Return m_cRecoveryTaxes.GetEnumerator
	End Function
	
	
	
	Public Sub New()
		MyBase.New()
	End Sub
End Class
