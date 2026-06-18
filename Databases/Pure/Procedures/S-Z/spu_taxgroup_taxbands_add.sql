SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_taxgroup_taxbands_add'
GO

CREATE PROCEDURE spu_taxgroup_taxbands_add
    @tax_group_id int,
    @tax_band_id int,
    @sequence smallint,
    @alloc_seq smallint,
    @alloc_rule smallint,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL
AS

SELECT @screenhierarchy=@screenhierarchy + ' / Tax Band(' + tb.description +')'
    FROM Tax_Band tb 
    WHERE tb.tax_band_id= @tax_band_id

    INSERT INTO Tax_group_tax_band (
        tax_group_id,
        tax_band_id,
        sequence,
        allocation_sequence,
        allocation_rule,
		userid,
	    uniqueid ,
	    screenhierarchy)
    VALUES (
        @tax_group_id,
        @tax_band_id,
        @sequence,
        @alloc_seq,
        @alloc_rule,
		@userid,
	    @uniqueid ,
	    @screenhierarchy)
GO
