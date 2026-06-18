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
    '              a Folder.
    '
    ' Edit History:
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

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

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Description: Adds a single Folder directly into the database.
    '        Note: The Folder will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Public Function DirectAdd(Optional ByRef vFolderNum As Integer = 0, Optional ByRef vFolderName As Object = Nothing, Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer
    Public Function DirectAdd(Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vFolderName As Object = Nothing, Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oFolder As bDOCFolder.Folder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Folder
            oFolder = New bDOCFolder.Folder()

            ' Populate Folder Attributes







            'developer guide no. 98
            m_lReturn = SetProperties(oFolder, gPMConstants.PMEComponentAction.PMAdd, vFolderNum:=vFolderNum, vFolderName:=vFolderName, vParentNum:=vParentNum, vExCode:=vExCode, vFolderLevel:=vFolderLevel, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Folder to the Database
            m_lReturn = AddItem(oFolder)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Folder Added

            If Not Informations.IsNothing(vFolderNum) Then
                vFolderNum = oFolder.FolderNum
            End If

            ' {* USER DEFINED CODE (End) *}

            oFolder = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oFolder As bDOCFolder.Folder) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oFolder:=oFolder)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add FolderNum as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Folder_Num", vValue:=oFolder.FolderNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamOutput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oFolder.FolderNum = m_oDatabase.Parameters.Item("Folder_Num").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Folder property values.
    '
    ' ***************************************************************** '
    'Developer Guie No 101
    'Private Function SetProperties(ByRef oFolder As bDOCFolder.Folder, ByRef iStatus As Integer, Optional ByRef vFolderNum As Integer = 0, Optional ByRef vFolderName As String = "", Optional ByRef vParentNum As Integer = 0, Optional ByRef vExCode As String = "", Optional ByRef vFolderLevel As Integer = 0, Optional ByRef vAccessLevel As Integer = 0, Optional ByRef vPassword As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#) As Integer
    Private Function SetProperties(ByRef oFolder As bDOCFolder.Folder, ByRef iStatus As Integer, Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vFolderName As Object = Nothing, Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vFolderNum:=vFolderNum, vFolderName:=vFolderName, vParentNum:=vParentNum, vExCode:=vExCode, vFolderLevel:=vFolderLevel, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vFolderNum:=vFolderNum, vFolderName:=vFolderName, vParentNum:=vParentNum, vExCode:=vExCode, vFolderLevel:=vFolderLevel, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to set a default parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vFolderNum:=vFolderNum, vFolderName:=vFolderName, vParentNum:=vParentNum, vExCode:=vExCode, vFolderLevel:=vFolderLevel, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vCreateDate:=vCreateDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to validate a parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oFolder


            If Not Informations.IsNothing(vFolderNum) Then
                If .FolderNum <> vFolderNum Then
                    .FolderNum = vFolderNum
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFolderName) Then
                If .FolderName.Trim() <> vFolderName.Trim() Then
                    .FolderName = vFolderName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vParentNum) Then
                If .ParentNum <> vParentNum Then
                    .ParentNum = vParentNum
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vExCode) Then
                If .ExCode.Trim() <> vExCode.Trim() Then
                    .ExCode = vExCode
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vFolderLevel) Then
                If .FolderLevel <> vFolderLevel Then
                    .FolderLevel = vFolderLevel
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAccessLevel) Then
                If .AccessLevel <> vAccessLevel Then
                    .AccessLevel = vAccessLevel
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPassword) Then
                If .Password.Trim() <> vPassword.Trim() Then
                    .Password = vPassword
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCreateDate) Then
                If .CreateDate <> vCreateDate Then
                    .CreateDate = vCreateDate
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
    Private Function AddInputParam(ByRef oFolder As bDOCFolder.Folder) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="folder_name", vValue:=oFolder.FolderName, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="parent_num", vValue:=oFolder.ParentNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ex_code", vValue:=oFolder.ExCode, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="folder_level", vValue:=oFolder.FolderLevel, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMInteger))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="access_level", vValue:=oFolder.AccessLevel, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMInteger))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="password", vValue:=oFolder.Password, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=oFolder.CreateDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Folder.
    '
    ' ***************************************************************** '
    'Developer guide no. 101
    'Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vFolderName As String = "", Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As String = "", Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As String = "", Optional ByRef vCreateDate As Object = Nothing) As Integer
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vFolderName As Object = Nothing, Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'Developer Guie No 115
        'If (Informations.IsNothing(vFolderNum)) Or (vFolderNum.Equals(0)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vFolderNum)) OrElse (vFolderNum.Equals(0)) OrElse (bDefaultAll) Then
            vFolderNum = 0
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vFolderName)) Or (String.IsNullOrEmpty(vFolderName)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vFolderName)) OrElse (String.IsNullOrEmpty(vFolderName)) OrElse (bDefaultAll) Then
            vFolderName = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vParentNum)) Or (vParentNum.Equals(0)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vParentNum)) OrElse (vParentNum.Equals(0)) OrElse (bDefaultAll) Then
            vParentNum = 0
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vExCode)) Or (String.IsNullOrEmpty(vExCode)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vExCode)) OrElse (String.IsNullOrEmpty(vExCode)) OrElse (bDefaultAll) Then
            vExCode = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vFolderLevel)) Or (vFolderLevel.Equals(0)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vFolderLevel)) OrElse (vFolderLevel.Equals(0)) OrElse (bDefaultAll) Then
            vFolderLevel = 0
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vAccessLevel)) Or (vAccessLevel.Equals(0)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vAccessLevel)) OrElse (vAccessLevel.Equals(0)) OrElse (bDefaultAll) Then
            vAccessLevel = 9
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vPassword)) Or (String.IsNullOrEmpty(vPassword)) Or (bDefaultAll) Then
        If (Informations.IsNothing(vPassword)) OrElse (String.IsNullOrEmpty(vPassword)) OrElse (bDefaultAll) Then
            vPassword = ""
        End If



        'Developer Guie No 115
        'If (Informations.IsNothing(vCreateDate)) Or (vCreateDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
        If (Informations.IsNothing(vCreateDate)) OrElse (vCreateDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vCreateDate = DateTime.Now
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Folder.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vFolderName As Object = Nothing, Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        '    If (IsMissing(vFolderNum) = True) _
        ''    Or (IsEmpty(vFolderNum) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If



        If (Informations.IsNothing(vFolderName)) Or (Object.Equals(vFolderName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vParentNum)) Or (Object.Equals(vParentNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vExCode) = True) _
        ''    Or (IsEmpty(vExCode) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vFolderLevel) = True) _
        ''    Or (IsEmpty(vFolderLevel) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vAccessLevel) = True) _
        ''    Or (IsEmpty(vAccessLevel) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vPassword) = True) _
        ''    Or (IsEmpty(vPassword) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vCreateDate) = True) _
        ''    Or (IsEmpty(vCreateDate) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Folder for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vFolderName As Object = Nothing, Optional ByRef vParentNum As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vFolderLevel As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vFolderNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vParentNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vFolderLevel), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vAccessLevel), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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
    'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
    'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
    'bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
        'bPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class