Option Strict Off
Option Explicit On
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyAH
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyAH
    '
    ' Date: 11/08/1999
    '
    ' Description: Describes the SIRPartyAH attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRPartyAH"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyAH As dSIRPartyAH.SIRPartyAH ' was dSIRPartyAH.SIRPartyAH

    Private m_bSIRParty As bSIRParty.Business

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    'EK 12/10/99
    Private m_sHandlerType As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property
    'EK 12/10/99
    Public Property HandlerType() As String
        Get

            Return m_sHandlerType
        End Get
        Set(ByVal Value As String)

            m_sHandlerType = Value

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public ReadOnly Property bSIRParty() As Object
        Get

            Return m_bSIRParty

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Create instance of data class
            m_dSIRPartyAH = New dSIRPartyAH.SIRPartyAH()
            '    Set m_dSIRPartyAH = New dSIRPartyAH.SIRPartyAH

            m_lReturn = m_dSIRPartyAH.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            ' Create Core Business Object



            m_bSIRParty = New bSIRParty.Business
            m_lReturn = m_bSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            Me.disposedValue = True
            If disposing Then
                If m_dSIRPartyAH IsNot Nothing Then
                    m_dSIRPartyAH.Dispose()
                End If
                m_dSIRPartyAH = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyAH.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults





            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vForename:=vForename, vInitials:=vInitials, vDepartmentID:=vDepartmentID, vPartyTitleCode:=vPartyTitleCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIRPartyAH property values.
    '
    ' SJP (CMG)     03/04/2003          PS235
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vCommissionCnt As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters





                'Developer Guide No. 101
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vForename:=vForename, vInitials:=vInitials, vDepartmentID:=vDepartmentID, vPartyTitleCode:=vPartyTitleCode, vCommissionCnt:=vCommissionCnt), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vForename:=vForename, vInitials:=vInitials, vDepartmentID:=vDepartmentID, vPartyTitleCode:=vPartyTitleCode, vCommissionCnt:=vCommissionCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False
            'EK 25/11/99 Modified to match standard
            ' Set Property values.
            With m_dSIRPartyAH



                'If (Not Informations.IsNothing(vPartyCnt)) And (Not vPartyCnt.Equals(0)) Then
                If (Not Informations.IsNothing(vPartyCnt)) And (Not Object.Equals(vPartyCnt, Nothing)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Informations.IsNothing(vForename)) And (Not Object.Equals(vForename, Nothing)) Then


                    'Developer Guide No. 24
                    .Forename = vForename
                End If



                If (Not Informations.IsNothing(vInitials)) And (Not Object.Equals(vInitials, Nothing)) Then


                    'Developer Guide No. 24
                    .Initials = vInitials
                End If



                'If (Not Informations.IsNothing(vDepartmentID)) And (Not vDepartmentID.Equals(0)) Then
                If (Not Informations.IsNothing(vDepartmentID)) And (Not Object.Equals(vDepartmentID, Nothing)) Then
                    .DepartmentID = vDepartmentID
                End If



                If (Not Informations.IsNothing(vPartyTitleCode)) And (Not Object.Equals(vPartyTitleCode, Nothing)) Then


                    'Developer Guide No. 24
                    .PartyTitleCode = vPartyTitleCode
                End If

                ' SJP (CMG) PS235 030402003


                If (Not Informations.IsNothing(vCommissionCnt)) And (Not Object.Equals(vCommissionCnt, Nothing)) Then


                    'Developer Guide No. 24
                    .CommissionCnt = vCommissionCnt
                End If

                If Not String.IsNullOrEmpty(sUniqueId) AndAlso Not String.IsNullOrEmpty(sScreenHierarchy) Then
                    .UniqueId = sUniqueId
                    .ScreenHierarchy = sScreenHierarchy
                End If

                ' If we have changed one of the properties, update the status
                'EK 15/11/99 bSIRParty changes do not update this status so updates are
                'not being done. If this is wrong blame me
                '        If (bDataChanged = True) Then
                '            m_iDatabaseStatus = iStatus%
                '        End If
                m_iDatabaseStatus = iStatus
                'EK 15/11/99 End
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRPartyAH property values.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vCommissionCnt As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyAH


                'Developer Guide No. 118
                vPartyCnt = .PartyCnt



                'If Not Informations.IsNothing(vForename) Then


                vForename = .Forename



                'If Not Informations.IsNothing(vInitials) Then


                vInitials = .Initials
                ' End If

                ' CTAF 150800 - Check if .Department's null

                'If Not Informations.IsNothing(vDepartmentID) Then

                If Convert.IsDBNull(.DepartmentID) Or Informations.IsNothing(.DepartmentID) Then
                    vDepartmentID = 0
                Else
                    vDepartmentID = .DepartmentID
                End If



                'If Not Informations.IsNothing(vPartyTitleCode) Then


                vPartyTitleCode = .PartyTitleCode



                'If Not Informations.IsNothing(vCommissionCnt) Then

                If Convert.IsDBNull(.CommissionCnt) Or Informations.IsNothing(.CommissionCnt) Then
                    vCommissionCnt = 0
                Else

                    vCommissionCnt = .CommissionCnt
                End If


                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAH

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                'EK 12/10/99
                .HandlerType = HandlerType

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAH
                'EK 12/10/99
                .HandlerType = HandlerType
                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyAH Added
                PartyCnt = .PartyCnt

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAH
                'EK 12/10/99
                .HandlerType = HandlerType

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAH
                'EK 12/10/99
                .HandlerType = HandlerType

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyAH.
    '
    ' SJP (CMG)         03/04/2003      PS235
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vShortname As String = "", Optional ByRef vName As String = "", Optional ByRef vForename As String = "", Optional ByRef vInitials As String = "", Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vPartyTitleCode As String = "", Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCommissionCnt As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If
        'EK 15/11/99


        If (Informations.IsNothing(vShortname)) Or (String.IsNullOrEmpty(vShortname)) Or (bDefaultAll) Then
            vShortname = ""
        End If



        If (Informations.IsNothing(vName)) Or (String.IsNullOrEmpty(vName)) Or (bDefaultAll) Then
            vName = ""
        End If



        If (Informations.IsNothing(vForename)) Or (String.IsNullOrEmpty(vForename)) Or (bDefaultAll) Then
            vForename = ""
        End If



        If (Informations.IsNothing(vInitials)) Or (String.IsNullOrEmpty(vInitials)) Or (bDefaultAll) Then
            vInitials = ""
        End If



        If (Informations.IsNothing(vDepartmentID)) OrElse (vDepartmentID.Equals(0)) Or (bDefaultAll) Then
            vDepartmentID = 0
        End If



        If (Informations.IsNothing(vPartyTitleCode)) Or (String.IsNullOrEmpty(vPartyTitleCode)) Or (bDefaultAll) Then
            vPartyTitleCode = ""
        End If

        ' SJP(CMG) PS235 03042003


        If (Informations.IsNothing(vCommissionCnt)) OrElse (vCommissionCnt.Equals(0)) Or (bDefaultAll) Then
            vCommissionCnt = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyAH for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vCommissionCnt As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDepartmentID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vDepartmentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCommissionCnt) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vCommissionCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

