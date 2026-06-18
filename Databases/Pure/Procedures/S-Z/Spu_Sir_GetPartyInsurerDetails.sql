SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Sir_GetPartyInsurerDetails'
GO

CREATE PROCEDURE Spu_Sir_GetPartyInsurerDetails  
@PartyCnt int = Null,  
@Branch varchar(1020) = Null,  
@FileCode Varchar(10)= Null,  
@ShortName Varchar(20) = Null,  
@Name Varchar(255) = Null,  
@is_ri_broker int = Null,  
@Is_FAX int = Null ,  
@ri_type_code Varchar(10) = Null,  
@is_retained varchar(20)= Null  
AS  
  
Declare @sSQL Varchar(4000)  
Declare @Broker Varchar(10)  
Declare @Retained Varchar(10)  
Declare @Reinsurer Varchar(10)  
IF  @Branch IS NULL
	SELECT @Branch=source_id from party where party_cnt=@PartyCnt
Set @Broker ='Broker'  
Set @Retained = 'Retained'  
Set @Reinsurer ='Reinsurer'  
SET @ShortName = REPLACE(@ShortName,'''','''''' )

Set @sSQL=' SELECT DISTINCT  
 shortname,  
 name,  
 Acct_Type = CASE WHEN is_RI_Broker=1  
    THEN ''' + @Broker + '''  
    WHEN is_Retained=1  
    THEN ''' + @Retained + '''  
    ELSE ''' + @Reinsurer  + ''' END,  
 address.address1,  
 address.address2,  
 address.postal_code,  
 reinsurance_type,  
 party.file_code,  
 Party.source_id,  
 Source.Description,  
 Party.party_cnt,  
 party_type.description,  
 Party_Insurer.default_comm_rate/100,  
 Party.domiciled_for_tax, 
 is_RI_Broker,  
 is_retained,  
 Reinsurance_Type.code,  
 Source.code,  
 party.tax_exempt,  
 party.tax_percentage,  
 party_insurer.tax_group_id,  
 party.tax_number,  
 tax_group.code   
 FROM Party WITH(INDEX(I__Party__party_type_id)),  
 Address_Usage_Type,  
 Party_Type,  
 Reinsurance_Type,  
 Source,  
 Address,  
 Party_Address_Usage,  
 Party_Insurer LEFT JOIN Tax_group on Party_Insurer.tax_group_id=tax_group.tax_group_id 
 WHERE Party_Type.party_type_id = Party.party_type_id  
 AND Party.is_deleted = 0  
 AND Party.party_cnt = Party_Insurer.party_cnt  
 AND party_insurer.reinsurance_type = Reinsurance_Type.reinsurance_type_id  
 AND Source.source_id = Party.source_id  
 AND Party_Address_Usage.party_cnt = Party.party_cnt  
 AND Party_Address_Usage.address_cnt = Address.address_cnt  
 AND Address_Usage_Type.address_usage_type_id = Party_Address_Usage.address_usage_type_id  
 AND party.source_id in (' + @Branch + ')'  
  
if @FileCode is not null  
set @sSql = @sSql + ' AND party.file_code like ''' + @FileCode + ''''  
  
if @ShortName is not null  
set @sSql = @sSql + ' AND party.shortname like ''' + @ShortName + ''''  
  
if @Name is not null  
set @sSql = @sSql + ' AND party.name like ''' + @Name + ''''  
  
if @is_ri_broker is not null  
Begin  
	set @sSql = @sSql + ' AND party_insurer.is_ri_broker <> 1 '  
	set @sSql = @sSql + ' AND Party_Insurer.is_retained <> 1'

	if @PartyCnt is not null  
		set @sSql = @sSql + ' AND party.party_cnt <> ' + convert(varchar(10),@PartyCnt)  
End  
else  
Begin  
	if @PartyCnt is not null  
	set @sSql = @sSql + ' AND party.party_cnt = ' + convert(varchar(10),@PartyCnt)  
End  

If @ri_type_code IS NULL  
BEGIN    
if @Is_FAX = 1  
	set @sSql = @sSql + ' AND (reinsurance_type.code = ''FAX'' or reinsurance_type.code = ''FAC'' or reinsurance_type.code = ''RET'')'  
else  
	set @sSql = @sSql + ' AND (reinsurance_type.code = ''FAP'' or reinsurance_type.code = ''FAC'')'  
END  
ELSE  
 set @sSql = @sSql + ' AND (reinsurance_type.code = ''' + @ri_type_code + ''')'  
  
if @is_retained is not null  
set @sSql = @sSql + ' AND Party_Insurer.is_retained = 1'  
 	    
set @sSql = @sSql + ' Order By Shortname'  
  
Exec (@sSql)  

