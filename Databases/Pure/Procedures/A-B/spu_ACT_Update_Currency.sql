SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Currency'
GO


CREATE PROCEDURE spu_ACT_Update_Currency
    @currency_id smallint,
    @caption_id int,
    @iso_code char(4),
    @description varchar(255),
    @minor_part varchar(30),
    @code char(10),
    @symbol char(4),
    @alignment char(1),
    @decimal_places tinyint,
    @is_deleted tinyint,
    @effective_date datetime,
    @format_string varchar(255),
    @round_to_places tinyint
AS


BEGIN
UPDATE Currency
    SET
    caption_id=@caption_id,
    iso_code=@iso_code,
    description=@description,
    minor_part=@minor_part,
    code=@code,
    symbol=@symbol,
    alignment=@alignment,
    decimal_places=@decimal_places,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    format_string=@format_string,
    round_to_places=@round_to_places
WHERE currency_id = @currency_id
END
GO


