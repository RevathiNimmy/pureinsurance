Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/05/99
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' DAK261199 - Prevent Form being loaded twice
    ' DAK011299 - Add launch object button
    ' DAK031299 - Launch button is on frmDetails
    ' DAK031299 - Add SetKeyArray to Launch linked object
    ' DAK031299 - Set focus on the list view
    ' DAK071299 - Change caption on delete/undelete when deleting or
    '             undeleting
    ' DAK081299 - Do not get current key if there are none
    ' DAK091299 - Prefix code with "L" for key - allow for numeric code.
    ' DAK141299 - Set IDColumn properties on frmDetails
    ' DAK150600 - Get correct row number from tag
    ' MEvans : 25-11-2004 : PN13704 :
    '             Updated generic printing to base printer orientation
    '             on size of document.
    ' VB20050311 - PN15689 Removed Country Field from Country Lookup
    ' RKS160305  - PN19460 fix (removed the changes done by VB for PN15689)
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    'Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_sStepStatus As String = ""
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    ' {* USER DEFINED CODE (Begin) *}

    Private m_sTableName As String = ""
    Private m_iProductFamily As Integer

    Private m_vColumns As Array
    Private m_vKeyColumns As Object
    Private m_vData As Array

    'Private m_sInterface_Component As String

    Private m_bDisableExport As Boolean

    ' DisablePrint
    Private m_bDisablePrint As Boolean

    ' PMAuthorityLevel
    Private m_lPMAuthorityLevel As Integer
    ' PrivilegeLevel
    Private m_iPrivilegeLevel As Integer
    ' LinkedCaption
    Private m_sLinkedCaption As String = ""
    ' LinkedObjectName
    Private m_sLinkedObjectName As String = ""
    ' LinkedClassName
    Private m_sLinkedClassName As String = ""
    'DAK031299
    ' CurrentKey
    Private m_sCurrentKey As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMMaintainLookup.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Instance of the details form
    Private WithEvents m_frmDetails As frmDetails

    Private m_bIsRI2007Enabled As Boolean

    Private sortColumn As Integer = -1

    Private m_bSomethingChanged As Boolean = False

    Private m_sUniqueId As String
    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' Events
    Public Event LaunchLinkedObject(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer, ByVal v_vSetKeyArray As Object, ByRef o_bSomethingChanged As Boolean)
    ' PUBLIC Property Procedures (Begin)

    Public Property DisablePrint() As Boolean
        Get
            Return m_bDisablePrint
        End Get
        Set(ByVal Value As Boolean)
            m_bDisablePrint = Value
        End Set
    End Property

    Public Property DisableExport() As Boolean
        Get
            Return m_bDisableExport
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableExport = Value
        End Set
    End Property

    Public Property ProductFamily() As Integer
        Get
            Return m_iProductFamily
        End Get
        Set(ByVal Value As Integer)
            m_iProductFamily = Value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return m_sTableName
        End Get
        Set(ByVal Value As String)
            m_sTableName = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    'Public Property Get Interface_Component() As String
    '    Interface_Component = Trim(m_sInterface_Component$)
    'End Property
    'Public Property Let Interface_Component(sInterface_Component As String)
    '    m_sInterface_Component$ = Trim(sInterface_Component$)
    'End Property


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
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

            m_dtEffectiveDate = Value

        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property PrivilegeLevel() As Integer
        Get
            Return m_iPrivilegeLevel
        End Get
        Set(ByVal Value As Integer)
            m_iPrivilegeLevel = Value
        End Set
    End Property

    Public Property LinkedCaption() As String
        Get
            Return m_sLinkedCaption.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedCaption = Value.Trim()
        End Set
    End Property

    Public Property LinkedObjectName() As String
        Get
            Return m_sLinkedObjectName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedObjectName = Value.Trim()
        End Set
    End Property

    Public Property LinkedClassName() As String
        Get
            Return m_sLinkedClassName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedClassName = Value.Trim()
        End Set
    End Property

    'DAK031299
    Public Property CurrentKey() As String
        Get
            Return m_sCurrentKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sCurrentKey = Value.Trim()
        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConnectDatabase
    '
    ' Description: Called after initialise
    '
    ' ***************************************************************** '
    Public Function ConnectDatabase() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.ConnectDatabase(v_lProductFamily:=ProductFamily)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConnectDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConnectDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *************************************************************************** '
    ' Name: GetTableFKIndex
    '
    ' Description: Gets the column_id of an array that maps to the FK index
    '
    ' CTAF 160902 - Created to cope with tables that have had columns dropped
    '
    ' *************************************************************************** '
    Private Function GetTableFKIndex(ByVal v_lKeyColumnID As Integer, ByRef r_lRealColumnID As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lLoop1 As Integer = m_vColumns.GetLowerBound(1) To m_vColumns.GetUpperBound(1)
                If CDbl(m_vColumns(ACColumnID, lLoop1)) = v_lKeyColumnID Then
                    r_lRealColumnID = lLoop1
                    Exit For
                End If
            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to match foreign key to real column on table.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTableFKIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' *************************************************************************** '
    ' Name: RearrangeColumns
    '
    ' Description: Rearranges into the following order ...
    '
    ' Column ID, Caption ID, Code, Description, IsDeleted, Effective Date, Extras
    '
    ' *************************************************************************** '
    Private Function RearrangeColumns() As Integer

        Dim result As Integer = 0
        Dim sTable, sName As String
        Dim vNewColumns As Array

        Dim sColumnID As String = ""

        Dim lCurrentExtra As Integer

        ' CTAF 160902
        Dim lRealColumnID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the column_id name
            sColumnID = TableName.ToLower() & "_id"

            ' Start at position 6 for extras
            lCurrentExtra = 6

            ' Create a temporary array that will be the right order
            vNewColumns = Array.CreateInstance(GetType(Object), New Integer() {m_vColumns.GetUpperBound(0) - m_vColumns.GetLowerBound(0) + 1, m_vColumns.GetUpperBound(1) - m_vColumns.GetLowerBound(1) + 1}, New Integer() {m_vColumns.GetLowerBound(0), m_vColumns.GetLowerBound(1)})

            For iLoop1 As Integer = m_vColumns.GetLowerBound(1) To m_vColumns.GetUpperBound(1)

                sName = CStr(m_vColumns(ACColumnName, iLoop1)).ToLower()
                If sname = "option" Then
                    sname = "[" & sname & "]"
                End If

                Select Case (sName)
                    Case ACColumnNameCaptionID ' caption_id
                        vNewColumns(ACColumnName, ACCaptionID) = ACColumnNameCaptionID
                        vNewColumns(ACColumnID, ACCaptionID) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, ACCaptionID) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, ACCaptionID) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, ACCaptionID) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, ACCaptionID) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, ACCaptionID) = m_vColumns(ACColumnScale, iLoop1)

                    Case ACColumnNameCode ' code
                        vNewColumns(ACColumnName, ACCode) = ACColumnNameCode
                        vNewColumns(ACColumnID, ACCode) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, ACCode) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, ACCode) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, ACCode) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, ACCode) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, ACCode) = m_vColumns(ACColumnScale, iLoop1)

                    Case ACColumnNameDescription ' description
                        vNewColumns(ACColumnName, ACDescription) = ACColumnNameDescription
                        vNewColumns(ACColumnID, ACDescription) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, ACDescription) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, ACDescription) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, ACDescription) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, ACDescription) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, ACDescription) = m_vColumns(ACColumnScale, iLoop1)

                    Case ACColumnNameIsDeleted ' is_deleted
                        vNewColumns(ACColumnName, ACIsDeleted) = ACColumnNameIsDeleted
                        vNewColumns(ACColumnID, ACIsDeleted) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, ACIsDeleted) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, ACIsDeleted) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, ACIsDeleted) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, ACIsDeleted) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, ACIsDeleted) = m_vColumns(ACColumnScale, iLoop1)

                    Case ACColumnNameEffectiveDate ' effective_date
                        vNewColumns(ACColumnName, ACEffectiveDate) = ACColumnNameEffectiveDate
                        vNewColumns(ACColumnID, ACEffectiveDate) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, ACEffectiveDate) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, ACEffectiveDate) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, ACEffectiveDate) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, ACEffectiveDate) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, ACEffectiveDate) = m_vColumns(ACColumnScale, iLoop1)

                    Case sColumnID ' column_id
                        vNewColumns(ACColumnName, ACColumnID) = sColumnID
                        vNewColumns(ACColumnID, ACColumnID) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, ACColumnID) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, ACColumnID) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, ACColumnID) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, ACColumnID) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, ACColumnID) = m_vColumns(ACColumnScale, iLoop1)

                    Case Else

                        vNewColumns(ACColumnName, lCurrentExtra) = sName
                        vNewColumns(ACColumnID, lCurrentExtra) = m_vColumns(ACColumnID, iLoop1)
                        vNewColumns(ACColumnType, lCurrentExtra) = m_vColumns(ACColumnType, iLoop1)
                        vNewColumns(ACColumnLength, lCurrentExtra) = m_vColumns(ACColumnLength, iLoop1)
                        vNewColumns(ACColumnOffset, lCurrentExtra) = m_vColumns(ACColumnOffset, iLoop1)
                        vNewColumns(ACColumnPrecision, lCurrentExtra) = m_vColumns(ACColumnPrecision, iLoop1)
                        vNewColumns(ACColumnScale, lCurrentExtra) = m_vColumns(ACColumnScale, iLoop1)

                        lCurrentExtra += 1

                End Select

            Next iLoop1

            ' Check if we need to re-match the FK'd columns
            If g_bHasKeys Then

                For iLoop1 As Integer = m_vKeyColumns.GetLowerBound(1) To m_vKeyColumns.GetUpperBound(1)

                    ' CTAF 160902 - Get the position in the array for the column

                    m_lReturn = GetTableFKIndex(v_lKeyColumnID:=CInt(m_vKeyColumns(ACKeyColumnsIndex, iLoop1)), r_lRealColumnID:=lRealColumnID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sTable = CStr(m_vColumns(ACColumnName, lRealColumnID))

                    For iLoop2 As Integer = vNewColumns.GetLowerBound(1) To vNewColumns.GetUpperBound(1)

                        'SD 20/09/2002 Do a text comparison
                        '                If (sTable$ = vNewColumns(ACColumnName, iLoop2%)) Then
                        If String.Compare(sTable, CStr(vNewColumns(ACColumnName, iLoop2)), True) = 0 Then


                            m_vKeyColumns(ACKeyColumnsIndex, iLoop1) = iLoop2
                            Exit For
                        End If

                    Next iLoop2

                Next iLoop1
            End If

            ' Start-PN45545
            'To add element at the end to the array to hold temporary flag to Edit

            vNewColumns = ArraysHelper.RedimPreserve(Of Object(,))(vNewColumns, New Integer() {m_vColumns.GetUpperBound(0) - m_vColumns.GetLowerBound(0) + 1, m_vColumns.GetUpperBound(1) + 1 - m_vColumns.GetLowerBound(1) + 1}, New Integer() {m_vColumns.GetLowerBound(0), m_vColumns.GetLowerBound(1)})

            'To assign 0 to the last added element

            For iLoop1 As Integer = vNewColumns.GetLowerBound(1) To vNewColumns.GetUpperBound(0)
                vNewColumns(iLoop1, vNewColumns.GetUpperBound(1)) = 0
            Next

            'End -PN45545

            m_vColumns = vNewColumns

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RearrangeColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RearrangeColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetColumns
    '
    ' Description: Gets the column details
    '
    ' ***************************************************************** '
    Public Function GetColumns() As Integer

        Dim result As Integer = 0
        Dim sKey, sColumn, sNewColumn As String
        Dim bIsLookup As Boolean
        Dim vTempArray As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the business to get the column details


            m_lReturn = m_oBusiness.GetColumns(v_sTableName:=TableName, v_sProductCode:=gPMConstants.PMProductCode(ProductFamily), r_vColumns:=m_vColumns)

            If Not m_bIsRI2007Enabled And TableName.ToLower() = "ri_band" Then
                If Information.IsArray(m_vColumns) Then
                    m_vColumns = ArraysHelper.RedimPreserve(Of Object(,))(m_vColumns, New Integer() {m_vColumns.GetUpperBound(0) + 1, m_vColumns.GetUpperBound(1) - 1}, New Integer() {0, 0})
                End If
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve columns for table : " & TableName & _
                                   Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ensure correct database named in PMProduct.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumns", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' CF 310899
            ' Check its a lookup table
            m_lReturn = CheckLookup(v_vTableArray:=m_vColumns, r_bIsLookUp:=bIsLookup)
            If Not bIsLookup Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=TableName & " is not a valid lookup table.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumns", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the business to get which columns are keyed

            m_lReturn = m_oBusiness.GetKeyColumns(v_sTableName:=TableName, v_sProductCode:=gPMConstants.PMProductCode(ProductFamily), r_vKeyColumns:=m_vKeyColumns)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Do we have any key columns?
            If Not m_bIsRI2007Enabled And TableName.ToLower() = "ri_band" Then

                m_vKeyColumns = Nothing
            End If

            g_bHasKeys = (Information.IsArray(m_vKeyColumns))

            ' Re-arrange columns into a nice order
            m_lReturn = RearrangeColumns()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim sView As String = ""
            If g_bHasKeys Then
                ' CF 310899
                ' Check they're lookup columns

                For iLoop1 As Integer = m_vKeyColumns.GetLowerBound(1) To m_vKeyColumns.GetUpperBound(1)

                    'CJR 22/1/2003 Added to use a view if a constrained column is named after one.


                    m_lReturn = m_oBusiness.GetViewForColumn(v_sColName:=CStr(m_vColumns(ACColumnName, CInt(m_vKeyColumns(0, iLoop1)))), r_sView:=sView)

                    If sView <> "" Then

                        m_vKeyColumns(1, iLoop1) = sView
                    End If


                    ' Get the columns for this table

                    m_lReturn = m_oBusiness.GetColumns(v_sTableName:=m_vKeyColumns(1, iLoop1), v_sProductCode:=gPMConstants.PMProductCode(ProductFamily), r_vColumns:=vTempArray)

                    ' Check that this table is a lookup table

                    m_lReturn = CheckLookup(v_vTableArray:=vTempArray, r_bIsLookUp:=bIsLookup)

                    If Not bIsLookup Then
                        ' Zero the key column out

                        m_vKeyColumns(ACKeyColumnsIndex, iLoop1) = ""

                        m_vKeyColumns(ACKeyColumnsTable, iLoop1) = ""
                    End If

                Next iLoop1
            End If

            ' Clear the column headers
            lvwTable.Columns.Clear()

            ' Add the list view headers
            ' +1 so we skip the ID
            For iLoop1 As Integer = m_vColumns.GetLowerBound(1) + 1 To m_vColumns.GetUpperBound(1)
                sKey = "A" & iLoop1
                sColumn = CStr(m_vColumns(1, iLoop1))
                ' Dont show the is_deleted or caption_id fields
                If (sColumn = ACColumnNameCode) Or (sColumn = ACColumnNameDescription) Or (sColumn = ACColumnNameEffectiveDate) Then
                    m_lReturn = IDToEnglish(v_sID:=sColumn, r_sEnglish:=sNewColumn)
                    lvwTable.Columns.Add(sKey, sNewColumn, 94)
                End If
            Next iLoop1

            '        If (sColumn <> ACColumnNameIsDeleted) And _
            ''           (sColumn <> ACColumnNameCaptionID) Then

            g_bHasExtras = (m_vColumns.GetUpperBound(1) >= ACExtraStart)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sColumns As New StringBuilder
        Dim lCol As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            sColumns = New StringBuilder("")
            lCol = m_vColumns.GetUpperBound(1)

            '    If m_bIsRI2007Enabled = False And LCase(TableName) = "ri_band" Then
            '        lCol = lCol - 2
            '    End If

            For iLoop1 As Integer = m_vColumns.GetLowerBound(1) To lCol
                If CStr(m_vColumns(1, iLoop1)) <> "0" Then
                    sColumns.Append("[")
                End If

                sColumns.Append(CStr(m_vColumns(1, iLoop1)))

                If iLoop1 < lCol Then
                    sColumns.Append("], ")
                End If
            Next iLoop1




            m_lReturn = m_oBusiness.GetDetails(v_sTableName:=TableName, v_sColumns:=sColumns.ToString(), r_vData:=m_vData)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sKey As String
        Dim sText As String
        Dim sDate As String
        Dim sListItem() As String
        Dim oListViewItem() As ListViewItem

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' Set the caption on the form
            'DAK261199
            Text = gPMFunctions.ReformatText(TableName) & " Maintenance"

            ' CF 050999 - Start the batch
            'Developer Guide No. 178
            'm_lReturn = ListView6Func.ListViewBatchStart(lvwTable)

            ' Clear the list
            lvwTable.Items.Clear()

            If Information.IsArray(m_vData) Then
                ReDim sListItem(ACItemMaximum)
                ReDim oListViewItem(m_vData.GetUpperBound(1))

                For lLoop1 As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)

                    ' We need to protect against duplicates

                    lstItem = Nothing

                    sListItem(ACItemCode) = m_vData(ACCode, lLoop1).ToString.Trim()
                    sListItem(ACItemDescription) = m_vData(ACDescription, lLoop1).ToString.Trim()
                    sListItem(ACItemEffectiveDate) = DateTime.Parse(CStr(m_vData(ACEffectiveDate, lLoop1)).Trim()).ToString("d")

                    lstItem = New ListViewItem(sListItem)
                    lstItem.Tag = lLoop1.ToString
                    lstItem.Name = "L" & m_vData(ACCode, lLoop1).ToString
                    oListViewItem(lLoop1) = lstItem

                    If lstItem Is Nothing Then
                        ' We have a duplicate
                        MessageBox.Show("Lookup table corrupt! Duplicate keys found" & Strings.Chr(13) & Strings.Chr(10) & _
                                        "Please contact system administrator!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Return gPMConstants.PMEReturnCode.PMCancel
                    End If

                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(CStr(m_vData(ACIsDeleted, lLoop1)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        sText = CStr(m_vData(ACIsDeleted, lLoop1)).Trim().ToLower()
                        If sText = "false" Then
                            m_vData(ACIsDeleted, lLoop1) = 0
                        Else
                            m_vData(ACIsDeleted, lLoop1) = 1
                        End If
                    End If


                    'Developer Guide No. 12 (No Solution)
                    If (CDbl(m_vData(ACIsDeleted, lLoop1)) = 1) Then
                        lstItem.ForeColor = Color.Gray
                    End If


                Next lLoop1
                lvwTable.Items.AddRange(oListViewItem)

                'DAK031299
                If CurrentKey = "" Then
                    'lvwTable.FocusedItem = lvwTable.Items.Item(0)
                    lvwTable.Items(0).Selected = True
                    lvwTable.Select()
                Else
                    'lvwTable.FocusedItem = lvwTable.Items.Item(CurrentKey)
                    lvwTable.FullRowSelect = True
                    lvwTable.Items.Item(CurrentKey).Selected = True
                    lvwTable.Select()
                End If

            End If

            lvwTable.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
            ' Sort the widths out on the columns        
            lvwTable.Sorting = SortOrder.Ascending


            'DAK031299
            If Me.Visible Then
                lvwTable.Focus()
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try
        Return result

    End Function

    ''' <summary>
    ''' Updates all business members from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer

        Dim nResult As Integer = 0
        Dim oColumns, oValues As Object
        Dim nCol As Integer

        Dim nEvent_ID As Integer
        Try
            nResult = PMEReturnCode.PMTrue
            ' Update the business object.

            ' Check we have some data to update first
            If Not Information.IsArray(m_vData) Then
                Return nResult
            End If

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

           
            nCol = m_vColumns.GetUpperBound(1)


            ReDim oColumns(1, nCol)

            For nLoop1 As Integer = 0 To nCol
                oColumns(0, nLoop1) = m_vColumns(ACColumnType, nLoop1).ToString
                oColumns(1, nLoop1) = m_vColumns(ACColumnName, nLoop1).ToString
            Next nLoop1

            ' Make the values array
            ReDim oValues(m_vData.GetUpperBound(0))

            ' Loop through the data and see if its edit or add
            For nLoop1 As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)

                ' Update the values array
                For nLoop2 As Integer = 0 To m_vData.GetUpperBound(0)

                    oValues(nLoop2) = m_vData(nLoop2, nLoop1)
                Next nLoop2

                'wpr085
                If TableName.ToLower() = "bankaccount_default" AndAlso m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    MsgBox("Failed to update the record.You can have only one default bank accounts with same branch and same cashlisttype.", MsgBoxStyle.Information, "Failed to add duplicate record")
                    Return nResult
                End If

                If UCase(TableName) = UCase("BankAccount_Default") Then
                    Dim sValidationErrorMessage As String
                    m_lReturn = m_oBusiness.ValidateBankAccountDetailsForReceiptType(m_vData(ACColumnID, nLoop1), oValues, oColumns, sValidationErrorMessage)
                    If Len(sValidationErrorMessage) > 0 Then
                        MsgBox(sValidationErrorMessage, vbOKOnly + vbCritical, "Validation")
                        InterfaceToBusiness = PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If

                If CInt(m_vData(ACColumnID, nLoop1)) = 0 Then
                    ' Add
                    m_lReturn = m_oBusiness.Add(v_sTableName:=TableName, v_vColumns:=oColumns, r_vValues:=oValues, v_sUniqueId:=m_sUniqueId)

                    If m_lReturn = PMEReturnCode.PMTrue Then
                        ' Save the _id
                        m_vData(ACColumnID, nLoop1) = oValues(0)
                    End If

                    If TableName.ToLower() = "mta_event_description" OrElse TableName.ToLower() = "claim_event_description" Then
                        nEvent_ID = CInt(m_vData(ACColumnID, nLoop1))

                        m_lReturn = m_oBusiness.UpdateProductEvents(v_sTableName:=TableName, v_lEventId:=nEvent_ID)

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            nResult = PMEReturnCode.PMFalse

                            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                            Return nResult
                        End If

                    End If

                ElseIf (CInt(m_vData(m_vData.GetUpperBound(0), nLoop1)) = 1) Then  'PN45545- if array hold 1 means Edit or 0 means Add in the last element

                    ' Update
                    m_lReturn = m_oBusiness.Update(v_sTableName:=TableName, v_vColumns:=oColumns, v_vValues:=oValues, v_sUniqueId:=m_sUniqueId)

                End If

                ' Check for errors.
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    Return nResult
                End If
            Next nLoop1

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try


            ' Get the lookup values.

            'm_lReturn& = GetLookupValues()

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: CheckLookup
    '
    ' Description: Checks to make sure the passed columns meet the requirements
    '              to be a lookup table.
    '
    ' History: 31/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckLookup(ByVal v_vTableArray(,) As Object, ByRef r_bIsLookUp As Boolean) As Integer

        Dim result As Integer = 0
        Dim iCount As Integer
        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iCount = 0

            ' Check all the columns
            For iLoop1 As Integer = v_vTableArray.GetLowerBound(1) To v_vTableArray.GetUpperBound(1)


                sTemp = CStr(v_vTableArray(1, iLoop1)).Trim().ToLower()

                Select Case sTemp
                    ' Increment the counter if its a lookup column
                    Case ACColumnNameCaptionID, ACColumnNameCode, ACColumnNameDescription, ACColumnNameIsDeleted, ACColumnNameEffectiveDate
                        iCount += 1
                End Select

            Next iLoop1

            ' If we don't have matching lookup columns, then its not a winner.
            r_bIsLookUp = Not (iCount < 5)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            'm_lReturn& = m_oBusiness.GetNext()

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retreive the details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        'PN 38357 (RC)
        Dim result As Integer = 0
        Dim lCaptionID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' Loop around and get the caption_id's

            For lLoop1 As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)

                lCaptionID = CInt(m_vData(ACCaptionID, lLoop1))

                If (CDbl(m_vData(ACColumnID, lLoop1)) = 0) Or (CDbl(m_vData(m_vData.GetUpperBound(0), lLoop1)) = 1) Then 'PN45545
                    ' Go to the business and get the caption_id

                    m_lReturn = m_oBusiness.GetCaptionID(v_sCaption:=m_vData(ACDescription, lLoop1), r_lCaptionID:=lCaptionID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_vData(ACCaptionID, lLoop1) = lCaptionID
                End If
            Next lLoop1

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = DisplayCaptions()

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

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Disable view, edit and delete
            cmdView.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            ' Disable apply by default
            cmdApply.Enabled = False

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'ReDim m_ctlTabFirstLast(1, )

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApplyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    'SP150998 - compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ExtrasToArray
    '
    ' Description: Converts extras lookup fields to an array with
    '              caption info etc...
    '
    ' ***************************************************************** '
    Private Function ExtrasToArray(ByVal v_lRow As Integer, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim iUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vData) Then
                iUpper = m_vData.GetUpperBound(0)
            Else
                iUpper = m_vColumns.GetUpperBound(1)
            End If

            ReDim r_vArray(7, iUpper - ACExtraStart)

            For lLoop1 As Integer = ACExtraStart - 1 To iUpper - 1

                ' Set the column name. This is where the details form gets
                ' the name for the dynamic controls from.
                If m_bIsRI2007Enabled And TableName.ToLower() = "ri_band" Then

                    r_vArray(ACExtraCaption, lLoop1 - (ACExtraStart - 1)) = CStr(m_vColumns(ACColumnName, lLoop1)).Replace("_id", "")
                Else

                    r_vArray(ACExtraCaption, lLoop1 - (ACExtraStart - 1)) = m_vColumns(ACColumnName, lLoop1)
                End If

                ' If we're adding, then we wont have data, so leave it blank.
                If v_lRow <> -1 Then

                    r_vArray(ACExtraValue, lLoop1 - (ACExtraStart - 1)) = m_vData(lLoop1, v_lRow)
                Else

                    If r_vArray(ACExtraCaption, lLoop1 - (ACExtraStart - 1)) = "udl_version" Then
                        Dim version_id As Integer = 0
                        result = m_oBusiness.GetUDLVersion(TableName, version_id)
                        r_vArray(ACExtraValue, lLoop1 - (ACExtraStart - 1)) = version_id
                    Else
                        r_vArray(ACExtraValue, lLoop1 - (ACExtraStart - 1)) = ""
                    End If
                End If


                r_vArray(ACExtraLength, lLoop1 - (ACExtraStart - 1)) = m_vColumns(ACColumnLength, lLoop1)

                If m_bIsRI2007Enabled And TableName.ToLower() = "ri_band" Then

                    r_vArray(ACExtraOffset, lLoop1 - (ACExtraStart - 1)) = 1
                Else

                    r_vArray(ACExtraOffset, lLoop1 - (ACExtraStart - 1)) = m_vColumns(ACColumnOffset, lLoop1)
                End If


                r_vArray(ACExtraType, lLoop1 - (ACExtraStart - 1)) = m_vColumns(ACColumnType, lLoop1)

                If g_bHasKeys Then

                    For iLoop2 As Integer = m_vKeyColumns.GetLowerBound(1) To m_vKeyColumns.GetUpperBound(1)
                        ' CF 310899 - Check to make sure the column hasn't been zero'd

                        If CStr(m_vKeyColumns(ACKeyColumnsIndex, iLoop2)) <> "" Then

                            If CInt(m_vKeyColumns(ACKeyColumnsIndex, iLoop2)) = lLoop1 Then


                                r_vArray(ACExtraLookupTable, lLoop1 - (ACExtraStart - 1)) = m_vKeyColumns(ACKeyColumnsTable, iLoop2)
                            End If
                        End If
                    Next iLoop2
                End If

                'DJM 08/03/2004

                r_vArray(ACExtraPrecision, lLoop1 - (ACExtraStart - 1)) = m_vColumns(ACColumnPrecision, lLoop1)

                r_vArray(ACExtraScale, lLoop1 - (ACExtraStart - 1)) = m_vColumns(ACColumnScale, lLoop1)

            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExtrasToArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtrasToArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' ProcessDetails
    ''' </summary>
    ''' <param name="v_iTask"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function ProcessDetails(ByVal v_iTask As Integer) As Integer

        Dim nResult As Integer = 0
        Dim nCaptionID As Integer = 0
        Dim sCode As String = ""
        Dim sDescription As String = ""
        Dim nIsDeleted As Integer = 0
        Dim dtEffectiveDate As Date
        Dim oExtras As Object = Nothing
        'PN 38357 (RC)
        Dim bError As Boolean = False
        Dim iRow As Integer = 0
        Dim sOldCode As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_frmDetails = New frmDetails()

            With m_frmDetails
                ' Pre-process
                Select Case (v_iTask)
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMEdit

                        'DAK081299
                        'CurrentKey = lvwTable.FocusedItem.Name
                        CurrentKey = lvwTable.SelectedItems(0).Name

                        ' Get the values from the list
                        'DAK150600 - Get correct row number from tag

                        iRow = Convert.ToInt32(lvwTable.SelectedItems(0).Tag)

                        nCaptionID = CInt(m_vData(ACCaptionID, iRow))
                        sCode = CStr(m_vData(ACCode, iRow))
                        sOldCode = sCode
                        sDescription = CStr(m_vData(ACDescription, iRow))
                        nIsDeleted = CInt(m_vData(ACIsDeleted, iRow))
                        dtEffectiveDate = CDate(m_vData(ACEffectiveDate, iRow))

                        If Not m_bIsRI2007Enabled And TableName.ToLower() = "ri_band" Then
                        Else
                            If g_bHasExtras Then

                                m_lReturn = ExtrasToArray(v_lRow:=iRow, r_vArray:=oExtras)
                            End If
                        End If
                        ' Set the properties
                        'DAK141299
                        .IDColumnName = CStr(m_vColumns(1, ACColumnID))
                        .IDColumnValue = CInt(m_vData(ACColumnID, iRow))
                        .CaptionID = nCaptionID
                        .Code = sCode
                        .Description = sDescription
                        .IsDeleted = nIsDeleted
                        .EffectiveDate = dtEffectiveDate

                        'Developer Guide No. 24
                        .Extras = oExtras
                        .UniqueId = m_sUniqueId
                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' Default the parameters
                        'DC 27/07/00 Do not set to date and time just date only
                        '.EffectiveDate = Format$(Date$, "dd/mm/yyyy")
                        ' RDC 25042002 use Now, as Date$ causes dd/mm-to-mm/dd problems
                        .EffectiveDate = CDate(DateTime.Now.ToString("dd MMM yyyy"))
                        .IsDeleted = 0

                        ' -1 so it just initialises a blank array
                        If Not m_bIsRI2007Enabled And TableName.ToLower() = "ri_band" Then
                        Else
                            If g_bHasExtras Then

                                m_lReturn = ExtrasToArray(v_lRow:=-1, r_vArray:=oExtras)
                            End If
                        End If


                        'Developer Guide No. 24
                        .Extras = oExtras

                End Select

                ' Set the process mode
                .ProcessMode = v_iTask

                ' Set the product family
                .ProductFamily = ProductFamily

                ' Set Authority level
                .PMAuthorityLevel = PMAuthorityLevel

                ' Set Privilege level
                .PrivilegeLevel = PrivilegeLevel
                'MIPS01
                .TableName = TableName

                ' Set Details for linked button
                If LinkedCaption = "" Then
                    .cmdLinkObject.Visible = False
                    .cmdLinkObject.Text = ""
                Else
                    .cmdLinkObject.Visible = True
                    .cmdLinkObject.Text = LinkedCaption
                    If LinkedCaption.Trim().ToUpper() = ("Earning Pattern").ToUpper() Then
                        .cmdLinkObject.Width = VB6.TwipsToPixelsX(1510)
                    End If
                End If

                'RVH 12/08/2002 - Start
                '                 Check to see if the link object button should be
                '                 enabled or not - only enable it when the changes
                '                 have been applied...
                If LinkedCaption <> "" Then
                    If v_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                        iRow = Convert.ToInt32(lvwTable.SelectedItems(0).Tag)
                        'lRow = Convert.ToString(lvwTable.FocusedItem.Tag)
                        .cmdLinkObject.Enabled = Not (CDbl(m_vData(ACColumnID, iRow)) = 0)
                    Else
                        .cmdLinkObject.Enabled = False
                    End If
                End If
                'RVH 12/08/2002 - End

                ' Display form
                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    m_frmDetails = Nothing
                    Return nResult
                End If

                ' Dont update if we've cancelled
                If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                    m_frmDetails = Nothing
                    If Me.Visible Then
                        lvwTable.Focus()
                    End If
                    Return nResult
                End If

                ' Post-process

                Select Case (v_iTask)
                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' Get the properties
                        nCaptionID = .CaptionID
                        sCode = .Code
                        sDescription = .Description
                        nIsDeleted = .IsDeleted
                        dtEffectiveDate = .EffectiveDate

                        oExtras = .Extras

                        bError = False

                        ' Check the code doesn't already exist
                        If Information.IsArray(m_vData) Then
                            For lLoop1 As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)
                                If CStr(m_vData(ACCode, lLoop1)).Trim().ToLower() = sCode.Trim().ToLower() Then
                                    MessageBox.Show("Error: Unable to add duplicate code '" & sCode & "'",
                                                    "Duplicate Code", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    bError = True
                                    Exit For
                                End If
                            Next
                        End If

                        ' Only store it if no error
                        If Not bError Then

                            If Not Information.IsArray(m_vData) Then
                                m_vData = Array.CreateInstance(GetType(Object),
                                                               New Integer() {m_vColumns.GetUpperBound(1) + 1, 1},
                                                               New Integer() {0, 0})
                            Else
                                ' Expand the array
                                m_vData = ArraysHelper.RedimPreserve(Of Object(,))(m_vData,
                                                                                    New Integer() _
                                                                                       {m_vData.GetUpperBound(0) -
                                                                                        m_vData.GetLowerBound(0) + 1,
                                                                                        m_vData.GetUpperBound(1) + 1 -
                                                                                        m_vData.GetLowerBound(1) + 1},
                                                                                    New Integer() _
                                                                                       {m_vData.GetLowerBound(0),
                                                                                        m_vData.GetLowerBound(1)})
                            End If

                            iRow = m_vData.GetUpperBound(1)

                            m_vData(ACColumnID, iRow) = 0

                            ' Store the properties in the data array
                            m_vData(ACCaptionID, iRow) = nCaptionID
                            m_vData(ACCode, iRow) = sCode
                            m_vData(ACDescription, iRow) = sDescription
                            m_vData(ACIsDeleted, iRow) = nIsDeleted
                            m_vData(ACEffectiveDate, iRow) = dtEffectiveDate
                            m_vData(m_vData.GetUpperBound(0), iRow) = 0
                            If oExtras IsNot Nothing Then
                                For lLoop1 As Integer = ACExtraStart - 1 To m_vData.GetUpperBound(0) - 1
                                    m_vData(lLoop1, iRow) = oExtras(ACExtraValue, lLoop1 - (ACExtraStart - 1))
                                Next
                            Else
                                If Not m_bIsRI2007Enabled AndAlso TableName.ToLower() = "ri_band" Then
                                    For lLoop1 As Integer = ACExtraStart - 1 To m_vData.GetUpperBound(0) - 1
                                        m_vData(lLoop1, iRow) = ""
                                    Next
                                End if
                                m_vData(ACIsEdit, iRow) = 1
                            End If
                            'DAK091299
                            CurrentKey = "L" & sCode

                        End If

                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMEdit

                        ' Get the properties
                        nCaptionID = .CaptionID
                        sCode = .Code
                        sDescription = .Description
                        nIsDeleted = .IsDeleted
                        dtEffectiveDate = .EffectiveDate

                        oExtras = .Extras

                        bError = False

                        ' Check the code doesnt already exist
                        For lLoop1 As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)
                            If CStr(m_vData(ACCode, lLoop1)).Trim().ToLower() = sCode.Trim().ToLower() Then
                                ' and if it isnt the one we edited...
                                ' This shouldnt ever happen as they can't edit codes
                                If sCode.Trim() <> sOldCode.Trim() Then
                                    MessageBox.Show("Error: Unable to add duplicate code '" & sCode & "'",
                                                    "Duplicate Code", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    bError = True
                                    Exit For
                                End If
                            End If

                        Next

                        ' Only store it if no error
                        If Not bError Then
                            iRow = Convert.ToInt32(lvwTable.SelectedItems(0).Tag)
                            ' Store the properties in the data array
                            m_vData(ACCaptionID, iRow) = nCaptionID
                            m_vData(ACCode, iRow) = sCode
                            m_vData(ACDescription, iRow) = sDescription
                            m_vData(ACIsDeleted, iRow) = nIsDeleted
                            m_vData(ACEffectiveDate, iRow) = dtEffectiveDate
                            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                                m_vData(m_vData.GetUpperBound(0), iRow) = 1
                            End If
                            If m_bIsRI2007Enabled = False And LCase(TableName) = "ri_band" Then
                            Else
                                If (g_bHasExtras = True) AndAlso oExtras IsNot Nothing Then
                                    ' Handle any extras values
                                    For lLoop1 As Integer = ACExtraStart - 1 To m_vData.GetUpperBound(0) - 1
                                        If _
                                            ToSafeString(oExtras(ACExtraCaption, lLoop1 - (ACExtraStart - 1))) =
                                            "is_marketplace_data_model" AndAlso m_bSomethingChanged Then
                                            m_vData(lLoop1, iRow) = False
                                        Else
                                            m_vData(lLoop1, iRow) = oExtras(ACExtraValue, lLoop1 - (ACExtraStart - 1))

                                        End If
                                    Next
                                End If
                            End If

                        End If

                End Select

            End With

            m_frmDetails = Nothing

            ' Update the list
            m_lReturn = BusinessToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Only enable Apply if we edited or added
            If v_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' Enable the apply button
                cmdApply.Enabled = True
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem
    '
    ' Description: Deletes or undeletes an item
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Dim lRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the values from the list
            'DAK200600 - get correct row
            '    lRow& = lvwTable.SelectedItem.Index - 1
            lRow = Convert.ToString(lvwTable.FocusedItem.Tag)

            If Convert.ToString(cmdDelete.Tag) = ACTagDelete Then
                m_vData(ACIsDeleted, lRow) = "0"
                'DAK071299
                cmdDelete.Text = "&Delete"
                cmdDelete.Tag = ACTagUndelete
                cmdEdit.Enabled = True
            Else
                m_vData(ACIsDeleted, lRow) = "1"
                'DAK071299
                cmdDelete.Text = "&Undelete"
                cmdDelete.Tag = ACTagDelete
                cmdEdit.Enabled = False
            End If

            CurrentKey = lvwTable.FocusedItem.Name
            ' Update the list
            m_lReturn = BusinessToInterface()

            ' Enable the apply button
            cmdApply.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMAdd)

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'DAK031299
            If Me.Visible Then
                lvwTable.Focus()
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_sUniqueId = GetUniqueID()
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Do everything the same as OK button, except for hiding the form.

            'SD 09/08/2002
            ' Might want to update interface - if data was truncated. Note that we
            ' set the currentkey because it may be truncated in theory if it is Varchar
            ' Maybe this feature could be refined.

            If m_oBusiness.FieldsTruncated = TriState.True Then
                CurrentKey = ""
                m_lReturn = m_oGeneral.GetInterfaceDetails()
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                cmdApply.Enabled = False
            End If
            'Start(Sriram P)CacheBug

            m_lReturn = UpdateCache()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'End(Sriram P)CacheBug

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to apply changes.", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_CmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        Dim lRow As Integer = Convert.ToString(lvwTable.FocusedItem.Tag)
        m_vData(m_vData.GetUpperBound(0), lRow) = 1
        ' Mark the current item as deleted
        m_lReturn = DeleteItem()
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_lReturn = ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMEdit)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ExportDetails
    '
    ' Description: Saves all the details to a file
    '
    ' History: 05/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ExportDetails() As Integer
        Dim Err_ExportDetails As Boolean = False
        Dim Err_FileOpen As Boolean = False

        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim lFileHandle As Integer
        Dim sText As New StringBuilder
        Dim fileResult As Integer = 0
        'PN 38357 (RC)

        Try
            Err_FileOpen = True
            Err_ExportDetails = False

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the common dialog box

            With dlgMainSave


                .FileName = m_sTableName


                .OverwritePrompt = True


                .DefaultExt = ".csv"

                .Filter = "CSV (Comma delimited) (*.csv)|*.csv"

                ' Show the dialog box
                fileResult = .ShowDialog()


                ' Get the filename
                sFileName = .FileName
            End With
            If fileResult = Windows.Forms.DialogResult.Cancel Then
                Exit Function
            End If

            ' Switch back to the proper error handler
            Err_ExportDetails = True
            Err_FileOpen = False

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get the next free file
            lFileHandle = FileSystem.FreeFile()

            ' Open the file
            FileSystem.FileOpen(lFileHandle, sFileName, OpenMode.Output)

            ' Print column headers
            sText = New StringBuilder("")
            For lLoop1 As Integer = m_vColumns.GetLowerBound(1) To m_vColumns.GetUpperBound(1)

                sText.Append(CStr(m_vColumns(1, lLoop1)) & ",")

            Next lLoop1

            ' Chop the last comma off
            sText = New StringBuilder(sText.ToString().Substring(0, sText.ToString().Length - 1))

            FileSystem.PrintLine(lFileHandle, sText.ToString())

            ' Output the data
            For lLoop1 As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)

                sText = New StringBuilder("")

                ' Construct the string
                For lLoop2 As Integer = m_vData.GetLowerBound(0) To m_vData.GetUpperBound(0)
                    If IsNumeric(m_vData(lLoop2, lLoop1)) = False AndAlso Information.IsDate(m_vData(lLoop2, lLoop1)) Then 'Removing the time part PN 20132
                        sText.Append(CDate(m_vData(lLoop2, lLoop1)).ToString("dd MMM yyyy") & ",")
                    Else
                        If (CStr(m_vData(lLoop2, lLoop1)).Contains(",")) Then
                            sText.Append(String.Format("""{0}"",", CStr(m_vData(lLoop2, lLoop1))))

                        Else

                            sText.Append(CStr(m_vData(lLoop2, lLoop1)) & ",")
                        End If
                    End If
                Next lLoop2

                ' Chop the last comma off
                sText = New StringBuilder(sText.ToString().Substring(0, sText.ToString().Length - 1))

                FileSystem.PrintLine(lFileHandle, sText.ToString())

            Next lLoop1

            ' Close the file
            FileSystem.FileClose(lFileHandle)

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Message box to tell user its finished
            MessageBox.Show("Export Complete", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return result

        Catch excep As System.Exception
            If Not Err_ExportDetails And Not Err_FileOpen Then
                Throw excep
            End If

            ' This is just used to trap the cancel button on the dialog box
            If Err_FileOpen Then


                Return result

            End If
            If Err_ExportDetails Or Err_FileOpen Then


                result = gPMConstants.PMEReturnCode.PMError

                ' Set the mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExportDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExportDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
        End Try
    End Function

    Private Function ParseValidDate(ByVal s As String) As Boolean

        Dim dt As Date
        Return Date.TryParseExact(s, "dd/MM/yyyy hh:mm:ss", CultureInfo.CurrentCulture, _
                              DateTimeStyles.None, dt)
    End Function
    ''' <summary>
    ''' This procedure will handle cmdExport_Click event and export the data model script in case of table "GIS_DATA_MODEL"
    ''' export csv for all other type of tables 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExport.Click
        Dim nReturn As Integer

        Try
            ' Only try to export the data if there is some to export
            If Information.IsArray(m_vData) Then
                If m_sTableName.ToString.ToUpper().Trim() = "GIS_DATA_MODEL" Then
                    If MessageBox.Show("Do you wish to export data model script", "Export Data Model Script", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        nReturn = m_oBusiness.ExportDataModelScript(lvwTable.SelectedItems(0).Text)
                        If nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show(lvwTable.SelectedItems(0).Text.Trim() & " data model script exported successfully.", "Export Data Model", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to export " & lvwTable.SelectedItems(0).Text.Trim() & " data model script", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExport_Click")
                        End If
                    End If
                Else
                    nReturn = ExportDetails()
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to export details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExport_Click")
                    End If
                End If
            Else
                MessageBox.Show("There is no data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdExport_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExport_Click", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        Finally
            If Me.Visible Then
                Me.Activate()
                Me.BringToFront()
                lvwTable.Focus()
            End If
        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: PrintDetails
    '
    ' Description: Prints the details
    '
    ' History: 05/09/1999 CTAF - Created.
    '          14/01/2000 DAK - completely rewritten
    '          MEvans : 25-11-2004 : PN13704
    ' ***************************************************************** '
    Private Function PrintDetails() As Integer
        Dim result As Integer = 0
        'PN 38357 (RC)
        Dim lColWidth As Integer
        Dim sText As String = ""
        Dim lHeaderHeight, lRowHeight, lXPos, lYPos As Integer

        Dim lTotalWidth As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the printer properties
            With PrinterHelper.Printer
                .Copies = 1
            End With

            Dim vColWidth As Array = Array.CreateInstance(GetType(Object), New Integer() {m_vColumns.GetUpperBound(1) - m_vColumns.GetLowerBound(1) + 1}, New Integer() {m_vColumns.GetLowerBound(1)})

            ' Find the column widths for the titles
            With PrinterHelper.Printer
                .FontSize = 10

                PrinterHelper.Printer.Font = VB6.FontChangeName(PrinterHelper.Printer.Font, "Ariel")
                .FontBold = False
                .FontUnderline = True
            End With

            For lColSub As Integer = m_vColumns.GetLowerBound(1) To m_vColumns.GetUpperBound(1)
                If CStr(m_vColumns(1, lColSub)).ToLower() = m_sTableName.ToLower() & "_id" Then
                    vColWidth(lColSub) = PrinterHelper.Printer.TextWidth("idX")
                Else
                    vColWidth(lColSub) = PrinterHelper.Printer.TextWidth(CStr(m_vColumns(1, lColSub)) & "XXX")
                End If

            Next lColSub

            ' Find the column widths for the data
            With PrinterHelper.Printer
                .FontSize = 10

                PrinterHelper.Printer.Font = VB6.FontChangeName(PrinterHelper.Printer.Font, "Ariel")
                .FontBold = False
                .FontUnderline = False
            End With

            For lColSub As Integer = m_vData.GetLowerBound(0) To m_vData.GetUpperBound(0)

                For lRowSub As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)

                    lColWidth = CInt(PrinterHelper.Printer.TextWidth(CStr(m_vData(lColSub, lRowSub)) & "XXX"))

                    If lColWidth > CDbl(vColWidth(lColSub)) Then
                        vColWidth(lColSub) = lColWidth
                    End If

                Next lRowSub

                ' calculate the total width of the page
                lTotalWidth = CInt(lTotalWidth + CDbl(vColWidth(lColSub)))

            Next lColSub

            ' base the printer orientation on the size of the
            ' document being produced..
            If lTotalWidth > 14455 Then
                PrinterHelper.Printer.Orientation = PrinterHelper.PrinterObjectConstants.vbPRDPVertical
                lRowHeight = CInt(PrinterHelper.Printer.TextHeight("X") * 2)
            Else
                PrinterHelper.Printer.Orientation = PrinterHelper.PrinterObjectConstants.vbPRDPVertical
                lRowHeight = CInt(PrinterHelper.Printer.TextHeight("X") * 1.5)
            End If

            ' Print the report header
            ' RDC 22012001 code moved to new proc to enable printing on each page
            PrintHeader(lXPos, lYPos, lHeaderHeight)

            ' Print the column headers
            ' RDC 22012001 code moved to new proc to enable printing on each page
            PrintTitles(lXPos, lYPos, lHeaderHeight, vColWidth)

            ' Print the data
            With PrinterHelper.Printer
                .FontUnderline = False
            End With

            lYPos = lHeaderHeight


            For lRowSub As Integer = m_vData.GetLowerBound(1) To m_vData.GetUpperBound(1)
                lXPos = 0

                'Printing only non deleted items ... PN 20085
                If CStr(m_vData(ACIsDeleted, lRowSub)) <> "1" Then
                    ' RDC 22012001 check if new page required
                    ' start a new page if it is required

                    PrinterHelper.Printer.CurrentX = lXPos
                    PrinterHelper.Printer.CurrentY = lYPos

                    For lColSub As Integer = m_vData.GetLowerBound(0) To m_vData.GetUpperBound(0)
                        If CStr(m_vColumns(1, lColSub)).ToLower() <> "caption_id" And CBool(CStr(CStr(m_vColumns(1, lColSub)) <> "is_deleted").ToLower()) Then

                            sText = CStr(m_vData(lColSub, lRowSub))
                            ' Justfy numerics to the right
                            If CStr(m_vColumns(2, lColSub)).ToLower().IndexOf("int") >= 0 Then
                                PrinterHelper.Printer.CurrentX = CInt(lXPos + CDbl(vColWidth(lColSub)) - PrinterHelper.Printer.TextWidth(sText & "XXX"))
                            End If

                            PrinterHelper.Printer.Print(sText)
                            lXPos = CInt(lXPos + CDbl(vColWidth(lColSub)))
                            PrinterHelper.Printer.CurrentX = lXPos
                            PrinterHelper.Printer.CurrentY = lYPos

                        End If
                    Next lColSub

                    lYPos += lRowHeight

                End If

            Next lRowSub

            ' End the document
            PrinterHelper.Printer.EndDoc()

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            Select Case Information.Err().Number
                Case 482

                    ' Set the mouse pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to print. Check that there is a printer installed and that it is online.", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Case Else

                    ' Set the mouse pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' End the document
                    PrinterHelper.Printer.EndDoc()

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

            Return result

        End Try
    End Function

    ' RDC 22012001 Cut from PrintDetails to enable printing on each new page
    Private Sub PrintHeader(ByRef lXPos As Integer, ByRef lYPos As Integer, ByRef lHeaderHeight As Integer)

        Static lPage As Integer

        lXPos = 0
        lYPos = 0

        lPage += 1

        ' Set properties for the title
        With PrinterHelper.Printer
            .FontSize = 12

            PrinterHelper.Printer.Font = VB6.FontChangeName(PrinterHelper.Printer.Font, "Ariel")
            .FontBold = True
            .FontUnderline = True
            .CurrentX = lXPos
            .CurrentY = lYPos
        End With

        ' Start the printing
        Dim sText As String = "Table : " & m_sTableName
        PrinterHelper.Printer.Print(sText)
        lXPos = CInt(PrinterHelper.Printer.TextWidth(sText & "XXXXX"))
        PrinterHelper.Printer.CurrentX = lXPos
        PrinterHelper.Printer.CurrentY = lYPos

        sText = "PM Product Code : " & gPMConstants.PMProductCode(m_iProductFamily)
        PrinterHelper.Printer.Print(sText)
        lXPos = CInt(lXPos + PrinterHelper.Printer.TextWidth(sText & "XXXXX"))
        PrinterHelper.Printer.CurrentX = lXPos
        PrinterHelper.Printer.CurrentY = lYPos

        sText = DateTimeHelper.ToString(DateTime.Now)
        PrinterHelper.Printer.Print(sText)
        lXPos = CInt(lXPos + PrinterHelper.Printer.TextWidth(sText & "XXXXX"))
        PrinterHelper.Printer.CurrentX = lXPos
        PrinterHelper.Printer.CurrentY = lYPos

        sText = "Page: " & lPage
        PrinterHelper.Printer.Print(sText)

        lHeaderHeight = CInt(PrinterHelper.Printer.TextHeight("X") * 2)

    End Sub

    Private Sub PrintTitles(ByRef lXPos As Integer, ByRef lYPos As Integer, ByRef lHeaderHeight As Integer, ByRef vColWidth As Array)

        Dim sText As String = ""

        With PrinterHelper.Printer
            .FontSize = 10
            .FontBold = False
            .FontUnderline = True
        End With

        lXPos = 0
        lYPos = lHeaderHeight
        PrinterHelper.Printer.CurrentX = lXPos
        PrinterHelper.Printer.CurrentY = lYPos

        For lColSub As Integer = m_vColumns.GetLowerBound(1) To m_vColumns.GetUpperBound(1)
            If CStr(m_vColumns(1, lColSub)).ToLower() = m_sTableName.ToLower() & "_id" Then
                sText = "id"
            Else
                sText = CStr(m_vColumns(1, lColSub))
            End If

            If sText.ToLower() <> "caption_id" And sText.ToLower() <> "is_deleted" Then

                PrinterHelper.Printer.Print(sText)

                lXPos = CInt(lXPos + CDbl(vColWidth(lColSub)))
                PrinterHelper.Printer.CurrentX = lXPos
                PrinterHelper.Printer.CurrentY = lYPos

            End If

        Next lColSub

        lHeaderHeight = CInt(lHeaderHeight + PrinterHelper.Printer.TextHeight("X") * 2)

    End Sub


    ' ***************************************************************** '
    '
    ' Name: SetKeyEnablement
    '
    ' Description:
    '
    ' History: 01/12/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeyEnablement() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case PrivilegeLevel
                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges
                    cmdAdd.Enabled = True
                    cmdDelete.Enabled = True
                    cmdEdit.Enabled = True
                    cmdView.Enabled = True

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions
                    cmdAdd.Enabled = False
                    cmdDelete.Enabled = False
                    cmdEdit.Enabled = True
                    cmdView.Enabled = True

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly
                    cmdAdd.Enabled = False
                    cmdDelete.Enabled = False
                    cmdEdit.Enabled = False
                    cmdView.Enabled = True

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit
                    cmdAdd.Enabled = False
                    cmdDelete.Enabled = False
                    cmdEdit.Enabled = False
                    cmdView.Enabled = False

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminViewUserNone
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = False
                        cmdView.Enabled = True
                    Else
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = False
                        cmdView.Enabled = False
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserNone
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = True
                        cmdView.Enabled = True
                    Else
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = False
                        cmdView.Enabled = False
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = True
                        cmdView.Enabled = True
                    Else
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = False
                        cmdView.Enabled = True
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserNone
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        cmdAdd.Enabled = True
                        cmdDelete.Enabled = True
                        cmdEdit.Enabled = True
                        cmdView.Enabled = True
                    Else
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = False
                        cmdView.Enabled = False
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        cmdAdd.Enabled = True
                        cmdDelete.Enabled = True
                        cmdEdit.Enabled = True
                        cmdView.Enabled = True
                    Else
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = False
                        cmdView.Enabled = True
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        cmdAdd.Enabled = True
                        cmdDelete.Enabled = True
                        cmdEdit.Enabled = True
                        cmdView.Enabled = True
                    Else
                        cmdAdd.Enabled = False
                        cmdDelete.Enabled = False
                        cmdEdit.Enabled = True
                        cmdView.Enabled = True
                    End If

                Case Else
                    cmdAdd.Enabled = False
                    cmdDelete.Enabled = False
                    cmdEdit.Enabled = False
                    cmdView.Enabled = False

            End Select

            ' RDC 11102002 buttons should be disabled if no items in list
            If lvwTable.Items.Count = 0 Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
                cmdView.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
                cmdView.Enabled = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeyEnablement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeyEnablement", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click

        ' Only print if there's some data.
        If Information.IsArray(m_vData) Then
            m_lReturn = PrintDetails()
        Else
            MessageBox.Show("There is no data to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        'DAK031299
        If Me.Visible Then
            lvwTable.Focus()
        End If

    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        m_lReturn = ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMView)

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            With uctPMResizer

                .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdApply", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdView", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdExport", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdPrint", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("imgIcon", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("lvwTable", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .FormMinHeight = 5205
                .FormMinWidth = 5600

            End With

        End If
    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        '' Developer Guide No. 
        iPMFunc.ShowFormInTaskBar_Attach()

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMMaintainLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMMaintainLookup.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
            'm_lReturn = m_oGeneral.Initialise(frmInterface:=o_frmInterface, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' new instance of frmDetails
            m_frmDetails = New frmDetails()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.


        Dim vOptionValue, vOptionUnderWriting As Object

        iPMFunc.ShowFormInTaskBar_Detach()

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
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

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' Export button
            cmdExport.Visible = Not m_bDisableExport

            ' Print button
            cmdPrint.Visible = Not m_bDisablePrint


            ' {* USER DEFINED CODE (End) *}

            'QBENZ005

            'm_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=CStr(vOptionValue))
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_load", "Failed to get the value for SIROPTEnableRI2007.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" Then
                m_bIsRI2007Enabled = True
            End If

            'To set IsUnderWriting True or False

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionUnderWriting)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_load", "Failed to get the value for SIROPTUnderwriting.", gPMConstants.PMELogLevel.PMLogError)
            End If

            g_bUnderWriting = True

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = m_lReturn

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'DAK011299
            m_lReturn = SetKeyEnablement()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_sUniqueId = GetUniqueID()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            If lvwTable.Items.Count > 0 Then
                lvwTable.Items(0).Selected = True
                lvwTable.Select()
            End If
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Remove instance of frmDetails
            m_frmDetails = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With

            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwTable_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTable.ColumnClick
        ListViewFunc.SortListView(lvwTable, eventArgs)
    End Sub

    Private Sub lvwTable_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTable.DoubleClick
        Dim lId As Integer = Convert.ToString(lvwTable.SelectedItems(0).Tag)
        'DAK061299
        ' If theres a list item selected, then edit or view it
        If lvwTable.Items.Count > 0 Then
            If Not (lvwTable.FocusedItem Is Nothing) Then
                If CDbl(m_vData(ACIsDeleted, lId)) = 1 Then
                    cmdView_Click(cmdView, New EventArgs())
                Else
                    cmdEdit_Click(cmdEdit, New EventArgs())
                End If

            End If
        Else
            ' Otherwise add a new one
            If cmdAdd.Enabled Then
                cmdAdd_Click(cmdAdd, New EventArgs())
            End If

        End If

    End Sub


    Private Sub lvwTable_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTable.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ' Developer Guide No. 74
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        If lvwTable.GetItemAt(x, y) Is Nothing Then
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdView.Enabled = False
        Else
            SetKeyEnablement()
        End If

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (m_frmDetails_LaunchLinkedObject) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Public Sub m_frmDetails_LaunchLinkedObject(ByVal v_vSetKeyArray(,) As Object) Handles m_frmDetails.LaunchLinkedObject

        'DAK031299()
        Dim sComponent As String = LinkedObjectName & "." & LinkedClassName
        RaiseEvent LaunchLinkedObject(sComponent, PMAuthorityLevel, v_vSetKeyArray, m_bSomethingChanged)
        ' m_InterfaceRenamed.m_frmInterface_LaunchLinkedObject(sComponent, PMAuthorityLevel, v_vSetKeyArray)

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)



    ' ***************************************************************** '
    ' Name: CheckLinkedDataMandatory
    '
    ' Description: Gets LinkedDataMandatory flag.
    '
    ' ***************************************************************** '
    Public Function CheckLinkedDataMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get LinkedDataMandatory flag.

            m_lReturn = m_oBusiness.CheckLinkedDataMandatory(v_sTableName:=m_sTableName)



            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                MessageBox.Show("Before exiting lookup maintenance you must fill in the details for" & Strings.Chr(13) & Strings.Chr(10) & _
                                m_sLinkedCaption & " for each lookup entry.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get LinkedDataMandatory flag", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLinkedDataMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    'Start(Sriram P)CacheBug
    Public Function UpdateCache() As Integer
        Dim result As Integer = 0
        Dim v_lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
        Dim v_vSetKeyArray() As Object
        result = gPMConstants.PMEReturnCode.PMTrue

        Const kMethodName As String = "UpdateCache"


        Dim bPMMaintainLookup As Object
        Dim temp_bPMMaintainLookup As Object

        Try

            m_lReturn = g_oObjectManager.GetInstance(temp_bPMMaintainLookup, "bPMMaintainLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            bPMMaintainLookup = temp_bPMMaintainLookup

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of of the business component")
            End If


            m_lReturn = bPMMaintainLookup.UpdateCache(v_sTableName:=m_sTableName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Update the Cache")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function
    'End(Sriram P)CacheBug
    ' Developer Guide No. 11
    'Private Sub lvwTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTable.SelectedIndexChanged

    '    If Not cmdDelete.Enabled Then
    '        Exit Sub
    '    End If

    '    ' Developer Guide No. 11
    '    Dim lId As Integer = Convert.ToString(lvwTable.FocusedItem.Tag)

    '    If CDbl(m_vData(ACIsDeleted, lId)) = 1 Then
    '        cmdDelete.Text = "&Undelete"
    '        cmdDelete.Tag = ACTagDelete
    '        cmdEdit.Enabled = False
    '    Else
    '        cmdDelete.Text = "&Delete"
    '        cmdDelete.Tag = ACTagUndelete
    '        cmdEdit.Enabled = True
    '    End If

    'End Sub

    Private Sub lvwTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTable.Click
        If Not cmdDelete.Enabled Then
            Exit Sub
        End If

        Dim lId As Integer = Convert.ToString(lvwTable.SelectedItems(0).Tag)

        If CDbl(m_vData(ACIsDeleted, lId)) = 1 Then
            cmdDelete.Text = "&Undelete"
            cmdDelete.Tag = ACTagDelete
            cmdEdit.Enabled = False
        Else
            cmdDelete.Text = "&Delete"
            cmdDelete.Tag = ACTagUndelete
            cmdEdit.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' This procedure import the data model by using script file generated from other database. If fails log the error
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdImportMarketplaceDataModel_Click(sender As Object, e As EventArgs) Handles cmdImportMarketplaceDataModel.Click
        Dim nFileResult As Integer = 0
        Dim sFileName As String = String.Empty
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        'open file dialog with filter seletion for sql file only
        With dlgMainOpen
            .DefaultExt = ".sql"
            .Filter = "SQL Files (*.sql)|*.sql"
            ' Show the dialog box
            nFileResult = .ShowDialog
            ' Get the filename
            sFileName = .FileName
        End With
        'check if user cancel then no need to execute further
        If nFileResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Try
            If Not String.IsNullOrEmpty(sFileName) Then
                'call business function to run sql and return data model code
                Dim sDataModelCode As String = String.Empty
                nReturn = m_oBusiness.ImportMarketPlaceDataModel(sFileName:=sFileName, o_sDataModelCode:=sDataModelCode)
                If nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("DataModel " & sDataModelCode & " Imported Successfully", "Data Model Import", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Import Market Place Data Model", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdImportMarketplaceDataModel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdImportMarketplaceDataModel_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdImportMarketplaceDataModel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally
            'clear all objects here
        End Try

    End Sub
End Class
