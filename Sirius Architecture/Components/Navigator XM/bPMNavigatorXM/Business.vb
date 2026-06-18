Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

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


    Private Const ACClass As String = "Business"

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
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
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
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


            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.
            If Not Information.IsNothing(vTask) Then
                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then
                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RemoveBlankKeys
    '
    ' Description: removes items from an array that have no values
    '
    ' History: 10/09/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RemoveBlankKeys(ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vNewArray(,) As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have something to do?
            If Not Information.IsArray(r_vKeyArray) Then
                Return result
            End If

            vNewArray = Nothing

            ' Loop the array
            For iLoop1 As Integer = 0 To r_vKeyArray.GetUpperBound(1)
                'PN 44217
                If Information.IsArray(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                    ' Prep the array
                    If Information.IsArray(vNewArray) Then

                        iIndex = vNewArray.GetUpperBound(1) + 1
                        ReDim Preserve vNewArray(1, iIndex)
                    Else
                        iIndex = 0
                        ReDim vNewArray(1, iIndex)
                    End If

                    ' Store it


                    vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)


                    vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                Else

                    If CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) <> "" Then

                        ' Prep the array
                        If Information.IsArray(vNewArray) Then

                            iIndex = vNewArray.GetUpperBound(1) + 1
                            ReDim Preserve vNewArray(1, iIndex)
                        Else
                            iIndex = 0
                            ReDim vNewArray(1, iIndex)
                        End If

                        ' Store it


                        vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)


                        vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                    End If
                End If
            Next iLoop1


            r_vKeyArray = vNewArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveBlankKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveBlankKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CreateWorkTask
    '
    ' Description: Creates a Work Manager Task
    '
    ' History:  31/08/2001 CTAF - Created.
    '
    '           08/04/2003 Kevin Renshaw (CMG) Amended to cater for differing task days
    '           29/01/2004 SET  - Added UserId and GroupId parameters
    '
    ' ***************************************************************** '
    Public Function CreateWorkTask(ByVal v_sTaskCode As String, ByVal v_sDescription As String, ByRef r_vKeyArray(,) As Object, Optional ByVal v_lUserGroupID As Integer = 0, Optional ByVal v_lUserID As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oRoadmap As bSIRRoadmap.Business
        Dim lPartyCnt As Integer
        Dim iTaskDaysDue As Integer
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of bSIRRoadmap

            oRoadmap = New bSIRRoadmap.Business
            m_lReturn = oRoadmap.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            ' Default to no party
            lPartyCnt = 0


            Select Case v_sDescription
                Case "Complete New Business (Add a policy)"
                    m_lReturn = CType(bPMFunc.GetSystemOption(v_sUserName:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=19, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)

                Case "Complete New Business (Raise debit)"
                    m_lReturn = CType(bPMFunc.GetSystemOption(v_sUserName:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=21, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)

                Case "Complete New Business (Cash List)"
                    m_lReturn = CType(bPMFunc.GetSystemOption(v_sUserName:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=23, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)

                Case "Complete New Business (Cash List Item)"
                    m_lReturn = CType(bPMFunc.GetSystemOption(v_sUserName:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=23, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)
            End Select

            iTaskDaysDue = gPMFunctions.ToSafeInteger(sValue)

            'Default TaskDaysDue to 7
            If iTaskDaysDue = 0 Then
                iTaskDaysDue = 7
            End If


            ' Get the party count & TaskDaysDue
            If Information.IsArray(r_vKeyArray) Then
                For iLoop1 As Integer = 0 To r_vKeyArray.GetUpperBound(1)

                    If CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = PMNavKeyConst.PMKeyNamePartyCnt Then

                        lPartyCnt = CInt(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    ElseIf (CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = PMNavKeyConst.PMKeyNameTaskDaysDue) Then

                        iTaskDaysDue = CInt(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'Default TaskDueDate Key to 7 for future steps

                        r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1) = "7"
                    End If
                Next iLoop1


            End If

            ' Remove any empty keys from the task
            m_lReturn = CType(RemoveBlankKeys(r_vKeyArray:=r_vKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call Roadmap to create the task - Default to 7 days?

            m_lReturn = oRoadmap.CreateWorkManagerTask(v_lPartyCnt:=lPartyCnt, v_sDescription:=v_sDescription, v_sTask:=v_sTaskCode, v_lNumDays:=iTaskDaysDue, v_vKeyArray:=r_vKeyArray, v_lUserGroupID:=v_lUserGroupID, v_lUserID:=v_lUserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear up

            oRoadmap.Dispose()
            oRoadmap = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUserProperty
    '
    ' Description:
    '
    ' History: 14/05/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetUserProperty(ByVal v_sUserName As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call component services
            m_lReturn = CType(gPMComponentServices.GetUserProperty(v_sUserName:=v_sUserName, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserProperty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUserProperty
    '
    ' Description:
    '
    ' History: 14/05/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateUserProperty(ByVal v_sUserName As String, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call component services
            m_lReturn = CType(gPMComponentServices.UpdateUserProperty(v_sUserName:=v_sUserName, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserProperty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteUserProperty
    '
    ' Description:
    '
    ' History: 14/05/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteUserProperty(ByVal v_sUserName As String, ByVal v_sPropertyName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call component services
            m_lReturn = CType(gPMComponentServices.DeleteUserProperty(v_sUserName:=v_sUserName, v_sPropertyName:=v_sPropertyName), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteUserProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUserProperty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetXMLVersion
    '
    ' Description:
    '
    ' History: 01082003 RDC check XML file version against what is
    '                       in the database
    '
    ' ***************************************************************** '
    Public Function GetXMLVersion(ByVal sRoadmap As String, ByVal sXMLFileVersion As String, ByRef sDBVersion As String, ByRef sXML As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""
        Dim vTemp As String = ""

        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            sSQL = "SELECT file_version_number, xml_definition "
            sSQL = sSQL & "FROM pmnavxm_process "
            sSQL = sSQL & "WHERE file_name = '" & sRoadmap & "'"

            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetXMLVersion", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' not found in the database
                Return result
            End If

            If lNumRecs = 0 Then
                ' no records. XML file is temporary, probably a test run from the NAV XM editor
                sXML = ""
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            vTemp = oDatabase.Records.Fields()("file_version_number")


            If Convert.IsDBNull(vTemp) Or IsNothing(vTemp) Then
                ' can't be doing with nulls in the version number
                oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return result
            End If

            sDBVersion = vTemp

            If CInt(sXMLFileVersion) = CInt(sDBVersion) Then
                ' don't need the XML
                sXML = ""
            Else
                ' return the XML
                sXML = oDatabase.Records.Fields()("xml_definition")
            End If

            oDatabase.CloseDatabase()

            oDatabase = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetXMLVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetXMLVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' CTAF I have commented out the following as it was used by CNIC
    '' ***************************************************************** '
    ''
    '' Name: GetRiskGroup
    ''
    '' Description: Gets the risk group for the passed insurance file
    ''
    '' History: 17/05/2002 CTAF - Created.
    ''
    '' ***************************************************************** '
    'Public Function GetRiskGroup(ByVal v_lInsuranceFileCnt As Long, _
    ''                             ByRef r_lRiskGroupID As Long) As Long
    '
    'Dim sCServices As sPMServerCS.PMServerBusinessCS
    'Dim oDatabase As dPMDAO.Database
    'Dim bCloseDatabase As Boolean
    'Dim vResultArray As Variant
    '
    '    On Error GoTo Err_GetRiskGroup
    '
    '    GetRiskGroup = PMTrue
    '
    '    ' Get component services
    '    Set sCServices = New sPMServerCS.PMServerBusinessCS
    '
    '    m_lReturn& = sCServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
    ''                        v_lPMProductFamily:=pmePFSiriusSolutions, _
    ''                        r_bnewInstanceCreated:=bCloseDatabase, _
    ''                        r_oCheckedDatabase:=oDatabase)
    '    If (m_lReturn& <> PMTrue) Then
    '        GetRiskGroup = PMFalse
    '        Exit Function
    '    End If
    '
    '    oDatabase.Parameters.Clear
    '
    '    m_lReturn& = oDatabase.Parameters.Add( _
    ''                    sName:="insurance_file_cnt", _
    ''                    vValue:=v_lInsuranceFileCnt, _
    ''                    iDirection:=PMParamInput, _
    ''                    iDataType:=PMLong)
    '    If (m_lReturn& <> PMTrue) Then
    '        GetRiskGroup = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Perform the SQL
    '    m_lReturn& = oDatabase.SQLSelect( _
    ''                        sSQL:="{call spu_CNC_GetRiskGroup(?)}", _
    ''                        sSQLName:="GetRiskGroup", _
    ''                        bStoredProcedure:=True, _
    ''                        vResultArray:=vResultArray)
    '    If (m_lReturn& <> PMTrue) Then
    '        GetRiskGroup = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Get the risk group
    '    If (IsArray(vResultArray) = True) Then
    '        r_lRiskGroupID = vResultArray(0, 0)
    '    Else
    '        GetRiskGroup = PMNotFound
    '    End If
    '
    '    Exit Function
    '
    'Err_GetRiskGroup:
    '
    '    GetRiskGroup = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetRiskGroup Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetRiskGroup", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    '    Resume
    '
    'End Function
    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
