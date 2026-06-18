/* Select party_variant_address records for a given party (party_cnt) */
if exists (select * from sysobjects where id = object_id(N'dbo.spu_party_variant_address_selall') 
and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    drop procedure dbo.spu_party_variant_address_selall
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spu_party_variant_address_selall

AS

SELECT  party_variant_address_cnt
	party_cnt,
	address_cnt,
	original_address_cnt,
	effective_date,
	date_created,
	commit_ind
FROM party_variant_address
WHERE commit_ind = 0 
ORDER BY party_cnt, address_cnt

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
