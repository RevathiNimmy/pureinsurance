SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Address_add'
GO

CREATE PROCEDURE spe_Address_add
    @address_cnt int OUTPUT ,
    @source_id smallint ,
    @address_id int ,
    @address1 varchar(60) ,
    @address2 varchar(60) ,
    @address3 varchar(60) ,
    @address4 varchar(60) ,
    @postal_code varchar(20) ,
    @country_id smallint ,
    @created_by_id smallint ,
    @date_created datetime ,
    @modified_by_id smallint ,
    @last_modified datetime,
    @ExternalId int,
	@Address5 varchar(60) = NULL,
	@Address6 varchar(60) = NULL,
	@Address7 varchar(60) = NULL,
	@Address8 varchar(60) = NULL,
	@Address9 varchar(60) = NULL,
	@Address10 varchar(60) = NULL,
	@addresskey int = 0,  
    @UserId int = NULL,  
    @UniqueId VARCHAR(50) = NULL,  
    @ScreenHeirarchy VARCHAR(500) = NULL   
AS
BEGIN
IF ISNULL(@source_id,0) = 0 
                SELECT @source_id = 1

	IF (@addresskey = 0) AND (IsNull(@address_cnt,0)=0 OR @address_cnt = 0)
	  BEGIN	 
INSERT INTO Address (
    source_id,
    address_id,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    country_id,
    created_by_id,
    date_created,
    modified_by_id,
    last_modified,
    ExternalId,
	address5,
	address6,
	address7,
	address8,
	address9,
	address10,  
    UserId,  
    UniqueId,  
    ScreenHierarchy)
VALUES (
    @source_id,
    @address_id,
    @address1,
    @address2,
    @address3,
    @address4,
    @postal_code,
    @country_id,
    @created_by_id,
    @date_created,
    @modified_by_id,
    @last_modified,
    @ExternalId,
	@Address5,
	@Address6,
	@Address7,
	@Address8,
	@Address9,
	@Address10,  
    @UserId,  
    @UniqueId,  
    @ScreenHeirarchy 
    )

    SELECT @address_cnt = SCOPE_IDENTITY()
END
ELSE
BEGIN
 
    DECLARE @sAddressKey INT
    SET @sAddressKey =  CASE WHEN  @addresskey = 0 THEN @address_cnt ELSE @addresskey END
    UPDATE Address SET source_id = @source_id ,  		
			address1 = @address1,  
			address2 = @address2,  
			address3 = @address3,  
			address4 = @address4,
			postal_code = @postal_code,  
			country_id = @country_id,  
			created_by_id = @created_by_id,  
			date_created = @date_created,  
			modified_by_id = @modified_by_id,   
			last_modified = @last_modified,  
			ExternalId = @ExternalId,
			address5=@Address5,
			address6=@Address6,
			address7=@Address7,
			address8=@Address8,
			address9=@Address9,
			address10=@Address10,  
            UserId = @UserId,  
            UniqueId = @UniqueId,  
            ScreenHierarchy = @ScreenHeirarchy
	WHERE address_cnt =@sAddressKey
						 
			SELECT @address_cnt = @sAddressKey
END


END

GO
