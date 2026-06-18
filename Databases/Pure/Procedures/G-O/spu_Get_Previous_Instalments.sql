/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select count from pfinstalments table
Test Code     : Exec spu_Get_Previous_Instalments  
 */

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Previous_Instalments'
GO

CREATE PROCEDURE spu_Get_Previous_Instalments  
    @nPFprem_finance_cnt int,  
    @nPFprem_finance_version int,  
    @dDueDate Datetime  
AS  
	DECLARE @enumPFInstalmentStatus_Collected  INT = 3

	SELECT Count(*) FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
	AND pfprem_finance_version=@nPFprem_finance_version  
	AND DueDate < @dDueDate  
	AND InstalmentNumber <> 0 AND status <> @enumPFInstalmentStatus_Collected 


GO
