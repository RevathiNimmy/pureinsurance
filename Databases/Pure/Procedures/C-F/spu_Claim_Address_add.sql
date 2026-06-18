SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Claim_Address_add'
GO


CREATE PROCEDURE spu_Claim_Address_add
    @address_cnt int OUTPUT,
    @source_id smallint,
    @address_id int,
    @address1 varchar(60),
    @address2 varchar(60),
    @address3 varchar(60),
    @address4 varchar(60),
    @postal_code varchar(20),
    @country_id smallint,
    @created_by_id smallint,
    @date_created datetime,
    @modified_by_id smallint,
    @last_modified datetime,
    @address_usage_type_id int,
    @update_address bit
AS


BEGIN
IF @source_id = 0
 SELECT @source_id = 1
IF @source_id = NULL
 SELECT @source_id = 1

SELECT @address_id=ISNULL(@address_id,0)

INSERT INTO Claim_Address (
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
 address_usage_type_id)
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
 @address_usage_type_id)
END

SELECT @address_cnt = @@IDENTITY

IF (@address_id<>0)
BEGIN
	IF(@update_address=1)
    	BEGIN
		UPDATE Address SET
		address1=@address1,
		address2=@address2,
		address3=@address3,
		address4=@address4,
		postal_code=@postal_code,
		country_id=@country_id,
		modified_by_id=@modified_by_id,
		last_modified=@last_modified
		WHERE
		address_cnt=@address_id
	END

	UPDATE Claim_Address SET
	address1=@address1,
	address2=@address2,
	address3=@address3,
	address4=@address4,
	postal_code=@postal_code,
	country_id=@country_id,
	modified_by_id=@modified_by_id,
	last_modified=@last_modified
	WHERE
	address_id=@address_id

END
GO


