Option Strict Off
Option Explicit On
Imports SSP.Shared
'refer Developer Guide No. 129
Friend NotInheritable Class GISProperty

    Implements IDisposable
    Private Const ACClass As String = "GISProperty"

    Private m_lID As Integer = -1
    Private m_lGISPropertyID As Integer
    Private m_lGISObjectID As Integer = 0
    Private m_sPropertyName As String = ""
    Private m_sColumnName As String = ""
    Private m_lGISDataType As Integer = 0
    Private m_bIsInputProperty As Boolean = False
    Private m_bIsIdentifyingProperty As Boolean = False
    Private m_bIsPrimaryKey As Boolean = False

    Private m_vPolarisPropertyID As Object = DBNull.Value
    Private m_bIsDeleted As Boolean = False
    Private m_bIsSearchProperty As Boolean = False

    Private m_vIndexLinkingID As Object = DBNull.Value
    Private m_lEditFlags As Integer = 0
    Private m_lSpecialsType As Integer = 0

    Private m_vSpecialsTypeReference As Object = DBNull.Value
    Private m_bIsInMISExport As Boolean
    Private m_bisFormattedText As Boolean
    Private m_bOverrideDefaultDataType As Boolean
    Private m_iOverrideDefaultDataTypeByPrec As Integer
    Private m_iOverrideDefaultDataTypeByScale As Integer
    Private m_lIsChaseCycleProperty As Boolean
    Private m_uUIDProperty As String
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

    Public Property GISPropertyID() As Integer
        Get
            Return m_lGISPropertyID
        End Get
        Set(ByVal Value As Integer)
            m_lGISPropertyID = Value
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

    Public Property ColumnName() As String
        Get
            Return m_sColumnName
        End Get
        Set(ByVal Value As String)
            m_sColumnName = Value
        End Set
    End Property

    Public Property PropertyName() As String
        Get
            Return m_sPropertyName
        End Get
        Set(ByVal Value As String)
            m_sPropertyName = Value
        End Set
    End Property

    Public Property GISDataType() As Integer
        Get
            Return m_lGISDataType
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataType = Value
        End Set
    End Property

    Public Property IsInputProperty() As Boolean
        Get
            Return m_bIsInputProperty
        End Get
        Set(ByVal Value As Boolean)
            m_bIsInputProperty = Value
        End Set
    End Property

    Public Property IsIdentifyingProperty() As Boolean
        Get
            Return m_bIsIdentifyingProperty
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIdentifyingProperty = Value
        End Set
    End Property

    Public Property IsPrimaryKey() As Boolean
        Get
            Return m_bIsPrimaryKey
        End Get
        Set(ByVal Value As Boolean)
            m_bIsPrimaryKey = Value
        End Set
    End Property

    Public Property PolarisPropertyID() As Object
        Get
            Return m_vPolarisPropertyID
        End Get
        Set(ByVal Value As Object)


            m_vPolarisPropertyID = Value
        End Set
    End Property

    Public Property IsDeleted() As Boolean
        Get
            Return m_bIsDeleted
        End Get
        Set(ByVal Value As Boolean)
            m_bIsDeleted = Value
        End Set
    End Property

    Public Property IsSearchProperty() As Boolean
        Get
            Return m_bIsSearchProperty
        End Get
        Set(ByVal Value As Boolean)
            m_bIsSearchProperty = Value
        End Set
    End Property

    Public Property IndexLinkingID() As Object
        Get
            Return m_vIndexLinkingID
        End Get
        Set(ByVal Value As Object)


            m_vIndexLinkingID = Value
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

    Public Property SpecialsType() As Integer
        Get
            Return m_lSpecialsType
        End Get
        Set(ByVal Value As Integer)
            m_lSpecialsType = Value
        End Set
    End Property

    Public Property SpecialsTypeReference() As Object
        Get
            Return m_vSpecialsTypeReference
        End Get
        Set(ByVal Value As Object)


            m_vSpecialsTypeReference = Value
        End Set
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)
            m_oDatabase = Value
        End Set
    End Property


    Public Property IsInMISExport() As Boolean
        Get
            Return m_bIsInMISExport
        End Get
        Set(ByVal Value As Boolean)
            m_bIsInMISExport = Value
        End Set
    End Property

    Public Property IsFormattedText() As Boolean
        Get
            Return m_bisFormattedText
        End Get
        Set(ByVal Value As Boolean)
            m_bisFormattedText = Value
        End Set
    End Property





    Public Property OverrideDefaultDataType() As Boolean
        Get
            Return m_bOverrideDefaultDataType
        End Get
        Set(ByVal Value As Boolean)
            m_bOverrideDefaultDataType = Value
        End Set
    End Property

    Public Property OverrideDefaultDataTypeByPrec() As Integer
        Get
            Return m_iOverrideDefaultDataTypeByPrec
        End Get
        Set(ByVal value As Integer)
            m_iOverrideDefaultDataTypeByPrec = value
        End Set
    End Property

    Public Property OverrideDefaultDataTypeByScale() As Integer
        Get
            Return m_iOverrideDefaultDataTypeByScale
        End Get
        Set(ByVal value As Integer)
            m_iOverrideDefaultDataTypeByScale = value
        End Set
    End Property

    Public Property IsChaseCycleProperty() As Boolean
        Get
            Return m_lIsChaseCycleProperty
        End Get
        Set(ByVal Value As Boolean)


            m_lIsChaseCycleProperty = CInt(Value)
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

    Public Function AddToDatabase() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim iBoolean As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase.Parameters

                .Clear()

                'Add GISPropertyID as OUTPUT
                m_lReturn = .Add(sName:="gis_property_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter gis_property_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                'Now the INPUT parameters
                m_lReturn = .Add(sName:="gis_object_id", vValue:=CStr(m_lGISObjectID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter gis_object_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="property_name", vValue:=m_sPropertyName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter property_name", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="column_name", vValue:=m_sColumnName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter column_name", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="data_type", vValue:=CStr(m_lGISDataType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter data_type", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                iBoolean = If(m_bIsInputProperty, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                ' Developer Guide No. 101
                m_lReturn = .Add(sName:="is_input_property", vValue:=(iBoolean), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_input_property", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                iBoolean = If(m_bIsIdentifyingProperty, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                m_lReturn = .Add(sName:="is_identifying_property", vValue:=CStr(iBoolean), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_identifying_property", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                iBoolean = If(m_bIsPrimaryKey, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                m_lReturn = .Add(sName:="is_primary_key", vValue:=CStr(iBoolean), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_primary_key", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If


                ' Developer Guide No. 101
                m_lReturn = .Add(sName:="polaris_property_id", vValue:=(m_vPolarisPropertyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter polaris_property_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                iBoolean = If(m_bIsDeleted, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                m_lReturn = .Add(sName:="is_deleted", vValue:=CStr(iBoolean), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_deleted", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                iBoolean = If(m_bIsSearchProperty, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                m_lReturn = .Add(sName:="is_search_property", vValue:=CStr(iBoolean), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_search_property", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If


                ' Developer Guide No. 101
                m_lReturn = .Add(sName:="index_linking_id", vValue:=(m_vIndexLinkingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter index_linking_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="Edit_Flags", vValue:=CStr(m_lEditFlags), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter edit_flags", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="Specials_Type", vValue:=CStr(m_lSpecialsType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter specials_type", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If


                ' Developer Guide No. 101
                m_lReturn = .Add(sName:="Specials_Type_Reference", vValue:=Convert.ToString(m_vSpecialsTypeReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter specials_type_reference", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="is_in_mis_export", vValue:=CStr(m_bIsInMISExport), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter specials_type_reference", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = .Add(sName:="is_formatted_text", vValue:=CStr(m_bisFormattedText), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter specials_type_reference", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If
                iBoolean = If(m_lIsChaseCycleProperty, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse)
                m_lReturn = .Add(sName:="is_chase_cycle_property", vValue:=CStr(iBoolean), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter is_search_property", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertGISPropertyParamSQL, sSQLName:=ACInsertGISPropertyParamName, bStoredProcedure:=ACInsertGISPropertyParamStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & ACInsertGISPropertySQL, vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase")
                    Return result
                End If

                m_lGISPropertyID = gPMFunctions.NullToLong(.Item("gis_property_id").Value)

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddToDatabase", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub New()
        MyBase.New()
    End Sub
End Class
