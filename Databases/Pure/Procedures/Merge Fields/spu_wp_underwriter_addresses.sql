SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_underwriter_addresses'
go

CREATE PROCEDURE spu_wp_underwriter_addresses
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT


AS

DECLARE @AddressDescription Varchar(255)
DECLARE @AddressCode char(10)
Declare @ssql varchar(8000)
Declare @ssql3 varchar(8000)
DECLARE @TempAddressType Varchar(25)
DECLARE @Address1 Varchar(60)
DECLARE @Address2 Varchar(60)
DECLARE @Address3 Varchar(60)
DECLARE @Address4 Varchar(60)
DECLARE @Postcode Varchar(20)
DECLARE @Country Varchar(60)
DECLARE @CorrespondenceCode char(10)

if exists (select * from tempdb..sysobjects where name = '#tempaddresses')
    drop table #tempaddresses

SET NOCOUNT ON

SELECT @CorrespondenceCode='3131 XCO'


DECLARE cAddresses CURSOR SCROLL FOR
    Select description, code from address_usage_type where is_deleted=0
    
OPEN cAddresses

select @ssql = 'address_id int'
FETCH FIRST FROM cAddresses INTO @AddressDescription, @AddressCode
WHILE @@FETCH_STATUS = 0 BEGIN

    select @AddressDescription = replace(@AddressDescription,'!','') 
    select @AddressDescription = replace(@AddressDescription,'"','')
    select @AddressDescription = replace(@AddressDescription,'£','') 
    select @AddressDescription = replace(@AddressDescription,'$','')
    select @AddressDescription = replace(@AddressDescription,'%','') 
    select @AddressDescription = replace(@AddressDescription,'^','')
    select @AddressDescription = replace(@AddressDescription,'&','') 
    select @AddressDescription = replace(@AddressDescription,'*','')
    select @AddressDescription = replace(@AddressDescription,'(','') 
    select @AddressDescription = replace(@AddressDescription,')','')
    select @AddressDescription = replace(@AddressDescription,'-','') 
    select @AddressDescription = replace(@AddressDescription,'+','')
    select @AddressDescription = replace(@AddressDescription,'=','')
    select @AddressDescription = replace(@AddressDescription,'[','') 
    select @AddressDescription = replace(@AddressDescription,']','')
    select @AddressDescription = replace(@AddressDescription,':','') 
    select @AddressDescription = replace(@AddressDescription,';','')
    select @AddressDescription = replace(@AddressDescription,'@','') 
    select @AddressDescription = replace(@AddressDescription,'~','')
    select @AddressDescription = replace(@AddressDescription,'#','') 
    select @AddressDescription = replace(@AddressDescription,'<','')
    select @AddressDescription = replace(@AddressDescription,'>','') 
    select @AddressDescription = replace(@AddressDescription,',','')
    select @AddressDescription = replace(@AddressDescription,'.','') 
    select @AddressDescription = replace(@AddressDescription,'?','')
    select @AddressDescription = replace(@AddressDescription,'/','')
    select @AddressDescription = replace(@AddressDescription,'\','')
    select @AddressDescription = replace(@AddressDescription,'|','')

    if len(@ssql) > 0 
    BEGIN
        select @ssql = @ssql + ','
    END
    select @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address1 VARCHAR(60) NULL, '
    select @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address2 VARCHAR(60) NULL, '
    select @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address3 VARCHAR(60) NULL, '
    select @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address4 VARCHAR(60) NULL, '
    select @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Postcode VARCHAR(20) NULL, '
    select @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Country VARCHAR(60) NULL '
    FETCH NEXT FROM cAddresses INTO @AddressDescription, @AddressCode
END
select @ssql = 'CREATE TABLE #tempaddresses (' + rtrim(@ssql) + ')'

select @ssql3 = ''
FETCH FIRST FROM cAddresses INTO @AddressDescription, @AddressCode
WHILE @@FETCH_STATUS = 0 BEGIN

    select @AddressDescription = replace(@AddressDescription,'!','') 
    select @AddressDescription = replace(@AddressDescription,'"','')
    select @AddressDescription = replace(@AddressDescription,'£','') 
    select @AddressDescription = replace(@AddressDescription,'$','')
    select @AddressDescription = replace(@AddressDescription,'%','') 
    select @AddressDescription = replace(@AddressDescription,'^','')
    select @AddressDescription = replace(@AddressDescription,'&','') 
    select @AddressDescription = replace(@AddressDescription,'*','')
    select @AddressDescription = replace(@AddressDescription,'(','') 
    select @AddressDescription = replace(@AddressDescription,')','')
    select @AddressDescription = replace(@AddressDescription,'-','') 
    select @AddressDescription = replace(@AddressDescription,'+','')
    select @AddressDescription = replace(@AddressDescription,'=','')
    select @AddressDescription = replace(@AddressDescription,'[','') 
    select @AddressDescription = replace(@AddressDescription,']','')
    select @AddressDescription = replace(@AddressDescription,':','') 
    select @AddressDescription = replace(@AddressDescription,';','')
    select @AddressDescription = replace(@AddressDescription,'@','') 
    select @AddressDescription = replace(@AddressDescription,'~','')
    select @AddressDescription = replace(@AddressDescription,'#','') 
    select @AddressDescription = replace(@AddressDescription,'<','')
    select @AddressDescription = replace(@AddressDescription,'>','') 
    select @AddressDescription = replace(@AddressDescription,',','')
    select @AddressDescription = replace(@AddressDescription,'.','') 
    select @AddressDescription = replace(@AddressDescription,'?','')
    select @AddressDescription = replace(@AddressDescription,'/','')
    select @AddressDescription = replace(@AddressDescription,'\','')
    select @AddressDescription = replace(@AddressDescription,'|','')
    
	/*Initialise variables before getting next address.*/
	SELECT @Address1 = NULL
	SELECT @Address2 = NULL
	SELECT @Address3 = NULL
	SELECT @Address4 = NULL
	SELECT @Postcode = NULL
	SELECT @Country = NULL

    /* If passed insurancefilecnt, set underwriter to the underwriter on the policy */
	IF ISNULL(@InsuranceFileCnt, '') <> ''
	BEGIN 
		select @partycnt = ( select fsa_underwriter_cnt from insurance_file where insurance_file_cnt = @InsuranceFileCnt )
	END

    exec spu_wp_get_underwriter_address @PartyCnt, @InsuranceFileCnt, @ClaimCnt,@AddressCode,@Address1 OUTPUT,@Address2 OUTPUT,@Address3 OUTPUT,@Address4 OUTPUT,@Postcode OUTPUT,@Country Output
    
    --Default in Correspondence address if blank
    IF (UPPER(@AddressCode)<>@CorrespondenceCode) AND (@Address1 IS NULL) AND (@Address2 IS NULL) AND (@Address3 IS NULL) AND (@Address4 IS NULL) AND (@Postcode IS NULL) AND (@Country IS NULL)
    BEGIN
    	exec spu_wp_get_underwriter_address @PartyCnt,@InsuranceFileCnt,@ClaimCnt,@CorrespondenceCode,@Address1 OUTPUT,@Address2 OUTPUT,@Address3 OUTPUT,@Address4 OUTPUT,@Postcode OUTPUT,@Country Output
    END
    
    select @Address1 = isnull(@Address1,' ')
    select @Address2 = isnull(@Address2,' ')
    select @Address3 = isnull(@Address3,' ')
    select @Address4 = isnull(@Address4,' ')
    select @Postcode = isnull(@Postcode,' ')
    select @Country = isnull(@Country,' ')
    if len(@ssql3) > 0 
    BEGIN
        select @ssql3 = @ssql3 + ', '
    END
    select @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address1 = ''' + replace(@Address1,'''','''''') + ''', '
    select @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address2 = ''' + replace(@Address2,'''','''''') + ''', '
    select @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address3 = ''' + replace(@Address3,'''','''''') + ''', '
    select @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address4 = ''' + replace(@Address4,'''','''''') + ''', '
    select @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Postcode = ''' + replace(@Postcode,'''','''''') + ''', '
    select @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Country = ''' + replace(@Country,'''','''''') + ''''
    FETCH NEXT FROM cAddresses INTO @AddressDescription, @AddressCode
END
select @ssql3 = 'Update #tempaddresses set ' + @ssql3

CLOSE cAddresses
DEALLOCATE cAddresses

--Execute the Main query
exec (@ssql + 'insert into #tempaddresses([address_id]) values(1)' + @ssql3 + 'select * from #tempaddresses')

if exists (select * from tempdb..sysobjects where name = '#tempaddresses')
    drop table #tempaddresses

GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS OFF
GO
