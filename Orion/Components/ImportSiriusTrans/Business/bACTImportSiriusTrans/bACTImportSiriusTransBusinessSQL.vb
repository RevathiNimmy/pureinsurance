Option Strict On
Option Explicit On
Imports System
Module BusinessSQL

    Public Const kDoAccountExistsStored As Boolean = True
    Public Const kDoAccountExistsName As String = "DoAccountExists"
    Public Const kDoAccountExistsSQL As String = "spu_Get_AccountIdFromShortCode"

    Public Const ACGetStatsCurrencyXRateStored As Boolean = True
    Public Const ACGetStatsCurrencyXRateName As String = "GetStatsCurrencyXRate"
    Public Const ACGetStatsCurrencyXRateSQL As String = "spu_GetStatsCurrencyXRate"

    Public Const kRoundingStored As Boolean = True
    Public Const kRoundingName As String = "Rounding"
    Public Const kRoundingSQL As String = "spu_ACT_Trans_Detail_Rounding"

End Module
