Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMUserGroup.
    '
    ' Edit History:
    ' Tom231098 - Nicked lock, stock and barrel from PMUser.
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

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)



    Public Property LanguageId() As Integer
        Get
            Return m_iLanguageID
        End Get
        Set(ByVal Value As Integer)
            m_iLanguageID = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

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


            If Informations.IsNothing(vDatabase) Then

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetCaptionID (Public)
    '
    ' Description: The caption is passed as a string, and the stored
    ' procedure does all the hard work, returning the caption id
    '
    ' ***************************************************************** '
    Public Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionId As Integer) As Integer
        Return GetCaptionID(v_sCaption:=v_sCaption, r_lCaptionId:=r_lCaptionId, v_iLanguageID:=0)
    End Function

    Public Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionId As Integer, ByVal v_iLanguageID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1)
            If v_iLanguageID = 0 Then
                v_iLanguageID = m_iLanguageID
            End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1)


            '    sSQL = ""
            '    sSQL = sSQL & "exec spu_pm_caption "
            '    sSQL = sSQL & m_iLanguageID
            '    sSQL = sSQL & ", '" & v_sCaption & "'"
            '
            '    ' Execute SQL Statement
            '    m_lError& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL, _
            ''        sSQLName:="GetPMCaptionName", _
            ''        bStoredProcedure:=False)
            '
            '    If (m_lError& <> PMTrue) Then
            '        GetCaptionID = PMFalse
            '        Exit Function
            '    End If
            '
            '    r_lCaptionId = m_oDatabase.Records.Item(1).Fields.Item(1).Value

            ' CTAF 181200 - Rewritten properly

            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the language_id parameter
            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1)
            'Changed the languageid from m_iLanguageID to v_iLanguageID
            m_lError = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(v_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the caption_id parameter
            m_lError = m_oDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the query
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetCaptionSQL, sSQLName:=ACGetCaptionName, bStoredProcedure:=ACGetCaptionStored, vResultArray:=vResultArray)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the caption if it was returned
            If Informations.IsArray(vResultArray) Then

                r_lCaptionId = CInt(vResultArray(0, 0))
            Else
                r_lCaptionId = 0
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptionDesc (Public)
    '
    ' Description: This one gets the caption from the code
    '
    ' ***************************************************************** '
    Public Function GetCaptionDesc(ByVal v_lCaptionId As Integer, ByRef r_sCaption As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    sSQL = ""
            '    sSQL = sSQL & "exec spu_pm_caption_desc "
            '    sSQL = sSQL & m_iLanguageID
            '    sSQL = sSQL & ", " & v_lCaptionId
            '
            '    ' Execute SQL Statement
            '    m_lError& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL, _
            ''        sSQLName:="GetPMCaptionDescName", _
            ''        bstoredprocedure:=False)
            '
            '    If (m_lError& <> PMTrue) Then
            '        GetCaptionDesc = PMFalse
            '        Exit Function
            '    End If
            '
            '    r_sCaption = m_oDatabase.Records.Item(1).Fields.Item(1).Value

            ' CTAF 181200 - Rewritten properly

            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the language_id parameter
            m_lError = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the caption_id parameter
            m_lError = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_lCaptionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the query
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetCaptionDescSQL, sSQLName:=ACGetCaptionDescName, bStoredProcedure:=ACGetCaptionDescStored, vResultArray:=vResultArray)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the caption if it was returned
            If Informations.IsArray(vResultArray) Then

                r_sCaption = CStr(vResultArray(0, 0))
            Else
                r_sCaption = ""
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionDesc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
