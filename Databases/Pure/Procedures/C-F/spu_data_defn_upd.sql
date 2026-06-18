EXECUTE DDLDropProcedure 'spu_data_defn_upd'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO




CREATE PROCEDURE spu_data_defn_upd
	@data_defn_id int ,
	@type_id int,                
	@Description varchar(50) ,                  
	@Caption varchar(50),                        
	@type int,                              
	@display_order int,                                    
	@Mandatory bit,                                      
	@read_only bit,
	@Claim_party_type_id int,              
	@Claim_Lookup_id int,  
	@tab_ID int,                                                   
	@Mode bit                              
          
AS
BEGIN                                                           
    Declare @count numeric		                           
   Declare @defnid int,@buffer int
If @Mode=0           
	Begin               
	     exec spu_sort_disp_order_upd @display_order,@data_defn_id  
             UPDATE Risk_data_definition
             SET Risk_type_id=@type_id, Description=@Description, Caption=@Caption,
		 type=@type, display_order=@display_order, Mandatory=@Mandatory, read_only=@read_only,
		 Claim_party_type_id=@Claim_party_type_id, Claim_Lookup_id=@Claim_Lookup_id, Tab_ID = @Tab_ID
             WHERE risk_data_defn_id=@data_defn_id
                                                                  
--CODE MODIFIED FOR PARTY DISPLAY_ORDER PROBLEM 				AUTHOR: SUHEL K	                                                              
--           If @display_order is Null 
                                                                                
--	     Begin                                                                    
		DECLARE sort_cursor CURSOR FOR 
			Select risk_data_defn_id,display_order from risk_data_definition where risk_type_id=@type_id                                                                                       
				And display_order IS NOT NULL order by display_order ASC
		OPEN sort_cursor                                                                                                                                 
		FETCH NEXT FROM sort_cursor INTO @defnid,@buffer
		--Begin loop                                                                                                              
		Select @count=1                                                                                                                                                                               
		WHILE @@FETCH_STATUS = 0                                                                                                           
		BEGIN                                                                                                                                        
			Update Risk_data_definition                                                                                                                                        
			Set display_order=@count                                                                                                                                        
			Where risk_data_defn_id=@defnid
			Select @count=@count+1	                                                                                                                                     
			FETCH NEXT FROM sort_cursor INTO @defnid,@buffer                                                                                                                                                          
		END                                                                                                                                                          
		Close sort_cursor                                                                                                                                                                        
		Deallocate sort_cursor                                                                                                                                                                            
--	     end 		  	                                                                                                                                          
	End                                                                                                                                                                                
Else if 
 @Mode=1
	Begin
	     exec spu_sort_disp_order_upd_peril @display_order,@data_defn_id 
	     UPDATE Peril_data_definition
	     SET Peril_type_id=@type_id, Description=@Description, Caption=@Caption, 
                 type=@type, display_order= @display_order, Mandatory=@Mandatory, read_only=@read_only, 
                 Claim_party_type_id=@Claim_party_type_id, Claim_Lookup_id=@Claim_Lookup_id, Tab_ID = @Tab_ID
             WHERE peril_data_defn_id=@data_defn_id                                                                                                              
                                                
--CODE MODIFIED FOR PARTY DISPLAY_ORDER PROBLEM 				AUTHOR: SUHEL K	                                                                                                                                                            
                                                         
--            If @display_order is Null 
                                                            
--	     Begin                                                
		DECLARE sort_cursor CURSOR FOR                                                                        
			Select peril_data_defn_id,display_order from peril_data_definition where peril_type_id=@type_id
				And display_order IS NOT NULL order by display_order ASC
		OPEN sort_cursor                                                      
--		Declare @count numeric		                                         
--		Declare @defnid numeric,@buffer numeric 
		FETCH NEXT FROM sort_cursor INTO @defnid,@buffer
		--Begin loop
		Select @count=1         
		WHILE @@FETCH_STATUS = 0
		BEGIN
			Update Peril_data_definition
			Set display_order=@count
			Where peril_data_defn_id=@defnid
			Select @count=@count+1	
			FETCH NEXT FROM sort_cursor INTO @defnid,@buffer
		END                   
		Close sort_cursor                         
		Deallocate sort_cursor      
--	     end 		  	
        End                                          
END




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
