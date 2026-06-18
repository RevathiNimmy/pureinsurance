SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_sir_commission_sel_all  
GO

CREATE PROCEDURE spu_sir_commission_sel_all  
AS  
  
BEGIN  
  
/********************************************************************************************************/  
/* Stored Procedure spu_sir_commission_sel_all                                                          */  
/********************************************************************************************************/  
/* Revision             Description of Modification                                     Date        Who */  
/* --------             ---------------------------                                     ----        --- */  
/* 1.0                  SR07092000 - Created                                                            */  
/* 1.1                  SR20092000 - Party type changed to Party agent type                             */  
/* 1.1                  CMG/PB12072002 - Commission Group added                                         */  
/********************************************************************************************************/  
  
    SELECT  
        ISNULL(PAT.Code , 'All'),  
        ISNULL(PT.Shortname, 'All'),  
        ISNULL(P.code, 'All'),  
        ISNULL(RT.Code , 'All'),  
        ISNULL(TT.Code , 'All'),  
        ISNULL(CB.Code,'All'),  
        ISNULL(CG.Code,'All'),  
        CA.Rate ,  
        CA.Is_value,  
        CA.Effective_Date ,  
        CA.Party_Type,  
        CA.PArty_cnt,  
        CA.Product_id,  
        CA.Risk_type_id,  
        CA.transaction_type_id,  
        CA.Commission_band_id,  
        CA.Commission_grouping,  
        CA.Is_Deleted,  
        ISNULL(CA.tax_group_id,0),  
        TG.description,  
 	  CA.maximum_rate,  
	  ISNULL(CL.commission_level_id,0),
	  CL.description


    FROM  
        Commission_Arrangement CA  
        LEFT OUTER JOIN Party_Agent_Type PAT  
            ON CA.Party_Type = PAT.party_agent_type_id  
        LEFT OUTER JOIN Party   PT  
            ON CA.Party_cnt = PT.Party_cnt  
        LEFT OUTER JOIN Product P  
            ON CA.Product_id = P.Product_id  
        LEFT OUTER JOIN Risk_Type RT  
            ON CA.Risk_Type_id = RT.Risk_type_id  
        LEFT OUTER JOIN Transaction_Type TT  
            ON CA.Transaction_type_id = TT.Transaction_type_id  
        LEFT OUTER JOIN Commission_Band CB  
            ON CA.Commission_band_id = CB.Commission_band_id  
        LEFT OUTER JOIN Commission_Grouping CG  
            ON CA.Commission_grouping = CG.Commission_grouping_id  
        LEFT OUTER JOIN Tax_Group TG  
            ON CA.tax_group_id = TG.tax_group_id  
		 LEFT OUTER JOIN commission_level CL
            ON CL.commission_level_id = CA.commission_level_id

  
END  
GO