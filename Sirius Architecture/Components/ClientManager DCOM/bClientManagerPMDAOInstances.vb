Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

Imports SSP.Shared
<Serializable()> _
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the PMDAO Instance Collection
    'Private m_colPMDAOInstances As Collection
    Private m_colPMDAOInstances As Dictionary(Of Object, Object)

    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

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
        Dim oPMDAOInstance As bClientManager.PMDAOInstance
        ' RDC 13062002 CompServ replaced by BAS module


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
                '       Set oComponentServices = New PMServerBusinessCS

                '        lReturn = oComponentServices.NewDatabase( _
                'v_lPMProductFamily:=v_eProductFamily, _
                'r_oDatabase:=GetPMDAOInstance)


                lReturn = CType(NewDatabase(g_sUsername, g_iSourceID, g_iLanguageID, v_lPMProductFamily:=v_eProductFamily, r_oDatabase:=result), gPMConstants.PMEReturnCode)
                '        Set oComponentServices = Nothing

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
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPMDAOInstance from collection", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMDAOInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function NewDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sDSN As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the DSN for this Product Family
            sDSN = GetDSN(v_lPMProductFamily:=v_lPMProductFamily)

            ' Create a New instance of PMDAO
            r_oDatabase = New dPMDAO.Database

            ' Open the database, using the DSN if we know what it should be

            ' RDC 27062002 new parameters required for OpenDatabase method

            If sDSN.Trim() = "" Then
                Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp)
            Else
                Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp, vDSN:=sDSN)
                '        NewDatabase = r_oDatabase.OpenDatabase(vDSN:=sDSN)
            End If

        Catch
        End Try



        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        r_oDatabase = Nothing

        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a new instance of PMDAO for DSN - " & sDSN, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDatabase", excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

        Return result

    End Function

    Private Function GetDSN(ByVal v_lPMProductFamily As Integer) As String

        Dim result As String = String.Empty


        result = ""

        ' Work out the correct DSN to open

        Select Case v_lPMProductFamily
            Case gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting, gPMConstants.PMEProductFamily.pmePFSiriusBroking, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMEProductFamily.pmePFDocumaster, gPMConstants.PMEProductFamily.pmePFGeminiII, gPMConstants.PMEProductFamily.pmePFClaims
                Return gPMConstants.PMSiriusSolutionsDSN
                '      Case pmePFSiriusArchitecture
                '        GetDSN = PMSiriusArchitectureDSN
                '      Case pmePFSiriusUnderwriting
                '        GetDSN = PMSiriusUnderwritingDSN
                '      Case pmePFSiriusBroking
                '        GetDSN = PMSiriusBrokingDSN
                '      Case pmePFSiriusSolutions
                '        GetDSN = PMSiriusSolutionsDSN
            Case gPMConstants.PMEProductFamily.pmePFGemini
                Return gPMConstants.PMGeminiDSN
                '      Case pmePFOrion
                '        GetDSN = PMOrionDSN
            Case gPMConstants.PMEProductFamily.pmePFVoyager
                Return gPMConstants.PMVoyagerDSN
            Case gPMConstants.PMEProductFamily.pmePFMercury
                Return gPMConstants.PMMercuryDSN
                '      Case pmePFDocumaster
                '        GetDSN = PMDocumasterDSN
            Case gPMConstants.PMEProductFamily.pmePFNirvana
                Return gPMConstants.PMNirvanaDSN
                'RFC060799 - Added GeminiII Product Family, DSN etc etc
                '      Case pmePFGeminiII
                '        GetDSN = PMGeminiIIDSN
                '      Case pmePFClaims
                '        GetDSN = PMClaimsDSN
                'RDC 13092002
            Case gPMConstants.PMEProductFamily.pmePFDocumasterScan
                Return gPMConstants.PMDocumasterScanDSN
                'JSB 09/09/03
            Case gPMConstants.PMEProductFamily.pmePFMediquote
                Return gPMConstants.PMMediquoteDSN
            Case gPMConstants.PMEProductFamily.pmePFSwift
                Return gPMConstants.PMSwiftDSN
            Case Else
                Return ""
        End Select




        ' Error Section.

        Return ""

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
        Dim oPMDAOInstance As BCLIENTMANAGER.PMDAOInstance
        Dim sPMDAOInstanceIndex As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a Local PMDAOInstance reference
            oPMDAOInstance = New BCLIENTMANAGER.PMDAOInstance()

            ' Set the PMDAOInstance Properties
            With oPMDAOInstance
                .Family = v_eProductFamily
                .Database = v_oPMDAOInstance
            End With

            ' Derive the PMDAOInstance Index
            sPMDAOInstanceIndex = GenerateKey(v_eProductFamily:=v_eProductFamily)

            ' Add the supplied PMDAOInstance into the collection
            m_colPMDAOInstances.Add(oPMDAOInstance, sPMDAOInstanceIndex)

            ' Release the local reference
            oPMDAOInstance = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add PMDAOInstance to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            Return "K" & v_eProductFamily.ToString.ToString

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey for - " & v_eProductFamily, vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Item(ByVal v_eProductFamily As gPMConstants.PMEProductFamily) As BCLIENTMANAGER.PMDAOInstance

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
            m_colPMDAOInstances = New Dictionary(Of Object, Object)
        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Class Initialise


            m_colPMDAOInstances = New Dictionary(Of Object, Object)
        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

