/* Select party_variant_address records for a given party (party_cnt) */
if exists (select * from sysobjects where id = object_id(N'dbo.spu_party_variant_address_upd') 
and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    drop procedure dbo.spu_party_variant_address_upd
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spu_party_variant_address_upd

@party_cnt int

AS

UPDATE party_variant_address
SET commit_ind = 1
WHERE party_cnt = @party_cnt


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
