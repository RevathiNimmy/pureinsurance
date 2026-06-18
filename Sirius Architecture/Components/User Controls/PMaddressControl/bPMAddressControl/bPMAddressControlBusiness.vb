Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
Imports System.Runtime.ExceptionServices
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 12/06/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, rules required to manipulate
    '              a uctPMAddressControl.
    '
    ' Edit History: TF120698 - Created
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

    Private m_lQASDatabaseID As Integer

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of PickListNodes (Private)
    Private m_oPickListNodes As bPMAddressControl.PickListNodes

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Current Item in Pick List
    Private m_lCurrentListItem As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Product Family - (defined in User Control)
    Private m_lProductFamily As Integer

    ' Database Search IDs
    Private m_lPMDatabaseID As Integer
    'Private m_lQASDatabaseID As Long

    'Address lines for mapping QAS address lines to PM
    Private m_sQAS2PMAddress1 As String = ""
    Private m_sQAS2PMAddress2 As String = ""
    Private m_sQAS2PMAddress3 As String = ""
    Private m_sQAS2PMAddress4 As String = ""
    Private m_sQASFormat As String = ""

    ' RDC 10072002
    Private m_sWarningMessage As String = ""

    ' QASPro .INI file location
    Private m_sQASPath As String = ""

    'Flag to indicate that QAS has been initialised
    Private m_lQASInitialised As gPMConstants.PMEReturnCode

    ' Successful return from QAS function
    Private Const QASSuccess As Integer = 0
    Private Const QASNotDefined As gPMConstants.PMEReturnCode = -1040
    ' PRIVATE Data Members (End)

    ' ***************************************************************** '
    ' Name: MapAddressLines (Private)
    '
    ' Description: Maps the Composite QAS Address lines to a single
    ' PM Address Line
    ' ***************************************************************** '
    Private Function MapAddressLines(ByRef r_oPickListNode As bPMAddressControl.PickListNode, ByRef sQASAddressLines As String) As String

        Dim QASAddressLines As List(Of Object)
        Dim sPartToAdd As String = ""
        Dim sPMAddressLine As New StringBuilder



        'Convert comma delimited QASAddress Line IDs to List of IDs
        QASAddressLines = GetQASAddressLines(sQASAddressLines)

        'Add the QAS Address Components to this PM address line
        '(IB)111199 - Added sPartToAdd so can test if empty string before append of pad space & string
        sPMAddressLine = New StringBuilder("")
        For lPtr As Integer = 1 To QASAddressLines.Count
            sPartToAdd = r_oPickListNode.AddressComponent(ToSafeInteger(QASAddressLines.Item(lPtr))).Trim()
            If sPartToAdd <> "" Then
                sPMAddressLine.Append(" " & sPartToAdd)
            End If
        Next lPtr


        Return sPMAddressLine.ToString().Trim()

    End Function

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    Public Property PMDatabaseID() As Integer
        Get

            Return m_lPMDatabaseID

        End Get
        Set(ByVal Value As Integer)

            m_lPMDatabaseID = Value

        End Set
    End Property


    Public Property QASDatabaseID() As Integer
        Get

            Return m_lQASDatabaseID

        End Get
        Set(ByVal Value As Integer)

            m_lQASDatabaseID = Value

        End Set
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oPickListNodes.Count()
                    m_lCurrentRecord = m_oPickListNodes.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property


    Public Property QAS2PMAddress1() As String
        Get
            Return m_sQAS2PMAddress1
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress1 = Value
        End Set
    End Property




    Public Property QAS2PMAddress2() As String
        Get
            Return m_sQAS2PMAddress2
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress2 = Value
        End Set
    End Property


    Public Property QAS2PMAddress3() As String
        Get
            Return m_sQAS2PMAddress3
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress3 = Value
        End Set
    End Property



    Public Property QAS2PMAddress4() As String
        Get
            Return m_sQAS2PMAddress4
        End Get
        Set(ByVal Value As String)
            m_sQAS2PMAddress4 = Value
        End Set
    End Property


    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oPickListNodes.Count()

        End Get
    End Property

    ' RDC10072002 new property for QAS warning return codes
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    Public Property WarningMessage() As String
        Get
            Return m_sWarningMessage
        End Get
        Set(ByVal Value As String)
            m_sWarningMessage = Value
        End Set
    End Property
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

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS

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

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            'QAS has not been initialsied
            m_lQASInitialised = gPMConstants.PMEReturnCode.PMFalse

            'RWH(21/09/2000) Create instance of database object for GetCountryCodes
            '    Set oComponentServices = New PMServerBusinessCS

            '    m_lReturn& = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oComponentServices = Nothing

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
            If disposing Then
                m_oPickListNodes = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If m_lQASInitialised = gPMConstants.PMEReturnCode.PMTrue Then
                    Select Case QASDatabaseID
                        Case 1 'if QAS Rapid installed
                            R_QAInitialise(0)
                        Case 3 'if QAS Names installed
                            N_QAInitialise(0)
                        Case Else 'Default to QAS Pro
                            QAInitialise(0)
                    End Select
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PickListNode
    '
    ' ***************************************************************** '
    'DAK140700 - add r_iCountryId
    'JDW for CNIC 21/08/01 added QAS names bits
    Public Function GetDetails(ByVal v_lPickListNodeID As Integer, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "", Optional ByRef r_lPMAddressCnt As Integer = 0, Optional ByRef r_sOrganisation As String = "", Optional ByRef r_lNodeID As Integer = 0, Optional ByRef r_lParentNodeID As Integer = 0, Optional ByRef r_iLevel As Integer = 0, Optional ByRef r_iCount As Integer = 0, Optional ByRef r_iNoOfChildren As Integer = 0, Optional ByRef r_sCaption As String = "", Optional ByRef r_sPOBox As String = "", Optional ByRef r_sSubPremises As String = "", Optional ByRef r_sBuildingName As String = "", Optional ByRef r_sBuildingNumber As String = "", Optional ByRef r_sDependentThoroughfare As String = "", Optional ByRef r_sThoroughfare As String = "", Optional ByRef r_sDependentLocality As String = "", Optional ByRef r_sLocality As String = "", Optional ByRef r_sPostTown As String = "", Optional ByRef r_sCounty As String = "", Optional ByRef r_iCountryId As Integer = 0, Optional ByRef r_sTitle As String = "", Optional ByRef r_sInitial As String = "", Optional ByRef r_sForename As String = "", Optional ByRef r_sSurname As String = "") As Integer

        Dim result As Integer = 0
        Dim oPickListNode As bPMAddressControl.PickListNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oPickListNode = m_oPickListNodes.Item(v_lPickListNodeID)

            ' Get the PickListNode Property Values
            m_lReturn = CType(GetProperties(oPickListNode:=oPickListNode, r_lNodeID:=r_lNodeID, r_lParentNodeID:=r_lParentNodeID, r_iLevel:=r_iLevel, r_iCount:=r_iCount, r_iNoOfChildren:=r_iNoOfChildren, r_sCaption:=r_sCaption), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            With oPickListNode

                'Map the Composite QAS AddressLines to single PM Address Lines
                '(Only if all address lines are empty (meaning that this is QAS mode))
                'Since mapping the address lines from QAS to PM takes time it is only
                'acceptable for the single selected address

                If .AddressLine1 = "" And .AddressLine2 = "" And .AddressLine3 = "" And .AddressLine4 = "" Then
                    .AddressLine1 = MapAddressLines(oPickListNode, m_sQAS2PMAddress1)
                    .AddressLine2 = MapAddressLines(oPickListNode, m_sQAS2PMAddress2)
                    .AddressLine3 = MapAddressLines(oPickListNode, m_sQAS2PMAddress3)
                    .AddressLine4 = MapAddressLines(oPickListNode, m_sQAS2PMAddress4)
                End If

                r_sAddressLine1 = .AddressLine1
                r_sAddressLine2 = .AddressLine2
                r_sAddressLine3 = .AddressLine3
                r_sAddressLine4 = .AddressLine4
                r_lPMAddressCnt = .PMAddressCnt
                r_sPostCode = .PostCode
                r_sOrganisation = .Organisation
                r_sPOBox = .POBox
                r_sSubPremises = .SubPremises
                r_sBuildingName = .BuildingName
                r_sBuildingNumber = .BuildingNumber
                r_sDependentThoroughfare = .DependentThoroughfare
                r_sThoroughfare = .Thoroughfare
                r_sDependentLocality = .DependentLocality
                r_sLocality = .Locality
                r_sPostTown = .PostTown
                r_sCounty = .County
                'JDW 21/08/2001 for CNIC
                r_sTitle = .Title
                r_sForename = .Forename
                r_sInitial = .Initial
                r_sSurname = .Surname

                r_iCountryId = .CountryId
            End With

            oPickListNode = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Details Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddNode (Public)
    '
    ' Description: Adds the supplied PickListNode into the Collection.
    '
    ' ***************************************************************** '
    Public Function AddNode(ByVal v_sNodeType As String, ByRef r_lNodeID As Integer, Optional ByVal v_lParentNodeID As Integer = -1, Optional ByVal v_vAddressArray(,) As Object = Nothing, Optional ByVal v_lHandle As Integer = 1, Optional ByVal nCountryIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oPickListNode As bPMAddressControl.PickListNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PickListNode
            oPickListNode = New bPMAddressControl.PickListNode()

            'developer guide no. 9
            m_lReturn = oPickListNode.Initialise()

            ' Populate PickListNode Attributes

            m_lReturn = CType(SetProperties(r_oPickListNode:=oPickListNode, v_sNodeType:=v_sNodeType, v_lParentNodeID:=v_lParentNodeID, v_vAddressArray:=v_vAddressArray, v_lHandle:=v_lHandle, nCountryIndex:=nCountryIndex), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oPickListNode = Nothing
                Return result
            End If

            ' Return new Node ID
            r_lNodeID = oPickListNode.NodeID

            ' Add PickListNode to collection
            If m_oPickListNodes.Count = 0 Then
                m_oPickListNodes.Add(Nothing)
            End If
            m_lReturn = CType(m_oPickListNodes.Add(oNewPickListNode:=oPickListNode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oPickListNode = Nothing
                Return result
            End If

            oPickListNode = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Node Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindAddress (Public)
    '
    ' Description: Retrieve address details from available databases.
    ' ***************************************************************** '
    'DAK140700 - add r_iCountryId
    'developer guide no. 71(Guide)
    Public Function FindAddress(ByRef r_vAddressArray As Object, ByRef r_vPickList As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "", Optional ByRef r_lPMAddressCnt As Integer = 0, Optional ByVal v_sNumber As String = "", Optional ByVal v_sOrganisation As String = "", Optional ByVal v_sStreet As String = "", Optional ByVal v_sLocality As String = "", Optional ByVal v_sTown As String = "", Optional ByVal v_sCounty As String = "", Optional ByRef r_iCountryId As Integer = 0) As Integer

        ' RDC 130602002 CompServ replaced iwth BAS module
        'Dim oComponentServices As PMServerBusinessCS

        Dim result As Integer = 0
        Dim sSearch As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear arrays
            'developer guide no. 71(Guide)
            'r_vAddressArray = ""
            'r_vPickList = ""

            ' Initialise Pick List collection
            m_oPickListNodes = New bPMAddressControl.PickListNodes()

            ' Only open database if PMDatabaseID set by User Control
            If PMDatabaseID > 0 Then
                ' Make sure not already opened
                If Not m_bCloseDatabase Then
                    m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMDatabaseID, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' Determine current database
                Select Case m_oDatabase.CurrentDSN
                    Case gPMConstants.PMSiriusSolutionsDSN
                        'DAK140700 - add r_iCountryId
                        m_lReturn = CType(FindSiriusSolutionsAddress(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, r_sAddressLine2:=r_sAddressLine2, r_sAddressLine3:=r_sAddressLine3, r_sAddressLine4:=r_sAddressLine4, r_lPMAddressCnt:=r_lPMAddressCnt, r_iCountryId:=r_iCountryId), gPMConstants.PMEReturnCode)
                    Case gPMConstants.PMGeminiDSN
                        'DAK140700 - add r_iCountryId
                        m_lReturn = CType(FindGeminiAddress(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, r_sAddressLine2:=r_sAddressLine2, r_sAddressLine3:=r_sAddressLine3, r_sAddressLine4:=r_sAddressLine4, r_lPMAddressCnt:=r_lPMAddressCnt, r_iCountryId:=r_iCountryId), gPMConstants.PMEReturnCode)
                    Case Else
                        'DAK140700 - add r_iCountryId
                        m_lReturn = CType(FindSiriusSolutionsAddress(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, r_sAddressLine2:=r_sAddressLine2, r_sAddressLine3:=r_sAddressLine3, r_sAddressLine4:=r_sAddressLine4, r_lPMAddressCnt:=r_lPMAddressCnt, r_iCountryId:=r_iCountryId), gPMConstants.PMEReturnCode)
                End Select

                'If we have data then exit
                If Informations.IsArray(r_vPickList) Then
                    If r_vPickList.GetUpperBound(1) > 0 Then
                        Return result
                    End If
                End If

            End If

            Dim sOptionValue As String = String.Empty
            'm_lReturn = bPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionQASDatabaseID, r_sOptionValue:=sOptionValue)
            m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, GeneralConst.kSystemOptionQASDatabaseID, sOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            QASDatabaseID = gPMFunctions.ToSafeInteger(sOptionValue)


            If QASDatabaseID > 0 Then
                Select Case QASDatabaseID
                    Case 1
                        ' RDC 25082000
                        ' Rapid database option enabled on control's database property page
                        m_lReturn = CType(FindQASRapidAddress(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, r_sAddressLine2:=r_sAddressLine2, r_sAddressLine3:=r_sAddressLine3, r_sAddressLine4:=r_sAddressLine4), gPMConstants.PMEReturnCode)
                    Case 2
                        m_lReturn = CType(FindQASPro610Address(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, r_sAddressLine2:=r_sAddressLine2, r_sAddressLine3:=r_sAddressLine3, r_sAddressLine4:=r_sAddressLine4), gPMConstants.PMEReturnCode)
                        'JDW 20/08/01
                    Case 3
                        m_lReturn = CType(FindQASNamesAddress(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, r_sAddressLine2:=r_sAddressLine2, r_sAddressLine3:=r_sAddressLine3, r_sAddressLine4:=r_sAddressLine4), gPMConstants.PMEReturnCode)
                    Case 4
                        m_lReturn = CType(FindPAFAddress(r_vAddressArray:=r_vAddressArray, r_vPickList:=r_vPickList, r_sPostCode:=r_sPostCode, r_sAddressLine1:=r_sAddressLine1, sCountryId:=r_iCountryId), gPMConstants.PMEReturnCode)
                End Select
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Find Address Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindSiriusSolutionsAddress (Private)
    '
    ' Description: Retrieve address details from SIRIUS SOLUTIONS database.
    ' ***************************************************************** '
    'DAK140700 - add r_iCountryId
    ' developer guide no. 71(Guide)
    Private Function FindSiriusSolutionsAddress(ByRef r_vAddressArray As Object, ByRef r_vPickList As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "", Optional ByRef r_lPMAddressCnt As Object = Nothing, Optional ByRef r_iCountryId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSearch As String = ""
        Dim lNodeID As Integer
        Dim lParentNodeID As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create search string
        sSearch = ""


        If (Not Informations.IsNothing(r_lPMAddressCnt)) And (r_lPMAddressCnt > 0) Then
            'DAK140700 - add country_id
            sSearch = "SELECT address_cnt, address1, address2, address3, address4, postal_code, country_id"
            sSearch = sSearch & " FROM Address"
            sSearch = sSearch & " WHERE address_cnt =" & CStr(r_lPMAddressCnt)
        Else

            If Not Informations.IsNothing(r_sAddressLine1) Then
                If r_sAddressLine1 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address1 LIKE '" & AddSQLWildcards(r_sAddressLine1) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sAddressLine2) Then
                If r_sAddressLine2 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address2 LIKE '" & AddSQLWildcards(r_sAddressLine2) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sAddressLine3) Then
                If r_sAddressLine3 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address3 LIKE '" & AddSQLWildcards(r_sAddressLine3) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sAddressLine4) Then
                If r_sAddressLine4 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address4 LIKE '" & AddSQLWildcards(r_sAddressLine4) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sPostCode) Then
                If r_sPostCode <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " postal_code LIKE '" & AddSQLWildcards(r_sPostCode) & "'"
                End If
            End If
            'DAK140700

            If Not Informations.IsNothing(r_iCountryId) Then
                If r_iCountryId > 0 Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " country_id = " & CStr(r_iCountryId)
                End If
            End If

            sSearch = " FROM Address WHERE" & sSearch
            'DAK140700 - add country_id
            sSearch = "SELECT address_cnt, address1, address2, address3, address4, postal_code, country_id" & sSearch
        End If

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=sSearch, sSQLName:="SearchSirius", bStoredProcedure:=False)

            If .Records.Count() > 0 Then

                For lCount As Integer = 0 To .Records.Count()

                    m_lCurrentRecord = lCount


                    'developer guide no. 98
                    m_lReturn = CType(AddNode(v_sNodeType:=gPMConstants.PMSiriusSolutionsDSN, r_lNodeID:=lNodeID, v_lParentNodeID:=lParentNodeID), gPMConstants.PMEReturnCode)

                Next lCount

            End If

        End With

        ' Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count())

        For lCount As Integer = 1 To m_oPickListNodes.Count()

            r_vPickList(0, lCount) = m_oPickListNodes.Item(lCount).NodeID

            r_vPickList(1, lCount) = m_oPickListNodes.Item(lCount).Level

            r_vPickList(2, lCount) = m_oPickListNodes.Item(lCount).Caption

            r_vPickList(3, lCount) = m_oPickListNodes.Item(lCount).ParentNodeID
        Next lCount

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FindGeminiAddress (Private)
    '
    ' Description: Retrieve address details from GEMINI database.
    ' ***************************************************************** '
    'DAK140700 - add r_iCountryId
    Private Function FindGeminiAddress(ByRef r_vAddressArray As Object, ByRef r_vPickList(,) As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "", Optional ByRef r_lPMAddressCnt As Object = Nothing, Optional ByRef r_iCountryId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSearch As String = ""
        Dim lNodeID, lParentNodeID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create search string
        sSearch = ""


        If Not Informations.IsNothing(r_lPMAddressCnt) Then
            If r_lPMAddressCnt > 0 Then
                sSearch = " FROM Address WHERE" & sSearch
                'DAK140700 - add country_id
                sSearch = "SELECT address_cnt, address1, address2, address3, address4, postal_code, country_id" & sSearch
            End If
        Else

            If Not Informations.IsNothing(r_sAddressLine1) Then
                If r_sAddressLine1 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address1 LIKE '" & AddSQLWildcards(r_sAddressLine1) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sAddressLine2) Then
                If r_sAddressLine2 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address2 LIKE '" & AddSQLWildcards(r_sAddressLine2) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sAddressLine3) Then
                If r_sAddressLine3 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address3 LIKE '" & AddSQLWildcards(r_sAddressLine3) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sAddressLine4) Then
                If r_sAddressLine4 <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " address4 LIKE '" & AddSQLWildcards(r_sAddressLine4) & "'"
                End If
            End If

            If Not Informations.IsNothing(r_sPostCode) Then
                If r_sPostCode <> "" Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " postal_code LIKE '" & AddSQLWildcards(r_sPostCode) & "'"
                End If
            End If
            'DAK140700
            If Not False Then
                If r_iCountryId > 0 Then
                    If sSearch > "" Then
                        sSearch = sSearch & " AND"
                    End If
                    sSearch = sSearch & " country_id = " & CStr(r_iCountryId)
                End If
            End If

            sSearch = " FROM Address WHERE" & sSearch
            'DAK140700 - add country_id
            sSearch = "SELECT address_cnt, address1, address2, address3, address4, postal_code, country_id" & sSearch
        End If

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=sSearch, sSQLName:="SearchGemini", bStoredProcedure:=False)

            If .Records.Count() > 1 Then

                For lCount As Integer = 1 To .Records.Count()

                    m_lReturn = CType(AddNode(v_sNodeType:=gPMConstants.PMGeminiDSN, r_lNodeID:=lNodeID, v_lParentNodeID:=lParentNodeID), gPMConstants.PMEReturnCode)

                Next lCount

            End If

        End With

        ' Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count())

        For lCount As Integer = 1 To m_oPickListNodes.Count()

            r_vPickList(0, lCount) = m_oPickListNodes.Item(lCount).NodeID

            r_vPickList(1, lCount) = m_oPickListNodes.Item(lCount).Level

            r_vPickList(2, lCount) = m_oPickListNodes.Item(lCount).Caption

            r_vPickList(3, lCount) = m_oPickListNodes.Item(lCount).ParentNodeID
        Next lCount

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FindQASRapidAddress (Private)
    '
    ' Description: Retrieve address details from QASRapid database.
    ' Updates:
    '     RDC 25082000 - Code cut from FindQASProAddress, and modified
    '                    to use Rapid calls and constants.
    ' ***************************************************************** '
    Private Function FindQASRapidAddress(ByRef r_vAddressArray As Object, ByRef r_vPickList(,) As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "") As Integer

        Dim result As Integer = 0
        Dim sSearch As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' RDC 10072002 new property used to establish if
        ' return code is 'real' error or not
        WarningMessage = ""

        'Build the Search String


        If Not Informations.IsNothing(r_sAddressLine1) Then
            If r_sAddressLine1 <> "" Then
                sSearch = sSearch & r_sAddressLine1 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine2) Then
            If r_sAddressLine2 <> "" Then
                sSearch = sSearch & r_sAddressLine2 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine3) Then
            If r_sAddressLine3 <> "" Then
                sSearch = sSearch & r_sAddressLine3 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine4) Then
            If r_sAddressLine4 <> "" Then
                sSearch = sSearch & r_sAddressLine4 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sPostCode) Then
            If r_sPostCode <> "" Then
                sSearch = sSearch & r_sPostCode & ", "
            End If
        End If

        sSearch = sSearch.Trim()

        If sSearch.Length > 0 Then
            sSearch = sSearch.Substring(0, sSearch.Length - 1)
        End If

        If sSearch = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Initialise QAS
        If m_lQASInitialised = gPMConstants.PMEReturnCode.PMFalse Then

            ' Find the QAddress.ini file location from the registry
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASRapid_Path", r_sSettingValue:=m_sQASPath), gPMConstants.PMEReturnCode)

            'If not found then set to empty string
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sQASPath = ""
            ElseIf FileSystem.Dir(m_sQASPath, FileAttribute.Normal) = "" Then
                m_sQASPath = ""
            End If

            ' Add null terminator for QAS 'C' function
            m_sQASPath = m_sQASPath & Strings.ChrW(0).ToString()

            '(IB)121199 - Added format section to ini file.
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASRapid_Format", r_sSettingValue:=m_sQASFormat), gPMConstants.PMEReturnCode)

            'If not found then set to empty string
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sQASFormat = ""
            End If

            ' Add null terminator for QAS 'C' function
            m_sQASFormat = m_sQASFormat & Strings.ChrW(0).ToString()

            ' Preserve initialisation for multiple calls to QAS
            R_QAInitialise(1)
            m_lQASInitialised = gPMConstants.PMEReturnCode.PMTrue
        End If

        ' Open QAS
        '(IB)121199 - using new format section aswell
        m_lReturn = CType(QARapid_Open(m_sQASPath, m_sQASFormat), gPMConstants.PMEReturnCode)

        If m_lReturn < QASSuccess Then
            QARapid_Close()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call QASRapid Search function
        m_lReturn = CType(QARapid_Search(MakeCString(sSearch)), gPMConstants.PMEReturnCode)

        If m_lReturn < QASSuccess Then

            ' RDC 10072002 some return codes are warnings, so we can proceed

            Select Case m_lReturn
                Case qaerr_AREALEVEL
                    WarningMessage = PMQAS_WARN_AREALEVEL
                Case qaerr_DISTRICTLEVEL
                    WarningMessage = PMQAS_WARN_DISTRICTLEVEL
                Case qaerr_SECTORLEVEL
                    WarningMessage = PMQAS_WARN_SECTORLEVEL
                Case qaerr_HALFSECTORLEVEL
                    WarningMessage = PMQAS_WARN_HALFSECTORLEVEL
                Case qaerr_NUMBEREDFLAT
                    WarningMessage = PMQAS_WARN_NUMBEREDFLAT
                Case qaerr_POSTCODERECODED
                    WarningMessage = PMQAS_WARN_POSTCODERECODED
                Case qaerr_SUBSMADE
                    WarningMessage = PMQAS_WARN_SUBSMADE
                Case Else
                    QARapid_Close()
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select
        End If

        'm_lReturn& = QARapid_Select( _
        'MakeCString("!0"), _
        'vi2:=50, _
        'vi3:=1)

        ' Process Pick Lists
        m_lReturn = CType(ProcessPickLists(v_sNodeType:="QASRapid"), gPMConstants.PMEReturnCode)

        ' Release resources used by QASRapid Search
        QARapid_EndSearch()

        ' Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count())

        For lCount As Integer = 1 To m_oPickListNodes.Count()

            r_vPickList(0, lCount) = m_oPickListNodes.Item(lCount).NodeID

            r_vPickList(1, lCount) = m_oPickListNodes.Item(lCount).Level

            r_vPickList(2, lCount) = m_oPickListNodes.Item(lCount).Caption

            r_vPickList(3, lCount) = m_oPickListNodes.Item(lCount).ParentNodeID
        Next lCount

        QARapid_Close()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FindQASProAddress (Private)
    '
    ' Description: Retrieve address details from QASPro database.
    ' ***************************************************************** '
    Private Function FindQASProAddress(ByRef r_vAddressArray As Object, ByRef r_vPickList(,) As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "") As Integer

        Dim result As Integer = 0
        Dim sSearch As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' RDC 10072002 new property used to establish if
        ' return code is 'real' error or not
        WarningMessage = ""

        'Build the Search String


        If Not Informations.IsNothing(r_sAddressLine1) Then
            If r_sAddressLine1 <> "" Then
                sSearch = sSearch & r_sAddressLine1 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine2) Then
            If r_sAddressLine2 <> "" Then
                sSearch = sSearch & r_sAddressLine2 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine3) Then
            If r_sAddressLine3 <> "" Then
                sSearch = sSearch & r_sAddressLine3 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine4) Then
            If r_sAddressLine4 <> "" Then
                sSearch = sSearch & r_sAddressLine4 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sPostCode) Then
            If r_sPostCode <> "" Then
                sSearch = sSearch & r_sPostCode & ", "
            End If
        End If

        sSearch = sSearch.Trim()

        If sSearch.Length > 0 Then
            sSearch = sSearch.Substring(0, sSearch.Length - 1)
        End If

        If sSearch = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Initialise QAS
        If m_lQASInitialised = gPMConstants.PMEReturnCode.PMFalse Then


            ' Find the QAddress.ini file location from the registry
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_Path", r_sSettingValue:=m_sQASPath), gPMConstants.PMEReturnCode)

            'If not found then set to empty string
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sQASPath = ""
            ElseIf FileSystem.Dir(m_sQASPath, FileAttribute.Normal) = "" Then
                m_sQASPath = ""
            End If

            ' Add null terminator for QAS 'C' function
            m_sQASPath = m_sQASPath & Strings.ChrW(0).ToString()

            '(IB)121199 - Added format section to ini file.
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_Format", r_sSettingValue:=m_sQASFormat), gPMConstants.PMEReturnCode)

            'If not found then set to empty string
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sQASFormat = ""
            End If

            ' Add null terminator for QAS 'C' function
            m_sQASFormat = m_sQASFormat & Strings.ChrW(0).ToString()

            ' Preserve initialisation for multiple calls to QAS
            QAInitialise(1)
            m_lQASInitialised = gPMConstants.PMEReturnCode.PMTrue
        End If

        ' Open QAS
        '(IB)121199 - using new format section aswell
        m_lReturn = CType(QAPro_Open(m_sQASPath, m_sQASFormat), gPMConstants.PMEReturnCode)

        If m_lReturn < QASSuccess Then
            QAPro_Close()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call QASPro Search function
        m_lReturn = CType(QAPro_Search(MakeCString(sSearch)), gPMConstants.PMEReturnCode)

        If m_lReturn < QASSuccess Then

            ' RDC 10072002 some return codes are warnings, so we can proceed

            Select Case m_lReturn
                Case qaerr_AREALEVEL
                    WarningMessage = PMQAS_WARN_AREALEVEL
                Case qaerr_DISTRICTLEVEL
                    WarningMessage = PMQAS_WARN_DISTRICTLEVEL
                Case qaerr_SECTORLEVEL
                    WarningMessage = PMQAS_WARN_SECTORLEVEL
                Case qaerr_HALFSECTORLEVEL
                    WarningMessage = PMQAS_WARN_HALFSECTORLEVEL
                Case qaerr_NUMBEREDFLAT
                    WarningMessage = PMQAS_WARN_NUMBEREDFLAT
                Case qaerr_POSTCODERECODED
                    WarningMessage = PMQAS_WARN_POSTCODERECODED
                Case qaerr_SUBSMADE
                    WarningMessage = PMQAS_WARN_SUBSMADE
                Case Else
                    QAPro_Close()
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

        End If

        'm_lReturn& = QAPro_Select( _
        'MakeCString("!0"), _
        'vi2:=50, _
        'vi3:=1)

        ' Process Pick Lists
        m_lReturn = CType(ProcessPickLists(v_sNodeType:="QASPro"), gPMConstants.PMEReturnCode)

        ' Release resources used by QASPro Search
        QAPro_EndSearch()

        ' Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count())

        For lCount As Integer = 1 To m_oPickListNodes.Count()

            r_vPickList(0, lCount) = m_oPickListNodes.Item(lCount).NodeID

            r_vPickList(1, lCount) = m_oPickListNodes.Item(lCount).Level

            r_vPickList(2, lCount) = m_oPickListNodes.Item(lCount).Caption

            r_vPickList(3, lCount) = m_oPickListNodes.Item(lCount).ParentNodeID
        Next lCount

        QAPro_Close()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FindQASNamesAddress (Private)
    '
    ' Description: Retrieve address details from QASNames database.
    ' ***************************************************************** '
    Private Function FindQASNamesAddress(ByRef r_vAddressArray As Object, ByRef r_vPickList(,) As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "") As Integer

        Dim result As Integer = 0
        Dim sSearch As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' RDC 10072002 new property used to establish if
        ' return code is 'real' error or not
        WarningMessage = ""

        'Build the Search String


        If Not Informations.IsNothing(r_sAddressLine1) Then
            If r_sAddressLine1 <> "" Then
                sSearch = sSearch & r_sAddressLine1 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine2) Then
            If r_sAddressLine2 <> "" Then
                sSearch = sSearch & r_sAddressLine2 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine3) Then
            If r_sAddressLine3 <> "" Then
                sSearch = sSearch & r_sAddressLine3 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sAddressLine4) Then
            If r_sAddressLine4 <> "" Then
                sSearch = sSearch & r_sAddressLine4 & ", "
            End If
        End If


        If Not Informations.IsNothing(r_sPostCode) Then
            If r_sPostCode <> "" Then
                sSearch = sSearch & r_sPostCode & ", "
            End If
        End If

        sSearch = sSearch.Trim()

        If sSearch.Length > 0 Then
            sSearch = sSearch.Substring(0, sSearch.Length - 1)
        End If

        If sSearch = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Initialise QAS
        If m_lQASInitialised = gPMConstants.PMEReturnCode.PMFalse Then


            ' Find the QAddress.ini file location from the registry
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASNames_Path", r_sSettingValue:=m_sQASPath), gPMConstants.PMEReturnCode)

            'If not found then set to empty string
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_sQASPath = ""
            ElseIf FileSystem.Dir(m_sQASPath, FileAttribute.Normal) = "" Then
                m_sQASPath = ""
            End If

            ' Add null terminator for QAS 'C' function
            m_sQASPath = m_sQASPath & Strings.ChrW(0).ToString()

            ' Preserve initialisation for multiple calls to QAS
            N_QAInitialise(1)
            m_lQASInitialised = gPMConstants.PMEReturnCode.PMTrue
        End If

        ' Open QAS
        '(IB)121199 - using new format section aswell
        m_lReturn = CType(QANames_Open(m_sQASPath, ""), gPMConstants.PMEReturnCode)

        If m_lReturn < QASSuccess Then
            QANames_Close()
            'JES REMOVE
            'MessageBox.Show("Can't open QAS Ret:" & m_lReturn, Application.ProductName)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call QASNames Search function
        m_lReturn = CType(QANames_Search(MakeCString(sSearch)), gPMConstants.PMEReturnCode)

        If m_lReturn < QASSuccess Then

            Select Case m_lReturn
                Case qaerr_AREALEVEL
                    WarningMessage = PMQAS_WARN_AREALEVEL
                Case qaerr_DISTRICTLEVEL
                    WarningMessage = PMQAS_WARN_DISTRICTLEVEL
                Case qaerr_SECTORLEVEL
                    WarningMessage = PMQAS_WARN_SECTORLEVEL
                Case qaerr_HALFSECTORLEVEL
                    WarningMessage = PMQAS_WARN_HALFSECTORLEVEL
                Case qaerr_NUMBEREDFLAT
                    WarningMessage = PMQAS_WARN_NUMBEREDFLAT
                Case qaerr_POSTCODERECODED
                    WarningMessage = PMQAS_WARN_POSTCODERECODED
                Case qaerr_SUBSMADE
                    WarningMessage = PMQAS_WARN_SUBSMADE
                Case Else
                    QANames_Close()
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

        End If

        ' Process Pick Lists
        m_lReturn = CType(ProcessPickLists(v_sNodeType:="QASNames"), gPMConstants.PMEReturnCode)

        ' Release resources used by QASPro Search
        QANames_EndSearch()

        ' Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count())

        For lCount As Integer = 1 To m_oPickListNodes.Count()

            r_vPickList(0, lCount) = m_oPickListNodes.Item(lCount).NodeID

            r_vPickList(1, lCount) = m_oPickListNodes.Item(lCount).Level

            r_vPickList(2, lCount) = m_oPickListNodes.Item(lCount).Caption

            r_vPickList(3, lCount) = m_oPickListNodes.Item(lCount).ParentNodeID
        Next lCount

        QANames_Close()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetQASAddressComponents (Private)
    '
    ' Description: Retrieve address component details from QAS
    ' ***************************************************************** '
    ' RDC 05092000 Optional parm added for QASRapid integration - passed to GetQASAddressLine
    Private Function SetQASAddressComponents(ByRef r_oPickListNode As bPMAddressControl.PickListNode, Optional ByVal v_sNodeType As String = "", Optional ByVal v_lHandle As Integer = 1) As Integer

        Dim result As Integer = 0
        Dim sBuffer As String = ""
        Dim lInfo As Long
        Dim lLineCount As Long



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_sNodeType = "QASPro" Then
            With r_oPickListNode
                m_lReturn = QA_FormatResult(vi1:=v_lHandle, vi2:=m_lCurrentListItem, vs3:="", ri4:=lLineCount, rl5:=lInfo)

                m_lReturn = GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE1, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType, v_lHandle:=v_lHandle)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    SetQASAddressComponents = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                .AddressLine1 = sBuffer

                m_lReturn = GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE2, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType, v_lHandle:=v_lHandle)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    SetQASAddressComponents = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                .AddressLine2 = sBuffer

                m_lReturn = GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE3, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType, v_lHandle:=v_lHandle)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    SetQASAddressComponents = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                .AddressLine3 = sBuffer

                m_lReturn = GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE4, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType, v_lHandle:=v_lHandle)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    SetQASAddressComponents = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                .AddressLine4 = sBuffer

                m_lReturn = GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_POSTCODE, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType, v_lHandle:=v_lHandle)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    SetQASAddressComponents = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                .PostCode = sBuffer

            End With

            Exit Function
        End If


        With r_oPickListNode
            sBuffer = ""
            ' Get Organisation item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_ORGANISATION, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            ' RDC 05092000 - not a QASRapid element
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            .Organisation = sBuffer

            sBuffer = ""
            ' Get PO Box item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_POBOX, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            ' RDC 05092000 - not a QASRapid element
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .POBox = sBuffer

            sBuffer = ""
            ' Get Sub Premises item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_SUBPREMISES, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            ' RDC 05092000 - not a QASRapid element
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .SubPremises = sBuffer

            sBuffer = ""
            ' Get Building Name item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            ' RDC 05092000 - not a QASRapid element
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .BuildingName = sBuffer

            sBuffer = ""
            ' Get Building Number item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNUMBER, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            ' RDC 05092000 - not a QASRapid element
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .BuildingNumber = sBuffer

            sBuffer = ""
            ' Get Dependent Thoroughfare item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_DEPTHORO, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .DependentThoroughfare = sBuffer

            sBuffer = ""
            ' Get Thoroughfare item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_THORO, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .Thoroughfare = sBuffer

            sBuffer = ""
            ' Get Dependent Locality item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_DEPLOCAL, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .DependentLocality = sBuffer

            sBuffer = ""
            ' Get Locality item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_LOCAL, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .Locality = sBuffer

            sBuffer = ""
            ' Get Post Town item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_POSTTOWN, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .PostTown = sBuffer

            sBuffer = ""
            ' Get County item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_COUNTY, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            .County = sBuffer

            sBuffer = ""
            ' Get Post Code item
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_POSTCODE, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> PMTrue) Then
            If v_sNodeType <> "QASRapid" And m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result =  gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Jes 24 October 2002
            .PostCode = RemoveSpace(sBuffer)

            'JDW added for CNIC 20/08/01
            If v_sNodeType = "QASNamesName" Then
                'get title
                sBuffer = ""
                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_TITLE, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result =  gPMConstants.PMEReturnCode.PMFalse
                End If

                .Title = sBuffer

                'get forename
                sBuffer = ""
                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_FORENAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result =  gPMConstants.PMEReturnCode.PMFalse
                End If

                .Forename = sBuffer

                'get initial
                sBuffer = ""
                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_INITIAL, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result =  gPMConstants.PMEReturnCode.PMFalse
                End If

                .Initial = sBuffer

                'get surname
                sBuffer = ""
                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_SURNAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result =  gPMConstants.PMEReturnCode.PMFalse
                End If

                .Surname = sBuffer
            End If

            sBuffer = ""
            m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_POSTCODE, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

        End With

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetQASAddressLines (Private)
    '
    ' Description: Retrieve QAS address lines that map to a single PM address
    ' ***************************************************************** '
    Private Function GetQASAddressLines(ByRef sAddressIDList As String) As List(Of Object)

        Dim result As List(Of Object) = Nothing
        Dim iStrtPos, iSepPos As Integer



        result = New List(Of Object)

        If sAddressIDList.Trim() = "" Then
            Return result
        End If

        iStrtPos = 1
        iSepPos = 1

        iSepPos = InStr(iStrtPos, sAddressIDList, ",")

        While iSepPos <> 0
            result.Add(ToSafeInteger((sAddressIDList.Substring(iStrtPos - 1, Math.Min(sAddressIDList.Length, iSepPos - iStrtPos)))))
            iStrtPos = iSepPos + 1

            iSepPos = InStr(iStrtPos, sAddressIDList, ",")
        End While

        iSepPos = sAddressIDList.Length + 1
        result.Add(ToSafeInteger((sAddressIDList.Substring(iStrtPos - 1, Math.Min(sAddressIDList.Length, iSepPos - iStrtPos)))))

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: ProcessPickLists (Private)
    '
    ' Description: Create Node objects for each listable address item
    '
    ' ***************************************************************** '
    Private Function ProcessPickLists(ByVal v_sNodeType As String, Optional ByVal v_lParentNodeID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lItemCount, lResult, lNodeID As Integer
        Dim sBuffer As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        Select Case v_sNodeType
            Case gPMConstants.PMSiriusDSN
                With m_oDatabase
                    For lCount As Integer = 1 To .Records.Count()
                        ' Add this node only
                        m_lReturn = CType(AddNode(v_sNodeType:=v_sNodeType, r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID), gPMConstants.PMEReturnCode)
                    Next lCount
                End With

            Case "QASPro"

                ' Call QASPro Item Count function
                Select Case m_lQASDatabaseID
                    Case 3 'if QAS Names installed
                        lItemCount = N_QAPro_Count()
                    Case Else 'Default to QAS Pro
                        lItemCount = QAPro_Count()
                End Select

                For lCount As Integer = 0 To lItemCount - 1

                    m_lCurrentListItem = lCount

                    Select Case m_lQASDatabaseID
                        Case 3 'if QAS Names installed
                            m_lReturn = CType(N_QAPro_GetItemInfo(vl1:=lCount, vi2:=qapro_STEPINFO, rl3:=lResult), gPMConstants.PMEReturnCode)
                        Case Else 'Default to QAS Pro
                            m_lReturn = CType(QAPro_GetItemInfo(vl1:=lCount, vi2:=qapro_STEPINFO, rl3:=lResult), gPMConstants.PMEReturnCode)
                    End Select

                    If lResult <> 0 Then
                        ' Add this node if not range
                        Select Case m_lQASDatabaseID
                            Case 3 'if QAS Names installed
                                m_lReturn = CType(N_QAPro_GetItemInfo(vl1:=lCount, vi2:=qapro_TYPEINFO, rl3:=lResult), gPMConstants.PMEReturnCode)
                            Case Else 'Default to QAS Pro
                                m_lReturn = CType(QAPro_GetItemInfo(vl1:=lCount, vi2:=qapro_TYPEINFO, rl3:=lResult), gPMConstants.PMEReturnCode)
                        End Select

                        If lResult = 0 Then
                            m_lReturn = CType(AddNode(v_sNodeType:="QASPro", r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID), gPMConstants.PMEReturnCode)
                        Else
                            lNodeID = v_lParentNodeID
                        End If

                        ' Step into next level
                        Select Case m_lQASDatabaseID
                            Case 3 'if QAS Names installed
                                m_lReturn = CType(N_QAPro_StepIn(vl1:=lCount), gPMConstants.PMEReturnCode)
                            Case Else 'Default to QAS Pro
                                m_lReturn = CType(QAPro_StepIn(vl1:=lCount), gPMConstants.PMEReturnCode)
                        End Select

                        ' Call this function recursively to process next level
                        m_lReturn = CType(ProcessPickLists(v_sNodeType:="QASPro", v_lParentNodeID:=lNodeID), gPMConstants.PMEReturnCode)

                        ' Step back out
                        Select Case m_lQASDatabaseID
                            Case 3 'if QAS Names installed
                                m_lReturn = CType(N_QAPro_StepOut(), gPMConstants.PMEReturnCode)
                            Case Else 'Default to QAS Pro
                                m_lReturn = CType(QAPro_StepOut(), gPMConstants.PMEReturnCode)
                        End Select
                    Else
                        ' Add this node only
                        m_lReturn = CType(AddNode(v_sNodeType:="QASPro", r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID), gPMConstants.PMEReturnCode)
                    End If

                Next lCount

            Case "QASRapid"
                ' RDC 25082000
                ' Code cut from QASPro section above and modified for Rapid

                ' Call QASRapid Item Count function
                lItemCount = QARapid_Count()

                For lCount As Integer = 0 To lItemCount - 1

                    m_lReturn = CType(AddNode(v_sNodeType:="QASRapid", r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID), gPMConstants.PMEReturnCode)

                Next lCount

                'JDW CNIC addition 20/08/2001 for QASNames
            Case "QASNames"


                ' Call QASNames Item Count function
                lItemCount = QANames_Count()

                For lCount As Integer = 0 To lItemCount - 1

                    m_lCurrentListItem = lCount
                    'can we step in ? 1=yes 0=no
                    m_lReturn = CType(QANames_GetItemInfo(vl1:=lCount, vi2:=qanames_STEPINFO, rl3:=lResult), gPMConstants.PMEReturnCode)

                    'this loop excludes end name nodes
                    If lResult <> 0 Then
                        'add node
                        m_lReturn = CType(AddNode(v_sNodeType:="QASNames", r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID), gPMConstants.PMEReturnCode)

                        ' Step into next level
                        m_lReturn = CType(QANames_StepIn(vl1:=lCount), gPMConstants.PMEReturnCode)

                        ' Call this function recursively to process next level
                        m_lReturn = CType(ProcessPickLists(v_sNodeType:="QASNames", v_lParentNodeID:=lNodeID), gPMConstants.PMEReturnCode)

                        ' Step back out
                        m_lReturn = CType(QANames_Back(), gPMConstants.PMEReturnCode)
                    Else
                        'check to see if node is empty
                        sBuffer = "" & Strings.ChrW(0).ToString()
                        m_lReturn = CType(QANames_AddrLine(vl1:=lCount, vi2:=qanamefields_SURNAME, rs3:=sBuffer, vi4:=255), gPMConstants.PMEReturnCode)

                        If sBuffer.Trim() = Strings.ChrW(0).ToString() Then
                            'dont do anything
                        Else
                            ' Add name node
                            m_lReturn = CType(AddNode(v_sNodeType:="QASNamesName", r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID), gPMConstants.PMEReturnCode)
                        End If
                    End If

                Next lCount

            Case Else

        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetQASAddressLine (Private)
    '
    ' Description: Retrieve individual address line from QAS list
    ' ***************************************************************** '
    Public Function GetQASAddressLine(ByVal v_lItemID As Integer, ByVal v_iLineID As Integer, ByRef r_sAddressLine As String, Optional ByVal v_sNodeType As String = "", Optional ByVal v_lHandle As Integer = 1) As Integer

        Dim result As Integer = 0
        Dim sBuffer, sBuffer2 As String
        Dim lInfo As Long
        Dim lFLagFormated As Long

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set empty buffers for QAS 'C' function
            sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
            sBuffer2 = New String(" "c, 255) & Strings.ChrW(0).ToString()

            ' Get Address line
            '(IB)111199 - If requested to use ini file format then make
            'sure use function which returns lines using it
            'otherwise use original raw format
            'If m_sQASFormat <> "" Then

            ' CTAF 180100 - Fixed problem when not having a Format as m_sQASFormat is sufixed
            ' with a NULL character at the start of the program, so this was always going into QASPro_FormatLine
            ' RDC 05092000 Add QASRapid functions - switched by optional function parm v_sNodeType$
            Select Case v_sNodeType
                ' QASRapid
                Case "QASRapid"
                    If m_sQASFormat <> Strings.ChrW(0).ToString() Then
                        m_lReturn = CType(QARapid_FormatLine(vl1:=v_lItemID, vi2:=v_iLineID, rs3:=sBuffer, vi4:=255), gPMConstants.PMEReturnCode)
                    Else
                        m_lReturn = CType(QARapid_AddrLine(vl1:=v_lItemID, vi2:=v_iLineID, rs3:=sBuffer, vi4:=256), gPMConstants.PMEReturnCode)
                    End If

                    ' Names
                Case "QASNames"
                    m_lReturn = CType(QANames_AddrLine(vl1:=v_lItemID, vi2:=v_iLineID, rs3:=sBuffer, vi4:=255), gPMConstants.PMEReturnCode)

                Case "QASNamesName"
                    m_lReturn = CType(QANames_AddrLine(vl1:=v_lItemID, vi2:=v_iLineID, rs3:=sBuffer, vi4:=255), gPMConstants.PMEReturnCode)

                    ' QASPro
                Case Else
                    If m_sQASFormat <> Strings.ChrW(0).ToString() Then
                        Select Case m_lQASDatabaseID
                            Case 3 'if QAS Names installed
                                m_lReturn = CType(N_QAPro_FormatLine(vl1:=v_lItemID, vi2:=v_iLineID, vs3:=sBuffer2, rs4:=sBuffer, vi5:=255), gPMConstants.PMEReturnCode)
                            Case Else 'Default to QAS Pro
                                m_lReturn = CType(QA_GetFormattedLine(vi1:=v_lHandle, vi2:=v_iLineID, rs3:=sBuffer, vi4:=255, rs5:=sBuffer2, vi6:=255, rl7:=lInfo), gPMConstants.PMEReturnCode)
                                'm_lReturn = CType(QAPro_FormatLine(vl1:=v_lItemID, vi2:=v_iLineID, vs3:=sBuffer2, rs4:=sBuffer, vi5:=255), gPMConstants.PMEReturnCode)
                        End Select
                    Else
                        m_lReturn = CType(QAPro_AddrLine(vl1:=v_lItemID, vi2:=v_iLineID, vs3:=sBuffer2, rs4:=sBuffer, vi5:=255), gPMConstants.PMEReturnCode)

                        Select Case m_lQASDatabaseID
                            Case 3 'if QAS Names installed
                                m_lReturn = CType(N_QAPro_AddrLine(vl1:=v_lItemID, vi2:=v_iLineID, vs3:=sBuffer2, rs4:=sBuffer, vi5:=255), gPMConstants.PMEReturnCode)
                            Case Else 'Default to QAS Pro

                                m_lReturn = CType(QA_GetFormattedLine(vi1:=v_lHandle, vi2:=v_iLineID%, rs3:=sBuffer2, vi4:=sBuffer, rs5:=sBuffer, vi6:=255, rl7:=lFLagFormated), gPMConstants.PMEReturnCode)
                                'm_lReturn = CType(QAPro_AddrLine(vl1:=v_lHandle, vi2:=v_iLineID, rs3:=sBuffer, vi4:=255, rs5:=sBuffer2, vi6:=255, rl7:=lInfo), gPMConstants.PMEReturnCode)
                        End Select
                    End If
            End Select

            'MKW 140203 1.6.9 --> 1.8.6 Catchup PN1548
            'If (m_lReturn& <> QASSuccess) Then
            If ((m_lReturn <> gPMConstants.PMEReturnCode.PMFalse) And PMTrim(sBuffer).Length = 1) Or (m_lReturn = QASNotDefined) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sAddressLine = ""
                Return result
            End If

            r_sAddressLine = PMTrim(sBuffer)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get QAS Address Line Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQASAddressLine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PickListNode property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef r_oPickListNode As bPMAddressControl.PickListNode, ByVal v_sNodeType As String, Optional ByVal v_lParentNodeID As Integer = -1, Optional ByVal v_vAddressArray(,) As Object = Nothing, Optional ByVal v_lHandle As Integer = 1, Optional ByVal nCountryIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sBuffer, sField As String
        Dim lDetail As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With r_oPickListNode

            ' Set consecutive ID of new node
            If nCountryIndex > 0 Then
                .NodeID = nCountryIndex + 1
            Else
                .NodeID = m_oPickListNodes.Count() + 1
            End If

            ' Set Node Type
            .NodeType = v_sNodeType

            ' Set properties relating to parent node

            If (Not False) And (Not v_lParentNodeID.Equals(0)) Then
                If v_lParentNodeID > -1 Then
                    .ParentNodeID = v_lParentNodeID
                    With m_oPickListNodes.Item(v_lParentNodeID)
                        ' Level is one greater than parent
                        r_oPickListNode.Level = .Level + 1
                        ' Set consecutive child count
                        r_oPickListNode.Count = .NoOfChildren + 1
                        ' Increment parent object NoOfChildren
                        .NoOfChildren += 1
                    End With
                Else
                    .ParentNodeID = 0
                    .Level = 0
                    .Count = 0
                    .NoOfChildren = 0
                End If
            End If
            ' Set caption
            Select Case .NodeType
                Case gPMConstants.PMSiriusSolutionsDSN
                    sBuffer = ""
                    With m_oDatabase.Records.Item(m_lCurrentRecord)

                        sField = Convert.ToString(Convert.ToString(.Fields()("address1"))).Trim()
                        If sField <> "" Then
                            If sBuffer <> "" Then
                                sBuffer = sBuffer & ","
                            End If
                            sBuffer = sBuffer & Convert.ToString(.Fields()("address1"))
                        End If
                        r_oPickListNode.AddressLine1 = sField

                        sField = Convert.ToString(.Fields()("address2")).Trim()
                        If sField <> "" Then
                            If sBuffer <> "" Then
                                sBuffer = sBuffer & ","
                            End If
                            sBuffer = sBuffer & Convert.ToString(.Fields()("address2"))
                        End If
                        r_oPickListNode.AddressLine2 = sField

                        sField = Convert.ToString(.Fields()("address3")).Trim()
                        If sField <> "" Then
                            If sBuffer <> "" Then
                                sBuffer = sBuffer & ","
                            End If
                            sBuffer = sBuffer & Convert.ToString(.Fields()("address3"))
                        End If
                        r_oPickListNode.AddressLine3 = sField

                        sField = Convert.ToString(.Fields()("address4")).Trim()
                        If sField <> "" Then
                            If sBuffer <> "" Then
                                sBuffer = sBuffer & ","
                            End If
                            sBuffer = sBuffer & Convert.ToString(.Fields()("address4"))
                        End If
                        r_oPickListNode.AddressLine4 = sField

                        sField = Convert.ToString(.Fields()("postal_code")).Trim()
                        If sField <> "" Then
                            If sBuffer <> "" Then
                                sBuffer = sBuffer & ","
                            End If
                            sBuffer = sBuffer & Convert.ToString(.Fields()("postal_code"))
                        End If
                        r_oPickListNode.PostCode = sField

                        sField = Convert.ToString(.Fields()("address_cnt")).Trim()
                        r_oPickListNode.PMAddressCnt = ToSafeInteger(sField)
                        'DAK140700
                        sField = Convert.ToString(.Fields()("country_id")).Trim()
                        r_oPickListNode.CountryId = ToSafeInteger(sField)

                    End With
                    .Caption = sBuffer
                Case gPMConstants.PMGeminiDSN
                Case Else
                    Select Case QASDatabaseID
                        ' QASRapid
                        ' RDC 05092000 lifted from QASPro version
                        ' API calls changed to reflect declarations in QASRapidConst.Bas
                        Case 1
                            If .ParentNodeID = 0 Then
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(QARapid_ListItem(vl1:=m_lCurrentListItem, rs2:=sBuffer, vi3:=0, vi4:=255), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = PMTrim(sBuffer)
                            Else
                                ' Add Organisation Name
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_ORGANISATION, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = .Caption & PMTrim(sBuffer)

                                ' Add Sub-premise Name
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_SUBPREMISES, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                If .Caption <> "" And sBuffer <> "" Then
                                    .Caption = .Caption & ", "
                                End If
                                .Caption = .Caption & PMTrim(sBuffer)

                                ' Add Building Name
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                If .Caption <> "" And sBuffer <> "" Then
                                    .Caption = .Caption & ", "
                                End If
                                .Caption = .Caption & PMTrim(sBuffer)

                                ' Add Building Number
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNUMBER, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                If .Caption <> "" And sBuffer <> "" Then
                                    .Caption = .Caption & ", "
                                End If
                                .Caption = .Caption & PMTrim(sBuffer)
                            End If

                            m_lReturn = CType(SetQASAddressComponents(r_oPickListNode:=r_oPickListNode, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                            ' QASPro
                        Case 2

                            If .ParentNodeID = 0 Then
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(QA_GetResultDetail(vi1:=v_lHandle, vi2:=m_lCurrentListItem, vi3:=qaproresultstr_PARTIALADDRESS, rl4:=lDetail, rs5:=sBuffer, vi6:=255), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = PMTrim(sBuffer)
                            Else
                                ' Add Organisation Name
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                'm_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_ORGANISATION, r_sAddressLine:=sBuffer), gPMConstants.PMEReturnCode)
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE1, r_sAddressLine:=sBuffer, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = .Caption & PMTrim(sBuffer)

                                ' Add Sub-premise Name
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                'm_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_SUBPREMISES, r_sAddressLine:=sBuffer), gPMConstants.PMEReturnCode)
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE2, r_sAddressLine:=sBuffer, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                If .Caption <> "" And sBuffer <> "" Then
                                    .Caption = .Caption & ", "
                                End If
                                .Caption = .Caption & PMTrim(sBuffer)

                                ' Add Building Name
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                'm_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNAME, r_sAddressLine:=sBuffer), gPMConstants.PMEReturnCode)
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE3, r_sAddressLine:=sBuffer, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                If .Caption <> "" And sBuffer <> "" Then
                                    .Caption = .Caption & ", "
                                End If
                                .Caption = .Caption & PMTrim(sBuffer)

                                ' Add Building Number
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                'm_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNUMBER, r_sAddressLine:=sBuffer), gPMConstants.PMEReturnCode)
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qaprofields_ADDRESSLINE4, r_sAddressLine:=sBuffer, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                If .Caption <> "" And sBuffer <> "" Then
                                    .Caption = .Caption & ", "
                                End If
                                .Caption = .Caption & PMTrim(sBuffer)
                            End If
                            m_lReturn = CType(SetQASAddressComponents(r_oPickListNode:=r_oPickListNode, v_sNodeType:=v_sNodeType, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)

                            ' QASNames
                        Case 3
                            'put together a presentation name for node
                            If .NodeType = "QASNamesName" Then
                                'title
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_TITLE, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = PMTrim(sBuffer)
                                'forename
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_FORENAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = .Caption & " " & PMTrim(sBuffer)
                                'surname
                                sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qanamefields_SURNAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                sBuffer = UnMakeCString(sBuffer)
                                .Caption = .Caption & " " & PMTrim(sBuffer)

                                If .Caption = "  " Then

                                Else
                                    m_lReturn = CType(SetQASAddressComponents(r_oPickListNode:=r_oPickListNode, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                End If

                            Else

                                If .ParentNodeID = 0 Then
                                    sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                    m_lReturn = CType(QANames_ListItem(vl1:=m_lCurrentListItem, rs2:=sBuffer, vi3:=0, vi4:=255), gPMConstants.PMEReturnCode)
                                    sBuffer = UnMakeCString(sBuffer)
                                    .Caption = PMTrim(sBuffer)
                                Else
                                    'orgname
                                    sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                    m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_ORGANISATION, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                    sBuffer = UnMakeCString(sBuffer)
                                    .Caption = PMTrim(sBuffer)

                                    ' Add Building Name
                                    sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                    m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNAME, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                    sBuffer = UnMakeCString(sBuffer)
                                    If .Caption <> "" And sBuffer <> "" Then
                                        .Caption = .Caption & ", "
                                    End If
                                    .Caption = .Caption & PMTrim(sBuffer)

                                    ' Add Building Number
                                    sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                    m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_BUILDINGNUMBER, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                    sBuffer = UnMakeCString(sBuffer)
                                    If .Caption <> "" And sBuffer <> "" Then
                                        .Caption = .Caption & ", "
                                    End If
                                    .Caption = .Caption & PMTrim(sBuffer)

                                    ' Add Sub Premises - PKH(CMG)25/07/02
                                    sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                    m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_SUBPREMISES, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                    sBuffer = UnMakeCString(sBuffer)
                                    '                                If ((.Caption <> "" And sBuffer$ <> "") = True) Then
                                    '                                    .Caption = .Caption & ", "
                                    '                                End If
                                    .Caption = .Caption & PMTrim(sBuffer)

                                    ' Add Thoroughfare
                                    sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                                    m_lReturn = CType(GetQASAddressLine(v_lItemID:=m_lCurrentListItem, v_iLineID:=qafields_THORO, r_sAddressLine:=sBuffer, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)
                                    sBuffer = UnMakeCString(sBuffer)
                                    If .Caption <> "" And sBuffer <> "" Then
                                        .Caption = .Caption & ", "
                                    End If
                                    .Caption = .Caption & PMTrim(sBuffer)

                                    m_lReturn = CType(SetQASAddressComponents(r_oPickListNode:=r_oPickListNode, v_sNodeType:=v_sNodeType), gPMConstants.PMEReturnCode)

                                End If
                                'node type
                            End If

                            ' PAFWrapper
                        Case 4
                            sBuffer = New String(" "c, 255) & Strings.ChrW(0).ToString()
                            .AddressLine1 = CStr(v_vAddressArray(0, .NodeID - 1))
                            sBuffer = CStr(v_vAddressArray(0, .NodeID - 1))
                            .AddressLine2 = CStr(v_vAddressArray(1, .NodeID - 1))
                            sBuffer = sBuffer & "," & CStr(v_vAddressArray(1, .NodeID - 1))
                            .AddressLine3 = CStr(v_vAddressArray(2, .NodeID - 1))
                            sBuffer = sBuffer & "," & CStr(v_vAddressArray(2, .NodeID - 1))
                            .AddressLine4 = CStr(v_vAddressArray(3, .NodeID - 1))
                            sBuffer = sBuffer & "," & CStr(v_vAddressArray(3, .NodeID - 1))
                            .PostCode = CStr(v_vAddressArray(4, .NodeID - 1))
                            sBuffer = sBuffer & "," & CStr(v_vAddressArray(4, .NodeID - 1))
                            .CountryId = ToSafeInteger(CStr(v_vAddressArray(5, .NodeID - 1)))
                            sBuffer = sBuffer & "," & CStr(v_vAddressArray(5, .NodeID - 1))
                            sBuffer = UnMakeCString(sBuffer)
                            .Caption = PMTrim(sBuffer)
                    End Select
                    'DB or QAS
            End Select
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PickListNode property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oPickListNode As bPMAddressControl.PickListNode, Optional ByRef r_lNodeID As Integer = -1, Optional ByRef r_lParentNodeID As Integer = -1, Optional ByRef r_iLevel As Integer = -1, Optional ByRef r_iCount As Integer = -1, Optional ByRef r_iNoOfChildren As Integer = -1, Optional ByRef r_sCaption As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPickListNode

            If Not False Then
                r_lNodeID = .NodeID
            End If

            If Not False Then
                r_lParentNodeID = .ParentNodeID
            End If

            If Not False Then
                r_iLevel = .Level
            End If

            If Not False Then
                r_iCount = .Count
            End If

            If Not False Then
                r_iNoOfChildren = .NoOfChildren
            End If

            If Not False Then
                r_sCaption = .Caption
            End If

        End With

        Return result

    End Function

    Private Function PMTrim(ByVal v_sData As String) As String

        Dim result As String = String.Empty
        Dim iCharPos As Integer



        ' Remove Null character onwards
        iCharPos = (v_sData.IndexOf(Strings.ChrW(0).ToString()) + 1)
        If iCharPos > 0 Then
            v_sData = v_sData.Substring(0, iCharPos - 1)
        End If

        ' Do standard trim
        v_sData = v_sData.Trim()

        'Remove Additional Obsolete data

        'Get Rid of Plus
        If v_sData.StartsWith("+") Then
            v_sData = v_sData.Substring(v_sData.Length - (v_sData.Length - 1))
        End If

        'Get rid of percentage
        If v_sData.EndsWith("%") Then
            v_sData = v_sData.Substring(0, v_sData.Length - 4)
        End If

        ' Do standard trim
        v_sData = v_sData.Trim()

        ' Set function return to new string

        Return v_sData

    End Function

    Private Function AddSQLWildcards(ByVal v_sData As String) As String

        Dim result As String = String.Empty
        Dim iCharPos As Integer



        ' Replace spaces with %
        Do
            iCharPos = (v_sData.IndexOf(" "c) + 1)
            If iCharPos = 0 Then
                Exit Do
            Else
                Mid(v_sData, iCharPos, 1) = "%"
            End If
        Loop

        ' Do standard trim
        v_sData = v_sData.Trim()

        ' Set function return to new string

        Return v_sData & "%"

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetCountryCodes
    '
    ' Description: Retrieves all country_id's and their associated codes.
    '
    ' History: 15/09/2000 RWH - Created.
    ' SP050202 - should check ubound of array not count
    ' PW280602 - return the country iso_code instead (this should be
    '            ok 'cos this method is always used for the same
    '            purpose)
    ' ***************************************************************** '
    Public Function GetCountryCodes(ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSQLString As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'PW280602 - select the iso_code instead
            sSQLString = "spu_get_country_config"

            'If m_oDatabase.Parameters.Count() > 0 Then
            m_oDatabase.Parameters.Clear()
            'End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLString, sSQLName:="spu_get_country_config", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            'SP050202
            'lRecordCount& = m_oDatabase.Records.Count

            ' Do we have any records ?
            If Not Informations.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCountryCodes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCountryCodes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Author        : jes
    ' Description   : Removes unwanted space in postcode on return from QAS names
    ' Edit History  : Created 24 October 2002
    ' ***************************************************************** '
    Private Function RemoveSpace(ByVal v_sString As String) As String

        Dim result As String = String.Empty
        Dim sString As String = ""



        sString = v_sString.Replace(" ", "")
        sString = sString.Substring(0, sString.Length - 3) & " " & sString.Substring(sString.Length - 3)


        Return sString

    End Function
    ' ***************************************************************** '
    ' Name: FindPAFAddress (Private)
    '
    ' Description:
    ' ***************************************************************** '
    Private Function FindPAFAddress(ByRef r_vAddressArray(,) As Object, ByRef r_vPickList(,) As Object, Optional ByRef r_sPostCode As String = "", Optional ByRef r_sAddressLine1 As String = "", Optional ByVal sCountryId As String = "") As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim v_sScript As String = ""
        Dim vValid As Boolean
        Dim lNodeID As Integer
        Dim v_vResultsArray(,) As Object = Nothing
        Dim sOptionValue As String = String.Empty
        Dim sResultCountryId As String = String.Empty

        'nResult = bPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionRuleTypeAddressLookup, r_sOptionValue:=sOptionValue)
        nResult = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, kSystemOptionRuleTypeAddressLookup, sOptionValue)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sOptionValue = "1" Then
            nResult = CType(GetScriptFile(v_sScript), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            RunScript(v_sScript, r_sPostCode, r_sAddressLine1, r_vAddressArray, vValid)
        Else
            nResult = CType(GetCompiledRuleFile(v_sScript), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            RunCompiledRules(sAssemblyClassName:=v_sScript, sPostcode:=r_sPostCode, sAddressLine1:=r_sAddressLine1, vAddressArray:=r_vAddressArray, bValidDetails:=vValid)
        End If

        If vValid Then
            If Informations.IsArray(r_vAddressArray) Then
                'get all countries in an array
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCountrySQL, sSQLName:=kGetCountrySQLName, bStoredProcedure:=False, vResultArray:=v_vResultsArray)


                For lCountCountry As Integer = v_vResultsArray.GetLowerBound(1) To v_vResultsArray.GetUpperBound(1)
                    If (CStr(v_vResultsArray(0, lCountCountry)).Trim().ToUpper() = sCountryId) Then
                        sResultCountryId = CStr(v_vResultsArray(0, lCountCountry)).Trim().ToUpper()
                        Exit For
                    End If
                Next lCountCountry

                For lCount As Integer = 0 To r_vAddressArray.GetUpperBound(1)
                    'get country id for respective country and set it in Address Array                  
                    For lCountCountry As Integer = v_vResultsArray.GetLowerBound(1) To v_vResultsArray.GetUpperBound(1)
                        If (CStr(v_vResultsArray(1, lCountCountry)).Trim().ToUpper() = CStr(r_vAddressArray(5, lCount)).Trim().ToUpper()) Then
                            r_vAddressArray(5, lCount) = v_vResultsArray(0, lCountCountry)
                            Exit For
                        End If
                    Next lCountCountry
                    ' Add this node only
                    If CStr(r_vAddressArray(5, lCount)).Trim().ToUpper() = sResultCountryId Then
                        m_lReturn = CType(AddNode(v_sNodeType:="PAFAddress", r_lNodeID:=lNodeID, v_vAddressArray:=r_vAddressArray, nCountryIndex:=lCount), gPMConstants.PMEReturnCode)
                    End If

                Next lCount
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        ElseIf Not vValid Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count())

        For lCount As Integer = 1 To m_oPickListNodes.Count()

            r_vPickList(0, lCount) = m_oPickListNodes.Item(lCount).NodeID

            r_vPickList(1, lCount) = m_oPickListNodes.Item(lCount).Level

            r_vPickList(2, lCount) = m_oPickListNodes.Item(lCount).Caption

            r_vPickList(3, lCount) = m_oPickListNodes.Item(lCount).ParentNodeID
        Next lCount

        Return nResult

    End Function
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetScriptFile (Private)
    ' PURPOSE:locate the appropriate script file
    ' ---------------------------------------------------------------------------
    Private Function GetScriptFile(ByRef v_sScript As String) As Integer

        Dim result As Integer = 0
        Dim sFullPath As String = ""
        Dim intFile As Integer
        Dim lFileLength As Integer
        Dim sPathName As String = ""
        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String



        result = gPMConstants.PMEReturnCode.PMTrue

        '
        'get the path to the validation script from the registry
        '
        lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

        'Build the path to the script file
        sFullPath = sPathName & "\" & "PAFWrapper.rul"

        'locate the file
        If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then Return result

        intFile = FileSystem.FreeFile()

        FileSystem.FileOpen(intFile, sFullPath, OpenMode.Input)
        lFileLength = FileSystem.LOF(intFile)

        'read the basic into the string variable
        sStr2 = FileSystem.InputString(intFile, lFileLength)

        FileSystem.FileClose(intFile)

        'build the full script
        sStr = ""

        sStr = sStr & "Option Explicit" & Strings.ChrW(13) & Strings.ChrW(10)

        sStr = sStr & sStr2 & Strings.ChrW(13) & Strings.ChrW(10)

        'return the script
        v_sScript = sStr.Trim()


        ' Do any tidy up, e.g. Set x = Nothing here

        Return result

    End Function

    ''' <summary>
    ''' Get compiled rule file based on system option
    ''' </summary>
    ''' <param name="v_sScript"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCompiledRuleFile(ByRef v_sScript As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        'nResult = bPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionCompiledRuleAddressLookup, r_sOptionValue:=v_sScript)
        nResult = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, GeneralConst.kSystemOptionCompiledRuleAddressLookup, v_sScript)

        Return nResult

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: RunScript (Private)
    ' PURPOSE:Runs the appropriate validation script
    ' ---------------------------------------------------------------------------
	<HandleProcessCorruptedStateExceptions>
    Private Sub RunScript(ByVal v_sScript As String, Optional ByVal v_sPostcode As String = "", Optional ByVal v_sAddressLine1 As String = "", Optional ByRef r_vAddressArray(,) As Object = Nothing, Optional ByRef v_bValidDetails As Boolean = False)

        Dim oSharedStorage As SharedStorage
        Dim oScriptControl As MSScriptControl.ScriptControl
        Dim sStr As String = ""

        Try

            'create script control object
            oScriptControl = New MSScriptControl.ScriptControl()

            oScriptControl.Language = "VBScript"

            'create shared storage object, used to hold values that are read/writable from the VB script file
            oSharedStorage = New SharedStorage()

            'check for presense of v_sPostCode and store in SharedStorage object

            If Not Informations.IsNothing(v_sPostcode) Then
                oSharedStorage.vPostCode = v_sPostcode.Trim()
            End If

            'check for presense of v_sAddressLine1 and store in SharedStorage object

            If Not Informations.IsNothing(v_sAddressLine1) Then
                oSharedStorage.vAddressLine1 = v_sAddressLine1.Trim()
            End If

            'Add reference to sharedstorage object on the scriptcontrol object
            oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)

            'read in the script and run it
            oScriptControl.AddCode(v_sScript.Trim())
            oScriptControl.Run("start")

            'return valid flag is applicable

            'If Not Informations.IsNothing(v_bValidDetails) Then
            v_bValidDetails = oSharedStorage.vValid
            'End If


            'If Not Informations.IsNothing(r_vAddressArray) Then
            r_vAddressArray = oSharedStorage.vAddressArray.Clone()
            'End If


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RunScript", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            End Select

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            oSharedStorage = Nothing
            oScriptControl = Nothing
        End Try

        Exit Sub

    End Sub

    ''' <summary>
    ''' Run compiled rules based on address lookup system option
    ''' </summary>
    ''' <param name="sAssemblyClassName"></param>
    ''' <param name="sPostcode"></param>
    ''' <param name="sAddressLine1"></param>
    ''' <param name="vAddressArray"></param>
    ''' <param name="bValidDetails"></param>
    ''' <remarks></remarks>
    Private Sub RunCompiledRules(ByVal sAssemblyClassName As String,
                                 ByVal sPostcode As String,
                                 ByVal sAddressLine1 As String,
                                 ByRef vAddressArray(,) As Object,
                                 ByRef bValidDetails As Boolean)

        Dim oSharedStorage As SharedStorage
        Dim oRules As Object

        'create shared storage object, used to hold values that are read/writable from the VB script file
        oSharedStorage = New SharedStorage()

        'check for presense of v_sPostCode and store in SharedStorage object
        If Not Informations.IsNothing(sPostcode) Then
            oSharedStorage.vPostCode = sPostcode.Trim()
        End If

        'check for presense of v_sAddressLine1 and store in SharedStorage object
        If Not Informations.IsNothing(sAddressLine1) Then
            oSharedStorage.vAddressLine1 = sAddressLine1.Trim()
        End If

        oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)
        If Not (oRules Is Nothing) Then
            oRules.oSharedStorage = oSharedStorage
            oRules.Start()

            bValidDetails = oRules.oSharedStorage.vValid
            ' vAddressArray = VB6.CopyArray(oRules.oSharedStorage.vAddressArray)
            ' This need to be tested
            vAddressArray = CType(oRules.oSharedStorage.vAddressArray, Object(,)).Clone()
        End If

        oSharedStorage = Nothing
        Exit Sub

    End Sub

    Private Function FindQASPro610Address(ByRef r_vAddressArray As Object, ByRef r_vPickList As Object, Optional ByRef r_sPostCode As Object = Nothing, Optional ByRef r_sAddressLine1 As Object = Nothing, Optional ByRef r_sAddressLine2 As Object = Nothing, Optional ByRef r_sAddressLine3 As Object = Nothing, Optional ByRef r_sAddressLine4 As Object = Nothing) As Long

        Dim sSearch As String = ""
        Dim lCount As Long
        Dim lngHandle As Integer
        Dim sMessage As String
        Dim sCurrentPath As String

        Const kINIFILENAME As String = "QAWSERVE.INI"



        FindQASPro610Address = gPMConstants.PMEReturnCode.PMTrue



        WarningMessage = ""

        'Build the Search String

        If Not String.IsNullOrEmpty(r_sAddressLine1) Then
            sSearch = sSearch & r_sAddressLine1 & ", "
        End If

        If Not String.IsNullOrEmpty(r_sAddressLine2) Then
            sSearch = sSearch & r_sAddressLine2 & ", "
        End If

        If Not String.IsNullOrEmpty(r_sAddressLine3) Then
            sSearch = sSearch & r_sAddressLine3 & ", "
        End If

        If Not String.IsNullOrEmpty(r_sAddressLine4) Then
            sSearch = sSearch & r_sAddressLine4 & ", "
        End If

        If Not String.IsNullOrEmpty(r_sPostCode) Then
            sSearch = sSearch & r_sPostCode & ", "
        End If

        sSearch = (sSearch.Trim)

        If (sSearch.Length) > 0 Then
            sSearch = Mid(sSearch, 1, (sSearch.Length) - 1)
        End If

        If sSearch = "" Then
            FindQASPro610Address = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If (m_lQASInitialised = gPMConstants.PMEReturnCode.PMFalse) Then

            m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_Path", r_sSettingValue:=m_sQASPath), gPMConstants.PMEReturnCode)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_sQASPath = ""
            ElseIf Dir(m_sQASPath) = "" Then
                m_sQASPath = ""
            End If

            m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_Format", r_sSettingValue:=m_sQASFormat), gPMConstants.PMEReturnCode)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_sQASFormat = ""
            End If

            sCurrentPath = Left(m_sQASPath, (m_sQASPath.Length) - ((kINIFILENAME.Length) + 1))
            System.Environment.SetEnvironmentVariable("PATH", Environ("PATH") & ";" & sCurrentPath)
            m_lQASInitialised = gPMConstants.PMEReturnCode.PMTrue
        End If


        'm_sQASPath, m_sQASFormat
        m_lReturn = CType(QA_Open(m_sQASPath, m_sQASFormat, lngHandle), gPMConstants.PMEReturnCode)
        'm_lReturn = QA_Open("", "", lngHandle)

        If (m_lReturn < QASSuccess) Then
            QA_Close(lngHandle)
            FindQASPro610Address = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = CType(QA_SetActiveLayout(vi1:=lngHandle, vs2:=m_sQASFormat), gPMConstants.PMEReturnCode)

        ' Call QASPro Search function
        m_lReturn = CType(QA_Search(lngHandle, MakeCString(sSearch)), gPMConstants.PMEReturnCode)

        If (m_lReturn < QASSuccess) Then

            Select Case m_lReturn
                Case qaerr_AREALEVEL
                    sMessage = PMQAS_WARN_AREALEVEL
                Case qaerr_DISTRICTLEVEL
                    sMessage = PMQAS_WARN_DISTRICTLEVEL
                Case qaerr_SECTORLEVEL
                    sMessage = PMQAS_WARN_SECTORLEVEL
                Case qaerr_HALFSECTORLEVEL
                    sMessage = PMQAS_WARN_HALFSECTORLEVEL
                Case qaerr_NUMBEREDFLAT
                    sMessage = PMQAS_WARN_NUMBEREDFLAT
                Case qaerr_POSTCODERECODED
                    sMessage = PMQAS_WARN_POSTCODERECODED
                Case qaerr_SUBSMADE
                    sMessage = PMQAS_WARN_SUBSMADE
                Case Else

                    sMessage = New String(" "c, 255) & Strings.ChrW(0).ToString()

                    m_lReturn = CType(QA_ErrorMessage(m_lReturn, sMessage, 255), gPMConstants.PMEReturnCode)

                    QA_Close(lngHandle)

                    Call QA_Shutdown()

                    FindQASPro610Address = gPMConstants.PMEReturnCode.PMFalse

                    FindQASPro610Address = gPMConstants.PMEReturnCode.PMError
                    Exit Function
            End Select

        End If

        ' Process Pick Lists
        m_lReturn = CType(Process610PickLists(v_sNodeType:="QASPro", v_lHandle:=lngHandle), gPMConstants.PMEReturnCode)

        ' Release resources used by QASPro Search
        Call QA_EndSearch(lngHandle)

        Call QA_Close(lngHandle)

        Call QA_Shutdown()

        ' Add objects to Pick list array
        ReDim r_vPickList(3, m_oPickListNodes.Count)

        For lCount& = 1 To m_oPickListNodes.Count
            r_vPickList(0, lCount&) = m_oPickListNodes.Item(lCount&).NodeID
            r_vPickList(1, lCount&) = m_oPickListNodes.Item(lCount&).Level
            r_vPickList(2, lCount&) = m_oPickListNodes.Item(lCount&).Caption
            r_vPickList(3, lCount&) = m_oPickListNodes.Item(lCount&).ParentNodeID
        Next lCount&



        Exit Function

    End Function


    Private Function Process610PickLists( _
    ByVal v_sNodeType As String, _
    Optional ByVal v_lParentNodeID As Integer = -1, _
    Optional ByVal v_lHandle As Integer = 1) As Long
        Dim lNodeID As Long
        Dim lItemCount As Long
        Dim lCount As Long
        Dim sDescription As String = ""
        Dim lConfidence As Long
        Dim lFlag As Long
        Dim lDescriptionLength As Long
        Dim lPicklistSize As Long
        Dim lPotential As Long
        Dim lSearchState As Long
        Dim blnCanStep As Boolean


        Process610PickLists = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(QA_GetSearchStatus(vi1:=v_lHandle, ri2:=lPicklistSize, ri3:=lPotential, rl4:=lSearchState), gPMConstants.PMEReturnCode)

        lItemCount = lPotential

        For lCount& = 0 To lItemCount& - 1

            m_lCurrentListItem = lCount

            m_lReturn = CType(QA_GetResult(vi1:=v_lHandle, vi2:=lCount, rs3:=sDescription, vi4:=lDescriptionLength, ri5:=lConfidence, rl6:=lFlag), gPMConstants.PMEReturnCode)

            If lFlag And qaproresult_CANSTEP Then
                blnCanStep = True
            Else
                blnCanStep = False
            End If


            If blnCanStep = True Then

                lNodeID = v_lParentNodeID

                m_lReturn = CType(QA_StepIn(vi1:=v_lHandle, vi2:=lCount), gPMConstants.PMEReturnCode)
                m_lReturn = CType(Process610PickLists(v_sNodeType:="QASPro", v_lParentNodeID:=lNodeID, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)

                m_lReturn = CType(QA_StepOut(vi1:=v_lHandle), gPMConstants.PMEReturnCode)

            End If

            If lFlag And qaproresult_FULLADDRESS Then

                m_lReturn = CType(AddNode(v_sNodeType:="QASPro", r_lNodeID:=lNodeID, v_lParentNodeID:=v_lParentNodeID, v_lHandle:=v_lHandle), gPMConstants.PMEReturnCode)

            End If

        Next lCount






        Exit Function

    End Function
End Class
