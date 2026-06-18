SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Address_check'
GO


CREATE PROCEDURE spu_Address_check
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
    @last_modified datetime
AS


BEGIN
select  @address_cnt = address_cnt
from    address
where   isnull(address1, 'zzz') = isnull(@address1, 'zzz')
and isnull(address2, 'zzz') = isnull(@address2, 'zzz')
and isnull(address3, 'zzz') = isnull(@address3, 'zzz')
and isnull(address4, 'zzz') = isnull(@address4, 'zzz')
and (case postal_code
    when convert(varchar(20), address_id) then ''
    else isnull(postal_code, '')
    end) = isnull(@postal_code, '')
End
GO


