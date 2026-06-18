Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide 129
Imports SharedFiles
Imports Artinsoft.VB6.Utils

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 06 May 1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iACTFindAccount"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"
    ' Instance of the form

    ' Username.
    Public g_sUsername As New FixedLengthString(12)
    Public g_iUserID As Integer
    Public g_sPassword As New FixedLengthString(30)
    Public g_sCallingAppName As String = ""
    Public g_iCompanyID As Integer
    Public g_iCurrencyID As Integer
    Public g_iLogLevel As Integer

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Constants for SubItems of ListView (ColumnHeader indicies are these + 1)
    Public Const ACListIShortCode As Integer = 0
    Public Const ACListIAccountName As Integer = 1
    Public Const ACListIForename As Integer = 2
    Public Const ACListIAddress As Integer = 3
    Public Const ACListIAccountStatus As Integer = 4
    Public Const ACListILedger As Integer = 5
    Public Const ACListIAccountType As Integer = 6
    Public Const ACListIBalance As Integer = 7
    Public Const ACListISourceID As Integer = 8

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle As Integer = 101
    Public Const ACTabTitleCount As Integer = 2

    ' ColumnHeaders
    Public Const ACListTitle As Integer = 110
    Public Const ACListTitleCount As Integer = 5

    Public Const ACColShortCode As Integer = 110
    Public Const ACColName As Integer = 111
    Public Const ACColLedger As Integer = 112
    Public Const ACColStatus As Integer = 133
    Public Const ACColAccountType As Integer = 113
    'eck050302
    Public Const ACColBalance As Integer = 134
    'DJM 21/10/2002 : Source ID added to the list view.
    Public Const ACColSourceID As Integer = 135

    ' Labels
    Public Const ACShortCode As Integer = 120
    Public Const ACFullKey As Integer = 121
    Public Const ACAccountName As Integer = 122
    Public Const ACLedgerID As Integer = 123
    Public Const ACAccountType As Integer = 124
    Public Const ACInsuranceRef As Integer = 125
    Public Const ACOperatorID As Integer = 126
    Public Const ACPurchaseOrderNo As Integer = 127
    Public Const ACPurchaseInvoiceNo As Integer = 128
    'cmg/pb Control removed bug 2253 Public Const ACDepartment = 129
    Public Const ACSpare As Integer = 130
    Public Const ACShowDeleted As Integer = 132
    Public Const ACShowBalance As Integer = 136

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307
    Public Const ACAllTypes As Integer = 308

    Public Const ACFirstName As Integer = 309
    Public Const ACAddress As Integer = 310

    ' Menus


    ' Constants for the lookup table array indexes.
    Public Const ACLLedgerType As Integer = 0
    Public Const ACLAccountType As Integer = 1
    Public Const ACLMax As Integer = 1

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 3

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'eck170500
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oPMUser As bPMUser.Business

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object
    Private m_vLedgers As Object

    ' Constants for the m_vLedgers data array indexes.
    Private Const ACILedgerID As Integer = 0
    Private Const ACILedgerName As Integer = 1
    Private Const ACILedgerTypeID As Integer = 2
    Private Const ACILedgerShortName As Integer = 3

    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

    Public Const ScreenHelpID As Integer = 15000

    Sub Main_Renamed()

    End Sub

    ' Global application functions

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.


            ReDim m_vLookupValues(3, ACLMax)

            ' Setup Lookup Table Names

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLLedgerType) = gACTLibrary.ACTLookupLedgerType

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLAccountType) = gACTLibrary.ACTLookupAccountType

            ' Do not supply a key
            For i As Integer = 0 To ACLMax

                m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = ""
            Next i

            ' Get all of the lookup values with the correct
            ' effective date.

            m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupRow
    '
    ' Description: Converts a lookup table name to its matching row index
    '              in the table of lookup values.
    '              May be used to indirect GetLookupDetails, GetLookupDesc.
    '              Returns -1 if no match found
    '
    ' ***************************************************************** '
    Public Function GetLookupRow(ByRef sLookupTable As String) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        Try

            result = -1

            bFoundMatch = False


            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.

                If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            If bFoundMatch Then
                result = lRow
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to match lookup table", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'developer guide no. 69
    Public Function GetLookupDetails(ByRef lLookupRow As Integer, ByRef ctlLookup As ComboBox, Optional ByVal vAllTypes As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.


            Dim sLookupDesc As String = ""
            If Not Information.IsNothing(vAllTypes) Then

                If CBool(vAllTypes) Then
                    ' First entry is all types (don't care)

                    sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    'developer guide no. 28
                    ctlLookup.Items.Add(New VB6.ListBoxItem(sLookupDesc, -1))
                End If
            End If
            'developer guide no. 28
            Dim newIndex As Integer = -1



            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Add the details to the control.

                'developer guide no. 28
                newIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(CStr(m_vLookupDetails(ACILedgerName, lCntr)), CInt(m_vLookupDetails(ACILedgerID, lCntr))))
                ' Check if this is the selected index.


                If m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow).Equals(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) Then

                    ctlLookup.SelectedIndex = newIndex
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.

            If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow)) = "" Then
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDesc
    '
    ' Description: Gets a description string for a given lookup set
    '              and lookup id.
    '
    ' ***************************************************************** '
    Public Function GetLookupDesc(ByRef lLookupRow As Integer, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the lookup
            ' string from the lookup details array when the
            ' lookup ID has been matched.




            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Check for a match on the ID.

                If CInt(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) = lLookupID Then
                    ' Found a match

                    ' Store the details to the lookup string.

                    sLookupDesc = CStr(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr)).Trim()

                    Exit For
                End If
            Next lCntr

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgers
    '
    ' Description: Gets all the ledgers for the company
    '
    ' ***************************************************************** '
    Public Function GetLedgers() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the ledgers from the business object.


            m_lReturn = g_oBusiness.GetLedgersQuery(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vLedgers, vCompanyID:=g_iCompanyID)

            ' Check the return values.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get document info
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get ledgers from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgers")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerDetails
    '
    ' Description: Appends the ledger type names from the lookup
    '              details array to the user defined ledger names
    '              then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'developer guide no. 69
    Public Function GetLedgerDetails(ByRef ctlLookup As ComboBox, Optional ByVal vAllTypes As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check that GetLookupValues and GetLedgers have been called
            If (Not Information.IsArray(m_vLookupValues)) Or (Not Information.IsArray(m_vLedgers)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Populate the control with
            ' the details from the ledger details array.

            Dim sLookupDesc As String = ""


            If Not Information.IsNothing(vAllTypes) Then

                If CBool(vAllTypes) Then
                    ' First entry is all types (don't care)

                    sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    'developer guide no. 28
                    ctlLookup.Items.Add(New VB6.ListBoxItem(sLookupDesc, -1))

                End If
            End If


            For i As Integer = m_vLedgers.GetLowerBound(1) To m_vLedgers.GetUpperBound(1)
                ' Get the ledger type names from the lookup details array.
                sLookupDesc = ""

                m_lReturn = CType(GetLookupDesc(lLookupRow:=ACLLedgerType, lLookupID:=CInt(m_vLedgers(ACILedgerTypeID, i)), sLookupDesc:=sLookupDesc), gPMConstants.PMEReturnCode)

                If CStr(m_vLedgers(ACILedgerName, i)) <> sLookupDesc Then
                    ' Append the ledger type name

                    sLookupDesc = CStr(m_vLedgers(ACILedgerName, i)) & " (" & sLookupDesc & ")"
                End If

                'develope guide no. 28
                ctlLookup.Items.Add(New VB6.ListBoxItem(CStr(m_vLookupDetails(ACILedgerName, i)), CInt(m_vLookupDetails(ACILedgerID, i))))
            Next i

            ' Set the controls index to zero


            'develope guide no. 28
            ctlLookup.SelectedIndex = 0
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the ledger details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerNames
    '
    ' Description: Gets the ledger names from the details array m_vLedgers
    '              for a particular ledger id
    '
    ' ***************************************************************** '
    Public Function GetLedgerNames(ByRef iLedgerID As Integer, Optional ByRef vLedgerFullName As String = "", Optional ByRef vLedgerName As String = "", Optional ByRef vLedgerShortName As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check that GetLookupValues and GetLedgers have been called
            If (Not Information.IsArray(m_vLookupValues)) Or (Not Information.IsArray(m_vLedgers)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim sLookupDesc As String = ""


            For i As Integer = m_vLedgers.GetLowerBound(1) To m_vLedgers.GetUpperBound(1)
                ' Look for a match on the ledger id

                If CDbl(m_vLedgers(ACILedgerID, i)) = iLedgerID Then
                    ' Found a match

                    If Not Information.IsNothing(vLedgerFullName) Then
                        ' Get the ledger type names from the lookup details array.

                        m_lReturn = CType(GetLookupDesc(lLookupRow:=ACLLedgerType, lLookupID:=CInt(m_vLedgers(ACILedgerTypeID, i)), sLookupDesc:=sLookupDesc), gPMConstants.PMEReturnCode)

                        If CStr(m_vLedgers(ACILedgerName, i)) <> sLookupDesc Then
                            ' Append the ledger type name

                            sLookupDesc = CStr(m_vLedgers(ACILedgerName, i)) & " (" & sLookupDesc & ")"
                        End If
                        vLedgerFullName = sLookupDesc
                    End If

                    If Not Information.IsNothing(vLedgerName) Then
                        ' Get the ledger name from the details array.

                        vLedgerName = CStr(m_vLedgers(ACILedgerName, i))
                    End If

                    If Not Information.IsNothing(vLedgerShortName) Then
                        ' Get the ledger short name from the details array.

                        vLedgerShortName = CStr(m_vLedgers(ACILedgerShortName, i))
                    End If

                    Exit For
                End If
            Next i

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the ledger names", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerNames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerTypeID
    '
    ' Description: Gets the ledger type ID from the details array m_vLedgers
    '              for a particular ledger id
    '
    ' ***************************************************************** '
    Public Function GetLedgerTypeID(ByRef iLedgerID As Integer, ByRef iLedgerTypeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iLedgerTypeID = -1

            ' Call GetLedgers if necessary
            If Not Information.IsArray(m_vLedgers) Then
                m_lReturn = CType(GetLedgers(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If



            For i As Integer = m_vLedgers.GetLowerBound(1) To m_vLedgers.GetUpperBound(1)
                ' Look for a match on the ledger id

                If CDbl(m_vLedgers(ACILedgerID, i)) = iLedgerID Then
                    ' Found a match

                    iLedgerTypeID = CInt(m_vLedgers(ACILedgerTypeID, i))
                    Exit For
                End If
            Next i

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ledger type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerTypeID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerID
    '
    ' Description: Gets the ledger ID from the details array m_vLedgers
    '              for the first matching ledger type id
    '
    ' ***************************************************************** '
    Public Function GetLedgerID(ByRef iLedgerTypeID As Integer, ByRef iLedgerID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iLedgerID = -1

            ' Call GetLedgers if necessary
            If Not Information.IsArray(m_vLedgers) Then
                m_lReturn = CType(GetLedgers(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If



            For i As Integer = m_vLedgers.GetLowerBound(1) To m_vLedgers.GetUpperBound(1)
                ' Look for a match on the ledger id

                If CDbl(m_vLedgers(ACILedgerTypeID, i)) = iLedgerTypeID Then
                    ' Found a match

                    iLedgerID = CInt(m_vLedgers(ACILedgerID, i))
                    Exit For
                End If
            Next i

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ledger id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountType
    '
    ' Description: Get the WinAccs account type
    '              for a given ledger type ID
    '
    ' ***************************************************************** '
    'Public Function GetAccountType( _
    ''    iLedgerTypeID As Integer) As Integer
    '    Select Case iLedgerTypeID
    '    Case PSLedgerTypeNominal
    '        GetAccountType = 4
    '    Case PSLedgerTypeSales
    '        GetAccountType = 7
    '    Case PSLedgerTypePurchase
    '        GetAccountType = 6
    '    Case Else
    '        GetAccountType = 4
    '    End Select
    'End Function

    ' ***************************************************************** '
    ' Name: OnColumnClick
    '
    ' Description: Called by a form's listview column click event
    '
    ' ***************************************************************** '
    Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)

        Dim lColumnHeaderIndex As Integer
        Dim lSortOrder As SortOrder

        Try

            lColumnHeaderIndex = ColumnHeader.Index + 1 - 1

            With ListView

                Select Case lColumnHeaderIndex
                    Case ACListIBalance
                        If lColumnHeaderIndex = ListViewHelper.GetSortKeyProperty(ListView) Then
                            lSortOrder = (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2
                        Else
                            lSortOrder = SortOrder.Ascending
                        End If
                        'developer guide no. 170
                        m_lReturn = CType(ListViewFunc.ListViewSortByStringValue(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=lSortOrder), gPMConstants.PMEReturnCode)

                        ListViewHelper.SetSortedProperty(ListView, False)
                        ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
                    Case Else

                        If lColumnHeaderIndex = ListViewHelper.GetSortKeyProperty(ListView) Then
                            ' If current sort column header is
                            ' pressed.
                            ' Set sort order opposite of
                            ' current direction.
                            ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                        Else
                            ' Sort by this column (ascending).
                            ListViewHelper.SetSortedProperty(ListView, False)

                            ' Turn off sorting so that the list
                            ' is not sorted twice
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                            ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
                            ListViewHelper.SetSortedProperty(ListView, True)
                        End If
                End Select
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

End Module
