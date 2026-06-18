SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_addresses'
go

CREATE PROCEDURE spu_wp_addresses
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT


AS

DECLARE @AddressDescription Varchar(MAX)
DECLARE @AddressCode char(10)  
Declare @ssql varchar(MAX)
Declare @ssql1 varchar(MAX)
Declare @ssql3 varchar(MAX)
Declare @ssql4 varchar(MAX)
Declare @ssql5 varchar(MAX)
DECLARE @TempAddressType Varchar(25)  
DECLARE @Address1 Varchar(60)  
DECLARE @Address2 Varchar(60)  
DECLARE @Address3 Varchar(60)  
DECLARE @Address4 Varchar(60)  
DECLARE @Address5 Varchar(60)  
 DECLARE @Address6 Varchar(60)  
 DECLARE @Address7 Varchar(60)  
 DECLARE @Address8 Varchar(60)  
 DECLARE @Address9 Varchar(60)  
 DECLARE @Address10 Varchar(60)  

DECLARE @Postcode Varchar(20)  
DECLARE @Country Varchar(60)  
DECLARE @CorrespondenceCode char(10)  
  
DECLARE @sAddress5 Varchar(60)
DECLARE @sAddress6 Varchar(60)
DECLARE @sAddress7 Varchar(60)
DECLARE @sAddress8 Varchar(60)
DECLARE @sAddress9 Varchar(60)
DECLARE @sAddress10 Varchar(60)
if exists (SELECT * from tempdb..sysobjects where name = '#tempaddresses')  
    drop table #tempaddresses  
  
SET NOCOUNT ON  
  
SELECT @CorrespondenceCode='3131 XCO'  
  
-- RAG 2003-09-10  
-- Added 'where is_deleted=0' to cursor to fix problem where  
-- correspondance address was not being merged for CMIB.  
  
DECLARE cAddresses CURSOR SCROLL FOR  
    SELECT description, code from address_usage_type where is_deleted=0  
    -- SELECT description, code from address_usage_type  
  
OPEN cAddresses  
  
SELECT @ssql = 'address_id int'  
SELECT @ssql5 = 'address_id '  
SELECT @ssql1 =''

FETCH FIRST FROM cAddresses INTO @AddressDescription, @AddressCode  
WHILE @@FETCH_STATUS = 0 BEGIN  
    if len(@ssql) > 0  AND len(@ssql1)=0  
    BEGIN  
        SELECT @ssql = @ssql + ','  
        SELECT @ssql5 = @ssql5 + ','  
    END  
  
    if len(@ssql1) > 0  
    BEGIN  
        SELECT @ssql1 = @ssql1 + ', '  
    END 
  
    --DC110304 PN10913 -start -to overcome problem with puntuation in address description and code  
    --    probably better way to do this but for now this will have to do  
    SELECT @AddressDescription = replace(@AddressDescription,'!','')  
    SELECT @AddressDescription = replace(@AddressDescription,'"','')
    SELECT @AddressDescription = replace(@AddressDescription,'£','') 
    SELECT @AddressDescription = replace(@AddressDescription,'$','')  
    SELECT @AddressDescription = replace(@AddressDescription,'%','')  
    SELECT @AddressDescription = replace(@AddressDescription,'^','')  
    SELECT @AddressDescription = replace(@AddressDescription,'&','')  
    SELECT @AddressDescription = replace(@AddressDescription,'*','')  
    SELECT @AddressDescription = replace(@AddressDescription,'(','')  
    SELECT @AddressDescription = replace(@AddressDescription,')','')  
    SELECT @AddressDescription = replace(@AddressDescription,'-','')  
    SELECT @AddressDescription = replace(@AddressDescription,'+','')  
    SELECT @AddressDescription = replace(@AddressDescription,'=','')  
    SELECT @AddressDescription = replace(@AddressDescription,'[','')  
    SELECT @AddressDescription = replace(@AddressDescription,']','')  
    SELECT @AddressDescription = replace(@AddressDescription,':','')  
    SELECT @AddressDescription = replace(@AddressDescription,';','')  
    SELECT @AddressDescription = replace(@AddressDescription,'@','')  
    SELECT @AddressDescription = replace(@AddressDescription,'~','')  
    SELECT @AddressDescription = replace(@AddressDescription,'#','')  
    SELECT @AddressDescription = replace(@AddressDescription,'<','')  
    SELECT @AddressDescription = replace(@AddressDescription,'>','')  
    SELECT @AddressDescription = replace(@AddressDescription,',','')  
    SELECT @AddressDescription = replace(@AddressDescription,'.','')  
    SELECT @AddressDescription = replace(@AddressDescription,'?','')  
    SELECT @AddressDescription = replace(@AddressDescription,'/','')  
    SELECT @AddressDescription = replace(@AddressDescription,'\','')  
    SELECT @AddressDescription = replace(@AddressDescription,'|','')
    IF ISNUMERIC(SUBSTRING(@AddressDescription, 1, 1)) > 0	
	BEGIN	
		SELECT @AddressDescription = '_' + @AddressDescription
	END    
     --DC110304 PN10913 -end  
  
  IF len(@ssql) <=8000
    BEGIN  
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address1 VARCHAR(60) NULL, '  
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address2 VARCHAR(60) NULL, '  
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address3 VARCHAR(60) NULL, '  
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address4 VARCHAR(60) NULL, '  
		SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address5 VARCHAR(60),'
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address6 VARCHAR(60),'
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address7 VARCHAR(60),'
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address8 VARCHAR(60),'
		SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address9 VARCHAR(60),'
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Address10 VARCHAR(60),'
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Postcode VARCHAR(20) NULL, '  
	    SELECT @ssql = @ssql + replace(@AddressDescription,' ','_')+'_Country VARCHAR(60) NULL '  
	    
	    IF ((SUBSTRING(@AddressDescription, 1, 1)= '_') AND (ISNUMERIC(SUBSTRING(@AddressDescription, 2, 1)) > 0))
	         BEGIN  
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address1 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address1'','  
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address2 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address2'','  
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address3 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address3'','  
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address4 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address4'','  
			   SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address5 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address5'','
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address6 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address6'','
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address7 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address7'','
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address8 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address8'','
			   SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address9 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address9'','
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address10 AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Address10'','
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Postcode AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Postcode'','  
	           SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Country AS ''' + replace(SUBSTRING(@AddressDescription, 2, LEN(@AddressDescription)),' ','_')+'_Country '''  
     		 END  
	    	ELSE
	    	 BEGIN	
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address1, '  
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address2, '  
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address3, '  
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address4, '  
				 SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address5,'
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address6,'
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address7,'
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address8,'
				 SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address9,'
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Address10,'
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Postcode, '  
	    	     SELECT @ssql5 = @ssql5 + replace(@AddressDescription,' ','_')+'_Country '  		
		 END
	    
    END
 ELSE
    BEGIN
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address1 VARCHAR(60) NULL, '  
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address2 VARCHAR(60) NULL, '  
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address3 VARCHAR(60) NULL, '  
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address4 VARCHAR(60) NULL, '  
		SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address5 VARCHAR(60),'
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address6 VARCHAR(60),'
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address7 VARCHAR(60),'
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address8 VARCHAR(60),'
		SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address9 VARCHAR(60),'
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Address10 VARCHAR(60),'
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Postcode VARCHAR(20) NULL, '  
	    SELECT @ssql1 = @ssql1 + replace(@AddressDescription,' ','_')+'_Country VARCHAR(60) NULL '  
    END
    
    FETCH NEXT FROM cAddresses INTO @AddressDescription, @AddressCode
END  
SELECT @ssql = 'CREATE TABLE #tempaddresses (' + rtrim(@ssql)
SELECT @ssql1= rtrim(@ssql1) + ')'

SELECT @ssql3 = ''  
SELECT @ssql4 = ''  
FETCH FIRST FROM cAddresses INTO @AddressDescription, @AddressCode  
WHILE @@FETCH_STATUS = 0 BEGIN  
  
    --DC110304 PN10913 -start -to overcome problem with puntuation in address description and code  
    --    probably better way to do this but for now this will have to do  
    SELECT @AddressDescription = replace(@AddressDescription,'!','')  
    SELECT @AddressDescription = replace(@AddressDescription,'"','')  
    SELECT @AddressDescription = replace(@AddressDescription,'£','')  
    SELECT @AddressDescription = replace(@AddressDescription,'$','')  
    SELECT @AddressDescription = replace(@AddressDescription,'%','')  
    SELECT @AddressDescription = replace(@AddressDescription,'^','')  
    SELECT @AddressDescription = replace(@AddressDescription,'&','')  
    SELECT @AddressDescription = replace(@AddressDescription,'*','')  
    SELECT @AddressDescription = replace(@AddressDescription,'(','')  
    SELECT @AddressDescription = replace(@AddressDescription,')','')  
    SELECT @AddressDescription = replace(@AddressDescription,'-','')  
    SELECT @AddressDescription = replace(@AddressDescription,'+','')  
    SELECT @AddressDescription = replace(@AddressDescription,'=','')  
    SELECT @AddressDescription = replace(@AddressDescription,'[','')  
    SELECT @AddressDescription = replace(@AddressDescription,']','')  
    SELECT @AddressDescription = replace(@AddressDescription,':','')  
    SELECT @AddressDescription = replace(@AddressDescription,';','')  
    SELECT @AddressDescription = replace(@AddressDescription,'@','')  
    SELECT @AddressDescription = replace(@AddressDescription,'~','')  
    SELECT @AddressDescription = replace(@AddressDescription,'#','')  
    SELECT @AddressDescription = replace(@AddressDescription,'<','')  
    SELECT @AddressDescription = replace(@AddressDescription,'>','')  
    SELECT @AddressDescription = replace(@AddressDescription,',','')  
    SELECT @AddressDescription = replace(@AddressDescription,'.','')  
    SELECT @AddressDescription = replace(@AddressDescription,'?','')  
    SELECT @AddressDescription = replace(@AddressDescription,'/','')  
    SELECT @AddressDescription = replace(@AddressDescription,'\','')  
    SELECT @AddressDescription = replace(@AddressDescription,'|','')  
    IF ISNUMERIC(SUBSTRING(@AddressDescription, 1, 1)) > 0	
    	BEGIN	
    		SELECT @AddressDescription = '_' + @AddressDescription
	END
     --DC110304 PN10913 -end  
  
 /*Initialise variables before getting next address.*/  
 SELECT @Address1 = NULL  
 SELECT @Address2 = NULL  
 SELECT @Address3 = NULL  
 SELECT @Address4 = NULL  
 SELECT @Postcode = NULL  
 SELECT @Country = NULL  
  
 SELECT @Address5 = NULL
 SELECT @Address6 = NULL
 SELECT @Address7 = NULL
 SELECT @Address8 = NULL
 SELECT @Address9 = NULL
 SELECT @Address10 = NULL
  
    EXEC spu_wp_get_address @PartyCnt, @InsuranceFileCnt, @ClaimCnt,@AddressCode,@Address1 OUTPUT,@Address2 OUTPUT,@Address3 OUTPUT,@Address4 OUTPUT,@Postcode OUTPUT,@Country Output, @Address5 OUTPUT, @Address6 OUTPUT, @Address7 OUTPUT, @Address8 OUTPUT, @Address9 OUTPUT, @Address10 OUTPUT
    --AR20050418 - PN17878 Default in Correspondence address if blank  
    IF (UPPER(@AddressCode)<>@CorrespondenceCode) AND (@Address1 IS NULL) AND (@Address2 IS NULL) AND (@Address3 IS NULL) AND (@Address4 IS NULL) AND (@Postcode IS NULL) AND (@Country IS NULL)  
    BEGIN  
     EXEC spu_wp_get_address @PartyCnt,@InsuranceFileCnt,@ClaimCnt,@CorrespondenceCode,@Address1 OUTPUT,@Address2 OUTPUT,@Address3 OUTPUT,@Address4 OUTPUT,@Postcode OUTPUT,@Country Output, @Address5 OUTPUT, @Address6 OUTPUT, @Address7 OUTPUT, @Address8 OUTPUT, @Address9 OUTPUT, @Address10 OUTPUT
    END  
  

    SELECT @Address1 = isnull(@Address1,'')

    SELECT @Address2 = isnull(@Address2,'')

    SELECT @Address3 = isnull(@Address3,'')

    SELECT @Address4 = isnull(@Address4,'')

    SELECT @Postcode = isnull(@Postcode,'')

    SELECT @Country = isnull(@Country,'')

	SELECT @Address5 = isnull(@Address5,'')

    SELECT @Address6 = isnull(@Address6,'')

    SELECT @Address7 = isnull(@Address7,'')

    SELECT @Address8 = isnull(@Address8,'')

	SELECT @Address9 = isnull(@Address9,'')

    SELECT @Address10 = isnull(@Address10,'')
  
    IF len(LTRIM(RTRIM(@ssql3))) > 0 AND len(LTRIM(RTRIM(@ssql4)))=0  
    BEGIN  
        SELECT @ssql3 = @ssql3 + ', '  
    END  
  
    if len(LTRIM(RTRIM(@ssql4))) > 0  
    BEGIN  
        SELECT @ssql4 = @ssql4 + ', '  
    END  
  
    IF len(LTRIM(RTRIM(@ssql3))) <=8000
    BEGIN  
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address1 = ''' + replace(@Address1,'''','''''') + ''', '  
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address2 = ''' + replace(@Address2,'''','''''') + ''', '  
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address3 = ''' + replace(@Address3,'''','''''') + ''', '  
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address4 = ''' + replace(@Address4,'''','''''') + ''', '  
	 SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address5=''' + replace(@Address5,'''','''''') + ''','
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address6=''' + replace(@Address6,'''','''''') + ''','
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address7=''' + replace(@Address7,'''','''''') + ''','
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address8=''' + replace(@Address8,'''','''''') + ''','
	 SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address9=''' + replace(@Address9,'''','''''') + ''','
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Address10=''' + replace(@Address10,'''','''''') + ''','
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Postcode = ''' + replace(@Postcode,'''','''''') + ''', '  
     SELECT @ssql3 = @ssql3 + replace(@AddressDescription,' ','_')+'_Country = ''' + replace(@Country,'''','''''') + ''''  
    END  
    ELSE  
    BEGIN  
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address1 = ''' + replace(@Address1,'''','''''') + ''', '  
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address2 = ''' + replace(@Address2,'''','''''') + ''', '  
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address3 = ''' + replace(@Address3,'''','''''') + ''', '  
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address4 = ''' + replace(@Address4,'''','''''') + ''', '  
	 SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address5=''' + replace(@Address5,'''','''''') + ''','
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address6=''' + replace(@Address6,'''','''''') + ''','
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address7=''' + replace(@Address7,'''','''''') + ''','
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address8=''' + replace(@Address8,'''','''''') + ''','
	 SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address9=''' + replace(@Address9,'''','''''') + ''','
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Address10=''' + replace(@Address10,'''','''''') + ''','
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Postcode = ''' + replace(@Postcode,'''','''''') + ''', '  
     SELECT @ssql4 = @ssql4 + replace(@AddressDescription,' ','_')+'_Country = ''' + replace(@Country,'''','''''') + ''''  
    END  
FETCH NEXT FROM cAddresses INTO @AddressDescription, @AddressCode  
END  
SELECT @ssql3 = 'Update #tempaddresses set ' + @ssql3  
  
CLOSE cAddresses  
DEALLOCATE cAddresses  
  
--Execute the Main query  
EXEC (@ssql + @ssql1 + 'insert into #tempaddresses([address_id]) values(1)' + @ssql3 + @ssql4 + 'SELECT ' + @ssql5 + ' from #tempaddresses')
  
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE NAME = '#tempaddresses')  
    DROP TABLE #tempaddresses  
    
GO
