Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129 (guide)
Friend NotInheritable Class PMDAOInstances
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMDAOInstances
    '
    ' Date: 26/03/1998
    '
    ' Description: Maintains the PMDAOInstance Collection.
    '
    '
    ' Edit History:
    ' RFC 12/06/1998 - SiriusBrokingDSN Added.
    ' RFC 19/08/1998 - SiriusUnderwriting, Solutions and Nirvana DSNs added.
    ' RFC 26/08/1998 - Amended to use the new ComponentServices
    '                  method NewDatabase.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMDAOInstances"

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
    Private m_colPMDAOInstances As FieldManagerDpmDaoKeyedCollection
    ' ***************************************************************** '
    ' Name: GetPMDAOInstance
    '
    ' Description: Gets PMDAOInstance from the collection for the
    '              Product Family. If one does not exist it creates one.
    '
    '
    ' ***************************************************************** '
    Public Function GetPMDAOInstance(Optional ByVal v_eProductFamily As gPMConstants.PMEProductFamily = -1) As dPMDAO.Database

        Dim result As dPMDAO.Database = Nothing
        Dim sDSN As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oPMDAOInstance As bSIRFieldManager.PMDAOInstance

        Try

            oPMDAOInstance = Nothing

            ' Try and get an existing one from the collection
            oPMDAOInstance = Item(v_eProductFamily)

            ' Did we get one
            If Not (oPMDAOInstance Is Nothing) Then
                ' Yes, so return it
                Return oPMDAOInstance.Database
            Else

                ' No, so create a new one

                ' RFC 260898
                ' Amended to use the new ComponentServices method NewDatabase

                lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=v_eProductFamily, r_oDatabase:=result), gPMConstants.PMEReturnCode)


                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return Nothing
                End If

                ' Add the New PMDAO Instance to the collection
                lReturn = CType(Add(v_eProductFamily:=v_eProductFamily, v_oPMDAOInstance:=result), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return Nothing
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = Nothing

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPMDAOInstance from collection", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMDAOInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single PMDAOInstance into the
    '              PMDAOInstances Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_eProductFamily As gPMConstants.PMEProductFamily, ByVal v_oPMDAOInstance As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim oPMDAOInstance As bSIRFieldManager.PMDAOInstance
        Dim sPMDAOInstanceIndex As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a Local PMDAOInstance reference
            oPMDAOInstance = New bSIRFieldManager.PMDAOInstance()
            lReturn = oPMDAOInstance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            ' Set the PMDAOInstance Properties
            With oPMDAOInstance
                .Family = v_eProductFamily
                .Database = v_oPMDAOInstance
            End With

            ' Derive the PMDAOInstance Index
            sPMDAOInstanceIndex = GenerateKey(v_eProductFamily:=v_eProductFamily)

            ' Add the supplied PMDAOInstance into the collection
            m_colPMDAOInstances.Add(oPMDAOInstance)

            ' Release the local reference
            oPMDAOInstance = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add PMDAOInstance to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description: GenerateKeys a Key for the supplied details.
    '
    '
    ' ***************************************************************** '
    Public Function GenerateKey(ByVal v_eProductFamily As gPMConstants.PMEProductFamily) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Summary PMDAOInstance

            Return "K" & (v_eProductFamily).ToString().Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey for - " & v_eProductFamily, vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of PMDAOInstances in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_colPMDAOInstances.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a PMDAOInstance from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByVal v_eProductFamily As gPMConstants.PMEProductFamily)

        Dim sKey As String = ""

        Try

            sKey = GenerateKey(v_eProductFamily)

            ' Remove from the collection based on the Key
            m_colPMDAOInstances.Remove(sKey)

        Catch



            ' If there was nothing to delete just return

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected PMDAO Instance from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByVal v_eProductFamily As gPMConstants.PMEProductFamily) As bSIRFieldManager.PMDAOInstance

        Dim sKey As String = ""

        Try

            sKey = GenerateKey(v_eProductFamily)

            ' Return the Item based on the PMDAOInstance

            Return m_colPMDAOInstances(sKey)

        Catch



            ' If not found return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the PMDAOInstances Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set PMDAOInstances Collection to Nothing
            m_colPMDAOInstances = Nothing


            ' Added by Scalability Update Program - 30/07/2002
            m_colPMDAOInstances = New FieldManagerDpmDaoKeyedCollection()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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


            ' Initialisation Code.

            Return result

        Catch excep As System.Exception



            ' Error.

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
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        Try

            ' Added by Scalability Update Program - 30/07/2002
            m_colPMDAOInstances = New FieldManagerDpmDaoKeyedCollection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMDAOInstances = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMDAOInstances
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMDAOInstances
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
