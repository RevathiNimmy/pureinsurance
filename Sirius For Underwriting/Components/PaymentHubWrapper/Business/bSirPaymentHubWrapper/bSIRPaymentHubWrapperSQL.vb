Option Strict Off
Option Explicit On

Module bSIRPaymentHubWrapperSQL

    Public Const kGetPaymentHUBConfigurationsSql As String = "spu_Get_Payment_HUB_Configurations"
    Public Const kGetPaymentHUBConfigurationsName As String = "Get_Payment_HUB_Configurations"

    Public Const kGetPartyCorrospondanceAddressCntStored As Boolean = True
    Public Const kGetPartyCorrospondanceAddressCntName As String = "Get_party_corrospondance_address_cnt"
    Public Const kGetPartyCorrospondanceAddressCntSQL As String = "spu_Get_party_corrospondance_address_cnt"

    Public Const kAddressSelStored As Boolean = True
    Public Const kAddressSelName As String = "Address_sel"
    Public Const kAddressSelSQL As String = "spe_Address_sel"

    Public Const kSelEmailContactStored As Boolean = True
    Public Const kSelEmailContactName As String = "Get Main Email ContactAddress"
    Public Const kSelEmailContactSQL As String = "spu_email_contact_select"

    Public Const kAddAndUpdateCashListDetailsStored As Boolean = True
    Public Const kAddAndUpdateCashListDetailsName As String = "Add And Update CashList Details"
    Public Const kAddAndUpdateCashListDetailsSQL As String = "spu_Add_And_Update_CashList_Details"

End Module