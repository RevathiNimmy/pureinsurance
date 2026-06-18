SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_policies_on_BG_for_receipt'
GO

CREATE PROCEDURE spu_get_policies_on_BG_for_receipt
        @Party_Cnt        INT

AS

DECLARE @Is_Agent        TINYINT

SELECT @Is_Agent = COUNT(party_cnt) from party_agent
        where party_cnt = @Party_Cnt

        SELECT         BG.bg_id,
                BG.bank_name_id,
                Bank.Bank_Name,
                BG.BG_Ref,
                IFBGL.DueDate,
                IFBGL.Insurance_File_cnt,
                IFI.Insurance_Ref,
                IFBGL.Amount,
                (SELECT ISNULL(SUM(outstanding_amount),0)
                        FROM TransDetail TD
                        WHERE TD.Document_id = D.Document_id AND TD.Account_id = A.Account_Id) AS outstanding_amount,
                IFI.source_id,
                S.Description as  SourceDescription,
                IFI.product_id,
                P.Description         as ProductDescription,
                S.Code as SourceCode,
                IFI.cover_start_date,
                IFI.expiry_date,
                P.Code        as ProductCode

        FROM         Insurance_File_BG_Link IFBGL
                        INNER JOIN Bank_Guarantee BG
                                ON BG.BG_id = IFBGL.BG_Id
                        INNER JOIN Bank
                                ON Bank.Bank_id = BG.Bank_name_Id
                        INNER JOIN Insurance_File IFI
                                ON IFI.Insurance_File_Cnt = IFBGL.Insurance_File_Cnt
                        INNER JOIN Document D
                                ON D.Insurance_File_Cnt = IFBGL.Insurance_File_Cnt
                        INNER JOIN Account A
                                ON A.Account_Key = BG.Party_cnt
                        INNER JOIN Source s
                                ON IFI.Source_Id = S.Source_Id
                        INNER JOIN Product P
                                ON IFI.Product_Id = P.Product_Id
        WHERE  BG.Party_cnt = @party_cnt
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
