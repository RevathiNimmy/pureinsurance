SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_clm_for_resv_type'
GO


CREATE PROCEDURE spu_get_clm_for_resv_type
    @Reserve_type_id int
AS


--SELECT Peril.Claim_id
--FROM Peril INNER JOIN
 -- Reserve ON Peril.Peril_id = Reserve.Peril_id
--WHERE (Reserve.Reserve_type_id = @Reserve_type_id)
--SELECT claim_peril.Claim_id, Reserve_type.Name
--FROM claim_peril,Reserve_type, Reserve
--WHERE (Reserve.Reserve_type_id = @Reserve_type_id)
--AND claim_Peril.claim_peril_id = Reserve.claim_peril_id
--AND Reserve.Reserve_type_id = Reserve_type.Reserve_type_id
SELECT peril_type_id FROM peril_type_reserve_type
WHERE Reserve_type_id=@Reserve_type_id
GO


