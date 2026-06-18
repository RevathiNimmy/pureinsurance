SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_Auto_Reconciliation_Validation'
GO

CREATE PROCEDURE spu_Auto_Reconciliation_Validation 
@Auto_ReconciliationRS_ID int

AS  
BEGIN  


-- Agent Group Validation 
update Account_Entry_RS set Transaction_Status = 'U' , comments ='Agent Group Net Paid Amount is negative.' where Premium_ReconciliationRS_id in (
select Premium_ReconciliationRS_id from Premium_ReconciliationRS ps inner join (
select Agent_Group_Code ,sum(acc.Net_Amount_Paid) TotalNetAmount from Auto_ReconciliationRS ARS
inner join Premium_ReconciliationRS PRS on PRS.Auto_ReconciliationRS_ID = ARS.Auto_ReconciliationRS_ID
inner join Account_Entry_RS ACC on ACC.Premium_ReconciliationRS_ID = PRS.Premium_ReconciliationRS_ID
where ars.Auto_ReconciliationRS_ID = @Auto_ReconciliationRS_ID
group by PRS.Agent_Group_Code 
having  sum(acc.Net_Amount_Paid) < 0 ) ts on ts.Agent_Group_Code = ps.Agent_Group_Code
where ps.Auto_ReconciliationRS_ID = @Auto_ReconciliationRS_ID)

-- check for entries whose acc_reference_number is like 0/Transdetailid, mark them U. 
update Account_Entry_RS set Transaction_Status = 'U' , comments ='Premium Finance Policies' where ACC_Reference_Number in (
select acc.ACC_Reference_Number  from Auto_ReconciliationRS ARS
inner join Premium_ReconciliationRS PRS on PRS.Auto_ReconciliationRS_ID = ARS.Auto_ReconciliationRS_ID
inner join Account_Entry_RS ACC on ACC.Premium_ReconciliationRS_ID = PRS.Premium_ReconciliationRS_ID
where ars.Auto_ReconciliationRS_ID = @Auto_ReconciliationRS_ID
and substring(acc.ACC_Reference_Number,0,CHARINDEX( '/',acc.ACC_Reference_Number)) = 0)


--Receipt Validation 
update Account_Entry_RS set Transaction_Status = 'U' , comments ='Receipt Transaction' where ACC_Reference_Number in (
select acc.ACC_Reference_Number  from Auto_ReconciliationRS ARS
inner join Premium_ReconciliationRS PRS on PRS.Auto_ReconciliationRS_ID = ARS.Auto_ReconciliationRS_ID
inner join Account_Entry_RS ACC on ACC.Premium_ReconciliationRS_ID = PRS.Premium_ReconciliationRS_ID
where ars.Auto_ReconciliationRS_ID = @Auto_ReconciliationRS_ID
and (UPPER(REVENUE_TYPE) like UPPER('RECEIPT')))

--Refund Validation 
update Account_Entry_RS set Transaction_Status = 'U' , comments ='Refund Transaction' where ACC_Reference_Number in (
select acc.ACC_Reference_Number  from Auto_ReconciliationRS ARS
inner join Premium_ReconciliationRS PRS on PRS.Auto_ReconciliationRS_ID = ARS.Auto_ReconciliationRS_ID
inner join Account_Entry_RS ACC on ACC.Premium_ReconciliationRS_ID = PRS.Premium_ReconciliationRS_ID
where ars.Auto_ReconciliationRS_ID = @Auto_ReconciliationRS_ID
and (UPPER(REVENUE_TYPE) like  UPPER('REFUND')))


END  
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO