SET QUOTED_IDENTIFIER OFF 
GO

EXECUTE DDLDropProcedure 'spu_PFInstalments_Update'
GO

CREATE PROCEDURE spu_PFInstalments_Update  
	@nPFprem_finance_cnt INT,  
	@nPFprem_finance_version INT,  
	@dDueDate DATETIME  
AS  
BEGIN  
 DECLARE @Amount MONEY  
 DECLARE @InstalmentNumber INT  
 DECLARE @InstalmentNumberCur INT  
 DECLARE @AmountCur MONEY  
 DECLARE @StatusCur INT  
 DECLARE @DueDateCur DATETIME  
 DECLARE @InstalmentNumberCurNew INT  
 DECLARE @AmountCurNew MONEY  
 DECLARE @StatusCurNew INT  
 DECLARE @DueDateCurNew DATETIME 
 DECLARE @AmountCurOld MONEY 
 SELECT @Amount= Amount FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
 AND pfprem_finance_version=@nPFprem_finance_version AND InstalmentNumber=1  
  
 SELECT @InstalmentNumber=InstalmentNumber FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
 AND pfprem_finance_version=@nPFprem_finance_version AND  DueDate >= @dDueDate ORDER BY InstalmentNumber DESC  
  
 UPDATE pfinstalments SET Amount=@Amount WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
 AND pfprem_finance_version=@nPFprem_finance_version AND InstalmentNumber=@InstalmentNumber  
  
 DECLARE InstalmentUpdate CURSOR FAST_FORWARD FOR  
   SELECT InstalmentNumber,Amount,Status,DueDate FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
  AND pfprem_finance_version=@nPFprem_finance_version - 1 AND DueDate < @dDueDate AND InstalmentNumber <> 0  
  
 OPEN InstalmentUpdate  
  
 FETCH NEXT FROM InstalmentUpdate INTO @InstalmentNumberCur,@AmountCur,@StatusCur,@DueDateCur  
  
 WHILE @@FETCH_STATUS = 0 BEGIN  
  
     UPDATE  pfinstalments SET  Amount=@AmountCur,Status=@StatusCur WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
     AND pfprem_finance_version=@nPFprem_finance_version 
     AND DueDate=@DueDateCur  
  
     FETCH NEXT FROM InstalmentUpdate INTO @InstalmentNumberCur,@AmountCur,@StatusCur,@DueDateCur  
  
 END  
  
 CLOSE InstalmentUpdate  
 DEALLOCATE InstalmentUpdate 

DECLARE InstalmentUpdateNew CURSOR Fast_forward FOR  
   SELECT InstalmentNumber,Amount,Status,DueDate FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
  AND pfprem_finance_version=@nPFprem_finance_version - 1 AND DueDate >= @dDueDate AND InstalmentNumber <> 0  
  
 OPEN InstalmentUpdateNew  
  
 FETCH NEXT FROM InstalmentUpdateNew INTO @InstalmentNumberCurNew,@AmountCurNew,@StatusCurNew,@DueDateCurNew  
  
 WHILE @@FETCH_STATUS = 0 BEGIN  
     SELECT @AmountCurOld=Amount FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
     AND pfprem_finance_version=@nPFprem_finance_version 
     AND DueDate=@DueDateCurNew 
     UPDATE  pfinstalments SET  Amount=(@AmountCurOld+@AmountCurNew),Status=@StatusCurNew WHERE pfprem_finance_cnt=@nPFprem_finance_cnt  
     AND pfprem_finance_version=@nPFprem_finance_version 
     AND DueDate=@DueDateCurNew  
  
     FETCH NEXT FROM InstalmentUpdateNew INTO @InstalmentNumberCurNew,@AmountCurNew,@StatusCurNew,@DueDateCurNew 
  
 END  
  
 CLOSE InstalmentUpdateNew 
 DEALLOCATE InstalmentUpdateNew
  
 UPDATE pfpremiumFinance
 SET FirstInstallment=(
						SELECT Amount
						FROM pfinstalments
						WHERE pfprem_finance_cnt=@nPFprem_finance_cnt
							  AND pfprem_finance_version=@nPFprem_finance_version
							  AND InstalmentNumber=1
					  )  
  WHERE pfprem_finance_cnt=@nPFprem_finance_cnt
  AND pfprem_finance_version=@nPFprem_finance_version  

 UPDATE pfpremiumFinance
 SET OthInstallments=(SELECT Amount FROM pfinstalments 
 WHERE pfprem_finance_cnt= @nPFprem_finance_cnt AND pfprem_finance_version=@nPFprem_finance_version AND InstalmentNumber=2)  
        WHERE pfprem_finance_cnt=@nPFprem_finance_cnt AND pfprem_finance_version=@nPFprem_finance_version 
END  
GO
