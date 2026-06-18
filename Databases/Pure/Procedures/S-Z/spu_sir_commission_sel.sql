SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_sir_commission_sel
GO
CREATE PROCEDURE spu_sir_commission_sel  
    @Party_Type_id INT,  
    @Party_cnt INT,  
    @Product_id INT,  
    @risk_type_id INT,  
    @transaction_type_id INT,  
    @commission_band_id INT,  
    @commission_group_id INT  
AS  
  
BEGIN  
  
/********************************************************************************************************/  
/* Stored Procedure spu_sir_commission_sel                                   */  
/********************************************************************************************************/  
/* Revision             Description of Modification                                     Date        Who */  
/* --------             ---------------------------                                     ----        --- */  
/* 1.0                  SR11092000 - Created                                */  
/* 1.1                  SR20092000 - Party type changed to party agent id               */  
/* 1.2                  CMG/PB12072002 - Commission group added               */  
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
        CA.is_deleted,  
        CA.tax_group_id,  
        TG.description,
		CL.commission_level_id,
		CL.description
 
    FROM  
        Commission_Arrangement CA  
        INNER JOIN Tax_Group TG  
            ON CA.tax_group_id = TG.tax_group_id  
		 LEFT OUTER JOIN commission_level CL
			ON CL.commission_level_id = CA.commission_level_id
            AND CA.Party_type = @party_type_id  
            AND CA.Party_cnt  = @party_cnt  
            AND CA.Product_id = @product_id  
            AND CA.Risk_type_id = @risk_type_id  
            AND CA.Transaction_type_id = @transaction_type_id  
            AND CA.Commission_band_id = @commission_band_id  
            AND CA.Commission_grouping = @commission_group_id  
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
  
END  
GO