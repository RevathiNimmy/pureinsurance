Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129 (guide)
<System.Runtime.InteropServices.ProgId("Results_NET.Results")>
Public NotInheritable Class Results
    'local variable to hold collection
    Private mCol As New ResultManagerKeyedCollection

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

    'SD 01/08/2002 scalability changes
    'Public Function Add(sKey As String, oRecord As adodb.Record) As Result
    Public Function Add(ByRef sKey As String, ByRef oRecord As dPMDAO.Records) As Result
        Dim vDatabase As Object = Nothing

        'create a new object
        Dim objNewMember As New Result
        Dim m_lReturn As gPMConstants.PMEReturnCode = CType(objNewMember.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

        ' Store the record object
        objNewMember.Key = sKey
        objNewMember.Record = oRecord

        'set the properties passed into the method
        mCol.Add(objNewMember)

        'return the object created
        Return objNewMember

    End Function

    Default Public ReadOnly Property Item(ByVal vntIndexKey As Object) As Result
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

    Public ReadOnly Property NewEnum() As Result
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
        mCol = New ResultManagerKeyedCollection()
    End Sub
    Public Sub New()
        MyBase.New()
    End Sub
    Protected Overrides Sub Finalize()
        'destroys collection when this class is terminated
        mCol = Nothing
    End Sub
End Class
