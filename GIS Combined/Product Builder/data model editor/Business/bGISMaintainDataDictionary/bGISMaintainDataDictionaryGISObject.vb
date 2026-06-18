Option Strict Off
Option Explicit On
'refer Developer Guide No. 129
Imports System.Text
Imports SSP.Shared

Friend NotInheritable Class GISObject

    Implements IDisposable
    ' RAW 02/09/2003 : CQ2158 : added delete cascade where missing
    Private Const ACClass As String = "GISObject"

    Private m_lID As Integer = -1
    Private m_lGISObjectID As Integer = 0
    Private m_lGISDataModelID As Integer = 0
    Private m_sObjectName As String = ""
    Private m_sTableName As String = ""
    Private m_lMaxInstances As Integer = 1
    Private m_bIsQuoteObject As Boolean = False

    ' Developer Guide No. 101
    Private m_vParentObjectID As Object = DBNull.Value

    Private m_vPolarisObjectID As Object = DBNull.Value
    Private m_iIsSelectableForScreen As Integer = False
    Private m_lIsNonGIS As Integer = SSP.Shared.GISDataModelType.GISOTRisk
    Private m_lEditFlags As Integer = 0
    Private m_lGISDataModelType As Integer
    Private m_sParentTableName As String = ""
    Private m_sGISDataModel As String = ""
    Private m_bIsBrokingObject As Boolean = False

    Private m_colGISProperty As ArrayList

    Private m_oDatabase As dPMDAO.Database
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public Property ID() As Integer
        Get
            Return m_lID
        End Get
        Set(ByVal Value As Integer)
            m_lID = Value
        End Set
    End Property

    Public Property GISObjectID() As Integer
        Get
            Return m_lGISObjectID
        End Get
        Set(ByVal Value As Integer)
            m_lGISObjectID = Value
        End Set
    End Property

    Public Property GISDataModelID() As Integer
        Get
            Return m_lGISDataModelID
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelID = Value
        End Set
    End Property

    Public Property ObjectName() As String
        Get
            Return m_sObjectName
        End Get
        Set(ByVal Value As String)
            m_sObjectName = Value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return m_sTableName
        End Get
        Set(ByVal Value As String)
            m_sTableName = Value
        End Set
    End Property

    Public Property MaxInstances() As Integer
        Get
            Return m_lMaxInstances
        End Get
        Set(ByVal Value As Integer)
            m_lMaxInstances = Value
        End Set
    End Property

    Public Property IsQuoteObject() As Boolean
        Get
            Return m_bIsQuoteObject
        End Get
        Set(ByVal Value As Boolean)
            m_bIsQuoteObject = Value
        End Set
    End Property

    Public Property ParentObjectID() As Integer
        Get
            ' Developer Guide No. 287
            Return NullToInteger(m_vParentObjectID)
        End Get
        Set(ByVal Value As Integer)

            m_vParentObjectID = CInt(Value)
        End Set
    End Property

    Public Property PolarisObjectID() As Object
        Get
            Return m_vPolarisObjectID
        End Get
        Set(ByVal Value As Object)


            m_vPolarisObjectID = Value
        End Set
    End Property

    Public Property IsSelectableForScreen() As Integer
        Get
            Return m_iIsSelectableForScreen
        End Get
        Set(ByVal Value As Integer)
            m_iIsSelectableForScreen = Value
        End Set
    End Property

    Public Property IsNonGIS() As Integer
        Get
            Return m_lIsNonGIS
        End Get
        Set(ByVal Value As Integer)
            m_lIsNonGIS = Value
        End Set
    End Property

    Public Property EditFlags() As Integer
        Get
            Return m_lEditFlags
        End Get
        Set(ByVal Value As Integer)
            m_lEditFlags = Value
        End Set
    End Property
    'refer Developer Guide No. 33
    Public Property GISDataModelType() As Object
        Get
            Return m_lGISDataModelType
        End Get
        'refer Developer Guide No. 33
        Set(ByVal Value As Object)
            m_lGISDataModelType = Value
        End Set
    End Property

    Public Property ParentTableName() As String
        Get
            Return m_sParentTableName
        End Get
        Set(ByVal Value As String)
            m_sParentTableName = Value
        End Set
    End Property

    Public Property GISDataModel() As String
        Get
            Return m_sGISDataModel
        End Get
        Set(ByVal Value As String)
            m_sGISDataModel = Value
        End Set
    End Property

    Public Property IsBrokingObject() As Boolean
        Get
            Return m_bIsBrokingObject
        End Get
        Set(ByVal Value As Boolean)
            m_bIsBrokingObject = Value
        End Set
    End Property

    Public ReadOnly Property GISProperties() As ArrayList
        Get
            Return m_colGISProperty
        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)
            m_oDatabase = Value
        End Set
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : AddGISProperties
    '
    ' Description   : Function to Add GIS properties.
    '
    ' Edit History  :
    ' TR 03/12/2004 : Brought over from 1.9 code for Claims Builder
    ' RAM20/11/2002 : Added to ReAllow_NCD Property to default Claims builder Object
    '                   (Ref. NRMA Project Process No : 204 )
    ' ***************************************************************** '
    Public Function AddGISProperties() As Integer

        Dim result As Integer = 0
        Dim iPropertyCount As Integer
        Dim oGISProperty As New GISProperty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_colGISProperty = New ArrayList()


            Select Case m_lGISDataModelType
                Case ACPolicyBinder

                    addKeys(New Object() {"Policy_binder_id"}, iPropertyCount)
                    addProperty("gis_policy_link_id", GISSharedConstants.GISDataTypeNumeric, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)

                    '****************
                    ' MEvans : 23-07-2003 : Add Claim Properties for Output and Doc Request
                Case ACClaimsOutput

                    addKeys(New Object() {"Policy_binder_id", "Output_id"}, iPropertyCount)

                    addProperty("Decline_reason", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("Refer_reason", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("policy_rating_section", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("risk_rating_section", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("Sum_insured", GISSharedConstants.GISDataTypeCurrency, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("Premium", GISSharedConstants.GISDataTypeCurrency, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("Rate", GISSharedConstants.GISDataTypeCurrency, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("Message", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)

                    'Case GISDataModelType.GISDMTypeRisk
                Case GISDMTypeRisk

                    addKeys(New Object() {"Policy_binder_id", "Output_id"}, iPropertyCount)
                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Decline_reason"
                        .ColumnName = "Decline_reason"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Refer_reason"
                        .ColumnName = "Refer_reason"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "policy_rating_section"
                        .ColumnName = "policy_rating_section"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "risk_rating_section"
                        .ColumnName = "risk_rating_section"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Sum_insured"
                        .ColumnName = "Sum_insured"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Premium"
                        .ColumnName = "Premium"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Rate"
                        .ColumnName = "Rate"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Message"
                        .ColumnName = "Message"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    ''Tomo151002
                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "rate_type_id"
                        .ColumnName = "rate_type_id"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing
                    'Additional properties for Broking
                    If m_bIsBrokingObject Then
                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "scheme_desc"
                            .ColumnName = "scheme_desc"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "gis_insurer"
                            .ColumnName = "gis_insurer"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "policy_start_date"
                            .ColumnName = "policy_start_date"
                            .GISDataType = GISSharedConstants.GISDataTypeDate
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "premium_minus_ipt"
                            .ColumnName = "premium_minus_ipt"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "created_by"
                            .ColumnName = "created_by"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "status"
                            .ColumnName = "status"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "reason"
                            .ColumnName = "reason"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "override_percent"
                            .ColumnName = "override_percent"
                            .GISDataType = GISSharedConstants.GISDataTypePercentage
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "override_amount"
                            .ColumnName = "override_amount"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "override_gross_premium"
                            .ColumnName = "override_gross_premium"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "override_auth_code"
                            .ColumnName = "override_auth_code"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "quote_status"
                            .ColumnName = "quote_status"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "cover_type"
                            .ColumnName = "cover_type"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "scheme_id"
                            .ColumnName = "scheme_id"
                            .GISDataType = GISSharedConstants.GISDataTypeNumeric
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "premium_inc_ipt"
                            .ColumnName = "premium_inc_ipt"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "cover_start_time"
                            .ColumnName = "cover_start_time"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "ipt_value"
                            .ColumnName = "ipt_value"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "policy_number"
                            .ColumnName = "policy_number"
                            .GISDataType = GISSharedConstants.GISDataTypeText
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "date_of_first_instalment"
                            .ColumnName = "date_of_first_instalment"
                            .GISDataType = GISSharedConstants.GISDataTypeDate
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "deposit"
                            .ColumnName = "deposit"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "finance_charge"
                            .ColumnName = "finance_charge"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "first_instalment"
                            .ColumnName = "first_instalment"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "following_instalments"
                            .ColumnName = "following_instalments"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "no_of_instalments"
                            .ColumnName = "no_of_instalments"
                            .GISDataType = GISSharedConstants.GISDataTypeNumeric
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "to_print"
                            .ColumnName = "to_print"
                            .GISDataType = GISSharedConstants.GISDataTypeOption
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "total_to_pay"
                            .ColumnName = "total_to_pay"
                            .GISDataType = GISSharedConstants.GISDataTypeCurrency
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing
                    Else
                        ' State id
                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "state_id"
                            .ColumnName = "state_id"
                            .GISDataType = GISSharedConstants.GISDataTypeNumeric
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing

                        ' Country id
                        iPropertyCount += 1
                        oGISProperty = New GISProperty()
                        With oGISProperty
                            .ID = iPropertyCount
                            .Database = m_oDatabase
                            .GISObjectID = m_lGISObjectID
                            .PropertyName = "country_id"
                            .ColumnName = "country_id"
                            .GISDataType = GISSharedConstants.GISDataTypeNumeric
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                            End If
                            'Add to collection
                            m_colGISProperty.Add(oGISProperty)
                        End With
                        oGISProperty = Nothing
                    End If

                    'CLG added 12/05/2004
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "lapsed_reason_code"
                        .ColumnName = "lapsed_reason_code"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Insurer_cnt"
                        .ColumnName = "Insurer_cnt"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_value"
                        .ColumnName = "Commission_value"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Agent_cnt"
                        .ColumnName = "Agent_cnt"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Agent_Commission_value"
                        .ColumnName = "Agent_Commission_value"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Ipt_override"
                        .ColumnName = "Ipt_override"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Cover_start_date"
                        .ColumnName = "Cover_start_date"
                        .GISDataType = GISSharedConstants.GISDataTypeDate
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Cover_End_date"
                        .ColumnName = "Cover_End_date"
                        .GISDataType = GISSharedConstants.GISDataTypeDate
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Effective_Date"
                        .ColumnName = "Effective_Date"
                        .GISDataType = GISSharedConstants.GISDataTypeDate
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Policy_Ref"
                        .ColumnName = "Policy_Ref"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing
                    iPropertyCount = iPropertyCount + 1
                    oGISProperty = New GISProperty
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Disable_Original_ProRata"
                        .ColumnName = "Disable_Original_ProRata"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            AddGISProperties = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount = iPropertyCount + 1
                    oGISProperty = New GISProperty
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Disable_New_ProRata"
                        .ColumnName = "Disable_New_ProRata"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            AddGISProperties = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    'Add this property regarding External task relation for E5
                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "UID"
                        .ColumnName = "UID"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing


                    'Case GISDataModelType.GISDMTypeClaim
                Case GISDMTypeClaim

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = m_sGISDataModel & "_Policy_binder_id"
                        .ColumnName = m_sGISDataModel & "_Policy_binder_id"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        .IsIdentifyingProperty = True
                        .IsPrimaryKey = True
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = m_sGISDataModel & "_Peril_id"
                        .ColumnName = m_sGISDataModel & "_Peril_id"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        .IsIdentifyingProperty = True
                        .IsPrimaryKey = True
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Reserve_details"
                        .ColumnName = "Reserve_details"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        .IsInputProperty = True
                        .EditFlags = GISSharedPropertyConstants.GISDSEditReadOnly
                        .SpecialsType = GISSharedPropertyConstants.ACOReserveID
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Payment_details"
                        .ColumnName = "Payment_details"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        .IsInputProperty = True
                        .EditFlags = GISSharedPropertyConstants.GISDSEditReadOnly
                        .SpecialsType = GISSharedPropertyConstants.ACOPaymentID
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                Case ACOutputDetails

                    addKeys(New Object() {"Policy_binder_id", "Output_id", "Output_Details_id"}, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "description"
                        .ColumnName = "description"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "running_premium"
                        .ColumnName = "running_premium"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "vehicle_ref"
                        .ColumnName = "vehicle_ref"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "base_premium"
                        .ColumnName = "base_premium"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "rate_type"
                        .ColumnName = "rate_type"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "rate_amt"
                        .ColumnName = "rate_amt"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "rate_pct"
                        .ColumnName = "rate_pct"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "sequence_no"
                        .ColumnName = "sequence_no"
                        .GISDataType = GISSharedConstants.GISDataTypeNumeric
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing


                Case ACOutputEndorsements

                    addKeys(New Object() {"Policy_binder_id", "Output_id", "Output_Endorsements_id"}, iPropertyCount)

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "description"
                        .ColumnName = "description"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                Case ACOutputExcess

                    addKeys(New Object() {"Policy_binder_id", "Output_id", "Output_Excess_id"}, iPropertyCount)

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "excess_type"
                        .ColumnName = "excess_type"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "amount"
                        .ColumnName = "amount"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                Case ACOutputFees

                    addKeys(New Object() {"Policy_binder_id", "Output_id", "Output_Fees_id"}, iPropertyCount)

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "short_name"
                        .ColumnName = "short_name"
                        .GISDataType = GISSharedConstants.GISDataTypeText
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "fee_amount"
                        .ColumnName = "fee_amount"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "fee_percentage"
                        .ColumnName = "fee_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "fee_discount"
                        .ColumnName = "fee_discount"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_amount"
                        .ColumnName = "commission_amount"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_percentage"
                        .ColumnName = "commission_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    'DC031104 PN16283 add extra property
                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "IPT_applied"
                        .ColumnName = "IPT_applied"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    'MKW 250606 Datasure Section Changes START
                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Tax"
                        .ColumnName = "Tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Tax_Group_Id"
                        .ColumnName = "Tax_Group_Id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        .SpecialsType = GISSharedPropertyConstants.ACOPMLookupTableName

                        'refer Developer Guide No. 24
                        .SpecialsTypeReference = "Tax_Group"

                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing
                    'MKW 250606 Datasure Section Changes End

                Case ACPolicy

                    addKeys(New Object() {"Policy_binder_id", "policy_id"}, iPropertyCount)
                    addProperty("NCB_Years", GISSharedConstants.GISDataTypeNumeric, True, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)

                Case ACPolicyBinder

                    addKeys(New Object() {"Policy_binder_id"}, iPropertyCount)
                    'addProperty("UID", GISSharedConstants.GISDataTypeUniqueidentifier, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)
                Case ACParty

                    addKeys(New Object() {"Policy_binder_id", "party_id"}, iPropertyCount)
                    addProperty("code", GISSharedConstants.GISDataTypeText, True, GISSharedPropertyConstants.GISDSEditReadOnly, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)

                Case ACOutputSections

                    addKeys(New Object() {"Policy_binder_id", "Output_id", "Output_Sections_id"}, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Section_COB_Rating_Section_Id"
                        .ColumnName = "Section_COB_Rating_Section_Id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Section_Premium_Excluding_Tax"
                        .ColumnName = "Section_Premium_Excluding_Tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Section_Tax_Applied"
                        .ColumnName = "Section_Tax_Applied"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Section_Premium_Including_Tax"
                        .ColumnName = "Section_Premium_Including_Tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Section_Tax_Group_Id"
                        .ColumnName = "Section_Tax_Group_Id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        .SpecialsType = GISSharedPropertyConstants.ACOPMLookupTableName

                        'refer Developer Guide No. 24
                        .SpecialsTypeReference = "Tax_Group"

                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Section_Id"
                        .ColumnName = "Section_Id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_Cnt"
                        .ColumnName = "Commission_Cnt"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_Percentage"
                        .ColumnName = "Commission_Percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_Charge"
                        .ColumnName = "Commission_Charge"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_Net"
                        .ColumnName = "Commission_Net"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_tax_Applied"
                        .ColumnName = "Commission_tax_Applied"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_tax_Group_Id"
                        .ColumnName = "Commission_tax_Group_Id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        .SpecialsType = GISSharedPropertyConstants.ACOPMLookupTableName

                        'refer Developer Guide No. 24

                        .SpecialsTypeReference = "Tax_Group"

                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Commission_Payable"
                        .ColumnName = "Commission_Payable"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Is_Minimum_Brokerage"
                        .ColumnName = "Is_Minimum_Brokerage"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "Override_Rate_Table"
                        .ColumnName = "Override_Rate_Table"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "tax_percentage"
                        .ColumnName = "tax_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "tax_amount"
                        .ColumnName = "tax_amount"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_tax"
                        .ColumnName = "commission_tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_tax_percentage"
                        .ColumnName = "commission_tax_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                Case ACOutputSectionsCoinsurers

                    addKeys(New Object() {"Policy_binder_id", "Output_id", "Output_Sections_id", "Output_Sections_Coinsurers_id"}, iPropertyCount)

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "party_cnt"
                        .ColumnName = "party_cnt"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "section_premium_excluding_tax"
                        .ColumnName = "section_premium_excluding_tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "section_tax_applied"
                        .ColumnName = "section_tax_applied"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "section_premium_including_tax"
                        .ColumnName = "section_premium_including_tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "section_tax_group_id"
                        .ColumnName = "section_tax_group_id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        .SpecialsType = GISSharedPropertyConstants.ACOPMLookupTableName

                        'refer Developer Guide No. 24

                        .SpecialsTypeReference = "Tax_Group"

                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_cnt"
                        .ColumnName = "commission_cnt"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_percentage"
                        .ColumnName = "commission_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_charge"
                        .ColumnName = "commission_charge"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_net"
                        .ColumnName = "commission_net"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_tax_applied"
                        .ColumnName = "commission_tax_applied"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_tax_group_id"
                        .ColumnName = "commission_tax_group_id"
                        .GISDataType = GISSharedConstants.GISDataTypeInteger
                        .SpecialsType = GISSharedPropertyConstants.ACOPMLookupTableName

                        'refer Developer Guide No. 24

                        .SpecialsTypeReference = "Tax_Group"
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_payable"
                        .ColumnName = "commission_payable"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "is_minimum_brokerage"
                        .ColumnName = "is_minimum_brokerage"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "override_rate_table"
                        .ColumnName = "override_rate_table"
                        .GISDataType = GISSharedConstants.GISDataTypeOption
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "tax_percentage"
                        .ColumnName = "tax_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "tax_amount"
                        .ColumnName = "tax_amount"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_tax"
                        .ColumnName = "commission_tax"
                        .GISDataType = GISSharedConstants.GISDataTypeCurrency
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing

                    iPropertyCount += 1
                    oGISProperty = New GISProperty()
                    With oGISProperty
                        .ID = iPropertyCount
                        .Database = m_oDatabase
                        .GISObjectID = m_lGISObjectID
                        .PropertyName = "commission_tax_percentage"
                        .ColumnName = "commission_tax_percentage"
                        .GISDataType = GISSharedConstants.GISDataTypePercentage
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
                        End If
                        'Add to collection
                        m_colGISProperty.Add(oGISProperty)
                    End With
                    oGISProperty = Nothing
                Case ACCaseGeneral

                    addKeys(New Object() {"Policy_binder_id", "General_ID"}, iPropertyCount)
                    addProperty("Case_Header", GISSharedConstants.GISDataTypeNumeric, True, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount, GISSharedPropertyConstants.ACOCaseHeader)
                    addProperty("Case_Claim_Links", GISSharedConstants.GISDataTypeNumeric, True, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount, GISSharedPropertyConstants.ACOCaseClaimList)
                Case ACRiskS4IDefault

                    addKeys(New Object() {"Policy_binder_id", "S4IDefault_id"}, iPropertyCount)
                    addProperty("Retroactive_Date", GISSharedConstants.GISDataTypeDate, True, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)
                    addProperty("UID", GISSharedConstants.GISDataTypeText, False, GISSharedPropertyConstants.GISDSEditNone, iPropertyCount)
                Case ACOutputPremiumBreakdown

                    addKeys(New Object() {"Policy_binder_id", "Output_PremiumBreakdown_id"}, iPropertyCount)


                    'Generic : AddGISPropertiesColumns
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "unique_id", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "header_group", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "level1", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "level2", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "level3", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "level4", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_header", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "orderby", GISSharedConstants.GISDataTypeInteger)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "description", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "sum_insured", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "percent_original", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "percent_base", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "percent_base_previous", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "percent_applicable", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "percent_override", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "percent_override_previous", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "base_applicable_multiplier", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_original", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_applicable", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_override", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_original", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_locked", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_overriden", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "risk_rating_section", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "rate_type_id", GISSharedConstants.GISDataTypeInteger)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "state_id", GISSharedConstants.GISDataTypeInteger)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "country_id", GISSharedConstants.GISDataTypeInteger)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "net_ap_rp", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "gross_ap_rp", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "reason_for_override", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_base", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "rating_string", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "use_si_for_rating", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "include_si_to_header", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_multiplier", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_override_calculated", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "rounding_amount", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "total", GISSharedConstants.GISDataTypePercentage, True, 15, 3)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "UID", GISSharedConstants.GISDataTypeText)

                Case ACOutputCommission

                    addKeys(New Object() {"Policy_binder_id", "Output_Commission_id"}, iPropertyCount)

                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "unique_id", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "Output_id", GISSharedConstants.GISDataTypeInteger)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "agent_party_code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "peril_code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "commission_original", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "commission_percent", GISSharedConstants.GISDataTypePercentage)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "commission_band_code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "maximum_commission", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "maximum_commission_percent", GISSharedConstants.GISDataTypePercentage)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "currency_code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "tax_group_code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_locked", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_overriden", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "reason_for_override", GISSharedConstants.GISDataTypeText)

                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "COB_code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "COB_description", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "agent_party_description", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "gross_annual_commission", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "net_annual_commission", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "net_annual_commission_overriden", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "net_ap_rp", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "net_ap_rp_overriden", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "gross_ap_rp", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "commission_percent_overriden", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_lead", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "is_retained", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "premium_ap_rp", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "commission_percent_overriden_previous", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "output_to_s4i", GISSharedConstants.GISDataTypeCurrency)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "UID", GISSharedConstants.GISDataTypeText)


                Case ACOutputReferrals

                    addKeys(New Object() {"Policy_binder_id", "Output_Referrals_id"}, iPropertyCount)

                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "unique_id", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "referral_date", GISSharedConstants.GISDataTypeDate)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "reason", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "code", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "oderby", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "can_override", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "retained", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "user_group", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "generated_by", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "task_created", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "back_task_created", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "UID", GISSharedConstants.GISDataTypeText)

                Case ACOutputReferralsAudit

                    addKeys(New Object() {"Policy_binder_id", "Output_Referrals_id", "Output_Referrals_Audit_id"}, iPropertyCount)

                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "unique_id", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "type", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "pmuser_id", GISSharedConstants.GISDataTypeInteger)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "username", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "notes", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "reference", GISSharedConstants.GISDataTypeText)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "approved", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "declined", GISSharedConstants.GISDataTypeOption)
                    AddGISPropertiesColumns(oGISProperty, m_colGISProperty, iPropertyCount, "UID", GISSharedConstants.GISDataTypeText)

            End Select

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGISProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function AddToDatabase() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase.Parameters

                .Clear()

                'Add GISObjectID as OUTPUT
                m_lReturn = .Add(sName:="gis_object_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter gis_object_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                'Now the INPUT parameters
                m_lReturn = .Add(sName:="gis_data_model_id", vValue:=CStr(m_lGISDataModelID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter gis_data_model_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="object_name", vValue:=m_sObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter object_name", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="table_name", vValue:=m_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter table_name", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="max_instances", vValue:=CStr(m_lMaxInstances), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter max_instances", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="is_quote_object", vValue:=CStr(If(Not m_bIsQuoteObject, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_quote_object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                ' Developer Guide No.85 
                m_lReturn = .Add(sName:="parent_object_id", vValue:=m_vParentObjectID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter parent_object_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                ' Developer Guide No.85 

                m_lReturn = .Add(sName:="polaris_object_id", vValue:=(m_vPolarisObjectID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter polaris_object_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="is_selectable_for_screen", vValue:=CStr(If(Not m_iIsSelectableForScreen, gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_selectable_for_screen", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="is_non_gis", vValue:=CStr(m_lIsNonGIS), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_non_gis", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="Edit_Flags", vValue:=CStr(m_lEditFlags), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter edit_flags", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertGISObjectSQL, sSQLName:=ACInsertGISObjectName, bStoredProcedure:=ACInsertGISObjectStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add process " & ACInsertGISObjectSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lGISObjectID = gPMFunctions.NullToLong(.Item("gis_object_id").Value)

            End With


            'Now add the Properties
            m_lReturn = CType(AddGISProperties(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process AddGISProperties()", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                Return result
            End If

            'Now create the Table
            m_lReturn = CreateTable()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process CreateTable()", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' CreateTable
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateTable() As gPMConstants.PMEReturnCode

        Dim nResult As Integer
        Dim sSQL As New StringBuilder
        Dim sTemp As String = ""
        Dim vKeyArray() As Object = Nothing
        Dim sDataType As String = ""
        Dim sForeignKeyCols As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            sSQL = New StringBuilder("CREATE TABLE " & m_sTableName & "(")

            For Each oProperty As GISProperty In m_colGISProperty

                With oProperty

                    If Not .EditFlags And GISSharedPropertyConstants.GISDSEditNoDBColumn Then
                        sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & .ColumnName & " ")

                        Select Case .GISDataType
                            Case GISSharedConstants.GISDataTypeComment
                                sSQL.Append(" TEXT")
                            Case GISSharedConstants.GISDataTypeText
                                sSQL.Append(" VARCHAR(")
                                sTemp = ""
                                If .IsSearchProperty Then
                                    sTemp = "40"
                                ElseIf (.SpecialsType = GISSharedPropertyConstants.ACOGISListID) Then
                                    sTemp = "70"
                                Else
                                    sTemp = "255"
                                End If
                                sSQL.Append(sTemp & ")")
                            Case GISSharedConstants.GISDataTypeNumeric, GISSharedConstants.GISDataTypeInteger
                                sSQL.Append(" INT")
                            Case GISSharedConstants.GISDataTypeDate
                                sSQL.Append(" DATETIME")
                            Case GISSharedConstants.GISDataTypeOption
                                sSQL.Append(" TINYINT")
                            Case GISSharedConstants.GISDataTypeCurrency
                                If .OverrideDefaultDataType Then
                                    sDataType = "NUMERIC( " & 19 + .OverrideDefaultDataTypeByPrec & "," & 4 + .OverrideDefaultDataTypeByScale & ")"
                                    sSQL.Append(" " & sDataType & " ")
                                Else
                                    If (m_sTableName).ToUpper() = (m_sGISDataModel & "_OUTPUT").ToUpper() AndAlso (oProperty.ColumnName).ToUpper() = "RATE" Then
                                        sSQL.Append(" NUMERIC(21,6)")
                                    Else
                                        sSQL.Append(" NUMERIC(19,4)")
                                    End If
                                End If
                            Case GISSharedConstants.GISDataTypePercentage
                                If .OverrideDefaultDataType Then
                                    sDataType = "NUMERIC( " & 9 + .OverrideDefaultDataTypeByPrec & "," & 6 + .OverrideDefaultDataTypeByScale & ")"
                                    sSQL.Append(sDataType.ToString())
                                Else
                                    sSQL.Append(" NUMERIC(9,6)")
                                End If

                                ' MEvans : 23/07/2003 : 223 Document Production Changes
                            Case GISSharedConstants.GISDataTypecode
                                sSQL.Append(" CHAR(10)")
                                '****************
                            Case GISSharedConstants.GISDataTypeText
                                sSQL.Append("uniqueidentifier DEFAULT NEWSEQUENTIALID(),")

                        End Select

                        'Set Nullability
                        If .IsPrimaryKey Then
                            sSQL.Append(" NOT")
                            'Also add to Primary Key array
                            If Informations.IsArray(vKeyArray) Then

                                ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
                            Else
                                ReDim vKeyArray(0)
                            End If

                            vKeyArray(vKeyArray.GetUpperBound(0)) = .ColumnName
                        End If
                        sSQL.Append(" NULL,")
                    End If
                End With
            Next oProperty


            'Finish off the create script
            sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1) & ")" & Strings.ChrW(13) & Strings.ChrW(10))

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Create table", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                Return nResult
            End If

            'Add the primary key
            If m_lGISDataModelType = ACPolicyBinder Then

                sSQL = New StringBuilder("ALTER TABLE " & m_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

                sSQL.Append("ADD PRIMARY KEY CLUSTERED (" & m_colGISProperty(0).ColumnName & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add primary key", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                    Return nResult
                End If

                'Create the index linking to the GIS_policy_link
                sSQL = New StringBuilder("CREATE INDEX XFK" & m_sTableName & " ON " & m_sTableName & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append("(gis_policy_link_id)" & Strings.ChrW(13) & Strings.ChrW(10))

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add index", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                    Return nResult
                End If


            Else
                ' SET 04112002 - Get primary key fields
                sSQL = New StringBuilder("ALTER TABLE " & m_sTableName & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append("ADD PRIMARY KEY CLUSTERED (")
                ' create a string containing the names of all primary key fields
                sTemp = ""
                For iTemp As Integer = 1 To m_colGISProperty.Count

                    If m_colGISProperty(iTemp).IsPrimaryKey Then

                        sTemp = sTemp & m_colGISProperty(iTemp).ColumnName & ", "
                    Else
                        Exit For
                    End If
                Next iTemp

                If sTemp.Length Then
                    If sTemp.EndsWith(", ") Then
                        sTemp = sTemp.Substring(0, sTemp.Length - 2)
                    End If
                    sSQL.Append(sTemp & ")")
                Else
                    ' error cos no primary key defined
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No primary key defined for table " & m_sTableName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                    Return nResult
                End If
                ' SET 04112002 - End

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add primary key", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                    Return nResult
                End If

                'Create the index linking to the GIS_policy_link
                sSQL = New StringBuilder("CREATE INDEX XFK" & m_sTableName & " ON " & m_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

                sSQL.Append("(" & m_colGISProperty(1).ColumnName & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add index", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                    Return nResult
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20040914 : Add Index to the following tables
                '               data_model_code_Specials Tables's  data_model_code_Policy_binder_id Property
                '               data_model_code__Disclosure's risk_cnt property
                '               Ref. Renewal Deadlock Issue and Performance enhancement of Renewal Processing
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' SPECIALS OBJECT
                If m_sTableName.ToLower() = (m_sGISDataModel & "_Specials").ToLower() Then
                    sSQL = New StringBuilder("CREATE INDEX [I__" & m_sTableName & "__" & m_sGISDataModel & "_Policy_Binder_id] ON [dbo].[" & m_sTableName & "] " & Strings.ChrW(13) & Strings.ChrW(10))
                    sSQL.Append("([" & m_sGISDataModel & "_Policy_Binder_id]" & ")  ON [PRIMARY]" & Strings.ChrW(13) & Strings.ChrW(10))

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add index", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Index. " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                        Return nResult
                    End If
                End If

                Dim stringArray() As Char = sTemp.ToCharArray()
                Array.Reverse(stringArray)
                sForeignKeyCols = Mid(sTemp, 1, sTemp.Length - ((New String(stringArray)).IndexOf(","c) + 1))
                sSQL = New StringBuilder("ALTER TABLE " & m_sTableName)
                sSQL.Append(" ADD FOREIGN KEY (" & sForeignKeyCols & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                ' RAW 02/09/2003 : CQ2158 : add delete cascade
                sSQL.Append("REFERENCES " & m_sParentTableName & GetDeleteCascadeText() & Strings.ChrW(13) & Strings.ChrW(10))

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add foreign key", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable")
                    Return nResult
                End If

            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTable", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                For Each oGISProperty As GISProperty In m_colGISProperty

                    oGISProperty.Dispose()
                    m_colGISProperty.Remove(1)

                Next oGISProperty
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub New()
        MyBase.New()
    End Sub

    ' ***************************************************************** '
    '
    ' Name: addKeys
    '
    ' Description:
    '
    ' History: 17/12/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function addKeys(ByVal v_vKeys() As Object, ByRef r_iPropertyCount As Integer) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addKeys")



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oGISProperty As GISProperty
        Dim sName As String = ""
        For iLoop As Integer = 0 To v_vKeys.GetUpperBound(0)
            r_iPropertyCount += 1
            oGISProperty = New GISProperty()
            With oGISProperty
                .ID = r_iPropertyCount
                .Database = m_oDatabase
                .GISObjectID = m_lGISObjectID

                sName = CStr(v_vKeys(iLoop))
                'gis_policy_link_id does not have a data model prefix
                If sName <> "gis_policy_link_id" Then
                    sName = m_sGISDataModel & "_" & sName
                End If
                .PropertyName = sName
                .ColumnName = sName
                .GISDataType = GISSharedConstants.GISDataTypeNumeric
                .IsIdentifyingProperty = True
                .IsPrimaryKey = True
                m_lReturn = .AddToDatabase()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Key " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeys")
                End If
                'Add to collection
                m_colGISProperty.Add(oGISProperty)
            End With
            oGISProperty = Nothing
        Next

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addKeys")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: addProperty
    '
    ' Description:
    '
    ' History: 19/12/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function addProperty(ByVal v_sName As String, ByVal v_lType As Integer, ByVal v_bInputProperty As Boolean,
                                 ByVal v_vEditFlags As Integer, ByRef r_iPropertyCount As Integer,
                                 Optional ByRef v_vSpecialsType As Object = Nothing,
                                 Optional ByRef v_vSpecialsTypeReference As Object = Nothing) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addProperty")



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oGISProperty As GISProperty

        r_iPropertyCount += 1
        oGISProperty = New GISProperty()
        With oGISProperty
            .ID = r_iPropertyCount
            .Database = m_oDatabase
            .GISObjectID = m_lGISObjectID
            .PropertyName = v_sName
            .ColumnName = v_sName
            .GISDataType = v_lType
            .IsInputProperty = v_bInputProperty
            .EditFlags = v_vEditFlags


            .SpecialsType = If(Informations.IsNothing(v_vSpecialsType), GISSharedPropertyConstants.ACOSpecialNone, CInt(v_vSpecialsType))


            'refer Developer Guide No. 24
            .SpecialsTypeReference = If(Informations.IsNothing(v_vSpecialsTypeReference), GISSharedPropertyConstants.ACOSpecialNone, v_vSpecialsTypeReference)

            m_lReturn = .AddToDatabase()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISProperties")
            End If

            'Add to collection
            m_colGISProperty.Add(oGISProperty)

        End With
        oGISProperty = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addProperty")

        Return result

    End Function
    Private Function AddGISPropertiesColumns(ByRef r_oGISProperty As GISProperty, ByRef r_colGISProperty As ArrayList, ByRef r_iPropertyCount As Integer, ByVal v_sColumnName As String, ByRef r_vGISSharedConstants As Object, Optional ByVal v_bOverrideDefaultDataType As Boolean = False, Optional ByVal v_iOverrideDefaultDataTypeByPrec As Integer = 0, Optional ByVal v_iOverrideDefaultDataTypeByScale As Integer = 0) As Integer

        'v_iOverrideDefaultDataTypeBy value should not make the value of scale & precision greater than 38.[SQLSERVER VERSION DEPENDENT]
        'scale should always be less than precision
        'v_iOverrideDefaultDataTypeBy will be used in CreateTable() function , 
        'Applicable for GISSharedConstants.GISDataTypeCurrency  & GISSharedConstants.GISDataTypePercentage
        Dim result As Integer = gPMConstants.PMEReturnCode.PMFalse

        r_iPropertyCount += 1
        r_oGISProperty = New GISProperty()
        With r_oGISProperty
            .ID = r_iPropertyCount
            .Database = m_oDatabase
            .GISObjectID = m_lGISObjectID
            .PropertyName = v_sColumnName
            .ColumnName = v_sColumnName
            .GISDataType = r_vGISSharedConstants
            .OverrideDefaultDataType = v_bOverrideDefaultDataType
            .OverrideDefaultDataTypeByPrec = v_iOverrideDefaultDataTypeByPrec
            .OverrideDefaultDataTypeByScale = v_iOverrideDefaultDataTypeByScale
            m_lReturn = .AddToDatabase()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Property " & .PropertyName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGISPropertiesColumns")
            End If
        End With
        r_colGISProperty.Add(r_oGISProperty)
        r_oGISProperty = Nothing
        Return result

    End Function
End Class
