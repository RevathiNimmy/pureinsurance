EXECUTE DDLDropProcedure spu_Get_gis_policy_link_RiskType
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_gis_policy_link_RiskType
    @gis_policy_link_id int,
    @transaction_type varchar(50) = NULL
AS

IF @transaction_type = 'C_CO'
  BEGIN
    SELECT rt.risk_type_id,
           rt.code,
           rt.description
    FROM   gis_policy_link gpl
           INNER JOIN claim clm
             ON gpl.claim_id = clm.claim_id
           INNER JOIN risk r
             ON clm.risk_type_id = r.risk_cnt
           INNER JOIN risk_type rt
             ON r.risk_type_id = rt.risk_type_id
    WHERE  gpl.gis_policy_link_id = @gis_policy_link_id
  END
ELSE
  BEGIN
    SELECT rt.risk_type_id,
           rt.code,
           rt.description
    FROM   gis_policy_link gpl
           INNER JOIN risk r
             ON gpl.risk_id = r.risk_cnt
           INNER JOIN risk_type rt
             ON r.risk_type_id = rt.risk_type_id
    WHERE  gpl.gis_policy_link_id = @gis_policy_link_id
  END

GO
