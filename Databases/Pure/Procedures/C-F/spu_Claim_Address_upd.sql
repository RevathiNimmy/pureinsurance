SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Claim_Address_upd'
GO


CREATE PROCEDURE spu_Claim_Address_upd
    @address_cnt int,
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

UPDATE Claim_Address
 SET
 source_id=@source_id,
 address_id=@address_id,
 address1=@address1,
 address2=@address2,
 address3=@address3,
 address4=@address4,
 postal_code=@postal_code,
 country_id=@country_id,
 created_by_id=@created_by_id,
 date_created=@date_created,
 modified_by_id=@modified_by_id,
 last_modified=@last_modified,
 address_usage_type_id=@address_usage_type_id
WHERE address_cnt = @address_cnt

GO


