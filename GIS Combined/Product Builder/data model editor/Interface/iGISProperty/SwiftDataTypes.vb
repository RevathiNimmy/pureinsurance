Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Module SwiftDataTypes
	
	Const ACClass As String = "SwiftDataTypes.bas"
	
	
	' ***************************************************************** '
	' Module Name: SwiftSpecials
	'
	' Date: 19/11/2004
	'
	' Description: Support for Swift Specials types
	'
	' Edit History:
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
    Public Function GetSwiftSpecialListTypeNames(ByRef r_vNames(,) As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim bGISMaintainDataDictionary As Object

        Dim oObjectManager As bObjectManager.ObjectManager

        Dim oBusiness As bGISMaintainDataDictionary.Business
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get an instance of the object manager
            oObjectManager = New bObjectManager.ObjectManager()

            lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bObjectManager.ObjectManager object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSwiftSpecialListTypeNames")
                Return result
            End If

            ' get an instance of the business object
            Dim temp_oBusiness As Object
            lReturn = oObjectManager.GetInstance(temp_oBusiness, "bGISMaintainDataDictionary.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bGISMaintainDataDictionary.Business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSwiftSpecialListTypeNames")
                Return result
            End If




            oBusiness.SwiftIntegration = GISSharedPropertyConstants.SwiftMode_DisplaySpecialsList


            lReturn = oBusiness.GetSwiftSpecialListTypes(r_vListTypesArray:=r_vNames)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to list of swift special list types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSwiftSpecialListTypeNames")
                Return result
            End If




        Catch ex As Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error", vApp:=ACApp, vClass:="SwiftSpecials", vMethod:="GetSwiftSpecialListTypeNames", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally


            oBusiness.Dispose()
            oObjectManager.Dispose()
        End Try
        Return result
    End Function
End Module
