SET QUOTED_IDENTIFIER ON
GO
Execute DDLDropProcedure 'spu_GetPolicyDetail'
GO
/*******************************************************************************************************/
/* spu_GetPolicyDetail    */                                                                              
/*******************************************************************************************************/
CREATE PROCEDURE spu_GetPolicyDetail
    @gis_policy_link_id INT
AS
SELECT ifrl.insurance_file_cnt, i.insured_cnt,i.insurance_folder_cnt
                           FROM gis_policy_link gpl,
                                insurance_file i,
                                insurance_file_risk_link ifrl
                          WHERE gpl.gis_policy_link_id = @gis_policy_link_id
                                AND gpl.risk_id = ifrl.risk_cnt
                                AND i.insurance_file_cnt = ifrl.insurance_file_cnt
SET QUOTED_IDENTIFIER OFF
GO

