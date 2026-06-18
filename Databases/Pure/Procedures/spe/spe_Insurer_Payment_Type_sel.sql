SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Insurer_Payment_Type_sel'
GO

CREATE PROCEDURE spe_Insurer_Payment_Type_sel
    @account_Id int
AS

/******************************************************************************************/
/* spe_Insurer_Payment_Type_sel ferch Lock Id type record from party_Insurer table*/
/* for a specific Account Id */
/* 1 parameters are passed in - @account_Id */
/* 1 parameter is returned - @locked_by */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/02/2007 Pramod Kumar */
/******************************************************************************************/

BEGIN

	SELECT 
	    P.Insurer_locking_type_id 
	FROM 
	    party_insurer AS P
	INNER JOIN Account AS A 
	    ON P.party_cnt = A.account_key
	WHERE 
	    A.account_id =@account_Id  
END	
GO