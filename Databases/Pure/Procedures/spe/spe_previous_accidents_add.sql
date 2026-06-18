SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_previous_accidents_add'
GO

CREATE PROCEDURE spe_previous_accidents_add  
    @party_cnt int,  
    @previous_accidents_id int,  
    @Date datetime,  
    @Description varchar(70),  
    @is_at_fault tinyint,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL  
  
AS  

If @previous_accidents_id=0 OR @previous_accidents_id IS NULL  
 BEGIN  
  SELECT @previous_accidents_id = ISNULL(MAX(previous_accidents_id),0) + 1
  FROM previous_accidents 
  WHERE party_cnt=@party_cnt  
 END  

BEGIN  
INSERT INTO previous_accidents (  
    party_cnt ,  
    previous_accidents_id ,  
    Date ,  
    Description ,  
    is_at_fault,
	UserId,
	UniqueId,
	ScreenHierarchy )  
VALUES (  
    @party_cnt,  
    @previous_accidents_id,  
    @Date,  
    @Description,  
    @is_at_fault,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)  
END  



GO
