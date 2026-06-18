SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Add_CurrencyRate'
GO

CREATE PROCEDURE spu_ACT_Add_CurrencyRate
    @effective_from datetime,
    @rate_against_base numeric(19,10),
    @currency_id smallint,
    @company_id smallint,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)
AS 
DECLARE @TypeOfRates TINYINT

    EXEC SPU_ACT_GETTYPEOFRATES
      @TypeOfRates OUTPUT

    IF @TypeOfRates = 1
      BEGIN

INSERT INTO CurrencyRate
(
    effective_from,
    rate_against_base,
    currency_id,
    company_id,
	UserId,
	UniqueId,
	ScreenHierarchy
)
 SELECT @effective_from,
                 @rate_against_base,
                 @currency_id,
                 s.source_id,
				 @user_id,
				 @unique_id,
				 @screen_hierarchy
          FROM   Source s
          WHERE  is_deleted = 0
      END
ELSE
      BEGIN
          INSERT INTO CurrencyRate
                      (effective_from,
                       rate_against_base,
                       currency_id,
                       company_id,
					   UserId,
					   UniqueId,
					   ScreenHierarchy)
VALUES
(
    @effective_from,
    @rate_against_base,
    @currency_id,
    @company_id,
    @user_id,
	@unique_id,
	@screen_hierarchy
)
 END 

GO

