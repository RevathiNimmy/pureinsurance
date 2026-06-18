SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Policy_Client_selall'
GO

CREATE PROCEDURE spu_Policy_Client_selall  
    @insurance_folder_cnt int,  
	@insurance_file_cnt int = 0,
    @party_cnt int  
AS  
  
    -- Keep our own count so we don't suffer from  
    -- sql resetting it.  
    DECLARE @rowcount int  

    DECLARE @return_party_cnt int 
    DECLARE @is_lead tinyint
    DECLARE @correspondence tinyint
    DECLARE @shortname varchar(20)
    DECLARE @resolvedname varchar(255)
    DECLARE @addressline1 varchar(60)

    -- Get the policy->client links  
	If (@insurance_file_cnt=0)
	   set @resolvedname=(select resolved_name from party where party_cnt= @party_cnt )
	else
	   set @resolvedname=(Select Insured_name from insurance_file where insurance_file_cnt  = @insurance_file_cnt)
    SELECT  
       @return_party_cnt = PC.party_cnt,  
       @is_lead = PC.is_lead,  
       @correspondence =  PC.correspondence,  
       @shortname = P.shortname,  
       @addressline1 = A.address1  
    FROM  
        Policy_Client PC  
    INNER JOIN  
        Party P  
        ON PC.party_cnt = P.party_cnt  
    INNER JOIN  
        Party_Address_Usage PAU  
        ON P.party_cnt = PAU.party_cnt  
    INNER JOIN  
        Address A  
        ON PAU.address_cnt = A.address_cnt AND PAU.address_usage_type_id = 4  
    WHERE  
        PC.insurance_folder_cnt = @insurance_folder_cnt  
  

    -- Store the affected rows  
    SELECT @rowcount = @@ROWCOUNT  
  
    -- If we have no links return a default one from the holder_cnt on the insurance_folder  
    -- dPMDAO should concatenate the results nicely so the vb layers won't know the difference  
    IF @rowcount = 0  
    BEGIN  
        SELECT  
           @return_party_cnt =  I.insurance_holder_cnt,  
           @is_lead =  1,  
           @correspondence =  1,  
           @shortname = P.shortname,  
           @addressline1 =  A.address1  
        FROM  
            Insurance_Folder I  
        INNER JOIN  
            Party P  
            ON I.insurance_holder_cnt = P.party_cnt  
        INNER JOIN  
            Party_Address_Usage PAU  
            ON P.party_cnt = PAU.party_cnt  
        INNER JOIN  
            Address A  
            ON PAU.address_cnt = A.address_cnt AND PAU.address_usage_type_id = 4  
        WHERE  
            I.insurance_folder_cnt = @insurance_folder_cnt  
  
        -- Update the affected rows  
        SELECT @rowcount = @@ROWCOUNT  
    END  
  
    -- If we STILL have no links return the supplied party_cnt's details  
    -- dPMDAO should concatenate the results nicely so the vb layers won't know the difference  
    IF @rowcount = 0  
    BEGIN  
        SELECT  
            @return_party_cnt = P.party_cnt,  
            @is_lead = 1,  
            @correspondence = 1,  
            @shortname = P.shortname,  
            @addressline1 = A.address1  
        FROM  
            Party P  
        INNER JOIN  
            Party_Address_Usage PAU  
            ON P.party_cnt = PAU.party_cnt  
        INNER JOIN  
            Address A  
            ON PAU.address_cnt = A.address_cnt AND PAU.address_usage_type_id = 4  
        WHERE  
            P.party_cnt = @party_cnt  
    END  

	--If Insurance_file_cnt is supplied
	If (@insurance_file_cnt<>0)	  
	   set @resolvedname=(Select Insured_name from insurance_file where insurance_file_cnt  = @insurance_file_cnt)

    SELECT  @return_party_cnt, 
            @is_lead,
            @correspondence,
            @shortname,
            @resolvedname,
            @addressline1


GO
