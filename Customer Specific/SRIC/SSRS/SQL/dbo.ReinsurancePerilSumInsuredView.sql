CREATE VIEW [dbo].[ReinsurancePerilSumInsuredView]
AS

SELECT    dbo.Insurance_File.insurance_file_cnt,
          dbo.Peril.risk_cnt,
		  dbo.Rating_Section.rating_section_type_id,
          dbo.Peril.peril_type_id,
          dbo.Peril_Type.code                       AS peril_type_code,
          dbo.Peril_Type.description				AS peril_type_description,
          dbo.Peril.annual_premium,
          dbo.Peril.this_premium,
          dbo.Peril.sum_insured                     AS ri_sum_insured,
          dbo.Peril.rating_sum_insured,
          dbo.Peril.is_premium,
          dbo.Peril.is_sum_insured,
          CASE 
          WHEN dbo.Peril_Type.Code = 'ECPFIRE' THEN CAST(ISNULL(dbo.ECPGF_FIRE.FIRE_TOTAL_SI_AMT,0) / dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
          WHEN dbo.Peril_Type.Code = 'ECPBUILDC' THEN CAST(ISNULL(dbo.ECPGF_FIRE.BC_TOTAL_SI_AMT,0) /   dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
          WHEN dbo.Peril_Type.Code = 'ECPOFFCONT' THEN CAST(ISNULL(dbo.ECPGF_FIRE.OC_TOTAL_SI_AMT,0) /   dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
          WHEN dbo.Peril_Type.Code = 'ECPBUSINT ' THEN CAST(ISNULL(dbo.ECPGF_FIRE.BI_TOTAL_SI_AMT,0) /   dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
          WHEN dbo.Peril_Type.Code ='ECPACCREC ' THEN CAST(ISNULL(dbo.ECPGF_FIRE.AR_TOTAL_SI_AMT,0) /   dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
          WHEN dbo.Peril_Type.Code = 'ECPFGLASS ' THEN CAST(ISNULL(dbo.ECPGF_FIRE.GLASS_TOTAL_SI_AMT,0) /dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFC1' or dbo.Peril_Type.code = 'HBPFC1' and rating_section_type_id = 263 Then CAST(ISNULL(GF.COL1_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code ='EBPFC2' or dbo.Peril_Type.code = 'HBPFC2' Then CAST(ISNULL(GF.col2_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFC3'or dbo.Peril_Type.code = 'HBPFC3' Then  CAST(ISNULL(GF.col3_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFC4' or dbo.Peril_Type.code = 'HBPFC4'Then CAST(ISNULL(GF.col4_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY) 
		  When dbo.Peril_Type.Code = 'EBPFC5' or dbo.Peril_Type.code = 'HBPFC5' Then CAST(ISNULL(GF.COL5_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY) 
		  When dbo.Peril_Type.Code = 'EBPFC7' or dbo.Peril_Type.code = 'HBPFC7' Then CAST(ISNULL(GF.col7_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY) 
	      When rating_section_type_id in (269,1186)  Then CAST(ISNULL(GF.BUILDINGS_ESC_SUM_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When rating_section_type_id in (270,1187) Then CAST(ISNULL(GF.BUILDINGS_INFL_1YR_SUM_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When rating_section_type_id in (271,1188) Then CAST(ISNULL(GF.BUILDINGS_INFL_2YR_SUM_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFC3ESC'  Then CAST(ISNULL(GF.PLANT_MACH_ESC_SUM_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFC3IY1'  Then CAST(ISNULL(GF.PLANT_MACH_INFL_1YR_SUM_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFC3IY2'  Then CAST(ISNULL(GF.PLANT_MACH_INFL_2YR_SUM_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPFXACPC ' or dbo.Peril_Type.code = 'HBPFXACPC' Then CAST(ISNULL(GFE.LIMIT_OF_INDEMNITY,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBIGPB' or dbo.Peril_Type.code = 'HBPBIGPB' Then CAST(ISNULL(BI.GROSS_PROFIT_BASIS_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBIGR' or dbo.Peril_Type.code = 'HBPBIGR'  Then CAST(ISNULL(Bi.GROSS_RENTALS_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBIREV' or dbo.Peril_Type.code = 'HBPBIREV' Then CAST(ISNULL(bi.REVENUE_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBIAICOW' or dbo.Peril_Type.code = 'HBPBIAICOW' Then CAST(ISNULL(BI.ICOW_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBIW' or dbo.Peril_Type.code = 'HBPBIW'Then CAST(ISNULL(bi.WAGES_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code ='EBPBIDW' Then CAST(ISNULL(bi.DUAL_WAGE_WEEKS_INSURED,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBIXACPC' or dbo.Peril_Type.code = 'HBPBIXACPC'  Then CAST(ISNULL(GFBI.LIMIT_OF_INDEMNITY,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.code = 'EBPBIXFP' or dbo.Peril_Type.code = 'HBPBIXFP' Then CAST(ISNULL(GFBI.LIMIT_OF_INDEMNITY,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBCBC' or dbo.Peril_Type.code = 'HBPBCBC' AND rating_section_type_id IN (1152,315) Then CAST(ISNULL(bc.BC_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBCMI' or dbo.Peril_Type.code = 'HBPBCMI' Then CAST(ISNULL(bc.MISC_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBCAR' or dbo.Peril_Type.code = 'HBPBCAR' Then CAST(ISNULL(bc.ADD_Rent_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When rating_section_type_id in (1153,316) Then CAST(ISNULL(BC.ESC_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When rating_section_type_id in (1155,317) Then CAST(ISNULL(bc.INFL_1YR_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When rating_section_type_id in (1156,318) Then CAST(ISNULL(bc.INFL_2YR_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPBCXACPC' or dbo.Peril_Type.code = 'HBPBCXACPC' Then CAST(ISNULL(GFBC.LIMIT_OF_INDEMNITY,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPOCOC' or dbo.Peril_Type.code = 'HBPOCOC'Then CAST(ISNULL(oc.OC_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPOCLOD' or dbo.Peril_Type.code = 'HBPOCLOD'Then CAST(ISNULL(oc.LOD_SI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPOCLD' or dbo.Peril_Type.code = 'HBPOCLD'  Then CAST(ISNULL(OC.LIABDSI,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.Code = 'EBPOCXACPC' or dbo.Peril_Type.code = 'HBPOCXACPC' Then CAST(ISNULL(GFOC.LIMIT_OF_INDEMNITY,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
		  When dbo.Peril_Type.code = 'EBPOCXICOW' or dbo.Peril_Type.code = 'HBPOCXICOW' Then CAST(ISNULL(GFOC.LIMIT_OF_INDEMNITY,0)/ dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY)
          ELSE dbo.Peril.sum_insured
          END                                                                          AS sum_insured,		
          dbo.Peril.ri_band,
          AccountingView.total_ri_premium,
          AccountingView.total_ri_sum_insured,
          NULL AS Section,--dbo.SectionType.Name                                         AS Section,
          NULL AS SectionTypeId, --dbo.SectionType.SectionTypeId                         AS SectionTypeId,
          ROUND(CAST(dbo.Peril.annual_premium * dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY),2) AS annual_premium_inclusive,
          ROUND(CAST(dbo.Peril.this_premium * dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY),2) AS this_premium_inclusive ,
          [OP].this_premium AS this_premium_original,
          ROUND(CAST([OP].this_premium * dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY),2) AS this_premium_original_inclusive,
          dbo.Peril.this_premium + (ISNULL([OP].this_premium,0)) AS transaction_premium,
          ROUND(CAST(dbo.Peril.this_premium  * dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY),2) +
                ROUND(CAST(ISNULL([OP].this_premium,0) * dbo.GetTaxFactor(dbo.Peril_Type.tax_group,dbo.Insurance_File.cover_start_date) AS MONEY),2) 
                AS transaction_premium_inclusive

      FROM  dbo.Peril
      INNER JOIN  dbo.Peril_Type                          ON dbo.Peril.peril_type_id = dbo.Peril_Type.peril_type_id
      INNER JOIN  dbo.Rating_Section                      ON dbo.Rating_Section.rating_section_id = dbo.Peril.rating_section_id AND dbo.Rating_Section.risk_cnt = dbo.Peril.risk_cnt
      INNER JOIN  dbo.insurance_file_risk_link			  ON dbo.insurance_file_risk_link.risk_cnt = dbo.Peril.risk_cnt
      INNER JOIN  dbo.Insurance_File                      ON dbo.Insurance_File.insurance_file_cnt = dbo.insurance_file_risk_link.insurance_file_cnt
      LEFT  JOIN  dbo.GIS_Policy_Link                     ON dbo.Peril.risk_cnt = dbo.GIS_Policy_Link.risk_id
      LEFT  JOIN  dbo.ECPGF_Policy_Binder				  ON dbo.GIS_Policy_Link.gis_policy_link_id = dbo.ECPGF_Policy_Binder.ECPGF_Policy_Binder_id
      LEFT  JOIN  dbo.ECPGF_FIRE                          ON dbo.ECPGF_Policy_Binder.ECPGF_Policy_Binder_id = dbo.ECPGF_FIRE.ECPGF_Policy_Binder_id
	  Left Join	dbo.GROUPFIRE2_Policy_Binder			  On dbo.GIS_Policy_Link.gis_policy_link_id = dbo.GROUPFIRE2_Policy_Binder.GROUPFIRE2_Policy_binder_id
	  LEFT  JOIN  [dbo].[GROUPFIRE2_FIRE]  GF             ON dbo.GROUPFIRE2_Policy_Binder.GROUPFIRE2_Policy_binder_id = gf.GROUPFIRE2_Policy_binder_id
	  Left Join 
	  (Select *
	   From GROUPFIRE2_FIRE_EXTENSION 
	   Where Extension_type = 25) GFE				      ON GFE.GROUPFIRE2_Policy_binder_id = GF.GROUPFIRE2_Policy_binder_id
	  Left Join   [dbo].[GROUPFIRE2_BI] BI				  ON dbo.GROUPFIRE2_Policy_Binder.GROUPFIRE2_Policy_binder_id = BI.GROUPFIRE2_Policy_binder_id
	  Left Join
	  (Select *
	  From [dbo].[GROUPFIRE2_EXTENSION]
	  Where EXTENSION_TYPE = 25) GFBI			  ON dbo.GROUPFIRE2_Policy_Binder.GROUPFIRE2_Policy_binder_id = GFBI.GROUPFIRE2_Policy_binder_id		  
	  Left Join	[dbo].[GROUPFIRE2_BC] BC				  ON dbo.GROUPFIRE2_Policy_Binder.GROUPFIRE2_Policy_binder_id = bc.GROUPFIRE2_Policy_binder_id
	  Left Join
	  (Select *
	  From [dbo].[GROUPFIRE2_BC_EXTENSION]
	  Where Extension_type = 25 OR EXTENSION_TYPE = 107) GFBC					  ON GFBC.GROUPFIRE2_Policy_binder_id = BC.GROUPFIRE2_Policy_binder_id

	  Left Join [dbo].[GROUPFIRE2_OC] OC				  ON dbo.GROUPFIRE2_Policy_Binder.gis_policy_link_id = OC.GROUPFIRE2_Policy_binder_id
	  Left Join
	  (Select *
	  From [dbo].[GROUPFIRE2_OC_EXTENSION]
	  Where Extension_type = 25 OR EXTENSION_TYPE = 118) GFOC			  ON GFOC.GROUPFIRE2_Policy_binder_id = OC.GROUPFIRE2_Policy_binder_id

      LEFT  JOIN  (SELECT dbo.Stats_Detail.risk_id,
                        dbo.Stats_Detail.peril_type_id,
                        -1 * SUM(dbo.Stats_Detail.this_premium_original) AS total_ri_premium,
                        -1 * SUM(dbo.Stats_Detail.sum_insured_home)           AS total_ri_sum_insured
                  FROM dbo.Stats_Detail
                  WHERE dbo.Stats_Detail.stats_detail_type IN ('FAC','FAX')
                  GROUP BY dbo.Stats_Detail.risk_id, dbo.Stats_Detail.peril_type_id) AS AccountingView ON AccountingView.risk_id = dbo.Peril.risk_cnt AND AccountingView.peril_type_id = dbo.Peril.peril_type_id
      --LEFT JOIN dbo.SectionTypePerilMap             ON dbo.SectionTypePerilMap.peril_code = dbo.Peril_Type.code
      --LEFT JOIN dbo.SectionType                     ON dbo.SectionType.SectionTypeId = dbo.SectionTypePerilMap.SectionTypeId
              --LEFT JOIN dbo.ProductSectionMap                           ON dbo.ProductSectionMap.SectionTypeId = dbo.SectionType.SectionTypeId AND dbo.ProductSectionMap.product_id = dbo.Insurance_File.product_id
      LEFT JOIN dbo.Risk ON dbo.Peril.risk_cnt = dbo.Risk.risk_cnt
      LEFT JOIN
      (
           SELECT [O].risk_cnt, [O].peril_id, [O].peril_type_id, [O].this_premium
           FROM dbo.Peril [O]
           INNER JOIN dbo.Rating_Section [OR] ON  [OR].rating_section_id = [O].rating_section_id AND [OR].risk_cnt = [O].risk_cnt
           WHERE [OR].original_flag = 1
      ) [OP]
      ON [OP].risk_cnt = dbo.Peril.risk_cnt AND [OP].peril_id = dbo.Peril.peril_id AND [OP].peril_type_id = dbo.Peril.peril_type_id          
      WHERE  dbo.Rating_Section.original_flag = 0 AND dbo.Risk.is_risk_selected = 1 
	 -- and dbo.risk.risk_cnt = 1735254 and dbo.Insurance_File.insurance_file_cnt = 323361




GO


