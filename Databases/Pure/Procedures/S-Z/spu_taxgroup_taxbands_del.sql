SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_taxgroup_taxbands_del'
GO

CREATE PROCEDURE spu_taxgroup_taxbands_del
    @tax_group_id INT,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL
AS

UPDATE tgtb  SET 
        userid=@UserId,
        uniqueid=@uniqueid,
		screenhierarchy=@screenhierarchy + + ' / Tax Band(' + tb.description +')'
    FROM Tax_group_tax_band tgtb INNER JOIN Tax_Band tb ON tb.tax_band_id=tgtb.tax_band_id
    WHERE
	    tax_group_id = @tax_group_id

    DELETE FROM 
    	Tax_group_tax_band
    WHERE
	    tax_group_id = @tax_group_id

GO


