Option Strict Off
Option Explicit On
Imports Microsoft.ApplicationBlocks.ExceptionManagement

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  22/05/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "cGISDataSetControl"

    ' Constant for the functions to identify
    ' which class this is.
    Public Const ACClass As String = "MainModule"

    Public Const ACSTSClientUserName As String = "STSClient"

    ' ***************************************************************** '
    Public Const ACPKColSuffix As String = "_ID"
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Object Instance Key Constants
    ' ***************************************************************** '

    Public Const ACObjectTagPrefix As String = "O"
    Public Const ACPropertyTagPrefix As String = "P"
    ' RFC250300 - Change OIKey separator to an underscore
    Public Const ACOIKeySeparator As String = "_"
    Public Const ACQuotePrefix As String = "S"

    ' ***************************************************************** '
    ' XML Stream Constants
    ' ***************************************************************** '
    ' Tags
    ' Note, Upercase is used as XML Tags are case sensitive
    Public Const ACXMLDataSet As String = "DATA_SET"
    Public Const ACXMLDataSetDef As String = "DATA_SET_DEFINITION"
    Public Const ACXMLRiskObjects As String = "RISK_OBJECTS"
    Public Const ACXMLQuoteObjects As String = "QUOTE_OBJECTS"
    Public Const ACXMLQuotes As String = "QUOTES"
    Public Const ACXMLDeletedObjects As String = "DELETED_OBJECTS"

    ' Attribute Names

    ' Object Def Attributes
    Public Const ACXMLAttribObjectID As String = "ObjectID"
    Public Const ACXMLAttribObjectName As String = "ObjectName"
    Public Const ACXMLAttribTableName As String = "TableName"
    Public Const ACXMLAttribMaxInstances As String = "MaxInstances"
    Public Const ACXMLAttribPolarisObjectID As String = "PolarisObjectID"
    Public Const ACXMLAttribIsQuoteObject As String = "IsQuoteObject"
    Public Const ACXMLAttribNextOINumber As String = "NextOINumber"

    ' RFC300103
    Public Const ACXMLAttribIsSelForScreen As String = "IsSelScreen"
    Public Const ACXMLAttribIsNonGIS As String = "IsNonGIS"
    Public Const ACXMLAttribEditFlags As String = "EditFlags"

    ' Property Def Attributes
    Public Const ACXMLAttribPropertyID As String = "PropertyID"
    Public Const ACXMLAttribPropertyName As String = "PropertyName"
    Public Const ACXMLAttribColumnName As String = "ColumnName"
    Public Const ACXMLAttribDataType As String = "DataType"
    Public Const ACXMLAttribIsPrimaryKey As String = "IsPrimaryKey"
    Public Const ACXMLAttribIsIdentProp As String = "IsIdentProp"
    Public Const ACXMLAttribGISListID As String = "GISListID"
    Public Const ACXMLAttribPolarisPropertyID As String = "PolarisPropertyID"

    ' Quotes Attributes
    Public Const ACXMLAttribNextQuoteNum As String = "NextQuoteNumber"

    ' Quote Attributes
    Public Const ACXMLAttribInsurer As String = "Insurer"
    Public Const ACXMLAttribInsurerID As String = "InsurerID"
    Public Const ACXMLAttribScheme As String = "Scheme"
    Public Const ACXMLAttribSchemeID As String = "SchemeID"
    Public Const ACXMLAttribSchemeVer As String = "SchemeVer"

    ' Object/Property Attributes
    'Public Const ACXMLAttribNextChildNum As String = "NextChildNumFor"
    'Public Const ACXMLAttribChildNum As String = "ChildNum"
    ' RFC 290200 - Attribute names shortened to reduce xml size
    Public Const ACXMLAttribOIKey As String = "OI"
    Public Const ACXMLAttribUpdateStatus As String = "US"
    Public Const ACXMLAttribIsAssumedInfo As String = "IA"
    Public Const ACXMLAttribSQLInsert As String = "SI"
    Public Const ACXMLAttribSQLUpdate As String = "SU"
    Public Const ACXMLAttribSQLDelete As String = "SD"
    Public Const ACXMLAttribSQLSelect As String = "SS"
    ' RFC 170501 - Clear Quote Output Attribute Added
    Public Const ACXMLAttribClearQuotes As String = "CQ"
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' DTD Constants
    ' ***************************************************************** '
    ' Tags

    Public Const ACDTDVersion As String = "<?xml version='1.0'?>"
    Public Const ACDTDDocType As String = "<!DOCTYPE "
    Public Const ACDTDDocTypeStart As String = " [ "
    Public Const ACDTDDocTypeEnd As String = "]>"
    Public Const ACDTDElement As String = "<!ELEMENT "
    Public Const ACDTDElementStart As String = " ("
    Public Const ACDTDElementEnd As String = ")> "
    Public Const ACDTDElementSep As String = " , "
    Public Const ACDTDElementMultiple As String = "*"
    Public Const ACDTDElementZeroOrOne As String = "?"
    Public Const ACDTDPCData As String = "#PCDATA"
    Public Const ACDTDAttrrib As String = "<!ATTLIST "
    Public Const ACDTDCData As String = "CDATA "
    Public Const ACDTDAny As String = "ANY>"
    Public Const ACDTDID As String = "ID "
    Public Const ACDTDRequired As String = "#REQUIRED> "
    Public Const ACDTDImplied As String = "#IMPLIED> "

    ' Example DTD
    '<!DOCTYPE Policy [
    '    <!ELEMENT Policy (Vehicle,Driver*)>
    '    <!ELEMENT Vehicle (Registration,Value)>
    '    <!ELEMENT Registration (#PCDATA)>
    '    <!ELEMENT Value (#PCDATA)>
    '    <!ELEMENT Driver (Surname,First_Name,DOB)>
    '    <!ELEMENT Surname (#PCDATA)>
    '    <!ELEMENT First_Name (#PCDATA)>
    '    <!ELEMENT DOB (#PCDATA)>
    '    <!ATTLIST Policy Policy_Number CDATA #REQUIRED>
    '    <!ATTLIST Policy Inception_Date CDATA #REQUIRED>
    '    <!ATTLIST Driver id ID #REQUIRED>
    ']>


    Public Const ACOIMGISSubKey As String = "GIS" ' CL230200

    ' RFC 290200 - Add the SQL Statements as Attributes of the Object
    ' SQL Constants moved from bGIS to here as we are now building the SQL Statements
    ' and holding them in the data set definition.

    ' Common
    Public Const ACSQLStartCol As String = " ( "
    Public Const ACSQLEndCol As String = " ) "
    Public Const ACSQLStartParam As String = " {"
    Public Const ACSQLEndParam As String = "} "
    Public Const ACSQLSeparator As String = " , "
    Public Const ACSQLAnd As String = " AND "
    Public Const ACSQLEquals As String = " = "
    Public Const ACSQLWhere As String = " WHERE "
    Public Const ACSQLParamPrefix As String = "@"

    ' Add SQL Constants
    Public Const ACAddSQLStart As String = "INSERT INTO "
    Public Const ACAddSQLValues As String = " VALUES "

    ' Update SQL Constants
    Public Const ACUpdSQLStart As String = "UPDATE "
    Public Const ACUpdSQLSet As String = " SET "

    ' Select SQL Constants
    Public Const ACSelSQLSelect As String = "SELECT "
    Public Const ACSelSQLFrom As String = " FROM "
    Public Const ACSelSQLOrderBy As String = " ORDER BY "
    Public Const ACSelSQLAsc As String = " ASC"

    'RFC200400 - Add Proper Delete Functionality
    ' Delete SQL Constants
    Public Const ACDelSQLDeleteFrom As String = "DELETE FROM "
    Public Const ACDelSQLWhere As String = " WHERE "

    ' ID Col
    Public Const ACIDCol As String = "_ID"

    ' ***************************************************************** '
    ' Name: LogMessage
    '
    ' Description: Wrapper function to the log message method of the
    '              message object.
    '
    ' ***************************************************************** '
    Public Sub LogMessage(ByRef iType As gPMConstants.PMELogLevel, ByRef sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object)
        LogMessage(iType, sMsg, vApp, vClass, vMethod, Nothing)
    End Sub
    Public Sub LogMessage(ByRef iType As gPMConstants.PMELogLevel, ByRef sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef excep As Exception)

        'Dim lErrorValue As Integer
        'Dim vTimestamp As Object
        'Dim lMessageID As Integer

        Try

            ' Check if we need to log this message.

            ' We cannot Initialise PMMessage, Log to Screen
            sMsg = "The following error occured in " & vApp & " : " & sMsg
            If excep IsNot Nothing AndAlso excep.InnerException IsNot Nothing Then
                sMsg = sMsg & Environment.NewLine & Environment.NewLine & excep.InnerException.ToString
            End If
            If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
                sMsg = sMsg & Environment.NewLine & excep.Message
            End If
            Dim ErrEx As New Exception(sMsg)
            ExceptionManager.Publish(ErrEx)

            Exit Sub

        Catch ex As Exception

            ' Error Section.

            Exit Sub

        End Try
    End Sub

End Module