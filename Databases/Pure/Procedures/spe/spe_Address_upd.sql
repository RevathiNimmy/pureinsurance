SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Address_upd'
GO
CREATE PROCEDURE spe_Address_upd
    @address_cnt int,  
    @source_id smallint,  
    @address_id int,  
    @address1 varchar(60),  
    @address2 varchar(60),  
    @address3 varchar(60),  
    @address4 varchar(60),  
    @postal_code varchar(20),  
    @country_id smallint,  
    @created_by_id smallint= NULL ,  
    @date_created datetime= NULL ,  
    @modified_by_id smallint = NULL,  
    @last_modified datetime = NULL,  
    @ExternalId int = NULL,
	@Address5 varchar(60) = NULL,
	@Address6 varchar(60) = NULL,
	@Address7 varchar(60) = NULL,
	@Address8 varchar(60) = NULL,
	@Address9 varchar(60) = NULL,
	@Address10 varchar(60) = NULL,
	@addresskey int = 0,  
    @UserId int,  
    @UniqueId VARCHAR(50),  
    @ScreenHeirarchy VARCHAR(500)
	  
AS  
BEGIN  
 
UPDATE Address  
    SET  
    source_id=@source_id,  
    address_id=@address_id,  
    address1=@address1,  
    address2=@address2,  
    address3=@address3,  
    address4=@address4,  
    postal_code=@postal_code,  
    country_id=@country_id,  
    created_by_id=ISNULL(@created_by_id,created_by_id),  
    date_created=ISNULL(@date_created,date_created),  
    modified_by_id=ISNULL(@modified_by_id,modified_by_id),  
    last_modified=ISNULL(@last_modified,last_modified),  
    ExternalId=ISNULL(@ExternalId,ExternalId) ,
	address5 = @Address5,
    address6 = @Address6,
    address7 = @Address7,
    address8 = @Address8,
    address9 = @Address9,
    address10 = @Address10,  
    UserId = @UserId,  
    UniqueId = @UniqueId,  
    ScreenHierarchy = @ScreenHeirarchy
WHERE address_cnt = @address_cnt  
  
END  
GO
