SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Risk_Links'
GO

CREATE  PROCEDURE spu_SAM_CLM_Get_Claim_Risk_Links  
  
@risk_cnt int,  
@links_option int  

AS

IF @links_option = 1

                 SELECT
	               pt.code as peril_type_code,
                       pt.description as peril_type_description,
                       SUM(p.sum_insured) as sum_insured
                 FROM peril p
                 INNER JOIN rating_section rs ON
                       (p.rating_section_id=rs.rating_section_id and p.risk_cnt=rs.risk_cnt)
                 INNER JOIN peril_type pt ON
	               pt.peril_type_id = p.peril_type_id
                 WHERE p.risk_cnt = @risk_cnt and rs.original_flag=0
                 GROUP BY pt.code, pt.description
                 order by pt.code

ELSE IF @links_option = 2

                 SELECT
	               pt.code as peril_type_code,
	               rt.name  as reserve_type_code,
	               rt.description
                 FROM peril p
	         INNER JOIN peril_type pt 
                       ON pt.peril_type_id = p.peril_type_id
               	 INNER JOIN peril_type_reserve_type ptrt
                       ON ptrt.peril_type_id = pt.peril_type_id
                 INNER JOIN reserve_type rt 
                       ON rt.reserve_type_id = ptrt.reserve_type_id
                 WHERE p.risk_cnt = @risk_cnt
                 GROUP BY pt.code, rt.name, rt.description
                 order by pt.code

ElSE IF @links_option = 3

                 SELECT
                       rt.Code as recovery_type_code,
	               rt.description,
                 --Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc) - (5.1.1)
                       rt.is_salvage
                 --End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc) - (5.1.1)

                 FROM recovery_type rt
                 --Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc) - (5.1.1)
                 -- WHERE is_salvage =0
                 --End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR25 - MaintainClaim - Salvage Recovery.doc) - (5.1.1)

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
