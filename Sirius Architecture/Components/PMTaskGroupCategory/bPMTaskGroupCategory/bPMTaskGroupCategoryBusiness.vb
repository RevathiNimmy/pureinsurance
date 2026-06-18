Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 4th October 1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMTaskGroupCategory.
    '
    ' Edit History:
    ' DAK041099 - Nicked lock, stock and barrel from PMTaskGroup.
    ' DAK071299 - Set SQL parameters correctly
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of PMTaskGroupCategories (Private)
    Private m_oPMTaskGroupCategories As bPMTaskGroupCategory.Categories

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

    ' Current Task Record Pointer (Private)
    Private m_lTaskCurrentRecord As Integer

    ' Response (Private)
    ' RDC 13062002 gPMLibraries replaced with gPM* BAS modules
    'Private m_lReturn As gPMLibraries.PMEReturnCode
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Caption lookup
    Private m_oCaption As bPMCaption.Business
    'Private m_oCaption As bPMCaption.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property


    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            If Value < 0 Then
                m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            ElseIf (Value > m_oPMTaskGroupCategories.Count()) Then
                m_lCurrentRecord = m_oPMTaskGroupCategories.Count()
            Else
                m_lCurrentRecord = Value
            End If

        End Set
    End Property


    Public Property TaskCurrentRecord() As Integer
        Get

            Return m_lTaskCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            If Value < 0 Then
                m_lTaskCurrentRecord = 0
            ElseIf (Value > m_oPMTaskGroupCategories.Count()) Then
                m_lTaskCurrentRecord = m_oPMTaskGroupCategories.Count()
            Else
                m_lTaskCurrentRecord = Value
            End If

        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            '    Set oComponentServices = New PMServerBusinessCS


            If Information.IsNothing(vDatabase) Then
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase)
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase, _
                'v_vDatabase:=vDatabase)

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't pass a database in, otherwise we'd corrupt the existing one...
            '    lReturn = oComponentServices.CreateBusinessObject(r_oObject:=m_oCaption, _
            'v_sClassName:="bPMCaption.Business", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel)

            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            'create empty users collection
            m_oPMTaskGroupCategories = New bPMTaskGroupCategory.Categories()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oCaption IsNot Nothing Then
                    m_oCaption.Dispose()
                    m_oCaption = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a single PMTaskGroupCategory directly into the database.
    '              Note: The PMTaskGroupCategory will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function Add(ByRef iPMTaskGroupCategoryID As Integer, ByRef sCategoryCode As String, ByRef sCategoryDescription As String, ByRef iIsDeleted As Integer, ByRef dtEffectiveDate As Date, ByRef lLicenceLimit As Integer, ByRef iIsBlockAboveLicenceLimit As Integer, ByRef iIsWarnAboveLicenceLimit As Integer, ByRef lWarnsSinceLicenceUpgrade As Integer) As Integer


        Dim result As Integer = 0
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMTaskGroupCategory
            oPMTaskGroupCategory = New bPMTaskGroupCategory.Category()

            ' Populate PMTaskGroupCategory Attributes
            m_lReturn = CType(SetProperties(oPMTaskGroupCategory:=oPMTaskGroupCategory, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMTaskGroupCategoryId:=iPMTaskGroupCategoryID, vCategoryCode:=sCategoryCode, vCategoryDescription:=sCategoryDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate, vLicenceLimit:=lLicenceLimit, vIsBlockAboveLicenceLimit:=iIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=iIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=lWarnsSinceLicenceUpgrade), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMTaskGroupCategory to the Database
            m_lReturn = CType(AddItem(oPMTaskGroupCategory), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMTaskGroupCategory Added
            iPMTaskGroupCategoryID = oPMTaskGroupCategory.TaskGroupCategoryID

            oPMTaskGroupCategory = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PMTaskGroupCategories and populate the
    ' Collection for the GroupCategoryId if supplied, else returns all of the groups
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vPMTaskGroupCategoryId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            m_lReturn = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Clear the Collection
            m_oPMTaskGroupCategories.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'if the Category Id was supplied

            If Not Information.IsNothing(vPMTaskGroupCategoryId) Then

                ' If the supplied keys are not valid, exit

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vPMTaskGroupCategoryId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vPMTaskGroupCategoryId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_category_id", vValue:=CStr(vPMTaskGroupCategoryId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the effective_date parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSql:=ACGetAllDetailsSQL, ssqlname:=ACGetAllDetailsName, bstoredprocedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' select all of the PMTaskGroupCategory records

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSql:=ACGetDetailsSQL, ssqlname:=ACGetDetailsName, bstoredprocedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New PMTaskGroupCategory
                    oPMTaskGroupCategory = New bPMTaskGroupCategory.Category()

                    m_lReturn = CType(SetPropertiesFromDB(oPMTaskGroupCategory:=oPMTaskGroupCategory, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMTaskGroupCategory to collection
                    m_lReturn = CType(m_oPMTaskGroupCategories.Add(oNewPMTaskGroupCategory:=oPMTaskGroupCategory), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMTaskGroupCategory = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required PMTaskGroupCategory tables and populates
    '              the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPMTaskGroupCategoryId As Object = Nothing, Optional ByRef vCategoryCode As Object = Nothing, Optional ByRef vCategoryDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLicenceLimit As Object = Nothing, Optional ByRef vIsBlockAboveLicenceLimit As Object = Nothing, Optional ByRef vIsWarnAboveLicenceLimit As Object = Nothing, Optional ByRef vWarnsSinceLicenceUpgrade As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMTaskGroupCategories.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMTaskGroupCategory = m_oPMTaskGroupCategories.Item(m_lCurrentRecord)

            ' Get the PMTaskGroupCategory Property Values










            m_lReturn = CType(GetProperties(oPMTaskGroupCategory:=oPMTaskGroupCategory, vStatus:=iStatus, vPMTaskGroupCategoryId:=CInt(vPMTaskGroupCategoryId), vCategoryCode:=CStr(vCategoryCode), vCategoryDescription:=CStr(vCategoryDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=CDate(vEffectiveDate), vLicenceLimit:=CInt(vLicenceLimit), vIsBlockAboveLicenceLimit:=CInt(vIsBlockAboveLicenceLimit), vIsWarnAboveLicenceLimit:=CInt(vIsWarnAboveLicenceLimit), vWarnsSinceLicenceUpgrade:=CInt(vWarnsSinceLicenceUpgrade)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMTaskGroupCategory = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied PMTaskGroupCategory into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPMTaskGroupCategoryId As Object = Nothing, Optional ByRef vCategoryCode As Object = Nothing, Optional ByRef vCategoryDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLicenceLimit As Object = Nothing, Optional ByRef vIsBlockAboveLicenceLimit As Object = Nothing, Optional ByRef vIsWarnAboveLicenceLimit As Object = Nothing, Optional ByRef vWarnsSinceLicenceUpgrade As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMTaskGroupCategories.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMTaskGroupCategory
            oPMTaskGroupCategory = New bPMTaskGroupCategory.Category()

            ' Populate PMTaskGroupCategory Attributes










            m_lReturn = CType(SetProperties(oPMTaskGroupCategory:=oPMTaskGroupCategory, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMTaskGroupCategoryId:=CInt(vPMTaskGroupCategoryId), vCategoryCode:=CStr(vCategoryCode), vCategoryDescription:=CStr(vCategoryDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=CDate(vEffectiveDate), vLicenceLimit:=CInt(vLicenceLimit), vIsBlockAboveLicenceLimit:=CInt(vIsBlockAboveLicenceLimit), vIsWarnAboveLicenceLimit:=CInt(vIsWarnAboveLicenceLimit), vWarnsSinceLicenceUpgrade:=CInt(vWarnsSinceLicenceUpgrade)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMTaskGroupCategory to collection
            m_lReturn = CType(m_oPMTaskGroupCategories.Add(oNewPMTaskGroupCategory:=oPMTaskGroupCategory), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMTaskGroupCategory = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the PMTaskGroupCategory
    '              specified and updates the PMTaskGroupCategory with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPMTaskGroupCategoryId As Object = Nothing, Optional ByRef vCategoryCode As Object = Nothing, Optional ByRef vCategoryDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLicenceLimit As Object = Nothing, Optional ByRef vIsBlockAboveLicenceLimit As Object = Nothing, Optional ByRef vIsWarnAboveLicenceLimit As Object = Nothing, Optional ByRef vWarnsSinceLicenceUpgrade As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTaskGroupCategories.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMTaskGroupCategory = m_oPMTaskGroupCategories.Item(lRow)

            ' Check the Status of the PMTaskGroupCategory

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMTaskGroupCategory.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update PMTaskGroupCategory Attributes










            m_lReturn = CType(SetProperties(oPMTaskGroupCategory:=oPMTaskGroupCategory, iStatus:=iStatus, vPMTaskGroupCategoryId:=CInt(vPMTaskGroupCategoryId), vCategoryCode:=CStr(vCategoryCode), vCategoryDescription:=CStr(vCategoryDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=CDate(vEffectiveDate), vLicenceLimit:=CInt(vLicenceLimit), vIsBlockAboveLicenceLimit:=CInt(vIsBlockAboveLicenceLimit), vIsWarnAboveLicenceLimit:=CInt(vIsWarnAboveLicenceLimit), vWarnsSinceLicenceUpgrade:=CInt(vWarnsSinceLicenceUpgrade)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMTaskGroupCategory
            oPMTaskGroupCategory = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified PMTaskGroupCategory can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTaskGroupCategories.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMTaskGroupCategory = m_oPMTaskGroupCategories.Item(lRow)

            ' Check the Status of the PMTaskGroupCategory

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMTaskGroupCategory.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMTaskGroupCategory.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set deleted status to true
                oPMTaskGroupCategory.IsDeleted = gPMConstants.PMEVarTrueFalse.PMVarTrue

                'set database status to update
                oPMTaskGroupCategory.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Release reference to PMTaskGroupCategory
            oPMTaskGroupCategory = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oPMTaskGroupCategories.Count()
                Select Case m_oPMTaskGroupCategories.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroupCategory As bPMTaskGroupCategory.Category
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMTaskGroupCategories.Count()
                oPMTaskGroupCategory = m_oPMTaskGroupCategories.Item(lSub)


                Select Case oPMTaskGroupCategory.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(AddItem(oPMTaskGroupCategory), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(UpdateItem(oPMTaskGroupCategory), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        'do nothing because in this case the isdeleted flag is set to true
                        'and the row is simply updated

                End Select

            Next lSub

            ' Release last reference
            oPMTaskGroupCategory = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMTaskGroupCategories.Count()
                        m_oPMTaskGroupCategories.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Next lSub

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CountCategoryTasks
    '
    ' Description: Return the number of active tasks for the category.
    '
    ' ***************************************************************** '
    Public Function CountCategoryTasks(ByVal v_lPMTaskGroupCategoryID As Integer, ByRef r_lCategoryTaskCount As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no.112
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="task_group_category_id", vValue:=CStr(v_lPMTaskGroupCategoryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSql:=ACCountCategoryTasksSQL, ssqlname:=ACCountCategoryTasksName, bstoredprocedure:=ACCountCategoryTasksStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any Tasks then just return an empty string
            If m_oDatabase.Records.Count() < 1 Then
                Throw New Exception()
            End If

            oFields = m_oDatabase.Records.Item(1).Fields()

            r_lCategoryTaskCount = oFields("category_task_count")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CountCategoryTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CountCategoryTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category) As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oPMTaskGroupCategory:=oPMTaskGroupCategory), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACAddSQL, ssqlname:=ACAddName, bstoredprocedure:=ACAddStored, lNumberRecords:=lNumberRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPMTaskGroupCategory.TaskGroupCategoryID = m_oDatabase.Records.Item(1).Fields()(1)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oPMTaskGroupCategory:=oPMTaskGroupCategory), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add PMTaskGroupCategoryID as an INPUT param for an update
        'DAK071299
        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_category_id", vValue:=CStr(oPMTaskGroupCategory.TaskGroupCategoryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSql:=ACUpdateSQL, ssqlname:=ACUpdateName, bstoredprocedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If the record was NOT UpdateItemd reselect it to see if the data
        ' has been changed or the record deleted

        If lRecordsAffected > 0 Then

            ' UpdatedItem, No action required

        Else

            result = gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DeleteItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DeleteItem(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRecordsAffected As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear the Database Parameters Collection
    'm_oDatabase.Parameters.Clear()
    '
    ' Add the InsuranceFileID INPUT parameter
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(oPMTaskGroupCategory.TaskGroupCategoryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute SQL Statement
    'm_lReturn = m_oDatabase.SQLAction(sSql:=ACDeleteSQL, ssqlname:=ACDeleteName, bstoredprocedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' If record wasn't deleted, error
    'If lRecordsAffected > 0 Then
    ' Deleted, No action required
    'Else
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMTaskGroupCategory properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No.112
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMTaskGroupCategory

            .TaskGroupCategoryID = oFields("id")
            .CaptionID = oFields("caption_id")
            .CategoryCode = oFields("code")


            m_lReturn = m_oCaption.GetCaptionDesc(v_lCaptionID:=.CaptionID, r_sCaption:=sDescription)
            .CategoryDescription = sDescription

            .IsDeleted = oFields("is_deleted")

            .EffectiveDate = oFields("effective_date")

            .LicenceLimit = oFields("licence_limit")

            .IsBlockAboveLicenceLimit = oFields("is_block_above_licence_limit")

            .IsWarnAboveLicenceLimit = oFields("is_warn_above_licence_limit")

            .WarnsSinceLicenceUpgrade = oFields("warns_since_licence_upgrade")

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMTaskGroupCategory property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category, ByRef iStatus As Integer, Optional ByRef vPMTaskGroupCategoryId As Integer = 0, Optional ByRef vCategoryCode As String = "", Optional ByRef vCategoryDescription As String = "", Optional ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vLicenceLimit As Integer = 0, Optional ByRef vIsBlockAboveLicenceLimit As Integer = 0, Optional ByRef vIsWarnAboveLicenceLimit As Integer = 0, Optional ByRef vWarnsSinceLicenceUpgrade As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            m_lReturn = CType(MandatoryParameterCheck(vPMTaskGroupCategoryId:=vPMTaskGroupCategoryId, vCategoryCode:=vCategoryCode, vCategoryDescription:=vCategoryDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vLicenceLimit:=vLicenceLimit, vIsBlockAboveLicenceLimit:=vIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=vIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=vWarnsSinceLicenceUpgrade), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'supply defaults for any missing parameters
            m_lReturn = CType(DefaultMissingParameters(vPMTaskGroupCategoryId:=vPMTaskGroupCategoryId, vCategoryCode:=vCategoryCode, vCategoryDescription:=vCategoryDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vLicenceLimit:=vLicenceLimit, vIsBlockAboveLicenceLimit:=vIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=vIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=vWarnsSinceLicenceUpgrade), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check whether the values in the parameters are valid
        m_lReturn = CType(ValidateParameters(vPMTaskGroupCategoryId:=vPMTaskGroupCategoryId, vCategoryCode:=vCategoryCode, vCategoryDescription:=vCategoryDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vLicenceLimit:=vLicenceLimit, vIsBlockAboveLicenceLimit:=vIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=vIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=vWarnsSinceLicenceUpgrade), gPMConstants.PMEReturnCode)

        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Quick, let's go get the caption id

        If Not Information.IsNothing(vCategoryDescription) Then
            sDescription = vCategoryDescription


            m_lReturn = m_oCaption.GetCaptionID(v_scaption:=sDescription, r_lcaptionid:=lCaptionID)
        End If

        ' Set Property values.
        With oPMTaskGroupCategory

            If Not Information.IsNothing(vPMTaskGroupCategoryId) Then
                .TaskGroupCategoryID = vPMTaskGroupCategoryId
            End If

            If Not Information.IsNothing(vCategoryCode) Then
                .CategoryCode = vCategoryCode
            End If

            If Not Information.IsNothing(vCategoryDescription) Then
                .CategoryDescription = vCategoryDescription
                .CaptionID = lCaptionID
            End If

            If Not Information.IsNothing(vIsDeleted) Then
                .IsDeleted = vIsDeleted
            End If

            If Not Information.IsNothing(vEffectiveDate) Then
                .EffectiveDate = vEffectiveDate
            End If

            If Not Information.IsNothing(vLicenceLimit) Then
                .LicenceLimit = vLicenceLimit
            End If

            If Not Information.IsNothing(vIsBlockAboveLicenceLimit) Then
                .IsBlockAboveLicenceLimit = vIsBlockAboveLicenceLimit
            End If

            If Not Information.IsNothing(vIsWarnAboveLicenceLimit) Then
                .IsWarnAboveLicenceLimit = vIsWarnAboveLicenceLimit
            End If

            If Not Information.IsNothing(vWarnsSinceLicenceUpgrade) Then
                .WarnsSinceLicenceUpgrade = vWarnsSinceLicenceUpgrade
            End If

            .DatabaseStatus = iStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMTaskGroupCategory property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category, ByRef vStatus As gPMConstants.PMEComponentAction, Optional ByRef vPMTaskGroupCategoryId As Integer = 0, Optional ByRef vCategoryCode As String = "", Optional ByRef vCategoryDescription As String = "", Optional ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vLicenceLimit As Integer = 0, Optional ByRef vIsBlockAboveLicenceLimit As Integer = 0, Optional ByRef vIsWarnAboveLicenceLimit As Integer = 0, Optional ByRef vWarnsSinceLicenceUpgrade As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMTaskGroupCategory


            If Not Information.IsNothing(vPMTaskGroupCategoryId) Then
                vPMTaskGroupCategoryId = .TaskGroupCategoryID
            End If


            If Not Information.IsNothing(vCategoryCode) Then
                vCategoryCode = .CategoryCode
            End If


            If Not Information.IsNothing(vCategoryDescription) Then
                vCategoryDescription = .CategoryDescription
            End If


            If Not Information.IsNothing(vIsDeleted) Then
                vIsDeleted = .IsDeleted
            End If


            If Not Information.IsNothing(vEffectiveDate) Then
                vEffectiveDate = .EffectiveDate
            End If


            If Not Information.IsNothing(vLicenceLimit) Then
                vLicenceLimit = .LicenceLimit
            End If


            If Not Information.IsNothing(vIsBlockAboveLicenceLimit) Then
                vIsBlockAboveLicenceLimit = .IsBlockAboveLicenceLimit
            End If


            If Not Information.IsNothing(vIsWarnAboveLicenceLimit) Then
                vIsWarnAboveLicenceLimit = .IsWarnAboveLicenceLimit
            End If


            If Not Information.IsNothing(vWarnsSinceLicenceUpgrade) Then
                vWarnsSinceLicenceUpgrade = .WarnsSinceLicenceUpgrade
            End If

            If True Then
                vStatus = .DatabaseStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oPMTaskGroupCategory As bPMTaskGroupCategory.Category) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            sCaption = oPMTaskGroupCategory.CategoryDescription


            m_lReturn = m_oCaption.GetCaptionID(v_scaption:=sCaption, r_lcaptionid:=lCaptionID)

            m_lReturn = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oPMTaskGroupCategory.CategoryCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oPMTaskGroupCategory.CategoryDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oPMTaskGroupCategory.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(oPMTaskGroupCategory.EffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="licence_limit", vValue:=CStr(oPMTaskGroupCategory.LicenceLimit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_block_above_licence_limit", vValue:=CStr(oPMTaskGroupCategory.IsBlockAboveLicenceLimit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_warn_above_licence_limit", vValue:=CStr(oPMTaskGroupCategory.IsWarnAboveLicenceLimit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK071299
            m_lReturn = .Parameters.Add(sName:="warns_since_licence_upgrade", vValue:=CStr(oPMTaskGroupCategory.WarnsSinceLicenceUpgrade), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParameters (Private)
    '
    ' Description: Checks that all paramaters are valid for the datatype
    '
    ' ***************************************************************** '
    Private Function ValidateParameters(Optional ByRef vPMTaskGroupCategoryId As Object = Nothing, Optional ByRef vCategoryCode As Object = Nothing, Optional ByRef vCategoryDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLicenceLimit As Object = Nothing, Optional ByRef vIsBlockAboveLicenceLimit As Object = Nothing, Optional ByRef vIsWarnAboveLicenceLimit As Object = Nothing, Optional ByRef vWarnsSinceLicenceUpgrade As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Information.IsNothing(vPMTaskGroupCategoryId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPMTaskGroupCategoryId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPMTaskGroupCategoryId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vCategoryCode) Then

        End If


        If Not Information.IsNothing(vCategoryDescription) Then

        End If


        If Not Information.IsNothing(vIsDeleted) Then
            If vIsDeleted <> 0 And vIsDeleted <> 1 Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vIsDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vEffectiveDate) Then
            If Not Information.IsDate(vEffectiveDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vLicenceLimit) Then

        End If


        If Not Information.IsNothing(vIsBlockAboveLicenceLimit) Then

        End If


        If Not Information.IsNothing(vIsWarnAboveLicenceLimit) Then

        End If


        If Not Information.IsNothing(vWarnsSinceLicenceUpgrade) Then

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    '
    ' ***************************************************************** '
    Private Function DefaultMissingParameters(Optional ByRef vPMTaskGroupCategoryId As Byte = 0, Optional ByRef vCategoryCode As Object = Nothing, Optional ByRef vCategoryDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vLicenceLimit As Byte = 0, Optional ByRef vIsBlockAboveLicenceLimit As gPMConstants.PMEReturnCode = 0, Optional ByRef vIsWarnAboveLicenceLimit As gPMConstants.PMEReturnCode = 0, Optional ByRef vWarnsSinceLicenceUpgrade As Byte = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'set defaults for any properties which have not been supplied

        If Information.IsNothing(vPMTaskGroupCategoryId) Then
            vPMTaskGroupCategoryId = 1
        End If


        If Information.IsNothing(vIsDeleted) Then
            vIsDeleted = 0
        End If


        If Information.IsNothing(vEffectiveDate) Then
            vEffectiveDate = DateTime.Now
        End If


        If Information.IsNothing(vLicenceLimit) Then
            vLicenceLimit = 0
        End If


        If Information.IsNothing(vIsBlockAboveLicenceLimit) Then
            vIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
        End If


        If Information.IsNothing(vIsWarnAboveLicenceLimit) Then
            vIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
        End If


        If Information.IsNothing(vWarnsSinceLicenceUpgrade) Then
            vWarnsSinceLicenceUpgrade = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vPMTaskGroupCategoryId As Object = Nothing, Optional ByRef vCategoryCode As Object = Nothing, Optional ByRef vCategoryDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vLicenceLimit As Object = Nothing, Optional ByRef vIsBlockAboveLicenceLimit As Object = Nothing, Optional ByRef vIsWarnAboveLicenceLimit As Object = Nothing, Optional ByRef vWarnsSinceLicenceUpgrade As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Information.IsNothing(vCategoryCode) Or Information.IsNothing(vCategoryDescription) Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property Was Not Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="MandatoryParameterCheck", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
