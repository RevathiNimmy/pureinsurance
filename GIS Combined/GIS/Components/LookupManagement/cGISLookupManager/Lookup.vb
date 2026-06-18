Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Lookup_NET.Lookup")>
Public NotInheritable Class Lookup

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 19/09/2003
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


    ' ***************************************************************** '
    ' Class Name: Lookup
    '
    ' Date: 18/04/2000
    '
    ' Description:
    '
    ' Edit History: Created by Ian Bagley 18/4/00
    '
    ' ***************************************************************** '
    Private Const ACClass As String = "Lookup"

    Private lFileHandleI As Integer
    Private lFileHandleH As Integer
    Private lFileHandleD As Integer

    Dim m_IndexRecord As MainModule.Lookup_Header_Index = MainModule.Lookup_Header_Index.CreateInstance()
    Dim m_HeaderRecord As MainModule.Lookup_Header = MainModule.Lookup_Header.CreateInstance()
    Dim m_DataRecord As MainModule.Lookup_Data = MainModule.Lookup_Data.CreateInstance()

    'exposed data properties
    Private InsurerID As String = ""
    Private BusinessType As String = ""
    Private TableName As String = ""
    Private EffectiveDateTime As String = ""
    Private ModifiedDateTime As String = ""
    Private Status As String = ""
    Private Definition As String = ""
    Private ValidConstants As String = ""
    Private DefaultValue As String = ""
    Private Data_Start_ptr As String = ""
    Private Data_End_ptr As String = ""
    Private Level As String = ""
    Private LevelValue As String = ""
    Private TypeOfLevel As String = ""
    Private Header_Start_ptr As String = ""
    Private Header_End_ptr As String = ""

    Dim lNumIndexRecords As Integer
    Dim m_lReturn As gPMConstants.PMEReturnCode

    Dim m_lRequiredStatus As Integer
    Dim m_nRequiredBusinessType As Integer
    Dim m_lRequiredInsurerNumber As Integer
    Dim m_sRequiredProcessDate As String = ""
    Dim m_oBusiness As bSIRRuleLookup.Business
    Dim m_sgisdatamodelcode As String


    Public Property RequiredInsurerNumber() As Integer
        Get
            Return m_lRequiredInsurerNumber
        End Get
        Set(ByVal Value As Integer)
            m_lRequiredInsurerNumber = Value
        End Set
    End Property
    ' *********************************************************************************
    ' OpenFiles
    ' Opens index, header, and data files for random access
    ' *********************************************************************************
    Public Function OpenFiles(ByRef sModelCode As String, ByRef sBusinessType As String, ByRef lProcessDate As Date, ByRef lStatus As Integer, Optional ByRef lOpenIfNotExist As Integer = 0) As Integer

        ' Removed as the files are not used anymore and instead the data is retrieved from the DB via LookupRuleFromDB
        m_sgisdatamodelcode = sModelCode
        m_nRequiredBusinessType = sBusinessType
        m_lRequiredStatus = lStatus
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    <Obsolete>
    Public Function CloseFiles() As Integer

        ' Removed as the files are not used anymore and instead the data is retrieved from the DB via LookupRuleFromDB

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Function Name: Lookup
    '
    ' Description:
    '
    ' Edit History: Created by Ian Bagley 18/4/00
    '
    ' Modify on   : 26-06-2000
    ' Modify By   : Ram Chandrabose
    ' Comments    : Modified the Data Types and Case Convertion Added
    '               Variables Modified - dPrevValue, dNextLevel, sTable
    ' ***************************************************************** '
    Public Function Lookup(ByRef sTable As String, Optional ByRef vInput As Object = Nothing) As Object
        ' HG14072003 - Pass through call to lookup via this new function (done this
        '              way to preserve compatibility).
        'Return LookupByRule(sTable:=sTable, vInput:=vInput)
        Return LookupByRuleFromDB(sTable:=sTable, vInput:=vInput)

    End Function

    ' ***************************************************************** '
    ' Function Name: LookupByDate
    '
    ' Description:
    '
    ' Edit History: Created by Harbinder Gill 14/7/03
    '
    ' ***************************************************************** '
    Public Function LookupByDate(ByVal v_sTable As String, ByVal v_dEffectiveDate As Date, Optional ByVal v_vInput As Object = Nothing) As Object
        ' HG14072003 - Pass through call to lookup by date via this new function (done this
        '              way to preserve compatibility).
        'Return LookupByRule(sTable:=v_sTable, v_vEffectiveDate:=v_dEffectiveDate, vInput:=v_vInput)
        Return LookupByRuleFromDB(sTable:=v_sTable, v_vEffectiveDate:=v_dEffectiveDate, vInput:=v_vInput)
    End Function

    '******************************************************************
    'This function recieves user input and then calculates the gradient
    '******************************************************************
    Public Function GradientCalc(ByRef vLowerLevel As Object, ByRef vUpperLevel As Object, ByRef vLowerValue As Object, ByRef vUpperValue As Object, ByRef vParameterValue As String, ByRef vAnswer As Object) As Integer

        Try

            'declaring the variables
            Dim lr As Double
            Dim vr As Double
            Dim po As Double
            Dim ps As Double
            Dim pr As Double
            Dim px As Double
            Dim py As Double

            'calculating the difference between the upper level and the lower level


            lr = CDbl(CDbl(vUpperLevel) - CDbl(vLowerLevel))
            'calculating the diffence between the upper value and the lower value


            vr = CDbl(CDbl(vUpperValue) - CDbl(vLowerValue))
            'calculating the difference between the parameter value and the lower level

            po = CDbl(vParameterValue - CDbl(vLowerLevel))
            'making ps equal to dividing 100 by the difference between the upper and lower levels
            ps = 100 / lr
            'making pr equal to the ps(answer to 100 divide by lr) * po(difference between upper and lower values)
            pr = ps * po
            'making px equal to pr(ps * po) dividing by 100
            px = pr / 100
            'making py equal to px * vr(difference between upper and lower value)
            py = px * vr
            'vanswer equal to the total and the lower value


            vAnswer = CDbl(vLowerValue) + py

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try


        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Function LookupByRuleFromDB(ByRef sTable As String, Optional ByRef v_vEffectiveDate As Object = Nothing, Optional ByRef vInput As Object = Nothing) As Object

        'Dim result(,) As Object = Nothing
        Dim result As Object = Nothing

        Dim vResultHeader As Object = Nothing
        Dim vResultData As Object = Nothing
        Dim vReturnValue As Object = Nothing
        Dim iTableType As Object

        Dim vPrevValue As Object
        Dim dPrevLevel As Double

        Dim iPrevStepType As Integer
        Dim dPrePrevValue As Double

        Dim dNextLevel As Double

        If Informations.IsNothing(m_oBusiness) Then
            Initialise()
        End If

        sTable = sTable.ToUpper()

        ' HG14072003 - Set Effective Date (If its passed in)

        If Informations.IsNothing(v_vEffectiveDate) Then
            m_sRequiredProcessDate = ToSafeDate(DateTime.Now())
        End If


        If Informations.IsDate(v_vEffectiveDate) Then
            m_sRequiredProcessDate = ToSafeDate(v_vEffectiveDate)
        End If


        'lets determine the require status options
        Dim lStatusRequired As Integer = m_lRequiredStatus
        Dim lAlternateStatusRequired As Integer = lStatusRequired

        'set to allow either live or test to be valid
        If m_lRequiredStatus = iGISSharedConstants.GISLookupTestOrLive Then
            lStatusRequired = iGISSharedConstants.GISLookupTest
            lAlternateStatusRequired = iGISSharedConstants.GISLookupLive
        End If

        m_lReturn = m_oBusiness.GetLookupHeader(m_sgisdatamodelcode, vResultHeader, sTable, m_lRequiredInsurerNumber, m_nRequiredBusinessType, m_sRequiredProcessDate)
        If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
            Informations.Err().Description = "Unable to assign header details"
            Throw New System.Exception("10001")
        End If

        If Not Informations.IsNothing(vResultHeader) Then
            If ((CInt(vResultHeader(6, 0)) = lStatusRequired) Or (CInt(vResultHeader(6, 0)) = lAlternateStatusRequired)) And ToSafeDate(vResultHeader(4, 0)) <= ToSafeDate(m_sRequiredProcessDate) Then

                vReturnValue = vResultHeader(9, 0)

                m_lReturn = m_oBusiness.GetLookupData(vResultHeader(2, 0), r_vResultArray:=vResultData)

                If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                    Informations.Err().Description = "Unable to assign data details"
                    Throw New System.Exception("10004")
                End If
            End If
        End If
        If Informations.IsNothing(vResultData) Then
            Return vReturnValue
        End If

        If CDbl(vResultData(iLDType, iFirstRow)) = iLDLineTypeConstant Then
            iTableType = iLDTableTypeConstant
        Else
            iTableType = iLDTableTypeStepGradient
        End If

        Select Case iTableType
            Case iLDTableTypeConstant

                vInput = vInput.Trim().ToUpper()

                For iLevel As Integer = vResultData.GetLowerBound(1) To vResultData.GetUpperBound(1)

                    If vInput = CStr(vResultData(iLDLevel, iLevel)).Trim().ToUpper() Then

                        vReturnValue = vResultData(iLDValue, iLevel)

                    End If
                Next iLevel
            Case iLDTableTypeStepGradient
                'need to perform some math...
                'must check to see if lookup value is numerical

                If Not Informations.IsNumeric(vInput) Then
                    Informations.Err().Description = "Input type required to be numeric for specified table type"
                    Throw New System.Exception("10005")
                    Return result
                End If
                'must first determine the pair of levels which straddle the input value

                For iLevel As Integer = vResultData.GetLowerBound(1) To vResultData.GetUpperBound(1) - 1

                    If gPMFunctions.ToSafeDouble(vInput) >= gPMFunctions.ToSafeDouble(vResultData(iLDLevel, iLevel)) And gPMFunctions.ToSafeDouble(vInput) < gPMFunctions.ToSafeDouble(vResultData(iLDLevel, iLevel + 1)) Then
                        'ok got pair. i.e. prev and next

                        dPrevLevel = CDbl(vResultData(iLDLevel, iLevel))

                        dNextLevel = CDbl(vResultData(iLDLevel, iLevel + 1))

                        iPrevStepType = CInt(vResultData(iLDType, iLevel))

                        'Ram - 26-06-2000 (Vaiable Name Changed)

                        vPrevValue = vResultData(iLDValue, iLevel)

                        'if prevtype is step then we can simply assign prevvalue
                        If iPrevStepType = iLDLineTypeStep Then

                            vReturnValue = vPrevValue
                        End If
                        'if prevtype gradient then calculate value
                        If iPrevStepType = iLDLineTypeGradient Then
                            'need to grab the value from the pre-prev level

                            dPrePrevValue = CDbl(vResultData(iLDValue, iLevel - 1))
                            'now work out the return value suppying the prev and next levels
                            'and the preprev and prev values

                            m_lReturn = CType(GradientCalc(dPrevLevel, dNextLevel, dPrePrevValue, vPrevValue, vInput, vReturnValue), gPMConstants.PMEReturnCode)

                            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                                Informations.Err().Description = "Unable to complete gradient calculation"
                                Throw New System.Exception("10006")
                            End If
                        End If
                        'stop looping coz weve got what we want

                        iLevel = vResultData.GetUpperBound(1) - 1
                    End If
                Next iLevel
        End Select

        result = vReturnValue

        vResultHeader = Nothing
        vResultData = Nothing
        vReturnValue = Nothing
        iTableType = Nothing
        vPrevValue = Nothing

        Return result

    End Function
    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If Process.GetCurrentProcess.SessionId <> 0 Then

            '    ' Create an instance of the object manager.
            '    g_oObjectManager = New bObjectManager.ObjectManager()

            '    ' Call the initialise method.
            '    m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)


            '    ' Check for errors.
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        ' Failed to call the initialise method.
            '        result = gPMConstants.PMEReturnCode.PMFalse

            '        ' Set the object manager to nothing.
            '        g_oObjectManager = Nothing

            '        ' Log Error.
            '        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=Nothing)

            '        Return result
            '    End If

            '    ' Store the language ID from the object manager
            '    ' to the public variables, to enable us to use
            '    ' them throughout the object.
            '    With g_oObjectManager
            '        g_iLanguageID = .LanguageID
            '        g_iSourceID = .SourceID
            '    End With


            '    m_lReturn = g_oObjectManager.GetInstance(m_oBusiness, "bSIRRuleLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            '    ' Check if we have an instance of the Object Manager.
            '    If Not (g_oObjectManager Is Nothing) Then
            '        ' Call the terminate method.
            '        g_oObjectManager.Dispose()
            '        ' Destroy the instance of the object manager
            '        ' from memory.
            '        g_oObjectManager = Nothing
            '    End If

            'Else
            m_oBusiness = New bSIRRuleLookup.Business
                m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            'End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function
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

End Class


