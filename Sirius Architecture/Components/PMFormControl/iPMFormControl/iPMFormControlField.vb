Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("FormField_NET.FormField")> _
Public NotInheritable Class FormField 
	' ***************************************************************** '
	' Class Name: Field
	'
	' Date: 29-07-1997
	'
	' Description: Class to hold screen controls for validation
	'
	' Edit History:
	' RFC151098 - Use FormatField function to Format percentage
	' VB 14/02/2005 - PN18426 : PMAccountLookup Constant (for the AccountLookup UserControl)
	'                           And 'Trim()' function added in 'IsBlank' Get Property
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "FormField"
	
	Private m_ctlFormControl As Control
	Private m_lFieldFormat As gPMConstants.PMEFormatStyle
	Private m_lFieldType As gPMConstants.PMEDataType
	Private m_lGridColumn As Integer
	Private m_lGridRow As Integer
	Private m_bIsMandatory As Boolean
	Private m_bIsValid As Boolean
	Private m_bIsVisible As Boolean
	Private m_lControlType As gPMConstants.PMEControlType
	Private m_vBookMark As Object
	Private m_lCurrencyID As Integer
	Private m_lDecimalPlaces As Integer
	Private m_sFormatString As String = ""
	Private m_sFormatYear As String = ""
	Private m_iFormatLong As Integer
	Private m_sCaption As String = ""
	
	Public Property FormControl() As Object
		Get
			Return m_ctlFormControl
		End Get
		Set(ByVal Value As Object)
			m_ctlFormControl = Value
			m_lControlType = CType(GetControlType(m_ctlFormControl), gPMConstants.PMEControlType)
		End Set
	End Property
	
	Public Property FieldFormat() As Integer
		Get
			Return m_lFieldFormat
		End Get
		Set(ByVal Value As Integer)
			m_lFieldFormat = Value
		End Set
	End Property
	
	Public Property FieldType() As Integer
		Get
			Return m_lFieldType
		End Get
		Set(ByVal Value As Integer)
			m_lFieldType = Value
		End Set
	End Property
	
	Public Property IsMandatory() As Boolean
		Get
			Return m_bIsMandatory
		End Get
		Set(ByVal Value As Boolean)
			m_bIsMandatory = Value
		End Set
	End Property
	
	Public Property IsVisible() As Boolean
		Get
			Return m_bIsVisible
		End Get
		Set(ByVal Value As Boolean)
			m_bIsVisible = Value
		End Set
	End Property
	
	Public Property GridColumn() As Integer
		Get
			Return m_lGridColumn
		End Get
		Set(ByVal Value As Integer)
			m_lGridColumn = Value
		End Set
	End Property
	
	Public Property GridRow() As Integer
		Get
			Return m_lGridRow
		End Get
		Set(ByVal Value As Integer)
			m_lGridRow = Value
		End Set
	End Property
	
	Public ReadOnly Property ControlType() As Integer
		Get
			Return m_lControlType
		End Get
	End Property
	
	Public Property Text() As String
		Get
			
			Select Case m_lControlType
				Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMAccountLookup

					Return m_ctlFormControl.Text
					
				Case gPMConstants.PMEControlType.PMGrid
					'JK280898 Code Added to get Text from Row and Column of grid

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Col = m_lGridColumn
                    ReflectionHelper.SetMember(m_ctlFormControl, "Col", m_lGridColumn)

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Row = m_lGridRow
                    ReflectionHelper.SetMember(m_ctlFormControl, "Row", m_lGridRow)

					Return m_ctlFormControl.Text
					'JK280898 Following Commented out
					'     Text = m_ctlFormControl.Columns(m_lGridColumn).Value
					
				Case gPMConstants.PMEControlType.PMSpread

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Col = m_lGridColumn
                    ReflectionHelper.SetMember(m_ctlFormControl, "Col", m_lGridColumn)

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Row = m_lGridRow
                    ReflectionHelper.SetMember(m_ctlFormControl, "Row", m_lGridRow)

					Return m_ctlFormControl.Text
					
				Case gPMConstants.PMEControlType.PMCombo, gPMConstants.PMEControlType.PMListBox

                    'NIIT - Replaced with the Migrated code 1144 

                    'Return m_ctlFormControl.List(m_ctlFormControl.ListIndex)
                    Return ReflectionHelper.Invoke(m_ctlFormControl, "List", New Object() {ReflectionHelper.GetMember(m_ctlFormControl, "ListIndex")})

				Case Else
					Return ""
			End Select
			
		End Get
		Set(ByVal Value As String)
			
			Select Case m_lControlType
				Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMAccountLookup
                    m_ctlFormControl.Text = Value

                Case gPMConstants.PMEControlType.PMGrid
					'JK280898 Code Added to Set Text for Row and Column of grid

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Col = m_lGridColumn
                    ReflectionHelper.SetMember(m_ctlFormControl, "Col", m_lGridColumn)

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Row = m_lGridRow
                    ReflectionHelper.SetMember(m_ctlFormControl, "Row", m_lGridRow)
                    m_ctlFormControl.Text = Value
					'JK280898 Following Commented Out
					'      m_ctlFormControl.Columns(m_lGridColumn).Value = sText
					
				Case gPMConstants.PMEControlType.PMSpread

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Col = m_lGridColumn
                    ReflectionHelper.SetMember(m_ctlFormControl, "Col", m_lGridColumn)

                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.Row = m_lGridRow
                    ReflectionHelper.SetMember(m_ctlFormControl, "Row", m_lGridRow)
                    m_ctlFormControl.Text = Value
					
				Case gPMConstants.PMEControlType.PMCombo, gPMConstants.PMEControlType.PMListBox


                    'NIIT - Replaced with the Migrated code 1144 
                    'm_ctlFormControl.List(m_ctlFormControl.ListIndex) = Value
                    ReflectionHelper.SetMember(m_ctlFormControl, "List", New Object() {ReflectionHelper.GetMember(m_ctlFormControl, "ListIndex")}, Value)

				Case gPMConstants.PMEControlType.PMCheckBox

                    'NIIT - Replaced with the Migrated code 1144 
                    'Text = m_ctlFormControl.Value
                    Text = ReflectionHelper.GetMember(m_ctlFormControl, "Value")

				Case Else
                    Text = ""
			End Select
			
		End Set
	End Property
	
	Public Property BookMark() As Object
		Get
			Return m_vBookMark
		End Get
		Set(ByVal Value As Object)


			m_vBookMark = Value
		End Set
	End Property
	
	Public Property DecimalPlaces() As Integer
		Get
			Return m_lDecimalPlaces
		End Get
		Set(ByVal Value As Integer)
			m_lDecimalPlaces = Value
			m_sFormatString = "###########0"
			If m_lDecimalPlaces > 0 Then
				m_sFormatString = m_sFormatString & "." & New String("0", m_lDecimalPlaces)
			End If
		End Set
	End Property
	
	Public Property CurrencyID() As Integer
		Get
			Return m_lCurrencyID
		End Get
		Set(ByVal Value As Integer)
			m_lCurrencyID = Value
		End Set
	End Property
	
	Public ReadOnly Property FormattedText() As Object
		Get
			
			Select Case m_lFieldFormat
				Case gPMConstants.PMEFormatStyle.PMFormatDouble, gPMConstants.PMEFormatStyle.PMFormatDecimal
					Return StringsHelper.Format(Text, m_sFormatString)
					
				Case gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateLong
					If FieldValue = DateTime.FromOADate(ACDateLowValue) Or FieldValue = #12/30/1899# Then
						Return ""
					Else
						Return gPMFunctions.FormatField(iFormatType:=m_lFieldFormat, vFieldValue:=Text)
					End If
					
					'JK280898 Format Year Only
				Case gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
					Dim dbNumericTemp As Double
					If Information.IsDate(Text) Then
						'--> to return say 1999 after 1/2/99 is entered
						Return gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateYearOnly, Text)
					ElseIf (Double.TryParse(Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then 
						' add 1/ to year entered and then format to return
						' just the year.
						Return gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateYearOnly, "1/" & Text)
					Else
						Return ""
					End If
					
					' RFC 240898 - Format Percents with decimal places
					' Peter Finney 13/06/2003 - Format percents with specific decimal places
				Case gPMConstants.PMEFormatStyle.PMFormatPercent
					Dim dbNumericTemp2 As Double
					Select Case True
						Case Not Double.TryParse(Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) 'JK250898
							Return "" 'To format only when numeric value is entered
						Case Text.Trim().Length = 0
							Return ""
						Case m_lDecimalPlaces = 0
							' RFC 151098 - Use FormatField function to Format percentage
							Return gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Text)
						Case Else
							' Peter Finney 13/06/2003 - If set use specific decimal places
							Return gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Text, m_lDecimalPlaces)
					End Select
					
				Case Else
					Return gPMFunctions.FormatField(iFormatType:=m_lFieldFormat, vFieldValue:=Text)
			End Select
			
		End Get
	End Property
	
	Public ReadOnly Property FieldValue() As Object
		Get
			
			Select Case m_lControlType
				Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMGrid, gPMConstants.PMEControlType.PMSpread, gPMConstants.PMEControlType.PMAccountLookup
					Return gPMFunctions.UnFormatField(m_lFieldFormat, m_lFieldType, Text)
					
				Case gPMConstants.PMEControlType.PMCombo, gPMConstants.PMEControlType.PMListBox

                    If TypeName(FormControl) = "ComboBox" Then
                        If FormControl.selectedindex < 0 Then
                            Return -1
                        Else
                            'NIIT - Replaced with the Migrated code 1144 
                            'Return m_ctlFormControl.ItemData(m_ctlFormControl.ListIndex)
                            'Return ReflectionHelper.Invoke(m_ctlFormControl, "ItemData", New Object() {ReflectionHelper.GetMember(m_ctlFormControl, "selectedindex")})
                            Return CType(m_ctlFormControl, System.Windows.Forms.ComboBox).Items(CType(m_ctlFormControl, System.Windows.Forms.ComboBox).SelectedIndex).itemdata
                        End If
                    Else
                        If FormControl.ListIndex < 0 Then
                            Return -1
                        Else
                            'NIIT - Replaced with the Migrated code 1144 
                            'Return m_ctlFormControl.ItemData(m_ctlFormControl.ListIndex)
                            Return ReflectionHelper.Invoke(m_ctlFormControl, "ItemData", New Object() {ReflectionHelper.GetMember(m_ctlFormControl, "ListIndex")})
                        End If
                    End If
                Case gPMConstants.PMEControlType.PMCheckBox


                    'NIIT - Replaced with the Migrated code 1144 
                    If ReflectionHelper.GetMember(m_ctlFormControl, "CheckState") = CheckState.Checked Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case gPMConstants.PMEControlType.PMOptionButton

                    'NIIT - Replaced with the Migrated code 1144 
                    'If m_ctlFormControl.Value Then
                    If ReflectionHelper.GetMember(m_ctlFormControl, "Value") Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    Return gPMFunctions.UnFormatField(m_lFieldFormat, m_lFieldType, Text)
            End Select
			
		End Get
	End Property
	
	Public ReadOnly Property IsBlank() As Boolean
		Get
			'APS    29/01/03   Amended to check against 'Blank'
			Dim result As Boolean = False
			
			Select Case m_lControlType
				Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMGrid, gPMConstants.PMEControlType.PMAccountLookup

					If Text.Trim() = "" Or Convert.IsDBNull(Text) Or IsNothing(Text) Then
						result = True
					End If
					
				Case gPMConstants.PMEControlType.PMCombo
					'If a product screen control check for the text (None)
                    If FormControl.Name.Contains("cboGISLookup") Then


                        'developer guide no. 28
                        If ReflectionHelper.GetMember(FormControl, "ListIndex") < 0 Or FormControl.ItemCaption = "(None)" Then
                            result = True
                        End If
                    Else

                        'developer guide no. 28
                        If FormControl.GetType().FullName <> "PMLookupControl.cboPMLookup" Then
                            If ReflectionHelper.GetMember(FormControl, "SelectedIndex") < 0 Then
                                result = True
                            End If
                        Else
                            If ReflectionHelper.GetMember(FormControl, "ListIndex") < 0 Then
                                result = True
                            End If
                        End If
                    End If
					
				Case gPMConstants.PMEControlType.PMSpread

					If (Text = "") Or (Convert.IsDBNull(Text) Or IsNothing(Text)) Then
						result = True
					End If
					
				Case Else
					' Nothing
			End Select
			
			Return result
		End Get
	End Property
	
	Public Property Caption() As String
		Get
			m_sCaption = GetCaptionForControl(Me)
			Return m_sCaption
		End Get
		Set(ByVal Value As String)
			m_sCaption = Value
		End Set
	End Property
	
	Public Property IsValid() As Boolean
		Get
			Return m_bIsValid
		End Get
		Set(ByVal Value As Boolean)
			m_bIsValid = Value
		End Set
	End Property
End Class
