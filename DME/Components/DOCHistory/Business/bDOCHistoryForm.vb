Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 16/01/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a History.
    '
    ' Edit History:
    '
    ' DN270802 - Change embedded SQL to reflect table changes
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    'Store whether or not we are updating the history database
    Private m_bUpdateHDB As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '
            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
    End Property



    Public Property UpdateHDB() As Boolean
        Get

            Return m_bUpdateHDB


        End Get
        Set(ByVal Value As Boolean)

            m_bUpdateHDB = Value

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername = sUsername
            g_sPassword.Value = sPassword

            ' Set UserID
            g_iUserID = iUserID

            ' Set Calling Application
            g_sCallingAppName = sCallingAppName

            ' Set Language ID
            g_iLanguageID = iLanguageID

            ' Set Source ID
            g_iSourceID = iSourceID

            ' Set Currency ID
            g_iCurrencyID = iCurrencyID

            ' Set Log Level
            g_iLogLevel = iLogLevel

            ' Have we a valid Database Object Reference?

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New dPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = gPMComponentServices.NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            'Check if we are actually updating the history DB
            m_lReturn = UpdateHDBCheck()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single History directly into the database.
    '        Note: The History will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vHistoryID As Integer = 0, Optional ByRef vTask As Object = Nothing, Optional ByRef vCabinetcode As Object = Nothing, Optional ByRef vCabinetname As Object = Nothing, Optional ByRef vDrawercode As Object = Nothing, Optional ByRef vDrawername As Object = Nothing, Optional ByRef vFoldercode As Object = Nothing, Optional ByRef vFoldername As Object = Nothing, Optional ByRef vDocref As Object = Nothing, Optional ByRef vRequestDate As Object = Nothing, Optional ByRef vRequestTime As Object = Nothing, Optional ByRef vEventtype As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vVolume As Object = Nothing, Optional ByRef vPagefile As Object = Nothing, Optional ByRef vDoctype As Object = Nothing, Optional ByRef vFiller As Object = Nothing, Optional ByRef vHderror As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vProcessed As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oHistory As bDOCHistory.History

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If we are not actually updating the history database, then go.
            If Not m_bUpdateHDB Then
                Return result
            End If

            ' Create a new History
            oHistory = New bDOCHistory.History()

            ' Populate History Attributes



















            'Developer Guie No 98
            'm_lReturn = SetProperties(oHistory, gPMConstants.PMEComponentAction.PMAdd, vHistoryID:=vHistoryID, vTask:=CInt(vTask), vCabinetcode:=CStr(vCabinetcode), vCabinetname:=CStr(vCabinetname), vDrawercode:=CStr(vDrawercode), vDrawername:=CStr(vDrawername), vFoldercode:=CStr(vFoldercode), vFoldername:=CStr(vFoldername), vDocref:=CStr(vDocref), vRequestDate:=CStr(vRequestDate), vRequestTime:=CStr(vRequestTime), vEventtype:=CStr(vEventtype), vDescription:=CStr(vDescription), vVolume:=CStr(vVolume), vPagefile:=CStr(vPagefile), vDoctype:=CStr(vDoctype), vFiller:=CStr(vFiller), vHderror:=CStr(vHderror), vCreateDate:=CDate(vCreateDate), vProcessed:=CStr(vProcessed))
            m_lReturn = SetProperties(oHistory, gPMConstants.PMEComponentAction.PMAdd, vHistoryID:=vHistoryID, vTask:=vTask, vCabinetcode:=vCabinetcode, vCabinetname:=vCabinetname, vDrawercode:=vDrawercode, vDrawername:=vDrawername, vFoldercode:=vFoldercode, vFoldername:=vFoldername, vDocref:=vDocref, vRequestDate:=vRequestDate, vRequestTime:=vRequestTime, vEventtype:=vEventtype, vDescription:=vDescription, vVolume:=vVolume, vPagefile:=vPagefile, vDoctype:=vDoctype, vFiller:=vFiller, vHderror:=vHderror, vCreateDate:=vCreateDate, vProcessed:=vProcessed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the History to the Database
            m_lReturn = AddItem(oHistory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the History Added

            If Not Informations.IsNothing(vHistoryID) Then
                vHistoryID = oHistory.HistoryID
            End If

            ' {* USER DEFINED CODE (End) *}

            oHistory = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oHistory As bDOCHistory.History) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oHistory:=oHistory)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add HistoryID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="History_id", vValue:=oHistory.HistoryID, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamOutput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oHistory.HistoryID = m_oDatabase.Parameters.Item("History_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied History property values.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Private Function SetProperties(ByRef oHistory As bDOCHistory.History, ByRef iStatus As Integer, Optional ByRef vHistoryID As Integer = 0, Optional ByRef vTask As Integer = 0, Optional ByRef vCabinetcode As String = "", Optional ByRef vCabinetname As String = "", Optional ByRef vDrawercode As String = "", Optional ByRef vDrawername As String = "", Optional ByRef vFoldercode As String = "", Optional ByRef vFoldername As String = "", Optional ByRef vDocref As String = "", Optional ByRef vRequestDate As String = "", Optional ByRef vRequestTime As String = "", Optional ByRef vEventtype As String = "", Optional ByRef vDescription As String = "", Optional ByRef vVolume As String = "", Optional ByRef vPagefile As String = "", Optional ByRef vDoctype As String = "", Optional ByRef vFiller As String = "", Optional ByRef vHderror As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vProcessed As String = "") As Integer
    Private Function SetProperties(ByRef oHistory As bDOCHistory.History, ByRef iStatus As Integer, Optional ByRef vHistoryID As Object = Nothing, Optional ByRef vTask As Object = Nothing, Optional ByRef vCabinetcode As Object = Nothing, Optional ByRef vCabinetname As Object = Nothing, Optional ByRef vDrawercode As Object = Nothing, Optional ByRef vDrawername As Object = Nothing, Optional ByRef vFoldercode As Object = Nothing, Optional ByRef vFoldername As Object = Nothing, Optional ByRef vDocref As Object = Nothing, Optional ByRef vRequestDate As Object = Nothing, Optional ByRef vRequestTime As Object = Nothing, Optional ByRef vEventtype As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vVolume As Object = Nothing, Optional ByRef vPagefile As Object = Nothing, Optional ByRef vDoctype As Object = Nothing, Optional ByRef vFiller As Object = Nothing, Optional ByRef vHderror As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vProcessed As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vHistoryID:=vHistoryID, vTask:=vTask, vCabinetcode:=vCabinetcode, vCabinetname:=vCabinetname, vDrawercode:=vDrawercode, vDrawername:=vDrawername, vFoldercode:=vFoldercode, vFoldername:=vFoldername, vDocref:=vDocref, vRequestDate:=vRequestDate, vRequestTime:=vRequestTime, vEventtype:=vEventtype, vDescription:=vDescription, vVolume:=vVolume, vPagefile:=vPagefile, vDoctype:=vDoctype, vFiller:=vFiller, vHderror:=vHderror, vCreateDate:=vCreateDate, vProcessed:=vProcessed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vHistoryID:=vHistoryID, vTask:=vTask, vCabinetcode:=vCabinetcode, vCabinetname:=vCabinetname, vDrawercode:=vDrawercode, vDrawername:=vDrawername, vFoldercode:=vFoldercode, vFoldername:=vFoldername, vDocref:=vDocref, vRequestDate:=vRequestDate, vRequestTime:=vRequestTime, vEventtype:=vEventtype, vDescription:=vDescription, vVolume:=vVolume, vPagefile:=vPagefile, vDoctype:=vDoctype, vFiller:=vFiller, vHderror:=vHderror, vCreateDate:=vCreateDate, vProcessed:=vProcessed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to set a default parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vHistoryID:=vHistoryID, vTask:=vTask, vCabinetcode:=vCabinetcode, vCabinetname:=vCabinetname, vDrawercode:=vDrawercode, vDrawername:=vDrawername, vFoldercode:=vFoldercode, vFoldername:=vFoldername, vDocref:=vDocref, vRequestDate:=vRequestDate, vRequestTime:=vRequestTime, vEventtype:=vEventtype, vDescription:=vDescription, vVolume:=vVolume, vPagefile:=vPagefile, vDoctype:=vDoctype, vFiller:=vFiller, vHderror:=vHderror, vCreateDate:=vCreateDate, vProcessed:=vProcessed)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to validate a parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oHistory


            If Not Informations.IsNothing(vHistoryID) Then
                If .HistoryID <> vHistoryID Then
                    .HistoryID = vHistoryID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vTask) Then
                If .Task <> vTask Then
                    .Task = vTask
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCabinetcode) Then
                If ToSafeString(.Cabinetcode).Trim() <> Convert.ToString(vCabinetcode).Trim() Then
                    .Cabinetcode = vCabinetcode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCabinetname) Then
                If ToSafeString(.Cabinetname).Trim() <> vCabinetname.Trim() Then
                    .Cabinetname = vCabinetname
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDrawercode) Then
                If ToSafeString(.Drawercode).Trim() <> Convert.ToString(vDrawercode).Trim() Then
                    .Drawercode = vDrawercode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDrawername) Then
                If ToSafeString(.Drawername).Trim() <> vDrawername.Trim() Then
                    .Drawername = vDrawername
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFoldercode) Then
                If ToSafeString(.Foldercode).Trim() <> Convert.ToString(vFoldercode).Trim() Then
                    .Foldercode = vFoldercode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFoldername) Then
                If ToSafeString(.Foldername).Trim() <> vFoldername.Trim() Then
                    .Foldername = vFoldername
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDocref) Then
                If ToSafeString(.Docref).Trim() <> vDocref.Trim() Then
                    .Docref = vDocref
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRequestDate) Then
                If ToSafeString(.RequestDate).Trim() <> vRequestDate.Trim() Then
                    .RequestDate = vRequestDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRequestTime) Then
                If ToSafeString(.RequestTime).Trim() <> vRequestTime.Trim() Then
                    .RequestTime = vRequestTime
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vEventtype) Then
                If ToSafeString(.Eventtype).Trim() <> vEventtype.Trim() Then
                    .Eventtype = vEventtype
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDescription) Then
                If ToSafeString(.Description).Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vVolume) Then
                If ToSafeString(.Volume).Trim() <> vVolume.Trim() Then
                    .Volume = vVolume
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPagefile) Then
                If ToSafeString(.Pagefile).Trim() <> vPagefile.Trim() Then
                    .Pagefile = vPagefile
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDoctype) Then
                If ToSafeString(.Doctype).Trim() <> vDoctype.Trim() Then
                    .Doctype = vDoctype
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFiller) Then
                If ToSafeString(.Filler).Trim() <> vFiller.Trim() Then
                    .Filler = vFiller
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vHderror) Then
                If ToSafeString(.Hderror).Trim() <> vHderror.Trim() Then
                    .Hderror = vHderror
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCreateDate) Then
                If .CreateDate <> vCreateDate Then
                    .CreateDate = vCreateDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vProcessed) Then
                If ToSafeString(.Processed).Trim() <> vProcessed.Trim() Then
                    .Processed = vProcessed
                    bDataChanged = True
                End If
            End If


            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
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
    Private Function AddInputParam(ByRef oHistory As bDOCHistory.History) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="task", vValue:=oHistory.Task, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMInteger))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cabinetcode", vValue:=oHistory.Cabinetcode, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cabinetname", vValue:=oHistory.Cabinetname, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="drawercode", vValue:=oHistory.Drawercode, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="drawername", vValue:=oHistory.Drawername, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="foldercode", vValue:=oHistory.Foldercode, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="foldername", vValue:=oHistory.Foldername, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="docref", vValue:=oHistory.Docref, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="request_date", vValue:=oHistory.RequestDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="request_time", vValue:=oHistory.RequestTime, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="eventtype", vValue:=oHistory.Eventtype, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oHistory.Description, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="volume", vValue:=oHistory.Volume, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="pagefile", vValue:=oHistory.Pagefile, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="doctype", vValue:=oHistory.Doctype, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="filler", vValue:=oHistory.Filler, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="hderror", vValue:=oHistory.Hderror, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=oHistory.CreateDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="processed", vValue:=oHistory.Processed, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a History.
    '
    ' ***************************************************************** '
    'Developer Guie No 101
    'Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vHistoryID As Object = Nothing, Optional ByRef vTask As Byte = 0, Optional ByRef vCabinetcode As String = "", Optional ByRef vCabinetname As String = "", Optional ByRef vDrawercode As String = "", Optional ByRef vDrawername As String = "", Optional ByRef vFoldercode As String = "", Optional ByRef vFoldername As String = "", Optional ByRef vDocref As String = "", Optional ByRef vRequestDate As String = "", Optional ByRef vRequestTime As String = "", Optional ByRef vEventtype As String = "", Optional ByRef vDescription As String = "", Optional ByRef vVolume As String = "", Optional ByRef vPagefile As String = "", Optional ByRef vDoctype As String = "", Optional ByRef vFiller As String = "", Optional ByRef vHderror As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vProcessed As String = "") As Integer
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vHistoryID As Object = Nothing, Optional ByRef vTask As Object = Nothing, Optional ByRef vCabinetcode As Object = Nothing, Optional ByRef vCabinetname As Object = Nothing, Optional ByRef vDrawercode As Object = Nothing, Optional ByRef vDrawername As Object = Nothing, Optional ByRef vFoldercode As Object = Nothing, Optional ByRef vFoldername As Object = Nothing, Optional ByRef vDocref As Object = Nothing, Optional ByRef vRequestDate As Object = Nothing, Optional ByRef vRequestTime As Object = Nothing, Optional ByRef vEventtype As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vVolume As Object = Nothing, Optional ByRef vPagefile As Object = Nothing, Optional ByRef vDoctype As Object = Nothing, Optional ByRef vFiller As Object = Nothing, Optional ByRef vHderror As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vProcessed As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'Developer Guie No 115
        'If (Informations.IsNothing(vHistoryID)) Or (vHistoryID.Equals(0)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vHistoryID)) OrElse (vHistoryID.Equals(0)) OrElse (bDefaultAll) Then
            vHistoryID = 0
        End If



        'If (Informations.IsNothing(vTask)) Or (vTask.Equals(0)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vTask)) OrElse (vTask.Equals(0)) OrElse (bDefaultAll) Then
            vTask = 0
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vCabinetcode)) Or (String.IsNullOrEmpty(vCabinetcode)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vCabinetcode)) OrElse (String.IsNullOrEmpty(vCabinetcode)) OrElse (bDefaultAll) Then
            vCabinetcode = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vCabinetname)) Or (String.IsNullOrEmpty(vCabinetname)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vCabinetname)) OrElse (String.IsNullOrEmpty(vCabinetname)) OrElse (bDefaultAll) Then
            vCabinetname = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vDrawercode)) Or (String.IsNullOrEmpty(vDrawercode)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vDrawercode)) OrElse (String.IsNullOrEmpty(vDrawercode)) OrElse (bDefaultAll) Then
            vDrawercode = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vDrawername)) Or (String.IsNullOrEmpty(vDrawername)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vDrawername)) OrElse (String.IsNullOrEmpty(vDrawername)) OrElse (bDefaultAll) Then
            vDrawername = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vFoldercode)) Or (String.IsNullOrEmpty(vFoldercode)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vFoldercode)) OrElse (String.IsNullOrEmpty(vFoldercode)) OrElse (bDefaultAll) Then
            vFoldercode = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vFoldername)) Or (String.IsNullOrEmpty(vFoldername)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vFoldername)) OrElse (String.IsNullOrEmpty(vFoldername)) OrElse (bDefaultAll) Then
            vFoldername = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vDocref)) Or (String.IsNullOrEmpty(vDocref)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vDocref)) OrElse (String.IsNullOrEmpty(vDocref)) OrElse (bDefaultAll) Then
            vDocref = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vRequestDate)) Or (String.IsNullOrEmpty(vRequestDate)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vRequestDate)) OrElse (String.IsNullOrEmpty(vRequestDate)) OrElse (bDefaultAll) Then
            vRequestDate = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vRequestTime)) Or (String.IsNullOrEmpty(vRequestTime)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vRequestTime)) OrElse (String.IsNullOrEmpty(vRequestTime)) OrElse (bDefaultAll) Then
            vRequestTime = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vEventtype)) Or (String.IsNullOrEmpty(vEventtype)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vEventtype)) OrElse (String.IsNullOrEmpty(vEventtype)) OrElse (bDefaultAll) Then
            vEventtype = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vDescription)) OrElse (String.IsNullOrEmpty(vDescription)) OrElse (bDefaultAll) Then
            vDescription = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vVolume)) Or (String.IsNullOrEmpty(vVolume)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vVolume)) OrElse (String.IsNullOrEmpty(vVolume)) OrElse (bDefaultAll) Then
            vVolume = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vPagefile)) Or (String.IsNullOrEmpty(vPagefile)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vPagefile)) OrElse (String.IsNullOrEmpty(vPagefile)) OrElse (bDefaultAll) Then
            vPagefile = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vFiller)) Or (String.IsNullOrEmpty(vFiller)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vDoctype)) OrElse (String.IsNullOrEmpty(vDoctype)) OrElse (bDefaultAll) Then
            vDoctype = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vFiller)) Or (String.IsNullOrEmpty(vFiller)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vFiller)) OrElse (String.IsNullOrEmpty(vFiller)) OrElse (bDefaultAll) Then
            vFiller = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vHderror)) Or (String.IsNullOrEmpty(vHderror)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vHderror)) OrElse (String.IsNullOrEmpty(vHderror)) OrElse (bDefaultAll) Then
            vHderror = "N"
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vCreateDate)) Or (vCreateDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
        If (Informations.IsNothing(vCreateDate)) OrElse (vCreateDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vCreateDate = DateTime.Now
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vProcessed)) Or (String.IsNullOrEmpty(vProcessed)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vProcessed)) OrElse (String.IsNullOrEmpty(vProcessed)) OrElse (bDefaultAll) Then
            vProcessed = "N"
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: According to the task, this validates mandatory fields.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vHistoryID As Object = Nothing, Optional ByRef vTask As Integer = 0, Optional ByRef vCabinetcode As Object = Nothing, Optional ByRef vCabinetname As Object = Nothing, Optional ByRef vDrawercode As Object = Nothing, Optional ByRef vDrawername As Object = Nothing, Optional ByRef vFoldercode As Object = Nothing, Optional ByRef vFoldername As Object = Nothing, Optional ByRef vDocref As Object = Nothing, Optional ByRef vRequestDate As Object = Nothing, Optional ByRef vRequestTime As Object = Nothing, Optional ByRef vEventtype As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vVolume As Object = Nothing, Optional ByRef vPagefile As Object = Nothing, Optional ByRef vDoctype As Object = Nothing, Optional ByRef vFiller As Object = Nothing, Optional ByRef vHderror As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vProcessed As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        If (Informations.IsNothing(vTask)) Or (vTask.Equals(0)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCabinetcode)) Or (Object.Equals(vCabinetcode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (vTask = DOCADDCABINET) Or (vTask = DOCMODCABINET) Then



            If (Informations.IsNothing(vCabinetname)) Or (Object.Equals(vCabinetname, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDDRAWER) Or (vTask = DOCMODDRAWER) Or (vTask = DOCDELDRAWER) Or (vTask = DOCADDFOLDER) Or (vTask = DOCMODFOLDER) Or (vTask = DOCDELFOLDER) Or (vTask = DOCADDDOCUMENT) Or (vTask = DOCMODDOCUMENT) Or (vTask = DOCDELDOCUMENT) Then



            If (Informations.IsNothing(vDrawercode)) Or (Object.Equals(vDrawercode, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDDRAWER) Or (vTask = DOCMODDRAWER) Then



            If (Informations.IsNothing(vDrawername)) Or (Object.Equals(vDrawername, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDFOLDER) Or (vTask = DOCMODFOLDER) Or (vTask = DOCDELFOLDER) Or (vTask = DOCADDDOCUMENT) Or (vTask = DOCMODDOCUMENT) Or (vTask = DOCDELDOCUMENT) Then



            If (Informations.IsNothing(vFoldercode)) Or (Object.Equals(vFoldercode, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDFOLDER) Or (vTask = DOCMODFOLDER) Then



            If (Informations.IsNothing(vFoldername)) Or (Object.Equals(vFoldername, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDDOCUMENT) Or (vTask = DOCMODDOCUMENT) Or (vTask = DOCDELDOCUMENT) Then



            If (Informations.IsNothing(vDocref)) Or (Object.Equals(vDocref, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDDOCUMENT) Or (vTask = DOCMODDOCUMENT) Or (vTask = DOCDELDOCUMENT) Then



            If (Informations.IsNothing(vRequestDate)) Or (Object.Equals(vRequestDate, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDDOCUMENT) Or (vTask = DOCMODDOCUMENT) Or (vTask = DOCDELDOCUMENT) Then



            If (Informations.IsNothing(vRequestTime)) Or (Object.Equals(vRequestTime, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If vTask = DOCADDDOCUMENT Then



            If (Informations.IsNothing(vEventtype)) Or (Object.Equals(vEventtype, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If (vTask = DOCADDDOCUMENT) Or (vTask = DOCMODDOCUMENT) Then



            If (Informations.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If vTask = DOCADDDOCUMENT Then



            If (Informations.IsNothing(vVolume)) Or (Object.Equals(vVolume, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If vTask = DOCADDDOCUMENT Then



            If (Informations.IsNothing(vPagefile)) Or (Object.Equals(vPagefile, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        If vTask = DOCADDDOCUMENT Then



            If (Informations.IsNothing(vDoctype)) Or (Object.Equals(vDoctype, Nothing)) Then
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the History for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vHistoryID As Object = Nothing, Optional ByRef vTask As Object = Nothing, Optional ByRef vCabinetcode As Object = Nothing, Optional ByRef vCabinetname As Object = Nothing, Optional ByRef vDrawercode As Object = Nothing, Optional ByRef vDrawername As Object = Nothing, Optional ByRef vFoldercode As Object = Nothing, Optional ByRef vFoldername As Object = Nothing, Optional ByRef vDocref As Object = Nothing, Optional ByRef vRequestDate As Object = Nothing, Optional ByRef vRequestTime As Object = Nothing, Optional ByRef vEventtype As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vVolume As Object = Nothing, Optional ByRef vPagefile As Object = Nothing, Optional ByRef vDoctype As Object = Nothing, Optional ByRef vFiller As Object = Nothing, Optional ByRef vHderror As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vProcessed As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vHistoryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vTask), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsDate(vCreateDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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
        'bPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub




    ' ***************************************************************** '
    ' Name: UpdateHDBCheck (Private)
    '
    ' Description: Check the system table to see if this is an environment
    '               where the history table is updated.
    '               Valide values for column are only "Y" and "N".
    '
    ' ***************************************************************** '
    Private Function UpdateHDBCheck() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing




        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT update_history FROM DOC_system"

        'Construct SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="UPDATEHDBCHECK", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'Ensure value is valid and set update HDB property accordingly
        If Informations.IsArray(vResultArray) Then

            Select Case vResultArray(0, 0)
                Case "Y"
                    m_bUpdateHDB = True

                Case "N"
                    m_bUpdateHDB = False

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="update_history in system table does not contain a valid value.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateHDBCheck", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End Select

        Else

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="update_history in system table does not contain a valid value.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateHDBCheck", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End If

        Return result

    End Function
End Class

