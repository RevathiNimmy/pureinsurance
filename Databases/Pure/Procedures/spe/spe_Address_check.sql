SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Address_check'
GO

CREATE PROCEDURE spe_Address_check
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
    @ExternalId int
AS
BEGIN
select @address_cnt = address_cnt
    from address
    where
        address1=@address1 and
        address2=@address2 and
            address3=@address3 and
            address4=@address4 and
        postal_code=@postal_code and
        externalId=@ExternalId
End

GO

