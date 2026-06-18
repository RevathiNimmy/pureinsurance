Option Strict Off
Option Explicit On
'developer guide no. 129 (guide)
Imports SSP.Shared

Friend NotInheritable Class Fields
    'local variable to hold collection
    Private mCol As New FieldManagerKeyedCollection

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

    'RWH(03/10/2000) RSAIB Process 28. Risk Loop - Added sMainGroup as param.
    Public Function Add(ByRef sKey As String, ByRef sSQLString As String, ByRef sColName As String, ByRef sColType As Integer, ByRef sMainGroup As String, ByRef sSubGroup As String, ByRef iProductFamily As Integer, ByRef vDataModel As Object, ByRef vPropertyId As Object, ByRef sLoop1 As String, ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef sLoop4 As String, Optional ByRef iSpecialType As Integer = 0, Optional ByRef r_sTableName As String = "") As Field
        Dim vDatabase As Object = Nothing

        'create a new object
        Dim objNewMember As New Field

        Dim m_lReturn As gPMConstants.PMEReturnCode = CType(objNewMember.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

        'set the properties passed into the method
        objNewMember.Key =  sKey.ToUpper
        objNewMember.SQLString = sSQLString
        objNewMember.ColumnName = sColName
        objNewMember.ColumnType = sColType
        objNewMember.MainGroup = sMainGroup 'RWH(03/10/2000) RSAIB Process 28. Risk Loop.
        objNewMember.ProductFamily = iProductFamily
        objNewMember.SubGroup = sSubGroup

        objNewMember.DataModel = vDataModel


        objNewMember.PropertyId = vPropertyId
        objNewMember.SpecialType = iSpecialType
        'RWH(16/01/2001) RSAIB Process 28. Extra level of risk looping.
        objNewMember.Loop1 = sLoop1
        objNewMember.Loop2 = sLoop2
        objNewMember.Loop3 = sLoop3
        objNewMember.Loop4 = sLoop4
        objNewMember.TableName = r_sTableName

        mCol.Add(objNewMember)

        'return the object created
        Return objNewMember


    End Function

    Default Public ReadOnly Property Item(ByVal vntIndexKey As Object) As Field
        Get
            'used when referencing an element in the collection
            'vntIndexKey contains either the Index or Key to the collection,
            'this is why it is declared as a Variant
            'Syntax: Set foo = x.Item(xyz) or Set foo = x.Item(5)
            Try


                Return mCol.Item(vntIndexKey)

            Catch


                Return Nothing
            End Try

        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            'used when retrieving the number of elements in the
            'collection. Syntax: Debug.Print x.Count
            Return mCol.Count
        End Get
    End Property
    Public ReadOnly Property NewEnum() As Field
        Get
            'this property allows you to enumerate
            'this collection with the For...Each syntax
            Return mCol.GetEnumerator
        End Get
    End Property
    Public Sub Remove(ByRef vntIndexKey As Object)
        'used when removing an element from the collection
        'vntIndexKey contains either the Index or Key, which is why
        'it is declared as a Variant
        'Syntax: x.Remove(xyz)
        mCol.Remove(vntIndexKey)
    End Sub
    Public Sub Clear()
        mCol = Nothing
        mCol = New FieldManagerKeyedCollection()
    End Sub
    Public Sub New()
        MyBase.New()
    End Sub
    Protected Overrides Sub Finalize()
        'destroys collection when this class is terminated
        mCol = Nothing
    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
