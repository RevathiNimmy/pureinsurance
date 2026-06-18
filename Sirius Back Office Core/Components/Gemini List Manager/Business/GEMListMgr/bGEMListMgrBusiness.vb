Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'Modified by Vijay Pal on 5/20/2010 10:24:44 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 11/02/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Listcustom.
    '
    ' Edit History  : 1
    ' Modify By     : Ram Chandrabose
    ' Modified Date : 25-09-1999
    ' Comments      : Changed the lNumberRecords from 0 to PMAllRecords
    '                 in the SQLSelect Statement
    '                 Since 0 limits to fetch only 500 Records )
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 12/01/2004
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Listcustoms (Private)
    Private m_oListcustoms As Object

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    'Allow edit of list text
    Private m_bAllowListEdit As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lListCustomID As Integer

    'Reference to the component manager
    Private m_oComponentManager As Object

    'List of items
    'Modified by Vijay Pal on 5/20/2010 12:24:26 PM refer developer guide no. 190(Latest guide)
    'Private m_vListArray As String = ""
    Private m_vListArray As Object
    Private m_lListItems As Integer

    'List Properties
    Private m_lObjectID As Integer
    Private m_lPropertyID As Integer
    Private m_lListID As Integer
    Private m_sListDescription As String = ""
    Private m_sObjectTable As String = ""
    Private m_iBusinessType As Integer

    Public cSql As String = ""




    ' ***************************************************************** '
    ' Name: GetPolData (Public)
    '
    ' Description: Get the Polaris Data
    '
    '
    ' ***************************************************************** '
    Public Function GetPolData(ByVal v_lObjectID As Integer, ByVal v_lPropertyID As Integer, ByRef r_vListArray(,) As Object, ByRef r_lNumItems As Object) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing
        Dim lPositionID, lRedimStart, lRedimSize As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the polaris data

            m_lReturn = m_oComponentManager.GetPolarisListDetails(lPropertyId:=v_lPropertyID, lMaxItems:=65535, vListArray:=r_vListArray, lNumItems:=r_lNumItems)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return 1
            End If

            'If we have no data then exit
            If Not Information.IsArray(r_vListArray) Then
                Return result
            End If

            'Get the updates from the custom table
            sSql = "SELECT " & _
                   "lc.position_id, " & _
                   " '' lcString," & _
                   "0 lcFlags, " & _
                   "lc.value_id, " & _
                   "lc.text, " & _
                   "lc.abi_code, " & _
                   "lc.Command " & _
                   " FROM List_custom lc " & _
                   " WHERE lc.property_id = '" & CStr(v_lPropertyID) & "'"

            ' Ram - 25-09-1999 ( Added the Optional Parameter lNumberRecords with value PMAllRecords )
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:=CStr(False), bStoredProcedure:=False, vResultArray:=vResultSet, lnumberrecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return 2
            End If

            'Check if we have data
            If Information.IsArray(vResultSet) Then

                'If we have changed custom data, then get changed values

                For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                    'Get the position id

                    lPositionID = CInt(Conversion.Val(CStr(vResultSet(0, iPtr))))

                    'Find the Position ID
                    For iPtr2 As Integer = r_vListArray.GetLowerBound(1) To r_vListArray.GetUpperBound(1)

                        'Check if we have the same position

                        If lPositionID = Conversion.Val(CStr(r_vListArray(GEMListMgrConst.LSTPosID, iPtr2))) Then

                            'If we have found item then insert changed data


                            r_vListArray(GEMListMgrConst.LSTString, iPtr2) = vResultSet(1, iPtr)


                            r_vListArray(GEMListMgrConst.LSTFlags, iPtr2) = vResultSet(2, iPtr)


                            r_vListArray(GEMListMgrConst.LSTValueID, iPtr2) = vResultSet(3, iPtr)


                            r_vListArray(GEMListMgrConst.LSTPosID, iPtr2) = vResultSet(0, iPtr)


                            r_vListArray(GEMListMgrConst.LSTText, iPtr2) = vResultSet(4, iPtr)


                            r_vListArray(GEMListMgrConst.LSTABICode, iPtr2) = vResultSet(5, iPtr)


                            r_vListArray(GEMListMgrConst.LSTCommand, iPtr2) = vResultSet(6, iPtr)

                            r_vListArray(GEMListMgrConst.LSTType, iPtr2) = GEMListMgrConst.LSTTypeCustom

                            Exit For
                        End If

                    Next iPtr2

                Next iPtr

            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Get user data

            vResultSet = Nothing

            'Add any user data to the list
            sSql = "SELECT lu.list_user_id, lu.text, lu.abi_code " & _
                   " FROM List_user lu, Lists ls " & _
                   " WHERE lu.list_id = ls.list_id " & _
                   " AND ls.property_id = '" & CStr(v_lPropertyID) & "'"

            'Get data
            ' Ram - 25-09-1999 ( Added the Optional Parameter lNumberRecords with value PMAllRecords )
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:=CStr(False), bStoredProcedure:=False, vResultArray:=vResultSet, lnumberrecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return 3
            End If

            'If no Data then Exit
            If Not Information.IsArray(vResultSet) Then
                Return result
            End If

            'Add new user data to list
            lRedimStart = r_vListArray.GetUpperBound(1) + 1

            lRedimSize = lRedimStart + vResultSet.GetUpperBound(1)
            ReDim Preserve r_vListArray(GEMListMgrConst.LSTMax, lRedimSize)

            'Add in the new records

            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                r_vListArray(GEMListMgrConst.LSTString, lRedimStart + iPtr) = 0

                r_vListArray(GEMListMgrConst.LSTFlags, lRedimStart + iPtr) = 0

                r_vListArray(GEMListMgrConst.LSTValueID, lRedimStart + iPtr) = 0


                r_vListArray(GEMListMgrConst.LSTPosID, lRedimStart + iPtr) = vResultSet(0, iPtr)


                r_vListArray(GEMListMgrConst.LSTText, lRedimStart + iPtr) = vResultSet(1, iPtr)


                r_vListArray(GEMListMgrConst.LSTABICode, lRedimStart + iPtr) = vResultSet(2, iPtr)

                r_vListArray(GEMListMgrConst.LSTCommand, lRedimStart + iPtr) = ""

                r_vListArray(GEMListMgrConst.LSTType, lRedimStart + iPtr) = GEMListMgrConst.LSTTypeUser

                r_vListArray(GEMListMgrConst.LSTChanged, lRedimStart + iPtr) = False

            Next iPtr

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public WriteOnly Property ListArray() As String
        Set(ByVal Value As String)

            m_vListArray = Nothing
            m_vListArray = Value

        End Set
    End Property

    Public ReadOnly Property AllowEdit() As Boolean
        Get
            Return m_bAllowListEdit
        End Get
    End Property


    Public WriteOnly Property ListDescription() As String
        Set(ByVal Value As String)

            m_sListDescription = Value

        End Set
    End Property

    Public WriteOnly Property ListItems() As Integer
        Set(ByVal Value As Integer)

            m_lListItems = Value

        End Set
    End Property

    Public WriteOnly Property ObjectID() As Integer
        Set(ByVal Value As Integer)

            m_lObjectID = Value

        End Set
    End Property

    Public WriteOnly Property ObjectTable() As String
        Set(ByVal Value As String)

            m_sObjectTable = Value

        End Set
    End Property

    Public WriteOnly Property PropertyID() As Integer
        Set(ByVal Value As Integer)

            m_lPropertyID = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFGemini

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)


            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oListcustoms.Count

                    m_lCurrentRecord = m_oListcustoms.Count
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection

            Return m_oListcustoms.Count

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property ListCustomID() As Integer
        Get

            Return m_lListCustomID

        End Get
        Set(ByVal Value As Integer)

            m_lListCustomID = Value

        End Set
    End Property




    Public ReadOnly Property BusinessType() As Integer
        Get

            Return m_iBusinessType

        End Get
    End Property


    Private Function RTrim(ByVal sStr As String) As String

        'Trims a string, removeing spaces and nulls

        Dim iPtr As Integer

        Try

            iPtr = (sStr.IndexOf(Strings.Chr(0).ToString()) + 1)

            If iPtr <> 0 Then
                sStr = sStr.Substring(0, iPtr - 1)
            End If


            Return sStr.Trim()

        Catch
            sStr = ""
            Return sStr
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Update Polaris Data
    '
    ' ***************************************************************** '
    Public Function Update() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try  '

            result = gPMConstants.PMEReturnCode.PMTrue

            'Make sure that we have object and property IDs
            If (m_lObjectID = 0) Or (m_lPropertyID = 0) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If we have no data then exit
            If Not Information.IsArray(m_vListArray) Then
                Return CType(False, gPMConstants.PMEReturnCode)
            End If '

            'Update the Pol Lists table
            m_lListID = 0
            If StringsHelper.ToDoubleSafe(m_vListArray(GEMListMgrConst.LSTType, 0)) = GEMListMgrConst.LSTTypeUser Then
                m_lReturn = UpdatePolLists()

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (m_lListID = 0) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            For lPtr As Integer = m_vListArray.GetLowerBound(0) To m_lListItems

                'Only update changed data
                If CBool(m_vListArray(GEMListMgrConst.LSTChanged, lPtr)) Then

                    'Check the type of list item
                    If StringsHelper.ToDoubleSafe(m_vListArray(GEMListMgrConst.LSTType, lPtr)) = GEMListMgrConst.LSTTypeCustom Then

                        'Update the Custom data
                        m_lReturn = UpdatePolCustom(v_lListItem:=lPtr)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    ElseIf (StringsHelper.ToDoubleSafe(m_vListArray(GEMListMgrConst.LSTType, lPtr)) = GEMListMgrConst.LSTTypeUser) Then

                        'Update the User data

                        m_lReturn = UpdatePolUser(v_lListItem:=lPtr)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If

                End If

            Next lPtr

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdatePolCustom (Public)
    '
    ' Description: UpdatePolCustom Polaris Data
    '
    ' ***************************************************************** '
    Public Function UpdatePolCustom(ByRef v_lListItem As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing
        Dim sTemp As String = ""

        Try  '

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check if this item
            sSql = "SELECT lc.list_custom_id, lc.position_id " & _
                   "FROM List_custom lc " & _
                   "WHERE lc.property_id = '" & CStr(m_lPropertyID) & "'" & _
                   " AND lc.position_id = " & CStr(Conversion.Val(m_vListArray(GEMListMgrConst.LSTPosID, v_lListItem)))

            ' Ram - 25-09-1999 ( Added the Optional Parameter lNumberRecords with value PMAllRecords )
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetPolCustom", bStoredProcedure:=False, vResultArray:=vResultSet, lnumberrecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DN 02/07/99 - Stop SQL actions failing by checking for apostrphes
            sTemp = RTrim(m_vListArray(GEMListMgrConst.LSTText, v_lListItem))
            m_lReturn = GEMApostrophes(sTemp)
            m_vListArray(GEMListMgrConst.LSTText, v_lListItem) = sTemp

            If Information.IsArray(vResultSet) Then

                'If this custom polaris item exists then update it
                If m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem) = GEMListMgrConst.LSTAmmended Or m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem) = GEMListMgrConst.LSTDeleted Then

                    sSql = "UPDATE List_custom " & _
                           "SET value_id = " & m_vListArray(GEMListMgrConst.LSTValueID, v_lListItem) & ", " & _
                           " text = '" & RTrim(m_vListArray(GEMListMgrConst.LSTText, v_lListItem)) & "', " & _
                           " abi_code = '" & RTrim(m_vListArray(GEMListMgrConst.LSTABICode, v_lListItem)) & "', " & _
                           " command = '" & RTrim(m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem)) & "', " & _
                           " property_id = '" & CStr(m_lPropertyID) & "' " & _
                           "WHERE list_custom_id = " & CStr(Conversion.Val(CStr(vResultSet(0, 0))))
                Else

                    sSql = "DELETE FROM List_custom " & _
                           "WHERE list_custom_id = " & CStr(Conversion.Val(CStr(vResultSet(0, 0))))
                End If
            Else

                'If it does not exist then Add it
                If m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem) = GEMListMgrConst.LSTAmmended Or m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem) = GEMListMgrConst.LSTDeleted Then

                    'Insert the new custom list item
                    sSql = "INSERT INTO List_custom(" & _
                           "position_id, value_id, text, " & _
                           "abi_code, command, property_id) " & _
                           "VALUES( " & _
                           m_vListArray(GEMListMgrConst.LSTPosID, v_lListItem) & ", " & _
                           m_vListArray(GEMListMgrConst.LSTValueID, v_lListItem) & ", " & _
                           "'" & RTrim(m_vListArray(GEMListMgrConst.LSTText, v_lListItem)) & "', " & _
                           "'" & RTrim(m_vListArray(GEMListMgrConst.LSTABICode, v_lListItem)) & "', " & _
                           "'" & RTrim(m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem)) & "', " & _
                           "'" & CStr(m_lPropertyID) & "')"
                End If

            End If

            cSql = sSql


            'Execute Action
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdatePolCustom", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolCustom Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolCustom", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolUser (Public)
    '
    ' Description: UpdatePolUser Polaris Data
    '
    ' ***************************************************************** '
    Public Function UpdatePolUser(ByRef v_lListItem As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing
        Dim sTemp As String = ""

        Try  '

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check if this item
            sSql = "SELECT lu.list_user_id " & _
                   "FROM List_user lu " & _
                   " WHERE lu.list_user_id = " & _
                   Conversion.Val(m_vListArray(GEMListMgrConst.LSTPosID, v_lListItem))

            ' Ram - 25-09-1999 ( Added the Optional Parameter lNumberRecords with value PMAllRecords )
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetPolUser", bStoredProcedure:=False, vResultArray:=vResultSet, lnumberrecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DN 02/07/99 - Stop SQL actions failing by checking for apostrphes
            sTemp = RTrim(m_vListArray(GEMListMgrConst.LSTText, v_lListItem))
            m_lReturn = GEMApostrophes(sTemp)
            m_vListArray(GEMListMgrConst.LSTText, v_lListItem) = sTemp

            If Information.IsArray(vResultSet) Then


                If m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem) = GEMListMgrConst.LSTDeleted Then

                    'If delete has been flagged for this item then delete


                    sSql = "DELETE FROM List_user " & _
                           "WHERE list_USER_id = " & CStr(Conversion.Val(CStr(vResultSet(0, 0))))

                Else

                    'If this user item exists then update it


                    sSql = "UPDATE List_user " & _
                           "SET text = '" & RTrim(m_vListArray(GEMListMgrConst.LSTText, v_lListItem)) & "', " & _
                           " abi_code = '" & RTrim(m_vListArray(GEMListMgrConst.LSTABICode, v_lListItem)) & "' " & _
                           "WHERE list_USER_id = " & CStr(Conversion.Val(CStr(vResultSet(0, 0))))

                End If

            Else

                'If deleted then exit
                If m_vListArray(GEMListMgrConst.LSTCommand, v_lListItem) = GEMListMgrConst.LSTDeleted Then
                    Return result
                End If

                'If it does not exist the Add it

                'Insert the new USER list item
                sSql = "INSERT INTO List_user(" & _
                       "list_id, text, abi_code) " & _
                       "VALUES( " & _
                       m_lListID & ", " & _
                       "'" & RTrim(m_vListArray(GEMListMgrConst.LSTText, v_lListItem)) & "', " & _
                       "'" & RTrim(m_vListArray(GEMListMgrConst.LSTABICode, v_lListItem)) & "')"

            End If

            'Execute Action
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdatePolUser", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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



        Dim result As Integer = 0

        Dim sSetting As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password


            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create Listcustoms Collection
            'Set m_oListcustoms = nothing

            'Create referecne to the component manager
            'Modified by Vijay Pal on 5/20/2010 1:09:39 PM todolist, bComponentManager is not found
            'm_oComponentManager = New bComponentManager.Business()


            'Initialise the component manager

            m_lReturn = CType(m_oComponentManager, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            '(IB)290799 - read AllowListEdit setting from registry
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lpmeregsettingroot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lpmeproductfamily:=gPMConstants.PMEProductFamily.pmePFGemini, v_lpmeregsettinglevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_ssettingname:="AllowListEdit", v_ssubkey:="Settings", r_ssettingvalue:=sSetting)
            m_bAllowListEdit = sSetting = "1"


            'MN160799 - Make sure that this isn't a HKJ environment
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lpmeregsettingroot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lpmeproductfamily:=gPMConstants.PMEProductFamily.pmePFGemini, v_lpmeregsettinglevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_ssettingname:="MarineVessel", v_ssubkey:="Settings", r_ssettingvalue:=sSetting)


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If sSetting = "1" Then
                    m_iBusinessType = GemBusinessTypeMV
                    'The HKJ option is in place

                    m_oComponentManager.BusinessType = m_iBusinessType

                End If

            End If


            m_lReturn = m_oComponentManager.openpolaris

            '   If m_lReturn <> PMTrue Then

            '       LogMessage m_sUsername, _
            'iType:=PMLogWarning, _
            'sMsg:="Failed to Open Polaris Run Time Engine", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="Initialise"

            '  End If





            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function OriginalDescription(ByVal lPropertyId As Integer, ByVal sABI As String, ByRef sOriginal As String) As Integer

        Dim result As Integer = 0
        Dim oPolCall As Object = Nothing
        Dim lPosId As Integer


        Dim lValueId As Integer = 0
        sOriginal = New String(" "c, 130)

        result = gPMConstants.PMEReturnCode.PMTrue


        Dim lReturn As Integer = m_oComponentManager.GetPolCall(oPolCall)

        lReturn = oPolCall.QueryListFromAbi(lPropertyId, sABI, lValueId, sOriginal, lPosId)
        sOriginal = sOriginal.Substring(0, Math.Min(sOriginal.Length, sOriginal.Trim().Length - 1))

        Return result
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

                m_oListcustoms = Nothing

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_oComponentManager IsNot Nothing Then
                    m_oComponentManager.Dispose()
                    m_oComponentManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Listcustom directly into the database.
    '        Note: The Listcustom will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vListCustomID As Object = Nothing, Optional ByRef vPositionID As Object = Nothing, Optional ByRef vValueID As Object = Nothing, Optional ByRef vText As Object = Nothing, Optional ByRef vAbiCode As Object = Nothing, Optional ByRef vCommand As Object = Nothing, Optional ByRef vPropertyID As Object = Nothing, Optional ByRef vListID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oListcustom As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Listcustom
            'Set oListcustom = nothing

            m_lReturn = oListcustom.Initialise(vDatabase:=m_oDatabase)

            ' Populate Listcustom Attributes

            m_lReturn = oListcustom.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vListCustomID:=vListCustomID, vPositionID:=vPositionID, vValueID:=vValueID, vText:=vText, vAbiCode:=vAbiCode, vCommand:=vCommand, vPropertyID:=vPropertyID, vListID:=vListID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oListcustom = Nothing
                Return result
            End If

            ' Add the Listcustom to the Database

            m_lReturn = oListcustom.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oListcustom = Nothing
                Return result
            End If

            ' Retain the Primary Key of the Listcustom Added
            With oListcustom

                ListCustomID = .ListCustomID
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oListcustom = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Listcustom directly from the database.
    '        Note: The Listcustom will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vListCustomID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oListcustom As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Listcustom
            oListcustom = Nothing

            m_lReturn = oListcustom.Initialise(vDatabase:=m_oDatabase)

            ' Set Listcustom Primary Key

            m_lReturn = oListcustom.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vListCustomID:=vListCustomID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oListcustom = Nothing
                Return result
            End If

            ' Delete the Listcustom from the Database

            m_lReturn = oListcustom.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oListcustom = Nothing
                Return result
            End If

            oListcustom = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Ram - 25-09-1999 ( Changed lNumberRecords value from 0 to PMAllRecords )
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lnumberrecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Listcustoms and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vListCustomID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oFields As ADODB.Fields
        Dim oListcustom As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection

            m_oListcustoms.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key

            Dim dbNumericTemp2 As Double

            If (Not Information.IsNothing(vListCustomID)) And (Not Double.TryParse(CStr(vListCustomID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vListCustomID=" & CStr(vListCustomID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Information.IsNothing(vListCustomID) Then

                ' Create New Listcustom
                oListcustom = Nothing

                m_lReturn = oListcustom.Initialise(vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oListcustom


                    .ListCustomID = vListCustomID


                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add Listcustom to collection

                m_lReturn = m_oListcustoms.Add(oNewListcustom:=oListcustom)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oListcustom = Nothing

            Else

                ' No Key, Get All Records

                ' Ram - 25-09-1999 ( Changed the lNumberRecords value from 0 to PMAllRecords )
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSql:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lnumberrecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oListcustom = Nothing

                    m_lReturn = CType(oListcustom, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                    ' Set oFields to refer to one Record
                    oFields = m_oDatabase.Records.Item(lSub).Fields()

                    ' Set component primary keys from current record
                    With oListcustom

                        .ListCustomID = oFields("list_custom_id")


                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add Listcustom to collection

                    m_lReturn = m_oListcustoms.Add(oNewListcustom:=oListcustom)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oListcustom = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Listcustoms and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vListCustomID As Object = Nothing, Optional ByRef vPositionID As Object = Nothing, Optional ByRef vValueID As Object = Nothing, Optional ByRef vText As Object = Nothing, Optional ByRef vAbiCode As Object = Nothing, Optional ByRef vCommand As Object = Nothing, Optional ByRef vPropertyID As Object = Nothing, Optional ByRef vListID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oListcustom As Object
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection

            If m_lCurrentRecord < m_oListcustoms.Count Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If


            oListcustom = m_oListcustoms.Item(m_lCurrentRecord)

            ' Get the Listcustom Property Values

            m_lReturn = oListcustom.GetProperties(iStatus, vListCustomID:=vListCustomID, vPositionID:=vPositionID, vValueID:=vValueID, vText:=vText, vAbiCode:=vAbiCode, vCommand:=vCommand, vPropertyID:=vPropertyID, vListID:=vListID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oListcustom = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Listcustom into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vListCustomID As Object = Nothing, Optional ByRef vPositionID As Object = Nothing, Optional ByRef vValueID As Object = Nothing, Optional ByRef vText As Object = Nothing, Optional ByRef vAbiCode As Object = Nothing, Optional ByRef vCommand As Object = Nothing, Optional ByRef vPropertyID As Object = Nothing, Optional ByRef vListID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oListcustom As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)

            If m_oListcustoms.Count <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Listcustom
            oListcustom = Nothing

            m_lReturn = oListcustom.Initialise(vDatabase:=m_oDatabase)

            ' Populate Listcustom Attributes

            m_lReturn = oListcustom.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vListCustomID:=vListCustomID, vPositionID:=vPositionID, vValueID:=vValueID, vText:=vText, vAbiCode:=vAbiCode, vCommand:=vCommand, vPropertyID:=vPropertyID, vListID:=vListID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oListcustom = Nothing
                Return result
            End If

            ' Add Listcustom to collection

            m_lReturn = m_oListcustoms.Add(oNewListcustom:=oListcustom)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oListcustom = Nothing
                Return result
            End If

            oListcustom = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Listcustom
    '              specified and updates the Listcustom with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vListCustomID As Object = Nothing, Optional ByRef vPositionID As Object = Nothing, Optional ByRef vValueID As Object = Nothing, Optional ByRef vText As Object = Nothing, Optional ByRef vAbiCode As Object = Nothing, Optional ByRef vCommand As Object = Nothing, Optional ByRef vPropertyID As Object = Nothing, Optional ByRef vListID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oListcustom As Object
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)

            If (lRow < 1) Or (lRow > m_oListcustoms.Count) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit

            oListcustom = m_oListcustoms.Item(lRow)

            ' Check the Status of the Listcustom

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit


            Select Case oListcustom.DatabaseStatus
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

            ' Update Listcustom Attributes

            m_lReturn = oListcustom.SetProperties(iStatus:=iStatus, vListCustomID:=vListCustomID, vPositionID:=vPositionID, vValueID:=vValueID, vText:=vText, vAbiCode:=vAbiCode, vCommand:=vCommand, vPropertyID:=vPropertyID, vListID:=vListID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oListcustom = Nothing
                Return result
            End If

            ' Release reference to Listcustom
            oListcustom = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Listcustom can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oListcustom As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)

            If (lRow < 1) Or (lRow > m_oListcustoms.Count) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete

            oListcustom = m_oListcustoms.Item(lRow)

            ' Check the Status of the Listcustom

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete

            If oListcustom.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then

                oListcustom.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else

                oListcustom.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Listcustom
            oListcustom = Nothing

            Return result

        Catch excep As System.Exception




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

            For lSub As Integer = 1 To m_oListcustoms.Count

                Select Case m_oListcustoms.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vListCustomID As Object = Nothing, Optional ByRef vPositionID As Object = Nothing, Optional ByRef vValueID As Object = Nothing, Optional ByRef vText As Object = Nothing, Optional ByRef vAbiCode As Object = Nothing, Optional ByRef vCommand As Object = Nothing, Optional ByRef vPropertyID As Object = Nothing, Optional ByRef vListID As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Information.IsNothing(vListID)) Or (Object.Equals(vListID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '
    ' {* USER DEFINED CODE (End) *}
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
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    ' ***************************************************************** '
    ' Name: UpdatePolLists (Private)
    '
    ' Description: Update Polaris Data
    '
    ' ***************************************************************** '
    Private Function UpdatePolLists() As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue


        'Make sure we have the current list
        sSql = "SELECT list_id FROM Lists " & _
                   "WHERE property_id = '" & CStr(m_lPropertyID) & "'"

        ' Ram - 25-09-1999 ( Added the Optional Parameter lNumberRecords with value PMAllRecords )
        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckList", bStoredProcedure:=False, vResultArray:=vResultSet, lnumberrecords:=gPMConstants.PMAllRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check if it is an array
        If Information.IsArray(vResultSet) Then

            'Get the list id

            m_lListID = CInt(vResultSet(0, 0))

        Else
            sSql = "INSERT INTO Lists(" & _
                   "property_id, description)" & _
                   "VALUES(" & _
                   "'" & CStr(m_lPropertyID) & "', " & _
                       "'" & m_sListDescription & "')"

            'Execute Action
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdateList", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the list id
            vResultSet = Nothing

            ' Ram - 25-09-1999 ( Added the Optional Parameter lNumberRecords with value PMAllRecords )
            'Get the new id
            m_lReturn = m_oDatabase.SQLSelect(sSql:="SELECT Max(list_id) FROM Lists", sSQLName:="MaxListID", bStoredProcedure:=False, vResultArray:=vResultSet, lnumberrecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultSet) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the list id

            m_lListID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UpdateServerRLDF (Private)
    '
    ' Description: Update Polaris Data
    '
    ' ***************************************************************** '
    Public Function UpdateServerRLDF() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oListUpdate As New bGEMListUpdate.Form
            If oListUpdate Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(oListUpdate, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            'MN160799 - Pass across the business type so that the correct files are generated:

            oListUpdate.BusinessType = m_iBusinessType


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oListUpdate.ListUpdateProcess()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oListUpdate.Dispose()

            oListUpdate = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateServerRLDF Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateServerRLDF", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

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

