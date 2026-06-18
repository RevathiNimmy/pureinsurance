Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 28/04/1999
    '
    ' Description: Used by the interface to retrieve data from the
    '              party prospect tables
    '
    ' Edit History: 280499 - CTAF - Initial Version
    '
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "Business"

    ' Private references to the other classes
    Private m_oCampaign As Campaign
    Private m_oPolicy As Policy
    Private m_oProspect As Prospect

    ' Reference to the database
    Private m_oDatabase As dPMDAO.Database

    ' Close database instance or not?
    Private m_bCloseDatabase As Boolean

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ************* Public Properties (BEGIN) ************************* '

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ************* Public Properties (END) *************************** '


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Standard entry point for the program.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            ' dont do anything

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Entry point for the program.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
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


            ' Save the parameters

            ' New instance of server component services


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Prospect object
            m_oProspect = New Prospect()

            m_lReturn = CType(m_oProspect.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy object
            m_oPolicy = New Policy()
            m_lReturn = CType(m_oPolicy.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Campaign object
            m_oCampaign = New Campaign()
            m_lReturn = CType(m_oCampaign.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove instance of component services

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Exit point. Clear up time. Show's over...
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
                If m_oCampaign IsNot Nothing Then
                    m_oCampaign.Dispose()
                End If
                m_oCampaign = Nothing
                If m_oPolicy IsNot Nothing Then
                    m_oPolicy.Dispose()
                End If
                m_oPolicy = Nothing
                If m_oProspect IsNot Nothing Then
                    m_oProspect.Dispose()
                End If
                m_oProspect = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Prospect
            m_lReturn = CType(m_oProspect.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy

            If Not Information.IsNothing(vPolicyID) Then
                m_lReturn = CType(m_oPolicy.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Campaign

            If Not Information.IsNothing(vRecordNo) Then
                m_lReturn = CType(m_oCampaign.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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
    ' Name: GetNext
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ripple through and run GetNext on each class


            ' Prospect
            m_lReturn = CType(m_oProspect.GetNext(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy
            m_lReturn = CType(m_oPolicy.GetNext(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Campaign
            m_lReturn = CType(m_oCampaign.GetNext(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Prospect
            m_lReturn = CType(m_oProspect.DirectAdd(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy
            'No.  Policy is done via the add etc. button on the prospect form
            '    m_lReturn& = m_oPolicy.DirectAdd(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, _
            ''                                     vPolicyTypeID:=vPolicyTypeID, _
            ''                                     vRenewalDate:=vRenewalDate, _
            ''                                     vNoOfTimesQuoted:=vNoOfTimesQuoted)
            '    If (m_lReturn& <> PMTrue) Then
            '        DirectAdd = PMFalse
            '        Exit Function
            '    End If

            ' Campaign
            'No.  Campaigns are read only and are only added to the table when a campaign
            'is run and this party included.
            '    m_lReturn& = m_oCampaign.DirectAdd(vPartyCnt:=vPartyCnt, _
            ''                                       vRecordNo:=vRecordNo, _
            ''                                       vCampaignID:=vCampaignID)
            '    If (m_lReturn& <> PMTrue) Then
            '        DirectAdd = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Policy
            'No.  We do not delete piecemeal, we delete the lot.  Or do we?
            '    m_lReturn& = m_oPolicy.DirectDelete(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID)
            m_lReturn = CType(m_oPolicy.DirectDelete(vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Campaign
            'No.  We do not delete piecemeal, we delete the lot.  Or do we?
            '    m_lReturn& = m_oCampaign.DirectDelete(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo)
            m_lReturn = CType(m_oCampaign.DirectDelete(vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Prospect
            m_lReturn = CType(m_oProspect.DirectDelete(vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function EditAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Prospect
            m_lReturn = CType(m_oProspect.EditAdd(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy
            'No.  Policy is done via the add etc. button on the prospect form
            '    m_lReturn& = m_oPolicy.EditAdd(vPartyCnt:=vPartyCnt, _
            ''                                   vPolicyID:=vPolicyID, _
            ''                                   vPolicyTypeID:=vPolicyTypeID, _
            ''                                   vRenewalDate:=vRenewalDate, _
            ''                                   vNoOfTimesQuoted:=vNoOfTimesQuoted)
            '    If (m_lReturn& <> PMTrue) Then
            '        EditAdd = PMFalse
            '        Exit Function
            '    End If

            ' Campaign
            'No.  Campaigns are read only and are only added to the table when a campaign
            'is run and this party included.
            '    m_lReturn& = m_oCampaign.EditAdd(vPartyCnt:=vPartyCnt, _
            ''                                     vRecordNo:=vRecordNo, _
            ''                                     vCampaignID:=vCampaignID)
            '    If (m_lReturn& <> PMTrue) Then
            '        EditAdd = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function EditUpdate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Prospect
            m_lReturn = CType(m_oProspect.EditUpdate(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy
            'No.  Policy is done via the add etc. button on the prospect form
            '    m_lReturn& = m_oPolicy.EditUpdate(vPartyCnt:=vPartyCnt, _
            ''                                      vPolicyID:=vPolicyID, _
            ''                                      vPolicyTypeID:=vPolicyTypeID, _
            ''                                      vRenewalDate:=vRenewalDate, _
            ''                                      vNoOfTimesQuoted:=vNoOfTimesQuoted)
            '    If (m_lReturn& <> PMTrue) Then
            '        EditUpdate = PMFalse
            '        Exit Function
            '    End If

            ' Campaign
            'No.  Campaigns are read only and are only added to the table when a campaign
            'is run and this party included.
            '    m_lReturn& = m_oCampaign.EditUpdate(vPartyCnt:=vPartyCnt, _
            ''                                        vRecordNo:=vRecordNo, _
            ''                                        vCampaignID:=vCampaignID)
            '    If (m_lReturn& <> PMTrue) Then
            '        EditUpdate = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function EditDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Policy
            'No.  We do not delete piecemeal, we delete the lot.  Or do we?
            '    m_lReturn& = m_oPolicy.EditDelete(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID)
            m_lReturn = CType(m_oPolicy.EditDelete(vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Campaign
            'No.  We do not delete piecemeal, we delete the lot.  Or do we?
            '    m_lReturn& = m_oCampaign.EditDelete(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo)
            m_lReturn = CType(m_oCampaign.EditDelete(vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Prospect
            m_lReturn = CType(m_oProspect.EditDelete(vPartyCnt:=vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Prospect
            m_lReturn = CType(m_oProspect.Update(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy
            'No.  We don't update policies through here
            '    m_lReturn& = m_oPolicy.Update()
            '    If (m_lReturn& <> PMTrue) Then
            '        Update = PMFalse
            '        Exit Function
            '    End If

            ' Campaign
            'No.  We don't update campaigns through here
            '    m_lReturn& = m_oCampaign.Update()
            '    If (m_lReturn& <> PMTrue) Then
            '        Update = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProspectData
    '
    ' Description: Returns all info for a given party_cnt
    '
    '              Campaign info is returned in r_vCampaigns
    '              Policy info is returned in r_vPolicies
    '
    ' ***************************************************************** '
    Public Function GetProspectData(ByVal v_vPartyCnt As Object, ByRef r_vAgentReference As Object, ByRef r_vCurrentIntermediary As Object, ByRef r_vProspectStatusID As Object, ByRef r_vStrengthCodeID As Object, ByRef r_vPreviousInsurerCnt As Object, ByRef r_vPreviousBrokerCnt As Object, Optional ByRef r_vCampaigns As Array = Nothing, Optional ByRef r_vPolicies As Array = Nothing) As Integer

        Dim result As Integer = 0
        Dim vRecords, vPolicies As Object


        ' Campaign variables
        Dim lCampaignID As Integer
        Dim sDesc As String = ""
        Dim dtDate As Date

        ' Policy variables
        Dim vPolicyTypeID As String = ""
        Dim vRenewalDate As String = ""
        Dim vNoOfTimesQuoted As String = ""
        Dim vTargetPremium As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ************************************************************************
            ' ***********************   Prospect data   ******************************
            ' ************************************************************************

            ' Get the details for the main prospect
            m_lReturn = CType(m_oProspect.GetDetails(vPartyCnt:=v_vPartyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = CType(m_oProspect.GetNext(vPartyCnt:=v_vPartyCnt, vAgentReference:=r_vAgentReference, vCurrentIntermediary:=r_vCurrentIntermediary, vProspectStatusID:=r_vProspectStatusID, vStrengthCodeID:=r_vStrengthCodeID, vPreviousInsurerCnt:=r_vPreviousInsurerCnt, vPreviousBrokerCnt:=r_vPreviousBrokerCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ************************************************************************
            ' ***********************   Campaign data   ******************************
            ' ************************************************************************

            ' Get all the campaigns associated with this party
            m_lReturn = CType(m_oCampaign.GetRecordsForParty(v_vPartyCnt:=v_vPartyCnt, r_vRecords:=vRecords), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If (Information.IsArray(vRecords)) And (Not Information.IsNothing(r_vCampaigns)) Then

                ' Resize the campaign array

                r_vCampaigns = Array.CreateInstance(GetType(Object), New Integer() {4, vRecords.GetUpperBound(1) - vRecords.GetLowerBound(1) + 1}, New Integer() {0, vRecords.GetLowerBound(1)})


                For iLoop1 As Integer = vRecords.GetLowerBound(1) To vRecords.GetUpperBound(1)

                    m_lReturn = CType(m_oCampaign.GetCampaignDetails(v_vPartyCnt:=v_vPartyCnt, v_vRecordNo:=vRecords(0, iLoop1), v_vEffectiveDate:=DateTime.Now, r_vCampaignID:=lCampaignID, r_vDescription:=sDesc, r_vCampaignDate:=dtDate), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Fill in the array


                    r_vCampaigns(PMBCampaignRecordNo, iLoop1) = vRecords(0, iLoop1)

                    r_vCampaigns(PMBCampaignCampaignID, iLoop1) = lCampaignID

                    r_vCampaigns(PMBCampaignCampaignDate, iLoop1) = dtDate

                    r_vCampaigns(PMBCampaignDescription, iLoop1) = sDesc

                Next iLoop1

            End If

            ' ************************************************************************
            ' ************************   Policy data   *******************************
            ' ************************************************************************

            m_lReturn = CType(m_oPolicy.GetPoliciesForParty(v_vPartyCnt:=v_vPartyCnt, r_vPolicies:=vPolicies), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'TODO_MILAN
            'If (Information.IsArray(vPolicies)) And (Not Information.IsNothing(r_vPolicies)) Then
            If (Information.IsArray(vPolicies)) And (Not Information.IsNothing(vPolicies)) Then

                ' Resize to the amount of policies

                r_vPolicies = Array.CreateInstance(GetType(Object), New Integer() {6, vPolicies.GetUpperBound(1) - vPolicies.GetLowerBound(1) + 1}, New Integer() {0, vPolicies.GetLowerBound(1)})


                For iLoop1 As Integer = vPolicies.GetLowerBound(1) To vPolicies.GetUpperBound(1)

                    ' Get the details for this policy
                    m_lReturn = CType(m_oPolicy.GetPolicyDetails(v_vPartyCnt:=v_vPartyCnt, v_vPolicyID:=vPolicies(0, iLoop1), r_vPolicyTypeID:=vPolicyTypeID, r_vRenewalDate:=vRenewalDate, r_vNoOfTimesQuoted:=vNoOfTimesQuoted, r_vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Store them in the array


                    r_vPolicies(PMBPolicyPolicyID, iLoop1) = vPolicies(0, iLoop1)

                    r_vPolicies(PMBPolicyPolicyTypeID, iLoop1) = vPolicyTypeID

                    r_vPolicies(PMBPolicyRenewalDate, iLoop1) = vRenewalDate

                    r_vPolicies(PMBPolicyNoOfTimesQuoted, iLoop1) = vNoOfTimesQuoted
                    ' This isn't returned yet. Might not ever be

                    r_vPolicies(PMBPolicyTypeDescription, iLoop1) = "Need Lookup"

                    r_vPolicies(PMBPolicyTargetPremium, iLoop1) = vTargetPremium

                Next iLoop1

            End If

            ' All done!!

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProspectData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProspectData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeletePolicies
    '
    ' Description: Deletes the Policies.
    '
    ' ***************************************************************** '
    Public Function DeletePolicies(ByRef vPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Delete the Policies for the party
                m_lReturn = .SQLAction(sSQL:=ACDeletePoliciesSQL, sSQLName:=ACDeletePoliciesName, bStoredProcedure:=ACDeletePoliciesStored)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DeletePolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddPolicies
    '
    ' Description: Adds the policies.
    '
    ' ***************************************************************** '
    Public Function AddPolicies(ByRef vPartyCnt As Object, ByRef vPolicies As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vPolicies) Then
                Return result
            End If


            For i As Integer = vPolicies.GetLowerBound(1) To vPolicies.GetUpperBound(1)
                With m_oDatabase

                    .Parameters.Clear()

                    'Developer Guide No 101
                    'Start

                    'm_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=vPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    'm_lReturn = .Parameters.Add(sName:="prospect_policy_id", vValue:=CStr(i + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Parameters.Add(sName:="prospect_policy_id", vValue:=i + 1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    'm_lReturn = .Parameters.Add(sName:="risk_group_id", vValue:=CStr(vPolicies(0, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Parameters.Add(sName:="risk_group_id", vValue:=Convert.ToInt64(vPolicies(0, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    'm_lReturn = .Parameters.Add(sName:="renewal_date", vValue:=CStr(vPolicies(1, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                    m_lReturn = .Parameters.Add(sName:="renewal_date", vValue:=vPolicies(1, i), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


                    'm_lReturn = .Parameters.Add(sName:="no_of_times_quoted", vValue:=CStr(vPolicies(2, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Parameters.Add(sName:="no_of_times_quoted", vValue:=Convert.ToInt64(vPolicies(2, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    'm_lReturn = .Parameters.Add(sName:="target_premium", vValue:=CStr(vPolicies(3, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Parameters.Add(sName:="target_premium", vValue:=Convert.ToDecimal(vPolicies(3, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    'ends
                    'Add the Fees
                    m_lReturn = .SQLAction(sSQL:=ACInsertPoliciesSQL, sSQLName:=ACInsertPoliciesName, bStoredProcedure:=ACInsertPoliciesStored)

                End With

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AddPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
