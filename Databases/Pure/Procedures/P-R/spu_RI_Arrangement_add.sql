SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_RI_Arrangement_add'
GO

CREATE PROCEDURE spu_RI_Arrangement_add
    @ri_arrangement_id int output,
    @risk_cnt int,
    @ri_band_id int,
    @ri_model_id int,
    @sum_insured money,
    @premium money,
    @original_flag tinyint,
    @is_modified tinyint
AS

    -- Insert record
    Insert  ri_arrangement (
	    risk_cnt,
	    ri_band_id,
	    ri_model_id,
	    sum_insured,
	    premium,
	    original_flag, 
	    is_modified)
    Values (@risk_cnt,
	    @ri_band_id,
	    @ri_model_id,
	    @sum_insured,
	    @premium,
	    @original_flag, 
	    @is_modified)

    -- Get new ID
    Select @ri_arrangement_id = @@Identity   

Go



