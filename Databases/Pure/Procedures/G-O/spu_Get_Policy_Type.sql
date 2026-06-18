SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_Policy_Type'
GO

-- AMB 24/04/2003 - changed to use insurance_file_CNT 
--                  rather than insurance_file_ID

CREATE PROCEDURE spu_Get_Policy_Type
    @insurance_file_id int
AS

/* AK - 18042001
    Stored procedure to return Policy Type for the passed Policy_Id
*/

SELECT p.code from policy_type p, insurance_file i
    WHERE i.insurance_file_cnt = @insurance_file_id
    AND p.policy_type_id = i.policy_type_id

GO



