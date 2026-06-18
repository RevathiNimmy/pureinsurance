SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_policies_on_BG_for_id'
GO

CREATE PROCEDURE spu_get_policies_on_BG_for_id
        @bg_id        		INT
	
AS

DECLARE @Is_Agent       TINYINT,	
	@party_Cnt	INT
	
SELECT 	@party_Cnt = Party_cnt 
	FROM Bank_Guarantee
	WHERE BG_Id = @bg_id

SELECT @Is_Agent = COUNT(party_cnt) from party_agent
        where party_cnt = @Party_Cnt
        SELECT         

                PA.ShortName 		As ClientCode,
                PA.Resolved_Name 	As ClientName,
                IFI.Insurance_Ref	AS Insurance_Ref,
                P1.ShortName 		As AgentCode,
                S.Description 		AS SourceDescription,
                P.Description         	AS ProductDescription,
                IFBGL.Amount,
                IFI.cover_start_date,
                IFI.expiry_date,
                IFBGL.Insurance_File_cnt

        FROM         Insurance_File_BG_Link IFBGL
                        INNER JOIN Bank_Guarantee BG
                                ON BG.BG_id = IFBGL.BG_Id
                        INNER JOIN Bank
                                ON Bank.Bank_id = BG.Bank_name_Id
                        INNER JOIN Insurance_File IFI
                                ON IFI.Insurance_File_Cnt = IFBGL.Insurance_File_Cnt
                        INNER JOIN Party PA
                                ON IFI.Insured_Cnt = PA.Party_Cnt
                        LEFT JOIN Party P1
                                ON IFI.Lead_agent_cnt = P1.Party_Cnt
                        INNER JOIN Source s
                                ON IFI.Source_Id = S.Source_Id
                        INNER JOIN Product P
                                ON IFI.Product_Id = P.Product_Id
        WHERE BG.bg_id = @bg_id 
		AND
                (
                        (@Is_Agent = 1 AND IFI.lead_agent_cnt = @party_cnt)
                        OR
                        (@Is_Agent = 0 AND IFI.insured_cnt = @party_cnt)
                )

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
