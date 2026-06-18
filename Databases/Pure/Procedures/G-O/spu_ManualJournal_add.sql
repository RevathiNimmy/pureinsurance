 
 SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ManualJournal_add'
GO

CREATE Procedure spu_ManualJournal_add
      
@ManualJournal_Id int OUTPUT, 
@CreatedDate datetime,  
@Document_type_id int,   
@Source_id int,  
@is_reffered tinyint,  
@PMuser_id int,  
@Comment varchar(500),   
@Reverse_on datetime,
@Recurring_occurs int,
@PerPeriodOnDay int,    
@PerMonthOnDay int,  
@PerQuarterOnDay int,  

@Authorisation_Comment varchar(500)  
    
AS    
BEGIN
   
    
INSERT INTO ManualJournal    
(    
    CreatedDate,    
    DocumentType_id,    
    Source_id,    
    is_reffered,    
    PMuser_id,    
    Comment,    
    Reverses_on,    
    Recurring_Occurs,    
    PerPeriodOnDay,    
    PerMonthOnDay,    
    PerQuarterOnDay,    
    Authorisation_comment  
)    
VALUES    
(    
	@CreatedDate ,  
	@Document_type_id ,   
	@Source_id ,  
	@Is_reffered ,  
	@PMuser_id ,  
	@Comment ,   
	@Reverse_on ,  
	@Recurring_occurs ,  
	@PerPeriodOnDay ,    
	@PerMonthOnDay ,  
	@PerQuarterOnDay ,  
	@Authorisation_Comment  
    
)    

SELECT @ManualJournal_Id = SCOPE_IDENTITY()   
END
GO

