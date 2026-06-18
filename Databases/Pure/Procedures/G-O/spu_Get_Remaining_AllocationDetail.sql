SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Remaining_AllocationDetail'
Go 

CREATE PROCEDURE spu_Get_Remaining_AllocationDetail
(  
 @allocation_id INT,
 @insurance_file_cnt int,
 @reverse_transaction_log_id INT
)  

AS  

Declare @iDistinctDocCount as int,  
@document_ref varchar(50),  
@transdetail_id int,  
@allocated_amount numeric(19,4),  
@reverse_amount numeric(19,4),  
@reverse_document_ref varchar(50),  
@reverse_document_id int  
  
SET @reverse_amount = 0  
  
select @reverse_document_ref = document_ref ,  
@reverse_document_id = document_id from document where insurance_file_cnt = @insurance_file_cnt  
  
declare @DrCount int
declare @CrCount int
declare @ParentDocRef varchar(100)


select @CrCount = count(distinct document_ref) from AllocationDetail where allocation_id = @allocation_id and document_ref like 'SRP%'
 

select @DrCount = count(distinct document_ref) from AllocationDetail where allocation_id = @allocation_id and  document_ref like 'SPY%'
 

if  isnull(@DrCount,0) = 1 begin

	select @ParentDocRef = document_ref from AllocationDetail where allocation_id = @allocation_id and  document_ref like 'SPY%'
 
end 

if  isnull(@CrCount,0) = 1 begin

	select @ParentDocRef = document_ref from AllocationDetail where allocation_id = @allocation_id and document_ref like 'SRP%'
 
end 


SET @ParentDocRef = ISNULL(@ParentDocRef,'')
 
Select @iDistinctDocCount= Count( distinct document_ref ) from AllocationDetail  
where allocation_id = @allocation_id  
  
CREATE table #AllocationDetail  
 (transdetail_id int,  
 document_ref varchar(50),  
 alloc_base_amount numeric(19,4),  
 alloc_key varchar(100),
 parent_row int
 )  
  
IF @iDistinctDocCount > 2  
BEGIN  
 IF @ParentDocRef <> ''  
  BEGIN  
   DECLARE c_Temp CURSOR FAST_FORWARD  
   FOR  
   SELECT transdetail_id,  
   document_ref,  
   alloc_base_amount  
   FROM AllocationDetail  
   WHERE allocation_id = @allocation_id  
   OPEN c_Temp  
  
   FETCH NEXT  
   FROM c_Temp  
   INTO @transdetail_id,  @document_ref,@allocated_amount  
  
   WHILE (@@FETCH_STATUS = 0)  
   BEGIN  
  
    IF @document_ref = @reverse_document_ref  
    BEGIN  
     Set @reverse_amount =  @reverse_amount + @allocated_amount  
    END  
    ELSE  
      BEGIN  
     insert into #AllocationDetail (transdetail_id,document_ref,alloc_base_amount,alloc_key,parent_row)  
     values(@transdetail_id ,  @document_ref ,@allocated_amount , CAST(@transdetail_id as varchar) + '|' + cast(@allocated_amount as varchar), 0)  
      END  
   FETCH NEXT  
   FROM c_Temp  
   INTO @transdetail_id,  @document_ref,@allocated_amount  
   END  
  
    CLOSE c_Temp  
          DEALLOCATE c_Temp  
  
   Update #AllocationDetail set alloc_key = cast(transdetail_id as varchar) + '|' + cast((alloc_base_amount +  @reverse_amount) as varchar), parent_row = 1 where document_ref = @ParentDocRef 
  END  
  
  
 IF @ParentDocRef = ''  
 begin  
    update void_reverse_transaction_log_detail set is_reverse_allocated = 0 where reverse_transaction_log_id = @reverse_transaction_log_id and allocation_id = @allocation_id  
    end  
END  
  
SELECT * FROM #AllocationDetail 