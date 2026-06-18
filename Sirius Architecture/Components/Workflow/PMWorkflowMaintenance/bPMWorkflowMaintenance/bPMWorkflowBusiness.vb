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
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 22/01/2003
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMPackage.
    '
    ' Edit History:
    '               AMB 22/01/2003 - Shamelessly ripped from the Task Group components
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

    ' PRIVATE Data Members (Begin)

    ' Collection of PMPackages (Private)
    Private m_oPMPackages As bPMWorkflowMaintenance.PMPackages

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

    ' Current Task Record Pointer (Private)
    Private m_lTaskCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' RDC 17102002
    Private m_lReturn As Integer

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
            ElseIf (Value > m_oPMPackages.Count()) Then
                m_lCurrentRecord = m_oPMPackages.Count()
            Else
                m_lCurrentRecord = Value
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




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


            If Information.IsNothing(vDatabase) Then
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't pass a database in, otherwise we'd corrupt the existing one...

            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            'create empty users collection
            m_oPMPackages = New bPMWorkflowMaintenance.PMPackages()

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
    ' Description: Adds a single PMPackage directly into the database.
    '              Note: The PMPackage will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function Add(ByRef iPMPackageID As Integer, ByRef sGroupCode As String, ByRef sGroupDescription As String, ByRef iIsDeleted As Integer, ByRef dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMPackage
            oPMPackage = New bPMWorkflowMaintenance.PMPackage()

            ' Populate PMPackage Attributes
            m_lError = CType(SetProperties(oPMPackage:=oPMPackage, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMPackageId:=iPMPackageID, vPackageCode:=sGroupCode, vPackageDescription:=sGroupDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMPackage to the Database
            m_lError = CType(AddItem(oPMPackage), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMPackage Added
            iPMPackageID = oPMPackage.PackageID

            oPMPackage = Nothing

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
    ' Description: Gets the required PMPackages and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vPMPackageId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            m_lError = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMPackages.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'if the userid was supplied

            If Not Information.IsNothing(vPMPackageId) Then

                ' If the supplied keys are not valid, exit

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vPMPackageId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vPMPackageId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_workflow_id", vValue:=CStr(vPMPackageId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' select all of the PMPackage records

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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

                    ' Create New PMPackage
                    oPMPackage = New bPMWorkflowMaintenance.PMPackage()

                    m_lError = CType(SetPropertiesFromDB(oPMPackage:=oPMPackage, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMPackage to collection
                    m_lError = CType(m_oPMPackages.Add(oNewPMPackage:=oPMPackage), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMPackage = Nothing

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
    ' Name: GetDetailsByCode (Public)
    '
    ' Description: Gets the required PMPackages and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetailsByCode(Optional ByRef vPMPackageCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            'if the userid was supplied


            If Not Information.IsNothing(vPMPackageCode) Then

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_workflow_code", vValue:=CStr(vPMPackageCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsByCodeSQL, sSQLName:=ACGetDetailsByCodeName, bStoredProcedure:=ACGetDetailsByCodeStored, lNumberRecords:=0)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oDatabase.Records.Count() Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            Else
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

        Catch
        End Try



        ' Error.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsByCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsByCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required PMPackages and populate the Collection

    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPMPackageId As Object = Nothing, Optional ByRef vPackageCode As Object = Nothing, Optional ByRef vPackageDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMPackages.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMPackage = m_oPMPackages.Item(m_lCurrentRecord)

            ' Get the PMPackage Property Values






            m_lError = CType(GetProperties(oPMPackage:=oPMPackage, vStatus:=iStatus, vPMPackageId:=CInt(vPMPackageId), vPackageCode:=CStr(vPackageCode), vPackageDescription:=CStr(vPackageDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=CDate(vEffectiveDate)), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMPackage = Nothing

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
    ' Description: Adds the supplied PMPackage into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.

    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPMPackageId As Object = Nothing, Optional ByRef vPackageCode As Object = Nothing, Optional ByRef vPackageDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMPackages.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMPackage
            oPMPackage = New bPMWorkflowMaintenance.PMPackage()

            ' Populate PMPackage Attributes






            m_lError = CType(SetProperties(oPMPackage:=oPMPackage, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMPackageId:=CInt(vPMPackageId), vPackageCode:=CStr(vPackageCode), vPackageDescription:=CStr(vPackageDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=CDate(vEffectiveDate)), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMPackage to collection
            m_lError = CType(m_oPMPackages.Add(oNewPMPackage:=oPMPackage), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMPackage = Nothing

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
    ' Description: Validates that this action is valid on the PMPackage
    '              specified and updates the PMPackage with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPackageCode As Object = Nothing, Optional ByRef vPMPackageId As Object = Nothing, Optional ByRef vPackageDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMPackages.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMPackage = m_oPMPackages.Item(lRow)

            ' Check the Status of the PMPackage

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMPackage.DatabaseStatus
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

            ' Update PMPackage Attributes






            m_lError = CType(SetProperties(oPMPackage:=oPMPackage, iStatus:=iStatus, vPMPackageId:=CInt(vPMPackageId), vPackageCode:=CStr(vPackageCode), vPackageDescription:=CStr(vPackageDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=CDate(vEffectiveDate)), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMPackage
            oPMPackage = Nothing

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
    ' Description: Validate that the specified PMPackage can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMPackages.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMPackage = m_oPMPackages.Item(lRow)

            ' Check the Status of the PMPackage

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMPackage.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMPackage.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set deleted status to true
                oPMPackage.IsDeleted = gPMConstants.PMEVarTrueFalse.PMVarTrue

                'set database status to update
                oPMPackage.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Release reference to PMPackage
            oPMPackage = Nothing

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
    ' Name: EditIgnore (Public)
    '
    ' Description: Validate that the specified PMPackage can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditIgnore(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMPackages.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMPackage = m_oPMPackages.Item(lRow)

            'set database status to update
            oPMPackage.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMPackage
            oPMPackage = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditIgnore Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditIgnore", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditInclude (Public)
    '
    ' Description: Validate that the specified PMPackage can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditInclude(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMPackages.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMPackage = m_oPMPackages.Item(lRow)

            'set database status to update
            oPMPackage.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMPackage
            oPMPackage = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditInclude Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditInclude", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oPMPackages.Count()
                Select Case m_oPMPackages.Item(lSub).DatabaseStatus
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
        Dim oPMPackage As bPMWorkflowMaintenance.PMPackage
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMPackages.Count()
                oPMPackage = m_oPMPackages.Item(lSub)


                Select Case oPMPackage.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lError = CType(AddItem(oPMPackage), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lError = CType(UpdateItem(oPMPackage), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        'do nothing because in this case the isdeleted flag is set to true
                        'and the row is simply updated

                End Select

            Next lSub

            ' Release last reference
            oPMPackage = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMPackages.Count()
                        m_oPMPackages.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Next lSub

                Else

                    m_lError = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage) As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PMPackageID as an INPUT param for an update
        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_workflow_id", vValue:=CStr(oPMPackage.PackageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lError = CType(AddInputParam(oPMPackage:=oPMPackage), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLSelect(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lNumberRecords:=lNumberRecords)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ''''oPMPackage.PackageID = m_oDatabase.Records.Item(1).Fields.Item("pmwrk_workflow_id").Value

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PMPackageID as an INPUT param for an update
        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_workflow_id", vValue:=CStr(oPMPackage.PackageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lError = CType(AddInputParam(oPMPackage:=oPMPackage), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'Private Function DeleteItem(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage) As Integer
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
    'm_lError = m_oDatabase.Parameters.Add(sName:="pmuser_workflow_id", vValue:=CStr(oPMPackage.PackageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute SQL Statement
    'm_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Description: Sets the supplied PMPackage properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMPackage

            .PackageID = oFields("pmwrk_workflow_id")
            .CaptionID = oFields("caption_id")
            .PackageCode = oFields("code")


            m_lError = m_oCaption.GetCaptionDesc(v_lCaptionID:=.CaptionID, r_sCaption:=sDescription)
            .PackageDescription = sDescription


            If Convert.IsDBNull(oFields("is_deleted")) Or IsNothing(oFields("is_deleted")) Then
                .IsDeleted = 0
            Else
                .IsDeleted = oFields("is_deleted")
            End If


            If Convert.IsDBNull(oFields("effective_date")) Or IsNothing(oFields("effective_date")) Then
                .EffectiveDate = DateTime.Now
            Else
                .EffectiveDate = oFields("effective_date")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMPackage property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage, ByRef iStatus As Integer, Optional ByRef vPMPackageId As Integer = 0, Optional ByRef vPackageCode As String = "", Optional ByRef vPackageDescription As String = "", Optional ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            m_lError = CType(MandatoryParameterCheck(vPMPackageId:=vPMPackageId, vPackageCode:=vPackageCode, vPackageDescription:=vPackageDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'supply defaults for any missing parameters
            m_lError = CType(DefaultMissingParameters(vPMPackageId:=vPMPackageId, vPackageCode:=vPackageCode, vPackageDescription:=vPackageDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check whether the values in the parameters are valid
        m_lError = CType(ValidateParameters(vPMPackageId:=vPMPackageId, vPackageCode:=vPackageCode, vPackageDescription:=vPackageDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)

        If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Quick, let's go get the caption id

        If Not Information.IsNothing(vPackageDescription) Then
            sDescription = vPackageDescription


            m_lError = m_oCaption.GetCaptionID(v_sCaption:=sDescription, r_lCaptionID:=lCaptionID)
        End If

        ' Set Property values.
        With oPMPackage

            If Not Information.IsNothing(vPMPackageId) Then
                .PackageID = vPMPackageId
            End If

            If Not Information.IsNothing(vPackageCode) Then
                .PackageCode = vPackageCode
            End If

            If Not Information.IsNothing(vPackageDescription) Then
                .PackageDescription = vPackageDescription
                .CaptionID = lCaptionID
            End If

            If Not Information.IsNothing(vIsDeleted) Then
                .IsDeleted = vIsDeleted
            End If

            If Not Information.IsNothing(vEffectiveDate) Then
                .EffectiveDate = vEffectiveDate
            End If

            .DatabaseStatus = iStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMPackage property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage, ByRef vStatus As gPMConstants.PMEComponentAction, ByRef vPMPackageId As Integer, ByRef vPackageCode As String, ByRef vPackageDescription As String, ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse, ByRef vEffectiveDate As Date) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMPackage

            If True Then
                vPMPackageId = .PackageID
            End If

            If True Then
                vPackageCode = .PackageCode
            End If

            If True Then
                vPackageDescription = .PackageDescription
            End If

            If True Then
                vIsDeleted = .IsDeleted
            End If

            If True Then
                vEffectiveDate = .EffectiveDate
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
    Private Function AddInputParam(ByRef oPMPackage As bPMWorkflowMaintenance.PMPackage) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            sCaption = oPMPackage.PackageDescription


            m_lError = m_oCaption.GetCaptionID(v_sCaption:=sCaption, r_lCaptionID:=lCaptionID)

            m_lError = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="code", vValue:=oPMPackage.PackageCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="description", vValue:=oPMPackage.PackageDescription, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oPMPackage.IsDeleted), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(oPMPackage.EffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Private Function ValidateParameters(Optional ByRef vPMPackageId As Object = Nothing, Optional ByRef vPackageCode As Object = Nothing, Optional ByRef vPackageDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Information.IsNothing(vPMPackageId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPMPackageId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPMPackageId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vPackageCode) Then

        End If


        If Not Information.IsNothing(vPackageDescription) Then

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


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    ' ***************************************************************** '
    Private Function DefaultMissingParameters(Optional ByRef vPMPackageId As Byte = 0, Optional ByRef vPackageCode As Object = Nothing, Optional ByRef vPackageDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'set defaults for any properties which have not been supplied

        If Information.IsNothing(vPMPackageId) Then
            vPMPackageId = 1
        End If


        If Information.IsNothing(vIsDeleted) Then
            vIsDeleted = 0
        End If


        If Information.IsNothing(vEffectiveDate) Then
            vEffectiveDate = DateTime.Now
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vPMPackageId As Object = Nothing, Optional ByRef vPackageCode As Object = Nothing, Optional ByRef vPackageDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Information.IsNothing(vPackageCode) Or Information.IsNothing(vPackageDescription) Then

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
            m_lError = m_oDatabase.SQLBeginTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
            m_lError = m_oDatabase.SQLCommitTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
            m_lError = m_oDatabase.SQLRollbackTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
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
