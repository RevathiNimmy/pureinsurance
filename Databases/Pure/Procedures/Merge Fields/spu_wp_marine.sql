SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_marine'
GO


CREATE PROCEDURE spu_wp_marine
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @vessel_name VARCHAR(60) OUTPUT,
    @model VARCHAR(255),
    @category VARCHAR(255),
    @engine_type VARCHAR(255),
    @fuel_type VARCHAR(255),
    @engine_bhp INT OUTPUT,
    @length INT OUTPUT,
    @speed INT OUTPUT,
    @hull_material varchar( 70 ) OUTPUT,
    @year_built INT OUTPUT,
    @when_purchased DATETIME OUTPUT,
    @cost NUMERIC,
    @current_value NUMERIC,
    @usage VARCHAR(255),
    @cruising_range INT OUTPUT,
    @is_moored TINYINT OUTPUT,
    @where_moored VARCHAR(255),
    @months_in_commission INT OUTPUT,
    @laid_up_from DATETIME OUTPUT,
    @laid_up_to DATETIME OUTPUT,
    @is_ashore TINYINT OUTPUT
AS


SELECT
    @vessel_name=marine.vessel_name,
    @model=marine.model,
    @category=marine.category,
    @engine_type=marine.engine_type,
    @fuel_type=marine.fuel_type,
    @engine_bhp=marine.engine_bhp,
    @length=marine.length,
    @speed=marine.speed,
    @hull_material=marine.hull_material,
    @year_built=marine.year_built,
    @when_purchased=marine.when_purchased,
    @cost=marine.cost,
    @current_value=marine.current_value,
    @usage=marine.usage,
    @cruising_range=marine.cruising_range,
    @is_moored=marine.is_moored,
    @where_moored=marine.where_moored,
    @months_in_commission=marine.months_in_commission,
    @laid_up_from=marine.laid_up_from,
    @laid_up_to=marine.laid_up_to,
    @is_ashore=marine.is_ashore
FROM marine
WHERE marine.insurance_file_cnt = @insurancefilecnt
GO


