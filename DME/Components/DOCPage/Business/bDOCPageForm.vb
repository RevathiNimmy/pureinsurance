Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 16/01/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Page.
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
    ' Description: Adds a single Page directly into the database.
    '        Note: The Page will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vPageName As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vPageNum As Object = Nothing, Optional ByRef vPageType As Object = Nothing, Optional ByRef vPageSize As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vVolumeID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPage As bDOCPage.Page

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Page
            oPage = New bDOCPage.Page()

            ' Populate Page Attributes






            'Developer Guide 98
            'm_lReturn = SetProperties(oPage, gPMConstants.PMEComponentAction.PMAdd, vPageName:=CStr(vPageName), vDocNum:=CInt(vDocNum), vPageNum:=CInt(vPageNum), vPageType:=CStr(vPageType), vPageSize:=CInt(vPageSize), vCreateDate:=CDate(vCreateDate), vVolumeID:=CInt(vVolumeID))
            m_lReturn = SetProperties(oPage, gPMConstants.PMEComponentAction.PMAdd, vPageName:=vPageName, vDocNum:=vDocNum, vPageNum:=vPageNum, vPageType:=vPageType, vPageSize:=vPageSize, vCreateDate:=vCreateDate, vVolumeID:=vVolumeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Page to the Database
            m_lReturn = AddItem(oPage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            '    ' Return the ID of the Page Added
            '    If (IsMissing(vPageID) = False) Then
            '            vPageID = oPage.PageID
            '    End If

            ' {* USER DEFINED CODE (End) *}

            oPage = Nothing

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
    Private Function AddItem(ByRef oPage As bDOCPage.Page) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oPage:=oPage)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    ' Add PageID as an OUTPUT param for an insert
        '    m_lreturn& = m_oDatabase.Parameters.Add( _
        ''          sName:="Page_id", _
        ''          vValue:=oPage.PageID, _
        ''          iDirection:=PMParamOutput, _
        ''          iDataType:=PMLong)
        '
        '    If (m_lreturn& <> PMTrue) Then
        '        AddItem = PMFalse
        '        Exit Function
        '    End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    ' Get the ID of the record inserted
        '     oPage.PageID = m_oDatabase.Parameters.Item("Page_id").Value
        '
        '    If (m_lreturn& <> PMTrue) Then
        '        AddItem = PMFalse
        '        Exit Function
        '    End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Page property values.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Private Function SetProperties(ByRef oPage As bDOCPage.Page, ByRef iStatus As Integer, Optional ByRef vPageName As String = "", Optional ByRef vDocNum As Integer = 0, Optional ByRef vPageNum As Integer = 0, Optional ByRef vPageType As String = "", Optional ByRef vPageSize As Integer = 0, Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vVolumeID As Integer = 0) As Integer
    Private Function SetProperties(ByRef oPage As bDOCPage.Page, ByRef iStatus As Integer, Optional ByRef vPageName As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vPageNum As Object = Nothing, Optional ByRef vPageType As Object = Nothing, Optional ByRef vPageSize As Object = Nothing, Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vVolumeID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vPageName:=vPageName, vDocNum:=vDocNum, vPageNum:=vPageNum, vPageType:=vPageType, vPageSize:=vPageSize, vCreateDate:=vCreateDate, vVolumeID:=vVolumeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vPageName:=vPageName, vDocNum:=vDocNum, vPageNum:=vPageNum, vPageType:=vPageType, vPageSize:=vPageSize, vCreateDate:=vCreateDate, vVolumeID:=vVolumeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to set a default parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vPageName:=vPageName, vDocNum:=vDocNum, vPageNum:=vPageNum, vPageType:=vPageType, vPageSize:=vPageSize, vCreateDate:=vCreateDate, vVolumeID:=vVolumeID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to validate a parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oPage


            If Not Informations.IsNothing(vPageName) Then
                If .PageName.Trim() <> vPageName.Trim() Then
                    .PageName = vPageName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDocNum) Then
                If .DocNum <> vDocNum Then
                    .DocNum = vDocNum
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPageNum) Then
                If .PageNum <> vPageNum Then
                    .PageNum = vPageNum
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPageType) Then
                If .PageType.Trim() <> vPageType.Trim() Then
                    .PageType = vPageType
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPageSize) Then
                If .PageSize <> vPageSize Then
                    .PageSize = vPageSize
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCreateDate) Then
                If .CreateDate <> vCreateDate Then
                    .CreateDate = vCreateDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vVolumeID) Then
                If .VolumeID <> vVolumeID Then
                    .VolumeID = vVolumeID
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
    Private Function AddInputParam(ByRef oPage As bDOCPage.Page) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="page_name", vValue:=oPage.PageName, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="doc_num", vValue:=oPage.DocNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="page_num", vValue:=oPage.PageNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="page_type", vValue:=oPage.PageType, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="page_size", vValue:=oPage.PageSize, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=oPage.CreateDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="volume_id", vValue:=oPage.VolumeID, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Page.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    'Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPageName As String = "", Optional ByRef vDocNum As Byte = 0, Optional ByRef vPageNum As Byte = 0, Optional ByRef vPageType As String = "", Optional ByRef vPageSize As Byte = 0, Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vVolumeID As Integer = 0) As Integer
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPageName As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vPageNum As Object = Nothing, Optional ByRef vPageType As Object = Nothing, Optional ByRef vPageSize As Object = Nothing, Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vVolumeID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPageName)) OrElse (String.IsNullOrEmpty(vPageName)) OrElse (bDefaultAll) Then
            vPageName = ""
        End If



        If (Informations.IsNothing(vDocNum)) OrElse (vDocNum.Equals(0)) OrElse (bDefaultAll) Then
            vDocNum = 0
        End If



        If (Informations.IsNothing(vPageNum)) OrElse (vPageNum.Equals(0)) OrElse (bDefaultAll) Then
            vPageNum = 0
        End If



        If (Informations.IsNothing(vPageType)) OrElse (String.IsNullOrEmpty(vPageType)) OrElse (bDefaultAll) Then
            vPageType = ""
        End If



        If (Informations.IsNothing(vPageSize)) OrElse (vPageSize.Equals(0)) OrElse (bDefaultAll) Then
            vPageSize = 0
        End If



        If (Informations.IsNothing(vCreateDate)) OrElse (vCreateDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vCreateDate = DateTime.Now
        End If



        If (Informations.IsNothing(vVolumeID)) OrElse (vVolumeID.Equals(0)) OrElse (bDefaultAll) Then
            vVolumeID = DOCHD1_ID
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Page.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vPageName As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vPageNum As Object = Nothing, Optional ByRef vPageType As Object = Nothing, Optional ByRef vPageSize As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vVolumeID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPageName)) Or (Object.Equals(vPageName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vDocNum)) OrElse (Object.Equals(vDocNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPageNum)) OrElse (Object.Equals(vPageNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPageType)) OrElse (Object.Equals(vPageType, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vPageSize) = True) _
        ''    Or (IsEmpty(vPageSize) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vCreateDate) = True) _
        ''    Or (IsEmpty(vCreateDate) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vVolumeID) = True) _
        ''    Or (IsEmpty(vVolumeID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Page for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPageName As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vPageNum As Object = Nothing, Optional ByRef vPageType As Object = Nothing, Optional ByRef vPageSize As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vVolumeID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vDocNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vPageNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vPageSize), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsDate(vCreateDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vVolumeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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