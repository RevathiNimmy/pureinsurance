SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Renewal_Premium_Sel'
GO


CREATE PROCEDURE spu_SirRen_Renewal_Premium_Sel
    @Insurance_file_cnt int,
    @Gis_Scheme_Id int,
    @Data_Model char(10)
AS


BEGIN
    If @Data_Model = 'Household'
    Begin
        Select qo.out_total_ann_premium, qo.out_total_ann_ipt
        From Qh_Quote_Out qo,
             GIIHQuote_Binder qb,
             GIS_Policy_Link gp
        Where qo.gis_policy_link_id = qb.gis_policy_link_id
        And qo.GIIHQuote_Binder_Id = qb.GIIHQuote_Binder_Id
        And qb.gis_scheme_id = @gis_Scheme_id
        And qb.gis_policy_link_id = gp.gis_policy_link_id
        And gp.insurance_file_cnt = @insurance_file_cnt
    End

    If @Data_Model = 'Motor'
    Begin
        Select qo.premium, qo.ipt
        From GIIMQuick_Quote_Result qo,
             Quote_Binder qb,
             GIS_Policy_Link gp
        Where qo.gis_policy_link_id = qb.gis_policy_link_id
        And qo.Quote_Binder_Id = qb.Quote_Binder_Id
        And qb.gis_scheme_id = @gis_Scheme_id
        And qb.gis_policy_link_id = gp.gis_policy_link_id
        And gp.insurance_file_cnt = @insurance_file_cnt
    End
END
GO


