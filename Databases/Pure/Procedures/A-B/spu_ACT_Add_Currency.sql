SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Currency'
GO


CREATE PROCEDURE spu_ACT_Add_Currency
    @currency_id smallint OUTPUT,
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


IF (@currency_id = 0) OR (@currency_id = NULL)
BEGIN
    SELECT @currency_id = MAX(currency_id) + 1 FROM Currency
    IF (@currency_id = NULL) SELECT @currency_id = 1
END
BEGIN
INSERT INTO Currency (
    currency_id ,
    caption_id ,
    iso_code ,
    description ,
    minor_part ,
    code ,
    symbol ,
    alignment ,
    decimal_places ,
    is_deleted ,
    effective_date ,
    format_string ,
    round_to_places )
VALUES (
    @currency_id,
    @caption_id,
    @iso_code,
    @description,
    @minor_part,
    @code,
    @symbol,
    @alignment,
    @decimal_places,
    @is_deleted,
    @effective_date,
    @format_string,
    @round_to_places)
END
GO


