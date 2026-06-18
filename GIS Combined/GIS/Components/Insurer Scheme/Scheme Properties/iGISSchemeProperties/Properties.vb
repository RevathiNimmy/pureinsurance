Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Properties_NET.Properties")> _
Public NotInheritable Class Properties
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Properties
    '
    ' Date:  11/06/1999
    '
    ' Description: Contains a collection of param objects.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Properties"


    Private Const FLD_NO_PROFILE_PROPERTY_ID As Integer = 0
    Private Const FLD_NO_PROFILE_OBJECT_NAME As Integer = 1
    Private Const FLD_NO_PROFILE_PROPERTY_NAME As Integer = 2
    Private Const FLD_NO_PROFILE_REQUIRED As Integer = 3



    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    Private m_oPropertiesByID As iGISSchemeProperties.Propertys
    Private m_oPropertiesByName As iGISSchemeProperties.Propertys
    Private m_lGISSchemeId As Integer
    Private m_lReturn As Integer

    Dim m_oSchemeBusiness As bGISSchemeBusiness.Business

    Public ReadOnly Property GISSchemeId() As Integer
        Get
            Return m_lGISSchemeId
        End Get
    End Property


    ' PRIVATE Data Members (End)

    ' PUBLIC Properties (Begin)
    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise




        Dim result As Integer = 0

        Dim oObjectManager As bObjectManager.ObjectManager


        Try

            ' Initialisation Code.


            result = gPMConstants.PMEReturnCode.PMTrue

            oObjectManager = New bObjectManager.ObjectManager()
            If oObjectManager.Initialise(ACApp) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim temp_m_oSchemeBusiness As Object
            m_lReturn = oObjectManager.GetInstance(temp_m_oSchemeBusiness, "bGISSchemeBusiness.Business", gPMConstants.PMGetViaClientManager)
            m_oSchemeBusiness = temp_m_oSchemeBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oObjectManager.Dispose()

            oObjectManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oSchemeBusiness IsNot Nothing Then
                    m_oSchemeBusiness.Dispose()
                    m_oSchemeBusiness = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function StoreNavigationProfile(ByVal v_lPolicyLinkID As Integer, ByVal v_bPostQuoteProfile As Boolean, ByRef r_lSelectedSchemes() As Integer, Optional ByVal v_bUseSelectedScheme As Boolean = False, Optional ByRef v_bViewQuote As Boolean = False) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn, lSchemeId As Integer

        Dim vPropertiesArray As Object

        Try

            If v_bUseSelectedScheme Then


                lReturn = m_oSchemeBusiness.SelectSelectedScheme(v_lPolicyLinkID:=v_lPolicyLinkID, r_lSchemeID:=lSchemeId)

                ReDim r_lSelectedSchemes(0)
                r_lSelectedSchemes(0) = lSchemeId
            End If

            ' Store the scheme id for later
            m_lGISSchemeId = r_lSelectedSchemes(0)


            If m_oSchemeBusiness.GetNavigationProfile(v_lPolicyLinkID, v_bPostQuoteProfile, r_lSelectedSchemes, vPropertiesArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SJ220900 - Surpressed error message as it is not fatal
            ' BEGIN
            '    If Not IsArray(vPropertiesArray) Then
            '        StoreNavigationProfile = PMFalse
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="No properties returned", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="StoreNavigationProfile"
            '        Exit Function
            '    End If
            '
            '    If StoreProperties(vPropertiesArray) <> PMTrue Then
            '        StoreNavigationProfile = PMFalse
            '        Exit Function
            '    End If

            If Information.IsArray(vPropertiesArray) Then

                If StoreProperties(vPropertiesArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'SJ220900 - END



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreNavigationProfile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreNavigationProfile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function Required(ByRef r_iRequired As Integer, Optional ByVal v_lPropertyID As Integer = -1, Optional ByVal v_sObjectName As String = "", Optional ByVal v_sPropertyName As String = "") As Integer

        Dim sKey As String = ""
        Dim oProperty As Property_Renamed

        If v_lPropertyID <> -1 Then

            sKey = MakeIDKey(v_lPropertyID)
            oProperty = m_oPropertiesByID.Item(sKey)

        Else

            If v_sObjectName <> "" And v_sPropertyName <> "" Then

                sKey = MakeNameKey(v_sObjectName, v_sPropertyName)
                oProperty = m_oPropertiesByName.Item(sKey)

            Else

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="If Property ID is not supplied then both Object and Property names need to be supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="Required")


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

        End If

        If Not (oProperty Is Nothing) Then

            r_iRequired = oProperty.Required

        Else

            r_iRequired = 0

        End If

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    Private Function StoreProperties(ByVal v_vPropertiesArray(,) As Object) As Integer

        Dim result As Integer = 0



        If Not (m_oPropertiesByID Is Nothing) Then
            m_oPropertiesByID = Nothing
        End If

        m_oPropertiesByID = New iGISSchemeProperties.Propertys()

        If Not (m_oPropertiesByName Is Nothing) Then
            m_oPropertiesByName = Nothing
        End If

        m_oPropertiesByName = New iGISSchemeProperties.Propertys()

        For iCnt As Integer = v_vPropertiesArray.GetLowerBound(1) To v_vPropertiesArray.GetUpperBound(1)


            If m_oPropertiesByID.Add(CInt(v_vPropertiesArray(FLD_NO_PROFILE_REQUIRED, iCnt)), MakeIDKey(CInt(v_vPropertiesArray(FLD_NO_PROFILE_PROPERTY_ID, iCnt)))) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            If m_oPropertiesByName.Add(CInt(v_vPropertiesArray(FLD_NO_PROFILE_REQUIRED, iCnt)), MakeNameKey(CStr(v_vPropertiesArray(FLD_NO_PROFILE_OBJECT_NAME, iCnt)), CStr(v_vPropertiesArray(FLD_NO_PROFILE_PROPERTY_NAME, iCnt)))) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Next


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function MakeIDKey(ByVal v_lPropertyID As Integer) As String

        Return "P" & v_lPropertyID

    End Function

    Private Function MakeNameKey(ByVal v_sObjectName As String, ByVal v_sPropertyName As String) As String

        Return v_sObjectName.Trim() & "~" & v_sPropertyName.Trim()

    End Function


    ' PRIVATE Methods (End)







    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

