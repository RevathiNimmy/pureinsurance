--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************

EXECUTE DDLDropProcedure 'spu_SIRRen_CheckJobBatchRenewalInProcess'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIRRen_CheckJobBatchRenewalInProcess  
     @ProcessKey VARCHAR(40)  
  
AS  
BEGIN  
	IF @ProcessKey = 'SEL'
	BEGIN
		SELECT lock_name from pmlock(nolock) Where lock_name IN ('SEL','RENSEL')
	END

	IF @ProcessKey = 'INV'
	BEGIN
		SELECT lock_name from pmlock(nolock) Where lock_name IN ('INV','RENINV')
	END

	IF @ProcessKey = 'ACC'
	BEGIN
		SELECT lock_name from pmlock(nolock) Where lock_name IN ('ACC','RENACC')
	END

END  
 
