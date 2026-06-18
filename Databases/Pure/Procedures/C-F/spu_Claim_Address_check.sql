SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Claim_Address_check'
GO


CREATE PROCEDURE spu_Claim_Address_check
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
select @address_cnt = address_cnt
    from Claim_address
    where
        address1=@address1 and
        address2=@address2 and
        address3=@address3 and
        address4=@address4 and
        postal_code=@postal_code
End
GO


