SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_partytypes_for_risk'
GO


CREATE PROCEDURE spu_get_partytypes_for_risk
    @RiskTypeID int
AS


SELECT  rdd.Claim_Party_Type_ID,
        rdd.Mandatory,
        rdd.read_only,
        cpt.code
    from    Risk_Data_Definition rdd,
        claim_party_type cpt
    where   rdd.Risk_type_id = @RiskTypeId
    and rdd.Type = 6
    and rdd.claim_party_type_id = cpt.claim_party_type_id
    order by rdd.Display_Order
GO


